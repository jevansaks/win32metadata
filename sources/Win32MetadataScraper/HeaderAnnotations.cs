using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using ClangSharp;
using ClangSharp.Interop;
using MetadataUtils;

namespace Win32MetadataScraper
{
    internal sealed class HeaderAnnotations
    {
        private const string Owner = "Win32Metadata.";
        public HashSet<string> FunctionNames { get; } = new(StringComparer.Ordinal);
        private readonly List<(Attr Attribute, string EnumName, ClangSharp.Type Storage)> enumAssociations = new();
        private readonly HashSet<string> nativeEnums = new(StringComparer.Ordinal);
        public Dictionary<string, EnumObject> EnumGroups { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, (string Type, string Value)> EnumConstants { get; } = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ClangSharp.Type> enumStorage = new(StringComparer.Ordinal);

        public void Visit(Decl declaration)
        {
            if (declaration is EnumDecl nativeEnum) this.nativeEnums.Add(nativeEnum.Name);
            if (declaration is TypedefDecl alias && alias.UnderlyingType.CanonicalType is EnumType)
                this.nativeEnums.Add(alias.Name);
            uint? declarationStart = null;
            foreach (var attribute in declaration.Attrs)
            {
                if (attribute.Kind != CX_AttrKind.CX_AttrKind_Annotate ||
                    !attribute.Spelling.StartsWith(Owner, StringComparison.Ordinal))
                    continue;

                string payload = attribute.Spelling;
                if (payload.StartsWith(SupportedOSAnnotation.Prefix, StringComparison.Ordinal))
                {
                    this.VisitSupportedOS(declaration, attribute);
                    continue;
                }
                if (payload.StartsWith(Owner + "Enum(", StringComparison.Ordinal) ||
                    payload.StartsWith(Owner + "FlagsEnum(", StringComparison.Ordinal))
                {
                    this.VisitEnumGroup(declaration, attribute);
                    continue;
                }
                if (payload != Owner + "SetLastError")
                {
                    this.VisitParameterAnnotation(declaration, attribute, ref declarationStart);
                    continue;
                }
                if (attribute.CursorChildren.Any())
                    throw Error(attribute, "Win32Metadata.SetLastError accepts no arguments.");
                if (declaration is not FunctionDecl function || declaration is CXXMethodDecl || function.HasBody)
                    throw Error(attribute, "Win32Metadata.SetLastError requires an imported function declaration.");

                attribute.Extent.Start.GetExpansionLocation(out var attributeFile, out _, out _, out uint attributeOffset);
                declaration.Extent.Start.GetExpansionLocation(out var declarationFile, out _, out _, out _);
                declaration.Extent.End.GetExpansionLocation(out _, out _, out _, out uint declarationEnd);
                declarationStart ??= GetDeclarationStart(function, attribute);
                // Clang propagates an annotation to other redeclarations, including earlier ones.
                if (attributeFile.Equals(declarationFile) &&
                    attributeOffset > declarationStart.Value && attributeOffset < declarationEnd)
                    throw Error(attribute, "Win32Metadata.SetLastError must precede the complete function declaration.");

                this.FunctionNames.Add(function.Name);
            }

            if (declaration is FunctionDecl containingFunction)
                foreach (var parameter in containingFunction.Parameters)
                    this.Visit(parameter);
        }

        public void ValidateEnumAssociations()
        {
            foreach (var association in this.enumAssociations)
            {
                if (!this.nativeEnums.Contains(association.EnumName))
                    throw Error(association.Attribute, $"AssociatedEnum requires an existing native enum '{association.EnumName}'.");
                if (this.enumStorage.TryGetValue(association.EnumName, out var storage) &&
                    (association.Storage is not BuiltinType ||
                    GetIntegerStorage(storage, association.Attribute) != GetIntegerStorage(association.Storage, association.Attribute)))
                    throw Error(association.Attribute, $"AssociatedEnum storage conflicts with enum group '{association.EnumName}'.");
            }
        }

        private void VisitSupportedOS(Decl declaration, Attr attribute)
        {
            string version;
            try { version = SupportedOSAnnotation.Parse(attribute.Spelling); }
            catch (FormatException error) { throw Error(attribute, error.Message); }
            if (attribute.CursorChildren.Any())
                throw Error(attribute, "SupportedOSPlatform accepts no Clang expression arguments.");
            if ((declaration is not FunctionDecl && declaration is not RecordDecl &&
                declaration is not EnumDecl && declaration is not TypedefDecl) ||
                declaration is FunctionDecl function && function.HasBody)
                throw Error(attribute, "SupportedOSPlatform requires a function, method, record, enum or projected typedef declaration.");
            if (declaration is TypedefDecl alias &&
                (alias.UnderlyingType is TypedefType ||
                RemapDiscovery.UnwrapType(alias.UnderlyingType) is not PointerType pointer ||
                RemapDiscovery.UnwrapType(pointer.PointeeType) is not FunctionProtoType))
                throw Error(attribute, "SupportedOSPlatform typedefs require a projected callback; scalar and record aliases have no independent emitted use site. Annotate the owning record or enum instead.");
            if (declaration is FunctionDecl redeclaration &&
                !redeclaration.CanonicalDecl.Attrs.Any(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                    a.Spelling == attribute.Spelling))
                throw Error(attribute, "SupportedOSPlatform must appear on the first native function declaration; late-only annotations are not emitted by PInvokeGenerator.");
            foreach (var other in declaration.Attrs.Where(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                a.Spelling.StartsWith(SupportedOSAnnotation.Prefix, StringComparison.Ordinal)))
            {
                string otherVersion;
                try { otherVersion = SupportedOSAnnotation.Parse(other.Spelling); }
                catch (FormatException error) { throw Error(other, error.Message); }
                if (version != otherVersion)
                    throw Error(other, "Conflicting SupportedOSPlatform annotations.");
            }
        }

        private void VisitEnumGroup(Decl declaration, Attr attribute)
        {
            if (declaration is not TypedefDecl alias ||
                (alias.DeclContext is not TranslationUnitDecl && alias.DeclContext is not LinkageSpecDecl))
                throw Error(attribute, "Enum groups require an unscoped storage typedef.");
            if (alias.Attrs.Count(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                (a.Spelling.StartsWith(Owner + "Enum(", StringComparison.Ordinal) ||
                a.Spelling.StartsWith(Owner + "FlagsEnum(", StringComparison.Ordinal))) != 1)
                throw Error(attribute, "Duplicate or conflicting enum group annotations.");
            var storage = alias.UnderlyingType.CanonicalType;
            if (storage is EnumType)
                throw Error(attribute, "Enum groups require native integer storage, not an existing enum type.");
            string type = GetIntegerStorage(storage, attribute);
            var match = Regex.Match(attribute.Spelling, @"^Win32Metadata\.(FlagsEnum|Enum)\((.*)\)$");
            if (!match.Success) throw Error(attribute, "Malformed enum group annotation.");
            var names = match.Groups[2].Value.Split(',').Select(n => n.Trim()).ToArray();
            var arguments = attribute.CursorChildren.ToArray();
            if (names.Length == 0 || names.Length != arguments.Length ||
                names.Any(n => !Regex.IsMatch(n, @"^[A-Za-z_][A-Za-z_0-9]*$")) ||
                names.Distinct(StringComparer.Ordinal).Count() != names.Length)
                throw Error(attribute, "Enum groups require one unique constituent name per Clang expression argument.");
            var group = new EnumObject { name = alias.Name, type = type, flags = match.Groups[1].Value == "FlagsEnum" };
            for (int i = 0; i < names.Length; i++)
            {
                using var value = arguments[i].Handle.Evaluate;
                if (value.Kind != CXEvalResultKind.CXEval_Int || arguments[i] is not Expr expression)
                    throw Error(attribute, $"Enum constituent '{names[i]}' is not a supported integer constant expression.");
                ulong bits = value.IsUnsignedInt ? value.AsUnsigned : unchecked((ulong)value.AsLongLong);
                // Keep the compiler-typed constituent available to dependent constants.
                // The projected enum independently converts it to its authored storage.
                string constantType = GetIntegerStorage(expression.Type.CanonicalType, attribute);
                var constant = (Type: constantType, Value: FormatInteger(bits, constantType));
                if (this.EnumConstants.TryGetValue(names[i], out var existing) && existing != constant)
                    throw Error(attribute, $"Conflicting native values for enum constituent '{names[i]}'.");
                this.EnumConstants[names[i]] = constant;
                group.members.Add(new EnumObject.Member { name = names[i], value = FormatInteger(bits, type) });
            }
            if (this.EnumGroups.ContainsKey(alias.Name))
                throw Error(attribute, $"Duplicate enum group '{alias.Name}'.");
            this.EnumGroups.Add(alias.Name, group);
            this.enumStorage.Add(alias.Name, storage);
            this.nativeEnums.Add(alias.Name);
        }

        private static string GetIntegerStorage(ClangSharp.Type storage, Attr attribute)
        {
            if (storage is EnumType nativeEnum) storage = nativeEnum.Decl.IntegerType.CanonicalType;
            return storage.Kind switch
            {
                CXTypeKind.CXType_Char_U or CXTypeKind.CXType_UChar => "byte",
                CXTypeKind.CXType_Char_S or CXTypeKind.CXType_SChar => "sbyte",
                CXTypeKind.CXType_UShort => "ushort",
                CXTypeKind.CXType_Short => "short",
                CXTypeKind.CXType_UInt or CXTypeKind.CXType_ULong when storage.Handle.SizeOf == 4 => "uint",
                CXTypeKind.CXType_Int or CXTypeKind.CXType_Long when storage.Handle.SizeOf == 4 => "int",
                CXTypeKind.CXType_ULongLong or CXTypeKind.CXType_ULong when storage.Handle.SizeOf == 8 => "ulong",
                CXTypeKind.CXType_LongLong or CXTypeKind.CXType_Long when storage.Handle.SizeOf == 8 => "long",
                _ => throw Error(attribute, "Enum groups require supported native integer storage (8, 16, 32 or 64 bits)."),
            };
        }

        private static string FormatInteger(ulong bits, string type) =>
            type switch
            {
                "byte" => unchecked((byte)bits).ToString(CultureInfo.InvariantCulture),
                "sbyte" => unchecked((sbyte)bits).ToString(CultureInfo.InvariantCulture),
                "ushort" => unchecked((ushort)bits).ToString(CultureInfo.InvariantCulture),
                "short" => unchecked((short)bits).ToString(CultureInfo.InvariantCulture),
                "uint" => unchecked((uint)bits).ToString(CultureInfo.InvariantCulture),
                "int" => unchecked((int)bits).ToString(CultureInfo.InvariantCulture),
                "ulong" => bits.ToString(CultureInfo.InvariantCulture) + "UL",
                _ => unchecked((long)bits).ToString(CultureInfo.InvariantCulture) + "L",
            };

        private void VisitParameterAnnotation(Decl declaration, Attr attribute, ref uint? declarationStart)
        {
            ParameterAnnotation annotation;
            try { annotation = ParameterAnnotation.Parse(attribute.Spelling); }
            catch (FormatException error) { throw Error(attribute, error.Message); }
            if (attribute.CursorChildren.Any())
                throw Error(attribute, $"{attribute.Spelling} accepts no Clang expression arguments.");
            bool returnValue = declaration is FunctionDecl && annotation.Kind == ParameterAnnotationKind.AssociatedEnum;
            bool fieldValue = declaration is FieldDecl && annotation.Kind == ParameterAnnotationKind.AssociatedEnum;
            if (declaration is not ParmVarDecl && !returnValue && !fieldValue)
                throw Error(attribute, $"{attribute.Spelling} requires a parameter declaration.");
            if (declaration is ParmVarDecl redeclaredParameter && redeclaredParameter.DeclContext is FunctionDecl function)
            {
                int index = function.Parameters.TakeWhile(p => p != redeclaredParameter).Count();
                var first = function.CanonicalDecl;
                if (index >= first.Parameters.Count || !first.Parameters[index].Attrs.Any(a => a.Spelling == attribute.Spelling))
                    throw Error(attribute, "Owned parameter metadata must appear on the first native declaration; late-only parameter annotations are not inherited by PInvokeGenerator.");
            }
            if (declaration is ParmVarDecl parameter && annotation.Kind != ParameterAnnotationKind.AssociatedEnum &&
                parameter.Type.CanonicalType is not PointerType)
                throw Error(attribute, $"{attribute.Spelling} requires a pointer parameter.");
            attribute.Extent.Start.GetExpansionLocation(out var attributeFile, out _, out _, out uint attributeOffset);
            declaration.Extent.Start.GetExpansionLocation(out var declarationFile, out _, out _, out _);
            declaration.Extent.End.GetExpansionLocation(out _, out _, out _, out uint declarationEnd);
            declarationStart ??= GetDeclarationStart(declaration, attribute);
            if (attributeFile.Equals(declarationFile) &&
                attributeOffset > declarationStart.Value && attributeOffset < declarationEnd)
                throw Error(attribute, $"{attribute.Spelling} must precede the native declaration type.");
            var owned = declaration.Attrs.Where(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                a.Spelling.StartsWith(Owner, StringComparison.Ordinal)).Select(a => a.Spelling).ToArray();
            if (owned.Count(p => p == attribute.Spelling) > 1 ||
                (annotation.Kind == ParameterAnnotationKind.AssociatedEnum &&
                owned.Count(p => p.StartsWith(Owner + "AssociatedEnum", StringComparison.Ordinal)) > 1))
                throw Error(attribute, $"Duplicate or conflicting parameter annotation '{attribute.Spelling}'.");
            if (annotation.Kind == ParameterAnnotationKind.ContainsInteriorPointers)
            {
                bool output = declaration.Attrs.Any(a => a.Spelling.Contains("SAL_name", StringComparison.Ordinal) &&
                    (a.Spelling.Contains("_Out_writes", StringComparison.Ordinal) ||
                    a.Spelling.Contains("_Inout_updates", StringComparison.Ordinal)));
                if (!output)
                    throw Error(attribute, "ContainsInteriorPointers requires caller-owned output/inout buffer SAL.");
            }
            if (annotation.Kind == ParameterAnnotationKind.AssociatedEnum)
            {
                ClangSharp.Type storage = declaration is ParmVarDecl enumParameter ?
                    enumParameter.Type.CanonicalType : declaration is FieldDecl enumField ?
                    enumField.Type.CanonicalType : ((FunctionDecl)declaration).ReturnType.CanonicalType;
                if (storage is PointerType pointer) storage = pointer.PointeeType.CanonicalType;
                if (storage is not EnumType && (storage is not BuiltinType ||
                    storage.Kind < CXTypeKind.CXType_Bool || storage.Kind > CXTypeKind.CXType_UInt128 &&
                    (storage.Kind < CXTypeKind.CXType_Char_S || storage.Kind > CXTypeKind.CXType_Int128)))
                    throw Error(attribute, "AssociatedEnum requires integer or native enum storage, optionally through one pointer.");
                this.enumAssociations.Add((attribute, annotation.EnumName, storage));
            }
        }

        private static uint GetDeclarationStart(Decl function, Attr annotation)
        {
            string name = (function as NamedDecl)?.Name;
            var unit = function.TranslationUnit.Handle;
            var annotations = function.Attrs.Where(attr => attr.Kind == CX_AttrKind.CX_AttrKind_Annotate)
                .Select(attr =>
                {
                    attr.Extent.Start.GetExpansionLocation(out var file, out _, out _, out uint start);
                    return (File: file, Start: start);
                }).ToArray();
            function.Extent.Start.GetExpansionLocation(out var sourceFile, out _, out _, out uint sourceStart);
            function.Extent.End.GetExpansionLocation(out var endFile, out _, out _, out uint sourceEnd);
            if (!sourceFile.Equals(endFile))
                throw Error(annotation, $"Cannot locate native declaration prefix for '{name}' across files.");
            var range = CXSourceRange.Create(unit.GetLocationForOffset(sourceFile, sourceStart),
                unit.GetLocationForOffset(sourceFile, sourceEnd));
            var tokens = unit.Tokenize(range);
            try
            {
                for (int index = 0; index < tokens.Length; index++)
                {
                    var token = tokens[index];
                    if (token.Kind == CXTokenKind.CXToken_Comment) continue;
                    token.GetLocation(unit).GetExpansionLocation(out var file, out _, out _, out uint offset);
                    using var spelling = token.GetSpelling(unit);
                    string text = spelling.ToString();
                    bool annotationMacro = annotations.Any(attr => attr.File.Equals(file) && attr.Start == offset);
                    if (text != "__attribute__" && text != "[" && !annotationMacro) return offset;

                    // Macro attribute extents can collapse to the invocation token.
                    // Consume prefix arguments once, not once per annotation.
                    if (index + 1 < tokens.Length)
                    {
                        using var next = tokens[index + 1].GetSpelling(unit);
                        if (next.ToString() == "(" || text == "[")
                        {
                            string open = text == "[" ? "[" : "(";
                            string close = text == "[" ? "]" : ")";
                            int depth = text == "[" ? 1 : 0;
                            do
                            {
                                index++;
                                using var part = tokens[index].GetSpelling(unit);
                                if (part.ToString() == open) depth++;
                                else if (part.ToString() == close) depth--;
                            } while (depth > 0 && index + 1 < tokens.Length);
                        }
                    }
                }
                throw Error(annotation, $"Cannot locate native declaration prefix for '{name}'.");
            }
            finally { unit.DisposeTokens(tokens); }
        }

        private static FormatException Error(Attr attribute, string message)
        {
            attribute.Extent.Start.GetExpansionLocation(out var file, out uint line, out uint column, out _);
            return new FormatException($"{file.Name}:{line}:{column}: {message}");
        }
    }
}
