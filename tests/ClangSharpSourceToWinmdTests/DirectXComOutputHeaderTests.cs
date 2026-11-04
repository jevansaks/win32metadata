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
    public class DirectXComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/DirectXComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the DirectX output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static string ReadOriginal(string path, bool checkoutBytes = false)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string parent = PreRetirementCommit(root);
            var result = checkoutBytes
                ? LastErrorHeaderTests.Run("git", "-C", root, "cat-file", "--filters", parent + ":generation/WinSDK/" + path)
                : LastErrorHeaderTests.Run("git", "-C", root, "show", parent + ":generation/WinSDK/" + path);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            return result.Output;
        }

        internal static readonly (string Path, string[] Selectors)[] Owners =
        {
            ("um/d2d1.h", new[] { "D2D1CreateFactory::ppIFactory", "ID2D1Resource::GetFactory::factory" }),
            ("um/d3d10.h", new[] { "ID3D10Device::OpenSharedResource::ppResource" }),
            ("um/d3d12.h", new[] { "D3D12CreateRootSignatureDeserializer::ppRootSignatureDeserializer",
                "D3D12CreateVersionedRootSignatureDeserializer::ppRootSignatureDeserializer" }),
            ("um/d3dcompiler.h", new[] { "D3DReflect::ppReflector", "D3DReflectLibrary::ppReflector" }),
            ("um/dwrite.h", new[] { "DWriteCreateFactory::factory" }),
            ("um/d3d9on12.h", new[] { "IDirect3DDevice9On12::GetD3D12Device::ppvDevice",
                "IDirect3DDevice9On12::UnwrapUnderlyingResource::ppvResource12" }),
            ("shared/d3d9.h", new[] { "IDirect3DSurface9::GetContainer::ppContainer",
                "IDirect3DVolume9::GetContainer::ppContainer" }),
            ("um/dcomp.h", new[] { "DCompositionCreateDevice::dcompositionDevice",
                "DCompositionCreateDevice2::dcompositionDevice", "DCompositionCreateDevice3::dcompositionDevice",
                "IDCompositionTexture::GetAvailableFence::availableFence", "IDCompositionSurface::BeginDraw::updateObject" }),
            ("um/Presentation.h", new[] { "CreatePresentationFactory::presentationFactory",
                "IPresentationManager::GetPresentRetiringFence::fence" }),
            ("um/dxcapi.h", new[] { "DxcCreateInstance::ppv", "DxcCreateInstance2::ppv",
                "IDxcContainerReflection::GetPartReflection::ppvObject", "IDxcUtils::CreateReflection::ppvReflection",
                "IDxcCompiler3::Compile::ppResult", "IDxcCompiler3::Disassemble::ppResult" }),
            ("winrt/windows.ui.composition.interop.h", new[] { "ICompositionTextureInterop::GetAvailableFence::availableFence" }),
        };
        internal const string RetvalBoundary = "IPresentationManager::GetPresentRetiringFence::fence";
        internal static readonly string[] AllSelectors = Owners.SelectMany(owner => owner.Selectors).ToArray();
        internal static readonly string[] Selectors = AllSelectors.Where(selector => selector != RetvalBoundary).ToArray();
        internal static readonly string[] RetainedCallbacks =
        {
            "DxcCreateInstanceProc::ppv", "DxcCreateInstance2Proc::ppv",
            "PFN_D3D12_CREATE_ROOT_SIGNATURE_DESERIALIZER::ppRootSignatureDeserializer",
            "PFN_D3D12_CREATE_VERSIONED_ROOT_SIGNATURE_DESERIALIZER::ppRootSignatureDeserializer",
            "PFN_D3D12_CREATE_VERSIONED_ROOT_SIGNATURE_DESERIALIZER_FROM_SUBOBJECT_IN_LIBRARY::ppRootSignatureDeserializer",
        };
        internal const string DWriteSelector = "DWriteCreateFactory::factory";

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]" + (selector == DWriteSelector ? "void**" : "") + "\n",
                    selector == DWriteSelector ? selector + "=void**\n" : "", StringComparison.Ordinal);
            return text;
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalBytesAndInheritedCopies()
            => AssertNativeEditBytes(Owners.Select(owner => owner.Path).Where(path =>
                path != "um/dwrite.h" && path != "um/d3d10.h" && path != "um/d3d12.h" && path != "um/Presentation.h")
                .Concat(new[] { "um/d3d10.idl", "um/d3d12.idl", "um/Presentation.idl" }), historical: false);

        [TestMethod]
        public void HistoricalGeneratedHeaders_OnlyChangeSelectedSalBytesAndInheritedCopies()
            => AssertNativeEditBytes(new[] { "um/d3d10.h", "um/d3d10_1.h", "um/d3d12.h", "um/Presentation.h" },
                historical: true);

        private static void AssertNativeEditBytes(IEnumerable<string> paths, bool historical)
        {
            foreach (string path in paths)
            {
                string original = ReadOriginal("RecompiledIdlHeaders/" + path, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), path);
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(PatchOwner(path, original)),
                    historical ? ProductionSalLocalHeaderTests.ReadBeforeCutover("RecompiledIdlHeaders/" + path) :
                        File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                            "generation", "WinSDK", "RecompiledIdlHeaders", path)),
                    "No encoding, newline, unrelated declaration or template changes: " + path);
            }
        }

        internal static string PatchOwner(string path, string text)
        {
            if (path == "um/d3d10.idl")
                return text.Replace("[annotation(\"_Out_opt_\")] void ** ppResource",
                    "[annotation(\"_COM_Outptr_opt_\")] void ** ppResource");
            if (path == "um/d3d10.h" || path == "um/d3d10_1.h")
                return text.Replace("_Out_opt_  void **ppResource", "_COM_Outptr_opt_  void **ppResource");
            if (path == "um/d3d12.idl")
                return Regex.Replace(text, @"cpp_quote\( ""HRESULT WINAPI D3D12Create(?:Versioned)?RootSignatureDeserializer\([\s\S]+?ppRootSignatureDeserializer\);"" \)",
                    match => match.Value.Replace("_Out_ void**", "_COM_Outptr_ void**"));
            if (path == "um/Presentation.idl")
                return text.Replace("_Outptr_ void ** presentationFactory", "_COM_Outptr_ void ** presentationFactory");
            foreach (string selector in Owners.Single(owner => owner.Path == path).Selectors.Where(selector => selector != RetvalBoundary))
            {
                if (selector == DWriteSelector) continue;
                string[] parts = selector.Split("::");
                string pattern = parts.Length == 2 ? FunctionPattern(parts[0]) : MethodPattern(parts[1]);
                string scope = parts.Length == 2 ? text : Definition(text, parts[0]);
                string patched = Regex.Replace(scope, pattern, match =>
                {
                    string parameter = parts.Last();
                    return Regex.Replace(match.Value,
                        @"(?:(?:_Out_|_Outptr_|_Outptr_result_maybenull_|__out)\s+)?(?<type>(?:void|LPVOID|ID2D1Factory)\s*\*+\s*\**\s*)" +
                            Regex.Escape(parameter) + @"\b",
                        output => (output.Value.StartsWith("_Outptr_result_maybenull_", StringComparison.Ordinal) ?
                            "_COM_Outptr_result_maybenull_ " : "_COM_Outptr_ ") + output.Groups["type"].Value + parameter);
                });
                text = parts.Length == 2 ? patched : text.Replace(scope, patched, StringComparison.Ordinal);
            }
            return text;
        }

        [TestMethod]
        public void CompleteGraphicsInventory_RetiresOnlyEquivalentFactsAndKeepsRepresentation()
        {
            string original = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = original.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(169, records.Length);
            Assert.AreEqual(25, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors)
                CollectionAssert.Contains(records, selector + "=[ComOutPtr]" + (selector == DWriteSelector ? "void**" : ""));
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(SyncComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(MediaFoundationComOutputHeaderTests.RetireRecords(RetireRecords(original))))))), current,
                "Preserve all independent sidecars exactly.");
            CollectionAssert.AreEqual(SyncComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(MediaFoundationComOutputHeaderTests.RetireRecords(RetireRecords(string.Join("\n", records) + "\n"))))))).Split('\n').Where(line => line.Length != 0 && line != ClrComOutputHeaderTests.CombinedSelector + "=[Out]").ToArray(),
                current.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal) ||
                    line == DWriteSelector + "=void**").ToArray());
            Assert.AreEqual(62, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            StringAssert.Contains(current, RetvalBoundary + "=[ComOutPtr]\n");
            foreach (string selector in RetainedCallbacks) StringAssert.Contains(current, selector + "=[ComOutPtr]\n");
            CollectionAssert.AreEqual(SyncComOutputHeaderTests.RetireRecords(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(original)))).Split('\n').Where(IndependentContract).ToArray(),
                current.Split('\n').Where(IndependentContract).ToArray());
            string parent = PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot());
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", parent, "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);

            var files = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", parent, "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, files.ExitCode, files.Output);
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (string partition in new[] { "Direct2D.Common", "Direct2D", "Direct3D", "Direct3D9", "Direct3D9on12",
                "Direct3D10", "Direct3D11", "Direct3D11on12", "Direct3D12", "Direct3DDxc", "Direct3DFxc",
                "DirectComp", "DirectWrite", "Dxgi.Common", "Dxgi", "CompositionSwapchain", "WinRT.Composition" })
            {
                foreach (string path in File.ReadAllLines(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                    "generation", "WinSDK", "Partitions", partition, "settings.rsp"))
                    .Where(line => line.StartsWith("<IncludeRoot>/", StringComparison.Ordinal)))
                {
                    if (path == "<IncludeRoot>/um/dciddi.h")
                    {
                        Assert.IsFalse(File.Exists(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                            "generation", "WinSDK", "RecompiledIdlHeaders", "um", "dciddi.h")),
                            "Do not restore the already absent SDK owner.");
                        continue;
                    }
                    string originalPath = files.Output.Split('\n').Select(line => line.TrimEnd('\r')).Single(line =>
                        line.Equals("generation/WinSDK/RecompiledIdlHeaders/" + path.Substring("<IncludeRoot>/".Length),
                            StringComparison.OrdinalIgnoreCase));
                    string text = ReadOriginal(originalPath.Substring("generation/WinSDK/".Length));
                    foreach (string record in records)
                    {
                        string selector = record.Split('=')[0];
                        string[] parts = selector.Split("::");
                        if (parts.Length == 2 ? Regex.IsMatch(text, @"\b" + Regex.Escape(parts[0]) + @"\s*\)?\s*\(") :
                            Definition(text, parts[0]) is string definition && definition.Length != 0 &&
                                Regex.IsMatch(definition, MethodPattern(parts[1])))
                            found.Add(selector);
                    }
                }
            }
            CollectionAssert.AreEquivalent(AllSelectors.Concat(RetainedCallbacks).ToArray(), found.ToArray(),
                "Audit the complete 169-record corpus against every shipped graphics traversal owner.");

            static bool IndependentContract(string line) => line.Contains("[Out]", StringComparison.Ordinal) ||
                line.Contains("[CanReturn", StringComparison.Ordinal);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedNativeDeclarations_PreserveEveryFinalMetadataByte(string target)
            => AssertShippedDeclarations(target, Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders"));

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeRetvalBoundary_EmitsOneCanonicalFactWithMatchingLegacyCom(string target)
        {
            string original = ReadOriginal("RecompiledIdlHeaders/um/Presentation.h");
            string declaration = Regex.Match(Definition(original, "IPresentationManager"),
                MethodPattern("GetPresentRetiringFence")).Value;
            Assert.AreNotEqual("", declaration);
            string header = @"typedef long HRESULT;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
#define STDMETHODCALLTYPE __stdcall
struct __declspec(uuid(""" + Uuid(original, "IPresentationManager") + @""")) IPresentationManager {
" + declaration + "\n};";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit(new Dictionary<string, string> { [RetvalBoundary] = "[ComOutPtr]" });
            byte[] baseline = File.ReadAllBytes(output);
            string saved = Path.ChangeExtension(output, "old.winmd");
            File.Copy(output, saved);
            result = fixture.Scrape(header.Replace("_Out_retval_", "_COM_Outptr_"), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            output = fixture.Emit();
            using var oldPe = new PEReader(new MemoryStream(baseline));
            using var newPe = new PEReader(File.OpenRead(output));
            var oldReader = oldPe.GetMetadataReader();
            var newReader = newPe.GetMetadataReader();
            var oldMethod = Method(oldReader);
            var newMethod = Method(newReader);
            Assert.AreEqual(oldMethod.Attributes, newMethod.Attributes);
            Assert.AreEqual(oldMethod.ImplAttributes, newMethod.ImplAttributes);
            CollectionAssert.AreEqual(oldReader.GetBlobBytes(oldMethod.Signature), newReader.GetBlobBytes(newMethod.Signature));
            var oldParameters = oldMethod.GetParameters().Select(oldReader.GetParameter).ToArray();
            var newParameters = newMethod.GetParameters().Select(newReader.GetParameter).ToArray();
            CollectionAssert.AreEqual(oldParameters.Select(parameter => parameter.Attributes).ToArray(),
                newParameters.Select(parameter => parameter.Attributes).ToArray());
            string[] oldAttributes = Attributes(oldReader, oldParameters.Last());
            string[] newAttributes = Attributes(newReader, newParameters.Last());
            CollectionAssert.AreEqual(new[] { "ComOutPtrAttribute:01000000", "RetValAttribute:01000000" }, oldAttributes);
            CollectionAssert.AreEqual(oldAttributes, newAttributes);
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(output),
                "Authoritative native RetVal is singleton and canonical with either legacy COM or stock COM SAL.");
            var comparison = LastErrorHeaderTests.Run("dotnet", Path.Combine(AppContext.BaseDirectory, "WinmdUtils.dll"),
                "compare", "--first", saved, "--second", output);
            Assert.AreEqual(0, comparison.ExitCode, comparison.Output);
            StringAssert.Contains(comparison.Output, "No unknown differences found.", "Projected API shape is unchanged.");

            static MethodDefinition Method(MetadataReader reader) => reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "GetPresentRetiringFence");
            static string[] Attributes(MetadataReader reader, Parameter parameter) => parameter.GetCustomAttributes()
                .Select(reader.GetCustomAttribute).Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute) +
                    ":" + Convert.ToHexString(reader.GetBlobBytes(attribute.Value))).ToArray();
        }

        internal static void AssertShippedDeclarations(string target, string headers)
        {
            const string prefix = @"
#define WINAPI __stdcall
#define EXTERN_C extern ""C""
#define STDMETHODCALLTYPE __stdcall
#define STDAPI HRESULT __stdcall
#define DWRITE_EXPORT __stdcall
#define WIN_NOEXCEPT noexcept
#define DXC_API_IMPORT
#define CONST const
#define PURE = 0
#define THIS_
#define STDMETHOD(name) virtual HRESULT __stdcall name
#define IFACEMETHOD(name) virtual HRESULT __stdcall name
#define STDMETHOD_(type, name) virtual type __stdcall name
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned long UINT;
typedef unsigned int UINT32;
typedef unsigned long long UINT64;
typedef __SIZE_TYPE__ SIZE_T;
typedef const void* LPCVOID;
typedef void* LPVOID;
typedef void* HANDLE;
typedef const unsigned short* LPCWSTR;
struct GUID { unsigned long Data1; unsigned short Data2, Data3; unsigned char Data4[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFCLSID;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IMalloc : IUnknown {};
struct IDXGIDevice : IUnknown {};
struct IDirect3DResource9 : IUnknown {};
struct ID3D12CommandQueue : IUnknown {};
struct IDxcIncludeHandler : IUnknown {};
struct ID2D1Factory : IUnknown {};
struct DxcBuffer { const void* Ptr; SIZE_T Size; UINT32 Encoding; };
struct RECT { long left, top, right, bottom; };
struct POINT { long x, y; };
enum D2D1_FACTORY_TYPE { D2D1_FACTORY_TYPE_SINGLE_THREADED = 0 };
enum DWRITE_FACTORY_TYPE { DWRITE_FACTORY_TYPE_SHARED = 0 };
struct D2D1_FACTORY_OPTIONS { int debugLevel; };
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = AllSelectors.ToDictionary(selector => selector,
                selector => "[ComOutPtr]" + (selector == DWriteSelector ? "void**" : ""), StringComparer.Ordinal);
            var representation = new Dictionary<string, string> { [DWriteSelector] = "void**", [RetvalBoundary] = "[ComOutPtr]" };
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            string imports = "--with-librarypath\n" + string.Join("\n", Selectors.Where(selector => selector.Split("::").Length == 2)
                .Select(selector => mappings.Split('\n').Single(line => line.StartsWith(selector.Split("::")[0] + "=", StringComparison.Ordinal)).Trim()));
            var result = fixture.Scrape(prefix + Declarations(true), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(false), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit(representation);
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));

            // Native DWrite remains IUnknown**; only its independently configured representation is void**.
            string typed = fixture.Emit(new Dictionary<string, string> { [RetvalBoundary] = "[ComOutPtr]" });
            Assert.IsFalse(expected.SequenceEqual(File.ReadAllBytes(typed)), "Do not silently remove DWrite's independent type override.");

            string Declarations(bool legacy)
            {
                string declarations = "";
                foreach (var owner in Owners)
                {
                    string text = legacy ? ReadOriginal("RecompiledIdlHeaders/" + owner.Path) :
                        File.ReadAllText(Path.Combine(headers, owner.Path));
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
                            Assert.AreNotEqual("", definition, group.Key);
                            string uuid = Uuid(text, group.Key);
                            declarations += "struct __declspec(uuid(\"" + uuid + "\")) " + group.Key + " : public IUnknown {\n";
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
                string name = reader.GetString(method.Name);
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
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
                    if (!AllSelectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(selector == "ID3D10Device::OpenSharedResource::ppResource" ?
                        ParameterAttributes.Out | ParameterAttributes.Optional : ParameterAttributes.Out, parameter.Attributes, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    Assert.AreEqual(selector == "IPresentationManager::GetPresentRetiringFence::fence",
                        attributes.Any(a => ParameterHeaderTests.AttributeName(reader, a) == "RetValAttribute"), selector);
                    var attribute = attributes.Single(a => ParameterHeaderTests.AttributeName(reader, a) == "ComOutPtrAttribute");
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attribute.Value), selector);
                    byte[] type = reader.GetBlobBytes(method.Signature).Skip(start).Take(signature.Offset - start).ToArray();
                    if (selector == "ID2D1Resource::GetFactory::factory")
                    {
                        var typed = reader.GetBlobReader(method.Signature);
                        typed.Offset = start;
                        Assert.AreEqual(SignatureTypeCode.Pointer, typed.ReadSignatureTypeCode(), Convert.ToHexString(type));
                        Assert.AreEqual(SignatureTypeCode.TypeHandle, typed.ReadSignatureTypeCode(), Convert.ToHexString(type));
                        var typeHandle = (TypeDefinitionHandle)typed.ReadTypeHandle();
                        Assert.AreEqual("ID2D1Factory", reader.GetString(reader.GetTypeDefinition(typeHandle).Name));
                        Assert.AreEqual(signature.Offset, typed.Offset);
                    }
                    else CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 }, type, selector);
                }
                Assert.AreEqual(0, signature.RemainingBytes);
            }
            CollectionAssert.AreEquivalent(AllSelectors, found.ToArray());

            static void ReadType(ref BlobReader signature)
            {
                SignatureTypeCode type = signature.ReadSignatureTypeCode();
                if (type == SignatureTypeCode.Pointer || type == SignatureTypeCode.ByReference) ReadType(ref signature);
                else if (type == SignatureTypeCode.TypeHandle) signature.ReadTypeHandle();
                else Assert.IsTrue(type >= SignatureTypeCode.Void && type <= SignatureTypeCode.Object, type.ToString());
            }
        }

        internal static string FunctionPattern(string name) =>
            @"(?:HRESULT\s+(?:WINAPI|__stdcall)|STDAPI|EXTERN_C HRESULT DWRITE_EXPORT)\s+" + Regex.Escape(name) + @"\s*\([^;{}]+;";

        internal static string MethodPattern(string name) =>
            @"(?:virtual\s+(?:HRESULT|void)\s+STDMETHODCALLTYPE\s+" + Regex.Escape(name) + @"\s*\(|" +
            @"(?:STDMETHOD|IFACEMETHOD)\(" + Regex.Escape(name) + @"\)\s*\(|STDMETHOD_\(void,\s*" + Regex.Escape(name) + @"\)\s*\()[^;]+;";

        internal static string Definition(string text, string type) => Regex.Match(text,
            @"(?:\b" + Regex.Escape(type) + @"\s*:\s*public\s+\w+|" +
            @"DECLARE_INTERFACE(?:_IID)?_\(\s*" + Regex.Escape(type) + @",[^)]+\))\s*\{[\s\S]+?\n\s*\};").Value;

        private static string Uuid(string text, string type)
        {
            string guid = @"[0-9A-Fa-f-]{36}";
            Match match = Regex.Match(text, @"(?:MIDL_INTERFACE|DX_DECLARE_INTERFACE|DECLSPEC_UUID)\(""(?<uuid>" + guid +
                @")""\)\s*" + Regex.Escape(type) + @"\b|DECLARE_INTERFACE_IID_\(" + Regex.Escape(type) +
                @",[^,]+,\s*""(?<uuid>" + guid + @")""\)|CROSS_PLATFORM_UUIDOF\(" + Regex.Escape(type) + @",\s*""(?<uuid>" + guid + @")""\)");
            Assert.IsTrue(match.Success, type);
            return match.Groups["uuid"].Value;
        }
    }
}
