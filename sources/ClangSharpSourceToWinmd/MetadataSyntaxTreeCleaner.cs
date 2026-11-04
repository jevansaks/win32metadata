using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MetadataUtils;
using System.Xml.Linq;

namespace ClangSharpSourceToWinmd
{
    public static class MetadataSyntaxTreeCleaner
    {
        public const string ArgListVarName = "__arglist__";

        public static SyntaxTree CleanSyntaxTree(SyntaxTree tree, Dictionary<string, string> remaps, Dictionary<string, Dictionary<string, string>> enumAdditions, HashSet<string> enumsMakeFlags, Dictionary<string, string> requiredNamespaces, Dictionary<string, string> staticLibs, Dictionary<string, string> apiNamesToNamespaces, HashSet<string> nonEmptyStructs, HashSet<string> enumMemberNames, string filePath, HashSet<string> headerEnumNames = null, IReadOnlyDictionary<string, string> nativeTypedefs = null)
        {
            TreeRewriter treeRewriter = new TreeRewriter(remaps, enumAdditions, enumsMakeFlags, requiredNamespaces, staticLibs, apiNamesToNamespaces, nonEmptyStructs, enumMemberNames, headerEnumNames, nativeTypedefs);
            var newRoot = (CSharpSyntaxNode)treeRewriter.Visit(tree.GetRoot());
            var ret = CSharpSyntaxTree.Create(newRoot, null, filePath);
            return ret;
        }

        private class TreeRewriter : CSharpSyntaxRewriter
        {
            private static readonly Regex NativeTypeNameArrayRegex = new Regex(@"\[\s*(\d+)\s*\]$");
            private static readonly Regex ElementCountRegex = new Regex(@"(?:elementCount|byteCount)\((?:_Old_\()?([^\)]+)\)+");
            private static readonly Regex IsRegex = new Regex(@"[^\w::]");

            private HashSet<SyntaxNode> nodesWithMarshalAs = new HashSet<SyntaxNode>();
            private Dictionary<string, string> remaps;
            private Dictionary<Regex, string> regexRemaps = new Dictionary<Regex, string>();
            private Dictionary<string, Dictionary<string, string>> enumAdditions;
            private Dictionary<string, string> requiredNamespaces;
            private Dictionary<string, string> staticLibs;
            private Dictionary<string, string> apiNamesToNamespaces;
            private HashSet<string> visitedDelegateNames = new HashSet<string>();
            private HashSet<string> visitedStaticMethodNames = new HashSet<string>();
            private HashSet<string> nonEmptyStructs;
            private HashSet<string> enumMemberNames;
            private readonly HashSet<string> headerEnumNames;
            private readonly IReadOnlyDictionary<string, string> nativeTypedefs;
            private HashSet<string> enumsToMakeFlags;
            private HashSet<string> usingNamespaces = new HashSet<string>();

            public TreeRewriter(Dictionary<string, string> remaps, Dictionary<string, Dictionary<string, string>> enumAdditions, HashSet<string> enumsToMakeFlags, Dictionary<string, string> requiredNamespaces, Dictionary<string, string> staticLibs, Dictionary<string, string> apiNamesToNamespaces, HashSet<string> nonEmptyStructs, HashSet<string> enumMemberNames, HashSet<string> headerEnumNames, IReadOnlyDictionary<string, string> nativeTypedefs)
            {
                this.remaps = remaps;
                this.InitRegexRemaps();

                this.enumAdditions = enumAdditions;
                this.requiredNamespaces = requiredNamespaces;
                this.staticLibs = staticLibs;
                this.apiNamesToNamespaces = apiNamesToNamespaces;
                this.nonEmptyStructs = nonEmptyStructs;
                this.enumMemberNames = enumMemberNames;
                this.headerEnumNames = headerEnumNames ?? new HashSet<string>(StringComparer.Ordinal);
                this.nativeTypedefs = nativeTypedefs ?? new Dictionary<string, string>();
                this.enumsToMakeFlags = enumsToMakeFlags;
            }

            private void InitRegexRemaps()
            {
                foreach (var pair in this.remaps)
                {
                    if (IsRegex.IsMatch(pair.Key))
                    {
                        this.regexRemaps[new Regex(pair.Key)] = pair.Value;
                    }
                }
            }

            public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
            {
                var ret = (CompilationUnitSyntax)base.VisitCompilationUnit(node);

                // Add any namespaces we might need due to remappings
                var newUsings = new List<UsingDirectiveSyntax>();
                foreach (var remapNamespace in this.requiredNamespaces.Values.Distinct())
                {
                    if (!this.usingNamespaces.Contains(remapNamespace))
                    {
                        newUsings.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(remapNamespace)).NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));
                    }
                }

                if (newUsings.Count != 0)
                {
                    ret = ret.AddUsings(newUsings.ToArray());
                }

                return ret;
            }

            public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
            {
                this.usingNamespaces.Add(node.Name.ToString());
                return base.VisitUsingDirective(node);
            }

            private static SyntaxList<AttributeListSyntax> FixRemappedAttributes(
                SyntaxList<AttributeListSyntax> existingAttrList,
                List<AttributeSyntax> remappedListAttributes)
            {
                return FixRemappedAttributes(existingAttrList, remappedListAttributes, null);
            }

            private static SyntaxList<AttributeListSyntax> FixRemappedAttributes(
                SyntaxList<AttributeListSyntax> existingAttrList,
                List<AttributeSyntax> remappedListAttributes,
                AttributeTargetSpecifierSyntax target)
            {
                if (remappedListAttributes == null)
                {
                    return existingAttrList;
                }

                var replacedNames = new HashSet<string>(StringComparer.Ordinal);
                foreach (var attrNode in remappedListAttributes)
                {
                    var attrName = attrNode.Name.ToString();
                    if (attrName.EndsWith(EncodeHelpers.AttributeToRemoveSuffix))
                    {
                        var attrNameToRemove = attrName.Substring(0, attrName.Length - EncodeHelpers.AttributeToRemoveSuffix.Length);

                        existingAttrList = SyntaxUtils.RemoveAttribute(existingAttrList, attrNameToRemove);
                    }
                    else
                    {
                        // Remove any existing attribute with the same name
                        if (attrName is not "InvalidHandleValue" and not "InvalidHandleValueAttribute" ||
                            replacedNames.Add(attrName))
                            existingAttrList = SyntaxUtils.RemoveAttribute(existingAttrList, attrName);

                        var attrListNode =
                            SyntaxFactory.AttributeList(
                                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(attrNode));
                        if (target != null)
                        {
                            attrListNode = attrListNode.WithTarget(target);
                        }

                        existingAttrList = existingAttrList.Add(attrListNode);
                    }
                }

                return existingAttrList;
            }

            public override SyntaxNode VisitParameter(ParameterSyntax node)
            {
                if (node.Identifier.Text == "__arglist")
                {
                    // Special case __arglist to give it the type of "object" and a name we'll recognize in the emitter.
                    // Otherwise the compilation will fail if __arglist is found in any delegates
                    return node
                        .WithIdentifier(SyntaxFactory.Identifier(ArgListVarName))
                        .WithTrailingTrivia(SyntaxFactory.Space)
                        .WithType(SyntaxFactory.ParseTypeName("object")
                        .WithTrailingTrivia(SyntaxFactory.Space));
                }

                string fullName = GetFullNameWithoutArchSuffix(node);
                string headerType = this.GetHeaderEnumType(node.AttributeLists, node.Type.ToString(), fullName);

                if (this.GetRemapInfo(fullName, node.AttributeLists, out var listAttributes, node.Type.ToString(), out string newType, out string newName))
                {
                    var ownedNames = GetOwnedAnnotationNames(node.AttributeLists);
                    node = (ParameterSyntax)base.VisitParameter(node);
                    VerifyOwnedRemaps(fullName, ownedNames, node.AttributeLists, listAttributes);
                    if (headerType != null) RemoveHeaderAssociation(listAttributes);
                    node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes));

                    if (newName != null)
                    {
                        node = node.WithIdentifier(SyntaxFactory.Identifier(newName));
                    }

                    if (newType != null)
                    {
                        node = node.WithType(SyntaxFactory.ParseTypeName(newType).WithTrailingTrivia(SyntaxFactory.Space));
                    }
                }
                else
                {
                    node = (ParameterSyntax)base.VisitParameter(node);
                }

                // Get rid of default parameter values
                if (headerType != null)
                    node = node.WithType(SyntaxFactory.ParseTypeName(headerType).WithTrailingTrivia(SyntaxFactory.Space));
                if (node.Default != null)
                {
                    node = node.WithDefault(null);
                }

                return node;
            }

            private string[] GetOwnedAnnotationNames(SyntaxList<AttributeListSyntax> attributes) =>
                attributes.SelectMany(list => list.Attributes)
                    .Where(attribute => attribute.Name.ToString() == "NativeAnnotation")
                    .Select(attribute => (attribute.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token.ValueText)
                    .Where(payload => payload != null && payload.StartsWith(ParameterAnnotation.Owner, StringComparison.Ordinal) &&
                        payload != ParameterAnnotation.Owner + "SetLastError" &&
                        payload != ParameterAnnotation.Owner + "PreserveResult" &&
                        !payload.StartsWith(ParameterAnnotation.Owner + "AlsoUsableFor(", StringComparison.Ordinal) &&
                        !payload.StartsWith(ParameterAnnotation.Owner + "ImportLibrary(", StringComparison.Ordinal) &&
                        !payload.StartsWith(SupportedOSAnnotation.Prefix, StringComparison.Ordinal))
                    .SelectMany(payload => payload == ProducerAnnotation.Marker ? Array.Empty<string>() :
                        payload.StartsWith(ProducerAnnotation.Marker + "(", StringComparison.Ordinal) ?
                        new[] { "RAIIFree", "InvalidHandleValue" } : OwnedParameterNames(payload))
                    .Distinct(StringComparer.Ordinal).ToArray();

            private IEnumerable<string> OwnedParameterNames(string payload) =>
                new[] { ParameterAnnotation.Parse(payload) }
                    .Where(annotation => annotation.Kind != ParameterAnnotationKind.AssociatedEnum ||
                        !this.headerEnumNames.Contains(annotation.EnumName))
                    .Select(annotation => annotation.AttributeName);

            private string GetHeaderEnumType(SyntaxList<AttributeListSyntax> attributes, string type, string fullName)
            {
                var annotation = attributes.SelectMany(list => list.Attributes)
                    .Where(a => a.Name.ToString() == "NativeAnnotation")
                    .Select(a => (a.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token.ValueText)
                    .Where(p => p != null && p.StartsWith(ParameterAnnotation.Owner + "AssociatedEnum(", StringComparison.Ordinal))
                    .Select(ParameterAnnotation.Parse).SingleOrDefault();
                string enumName = annotation.EnumName;
                if (enumName == null)
                    enumName = SyntaxUtils.GetNativeTypeNameFromAttributesLists(attributes)?.TrimEnd('*', ' ');
                if (enumName == null || !this.headerEnumNames.Contains(enumName)) return null;
                string result = enumName + new string('*', type.Length - type.TrimEnd('*').Length);
                this.GetRemapInfo(fullName, attributes, out var remappedAttributes, type, out var remappedType, out _);
                if (remappedType != null && remappedType != result)
                    throw new FormatException($"Configured type for '{fullName}' conflicts with header enum group '{enumName}'.");
                foreach (var association in (remappedAttributes ?? new List<AttributeSyntax>())
                    .Where(attribute => attribute.Name.ToString() is "AssociatedEnum" or "AssociatedEnumAttribute"))
                    if (association.ArgumentList?.Arguments.Count != 1 ||
                        association.ArgumentList.Arguments[0].Expression is not LiteralExpressionSyntax literal ||
                        literal.Token.ValueText != enumName)
                        throw new FormatException($"Configured association for '{fullName}' conflicts with header enum group '{enumName}'.");
                return result;
            }

            private static void RemoveHeaderAssociation(List<AttributeSyntax> attributes) =>
                attributes?.RemoveAll(attribute => attribute.Name.ToString() is "AssociatedEnum" or "AssociatedEnumAttribute");

            private static void VerifyOwnedRemaps(string fullName, string[] ownedNames,
                SyntaxList<AttributeListSyntax> attributes, List<AttributeSyntax> remaps)
            {
                foreach (var ownedName in ownedNames)
                {
                    var existing = attributes.SelectMany(list => list.Attributes)
                        .Where(attribute => attribute.Name.ToString() == ownedName).ToArray();
                    var matching = (remaps ?? Enumerable.Empty<AttributeSyntax>())
                        .Where(remap => remap.Name.ToString() == ownedName ||
                            remap.Name.ToString() == ownedName + "Attribute").ToArray();
                    foreach (var remap in remaps ?? Enumerable.Empty<AttributeSyntax>())
                        if (remap.Name.ToString() == ownedName + EncodeHelpers.AttributeToRemoveSuffix)
                            throw new FormatException($"{fullName}: legacy remap conflicts with native {ownedName}.");
                    if (matching.Length != 0 && (matching.Length != existing.Length ||
                        matching.Where((remap, index) => !SyntaxFactory.AreEquivalent(existing[index].ArgumentList, remap.ArgumentList)).Any()))
                        throw new FormatException($"{fullName}: legacy remap conflicts with native {ownedName}.");
                    // Keep the entire native ordered group, including repeated invalid values.
                    remaps?.RemoveAll(remap => matching.Contains(remap));
                }
            }

            public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
            {
                // If the struct is empty and we found a non-empty struct in all the source files, delete it
                if (SyntaxUtils.IsEmptyStruct(node) && this.nonEmptyStructs.Contains(node.Identifier.ValueText) &&
                    !node.Modifiers.Any(token => token.IsKind(SyntaxKind.PartialKeyword)))
                {
                    return null;
                }

                string name = node.Identifier.Text;
                string fullName = GetFullNameWithoutArchSuffix(node);

                // Add Ansi or Unicode attributes to -A/-W APIs.
                if ((name.EndsWith("A") && this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}W")) ||
                   (!name.EndsWith("W") && this.apiNamesToNamespaces.ContainsKey($"{name}W")))
                {
                    var attributeList = SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.ParseName("Ansi"))));

                    node = node.AddAttributeLists(attributeList);
                }
                else if (name.EndsWith("W") && (this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}A") || this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}")))
                {
                    var attributeList = SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.ParseName("Unicode"))));

                    node = node.AddAttributeLists(attributeList);
                }

                foreach (var member in node.Members)
                {
                    if (!(member is FieldDeclarationSyntax))
                    {
                        continue;
                    }

                    var fieldName = ((FieldDeclarationSyntax)member).Declaration.Variables[0].Identifier.Value.ToString();
                    if (fieldName == "cbSize")
                    {
                        var attributeList = SyntaxFactory.AttributeList(
                                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                    SyntaxFactory.Attribute(
                                        SyntaxFactory.ParseName("StructSizeField"),
                                        SyntaxFactory.ParseAttributeArgumentList($"(\"{fieldName}\")"))));

                        node = node.AddAttributeLists(attributeList);
                    }
                }

                if (this.GetRemapInfo(fullName, node.AttributeLists, out var listAttributes, null, out _, out string newName))
                {
                    // Definition-wide documentation belongs to the compiler declaration, not its legacy facts partial.
                    if (System.IO.Path.GetFileName(node.SyntaxTree.FilePath) == "autotypes.modified.cs" && node.Members.Count == 0 &&
                        this.nativeTypedefs.ContainsKey(SyntaxUtils.GetEnclosingNamespace(node) + "." + name))
                        listAttributes?.RemoveAll(attribute => attribute.Name.ToString() is "Documentation" or "DocumentationAttribute");
                    if (node.SyntaxTree.FilePath.EndsWith(".headerTypedefs.modified.cs", StringComparison.Ordinal))
                    {
                        var ownedNames = node.AttributeLists.SelectMany(list => list.Attributes)
                            .Select(attribute => attribute.Name.ToString())
                            .Where(name => name is "NativeTypedef" or "AlsoUsableFor").ToArray();
                        var location = node.GetLocation().GetMappedLineSpan();
                        VerifyOwnedRemaps($"{location.Path}:{location.StartLinePosition.Line + 1}:{location.StartLinePosition.Character + 1}: {fullName}",
                            ownedNames, node.AttributeLists, listAttributes);
                    }
                    node = (StructDeclarationSyntax)base.VisitStructDeclaration(node);
                    node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes));

                    if (newName != null)
                    {
                        node = node.WithIdentifier(SyntaxFactory.Identifier(newName));
                    }
                }
                else
                {
                    node = (StructDeclarationSyntax)base.VisitStructDeclaration(node);
                }

                return node;
            }

            public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
            {
                const string ForceConstPrefix = "__forceconst__";

                string localName = node.Declaration.Variables.First().Identifier.ValueText;

                if (node.Modifiers.ToString() == "public const")
                {
                    // Only generated header backing constants must survive until expression binding.
                    if (node.SyntaxTree.FilePath.EndsWith(".headerConstants.modified.cs", StringComparison.Ordinal))
                        return node;
                    if (this.enumMemberNames.Contains(localName))
                        return null;
                }

                string fullName = GetFullNameWithoutArchSuffix(node);

                var nativeCasts = new Dictionary<string, string>(StringComparer.Ordinal);
                string nativeStorage = null;
                if (node.Modifiers.Any(SyntaxKind.ConstKeyword))
                {
                    this.TryGetNativeScalarStorage(node.Declaration.Type, out nativeStorage);
                    foreach (var cast in node.Declaration.DescendantNodes().OfType<CastExpressionSyntax>())
                        if (this.TryGetNativeScalarStorage(cast.Type, out string storage))
                            nativeCasts[cast.Type.ToString()] = storage;
                }

                this.GetRemapInfo(fullName, node.AttributeLists, out var listAttributes, node.Declaration.Type.ToString(), out string newType, out string newName);
                newType ??= nativeStorage;
                string headerType = this.GetHeaderEnumType(node.AttributeLists, node.Declaration.Type.ToString(), fullName);
                if (headerType != null)
                {
                    newType = headerType;
                    RemoveHeaderAssociation(listAttributes);
                }

                // ClangSharp mistakenly emits string[] for WCHAR[] Foo = "Bar".
                // Change it to string
                if (newType == null && node.Declaration.Type.ToString() == "string[]")
                {
                    newType = "string";
                }

                // We will turn static strings into constants
                if (node.Modifiers.ToString() == "public static" && node.Declaration.Type.ToString() == "string")
                {
                    var newModifiers =
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space),
                            SyntaxFactory.Token(SyntaxKind.ConstKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                    node = node.WithModifiers(newModifiers).WithLeadingTrivia(node.GetLeadingTrivia());

                    return node;
                }

                if (node.Modifiers.ToString() == "public static readonly")
                {
                    // Turn public static readonly Guids into string constants with an attribute
                    // to signal language projections to turn them into Guid constants. Guid constants 
                    // aren't allowed in metadata, requiring us to surface them this way
                    if (node.Declaration.Type.ToString() == "Guid")
                    {
                        var varInitializer = node.Declaration.Variables.First().Initializer;
                        if (varInitializer.Value is ObjectCreationExpressionSyntax objCreationSyntax)
                        {
                            node = node.RemoveNode(varInitializer, SyntaxRemoveOptions.KeepExteriorTrivia | SyntaxRemoveOptions.KeepEndOfLine);

                            var args = objCreationSyntax.ArgumentList.Arguments;
                            if (args.Count == 11)
                            {
                                var allArgs = $"(uint){args}";

                                string argsFormatted = $"({allArgs})";
                                var attrsList =
                                    SyntaxFactory.AttributeList(
                                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                            SyntaxFactory.Attribute(
                                                SyntaxFactory.ParseName(EncodeHelpers.GuidAttributeName),
                                                SyntaxFactory.ParseAttributeArgumentList(argsFormatted))));

                                node = node.AddAttributeLists(attrsList).WithLeadingTrivia(node.GetLeadingTrivia());
                            }
                            else if (objCreationSyntax.ArgumentList.Arguments.Count == 1)
                            {
                                var textValue = EncodeHelpers.RemoveQuotes(objCreationSyntax.ArgumentList.Arguments[0].ToString());

                                // If this is an invalid format, remove the node
                                if (!Guid.TryParse(textValue, out var guidVal))
                                {
                                    return null;
                                }

                                node = node.AddAttributeLists(EncodeHelpers.ConvertGuidToAttributeList(guidVal).WithLeadingTrivia(node.GetLeadingTrivia()));
                            }
                        }

                        return node;
                    }
                    // Some constants get scraped as public static readonly.
                    // If we see the prefix, force it to be a const
                    else if (localName.StartsWith(ForceConstPrefix))
                    {
                        var newModifiers =
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space),
                                SyntaxFactory.Token(SyntaxKind.ConstKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                        node = node.WithModifiers(newModifiers).WithLeadingTrivia(node.GetLeadingTrivia());

                        var variable = node.Declaration.Variables.First();
                        variable = variable.WithIdentifier(SyntaxFactory.ParseToken(localName.Substring(ForceConstPrefix.Length)));
                        node = node.WithDeclaration(node.Declaration.WithVariables(SyntaxFactory.SingletonSeparatedList(variable)));

                        return node;
                    }
                }

                // Assume [0] or [1] (ANYSIZE_ARRAY) at the end of a struct indicates flexible arrays and add the FlexibleArray attribute.
                // Parse NativeTypeName instead of the field declaration to support scenarios like arrays of structs which aren't scraped as arrays.
                var nativeTypeName = SyntaxUtils.GetNativeTypeNameFromAttributesLists(node.AttributeLists);
                if (nativeTypeName != null)
                {
                    var match = NativeTypeNameArrayRegex.Match(nativeTypeName);
                    if (match.Success && match.Groups[1].Value == "0" || match.Groups[1].Value == "1")
                    {
                        if (node.ToString() == (node.Parent as StructDeclarationSyntax).Members.Where(m => m is FieldDeclarationSyntax).Last().ToString())
                        {
                            var attributeList = SyntaxFactory.AttributeList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                        SyntaxFactory.Attribute(
                                            SyntaxFactory.ParseName("FlexibleArray"))));

                            node = node.AddAttributeLists(attributeList);
                        }
                    }
                }

                node = (FieldDeclarationSyntax)base.VisitFieldDeclaration(node);
                node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes));

                // Compile constants in native storage while retaining their native type attributes for WinMD emission.
                if (nativeCasts.Count != 0)
                    node = node.WithDeclaration(node.Declaration.ReplaceNodes(
                        node.Declaration.DescendantNodes().OfType<CastExpressionSyntax>()
                            .Where(cast => nativeCasts.ContainsKey(cast.Type.ToString())),
                        (original, rewritten) => rewritten.WithType(SyntaxFactory.ParseTypeName(nativeCasts[original.Type.ToString()]))));

                var firstVar = node.Declaration.Variables.First();

                if (newName != null)
                {
                    var newVar = SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(newName));
                    node = node.ReplaceNode(firstVar, newVar);
                }

                if (newType != null)
                {
                    node = node.WithDeclaration(node.Declaration.WithType(SyntaxFactory.ParseTypeName(newType).WithTrailingTrivia(SyntaxFactory.Space)));
                }

                return node;
            }

            private bool TryGetNativeScalarStorage(TypeSyntax type, out string storage)
            {
                if (type is PredefinedTypeSyntax || this.nativeTypedefs.Count == 0)
                {
                    storage = null;
                    return false;
                }
                string name = type.ToString().Replace("global::", "", StringComparison.Ordinal);
                bool found = this.nativeTypedefs.TryGetValue(name, out storage) ||
                    this.nativeTypedefs.TryGetValue(SyntaxUtils.GetEnclosingNamespace(type) + "." + name, out storage) ||
                    this.requiredNamespaces.TryGetValue(name, out string owner) &&
                        this.nativeTypedefs.TryGetValue(owner + "." + name, out storage);
                if (!found)
                {
                    var matches = this.usingNamespaces.Select(ns => this.nativeTypedefs.GetValueOrDefault(ns + "." + name))
                        .Where(value => value != null).ToArray();
                    if (matches.Length > 1)
                        throw new FormatException($"{type.GetLocation().GetMappedLineSpan()}: Ambiguous native typedef constant type '{name}'.");
                    if (matches.Length == 1) storage = matches[0];
                }
                if (storage is "sbyte" or "byte" or "short" or "ushort" or "int" or "uint" or "long" or "ulong")
                    return true;
                storage = null;
                return false;
            }

            public override SyntaxNode VisitAttributeList(AttributeListSyntax node)
            {
                var firstAttr = node.Attributes[0];
                var attrName = firstAttr.Name.ToString();
                if (attrName is "NativeAnnotation" or "SupportedOSPlatform")
                {
                    var headerOS = node.Parent.ChildNodes().OfType<AttributeListSyntax>()
                        .SelectMany(list => list.Attributes)
                        .Where(attribute => attribute.Name.ToString() == "NativeAnnotation")
                        .Select(attribute => (Attribute: attribute,
                            Payload: (attribute.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token.ValueText))
                        .Where(pair => pair.Payload != null && pair.Payload.StartsWith(SupportedOSAnnotation.Prefix, StringComparison.Ordinal))
                        .ToArray();
                    if (headerOS.Length != 0)
                    {
                        string version = SupportedOSAnnotation.Parse(headerOS[0].Payload);
                        if (headerOS.Any(pair => SupportedOSAnnotation.Parse(pair.Payload) != version))
                            throw new FormatException("Conflicting native SupportedOSPlatform annotations.");
                        if (attrName == "SupportedOSPlatform")
                        {
                            if (firstAttr.ArgumentList?.Arguments.Count != 1 ||
                                firstAttr.ArgumentList.Arguments[0].Expression is not LiteralExpressionSyntax legacy ||
                                legacy.Token.ValueText != version)
                                throw new FormatException("Legacy SupportedOSPlatform conflicts with native availability.");
                            return null;
                        }
                        if (headerOS.Skip(1).Any(pair => pair.Attribute == firstAttr))
                            return null;
                    }
                }

                switch (attrName)
                {
                    // We don't want more than one of these on an interface. Get rid of it if the 
                    // struct doesn't have a NativeTypeName, which means it's a forward declaration.
                    // The real declaration will have the SupportedOSPlatform that we'll keep 
                    case "SupportedOSPlatform":
                    {
                        if (node.Parent is StructDeclarationSyntax structDeclaration)
                        {
                            if (!structDeclaration.AttributeLists.Any(list => list.Attributes.Any(attr => attr.Name.ToString() == "NativeTypeName")))
                            {
                                return null;
                            }
                        }

                        // We don't want these for parameters or fields
                        if (node.Parent is ParameterSyntax || node.Parent is FieldDeclarationSyntax)
                        {
                            return null;
                        }

                        // We don't want these for interface methods
                        if (node.Parent is MethodDeclarationSyntax && node.Parent.Parent is StructDeclarationSyntax)
                        {
                            return null;
                        }

                        break;
                    }

                    case "Guid":
                    {
                        return this.ProcessGuidAttr(firstAttr);
                    }

                    case "UnmanagedFunctionPointer":
                    {
                        // ClangSharp can emit this attribute with no arguments.
                        // The typedef we're using of this attribute has no such ctor,
                        // so emit one that does, using WinApi as the default calling convention.
                        // Also, convert StdCall to Winapi
                        if (firstAttr.ArgumentList == null || firstAttr.ArgumentList.ToString() == "(CallingConvention.StdCall)")
                        {
                            return
                                SyntaxFactory.AttributeList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                        SyntaxFactory.Attribute(
                                            SyntaxFactory.ParseName("UnmanagedFunctionPointer"),
                                            SyntaxFactory.ParseAttributeArgumentList("(CallingConvention.Winapi)"))));
                        }

                        break;
                    }

                    case "NativeTypeName":
                    {
                        bool alreadyConst = 
                            node.Parent is FieldDeclarationSyntax parentField &&
                            parentField.Modifiers.Any(m => m.ToString() == "const");

                        var ret = this.ProcessNativeTypeNameAttr(alreadyConst, firstAttr);

                        return ret == null ? node : ret;
                    }

                    case "NativeAnnotation":
                    {
                        var payload = (firstAttr.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token.ValueText;
                        if (payload != null && payload.StartsWith(ParameterAnnotation.Owner, StringComparison.Ordinal))
                        {
                            if (payload == ParameterAnnotation.Owner + "SetLastError") return null;
                            if (payload.StartsWith(ParameterAnnotation.Owner + "ImportLibrary(", StringComparison.Ordinal)) return null;
                            // Typedef facts are resolved on the compiler-owned nominal declaration, not propagated uses.
                            if (payload.StartsWith(ParameterAnnotation.Owner + "AlsoUsableFor(", StringComparison.Ordinal)) return null;
                            if (payload == ParameterAnnotation.Owner + "PreserveResult")
                            {
                                if (node.Parent is not MethodDeclarationSyntax &&
                                    node.Parent is not DelegateDeclarationSyntax)
                                    throw new FormatException($"PreserveResult requires a generated method: {payload}");
                                // C# delegates have no method attribute target. Keep the existing compiler
                                // carrier until the emitter writes Invoke, rather than annotating the typedef.
                                return SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                                    node.Parent is DelegateDeclarationSyntax ?
                                        SyntaxFactory.Attribute(SyntaxFactory.ParseName("CppAttributeList"),
                                            SyntaxFactory.ParseAttributeArgumentList("(\"Win32Metadata.PreserveResult\")")) :
                                        SyntaxFactory.Attribute(SyntaxFactory.ParseName("PreserveSig"))));
                            }
                            if (payload.StartsWith(SupportedOSAnnotation.Prefix, StringComparison.Ordinal))
                            {
                                string version = SupportedOSAnnotation.Parse(payload);
                                return SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.Attribute(SyntaxFactory.ParseName("SupportedOSPlatform"),
                                        SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
                                            SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(version))))))));
                            }
                            if (payload.StartsWith(ProducerAnnotation.Marker, StringComparison.Ordinal))
                            {
                                if (node.Parent is not ParameterSyntax && node.Parent is not MethodDeclarationSyntax &&
                                    node.Parent is not DelegateDeclarationSyntax)
                                    throw new FormatException($"Owned producer annotation on an invalid generated target: {payload}");
                                if (payload == ProducerAnnotation.Marker)
                                {
                                    bool resolved = node.Parent.ChildNodes().OfType<AttributeListSyntax>()
                                        .SelectMany(list => list.Attributes).Any(attribute =>
                                            attribute.Name.ToString() == "NativeAnnotation" &&
                                            (attribute.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?
                                                .Token.ValueText.StartsWith(ProducerAnnotation.Marker + "(", StringComparison.Ordinal) == true);
                                    if (!resolved)
                                        throw new FormatException("RAIIFree reached the emitter without compiler-resolved producer facts.");
                                    return null;
                                }
                                var producer = ProducerAnnotation.Parse(payload);
                                var facts = new List<AttributeSyntax>
                                {
                                    SyntaxFactory.Attribute(SyntaxFactory.ParseName("RAIIFree"),
                                        SyntaxFactory.ParseAttributeArgumentList("(\"" + producer.Cleanup + "\")")),
                                };
                                facts.AddRange(producer.InvalidValues.Select(value =>
                                    SyntaxFactory.Attribute(SyntaxFactory.ParseName("InvalidHandleValue"),
                                        SyntaxFactory.ParseAttributeArgumentList("(" +
                                            value.ToString(System.Globalization.CultureInfo.InvariantCulture) + "L)"))));
                                var producerAttributes = SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(facts));
                                return node.Parent is ParameterSyntax ? producerAttributes :
                                    producerAttributes.WithTarget(SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Token(SyntaxKind.ReturnKeyword)));
                            }
                            var annotation = ParameterAnnotation.Parse(payload);
                            if (annotation.Kind == ParameterAnnotationKind.AssociatedEnum && this.headerEnumNames.Contains(annotation.EnumName))
                                return null;
                            if (node.Parent is not ParameterSyntax &&
                                !(annotation.Kind == ParameterAnnotationKind.AssociatedEnum &&
                                (node.Parent is MethodDeclarationSyntax || node.Parent is FieldDeclarationSyntax)))
                                throw new FormatException($"Owned parameter annotation on an invalid generated target: {payload}");
                            var args = annotation.EnumName == null ? null :
                                SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(annotation.EnumName)))));
                            var result = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Attribute(SyntaxFactory.ParseName(annotation.AttributeName), args)));
                            return node.Parent is MethodDeclarationSyntax ?
                                result.WithTarget(SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Token(SyntaxKind.ReturnKeyword))) : result;
                        }
                        return null;
                    }

                    case "CppAttributeList":
                    {
                        return this.CreateAttributeListForSal(node);
                    }
                }

                return base.VisitAttributeList(node);
            }

            public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
            {
                node = (EnumDeclarationSyntax)base.VisitEnumDeclaration(node);

                var enumName = node.Identifier.ValueText;
                if (this.GetRemapInfo(enumName, node.AttributeLists, out var listAttributes, null, out _, out _))
                {
                    node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes));
                }

                if (this.enumAdditions.TryGetValue(enumName, out var additionsList))
                {
                    List<EnumMemberDeclarationSyntax> newMembers = new List<EnumMemberDeclarationSyntax>();
                    foreach (var additionPair in additionsList)
                    {
                        var member =
                            SyntaxFactory.EnumMemberDeclaration(additionPair.Key)
                                .WithEqualsValue(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ParseExpression(additionPair.Value)));
                        newMembers.Add(member);
                    }

                    node = node.AddMembers(newMembers.ToArray());
                }

                if (this.enumsToMakeFlags.Contains(enumName))
                {
                    bool hasFlags = SyntaxUtils.GetAttribute(node.AttributeLists, "Flags") != null;
                    if (!hasFlags)
                    {
                        node =
                            node.AddAttributeLists(
                                SyntaxFactory.AttributeList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                        SyntaxFactory.Attribute(SyntaxFactory.ParseName("global::System.Flags")))).WithLeadingTrivia(node.GetLeadingTrivia()));
                    }
                }

                return node;
            }

            public override SyntaxNode VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
            {
                string fullName = GetFullNameWithoutArchSuffix(node);

                this.GetRemapInfo(fullName, node.AttributeLists, out var listAttributes, string.Empty, out string newType, out string newName);

                node = (EnumMemberDeclarationSyntax)base.VisitEnumMemberDeclaration(node);
                node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes));

                return node;
            }

            public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
            {
                string name = node.Identifier.Text;
                string fullName = SyntaxUtils.GetFullName(node);
                string fixedName = GetFullNameWithoutArchSuffix(node);

                // Remove duplicate delegates in this tree
                if (this.visitedDelegateNames.Contains(fullName))
                {
                    return null;
                }

                this.visitedDelegateNames.Add(fullName);

                // Add Ansi or Unicode attributes to -A/-W APIs.
                if ((name.EndsWith("A") && this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}W")) ||
                   (!name.EndsWith("W") && this.apiNamesToNamespaces.ContainsKey($"{name}W")))
                {
                    var attributeList = SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.ParseName("Ansi"))));

                    node = node.AddAttributeLists(attributeList);
                }
                else if (name.EndsWith("W") && (this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}A") || this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}")))
                {
                    var attributeList = SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.ParseName("Unicode"))));

                    node = node.AddAttributeLists(attributeList);
                }

                string returnFullName = $"{fixedName}::return";

                if (this.GetRemapInfo(returnFullName, node.AttributeLists, out var listAttributes, node.ReturnType.ToString(), out var newType, out _))
                {
                    node = (DelegateDeclarationSyntax)base.VisitDelegateDeclaration(node);

                    var target = SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Token(SyntaxKind.ReturnKeyword));
                    node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes, target));

                    if (newType != null)
                    {
                        node = node.WithReturnType(SyntaxFactory.ParseTypeName(newType).WithTrailingTrivia(SyntaxFactory.Space));
                    }

                    return node;
                }

                return base.VisitDelegateDeclaration(node);
            }

            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var dllImportAttr = SyntaxUtils.GetAttribute(node.AttributeLists, "DllImport");
                string dllName = dllImportAttr?.ArgumentList.Arguments[0].ToString();

                // Trim the leading and trailing quotes.
                dllName = dllName?.Substring(1, dllName.Length - 2);

                // Skip methods where we weren't given an import lib name. Should we warn the caller?
                if (dllImportAttr != null && string.IsNullOrEmpty(dllName))
                {
                    return null;
                }

                string name = node.Identifier.Text;
                string fullName = SyntaxUtils.GetFullName(node);
                string fixedFullName = GetFullNameWithoutArchSuffix(node);

                // Remove duplicate static methods
                if (node.Body == null)
                {
                    // If this function is supposed to be in a certain namespace, remove it if it's not.
                    // We only respect this for static methods
                    if (this.requiredNamespaces.TryGetValue(fixedFullName, out var requiredNamespace))
                    {
                        var ns = GetEnclosingNamespace(node);
                        if (ns != requiredNamespace)
                        {
                            return null;
                        }
                    }

                    // Remove duplicate methods in this tree
                    if (this.visitedStaticMethodNames.Contains(fullName))
                    {
                        return null;
                    }

                    this.visitedStaticMethodNames.Add(fullName);
                }
                // Any method with a body has to be part of a call to a vtable for an interface.
                // If it's not, get rid of it
                else if (!node.Body.ToString().Contains("GetDelegateForFunctionPointer"))
                {
                    return null;
                }

                var isInterface = node.Parent is StructDeclarationSyntax;
                var ownedReturnNames = GetOwnedAnnotationNames(node.AttributeLists);
                string headerReturnType = this.GetHeaderEnumType(node.AttributeLists, node.ReturnType.ToString(), fixedFullName + "::return");
                node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);

                // Add Ansi or Unicode attributes to -A/-W APIs.
                if ((name.EndsWith("A") && this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}W")) ||
                   (!name.EndsWith("W") && this.apiNamesToNamespaces.ContainsKey($"{name}W") && !isInterface))
                {
                    var attributeList = SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.ParseName("Ansi"))));

                    node = node.AddAttributeLists(attributeList);
                }
                else if (name.EndsWith("W") && (this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}A") || this.apiNamesToNamespaces.ContainsKey($"{name[0..^1]}")))
                {
                    var attributeList = SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.ParseName("Unicode"))));

                    node = node.AddAttributeLists(attributeList);
                }

                // Add the StaticLibrary attribute if one was specified for this DLL.
                if (!string.IsNullOrEmpty(dllName) && this.staticLibs.TryGetValue(dllName, out string staticLib))
                {
                    var attrListNode = SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(SyntaxFactory.ParseName("StaticLibrary"),
                            SyntaxFactory.ParseAttributeArgumentList($"(\"{staticLib}\")"))));

                    node = node.WithAttributeLists(node.AttributeLists.Add(attrListNode));
                }

                // See if there are any attributes directly on the method
                if (this.GetRemapInfo(fullName, node.AttributeLists, out var methodAttributes, null, out _, out _))
                {
                    node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, methodAttributes));
                }

                string returnFullName = $"{fixedFullName}::return";
                string nodeReturnType = node.ReturnType.ToString();

                // Find remap info for the return parameter for this method and apply any that we find
                if (this.GetRemapInfo(returnFullName, node.AttributeLists, out var listAttributes, nodeReturnType, out var newType, out _))
                {
                    VerifyOwnedRemaps(returnFullName, ownedReturnNames, node.AttributeLists, listAttributes);
                    if (headerReturnType != null) RemoveHeaderAssociation(listAttributes);
                    var target = SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Token(SyntaxKind.ReturnKeyword));
                    node = node.WithAttributeLists(FixRemappedAttributes(node.AttributeLists, listAttributes, target));

                    if (newType != null)
                    {
                        node = node.WithReturnType(SyntaxFactory.ParseTypeName(newType).WithTrailingTrivia(SyntaxFactory.Space));
                    }
                }

                if (headerReturnType != null)
                    node = node.WithReturnType(SyntaxFactory.ParseTypeName(headerReturnType).WithTrailingTrivia(SyntaxFactory.Space));
                return node;
            }

            private static string GetEnclosingNamespace(SyntaxNode node)
            {
                for (SyntaxNode currentNode = node; node != null; node = node.Parent)
                {
                    if (node is NamespaceDeclarationSyntax nsNode)
                    {
                        return nsNode.Name.ToString();
                    }
                }

                return null;
            }

            private static string GetFullNameWithoutArchSuffix(SyntaxNode node)
            {
                string ret = SyntaxUtils.GetFullName(node);

                ret = ret.Replace("____1", string.Empty);
                ret = ret.Replace("____2", string.Empty);

                return ret;
            }

            private string GetInfoForNativeType(string nativeTypeName, out bool isConst, out bool isNullTerminated, out bool isNullNullTerminated)
            {
                string metadataType = null;
                isConst = false;
                isNullTerminated = false;
                isNullNullTerminated = false;

                if (this.remaps.TryGetValue(nativeTypeName, out var attrs))
                {
                    if (attrs.Contains("[Const]"))
                    {
                        isConst = true;
                    }
                }

                switch (nativeTypeName)
                {
                    case "LPCGUID":
                    case "LPCVOID":
                        isConst = true;
                        break;

                    case "PCHAR":
                    case "LPCH":
                    case "PCH":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        break;

                    case "LPCCH":
                    case "PCCH":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        isConst = true;
                        break;

                    case "NPSTR":
                    case "LPSTR":
                    case "PSTR":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        isNullTerminated = true;
                        break;

                    case "LPCSTR":
                    case "PCSTR":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        isConst = true;
                        isNullTerminated = true;
                        break;

                    case "PZZSTR":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        isNullNullTerminated = true;
                        break;

                    case "CPZZSTR":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        isConst = true;
                        isNullNullTerminated = true;
                        break;

                    case "PWCHAR":
                    case "LPWCH":
                    case "PWCH":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32StringType;
                        break;

                    case "NWPSTR":
                    case "LPWSTR":
                    case "PWSTR":
                    case "LPOLESTR":
                    case "PNZWCH":
                    case "WCHAR *":
                    case "OLECHAR *":
                    case "wchar_t *":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32WideStringType;
                        isNullTerminated = true;
                        break;

                    case "LPCWSTR":
                    case "PCWSTR":
                    case "LPCWCH":
                    case "PCNZWCH":
                    case "LPCOLESTR":
                    case "const OLECHAR *":
                    case "const WCHAR *":
                    case "const wchar_t *":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32WideStringType;
                        isNullTerminated = true;
                        isConst = true;
                        break;

                    case "PZZWSTR":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32WideStringType;
                        isNullNullTerminated = true;
                        break;

                    case "PCZZWSTR":
                        metadataType = ClangSharpSourceWinmdGenerator.Win32WideStringType;
                        isConst = true;
                        isNullNullTerminated = true;
                        break;

                    default:
                        if (nativeTypeName.StartsWith("const ") || Regex.IsMatch(nativeTypeName, "^L?PCW?STR"))
                        {
                            isConst = true;
                        }

                        break;
                }

                return metadataType;
            }

            private void AddNativeArrayInfoAttribute(bool alreadyConst, string nativeTypeName, List<AttributeSyntax> attrsList)
            {
                if (string.IsNullOrWhiteSpace(nativeTypeName))
                {
                    return;
                }

                var metadataType = this.GetInfoForNativeType(nativeTypeName, out bool isConst, out bool isNullTerminated, out bool isNullNullTerminated);

                if (isConst && !alreadyConst)
                {
                    attrsList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("Const")));
                }

                if (metadataType == ClangSharpSourceWinmdGenerator.Win32StringType || metadataType == ClangSharpSourceWinmdGenerator.Win32WideStringType)
                {
                    if (!isNullTerminated)
                    {
                        attrsList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("NotNullTerminated")));
                    }

                    if (isNullNullTerminated)
                    {
                        attrsList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("NullNullTerminated")));
                    }
                }
            }

            private SyntaxNode ProcessGuidAttr(AttributeSyntax guidAttr)
            {
                string guidStr = guidAttr.ArgumentList.Arguments[0].ToString();
                guidStr = EncodeHelpers.RemoveQuotes(guidStr);

                Guid guid = Guid.Parse(guidStr);

                return EncodeHelpers.ConvertGuidToAttributeList(guid);
            }

            private SyntaxNode ProcessNativeTypeNameAttr(bool alreadyConst, AttributeSyntax nativeTypeNameAttr)
            {
                string nativeType = nativeTypeNameAttr.ArgumentList.Arguments[0].ToString();
                nativeType = EncodeHelpers.RemoveQuotes(nativeType);

                List<AttributeSyntax> attributeNodes = new List<AttributeSyntax>();

                this.AddNativeArrayInfoAttribute(alreadyConst, nativeType, attributeNodes);
                attributeNodes.Insert(0, nativeTypeNameAttr);

                var ret = SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(attributeNodes));
                if (((AttributeListSyntax)nativeTypeNameAttr.Parent).Target is AttributeTargetSpecifierSyntax target
                    && target.Identifier.ValueText == "return")
                {
                    ret =
                        ret.WithTarget(
                            SyntaxFactory.AttributeTargetSpecifier(
                                SyntaxFactory.Token(SyntaxKind.ReturnKeyword)));
                }

                return ret;
            }

            private SyntaxNode CreateAttributeListForSal(AttributeListSyntax cppAttrList)
            {
                if (cppAttrList.Parent is not ParameterSyntax paramNode)
                {
                    return null;
                }

                bool marshalAsAdded = this.nodesWithMarshalAs.Contains(paramNode);

                AttributeSyntax cppAttr = cppAttrList.Attributes[0];
                List<AttributeSyntax> attributesList = new List<AttributeSyntax>();

                string salText = cppAttr.ArgumentList.Arguments[0].ToString();
                salText = salText.Substring(1, salText.Length - 2);

                string nativeArrayInfoParams = null;
                bool isIn = false;
                bool isOut = false;
                bool isOpt = false;
                bool isReserved = false;
                bool isComOutPtr = false;
                bool isRetVal = false;
                bool isNullNullTerminated;
                bool isByteBuffer = false;
                bool? pre = null;
                bool? post = null;

                var salAttrs = GetSalAttributes(salText);
                isNullNullTerminated = salAttrs.Any(a => a.Name == "SAL_name" && a.P1 == "_NullNull_terminated_");

                foreach (var salAttr in salAttrs)
                {
                    if (salAttr.Name == "SAL_name")
                    {
                        if (salAttr.P1.StartsWith("__RPC__"))
                        {
                            // TODO: Handle ecount, xcount and others that deal with counts
                            bool deref = false;

                            string[] parts = salAttr.P1.Split('_');
                            foreach (var part in parts)
                            {
                                switch (part)
                                {
                                    case "deref":
                                        deref = true;
                                        break;

                                    case "in":
                                        isIn = true;
                                        break;

                                    case "out":
                                        isOut = true;
                                        break;

                                    case "inout":
                                        isIn = isOut = true;
                                        break;

                                    case "opt":
                                        // As per rpcsal.h, deref_opt means input optional but deref_***_opt doesn't
                                        if (deref && (isIn || isOut))
                                        {
                                            continue;
                                        }

                                        isOpt = true;
                                        break;
                                }
                            }

                            break;
                        }
                        else if (salAttr.P1 == "_Maybenull_" || salAttr.P1 == "_Pre_maybenull_" || salAttr.P1 == "_Pre_null_")
                        {
                            isIn = true;
                            isOpt = true;

                            continue;
                        }

                        if (salAttr.P1.Contains("_opt"))
                        {
                            isOpt = true;
                        }

                        if (salAttr.P1.StartsWith("_COM_Outptr"))
                        {
                            isComOutPtr = true;
                            continue;
                        }
                        else if (salAttr.P1.StartsWith("_Outptr_") && !isComOutPtr)
                        {
                            isOut = true;
                            isByteBuffer = salAttr.P1.Contains("bytebuffer");
                            continue;
                        }
                        else if (salAttr.P1 == "_Reserved_")
                        {
                            isReserved = true;
                            continue;
                        }
                    }

                    if (salAttr.Name == "SAL_retval")
                    {
                        isRetVal = true;
                        continue;
                    }

                    if (salAttr.Name == "SAL_pre")
                    {
                        pre = true;
                        continue;
                    }

                    if (salAttr.Name == "SAL_post")
                    {
                        pre = false;
                        post = true;
                        continue;
                    }

                    if (salAttr.Name == "SAL_end")
                    {
                        pre = post = false;
                    }

                    if (salAttr.Name == "SAL_valid")
                    {
                        if (pre.HasValue && pre.Value)
                        {
                            isIn = true;
                        }
                        else if (post.HasValue && post.Value)
                        {
                            isOut = true;
                        }
                        else
                        {
                            isIn = isOut = true;
                        }

                        continue;
                    }

                    if (!marshalAsAdded && (salAttr.Name == "SAL_writableTo" || salAttr.Name == "SAL_readableTo") && pre.HasValue && pre.Value)
                    {
                        nativeArrayInfoParams = GetArrayMarshalAsFromP1(paramNode, salAttr.P1);
                        if (!string.IsNullOrEmpty(nativeArrayInfoParams))
                        {
                            marshalAsAdded = true;
                        }

                        continue;
                    }
                }

                // If we didn't add marshal as yet, try again without using pre
                if (!marshalAsAdded && !isByteBuffer)
                {
                    var salAttr = salAttrs.FirstOrDefault(attr => attr.Name == "SAL_writableTo" || attr.Name == "SAL_readableTo");
                    if (salAttr != null)
                    {
                        nativeArrayInfoParams = GetArrayMarshalAsFromP1(paramNode, salAttr.P1);
                        if (!string.IsNullOrEmpty(nativeArrayInfoParams))
                        {
                            marshalAsAdded = true;
                        }
                    }
                }

                var nativeTypeName = SyntaxUtils.GetNativeTypeNameFromAttributesLists((cppAttrList.Parent as ParameterSyntax).AttributeLists);
                if (!string.IsNullOrEmpty(nativeArrayInfoParams))
                {
                    var attrName = SyntaxFactory.ParseName(nativeArrayInfoParams.Contains("BytesParamIndex") ? "MemorySize" : "NativeArrayInfo");
                    var args = SyntaxFactory.ParseAttributeArgumentList(nativeArrayInfoParams.ToString());
                    var finalAttr = SyntaxFactory.Attribute(attrName, args);
                    attributesList.Add(finalAttr);
                }
                else if (nativeTypeName != null)
                {
                    var match = NativeTypeNameArrayRegex.Match(nativeTypeName);
                    if (match.Success)
                    {
                        var size = Convert.ToUInt32(match.Groups[1].Value);

                        // Don't bother marking this as an array if it has 1 or less
                        if (size > 1)
                        {
                            var attrName = SyntaxFactory.ParseName("NativeArrayInfo");
                            var args = SyntaxFactory.ParseAttributeArgumentList($"(CountConst = {size})");
                            attributesList.Add(SyntaxFactory.Attribute(attrName, args));
                        }
                    }
                }

                if (isIn)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("In")));
                }

                if (isComOutPtr)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("ComOutPtr")));
                }
                else if (isOut)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("Out")));
                }

                if (isReserved)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("Reserved")));
                }

                if (isOpt || isReserved)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("Optional")));
                }

                if (isNullNullTerminated)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("NullNullTerminated")));
                }

                if (isRetVal)
                {
                    attributesList.Add(SyntaxFactory.Attribute(SyntaxFactory.ParseName("RetVal")));
                }

                if (attributesList.Count == 0)
                {
                    return null;
                }

                return SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(attributesList));

                string GetArrayMarshalAsFromP1(ParameterSyntax paramNode, string p1Text)
                {
                    ParameterListSyntax parameterListNode = (ParameterListSyntax)paramNode.Parent;
                    var match = ElementCountRegex.Match(p1Text);
                    StringBuilder ret = new StringBuilder("(");

                    if (match.Success)
                    {
                        string sizeOrParamName = match.Groups[1].Value;
                        if (int.TryParse(sizeOrParamName, out int size))
                        {
                            // Don't bother marking this as an array if it has 1 or less
                            if (size <= 1)
                            {
                                return string.Empty;
                            }

                            if (ret.Length != 1)
                            {
                                ret.Append(", ");
                            }

                            ret.Append($"CountConst = {size}");
                        }
                        else
                        {
                            sizeOrParamName = sizeOrParamName.Replace("*", string.Empty);
                            for (int i = 0; i < parameterListNode.Parameters.Count; i++)
                            {
                                if (parameterListNode.Parameters[i].Identifier.ValueText == sizeOrParamName)
                                {
                                    if (ret.Length != 1)
                                    {
                                        ret.Append(", ");
                                    }

                                    string propName = p1Text.StartsWith("elementCount") ? "CountParamIndex" : "BytesParamIndex";
                                    ret.Append($"{propName} = {i}");
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        // If it didn't match the regex and we don't see inexpressibleCount, we can't do 
                        // anything but return an empty string, because we don't know how to interpret it
                        if (!p1Text.StartsWith("inexpressibleCount"))
                        {
                            ret = new StringBuilder();
                        }
                    }

                    if (ret.Length > 1)
                    {
                        ret.Append(')');
                        return ret.ToString();
                    }

                    return string.Empty;

                }

                IEnumerable<SalAttribute> GetSalAttributes(string salArgsText)
                {
                    foreach (var attr in salArgsText.Split('^'))
                    {
                        // Owned annotations are also transported individually as NativeAnnotation.
                        if (!attr.StartsWith("Name=SAL_", StringComparison.Ordinal)) continue;
                        var salAttr = SalAttribute.CreateFromCppAttribute(attr);
                        if (salAttr != null)
                        {
                            yield return salAttr;
                        }
                    }
                }
            }

            private string GetRemapData(string fullName)
            {
                if (string.IsNullOrEmpty(fullName))
                {
                    return null;
                }

                string remapData = null;

                if (this.remaps.TryGetValue(fullName, out var remap))
                {
                    remapData += remap;
                }

                foreach (var pair in this.regexRemaps)
                {
                    if (pair.Key.IsMatch(fullName))
                    {
                        remapData += pair.Value;
                    }
                }

                return remapData;
            }

            private bool GetRemapInfo(string fullName, SyntaxList<AttributeListSyntax> existingAttrList, out List<AttributeSyntax> listAttributes, string currentType, out string newType, out string newName)
            {
                listAttributes = null;
                newType = null;
                newName = null;

                bool ret = false;
                string remapData = this.GetRemapData(fullName);
                if (!string.IsNullOrEmpty(remapData))
                {
                    ret = EncodeHelpers.DecodeRemap(remapData, out listAttributes, out newType, out newName);
                    if (newType != null)
                    {
                        // Try to keep the pointers at the same level if we're replacing
                        // a uint or int. The mismatch can happen in the auto-generated
                        // enum remaps which don't know if the params/fields getting replaced
                        // are pointers or not
                        if (currentType != null && (currentType.StartsWith("uint") || currentType.StartsWith("int") || currentType.StartsWith("short") || currentType.StartsWith("ushort")))
                        {
                            int starIndex = currentType.IndexOf('*');
                            if (starIndex != -1 && newType.IndexOf('*') == -1)
                            {
                                newType += currentType.Substring(starIndex);
                            }
                        }

                        // Add NativeTypeName attribute if it is missing 
                        bool hasNativeTypeName = false;

                        foreach (var attrList in existingAttrList.ToArray())
                        {
                            foreach (var attr in attrList.Attributes)
                            {
                                var attrName = attr.Name.ToString();
                                if (attrName == "NativeTypeName")
                                {
                                    hasNativeTypeName = true;
                                }
                            }
                        }

                        if (!hasNativeTypeName && !(currentType.Contains("IUnknown**")) && !(currentType.Contains("IInspectable**")))
                        {
                            var newCurrentType = "(\"" + currentType + "\")";
                            newCurrentType = newCurrentType.ToUpper();

                            var attrNameNode = SyntaxFactory.ParseName("NativeTypeName");
                            var argsNode = !string.IsNullOrEmpty(currentType) ? SyntaxFactory.ParseAttributeArgumentList(newCurrentType) : null;
                            var newNode = SyntaxFactory.Attribute(attrNameNode, argsNode);

                            if (listAttributes == null)
                            {
                                listAttributes = new List<AttributeSyntax>();
                            }

                            listAttributes.Add(newNode);
                        }
                    }
                }

                if (newType == null && currentType != null)
                {
                    // See if the type ends in the magic suffix. We use a remap fed to clangsharp 
                    // to keep some typedefs from following down to their original type
                    const string RevertToTypeSuffix = "___";

                    EncodeHelpers.SplitType(currentType, out var typeOnly, out var pointers);
                    if (typeOnly.EndsWith(RevertToTypeSuffix))
                    {
                        newType = typeOnly.Substring(0, typeOnly.Length - RevertToTypeSuffix.Length) + pointers;
                        ret = true;
                    }
                }

                return ret;
            }
        }

        private class SalAttribute
        {
            public static SalAttribute CreateFromCppAttribute(string attr)
            {
                SalAttribute ret = new SalAttribute();
                var parts = attr.Split(';');
                foreach (var part in parts)
                {
                    var nameAndValue = part.Split('=');
                    var name = nameAndValue[0].Trim();
                    var value = nameAndValue[1].Replace("\\\"", string.Empty);
                    switch (name)
                    {
                        case "Name":
                            ret.Name = value;
                            break;

                        case "p1":
                            ret.P1 = value;
                            break;

                        case "p2":
                            ret.P2 = value;
                            break;

                        case "p3":
                            ret.P3 = value;
                            break;
                    }
                }

                return ret;
            }

            public string Name { get; private set; }
            public string P1 { get; private set; }
            public string P2 { get; private set; }
            public string P3 { get; private set; }
        }
    }
}
