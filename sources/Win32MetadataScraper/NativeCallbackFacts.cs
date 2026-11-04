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
        private sealed class Callback
        {
            public Callback(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters, Callback prototype = null)
            {
                this.Owner = owner;
                this.Parameters = parameters;
                this.Prototype = prototype?.Prototype ?? owner.CanonicalDecl;
                this.Retvals = prototype?.Retvals ?? new HashSet<int>();
                this.DoubleNulls = prototype?.DoubleNulls ?? new HashSet<int>();
            }

            public NamedDecl Owner { get; }
            public IReadOnlyList<ParmVarDecl> Parameters { get; }
            public Decl Prototype { get; }
            public HashSet<int> Retvals { get; }
            public HashSet<int> DoubleNulls { get; }
        }

        private readonly Dictionary<Decl, Callback> callbacks = new();
        private readonly Dictionary<Decl, (TypedefDecl Owner, Decl Target)> aliases = new();
        private IReadOnlyDictionary<string, string> remaps;
        private IReadOnlyDictionary<string, string> types;
        private readonly List<DelegateDeclarationSyntax> vtableDelegates = new();
        private bool emittedNativeDelegate;

        public void Add(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters, int retval)
        {
            this.GetCallback(owner, parameters).Retvals.Add(retval);
        }

        public void AddDoubleNull(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters, int index)
        {
            this.GetCallback(owner, parameters).DoubleNulls.Add(index);
        }

        public void RegisterAlias(TypedefDecl declaration)
        {
            var canonical = declaration.UnderlyingType.CanonicalType;
            if (canonical is not FunctionProtoType &&
                (canonical is not PointerType pointer || pointer.PointeeType.CanonicalType is not FunctionProtoType))
                return;
            var type = RemapDiscovery.UnwrapType(declaration.UnderlyingType);
            if (type is PointerType address) type = RemapDiscovery.UnwrapType(address.PointeeType);
            if (type is TypedefType alias)
                this.aliases[declaration.CanonicalDecl] = (declaration, alias.Decl.CanonicalDecl);
        }

        private Callback GetCallback(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters)
        {
            if (owner is not TypedefDecl && owner is not FieldDecl)
                throw Error(owner, "Native callback facts require a defining typedef or field prototype.");
            var key = owner.CanonicalDecl;
            if (!this.callbacks.TryGetValue(key, out var item))
            {
                item = new Callback(owner, parameters);
                this.callbacks.Add(key, item);
            }
            else if (!item.Parameters.Select(p => p.CanonicalDecl).SequenceEqual(parameters.Select(p => p.CanonicalDecl)))
                throw Error(owner, "Conflicting native callback parameter identity.");
            return item;
        }

        public string Apply(string source, IReadOnlyDictionary<string, string> remaps, IReadOnlyDictionary<string, string> types)
        {
            if (this.callbacks.Count == 0) return source;
            foreach (var alias in this.aliases)
            {
                var target = alias.Value.Target;
                var visited = new HashSet<Decl> { alias.Key };
                while (this.aliases.TryGetValue(target, out var next))
                {
                    if (!visited.Add(target)) throw Error(alias.Value.Owner, "Cyclic native callback alias.");
                    target = next.Target;
                }
                if (this.callbacks.TryGetValue(target, out var prototype))
                    this.callbacks[alias.Key] = new Callback(alias.Value.Owner, prototype.Parameters, prototype);
            }
            this.remaps = remaps;
            this.types = types;
            this.vtableDelegates.Clear();
            this.emittedNativeDelegate = false;
            return this.Visit(CSharpSyntaxTree.ParseText(source).GetRoot()).ToFullString();
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            node = (CompilationUnitSyntax)base.VisitCompilationUnit(node);
            if (this.emittedNativeDelegate && !node.Usings.Any(u => u.Name.ToString() == "System.Runtime.InteropServices"))
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
                Projected(Name(c.Owner)) == node.Identifier.ValueText).GroupBy(c => c.Prototype)
                .Select(g => g.First()).ToArray();
            if (matches.Length > 1) throw Error(matches[0].Owner, "Ambiguous projected callback name.");
            if (matches.Length == 0) return base.VisitDelegateDeclaration(node);
            var callback = matches[0];
            if (callback.Owner.CanonicalDecl != callback.Prototype &&
                node.ParameterList.Parameters.Count == 0 && callback.Parameters.Count != 0)
            {
                var defining = node.Parent?.ChildNodes().OfType<DelegateDeclarationSyntax>()
                    .SingleOrDefault(d => d.Identifier.ValueText == Projected(Name((NamedDecl)callback.Prototype)) &&
                        d.ParameterList.Parameters.Count == callback.Parameters.Count);
                var retained = node.AttributeLists.SelectMany(list => list.Attributes)
                    .Where(a => a.Name.ToString() is not "NativeTypeName" and not "UnmanagedFunctionPointer")
                    .Select(a => SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(a)));
                if (defining == null)
                    return this.NativeDelegate(callback, node.Identifier.ValueText)
                        .AddAttributeLists(retained.ToArray()).WithTriviaFrom(node);
                node = defining.WithIdentifier(node.Identifier).AddAttributeLists(retained.ToArray()).WithTriviaFrom(node);
            }
            if (node.ParameterList.Parameters.Count != callback.Parameters.Count)
                throw Error(callback.Owner, $"Generated callback '{node.Identifier.ValueText}' parameter count " +
                    $"{node.ParameterList.Parameters.Count} differs from the native declaration ({callback.Parameters.Count}).");
            foreach (int index in callback.Retvals.Union(callback.DoubleNulls))
            {
                var parameter = node.ParameterList.Parameters[index];
                if (callback.Parameters[index].Name.Length != 0 &&
                    parameter.Identifier.ValueText != callback.Parameters[index].Name)
                    throw Error(callback.Owner, "Generated callback parameter binding differs from its native declaration.");
                var attributes = new List<AttributeListSyntax>();
                if (callback.Retvals.Contains(index)) attributes.Add(Retval());
                if (callback.DoubleNulls.Contains(index)) attributes.Add(DoubleNull());
                node = node.WithParameterList(node.ParameterList.WithParameters(
                    node.ParameterList.Parameters.Replace(parameter, parameter.AddAttributeLists(attributes.ToArray()))));
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
                string delegateName = node.Identifier.ValueText + "_" + fieldName;
                var declaration = this.NativeDelegate(slot, delegateName);
                node = node.ReplaceNode(generated, generated.WithDeclaration(generated.Declaration.WithType(
                    SyntaxFactory.ParseTypeName(delegateName).WithTrailingTrivia(SyntaxFactory.Space)))) as StructDeclarationSyntax;
                this.vtableDelegates.Add(declaration);
            }
            return base.VisitStructDeclaration(node);
        }

        private DelegateDeclarationSyntax NativeDelegate(Callback callback, string name)
        {
            var type = callback.Prototype switch
            {
                TypedefDecl alias => alias.UnderlyingType,
                FieldDecl field => field.Type,
                _ => throw Error(callback.Owner, "Native callback prototype has no defining declaration."),
            };
            var canonical = type.CanonicalType;
            if (canonical is PointerType nativePointer) canonical = nativePointer.PointeeType.CanonicalType;
            type = RemapDiscovery.UnwrapType(type);
            if (type is PointerType pointer) type = RemapDiscovery.UnwrapType(pointer.PointeeType);
            var prototype = type as FunctionProtoType;
            // Canonical storage retains calling conventions; the authored prototype retains return aliases.
            var nativePrototype = canonical as FunctionProtoType;
            if (prototype == null || nativePrototype == null || prototype.IsVariadic)
                throw Error(callback.Owner, "Native callback facts require a non-variadic native function prototype.");
            string convention = nativePrototype.CallConv switch
            {
                CXCallingConv.CXCallingConv_X86StdCall => "StdCall",
                CXCallingConv.CXCallingConv_C => "Cdecl",
                CXCallingConv.CXCallingConv_Win64 => "Winapi",
                _ => throw Error(callback.Owner, "Unsupported native callback calling convention."),
            };
            var parameters = callback.Parameters.Select((p, index) =>
            {
                string parameterName = p.Name.Length == 0 ? "param" + index :
                    SyntaxFacts.GetKeywordKind(p.Name) == SyntaxKind.None ? p.Name : "@" + p.Name;
                var parameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameterName))
                    .WithType(SyntaxFactory.ParseTypeName(this.TypeName(p.Type, callback.Owner)));
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
                if (callback.Retvals.Contains(index)) attributes.Add(Retval());
                if (callback.DoubleNulls.Contains(index)) attributes.Add(DoubleNull());
                return parameter.WithAttributeLists(SyntaxFactory.List(attributes));
            });
            this.emittedNativeDelegate = true;
            return SyntaxFactory.DelegateDeclaration(
                SyntaxFactory.ParseTypeName(this.TypeName(prototype.ReturnType, callback.Owner)), name)
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
                _ => throw Error(site, $"Unsupported native callback storage '{type.AsString}'."),
            };
        }

        private string Projected(string name)
        {
            if (this.remaps.TryGetValue(name, out string mapped)) return mapped;
            return name.Substring(name.LastIndexOf("::", StringComparison.Ordinal) + (name.Contains("::") ? 2 : 1));
        }

        private static AttributeListSyntax Retval() => Attribute("NativeAnnotation", "Win32Metadata.RetVal");

        private static AttributeListSyntax DoubleNull() =>
            SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Attribute(SyntaxFactory.ParseName("NullNullTerminated"))));

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
