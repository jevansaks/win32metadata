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
    public class ImagingComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/ImagingComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the imaging output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static byte[] ReadOriginalBytes(string path) => PropertyComOutputHeaderTests.ReadOriginalBytes(
            path, PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()));

        internal static string ReadOriginal(string path) => Encoding.Latin1.GetString(ReadOriginalBytes(path));

        internal static readonly (string Path, string[] Selectors)[] Owners =
        {
            ("um/wincodecsdk.h", new[] { "IWICMetadataReaderInfo::CreateInstance::ppIReader" }),
            ("um/wincodec.h", new[] { "IWICBitmapDecoderInfo::CreateInstance::ppIBitmapDecoder",
                "IWICBitmapEncoderInfo::CreateInstance::ppIBitmapEncoder",
                "IWICD3DTextureSource::GetTexture::ppTexture", "IWICD3DTextureSource::GetTransformedTexture::ppTexture" }),
            ("um/Sti.h", new[] { "StiCreateInstanceW::ppSti" }),
            ("um/wia_lh.h", new[] { "IWiaItem2::GetExtension::ppOut" }),
            ("um/PhotoAcquire.h", new[] { "IPhotoAcquireSource::BindToObject::ppv" }),
            ("um/CommCtrl.h", new[] { "ImageList_ReadEx::ppv", "HIMAGELIST_QueryInterface::ppv" }),
            ("um/commoncontrols.h", new[] { "ImageList_CoCreateInstance::ppv", "IImageList::Clone::ppv",
                "IImageList::GetDragImage::ppv", "IImageList::Merge::ppv", "IImageList2::GetCallback::ppv" }),
        };
        internal static readonly string[] Idls = { "wincodecsdk.idl", "wincodec.idl", "commoncontrols.idl" };
        internal static readonly string[] Selectors = Owners.SelectMany(owner => owner.Selectors).ToArray();

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]\n", "", StringComparison.Ordinal);
            return text;
        }

        [TestMethod]
        public void CompleteTraversal_AuditsAllEightyEightParentRecordsAndPreservesIndependentFacts()
        {
            string parent = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = parent.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(88, records.Length);
            Assert.AreEqual(15, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(SyncComOutputHeaderTests.RetireRecords(RetireRecords(parent)), current, "Preserve the complete independent sidecar exactly.");
            CollectionAssert.AreEqual(SyncComOutputHeaderTests.RetireRecords(RetireRecords(parent)).Split('\n').Where(Contract).ToArray(),
                current.Split('\n').Where(Contract).ToArray());
            Assert.AreEqual(62, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            foreach (string selector in ClrComOutputHeaderTests.RetainedSelectors)
                StringAssert.Contains(current, selector + "=" +
                    (selector == ClrComOutputHeaderTests.CombinedSelector ? "[Out]" : "") + "[ComOutPtr]\n");
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            var inventory = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, inventory.ExitCode, inventory.Output);
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string partition in new[] { "Wic", "Wia", "ImagingDevice", "PicAcq", "Controls" })
                foreach (string include in File.ReadAllLines(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                    "generation", "WinSDK", "Partitions", partition, "settings.rsp"))
                    .Where(line => line.StartsWith("<IncludeRoot>/", StringComparison.Ordinal)))
                {
                    if (include == "<IncludeRoot>/um/wiamdef--.h")
                    {
                        Assert.IsFalse(inventory.Output.Contains("/wiamdef--.h", StringComparison.OrdinalIgnoreCase),
                            "Existing disabled traversal is not a removed API to restore.");
                        continue;
                    }
                    paths.Add(inventory.Output.Split('\n').Select(line => line.TrimEnd('\r')).Single(line =>
                        line.Equals("generation/WinSDK/RecompiledIdlHeaders/" + include.Substring("<IncludeRoot>/".Length),
                            StringComparison.OrdinalIgnoreCase)));
                }
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (string path in paths)
            {
                string text = ReadOriginal(path.Substring("generation/WinSDK/".Length));
                foreach (string record in records)
                {
                    string selector = record.Split('=')[0];
                    string[] parts = selector.Split("::");
                    if (parts.Length == 3 && Regex.IsMatch(AudioComOutputHeaderTests.Definition(text, parts[0]), MethodPattern(parts[1])))
                        found.Add(selector);
                    else if (parts.Length == 2 && Regex.IsMatch(text, FunctionPattern(parts[0])))
                        found.Add(selector);
                }
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray(), "Complete physical traversals, not a prefix sample.");
            string inline = ReadOriginal("RecompiledIdlHeaders/um/commctrl.inl");
            CollectionAssert.AreEqual(ReadOriginalBytes("RecompiledIdlHeaders/um/commctrl.inl"),
                File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", "commctrl.inl")));
            Assert.IsTrue(inline.Contains("IsolationAwareImageList_ReadEx", StringComparison.Ordinal));
            Assert.IsTrue(inline.Contains("IsolationAwareHIMAGELIST_QueryInterface", StringComparison.Ordinal));

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
                string before = Encoding.Latin1.GetString(ReadOriginalBytes("RecompiledIdlHeaders/" + owner.Path));
                string after = PatchHeader(owner.Path, before);
                sites += Regex.Matches(after, "_COM_Outptr_").Count - Regex.Matches(before, "_COM_Outptr_").Count;
                CollectionAssert.AreEqual(Encoding.Latin1.GetBytes(after), File.ReadAllBytes(Path.Combine(
                    LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders",
                    owner.Path.Replace('/', Path.DirectorySeparatorChar))), owner.Path);
            }
            Assert.AreEqual(29, sites);
            foreach (string idl in Idls)
                CollectionAssert.AreEqual(Encoding.Latin1.GetBytes(PatchIdl(idl, Encoding.Latin1.GetString(
                    ReadOriginalBytes("RecompiledIdlHeaders/um/" + idl)))),
                    File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                        "RecompiledIdlHeaders", "um", idl)), idl);
        }

        internal static string PatchIdl(string name, string text) => name switch
        {
            "wincodecsdk.idl" => text.Replace("[out] IWICMetadataReader **ppIReader);",
                "[out, annotation(\"_COM_Outptr_\")] IWICMetadataReader **ppIReader);", StringComparison.Ordinal),
            "wincodec.idl" => text.Replace("[out] IWICBitmapDecoder **ppIBitmapDecoder",
                    "[out, annotation(\"_COM_Outptr_\")] IWICBitmapDecoder **ppIBitmapDecoder", StringComparison.Ordinal)
                .Replace("[out] IWICBitmapEncoder **ppIBitmapEncoder",
                    "[out, annotation(\"_COM_Outptr_\")] IWICBitmapEncoder **ppIBitmapEncoder", StringComparison.Ordinal)
                .Replace("[annotation(\"_Outptr_\")] void** ppTexture",
                    "[annotation(\"_COM_Outptr_\")] void** ppTexture", StringComparison.Ordinal),
            "commoncontrols.idl" => text.Replace("_Outptr_ void **ppv", "_COM_Outptr_ void **ppv", StringComparison.Ordinal)
                .Replace("annotation(\"_Outptr_\")] void** ppv", "annotation(\"_COM_Outptr_\")] void** ppv", StringComparison.Ordinal),
            _ => throw new ArgumentOutOfRangeException(nameof(name)),
        };

        internal static string PatchHeader(string path, string text)
        {
            foreach (string selector in Owners.Single(owner => owner.Path == path).Selectors)
            {
                string[] parts = selector.Split("::");
                string pattern = parts.Length == 2 ? FunctionPattern(parts[0]) :
                    @"(?:virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE " + parts[1] +
                        @"\s*\(|HRESULT\s*\(\s*STDMETHODCALLTYPE\s*\*" + parts[1] + @"\s*\)\s*\()[^;]+;";
                if (parts.Length == 3)
                {
                    string[] scopes = parts[0] == "IImageList" ? new[] { parts[0], "IImageList2" } : new[] { parts[0] };
                    foreach (string scope in scopes)
                        text = Regex.Replace(text, @"#ifndef __" + scope + @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" +
                            scope + @"_INTERFACE_DEFINED__ \*/", match => Replace(match.Value));
                }
                else text = Replace(text);

                string Replace(string scope) => Regex.Replace(scope, pattern, match =>
                {
                    string parameter = @"(?<!\w)(?<sal>_Out_|_Outptr_|__RPC__deref_out_opt)(?<space>\s+)" +
                        @"(?<type>[\w]+\s*\*+\s*\**\s*)" + parts[^1] + @"\b";
                    string changed = Regex.Replace(match.Value, parameter, value =>
                        (value.Groups["sal"].Value == "__RPC__deref_out_opt" ? "_COM_Outptr_result_maybenull_" : "_COM_Outptr_") +
                        value.Value.Substring(value.Groups["sal"].Length));
                    if (path == "um/Sti.h")
                        changed = changed.Replace("interface IStillImageW **ppSti", "_COM_Outptr_ interface IStillImageW **ppSti", StringComparison.Ordinal);
                    if (path == "um/wia_lh.h")
                        changed = changed.Replace("void **ppOut", "_COM_Outptr_ void **ppOut", StringComparison.Ordinal);
                    return changed;
                });
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
#define STDMETHODIMP HRESULT __stdcall
#define WINAPI __stdcall
#define WINCOMMCTRLAPI
#define interface struct
#define __RPC__in _In_
#define __RPC__deref_out_opt _Outptr_result_maybenull_
typedef long HRESULT;
typedef long LONG;
typedef unsigned long DWORD;
typedef unsigned int UINT;
typedef void* PVOID;
typedef void* HINSTANCE;
typedef void* HIMAGELIST;
typedef wchar_t* BSTR;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFCLSID;
typedef GUID WICPixelFormatGUID;
typedef int WICBitmapTransformOptions;
struct POINT { long x, y; };
struct WICRect;
struct IPropertyBag2;
struct IStream;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
typedef IUnknown* LPUNKNOWN;
struct IWICMetadataReader : IUnknown {};
struct IWICBitmapDecoder : IUnknown {};
struct IWICBitmapEncoder : IUnknown {};
struct IStillImageW : IUnknown {};
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
                        if (group.First().Split("::").Length == 2)
                        {
                            foreach (string selector in group)
                            {
                                var matches = Regex.Matches(text, FunctionPattern(selector.Split("::")[0]));
                                Assert.AreEqual(1, matches.Count, selector);
                                declarations += matches[0].Value + "\n";
                            }
                            continue;
                        }
                        string definition = AudioComOutputHeaderTests.Definition(text, group.Key);
                        Assert.IsTrue(definition.Length > 0, group.Key);
                        Match uuid = Regex.Match(definition, @"MIDL_INTERFACE\(""(?<uuid>[^""]+)""\)");
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
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                var parameters = method.GetParameters().Select(reader.GetParameter).Where(parameter => parameter.SequenceNumber != 0).ToArray();
                Assert.AreEqual(parameters.Length, signature.ReadCompressedInteger());
                ReadType(ref signature);
                foreach (var parameter in parameters)
                {
                    int start = signature.Offset;
                    ReadType(ref signature);
                    string selector = (method.Attributes.HasFlag(MethodAttributes.PinvokeImpl) ? name : owner + "::" + name) +
                        "::" + reader.GetString(parameter.Name);
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    Assert.AreEqual(MethodImplAttributes.IL, method.ImplAttributes & MethodImplAttributes.PreserveSig, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    CollectionAssert.AreEqual(new[] { "ComOutPtrAttribute" },
                        attributes.Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray(), selector);
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attributes.Single().Value), selector);
                    byte[] type = reader.GetBlobBytes(method.Signature).Skip(start).Take(signature.Offset - start).ToArray();
                    string? typed = selector switch
                    {
                        "IWICMetadataReaderInfo::CreateInstance::ppIReader" => "IWICMetadataReader",
                        "IWICBitmapDecoderInfo::CreateInstance::ppIBitmapDecoder" => "IWICBitmapDecoder",
                        "IWICBitmapEncoderInfo::CreateInstance::ppIBitmapEncoder" => "IWICBitmapEncoder",
                        "StiCreateInstanceW::ppSti" => "IStillImageW",
                        _ => null,
                    };
                    if (typed == null) CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 }, type, selector);
                    else
                    {
                        var typedSignature = reader.GetBlobReader(method.Signature);
                        typedSignature.Offset = start;
                        Assert.AreEqual(0x0f, typedSignature.ReadByte(), selector);
                        Assert.AreEqual(0x12, typedSignature.ReadByte(), selector);
                        var typeHandle = typedSignature.ReadTypeHandle();
                        Assert.AreEqual(HandleKind.TypeDefinition, typeHandle.Kind, selector);
                        Assert.AreEqual(typed, reader.GetString(reader.GetTypeDefinition((TypeDefinitionHandle)typeHandle).Name), selector);
                    }
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

        private static string MethodPattern(string name) =>
            @"virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE " + Regex.Escape(name) + @"\s*\([^;]+;";

        private static string FunctionPattern(string name) =>
            @"(?:WINCOMMCTRLAPI HRESULT WINAPI|STDMETHODIMP)\s+" + Regex.Escape(name) + @"\s*\([^;]+;";
    }
}
