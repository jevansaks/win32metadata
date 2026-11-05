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
        public Dictionary<string, List<string>> AssociatedConstants { get; } = new(StringComparer.Ordinal);
        private readonly List<(Decl Declaration, Attr Attribute, string Name)> constantAssociations = new();
        private readonly Dictionary<string, ClangSharp.Type> enumStorage = new(StringComparer.Ordinal);
        private readonly Dictionary<string, (string Version, Cursor Source)> supportedFunctions = new(StringComparer.Ordinal);
        private readonly SupportedOSScopes supportedScopes = new();
        private readonly HashSet<NamedDecl> supportedDeclarations = new();
        private readonly Dictionary<Decl, (string Version, Cursor Source)> explicitSupportedOS = new();
        public Dictionary<string, string> SupportedTypes { get; } = new(StringComparer.Ordinal);
        private readonly Dictionary<string, (string Module, Attr Attribute)> importLibraries = new(StringComparer.Ordinal);
        private readonly Dictionary<string, (ProducerAnnotation Annotation, FunctionDecl Function, Attr Attribute)> producers = new(StringComparer.Ordinal);
        private readonly Dictionary<string, HashSet<FunctionDecl>> functionDeclarations = new(StringComparer.Ordinal);
        private readonly List<(VarDecl Declaration, Attr Marker)> carriers = new();
        private readonly Dictionary<string, (NamedDecl Target, HashSet<string> Payloads)> nativeFacts = new(StringComparer.Ordinal);
        public NativeCallbackFacts CallbackFacts { get; } = new();

        public void MergeNativeFacts(List<string> attributes, List<string> excludes)
        {
            foreach (var carrier in this.carriers) excludes.Add(carrier.Declaration.Name);
            foreach (var fact in this.nativeFacts)
                foreach (string payload in fact.Value.Payloads)
                {
                    string attribute = fact.Key + "=" + payload;
                    if (fact.Value.Target is not EnumDecl || !attributes.Contains(attribute, StringComparer.Ordinal))
                        attributes.Add(attribute);
                }
        }

        public void ResolveCarriers()
        {
            foreach (var carrier in this.carriers)
            {
                var marker = carrier.Marker;
                var arguments = marker.CursorChildren.ToArray();
                if (arguments.Length != 1 || SizeOfOperand(arguments[0], marker) is not UnaryOperator address ||
                    address.Opcode != CXUnaryOperatorKind.CXUnaryOperator_AddrOf ||
                    StripParens(address.SubExpr) is not DeclRefExpr reference || reference.Decl is not NamedDecl target)
                    throw Error(marker, "Named MIDL carriers require one compiler-resolved address of a native member.");
                if (marker.Spelling == Owner + "Field")
                {
                    if (target is not FieldDecl)
                        throw Error(marker, "AnnotateField requires a native non-static record field.");
                }
                else
                {
                    if (target is not CXXMethodDecl method || method.HasBody || method.IsStatic)
                        throw Error(marker, "AnnotateMethod/AnnotateParameter requires one native instance method declaration without a body.");
                    if (this.functionDeclarations[QualifiedName(method)].Count != 1)
                        throw Error(marker, "Named MIDL carriers cannot target overloaded methods.");
                    if (marker.Spelling != Owner + "Method")
                    {
                        var match = Regex.Match(marker.Spelling, @"\AWin32Metadata\.Parameter\(([A-Za-z_][A-Za-z_0-9]*)\)\z");
                        if (!match.Success)
                            throw Error(marker, "Malformed named MIDL parameter carrier.");
                        target = method.Parameters.SingleOrDefault(p => p.Name == match.Groups[1].Value) ??
                            throw Error(marker, $"No native parameter '{match.Groups[1].Value}' on '{QualifiedName(method)}'.");
                    }
                }

                var facts = carrier.Declaration.Attrs.Where(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                    a != marker && a.Spelling.StartsWith(Owner, StringComparison.Ordinal)).ToArray();
                if (facts.Length == 0)
                    throw Error(marker, "Named MIDL carrier requires an accepted owned annotation.");
                foreach (var fact in facts)
                {
                    if (target.Attrs.Any(a => a.Spelling == fact.Spelling))
                        throw Error(fact, "Named MIDL carrier duplicates an annotation on the native target.");
                    if (fact.Spelling.StartsWith(Owner + "AssociatedEnum(", StringComparison.Ordinal) &&
                        target.Attrs.Any(a => a.Spelling.StartsWith(Owner + "AssociatedEnum(", StringComparison.Ordinal)))
                        throw Error(fact, "Named MIDL carrier conflicts with the native target's enum association.");
                    if (fact.Spelling.StartsWith(SupportedOSAnnotation.Prefix, StringComparison.Ordinal))
                    {
                        this.VisitSupportedOS(target, fact);
                        continue;
                    }
                    if (fact.Spelling == Owner + "PreserveResult")
                    {
                        if (target is not CXXMethodDecl || fact.CursorChildren.Any())
                            throw Error(fact, "PreserveResult carrier requires a method and no arguments.");
                    }
                    else if (fact.Spelling.StartsWith(ProducerAnnotation.Marker, StringComparison.Ordinal))
                    {
                        if (this.producers.ContainsKey(QualifiedName(target)))
                            throw Error(fact, "Duplicate or conflicting RAIIFree carrier.");
                        this.VisitProducer(target, fact, transported: true);
                        continue;
                    }
                    else
                    {
                        uint? start = null;
                        this.VisitParameterAnnotation(target, fact, ref start, transported: true);
                    }
                    this.AddNativeFact(target, NativePayload(fact.Spelling), fact);
                }
            }
        }

        private static string NativePayload(string payload) =>
            "NativeAnnotation(\"" + payload.Replace("\"", "\\\"") + "\")";

        private void AddNativeFact(NamedDecl target, string payload, Attr source = null, string selector = null)
        {
            selector ??= QualifiedName(target);
            if (!this.nativeFacts.TryGetValue(selector, out var existing))
                this.nativeFacts.Add(selector, (target, new HashSet<string>(StringComparer.Ordinal) { payload }));
            else
            {
                const string enumPrefix = "NativeAnnotation(\"" + Owner + "AssociatedEnum(";
                if (source != null && payload.StartsWith(enumPrefix, StringComparison.Ordinal) &&
                    existing.Payloads.Any(p => p.StartsWith(enumPrefix, StringComparison.Ordinal)))
                    throw Error(source, $"Duplicate or conflicting enum association on '{selector}'.");
                if (existing.Target != target || !existing.Payloads.Add(payload))
                {
                    if (source != null) throw Error(source, $"Duplicate or ambiguous native annotation target '{selector}'.");
                    if (existing.Target.CanonicalDecl != target.CanonicalDecl)
                        throw new FormatException($"{target.Location}: Ambiguous native annotation target '{selector}'.");
                }
            }
        }

        public void MergeProducers(List<string> attributes, IReadOnlyDictionary<string, string> remaps, IReadOnlyCollection<string> excludes)
        {
            foreach (var fact in this.producers)
            {
                var producer = fact.Value.Annotation;
                if (this.functionDeclarations[QualifiedName(fact.Value.Function)].Count != 1)
                    throw Error(fact.Value.Attribute, "RAIIFree on overloaded producer names is unsupported; configured attributes cannot distinguish every generated overload.");
                if (excludes.Any(exclude => Regex.IsMatch(producer.Cleanup, "\\A" +
                    Regex.Escape(exclude).Replace("\\*", ".*").Replace("\\?", ".") + "\\z")))
                    throw Error(fact.Value.Attribute, $"RAIIFree cleanup '{producer.Cleanup}' is excluded from metadata.");
                string cleanup = remaps.GetValueOrDefault(producer.Cleanup) ?? producer.Cleanup;
                string payload = new ProducerAnnotation(cleanup, producer.InvalidValues).Payload;
                attributes.Add(fact.Key + "=NativeAnnotation(\"" + payload.Replace("\"", "\\\"") + "\")");
            }
        }

        public void MergeImportLibraries(Dictionary<string, string> libraries, IReadOnlyDictionary<string, string> remaps)
        {
            foreach (var fact in this.importLibraries)
                libraries[remaps.GetValueOrDefault(fact.Key) ?? fact.Key] = fact.Value.Module;
        }

        public void MergeSupportedFunctions(List<string> attributes, IReadOnlyDictionary<string, string> remaps)
        {
            foreach (var fact in this.supportedFunctions)
            {
                string name = remaps.GetValueOrDefault(fact.Key) ?? fact.Key;
                string value = $"SupportedOSPlatform(\"{fact.Value.Version}\")";
                foreach (string configured in attributes.Where(a => a.StartsWith(name + "=SupportedOSPlatform", StringComparison.Ordinal)))
                    if (configured != name + "=" + value)
                        throw SupportedError(fact.Value.Source, $"Legacy SupportedOSPlatform conflicts with native availability for '{name}'.");
                if (!attributes.Contains(name + "=" + value))
                    attributes.Add(name + "=" + value);
            }
        }

        public void Visit(Decl declaration)
        {
            this.supportedScopes.Visit(declaration);
            var markers = declaration.Attrs.Where(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                (a.Spelling == Owner + "Method" || a.Spelling == Owner + "Field" ||
                a.Spelling.StartsWith(Owner + "Parameter(", StringComparison.Ordinal))).ToArray();
            if (markers.Length != 0)
            {
                if (declaration is not VarDecl carrier || markers.Length != 1 ||
                    declaration.DeclContext is not TranslationUnitDecl && declaration.DeclContext is not LinkageSpecDecl)
                    throw Error(markers[0], "Named MIDL carriers require a separate file-scope metadata declaration.");
                this.carriers.Add((carrier, markers[0]));
                return;
            }
            if (declaration is NamedDecl named &&
                (declaration is FunctionDecl or RecordDecl or EnumDecl or TypedefDecl))
                this.supportedDeclarations.Add(named);
            if (declaration is FunctionDecl declaredFunction)
            {
                string name = QualifiedName(declaredFunction);
                if (!this.functionDeclarations.TryGetValue(name, out var declarations))
                    this.functionDeclarations.Add(name, declarations = new HashSet<FunctionDecl>());
                declarations.Add(declaredFunction.CanonicalDecl);
            }
            if (declaration is EnumDecl nativeEnum)
            {
                this.nativeEnums.Add(nativeEnum.Name);
                if (nativeEnum.Name.Length != 0 && nativeEnum.IsScoped)
                    this.AddNativeFact(nativeEnum.CanonicalDecl, "ScopedEnum", selector: QualifiedName(nativeEnum));
                if (nativeEnum.Name.Length != 0 &&
                    nativeEnum.Attrs.Any(attribute => attribute.Kind == CX_AttrKind.CX_AttrKind_FlagEnum))
                    this.AddNativeFact(nativeEnum.CanonicalDecl, "Flags", selector: QualifiedName(nativeEnum));
            }
            if (declaration is TypedefDecl alias && alias.UnderlyingType.CanonicalType is EnumType)
                this.nativeEnums.Add(alias.Name);
            uint? declarationStart = null;
            foreach (var attribute in declaration.Attrs)
            {
                if (attribute.Kind != CX_AttrKind.CX_AttrKind_Annotate ||
                    !attribute.Spelling.StartsWith(Owner, StringComparison.Ordinal))
                    continue;

                string payload = attribute.Spelling;
                if (payload.StartsWith(AssociatedConstantAnnotation.Prefix, StringComparison.Ordinal))
                {
                    this.VisitAssociatedConstant(declaration, attribute);
                    continue;
                }
                if (payload.StartsWith(Owner + "AlsoUsableFor", StringComparison.Ordinal))
                {
                    if (declaration is not TypedefNameDecl)
                        throw Error(attribute, "AlsoUsableFor requires a genuine native typedef or alias.");
                    continue;
                }
                if (payload.StartsWith(ProducerAnnotation.Marker, StringComparison.Ordinal))
                {
                    this.VisitProducer(declaration, attribute);
                    continue;
                }
                if (payload.StartsWith(Owner + "ImportLibrary", StringComparison.Ordinal))
                {
                    this.VisitImportLibrary(declaration, attribute);
                    continue;
                }
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
                bool preserveResult = payload == Owner + "PreserveResult";
                if (payload != Owner + "SetLastError" && !preserveResult)
                {
                    this.VisitParameterAnnotation(declaration, attribute, ref declarationStart);
                    continue;
                }
                if (attribute.CursorChildren.Any())
                    throw Error(attribute, $"{payload} accepts no arguments.");
                if (preserveResult && declaration is TypedefDecl callback)
                {
                    if (RemapDiscovery.UnwrapType(callback.UnderlyingType) is not PointerType callbackPointer ||
                        RemapDiscovery.UnwrapType(callbackPointer.PointeeType) is not FunctionProtoType)
                        throw Error(attribute, "PreserveResult requires the defining callback typedef, not a scalar or an alias of a callback.");
                    if (callback.Attrs.Count(a => a.Spelling == payload) != 1)
                        throw Error(attribute, "Duplicate PreserveResult annotations.");
                    attribute.Extent.Start.GetExpansionLocation(out _, out _, out _, out uint offset);
                    if (offset > GetDeclarationStart(callback, attribute))
                        throw Error(attribute, "PreserveResult must precede the complete callback typedef.");
                    continue;
                }
                if (declaration is not FunctionDecl function || !preserveResult && declaration is CXXMethodDecl || function.HasBody)
                    throw Error(attribute, $"{payload} requires a function or method declaration without a body.");
                if (preserveResult)
                {
                    if (function.Attrs.Count(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate && a.Spelling == payload) != 1)
                        throw Error(attribute, "Duplicate PreserveResult annotations.");
                    if (!function.CanonicalDecl.Attrs.Any(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate && a.Spelling == payload))
                        throw Error(attribute, "PreserveResult must appear on the first native declaration; late-only annotations are not reliably emitted by PInvokeGenerator.");
                }

                attribute.Extent.Start.GetExpansionLocation(out var attributeFile, out _, out _, out uint attributeOffset);
                declaration.Extent.Start.GetExpansionLocation(out var declarationFile, out _, out _, out _);
                declaration.Extent.End.GetExpansionLocation(out _, out _, out _, out uint declarationEnd);
                uint functionStart = GetDeclarationStart(function, attribute, preserveResult && declaration is CXXMethodDecl);
                // Clang propagates an annotation to other redeclarations, including earlier ones.
                if (attributeFile.Equals(declarationFile) &&
                    attributeOffset > functionStart && attributeOffset < declarationEnd)
                    throw Error(attribute, preserveResult ?
                        $"{payload} must precede the native return type and calling convention." :
                        "Win32Metadata.SetLastError must precede the complete function declaration.");

                if (!preserveResult) this.FunctionNames.Add(function.Name);
            }

            if (declaration is FunctionDecl containingFunction)
            {
                this.VisitNativeParameters(containingFunction, containingFunction.Parameters);
                foreach (var parameter in containingFunction.Parameters)
                {
                    this.Visit(parameter);
                    this.VisitTypedefSal(parameter, parameter.Type);
                }
                this.VisitTypedefSal(containingFunction, containingFunction.ReturnType, returnValue: true);
            }
            if (declaration is TypedefDecl containingCallback)
            {
                this.CallbackFacts.RegisterAlias(containingCallback);
                var parameters = containingCallback.CursorChildren.OfType<ParmVarDecl>().ToArray();
                this.VisitNativeParameters(containingCallback, parameters);
                this.VisitCallbackParameters(containingCallback, parameters);
            }
            if (declaration is FieldDecl field)
            {
                var parameters = field.CursorChildren.OfType<ParmVarDecl>().ToArray();
                this.VisitNativeParameters(field, parameters);
                this.VisitCallbackParameters(field, parameters);
                this.VisitTypedefSal(field, field.Type);
            }
        }

        private void VisitCallbackParameters(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                this.Visit(parameters[i]);
                this.VisitTypedefSal(parameters[i], parameters[i].Type,
                    callbackFact: () => this.CallbackFacts.AddDoubleNull(owner, parameters, i));
            }
        }

        private void VisitTypedefSal(NamedDecl declaration, ClangSharp.Type type, bool returnValue = false,
            Action callbackFact = null)
        {
            if (HasDoubleNull(declaration.Attrs))
            {
                if (returnValue) this.AddNativeFact(declaration, "return: NullNullTerminated");
                return;
            }
            var visited = new HashSet<TypedefNameDecl>();
            while (RemapDiscovery.UnwrapType(type) is TypedefType alias && visited.Add(alias.Decl))
            {
                if (HasDoubleNull(alias.Decl.Attrs))
                {
                    if (callbackFact != null) callbackFact();
                    else this.AddNativeFact(declaration, (returnValue ? "return: " : "") + "NullNullTerminated");
                    return;
                }
                type = alias.Decl.UnderlyingType;
            }
        }

        private static bool HasDoubleNull(IEnumerable<Attr> attributes) =>
            attributes.Any(a => a.Spelling.Contains("SAL_name", StringComparison.Ordinal) &&
                a.Spelling.Contains("_NullNull_terminated_", StringComparison.Ordinal));

        private void VisitNativeParameters(NamedDecl owner, IReadOnlyList<ParmVarDecl> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                var start = i == 0 ? owner.Extent.Start : parameters[i - 1].Extent.End;
                var unit = owner.TranslationUnit.Handle;
                var tokens = unit.Tokenize(CXSourceRange.Create(start, parameter.Location));
                bool retval = false;
                int depth = i == 0 ? 0 : 1;
                try
                {
                    foreach (var token in tokens)
                    {
                        using var spelling = token.GetSpelling(unit);
                        string text = spelling.ToString();
                        if (token.Kind == CXTokenKind.CXToken_Comment)
                        {
                            if (depth > 0 && Regex.IsMatch(text, @"\A/\*\s*(?:\[[A-Za-z_][A-Za-z_0-9]*(?:\([^][]*\))?\]\s*)+\*/\z") &&
                                Regex.IsMatch(text, @"\[retval\]"))
                                retval = true;
                        }
                        else if (text == "(") depth++;
                        else if (text == ")") depth--;
                        else if (text == "," && depth == 1) retval = false;
                    }
                }
                finally { unit.DisposeTokens(tokens); }
                if (!retval) continue;
                if (parameter.Type.CanonicalType is not PointerType)
                    throw new FormatException($"{parameter.Location}: Native MIDL retval requires an output pointer parameter.");
                if (parameter.Attrs.Any(a => a.Spelling == Owner + "RetVal"))
                    throw new FormatException($"{parameter.Location}: Do not duplicate authoritative MIDL retval with the C/C++ fallback.");
                if (owner is FunctionDecl)
                    this.AddNativeFact(parameter, NativePayload(Owner + "RetVal"),
                        selector: QualifiedName(owner) + "::" + parameter.Name);
                else this.CallbackFacts.Add(owner, parameters, i);
            }
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
            foreach (var association in this.constantAssociations)
            {
                if (association.Declaration is not EnumDecl &&
                    (association.Declaration is not TypedefDecl alias || !this.EnumGroups.ContainsKey(alias.Name)))
                    throw Error(association.Attribute, "AssociatedConstant requires a native enum or metadata-only enum group.");
                string name = ((NamedDecl)association.Declaration).Name;
                if (this.EnumGroups.Values.Any(group => group.members.Any(member => member.name == association.Name)))
                    throw Error(association.Attribute, $"AssociatedConstant '{association.Name}' must remain a loose global constant, not an enum group member.");
                if (!this.AssociatedConstants.TryGetValue(name, out var names))
                    this.AssociatedConstants.Add(name, names = new List<string>());
                if (names.Contains(association.Name, StringComparer.Ordinal))
                    throw Error(association.Attribute, $"Duplicate AssociatedConstant '{association.Name}' on '{name}'.");
                names.Add(association.Name);
            }
        }

        private void VisitAssociatedConstant(Decl declaration, Attr attribute)
        {
            if (this.constantAssociations.Any(association =>
                association.Declaration.CanonicalDecl == declaration.CanonicalDecl &&
                association.Attribute.Extent.Equals(attribute.Extent)))
                return;
            string name;
            try { name = AssociatedConstantAnnotation.Parse(attribute.Spelling); }
            catch (FormatException error) { throw Error(attribute, error.Message); }
            var arguments = attribute.CursorChildren.ToArray();
            if (arguments.Length != 1)
                throw Error(attribute, "AssociatedConstant requires one compiler-evaluated loose constant.");
            Cursor expression = arguments[0];
            while (expression is ParenExpr || expression is CastExpr)
                expression = expression is ParenExpr parenthesized ? parenthesized.SubExpr : ((CastExpr)expression).SubExpr;
            if (expression is DeclRefExpr reference && reference.Decl is EnumConstantDecl member && member.Name == name)
                throw Error(attribute, "AssociatedConstant requires a loose global constant, not an existing enumerator.");
            this.AddEnumConstant(name, arguments[0], attribute);
            this.constantAssociations.Add((declaration, attribute, name));
        }

        private ulong AddEnumConstant(string name, Cursor argument, Attr attribute)
        {
            using var value = argument.Handle.Evaluate;
            if (value.Kind != CXEvalResultKind.CXEval_Int || argument is not Expr expression)
                throw Error(attribute, $"Enum constituent '{name}' is not a supported integer constant expression.");
            ulong bits = value.IsUnsignedInt ? value.AsUnsigned : unchecked((ulong)value.AsLongLong);
            string type = GetIntegerStorage(expression.Type.CanonicalType, attribute);
            var constant = (Type: type, Value: FormatInteger(bits, type));
            if (this.EnumConstants.TryGetValue(name, out var existing) && existing != constant)
                throw Error(attribute, $"Conflicting native values for enum constituent '{name}'.");
            this.EnumConstants[name] = constant;
            return bits;
        }

        private void VisitProducer(Decl declaration, Attr attribute, bool transported = false)
        {
            if (attribute.Spelling != ProducerAnnotation.Marker)
                throw Error(attribute, "RAIIFree requires typed cleanup and invalid-value arguments.");
            FunctionDecl function = declaration as FunctionDecl ??
                (declaration as ParmVarDecl)?.DeclContext as FunctionDecl;
            if (function == null || function.HasBody || declaration is not FunctionDecl && declaration is not ParmVarDecl)
                throw Error(attribute, "RAIIFree requires a producer function/method return or output parameter declaration without a body.");
            var resource = declaration is ParmVarDecl parameter ? parameter.Type.CanonicalType : function.ReturnType.CanonicalType;
            if (declaration is ParmVarDecl output)
            {
                bool hasOutputSal = output.Attrs.Any(a => a.Spelling.Contains("SAL_name", StringComparison.Ordinal) &&
                    (a.Spelling.Contains("_Out_", StringComparison.Ordinal) ||
                    a.Spelling.Contains("_Out_opt_", StringComparison.Ordinal) ||
                    a.Spelling.Contains("_Outptr", StringComparison.Ordinal) ||
                    a.Spelling.Contains("_COM_Outptr", StringComparison.Ordinal) ||
                    Regex.IsMatch(a.Spelling, @"__RPC__(?:deref_)?out(?:_|;|$)")));
                if (!hasOutputSal || resource is not PointerType pointer)
                    throw Error(attribute, "RAIIFree requires output pointer SAL on a producer output parameter.");
                resource = pointer.PointeeType.CanonicalType;
            }
            if (!IsResourceType(resource))
                throw Error(attribute, "RAIIFree requires a native pointer or integer resource, not void, a record or a callback.");
            if (!transported && declaration.Attrs.Count(a => a.Kind == CX_AttrKind.CX_AttrKind_Annotate &&
                a.Spelling.StartsWith(ProducerAnnotation.Marker, StringComparison.Ordinal)) != 1)
                throw Error(attribute, "Duplicate or conflicting RAIIFree annotations.");

            var first = function.CanonicalDecl;
            Decl firstSite = first;
            if (declaration is ParmVarDecl item)
            {
                int index = function.Parameters.TakeWhile(p => p != item).Count();
                if (index >= first.Parameters.Count)
                    throw Error(attribute, "Cannot resolve the first producer output declaration.");
                firstSite = first.Parameters[index];
            }
            attribute.Extent.Start.GetExpansionLocation(out var attributeFile, out _, out _, out uint offset);
            firstSite.Extent.Start.GetExpansionLocation(out var firstFile, out _, out _, out _);
            firstSite.Extent.End.GetExpansionLocation(out _, out _, out _, out uint firstEnd);
            if (!transported && (!firstSite.Attrs.Any(a => a.Spelling == ProducerAnnotation.Marker) ||
                !attributeFile.Equals(firstFile) || offset > firstEnd)
                )
                throw Error(attribute, "RAIIFree must appear on the first native declaration; late-only producer annotations are unsupported.");
            declaration.Extent.Start.GetExpansionLocation(out var siteFile, out _, out _, out _);
            declaration.Extent.End.GetExpansionLocation(out _, out _, out _, out uint siteEnd);
            if (!transported && attributeFile.Equals(siteFile) && offset > GetDeclarationStart(declaration, attribute, declaration is CXXMethodDecl) && offset < siteEnd)
                throw Error(attribute, "RAIIFree must precede the native declaration type and calling convention.");

            var arguments = attribute.CursorChildren.ToArray();
            if (arguments.Length != 2)
                throw Error(attribute, "RAIIFree requires sizeof(&(cleanup)) as its typed cleanup transport.");
            Expr cleanupExpression = SizeOfOperand(arguments[0], attribute);
            if (cleanupExpression is not UnaryOperator address ||
                address.Opcode != CXUnaryOperatorKind.CXUnaryOperator_AddrOf ||
                StripParens(address.SubExpr) is not DeclRefExpr reference ||
                reference.Decl is not FunctionDecl cleanup || cleanup is CXXMethodDecl ||
                cleanup.HasBody || cleanup.Parameters.Count != 1 ||
                cleanup.Type.CanonicalType is not FunctionProtoType prototype || prototype.IsVariadic ||
                !IsResourceType(cleanup.Parameters[0].Type.CanonicalType) ||
                !cleanup.Parameters[0].Type.CanonicalType.Equals(resource))
                throw Error(attribute, "RAIIFree cleanup must resolve to one declared non-member, non-variadic native function accepting exactly the produced resource type.");
            string cleanupName = QualifiedName(cleanup);
            var invalidExpressions = new List<Expr>();
            ReadInvalidExpressions(SizeOfOperand(arguments[1], attribute), invalidExpressions, attribute);
            var values = invalidExpressions.Select(argument => EvaluateInvalidValue(argument, attribute)).ToArray();
            var annotation = new ProducerAnnotation(cleanupName, values);
            string selector = QualifiedName((NamedDecl)declaration);
            if (this.producers.TryGetValue(selector, out var existing) &&
                (existing.Annotation.Payload != annotation.Payload || existing.Function.CanonicalDecl != first))
                throw Error(attribute, $"Conflicting or overloaded RAIIFree producer declaration '{selector}'.");
            this.producers[selector] = (annotation, function, attribute);
        }

        private static bool IsResourceType(ClangSharp.Type type) =>
            type is PointerType pointer ? pointer.PointeeType.CanonicalType is not FunctionType :
            type.Kind is CXTypeKind.CXType_Int or CXTypeKind.CXType_UInt or CXTypeKind.CXType_Long or
                CXTypeKind.CXType_ULong or CXTypeKind.CXType_LongLong or CXTypeKind.CXType_ULongLong;

        private static Expr StripParens(Expr expression) =>
            expression is ParenExpr parens ? StripParens(parens.SubExpr) : expression;

        private static Expr SizeOfOperand(Cursor argument, Attr attribute)
        {
            if (argument is not UnaryExprOrTypeTraitExpr size ||
                size.Kind != CX_UnaryExprOrTypeTrait.CX_UETT_SizeOf || size.IsArgumentType)
                throw Error(attribute, "RAIIFree requires unevaluated sizeof expression operands for its typed transport.");
            return StripParens(size.ArgumentExpr);
        }

        private static void ReadInvalidExpressions(Expr expression, List<Expr> values, Attr attribute)
        {
            if (expression is BinaryOperator comma && comma.Opcode == CXBinaryOperatorKind.CXBinaryOperator_Comma)
            {
                ReadInvalidExpressions(comma.LHS, values, attribute);
                values.Add(comma.RHS);
            }
            else if (expression is not IntegerLiteral literal || literal.Value != 0)
                throw Error(attribute, "Malformed RAIIFree invalid-value transport.");
        }

        private static string QualifiedName(NamedDecl declaration) =>
            declaration.DeclContext is NamedDecl parent ? QualifiedName(parent) + "::" + declaration.Name : declaration.Name;

        private static long EvaluateInvalidValue(Cursor argument, Attr attribute)
        {
            if (argument is not Expr expression)
                throw Error(attribute, "RAIIFree invalid values require supported integer constant expressions.");
            expression = StripParens(expression);
            long pointerWidth = 0;
            while (expression.Type.CanonicalType is PointerType)
            {
                pointerWidth = expression.Type.Handle.SizeOf;
                if (expression is not CastExpr cast)
                    throw Error(attribute, "RAIIFree pointer invalid values require a constant integer-to-pointer cast; address and pointer arithmetic expressions are unsupported.");
                expression = StripParens(cast.SubExpr);
            }
            var integerType = expression.Type.CanonicalType;
            if (integerType is EnumType enumeration) integerType = enumeration.Decl.IntegerType.CanonicalType;
            if (integerType is not BuiltinType || integerType.Kind < CXTypeKind.CXType_Bool ||
                integerType.Kind > CXTypeKind.CXType_LongDouble || integerType.Kind is
                    CXTypeKind.CXType_Float or CXTypeKind.CXType_Double or CXTypeKind.CXType_LongDouble ||
                integerType.Handle.SizeOf > 8)
                throw Error(attribute, "RAIIFree invalid values require supported integer constants representable by Int64 metadata.");
            using var value = expression.Handle.Evaluate;
            if (value.Kind != CXEvalResultKind.CXEval_Int)
                throw Error(attribute, "RAIIFree invalid value is not a compiler-evaluated integer constant expression.");
            ulong bits = value.IsUnsignedInt ? value.AsUnsigned : unchecked((ulong)value.AsLongLong);
            if (pointerWidth != 0)
                return pointerWidth == 4 ? unchecked((int)bits) : unchecked((long)bits);
            if (value.IsUnsignedInt && bits > long.MaxValue)
                throw Error(attribute, "RAIIFree unsigned invalid value cannot be represented by Int64 metadata.");
            return unchecked((long)bits);
        }

        private void VisitImportLibrary(Decl declaration, Attr attribute)
        {
            var match = Regex.Match(attribute.Spelling,
                "^Win32Metadata\\.ImportLibrary\\(\"([A-Za-z0-9_][A-Za-z0-9_.-]*)\"\\)$");
            if (!match.Success || attribute.CursorChildren.Any())
                throw Error(attribute, "ImportLibrary requires one quoted module filename and no Clang expression arguments.");
            if (declaration is not FunctionDecl function || declaration is CXXMethodDecl || function.HasBody)
                throw Error(attribute, "ImportLibrary requires an imported function declaration without a body.");
            attribute.Extent.Start.GetExpansionLocation(out var attributeFile, out _, out _, out uint attributeOffset);
            declaration.Extent.Start.GetExpansionLocation(out var declarationFile, out _, out _, out _);
            declaration.Extent.End.GetExpansionLocation(out _, out _, out _, out uint declarationEnd);
            if (attributeFile.Equals(declarationFile) &&
                attributeOffset > GetDeclarationStart(function, attribute) && attributeOffset < declarationEnd)
                throw Error(attribute, "ImportLibrary must precede the complete function declaration.");
            string module = match.Groups[1].Value;
            if (this.importLibraries.TryGetValue(function.Name, out var existing) && existing.Module != module)
                throw Error(attribute, $"Conflicting ImportLibrary annotations on redeclarations of '{function.Name}'.");
            this.importLibraries[function.Name] = (module, attribute);
        }

        private void VisitSupportedOS(Decl declaration, Attr attribute)
        {
            string version;
            try { version = SupportedOSAnnotation.Parse(attribute.Spelling); }
            catch (FormatException error) { throw Error(attribute, error.Message); }
            if (attribute.CursorChildren.Any())
                throw Error(attribute, "SupportedOSPlatform accepts no Clang expression arguments.");
            if ((declaration is not FunctionDecl && declaration is not RecordDecl &&
                declaration is not EnumDecl && declaration is not TypedefDecl &&
                declaration is not EnumConstantDecl &&
                (declaration is not VarDecl constant || !constant.Type.Handle.IsConstQualified ||
                    constant.IsLocalVarDeclOrParm || !constant.HasInit && constant.Definition?.HasInit != true)) ||
                declaration is FunctionDecl function && function.HasBody)
                throw Error(attribute, "SupportedOSPlatform requires a function, method, record, enum, projected typedef or constant declaration.");
            if (this.explicitSupportedOS.TryGetValue(declaration.CanonicalDecl, out var existing) && existing.Version != version)
                throw Error(attribute, "Conflicting SupportedOSPlatform annotations on native redeclarations.");
            this.explicitSupportedOS[declaration.CanonicalDecl] = (version, attribute);
            if (declaration is NamedDecl named)
                this.supportedDeclarations.Add(named);
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

        public void ResolveSupportedOS(TranslationUnit unit, NativeTypedefs typedefs, IReadOnlyList<string> arguments)
        {
            this.supportedScopes.Resolve(unit, arguments);
            var resolved = new Dictionary<Decl, (NamedDecl Target, string Version, Cursor Source)>();
            foreach (var target in this.supportedDeclarations)
            {
                bool explicitAnnotation = this.explicitSupportedOS.TryGetValue(target.CanonicalDecl, out var fact);
                if (!explicitAnnotation)
                {
                    if (target is FunctionDecl function && (function.HasBody || function.IsDefaulted || function.IsDeleted) ||
                        target is VarDecl or EnumConstantDecl)
                        continue;
                    fact = this.supportedScopes.Default(target);
                    if (fact.Version == null) continue;
                }
                if (target is TypedefDecl alias && !typedefs.IsNominal(alias) && !IsDefiningCallback(alias))
                {
                    if (explicitAnnotation)
                        throw SupportedError(fact.Source, "SupportedOSPlatform typedefs require a projected callback or selected native typedef; flattened aliases have no independent emitted use site.");
                    continue;
                }
                if (resolved.TryGetValue(target.CanonicalDecl, out var previous) && previous.Version != fact.Version)
                    throw SupportedError(fact.Source, "Conflicting SupportedOS defaults on native redeclarations; supply explicit availability.");
                resolved[target.CanonicalDecl] = (target, fact.Version, fact.Source);
            }
            foreach (var fact in resolved.Values)
            {
                if (fact.Target is FunctionDecl &&
                    this.functionDeclarations[QualifiedName(fact.Target)].Count != 1)
                    throw SupportedError(fact.Source, "SupportedOSPlatform on overloaded native names cannot be projected independently without annotating neighboring overloads.");
                if (fact.Target is TypedefDecl alias && typedefs.IsNominal(alias))
                    this.SupportedTypes[QualifiedName(alias)] = fact.Version;
                else if (fact.Target is FunctionDecl && fact.Target is not CXXMethodDecl)
                {
                    if (this.supportedFunctions.TryGetValue(fact.Target.Name, out var previous) && previous.Version != fact.Version)
                        throw SupportedError(fact.Source, "Conflicting SupportedOSPlatform on overloaded native names.");
                    this.supportedFunctions[fact.Target.Name] = (fact.Version, fact.Source);
                }
                else
                    this.AddNativeFact((NamedDecl)fact.Target.CanonicalDecl,
                        NativePayload(SupportedOSAnnotation.Prefix + "(\"" + fact.Version + "\")"));
            }
        }

        private static bool IsDefiningCallback(TypedefDecl alias) =>
            alias.UnderlyingType is not TypedefType &&
            (RemapDiscovery.UnwrapType(alias.UnderlyingType) is FunctionProtoType ||
            RemapDiscovery.UnwrapType(alias.UnderlyingType) is PointerType pointer &&
            RemapDiscovery.UnwrapType(pointer.PointeeType) is FunctionProtoType);

        private static FormatException SupportedError(Cursor source, string message)
        {
            source.Location.GetExpansionLocation(out var file, out uint line, out uint column, out _);
            return new FormatException($"{file}:{line}:{column}: {message}");
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
                // Keep the compiler-typed constituent available to dependent constants.
                // The projected enum independently converts it to its authored storage.
                ulong bits = this.AddEnumConstant(names[i], arguments[i], attribute);
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

        private void VisitParameterAnnotation(Decl declaration, Attr attribute, ref uint? declarationStart, bool transported = false)
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
            if (!transported && declaration is ParmVarDecl redeclaredParameter && redeclaredParameter.DeclContext is FunctionDecl function)
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
            if (!transported) declarationStart ??= GetDeclarationStart(declaration, attribute);
            if (!transported && attributeFile.Equals(declarationFile) &&
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
                    a.Spelling.Contains("_Inout_updates", StringComparison.Ordinal) ||
                    Regex.IsMatch(a.Spelling, @"__RPC__(?:out|inout)_[eb]count")));
                if (!output)
                    throw Error(attribute, "ContainsInteriorPointers requires caller-owned output/inout buffer SAL.");
            }
            if (annotation.Kind == ParameterAnnotationKind.AssociatedEnum)
            {
                ClangSharp.Type storage = declaration is ParmVarDecl enumParameter ?
                    enumParameter.Type.CanonicalType : declaration is FieldDecl enumField ?
                    enumField.Type.CanonicalType : ((FunctionDecl)declaration).ReturnType.CanonicalType;
                while (storage is PointerType pointer) storage = pointer.PointeeType.CanonicalType;
                if (storage is EnumType)
                    throw Error(attribute, "AssociatedEnum is redundant on a genuinely enum-typed declaration.");
                if (storage is not BuiltinType ||
                    storage.Kind < CXTypeKind.CXType_Bool || storage.Kind > CXTypeKind.CXType_UInt128 &&
                    (storage.Kind < CXTypeKind.CXType_Char_S || storage.Kind > CXTypeKind.CXType_Int128))
                    throw Error(attribute, "AssociatedEnum requires integer or native enum storage, optionally through pointers.");
                this.enumAssociations.Add((attribute, annotation.EnumName, storage));
            }
        }

        internal static uint GetDeclarationStart(Decl function, Attr annotation, bool allowMidlVirtual = false)
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
                    if (allowMidlVirtual && text == "virtual") continue;
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
