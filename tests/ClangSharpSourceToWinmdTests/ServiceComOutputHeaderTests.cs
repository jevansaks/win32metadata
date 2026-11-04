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
    public class ServiceComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/ServiceComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the service output migration fixture.");
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

        internal static readonly (string Owner, string[] Selectors)[] Owners =
        {
            ("ComSvcs.h", new[] { "ITransactionContextEx::CreateInstance::pObject",
                "ICreateWithTipTransactionEx::CreateInstance::pObject", "ICreateWithTransactionEx::CreateInstance::pObject",
                "CoCreateActivity::ppObj", "IComTrackingInfoCollection::Item::ppv",
                "ICreateWithLocalTransaction::CreateInstanceWithSysTx::pObject",
                "IObjectContext::CreateInstance::ppv", "IServicePool::GetObject::ppv", "MTSCreateActivity::ppobj" }),
            ("TxDtc.h", new[] { "IGetDispenser::GetDispenser::ppvObject",
                "IResourceManager::GetDistributedTransactionManager::ppvObject",
                "IResourceManagerFactory2::CreateEx::ppvResMgr" }),
            ("callobj.h", new[] { "CoGetInterceptor::ppv", "CoGetInterceptorFromTypeInfo::ppv" }),
        };
        internal static readonly string[] Selectors = Owners.SelectMany(owner => owner.Selectors).ToArray();

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]\n", "", StringComparison.Ordinal);
            return text;
        }

        [TestMethod]
        public void CompleteServiceTraversal_AuditsEveryParentRecordAndPreservesIndependentFacts()
        {
            string parent = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = parent.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(114, records.Length);
            Assert.AreEqual(14, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(RetireRecords(parent), current, "Preserve all independent sidecars exactly.");
            CollectionAssert.AreEqual(RetireRecords(parent).Split('\n').Where(Contract).ToArray(),
                current.Split('\n').Where(Contract).ToArray());
            Assert.AreEqual(100, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            var inventory = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, inventory.ExitCode, inventory.Output);
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string partition in new[] { "Cos", "DTC", "Com.CallObj", "TransactionServer" })
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
                    string scope = parts.Length == 2 ? text : Definition(text, parts[0]);
                    if (Regex.IsMatch(scope, parts.Length == 2 ? FunctionPattern(parts[0]) : MethodPattern(parts[1])))
                        found.Add(selector);
                }
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray(), "Complete physical service traversal, not a prefix sample.");

            static bool Contract(string line) => line.Contains("[ComOutPtr]", StringComparison.Ordinal) ||
                line.Contains("[Out]", StringComparison.Ordinal) || line.Contains("[CanReturn", StringComparison.Ordinal) ||
                line == DirectXComOutputHeaderTests.DWriteSelector + "=void**";
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalAndEveryInheritedCopy()
        {
            foreach (var owner in Owners)
            {
                byte[] original = ReadOriginalBytes("RecompiledIdlHeaders/um/" + owner.Owner);
                Assert.IsTrue(original.All(value => value <= 127), owner.Owner);
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(PatchHeader(owner.Owner, Encoding.ASCII.GetString(original))),
                    File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                        "RecompiledIdlHeaders", "um", owner.Owner)), owner.Owner);
            }
            byte[] idl = ReadOriginalBytes("RecompiledIdlHeaders/um/callobj.idl");
            Assert.IsTrue(idl.All(value => value <= 127));
            string expected = Encoding.ASCII.GetString(idl).Replace("[out]  void** ppv",
                "[out, annotation(\"_COM_Outptr_\")]  void** ppv", StringComparison.Ordinal);
            CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(expected), File.ReadAllBytes(Path.Combine(
                LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", "callobj.idl")));
        }

        internal static string PatchHeader(string owner, string text)
        {
            foreach (string selector in Owners.Single(entry => entry.Owner == owner).Selectors)
            {
                string[] parts = selector.Split("::");
                if (parts.Length == 2)
                    text = Regex.Replace(text, FunctionPattern(parts[0]), match => PatchParameter(match.Value, parts.Last()));
                else
                    text = Regex.Replace(text, @"#ifndef __\w+_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __\w+_INTERFACE_DEFINED__ \*/",
                        scope => scope.Value.StartsWith("#ifndef __" + parts[0] + "_INTERFACE_DEFINED__", StringComparison.Ordinal) ||
                            scope.Value.Contains("DECLSPEC_XFGVIRT(" + parts[0] + ", " + parts[1] + ")", StringComparison.Ordinal) ?
                            Regex.Replace(scope.Value, @"(?:virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE " +
                                Regex.Escape(parts[1]) + @"\s*\(|HRESULT\s*\(\s*STDMETHODCALLTYPE\s*\*" + Regex.Escape(parts[1]) +
                                @"\s*\)\s*\()[^;]+;", match => PatchParameter(match.Value, parts.Last())) : scope.Value);
            }
            return text;

            static string PatchParameter(string declaration, string parameter) =>
                Regex.Replace(declaration, @"(?:(?<sal>__RPC__deref_out_opt|_Out_)\s+)?(?<type>(?:void|LPVOID)\s*\*+\s*\**\s*)" +
                    Regex.Escape(parameter) + @"\b", match =>
                    (match.Groups["sal"].Value == "__RPC__deref_out_opt" ? "_COM_Outptr_result_maybenull_" : "_COM_Outptr_") +
                    (match.Groups["sal"].Success ? match.Value.Substring(match.Groups["sal"].Length) : " " + match.Value));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedNativeDeclarations_PreserveEveryFinalMetadataByte(string target) =>
            AssertShippedDeclarations(target, Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um"));

        internal static void AssertShippedDeclarations(string target, string headers)
        {
            string prefix = "#include \"" + Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "shared", "rpcsal.h") + "\"\n" + @"
#define STDMETHODCALLTYPE __stdcall
#define EXTERN_C extern ""C""
typedef long HRESULT;
typedef unsigned long ULONG;
typedef unsigned short* BSTR;
typedef char CHAR;
typedef void* LPVOID;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFCLSID;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct ITransaction : IUnknown {};
struct ITypeInfo : IUnknown {};
struct IResourceManagerSink : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            string imports = "--include-directory\n" + Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "shared") + "\n--with-librarypath\n" +
                string.Join("\n", Selectors.Where(selector => selector.Split("::").Length == 2)
                .Select(selector => mappings.Split('\n').Single(line => line.StartsWith(selector.Split("::")[0] + "=", StringComparison.Ordinal)).Trim()));
            var result = fixture.Scrape(prefix + Declarations(true), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] baseline = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(false), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(fixture.Emit(remaps)));

            string Declarations(bool legacy)
            {
                string declarations = "";
                foreach (var owner in Owners)
                {
                    string text = legacy ? ReadOriginal("RecompiledIdlHeaders/um/" + owner.Owner) :
                        File.ReadAllText(Path.Combine(headers, owner.Owner));
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
                            string definition = Definition(text, group.Key);
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
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    Assert.IsFalse(attributes.Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "RetValAttribute"), selector);
                    var com = attributes.Single(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ComOutPtrAttribute");
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(com.Value), selector);
                    CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 },
                        reader.GetBlobBytes(method.Signature).Skip(start).Take(signature.Offset - start).ToArray(), selector);
                }
                Assert.AreEqual(0, signature.RemainingBytes);
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray());

            static void ReadType(ref BlobReader signature)
            {
                SignatureTypeCode type = signature.ReadSignatureTypeCode();
                if (type == SignatureTypeCode.Pointer || type == SignatureTypeCode.ByReference) ReadType(ref signature);
                else if (type == SignatureTypeCode.TypeHandle) signature.ReadTypeHandle();
                else Assert.IsTrue(type >= SignatureTypeCode.Void && type <= SignatureTypeCode.Object, type.ToString());
            }
        }

        internal static string Definition(string text, string name) =>
            Regex.Match(text, @"#ifndef __" + Regex.Escape(name) + @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" +
                Regex.Escape(name) + @"_INTERFACE_DEFINED__ \*/").Value;

        internal static string FunctionPattern(string name) =>
            @"(?:EXTERN_C\s+)?HRESULT\s+__stdcall\s+" + Regex.Escape(name) + @"\s*\([^;{}]+;";

        private static string MethodPattern(string name) =>
            @"virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE " + Regex.Escape(name) + @"\s*\([^;]+;";
    }
}
