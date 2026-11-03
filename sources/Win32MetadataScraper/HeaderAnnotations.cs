using System;
using System.Collections.Generic;
using System.Linq;
using ClangSharp;
using ClangSharp.Interop;
using MetadataUtils;

namespace Win32MetadataScraper
{
    internal sealed class HeaderAnnotations
    {
        private const string Owner = "Win32Metadata.";
        public HashSet<string> FunctionNames { get; } = new(StringComparer.Ordinal);
        private readonly List<(Attr Attribute, string EnumName)> enumAssociations = new();
        private readonly HashSet<string> nativeEnums = new(StringComparer.Ordinal);

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
                if (!this.nativeEnums.Contains(association.EnumName))
                    throw Error(association.Attribute, $"AssociatedEnum requires an existing native enum '{association.EnumName}'.");
        }

        private void VisitParameterAnnotation(Decl declaration, Attr attribute, ref uint? declarationStart)
        {
            ParameterAnnotation annotation;
            try { annotation = ParameterAnnotation.Parse(attribute.Spelling); }
            catch (FormatException error) { throw Error(attribute, error.Message); }
            if (attribute.CursorChildren.Any())
                throw Error(attribute, $"{attribute.Spelling} accepts no Clang expression arguments.");
            bool returnValue = declaration is FunctionDecl && annotation.Kind == ParameterAnnotationKind.AssociatedEnum;
            if (declaration is not ParmVarDecl && !returnValue)
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
                    enumParameter.Type.CanonicalType : ((FunctionDecl)declaration).ReturnType.CanonicalType;
                if (storage is PointerType pointer) storage = pointer.PointeeType.CanonicalType;
                if (storage is not EnumType && (storage is not BuiltinType ||
                    storage.Kind < CXTypeKind.CXType_Bool || storage.Kind > CXTypeKind.CXType_UInt128 &&
                    (storage.Kind < CXTypeKind.CXType_Char_S || storage.Kind > CXTypeKind.CXType_Int128)))
                    throw Error(attribute, "AssociatedEnum requires integer or native enum storage, optionally through one pointer.");
                this.enumAssociations.Add((attribute, annotation.EnumName));
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
