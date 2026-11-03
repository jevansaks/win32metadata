using System;
using System.Collections.Generic;
using System.Linq;
using ClangSharp;
using ClangSharp.Interop;

namespace Win32MetadataScraper
{
    internal sealed class LastErrorAnnotations
    {
        private const string Owner = "Win32Metadata.";
        public HashSet<string> FunctionNames { get; } = new(StringComparer.Ordinal);

        public void Visit(Decl declaration)
        {
            uint? declarationStart = null;
            foreach (var attribute in declaration.Attrs)
            {
                if (attribute.Kind != CX_AttrKind.CX_AttrKind_Annotate ||
                    !attribute.Spelling.StartsWith(Owner, StringComparison.Ordinal))
                    continue;

                string payload = attribute.Spelling;
                if (payload != Owner + "SetLastError")
                    throw Error(attribute, $"Unknown or malformed owned annotation '{payload}'; this reader supports only valueless Win32Metadata.SetLastError.");
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

        private static uint GetDeclarationStart(FunctionDecl function, Attr annotation)
        {
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
                throw Error(annotation, $"Cannot locate native declaration prefix for '{function.Name}' across files.");
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
                throw Error(annotation, $"Cannot locate native declaration prefix for '{function.Name}'.");
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
