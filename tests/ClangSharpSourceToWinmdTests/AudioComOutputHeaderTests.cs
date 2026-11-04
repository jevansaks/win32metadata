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
    public class AudioComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/AudioComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the audio output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static byte[] ReadOriginalBytes(string path) => PropertyComOutputHeaderTests.ReadOriginalBytes(
            path, PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()));

        internal static string ReadOriginal(string path) => Encoding.Latin1.GetString(ReadOriginalBytes(path));

        internal static readonly (string Path, string[] Selectors)[] Owners =
        {
            ("um/mmdeviceapi.h", new[] { "IMMDevice::Activate::ppInterface" }),
            ("um/Audioclient.h", new[] { "IAudioClient::GetService::ppv" }),
            ("um/devicetopology.h", new[] { "IPart::Activate::ppvObject" }),
            ("um/dsound.h", new[] { "IDirectSoundBuffer8::GetObjectInPath::ppObject",
                "IDirectSoundCaptureBuffer8::GetObjectInPath::ppObject" }),
            ("um/strmif.h", new[] { "ICaptureGraphBuilder::FindInterface::ppint",
                "ICaptureGraphBuilder2::FindInterface::ppint", "IDvdGraphBuilder::GetDvdInterface::ppvIF" }),
            ("shared/ksproxy.h", new[] { "IKsTopology::CreateNodeInstance::Interface" }),
            ("um/vidcap.h", new[] { "IKsTopologyInfo::CreateNodeInstance::ppvObject" }),
            ("um/wmsdkidl.h", new[] { "IWMReaderAccelerator::GetCodecInterface::ppvCodecInterface" }),
        };
        internal static readonly string[] Idls = { "mmdeviceapi.idl", "Audioclient.idl", "devicetopology.idl",
            "axextend.idl", "dvdif.idl", "vidcap.idl", "wmsdkidl.idl" };
        internal static readonly string[] Selectors = Owners.SelectMany(owner => owner.Selectors).ToArray();

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]\n", "", StringComparison.Ordinal);
            return text;
        }

        [TestMethod]
        public void CompleteTraversal_AuditsAllNinetyNineParentRecordsAndPreservesIndependentFacts()
        {
            string parent = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = parent.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(99, records.Length);
            Assert.AreEqual(11, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(RetireRecords(parent), current, "Preserve the complete independent sidecar exactly.");
            CollectionAssert.AreEqual(RetireRecords(parent).Split('\n').Where(Contract).ToArray(),
                current.Split('\n').Where(Contract).ToArray());
            Assert.AreEqual(88, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            var inventory = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, inventory.ExitCode, inventory.Output);
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string partition in new[] { "Audio", "Audio.DirectSound", "Audio.Endpoints", "Media.DShow",
                "Media.DShow.Xml", "Media.DxMediaObjects", "Media.KernelStreaming", "WmFormat" })
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
                    if (parts.Length == 3 && Regex.IsMatch(Definition(text, parts[0]), MethodPattern(parts[1])))
                        found.Add(selector);
                    else if (parts.Length == 2 && Regex.IsMatch(text,
                        @"\b" + Regex.Escape(parts[0]) + @"\s*\("))
                        found.Add(selector);
                }
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray(), "Complete physical traversals, not a prefix sample.");

            static bool Contract(string line) => line.Contains("[ComOutPtr]", StringComparison.Ordinal) ||
                line.Contains("[Out]", StringComparison.Ordinal) || line.Contains("[CanReturn", StringComparison.Ordinal) ||
                line == DirectXComOutputHeaderTests.DWriteSelector + "=void**";
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalIncludingInheritedCopies()
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
            Assert.AreEqual(21, sites, "Both guarded materializations and inherited IAudioClient2/3 C entries.");
            foreach (string idl in Idls)
                CollectionAssert.AreEqual(Encoding.Latin1.GetBytes(PatchIdl(idl, Encoding.Latin1.GetString(
                    ReadOriginalBytes("RecompiledIdlHeaders/um/" + idl)))),
                    File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                        "RecompiledIdlHeaders", "um", idl)), idl);
        }

        internal static string PatchIdl(string name, string text) => name switch
        {
            "mmdeviceapi.idl" => new Regex(Regex.Escape("[out,iid_is(iid), annotation(\"_Out_\")] void** ppInterface"))
                .Replace(text, "[out,iid_is(iid), annotation(\"_COM_Outptr_\")] void** ppInterface", 1),
            "Audioclient.idl" => text.Replace("[out, iid_is(riid), annotation(\"_Out_\")] void **ppv",
                "[out, iid_is(riid), annotation(\"_COM_Outptr_\")] void **ppv", StringComparison.Ordinal),
            "devicetopology.idl" => text.Replace("[out,iid_is(refiid), annotation(\"_Out_opt_\")] void** ppvObject",
                "[out,iid_is(refiid), annotation(\"_COM_Outptr_opt_\")] void** ppvObject", StringComparison.Ordinal),
            "axextend.idl" => text.Replace("[out AM_ANNOTATION(\"_Out_\")] void **ppint",
                "[out AM_ANNOTATION(\"_COM_Outptr_\")] void **ppint", StringComparison.Ordinal),
            "dvdif.idl" => text.Replace("[out, annotation(\"_Out_\")] void **ppvIF",
                "[out, annotation(\"_COM_Outptr_\")] void **ppvIF", StringComparison.Ordinal),
            "vidcap.idl" => text.Replace("HRESULT CreateNodeInstance([in] DWORD dwNodeId, [in] REFIID iid, [out] void **ppvObject);",
                "HRESULT CreateNodeInstance([in, annotation(\"_In_\")] DWORD dwNodeId, [in, annotation(\"_In_\")] REFIID iid, " +
                "[out, annotation(\"_COM_Outptr_\")] void **ppvObject);", StringComparison.Ordinal),
            "wmsdkidl.idl" => text.Replace("[out] void     **ppvCodecInterface",
                "[out, annotation(\"_COM_Outptr_\")] void     **ppvCodecInterface", StringComparison.Ordinal),
            _ => throw new ArgumentOutOfRangeException(nameof(name)),
        };

        internal static string PatchHeader(string path, string text)
        {
            foreach (string selector in Owners.Single(owner => owner.Path == path).Selectors)
            {
                string[] parts = selector.Split("::");
                string pattern = path is "um/dsound.h" or "shared/ksproxy.h" ? MethodPattern(parts[1]) :
                    @"(?:virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE " + parts[1] +
                        @"\s*\(|HRESULT\s*\(\s*STDMETHODCALLTYPE\s*\*" + parts[1] + @"\s*\)\s*\()[^;]+;";
                if (path == "um/mmdeviceapi.h")
                    text = Regex.Replace(text, @"#ifndef __IMMDevice_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __IMMDevice_INTERFACE_DEFINED__ \*/",
                        match => Replace(match.Value));
                else text = Replace(text);

                string Replace(string scope) => Regex.Replace(scope, pattern, match =>
                    Regex.Replace(match.Value, @"(?<!\w)(?<sal>_Out_|_Outptr_|_Out_opt_)(?<space>\s+)(?<type>(?:void|LPVOID)\s*\*+\s*\**\s*)" +
                        parts[2] + @"\b", parameter => (parameter.Groups["sal"].Value == "_Out_opt_" ?
                            "_COM_Outptr_opt_" : "_COM_Outptr_") + parameter.Value.Substring(parameter.Groups["sal"].Length)));
            }
            return text;
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
#define STDMETHOD(name) virtual HRESULT STDMETHODCALLTYPE name
#define THIS_
#define PURE = 0
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef unsigned long ACCESS_MASK;
typedef void* LPVOID;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFGUID;
struct PROPVARIANT;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IBaseFilter : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            var result = fixture.Scrape(prefix + Declarations(true), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] baseline = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(false), target, sal: true);
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
                    string text = legacy ? ReadOriginal("RecompiledIdlHeaders/" + owner.Path) :
                        File.ReadAllText(Path.Combine(headers, owner.Path.Replace('/', Path.DirectorySeparatorChar)));
                    foreach (var group in owner.Selectors.GroupBy(selector => selector.Split("::")[0]))
                    {
                        string definition = Definition(text, group.Key);
                        Assert.IsTrue(definition.Length > 0, group.Key);
                        Match uuid = Regex.Match(definition, @"MIDL_INTERFACE\(""(?<uuid>[^""]+)""\)");
                        if (!uuid.Success)
                            uuid = Regex.Match(text, @"interface DECLSPEC_UUID\(""(?<uuid>[^""]+)""\)\s*" + group.Key + @";");
                        declarations += "struct " + (uuid.Success ? "__declspec(uuid(\"" +
                            uuid.Groups["uuid"].Value + "\")) " : "") + group.Key + " : public IUnknown {\n";
                        foreach (string selector in group)
                        {
                            var matches = Regex.Matches(definition, MethodPattern(selector.Split("::")[1]));
                            Assert.AreEqual(1, matches.Count, selector);
                            declarations += matches[0].Value + "\n";
                        }
                        declarations += "};\n";
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
                Assert.AreEqual(MethodImplAttributes.IL, method.ImplAttributes & MethodImplAttributes.PreserveSig);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                var parameters = method.GetParameters().Select(reader.GetParameter).Where(parameter => parameter.SequenceNumber != 0).ToArray();
                Assert.AreEqual(parameters.Length, signature.ReadCompressedInteger());
                ReadType(ref signature);
                foreach (var parameter in parameters)
                {
                    int start = signature.Offset;
                    ReadType(ref signature);
                    string selector = owner + "::" + name + "::" + reader.GetString(parameter.Name);
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(ParameterAttributes.Out | (owner == "IPart" ? ParameterAttributes.Optional : 0),
                        parameter.Attributes, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    CollectionAssert.AreEqual(new[] { "ComOutPtrAttribute" },
                        attributes.Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray(), selector);
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attributes.Single().Value), selector);
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

        internal static string Definition(string text, string name)
        {
            Match midl = Regex.Match(text, @"MIDL_INTERFACE\(""[^""]+""\)\s*" + Regex.Escape(name) + @"\s*:[^{]+\{[\s\S]+?\n\s*\};");
            if (midl.Success) return midl.Value;
            return Regex.Match(text, @"DECLARE_INTERFACE_\(" + Regex.Escape(name) + @",[^)]+\)\s*\{[\s\S]+?\n\s*\};").Value;
        }

        private static string MethodPattern(string name) =>
            @"(?:virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE " + Regex.Escape(name) + @"\s*\([^;]+;|" +
            @"STDMETHOD\(" + Regex.Escape(name) + @"\)\s*\([^;]+;)";
    }
}
