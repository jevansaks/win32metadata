using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class ClrComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/ClrComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the CLR output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static byte[] ReadOriginalBytes(string path) => PropertyComOutputHeaderTests.ReadOriginalBytes(
            path, PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()));

        internal static string ReadOriginal(string path)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var result = LastErrorHeaderTests.Run("git", "-C", root, "show",
                PreRetirementCommit(root) + ":generation/WinSDK/" + path);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            return result.Output;
        }

        internal static readonly (string Path, string[] Selectors)[] Owners =
        {
            ("um/mscoree.h", new[] { "ClrCreateManagedInstance::ppObject", "CorBindToCurrentRuntime::ppv",
                "CorBindToRuntime::ppv", "CorBindToRuntimeByCfg::ppv", "CorBindToRuntimeEx::ppv",
                "CorBindToRuntimeHost::ppv", "ICLRControl::GetCLRManager::ppObject", "IHostControl::GetHostManager::ppObject" }),
            ("um/metahost.h", new[] { "CLRCreateInstance::ppInterface" }),
            ("winrt/rometadata.h", new[] { "MetaDataGetDispenser::ppv" }),
        };
        internal static readonly string[] FamilySelectors = Owners.SelectMany(owner => owner.Selectors).ToArray();
        internal static readonly string[] Selectors = { "MetaDataGetDispenser::ppv" };
        internal static readonly string[] RetainedSelectors = FamilySelectors.Except(Selectors).ToArray();
        internal const string CombinedSelector = "CLRCreateInstance::ppInterface";

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]\n", "", StringComparison.Ordinal);
            return text;
        }

        [TestMethod]
        public void CompleteClrTraversal_AuditsAllHundredParentRecordsAndPreservesIndependentFacts()
        {
            string parent = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = parent.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(100, records.Length);
            Assert.AreEqual(10, FamilySelectors.Length);
            Assert.AreEqual(1, Selectors.Length);
            Assert.AreEqual(9, RetainedSelectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in FamilySelectors)
                CollectionAssert.Contains(records, selector + "=" + (selector == CombinedSelector ? "[Out]" : "") + "[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(RetireRecords(parent), current, "Preserve all independent sidecars exactly.");
            CollectionAssert.AreEqual(RetireRecords(parent).Split('\n').Where(Contract).ToArray(),
                current.Split('\n').Where(Contract).ToArray());
            Assert.AreEqual(99, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            foreach (string selector in RetainedSelectors)
                StringAssert.Contains(current, selector + "=" + (selector == CombinedSelector ? "[Out]" : "") + "[ComOutPtr]\n");
            foreach (string callback in new[] { "CLRCreateInstanceFnPtr::ppInterface", "CreateInterfaceFnPtr::ppInterface" })
                StringAssert.Contains(current, callback + "=[ComOutPtr]\n");
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            var inventory = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, inventory.ExitCode, inventory.Output);
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string partition in new[] { "ClrHosting", "WinRT.Metadata" })
                foreach (string include in File.ReadAllLines(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                    "generation", "WinSDK", "Partitions", partition, "settings.rsp"))
                    .Where(line => line.StartsWith("<IncludeRoot>/", StringComparison.Ordinal)))
                    paths.Add(inventory.Output.Split('\n').Select(line => line.TrimEnd('\r')).Single(line =>
                        line.Equals("generation/WinSDK/RecompiledIdlHeaders/" + include.Substring("<IncludeRoot>/".Length),
                            StringComparison.OrdinalIgnoreCase)));
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (string path in paths)
            {
                string text = ReadOriginal(path.Substring("generation/WinSDK/".Length));
                foreach (string record in records)
                {
                    string selector = record.Split('=')[0];
                    string[] parts = selector.Split("::");
                    if (Regex.IsMatch(parts.Length == 2 ? text : ServiceComOutputHeaderTests.Definition(text, parts[0]),
                        parts.Length == 2 ? FunctionPattern(parts[0]) : MethodPattern(parts[1])))
                        found.Add(selector);
                }
            }
            CollectionAssert.AreEquivalent(FamilySelectors, found.ToArray(),
                "One SDK-owned migration and nine unresolved generated-compatibility source boundaries; callbacks are separate.");

            static bool Contract(string line) => line.Contains("[ComOutPtr]", StringComparison.Ordinal) ||
                line.Contains("[Out]", StringComparison.Ordinal) || line.Contains("[CanReturn", StringComparison.Ordinal) ||
                line == DirectXComOutputHeaderTests.DWriteSelector + "=void**";
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeOneSdkSalSiteAndPreserveGeneratedCompatibilityBytes()
        {
            int sites = 0;
            foreach (var owner in Owners)
            {
                byte[] original = ReadOriginalBytes("RecompiledIdlHeaders/" + owner.Path);
                string before = Encoding.Latin1.GetString(original);
                string after = PatchHeader(owner.Path, before);
                sites += Regex.Matches(after, "_COM_Outptr_").Count - Regex.Matches(before, "_COM_Outptr_").Count;
                CollectionAssert.AreEqual(Encoding.Latin1.GetBytes(after), File.ReadAllBytes(Path.Combine(
                    LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders",
                    owner.Path.Replace('/', Path.DirectorySeparatorChar))), owner.Path);
            }
            Assert.AreEqual(1, sites, "Only MetaDataGetDispenser has an authoritative selected SDK source.");
        }

        internal static string PatchHeader(string path, string text)
        {
            if (path != "winrt/rometadata.h") return text;
            return Regex.Replace(text, FunctionPattern("MetaDataGetDispenser"),
                match => match.Value.Replace("__deref_out LPVOID FAR * ppv", "_COM_Outptr_ LPVOID FAR * ppv", StringComparison.Ordinal));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedNativeDeclarations_PreserveEveryFinalMetadataByte(string target) =>
            AssertShippedDeclarations(target, Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders"));

        internal static void AssertShippedDeclarations(string target, string headers)
        {
            string prefix = @"
#define STDMETHODCALLTYPE __stdcall
#define STDAPI extern ""C"" long __stdcall
#define DEPRECATED_CLR_STDAPI STDAPI
#define FAR
typedef long HRESULT;
typedef unsigned long DWORD;
typedef void* LPVOID;
typedef void VOID;
typedef const unsigned short* LPCWSTR;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFCLSID;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IStream : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = FamilySelectors.ToDictionary(selector => selector,
                selector => selector == CombinedSelector ? "[Out][ComOutPtr]" : "[ComOutPtr]", StringComparer.Ordinal);
            string mappings = string.Join("\n", new[] { "libMappings.rsp", "libMappingsManual.rsp" }
                .Select(file => File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", file))));
            string imports = "--with-librarypath\n" + string.Join("\n", FamilySelectors.Where(selector => selector.Split("::").Length == 2)
                .Select(selector => mappings.Split('\n').Single(line => line.StartsWith(selector.Split("::")[0] + "=", StringComparison.Ordinal)).Trim()));
            var result = fixture.Scrape(prefix + Declarations(true), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] baseline = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(false), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var remaining = RetainedSelectors.ToDictionary(selector => selector,
                selector => selector == CombinedSelector ? "[Out][ComOutPtr]" : "[ComOutPtr]", StringComparer.Ordinal);
            string output = fixture.Emit(remaining);
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(fixture.Emit(remaps)));

            string Declarations(bool legacy)
            {
                string declarations = "";
                foreach (var owner in Owners)
                {
                    string text = legacy ? ReadOriginal("RecompiledIdlHeaders/" + owner.Path) :
                        File.ReadAllText(Path.Combine(headers, owner.Path.Replace('/', Path.DirectorySeparatorChar)));
                    foreach (var group in owner.Selectors.GroupBy(selector => selector.Split("::")[0]))
                    {
                        if (group.First().Split("::").Length == 2)
                        {
                            var matches = Regex.Matches(text, FunctionPattern(group.Key));
                            Assert.AreEqual(1, matches.Count, group.Key);
                            declarations += matches[0].Value + "\n";
                        }
                        else
                        {
                            string definition = ServiceComOutputHeaderTests.Definition(text, group.Key);
                            Match uuid = Regex.Match(definition, @"MIDL_INTERFACE\(""(?<uuid>[0-9A-Fa-f-]{36})""\)");
                            Assert.IsTrue(uuid.Success, group.Key);
                            declarations += "struct __declspec(uuid(\"" + uuid.Groups["uuid"].Value + "\")) " + group.Key + " : public IUnknown {\n";
                            foreach (string selector in group)
                            {
                                var matches = Regex.Matches(definition, MethodPattern(selector.Split("::")[1]));
                                Assert.AreEqual(1, matches.Count, selector);
                                declarations += matches[0].Value + "\n";
                            }
                            declarations += "};\n";
                        }
                    }
                }
                return declarations;
            }
        }

        private static void AssertFinalFacts(string path)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (var handle in reader.MethodDefinitions)
            {
                var method = reader.GetMethodDefinition(handle);
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                string name = reader.GetString(method.Name);
                string methodSelector = method.Attributes.HasFlag(MethodAttributes.PinvokeImpl) ? name : owner + "::" + name;
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                var parameters = method.GetParameters().Select(reader.GetParameter).Where(parameter => parameter.SequenceNumber != 0).ToArray();
                Assert.AreEqual(parameters.Length, signature.ReadCompressedInteger());
                ReadType(ref signature);
                foreach (var parameter in parameters)
                {
                    int start = signature.Offset;
                    ReadType(ref signature);
                    string selector = methodSelector + "::" + reader.GetString(parameter.Name);
                    if (!FamilySelectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    CollectionAssert.AreEqual(new[] { "ComOutPtrAttribute" },
                        attributes.Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray(), selector);
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attributes.Single().Value), selector);
                    CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 },
                        reader.GetBlobBytes(method.Signature).Skip(start).Take(signature.Offset - start).ToArray(), selector);
                }
                Assert.AreEqual(0, signature.RemainingBytes);
            }
            CollectionAssert.AreEquivalent(FamilySelectors, found.ToArray());

            static void ReadType(ref BlobReader signature)
            {
                SignatureTypeCode type = signature.ReadSignatureTypeCode();
                if (type == SignatureTypeCode.Pointer || type == SignatureTypeCode.ByReference) ReadType(ref signature);
                else if (type == SignatureTypeCode.TypeHandle) signature.ReadTypeHandle();
                else Assert.IsTrue(type >= SignatureTypeCode.Void && type <= SignatureTypeCode.Object, type.ToString());
            }
        }

        internal static string FunctionPattern(string name) =>
            @"(?:DEPRECATED_CLR_STDAPI|STDAPI)\s+" + Regex.Escape(name) + @"\s*\([^;{}]+;";

        private static string MethodPattern(string name) =>
            @"virtual HRESULT STDMETHODCALLTYPE " + Regex.Escape(name) + @"\s*\([^;]+;";
    }
}
