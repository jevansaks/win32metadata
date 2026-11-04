using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ClangSharp;
using ClangSharp.Interop;

namespace Win32MetadataScraper
{
    // Nominal projection is a reader policy; storage and alias relationships come from this TU.
    internal sealed class NativeTypedefs
    {
        private const string Association = "Win32Metadata.AlsoUsableFor";
        private readonly Dictionary<string, TypedefNameDecl> declarations = new(StringComparer.Ordinal);
        private readonly Dictionary<string, TypedefNameDecl> associations = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> storage = new(StringComparer.Ordinal);
        private readonly HashSet<string> emitted = new(StringComparer.Ordinal);

        public void Visit(Decl declaration)
        {
            if (declaration is TypedefNameDecl alias)
                this.declarations[Name(alias)] = alias;
            foreach (var attribute in declaration.Attrs.Where(a =>
                a.Kind == CX_AttrKind.CX_AttrKind_Annotate && a.Spelling.StartsWith(Association, StringComparison.Ordinal)))
            {
                if (declaration is not TypedefNameDecl site)
                    throw Error(declaration, "AlsoUsableFor requires a genuine native typedef or alias.");
                var match = Regex.Match(attribute.Spelling,
                    @"^Win32Metadata\.AlsoUsableFor\((?:::)?[A-Za-z_][A-Za-z_0-9]*(?:::[A-Za-z_][A-Za-z_0-9]*)*\)$");
                var arguments = attribute.CursorChildren.ToArray();
                if (!match.Success || arguments.Length != 1 ||
                    arguments[0] is not UnaryExprOrTypeTraitExpr size ||
                    size.Kind != CX_UnaryExprOrTypeTrait.CX_UETT_SizeOf || !size.IsArgumentType ||
                    RemapDiscovery.UnwrapType(size.ArgumentType) is not TypedefType target)
                    throw Error(declaration, "Unknown or malformed AlsoUsableFor association; expected one compiler-resolved native typedef operand.");
                attribute.Extent.Start.GetExpansionLocation(out _, out _, out _, out uint offset);
                if (offset > HeaderAnnotations.GetDeclarationStart(declaration, attribute))
                    throw Error(declaration, "AlsoUsableFor must precede the complete native typedef declaration.");
                if (this.associations.ContainsKey(Name(site)))
                    throw Error(declaration, "Duplicate or conflicting AlsoUsableFor associations.");
                this.associations.Add(Name(site), target.Decl);
            }
        }

        public void Configure(IEnumerable<string> selected, string ns, ISet<string> traversal,
            Dictionary<string, string> remaps, Dictionary<string, string> types, List<string> excludes)
        {
            var owners = selected.Select(option => option.Split('=', 2))
                .ToDictionary(parts => parts[0], parts => parts.Length == 2 ? parts[1] : null, StringComparer.Ordinal);
            var nominal = owners.Keys.ToHashSet(StringComparer.Ordinal);
            foreach (var association in this.associations.ToArray())
            {
                nominal.Add(association.Key);
                string target = Name(association.Value);
                var site = this.declarations[association.Key];
                if (!this.declarations.TryGetValue(target, out var targetAlias))
                    throw Error(site, $"AlsoUsableFor target '{target}' is not an existing native typedef.");
                if (target == association.Key)
                    throw Error(site, "AlsoUsableFor cannot refer to its own typedef.");
                if (!site.UnderlyingType.CanonicalType.Equals(targetAlias.UnderlyingType.CanonicalType))
                    throw Error(site, $"AlsoUsableFor target '{target}' has conflicting native storage.");
                nominal.Add(target);
            }

            foreach (string name in nominal.Where(this.declarations.ContainsKey))
            {
                var alias = this.declarations[name];
                if (Excluded(name))
                    throw Error(alias, $"Native typedef '{name}' is excluded from metadata.");
                if (alias.Attrs.Any(a => a.Spelling.StartsWith("Win32Metadata.Enum(", StringComparison.Ordinal) ||
                    a.Spelling.StartsWith("Win32Metadata.FlagsEnum(", StringComparison.Ordinal)))
                    throw Error(alias, "Enum groups are not nominal native typedef projections.");
                string value = this.Storage(alias.UnderlyingType, alias, remaps, new HashSet<string>());
                this.storage.Add(name, value);
                if (this.storage.Keys.Count(key => ProjectedName(key) == ProjectedName(name)) != 1)
                    throw Error(alias, $"Native typedef '{name}' has an ambiguous projected name; an explicit namespace policy is required.");
                alias.Location.GetPresumedLocation(out var file, out _, out _);
                // Retain existing explicit namespace ownership even when the SDK defines the typedef in a shared include.
                if (owners.TryGetValue(name, out string owner) && owner != null ? owner == ns :
                    traversal.Count == 0 || traversal.Contains(Path.GetFullPath(file.ToString())))
                    this.emitted.Add(name);
                if (remaps.TryGetValue(name, out string configured) && configured != name && configured != value)
                    throw Error(alias, $"Configured remap for '{name}' conflicts with native storage '{value}'.");
                remaps[name] = ProjectedName(name);
                if (types.TryGetValue(name, out string configuredType) && configuredType != ProjectedName(name))
                    throw Error(alias, $"Configured type for '{name}' conflicts with native nominal identity.");
                types[name] = ProjectedName(name);
                if (!excludes.Contains(name)) excludes.Add(name);
            }

            foreach (var pair in this.declarations)
            {
                if (this.storage.ContainsKey(pair.Key)) continue;
                string value = AliasReference(pair.Value.UnderlyingType, pair.Value, new HashSet<string>());
                if (value == null) continue;
                if (types.TryGetValue(pair.Key, out string configured) && configured != value ||
                    remaps.TryGetValue(pair.Key, out configured) && configured != value && configured != pair.Key)
                    throw Error(pair.Value, $"Configured alias '{pair.Key}' conflicts with native storage '{value}'.");
                types[pair.Key] = value;
            }

            bool Excluded(string name) => excludes.Any(pattern =>
                Regex.IsMatch(name, "\\A" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "\\z"));

            string AliasReference(ClangSharp.Type type, TypedefNameDecl site, HashSet<string> visited)
            {
                long size = type.Handle.SizeOf;
                type = RemapDiscovery.UnwrapType(type);
                if (type is PointerType pointer)
                {
                    string pointee = AliasReference(pointer.PointeeType, site, visited);
                    if (pointee != null)
                    {
                        using var target = site.TranslationUnit.Handle.TargetInfo;
                        if (size * 8 != target.PointerWidth)
                            throw Error(site, "Unsupported native typedef alias pointer width for this target.");
                    }
                    return pointee == null ? null : pointee + "*";
                }
                if (type is not TypedefType reference || !visited.Add(Name(reference.Decl))) return null;
                string name = Name(reference.Decl);
                return this.storage.ContainsKey(name) ? ProjectedName(name) :
                    AliasReference(reference.Decl.UnderlyingType, site, visited);
            }
        }

        public void Write(string output, string ns)
        {
            if (File.Exists(output)) File.Delete(output);
            if (this.emitted.Count == 0) return;
            using var writer = new StreamWriter(output, false, new UTF8Encoding(false));
            writer.WriteLine("using System;\nusing Windows.Win32.Foundation.Metadata;");
            writer.WriteLine($"namespace {ns}\n{{");
            foreach (string name in this.emitted.OrderBy(n => n, StringComparer.Ordinal))
            {
                string value = this.storage[name];
                this.declarations[name].Location.GetPresumedLocation(out var file, out uint line, out _);
                string path = file.ToString().Replace("\\", "\\\\").Replace("\"", "\\\"");
                writer.WriteLine($"#line {line} \"{path}\"");
                writer.WriteLine("    [NativeTypedef]");
                if (this.associations.TryGetValue(name, out var association))
                    writer.WriteLine($"    [AlsoUsableFor(\"{ProjectedName(Name(association))}\")]");
                writer.WriteLine($"    public unsafe partial struct {ProjectedName(name)}\n    {{");
                writer.WriteLine($"        public {value} Value;\n    }}");
            }
            writer.WriteLine("}");
        }

        private string Storage(ClangSharp.Type type, TypedefNameDecl site,
            IReadOnlyDictionary<string, string> remaps, HashSet<string> visited)
        {
            long size = type.Handle.SizeOf;
            type = RemapDiscovery.UnwrapType(type);
            if (type is TypedefType reference)
            {
                string name = Name(reference.Decl);
                if (!visited.Add(name)) throw Error(site, "Unsupported cyclic native typedef chain.");
                if (remaps.TryGetValue(name, out string mapped) && mapped is "IntPtr" or "UIntPtr")
                {
                    var canonical = type.CanonicalType;
                    using var target = site.TranslationUnit.Handle.TargetInfo;
                    bool signed = canonical.Kind is CXTypeKind.CXType_Int or CXTypeKind.CXType_Long or CXTypeKind.CXType_LongLong;
                    bool unsigned = canonical.Kind is CXTypeKind.CXType_UInt or CXTypeKind.CXType_ULong or CXTypeKind.CXType_ULongLong;
                    if (!(mapped == "IntPtr" ? signed : unsigned) ||
                        canonical.Handle.SizeOf * 8 != target.PointerWidth)
                        throw Error(site, $"Pointer-sized projection '{mapped}' conflicts with native width or signedness.");
                    return mapped;
                }
                return this.Storage(reference.Decl.UnderlyingType, site, remaps, visited);
            }
            if (type is PointerType pointer)
            {
                using var target = site.TranslationUnit.Handle.TargetInfo;
                if (size * 8 != target.PointerWidth)
                    throw Error(site, "Unsupported native typedef pointer width for this target.");
                var pointee = pointer.PointeeType.CanonicalType;
                if (pointee.Kind == CXTypeKind.CXType_Void ||
                    pointee is RecordType record && !record.Decl.IsCompleteDefinition)
                    return "void*";
                throw Error(site, "Unsupported native typedef pointer shape; expected void or an incomplete opaque record.");
            }
            return type.CanonicalType.Kind switch
            {
                CXTypeKind.CXType_Char_U or CXTypeKind.CXType_UChar when size == 1 => "byte",
                CXTypeKind.CXType_Char_S or CXTypeKind.CXType_SChar when size == 1 => "sbyte",
                CXTypeKind.CXType_Short when size == 2 => "short",
                CXTypeKind.CXType_UShort when size == 2 => "ushort",
                CXTypeKind.CXType_Int or CXTypeKind.CXType_Long when size == 4 => "int",
                CXTypeKind.CXType_UInt or CXTypeKind.CXType_ULong when size == 4 => "uint",
                CXTypeKind.CXType_LongLong when size == 8 => "long",
                CXTypeKind.CXType_ULongLong when size == 8 => "ulong",
                _ => throw Error(site, "Unsupported native typedef storage; expected an integer or opaque pointer, not a record, enum, array or callback."),
            };
        }

        private static string ProjectedName(string name) =>
            name.Substring(name.LastIndexOf("::", StringComparison.Ordinal) + (name.Contains("::") ? 2 : 1));

        private static string Name(NamedDecl declaration)
        {
            var context = declaration.DeclContext;
            while (context is LinkageSpecDecl linkage) context = linkage.DeclContext;
            return context is NamedDecl parent ? Name(parent) + "::" + declaration.Name : declaration.Name;
        }

        private static FormatException Error(Decl declaration, string message)
        {
            declaration.Location.GetPresumedLocation(out var file, out uint line, out uint column);
            return new FormatException($"{file}:{line}:{column}: {message}");
        }
    }
}
