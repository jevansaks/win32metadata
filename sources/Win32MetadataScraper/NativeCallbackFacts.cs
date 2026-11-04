using System;
using System.Collections.Generic;
using System.Linq;
using ClangSharp;
using ClangSharp.Interop;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Win32MetadataScraper
{
    // PInvokeGenerator does not apply qualified parameter attributes to callback typedefs,
    // and compatible-codegen represents anonymous C vtable slots as IntPtr.
    internal sealed class NativeCallbackFacts : CSharpSyntaxRewriter
    {
        private readonly Dictionary<string, (NamedDecl Owner, IReadOnlyList<ParmVarDecl> Parameters, HashSet<int> Retvals)> callbacks = new(StringComparer.Ordinal);
        private IReadOnlyDictionary<string, string> remaps;
        private IReadOnlyDictionary<string, string> types;
        private readonly List<DelegateDeclarationSyntax> vtableDelegates = new();
        private bool emittedVtableDelegate;

        public void Add(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters, int retval)
        {
            if (owner is not TypedefDecl && owner is not FieldDecl) return;
            string key = Name(owner);
            if (!this.callbacks.TryGetValue(key, out var item))
            {
                item = (owner, parameters, new HashSet<int>());
                this.callbacks.Add(key, item);
            }
            item.Retvals.Add(retval);
        }

        public string Apply(string source, IReadOnlyDictionary<string, string> remaps, IReadOnlyDictionary<string, string> types)
        {
            if (this.callbacks.Count == 0) return source;
            this.remaps = remaps;
            this.types = types;
            this.vtableDelegates.Clear();
            this.emittedVtableDelegate = false;
            return this.Visit(CSharpSyntaxTree.ParseText(source).GetRoot()).ToFullString();
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            node = (CompilationUnitSyntax)base.VisitCompilationUnit(node);
            if (this.emittedVtableDelegate && !node.Usings.Any(u => u.Name.ToString() == "System.Runtime.InteropServices"))
                node = node.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices"))
                    .NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));
            return node;
        }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            node = (NamespaceDeclarationSyntax)base.VisitNamespaceDeclaration(node);
            foreach (var declaration in this.vtableDelegates)
                if (node.Members.OfType<BaseTypeDeclarationSyntax>().Any(t => t.Identifier.ValueText == declaration.Identifier.ValueText) ||
                    node.Members.OfType<DelegateDeclarationSyntax>().Any(t => t.Identifier.ValueText == declaration.Identifier.ValueText))
                    throw new FormatException($"Native vtable callback '{declaration.Identifier.ValueText}' conflicts with a generated type.");
            node = node.AddMembers(this.vtableDelegates.ToArray());
            this.vtableDelegates.Clear();
            return node;
        }

        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            var matches = this.callbacks.Values.Where(c => c.Owner is TypedefDecl &&
                Projected(Name(c.Owner)) == node.Identifier.ValueText).ToArray();
            if (matches.Length > 1) throw Error(matches[0].Owner, "Ambiguous projected callback name.");
            if (matches.Length == 0) return base.VisitDelegateDeclaration(node);
            var callback = matches[0];
            if (node.ParameterList.Parameters.Count != callback.Parameters.Count)
                throw Error(callback.Owner, "Generated callback parameter count differs from the native declaration.");
            foreach (int index in callback.Retvals)
            {
                var parameter = node.ParameterList.Parameters[index];
                if (parameter.Identifier.ValueText != callback.Parameters[index].Name)
                    throw Error(callback.Owner, "Generated callback parameter binding differs from its native declaration.");
                node = node.WithParameterList(node.ParameterList.WithParameters(
                    node.ParameterList.Parameters.Replace(parameter, parameter.AddAttributeLists(Retval()))));
            }
            return node;
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var slots = this.callbacks.Values.Where(c => c.Owner is FieldDecl field &&
                Projected(Name((NamedDecl)field.DeclContext)) == node.Identifier.ValueText).ToArray();
            foreach (var slot in slots)
            {
                var field = (FieldDecl)slot.Owner;
                string fieldName = Projected(Name(field));
                var generated = node.Members.OfType<FieldDeclarationSyntax>()
                    .SingleOrDefault(f => f.Declaration.Variables.Single().Identifier.ValueText == fieldName);
                if (generated == null) continue;
                var pointer = RemapDiscovery.UnwrapType(field.Type) as PointerType;
                var prototype = pointer == null ? null : RemapDiscovery.UnwrapType(pointer.PointeeType) as FunctionProtoType;
                // Canonical storage retains calling conventions; the authored prototype retains return aliases.
                var nativePointer = field.Type.CanonicalType as PointerType;
                var nativePrototype = nativePointer?.PointeeType.CanonicalType as FunctionProtoType;
                if (prototype == null || nativePrototype == null || prototype.IsVariadic)
                    throw Error(field, "Native retval vtable slot requires a non-variadic native function pointer.");
                string delegateName = node.Identifier.ValueText + "_" + fieldName;
                string convention = nativePrototype.CallConv switch
                {
                    CXCallingConv.CXCallingConv_X86StdCall => "StdCall",
                    CXCallingConv.CXCallingConv_C => "Cdecl",
                    CXCallingConv.CXCallingConv_Win64 => "Winapi",
                    _ => throw Error(field, "Unsupported native vtable calling convention."),
                };
                var parameters = slot.Parameters.Select((p, index) =>
                {
                    string name = SyntaxFacts.GetKeywordKind(p.Name) == SyntaxKind.None ? p.Name : "@" + p.Name;
                    var parameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
                        .WithType(SyntaxFactory.ParseTypeName(this.TypeName(p.Type, field)));
                    var attributes = new List<AttributeListSyntax> { Attribute("NativeTypeName", p.Type.AsString) };
                    var storage = p.Type.CanonicalType;
                    if (storage is PointerType or ReferenceType && storage.PointeeType.IsLocalConstQualified)
                        attributes.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Attribute(SyntaxFactory.ParseName("Const")))));
                    var sal = p.Attrs.Where(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                        a.Spelling.StartsWith("Name=SAL_", StringComparison.Ordinal)).Select(a => a.Spelling).ToArray();
                    if (sal.Length != 0) attributes.Add(Attribute("CppAttributeList", string.Join("^", sal)));
                    attributes.AddRange(p.Attrs.Where(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                        a.Spelling.StartsWith("Win32Metadata.", StringComparison.Ordinal))
                        .Select(a => Attribute("NativeAnnotation", a.Spelling)));
                    if (slot.Retvals.Contains(index)) attributes.Add(Retval());
                    return parameter.WithAttributeLists(SyntaxFactory.List(attributes));
                });
                var declaration = SyntaxFactory.DelegateDeclaration(
                    SyntaxFactory.ParseTypeName(this.TypeName(prototype.ReturnType, field)), delegateName)
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.UnsafeKeyword)))
                    .WithAttributeLists(SyntaxFactory.List(new[]
                    {
                        SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(
                            SyntaxFactory.ParseName("UnmanagedFunctionPointer"),
                            SyntaxFactory.ParseAttributeArgumentList("(CallingConvention." + convention + ")")))),
                        Attribute("NativeTypeName", prototype.ReturnType.AsString).WithTarget(
                            SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Token(SyntaxKind.ReturnKeyword))),
                    }))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    .NormalizeWhitespace().WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
                node = node.ReplaceNode(generated, generated.WithDeclaration(generated.Declaration.WithType(
                    SyntaxFactory.ParseTypeName(delegateName).WithTrailingTrivia(SyntaxFactory.Space)))) as StructDeclarationSyntax;
                this.vtableDelegates.Add(declaration);
                this.emittedVtableDelegate = true;
            }
            return base.VisitStructDeclaration(node);
        }

        private string TypeName(ClangSharp.Type type, Decl site)
        {
            type = RemapDiscovery.UnwrapType(type);
            if (type is TypedefType alias)
            {
                string name = Name(alias.Decl);
                if (this.types.TryGetValue(name, out string configured)) return configured;
                return this.TypeName(alias.Decl.UnderlyingType, site);
            }
            if (type is PointerType pointer) return this.TypeName(pointer.PointeeType, site) + "*";
            if (type is LValueReferenceType reference) return this.TypeName(reference.PointeeType, site) + "*";
            if (type is TagType tag) return Projected(Name(tag.Decl));
            return type.Kind switch
            {
                CXTypeKind.CXType_Void => "void",
                CXTypeKind.CXType_Bool => "bool",
                CXTypeKind.CXType_Char_U or CXTypeKind.CXType_UChar => "byte",
                CXTypeKind.CXType_Char_S or CXTypeKind.CXType_SChar => "sbyte",
                CXTypeKind.CXType_WChar or CXTypeKind.CXType_UShort => "ushort",
                CXTypeKind.CXType_Short => "short",
                CXTypeKind.CXType_Int or CXTypeKind.CXType_Long when type.Handle.SizeOf == 4 => "int",
                CXTypeKind.CXType_UInt or CXTypeKind.CXType_ULong when type.Handle.SizeOf == 4 => "uint",
                CXTypeKind.CXType_LongLong => "long",
                CXTypeKind.CXType_ULongLong => "ulong",
                CXTypeKind.CXType_Float => "float",
                CXTypeKind.CXType_Double => "double",
                _ => throw Error(site, $"Unsupported native retval callback storage '{type.AsString}'."),
            };
        }

        private string Projected(string name)
        {
            if (this.remaps.TryGetValue(name, out string mapped)) return mapped;
            return name.Substring(name.LastIndexOf("::", StringComparison.Ordinal) + (name.Contains("::") ? 2 : 1));
        }

        private static AttributeListSyntax Retval() => Attribute("NativeAnnotation", "Win32Metadata.RetVal");

        private static AttributeListSyntax Attribute(string name, string payload) =>
            SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(name), SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(payload))))))));

        private static string Name(NamedDecl declaration) =>
            declaration.DeclContext is NamedDecl parent ? Name(parent) + "::" + declaration.Name : declaration.Name;

        private static FormatException Error(Decl declaration, string message)
        {
            declaration.Location.GetPresumedLocation(out var file, out uint line, out uint column);
            return new FormatException($"{file}:{line}:{column}: {message}");
        }
    }
}
