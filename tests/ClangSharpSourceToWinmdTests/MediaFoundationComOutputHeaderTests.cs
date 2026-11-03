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
    public class MediaFoundationComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/MediaFoundationComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the Media Foundation output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static byte[] ReadOriginalBytes(string path) => PropertyComOutputHeaderTests.ReadOriginalBytes(
            path, PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()));

        internal static string ReadOriginal(string path, bool checkoutBytes = false)
        {
            if (checkoutBytes) return Encoding.Latin1.GetString(ReadOriginalBytes(path));
            string root = LastErrorHeaderTests.RepositoryRoot();
            var result = LastErrorHeaderTests.Run("git", "-C", root, "show",
                PreRetirementCommit(root) + ":generation/WinSDK/" + path);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            return result.Output;
        }

        internal static readonly (string Owner, string[] Selectors)[] Owners =
        {
            ("mfapi.h", new[] { "MFCreateAttributes::ppMFAttributes", "MFCreateMediaExtensionActivate::ppvObject",
                "MFCreateVideoSampleAllocatorEx::ppSampleAllocator" }),
            ("mfcaptureengine.h", new[] { "IMFCaptureEngineClassFactory::CreateInstance::ppvObject" }),
            ("mfsharingengine.h", new[] { "IMFImageSharingEngineClassFactory::CreateInstanceFromUDN::ppEngine" }),
            ("mfreadwrite.h", new[] { "IMFReadWriteClassFactory::CreateInstanceFromURL::ppvObject",
                "IMFReadWriteClassFactory::CreateInstanceFromObject::ppvObject", "IMFSinkWriter::GetServiceForStream::ppvObject",
                "IMFSourceReader::GetServiceForStream::ppvObject" }),
            ("mfobjects.h", new[] { "IMFDXGIBuffer::GetResource::ppvObject", "IMFDXGIBuffer::GetUnknown::ppvObject",
                "IMFDXGIDeviceManager::GetVideoService::ppService" }),
            ("mfidl.h", new[] { "IMFInputTrustAuthority::GetDecrypter::ppv", "IMFObjectReferenceStream::LoadReference::ppv",
                "IMFRemoteProxy::GetRemoteHost::ppv", "IMFRemoteProxy::GetRemoteObject::ppv",
                "MFCreateNetSchemePlugin::ppvHandler", "MFCreatePropertiesFromMediaType::ppv",
                "MFCreateStreamOnMFByteStreamEx::ppv", "MFGetService::ppvObject" }),
            ("mfmediaengine.h", new[] { "IMFMediaEngineClassFactory4::CreateContentDecryptionModuleFactory::ppvObject" }),
            ("mfd3d12.h", new[] { "MFCreateD3D12SynchronizationObject::ppvSyncObject" }),
            ("evr.h", new[] { "MFCreateVideoMixer::ppv", "MFCreateVideoMixerAndPresenter::ppvVideoMixer",
                "MFCreateVideoMixerAndPresenter::ppvVideoPresenter", "MFCreateVideoPresenter::ppVideoPresenter",
                "MFCreateVideoRenderer::ppVideoRenderer", "MFCreateVideoSampleAllocator::ppSampleAllocator" }),
            ("dxva2api.h", new[] { "DXVA2CreateVideoService::ppService", "IDirect3DDeviceManager9::GetVideoService::ppService" }),
        };
        internal static readonly string[] Selectors = Owners.SelectMany(owner => owner.Selectors).ToArray();
        internal static readonly string[] IdlOwners = Owners.Where(owner => owner.Owner != "mfapi.h")
            .Select(owner => owner.Owner == "mfidl.h" ? "Mfidl.idl" : owner.Owner == "mfobjects.h" ?
                "Mfobjects.idl" : Path.ChangeExtension(owner.Owner, "idl")).ToArray();

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]\n", "", StringComparison.Ordinal);
            return text;
        }

        [TestMethod]
        public void CompleteMfTraversal_AuditsEveryParentRecordAndPreservesIndependentFacts()
        {
            string parent = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = parent.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(144, records.Length);
            Assert.AreEqual(30, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(ImagingComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(RetireRecords(parent))))))), current, "Preserve all independent sidecars exactly.");
            CollectionAssert.AreEqual(ImagingComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(RetireRecords(parent))))))).Split('\n').Where(Contract).ToArray(),
                current.Split('\n').Where(Contract).ToArray(), "All independent output and result records remain exact.");
            Assert.AreEqual(73, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            var inventory = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, inventory.ExitCode, inventory.Output);
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (string include in File.ReadAllLines(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "Partitions", "Mf", "settings.rsp")).Where(line => line.StartsWith("<IncludeRoot>/", StringComparison.Ordinal)))
            {
                string path = inventory.Output.Split('\n').Select(line => line.TrimEnd('\r')).Single(line =>
                    line.Equals("generation/WinSDK/RecompiledIdlHeaders/" + include.Substring("<IncludeRoot>/".Length),
                        StringComparison.OrdinalIgnoreCase));
                string text = ReadOriginal(path.Substring("generation/WinSDK/".Length));
                foreach (string record in records)
                {
                    string selector = record.Split('=')[0];
                    string[] parts = selector.Split("::");
                    string scope = parts.Length == 2 ? text : DirectXComOutputHeaderTests.Definition(text, parts[0]);
                    if (Regex.IsMatch(scope, parts.Length == 2 ? FunctionPattern(parts[0]) : MethodPattern(parts[1])))
                        found.Add(selector);
                }
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray(), "Complete selected MF physical ownership, not a name-prefix sample.");

            static bool Contract(string line) => line.Contains("[ComOutPtr]", StringComparison.Ordinal) ||
                line.Contains("[Out]", StringComparison.Ordinal) || line.Contains("[CanReturn", StringComparison.Ordinal) ||
                line == DirectXComOutputHeaderTests.DWriteSelector + "=void**";
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalAndAllInheritedVtableCopies()
        {
            foreach (var owner in Owners)
            {
                string original = ReadOriginal("RecompiledIdlHeaders/um/" + owner.Owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner.Owner);
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(PatchHeader(owner.Owner, original)),
                    File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                        "RecompiledIdlHeaders", "um", owner.Owner)), owner.Owner);
            }
            foreach (string owner in IdlOwners)
            {
                string original = Encoding.Latin1.GetString(ReadOriginalBytes(
                    "RecompiledIdlHeaders/um/" + owner));
                string patched = Encoding.Latin1.GetString(File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                    "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)));
                string[] before = original.Split('\n'), after = patched.Split('\n');
                Assert.AreEqual(before.Length, after.Length, owner);
                int changed = 0;
                for (int line = 0; line < before.Length; line++)
                {
                    if (before[line] == after[line]) continue;
                    changed++;
                    string actual = after[line];
                    Assert.AreEqual(1, Regex.Matches(actual, "_COM_Outptr_").Count, owner + ":" + (line + 1));
                    Assert.IsTrue(new[]
                    {
                        actual.Replace("_COM_Outptr_", "_Outptr_", StringComparison.Ordinal),
                        actual.Replace("_COM_Outptr_", "_Out_", StringComparison.Ordinal),
                        actual.Replace("_COM_Outptr_ ", "", StringComparison.Ordinal),
                        actual.Replace(", annotation(\"_COM_Outptr_\")", "", StringComparison.Ordinal),
                    }.Contains(before[line]), owner + ":" + (line + 1));
                }
                Assert.AreEqual(Owners.Single(entry => entry.Owner.Equals(Path.ChangeExtension(owner, "h"),
                    StringComparison.OrdinalIgnoreCase)).Selectors.Length, changed, owner);
            }
        }

        internal static string PatchHeader(string owner, string text)
        {
            foreach (var group in Owners.Single(entry => entry.Owner == owner).Selectors.GroupBy(selector => selector.Split("::")[0]))
            {
                if (group.First().Split("::").Length == 2)
                    text = Regex.Replace(text, FunctionPattern(group.Key), match => PatchParameters(match.Value, group));
                else
                {
                    string pattern = @"#ifndef __" + Regex.Escape(group.Key) +
                        @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" + Regex.Escape(group.Key) + @"_INTERFACE_DEFINED__ \*/";
                    // Derived C vtables repeat their base method's authoritative declaration.
                    foreach (string selector in group)
                        text = Regex.Replace(text, pattern, match => PatchMethod(match.Value, selector));
                    foreach (Match definition in Regex.Matches(text, @"#ifndef __\w+_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __\w+_INTERFACE_DEFINED__ \*/"))
                    {
                        string scope = definition.Value;
                        if (scope.StartsWith("#ifndef __" + group.Key + "_INTERFACE_DEFINED__", StringComparison.Ordinal)) continue;
                        if (!scope.Contains("DECLSPEC_XFGVIRT(" + group.Key + ", ", StringComparison.Ordinal)) continue;
                        string patched = scope;
                        foreach (string selector in group) patched = PatchMethod(patched, selector);
                        text = text.Replace(scope, patched, StringComparison.Ordinal);
                    }
                }
            }
            return text;

            static string PatchMethod(string text, string selector)
            {
                string method = selector.Split("::")[1];
                return Regex.Replace(text, @"(?:virtual\s+(?:/\* \[local\] \*/\s+)?HRESULT STDMETHODCALLTYPE " +
                    Regex.Escape(method) + @"\s*\(|HRESULT\s*\(\s*STDMETHODCALLTYPE\s*\*" + Regex.Escape(method) +
                    @"\s*\)\s*\()[^;]+;", match => PatchParameters(match.Value, new[] { selector }));
            }
        }

        private static string PatchParameters(string text, IEnumerable<string> selectors)
        {
            foreach (string selector in selectors)
                text = Regex.Replace(text, @"(?:(?<sal>_Out_|_Outptr_|_Outptr_result_maybenull_)\s+)?(?<type>(?:void|LPVOID|IMFAttributes|IMFImageSharingEngine)\s*\*+\s*\**\s*)" +
                    Regex.Escape(selector.Split("::").Last()) + @"\b", match =>
                    match.Value.StartsWith("_COM_", StringComparison.Ordinal) ? match.Value :
                    (match.Groups["sal"].Value == "_Outptr_result_maybenull_" ? "_COM_Outptr_result_maybenull_" : "_COM_Outptr_") +
                    (match.Groups["sal"].Success ? match.Value.Substring(match.Groups["sal"].Length) : " " + match.Value));
            return text;
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
            const string prefix = @"
#define WINAPI __stdcall
#define STDMETHODCALLTYPE __stdcall
#define STDAPI HRESULT __stdcall
#define EVRPUBLIC(func) STDAPI func
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned long UINT32;
typedef void* LPVOID;
typedef void* HANDLE;
typedef const unsigned short* LPCWSTR;
typedef const unsigned short* PCWSTR;
typedef unsigned short* BSTR;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFGUID;
typedef const GUID& REFCLSID;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IMFAttributes : IUnknown {};
struct IMFImageSharingEngine : IUnknown {};
struct IMFMediaType : IUnknown {};
struct IMFByteStream : IUnknown {};
struct ID3D12Device : IUnknown {};
struct IDirect3DDevice9 : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            string imports = "--with-librarypath\n" + string.Join("\n", Selectors.Where(selector => selector.Split("::").Length == 2)
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
                            string definition = DirectXComOutputHeaderTests.Definition(text, group.Key);
                            Match uuid = Regex.Match(text, @"MIDL_INTERFACE\(""(?<uuid>[0-9A-Fa-f-]{36})""\)\s*" + group.Key + @"\b");
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
                    if (selector == "MFCreateAttributes::ppMFAttributes" || selector == "IMFImageSharingEngineClassFactory::CreateInstanceFromUDN::ppEngine")
                    {
                        var type = reader.GetBlobReader(method.Signature);
                        type.Offset = start;
                        Assert.AreEqual(SignatureTypeCode.Pointer, type.ReadSignatureTypeCode(), selector);
                        Assert.AreEqual(SignatureTypeCode.TypeHandle, type.ReadSignatureTypeCode(), selector);
                        var typeHandle = (TypeDefinitionHandle)type.ReadTypeHandle();
                        Assert.AreEqual(selector == "MFCreateAttributes::ppMFAttributes" ? "IMFAttributes" : "IMFImageSharingEngine",
                            reader.GetString(reader.GetTypeDefinition(typeHandle).Name), selector);
                        Assert.AreEqual(signature.Offset, type.Offset);
                    }
                    else CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 },
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

        internal static string FunctionPattern(string name) =>
            @"(?:STDAPI\s+" + Regex.Escape(name) + @"|EVRPUBLIC\(" + Regex.Escape(name) +
            @"\)|HRESULT\s+" + Regex.Escape(name) + @")\s*\([^;{}]+;";

        internal static string MethodPattern(string name) =>
            @"virtual\s+(?:/\* \[local\] \*/\s+)?HRESULT STDMETHODCALLTYPE " + Regex.Escape(name) + @"\s*\([^;]+;";
    }
}
