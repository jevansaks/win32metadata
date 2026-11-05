using System;
using System.Collections.Generic;
using ClangSharp;
using ClangSharp.Interop;
using MetadataUtils;

namespace Win32MetadataScraper
{
    internal sealed class SupportedOSScopes
    {
        private const string Begin = "Win32Metadata.SupportedOSBegin:";
        private const string End = "Win32Metadata.SupportedOSEnd";
        private readonly List<StaticAssertDecl> markers = new();
        private readonly List<CXCursor> includes = new();
        private readonly List<CXFile> roots = new();
        private readonly List<(StaticAssertDecl Begin, StaticAssertDecl End, int Instance, string Version)> scopes = new();
        private CXSourceLocation mainStart;
        private TranslationUnit unit;
        private IReadOnlyList<string> arguments;
        private HashSet<CXFile> pchRoots;

        public void Visit(Decl declaration)
        {
            if (declaration is StaticAssertDecl marker && marker.Message != null)
            {
                using var message = marker.Message.Handle.StringLiteralValue;
                string text = message.ToString();
                if (text.StartsWith(Begin, StringComparison.Ordinal) || text == End)
                    this.markers.Add(marker);
            }
        }

        public unsafe void Resolve(TranslationUnit unit, IReadOnlyList<string> arguments)
        {
            if (this.markers.Count == 0) return;
            this.unit = unit;
            this.arguments = arguments;
            using var source = unit.Handle.Spelling;
            this.mainStart = unit.Handle.GetLocationForOffset(unit.Handle.GetFile(source.ToString()), 0);
            unit.TranslationUnitDecl.Handle.VisitChildren((cursor, parent, data) =>
            {
                if (cursor.Kind == CXCursorKind.CXCursor_InclusionDirective)
                    this.includes.Add(cursor);
                return CXChildVisitResult.CXChildVisit_Continue;
            }, default);
            this.markers.Sort((left, right) => Compare(left.Location, right.Location));
            var stacks = new Dictionary<int, Stack<(StaticAssertDecl Marker, string Version)>>();
            foreach (var marker in this.markers)
            {
                int instance = this.IncludeInstance(marker.Location);
                if (!stacks.TryGetValue(instance, out var stack))
                    stacks.Add(instance, stack = new());
                using var literal = marker.Message.Handle.StringLiteralValue;
                string message = literal.ToString();
                if (message.StartsWith(Begin, StringComparison.Ordinal))
                {
                    string version;
                    try { version = SupportedOSAnnotation.Parse(message.Substring(Begin.Length)); }
                    catch (FormatException error) { throw Error(marker, error.Message); }
                    stack.Push((marker, version));
                }
                else
                {
                    if (stack.Count == 0)
                        throw Error(marker, "SupportedOS END has no BEGIN in this physical file and include instance.");
                    var start = stack.Pop();
                    this.scopes.Add((start.Marker, marker, instance, start.Version));
                }
            }
            foreach (var stack in stacks.Values)
                if (stack.Count != 0)
                    throw Error(stack.Peek().Marker, "Unclosed SupportedOS BEGIN in this physical file and include instance.");
        }

        public (string Version, Cursor Source) Default(Decl declaration)
        {
            if (this.scopes.Count == 0) return default;
            declaration.Location.GetExpansionLocation(out var file, out _, out _, out _);
            if (!this.scopes.Exists(scope =>
            {
                scope.Begin.Location.GetExpansionLocation(out var scopeFile, out _, out _, out _);
                return scopeFile.Equals(file);
            })) return default;
            int instance = this.IncludeInstance(declaration.Location);
            (string Version, Cursor Source) result = default;
            foreach (var scope in this.scopes)
                if (scope.Instance == instance && Compare(scope.Begin.Location, declaration.Location) < 0 &&
                    Compare(declaration.Location, scope.End.Location) < 0 &&
                    (result.Source == null || Compare(result.Source.Location, scope.Begin.Location) < 0))
                    result = (scope.Version, scope.Begin);
            return result;
        }

        private int IncludeInstance(CXSourceLocation location)
        {
            location.GetExpansionLocation(out var file, out uint line, out uint column, out _);
            int result = 0;
            for (int i = 0; i < this.includes.Count; i++)
            {
                var include = this.includes[i];
                // SourceManager orders an included file between its include location
                // and the following token in the including file. Keep the actual
                // locations: filename/offset alone conflates repeated includes.
                if (include.IncludedFile.Equals(file) &&
                    Compare(include.Location, location) < 0 && Compare(location, include.Extent.End) < 0 &&
                    (result == 0 || Compare(this.includes[result - 1].Location, include.Location) < 0))
                    result = i + 1;
            }
            if (result == 0 && !location.IsFromMainFile)
            {
                // The original PCH root has no inclusion directive. Its locations
                // precede the current main file; local re-includes do not. File-to-
                // location lookup is insufficient: it selects the later instance.
                bool root;
                try { root = Compare(location, this.mainStart) < 0 && this.IsPchRoot(file); }
                catch (FormatException error) { throw new FormatException($"{file}:{line}:{column}: {error.Message}"); }
                if (root)
                {
                    int instance = this.roots.IndexOf(file);
                    if (instance < 0)
                    {
                        instance = this.roots.Count;
                        this.roots.Add(file);
                    }
                    return -instance - 1;
                }
                throw new FormatException($"{file}:{line}:{column}: Cannot resolve SupportedOS physical include instance; preserve the detailed preprocessing record in precompiled headers.");
            }
            return result;
        }

        private bool IsPchRoot(CXFile file)
        {
            if (this.pchRoots == null)
            {
                this.pchRoots = new();
                for (int i = 0; i + 1 < this.arguments.Count; i++)
                {
                    if (this.arguments[i] != "-include-pch") continue;
                    string path = this.arguments[++i];
                    using var index = CXIndex.Create(excludeDeclarationsFromPch: false, displayDiagnostics: false);
                    var error = CXTranslationUnit.Create(index, path, out var pch);
                    if (error != CXErrorCode.CXError_Success)
                        throw new FormatException($"Cannot read SupportedOS PCH root '{path}': {error}.");
                    using (pch)
                    {
                        using var source = pch.Spelling;
                        this.pchRoots.Add(this.unit.Handle.GetFile(source.ToString()));
                    }
                }
            }
            return this.pchRoots.Contains(file);
        }

        private static int Compare(CXSourceLocation left, CXSourceLocation right) =>
            left.Equals(right) ? 0 : clang.isBeforeInTranslationUnit(left, right) != 0 ? -1 : 1;

        private static FormatException Error(Decl source, string message)
        {
            source.Location.GetExpansionLocation(out var file, out uint line, out uint column, out _);
            return new FormatException($"{file}:{line}:{column}: {message}");
        }
    }
}
