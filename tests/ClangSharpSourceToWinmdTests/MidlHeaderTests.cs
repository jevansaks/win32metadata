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
    public class MidlHeaderTests
    {
        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ProductionSalLocal_ActualNativeDirectionsAndElementCountReachFinalMetadata(string target)
        {
            const ParameterAttributes inout = ParameterAttributes.In | ParameterAttributes.Out;
            const ParameterAttributes optionalIn = ParameterAttributes.In | ParameterAttributes.Optional;
            var cases = new (string Header, string Owner, string Method, string Parameter, ParameterAttributes Before,
                ParameterAttributes After)[]
            {
                ("d3d12.h", "ID3D12CommandQueue1", "GetProcessPriority", "pOutValue", inout, ParameterAttributes.In),
                ("d3d12.h", "ID3D12CommandQueue1", "GetGlobalPriority", "pOutValue", inout, ParameterAttributes.In),
                ("d3d12.h", "ID3D12CommandQueue", "Signal", "pFence", inout, optionalIn),
                ("d3d12.h", "ID3D12CommandQueue", "Wait", "pFence", inout, optionalIn),
                ("wmsdkidl.h", "IWMProfileManager2", "GetSystemProfileVersion", "pdwVersion", inout, ParameterAttributes.In),
                ("wmsdkidl.h", "IWMProfileManagerLanguage", "GetUserLanguageID", "wLangID", inout, ParameterAttributes.In),
                ("wmsdkidl.h", "IWMWriterPostView", "SetPostViewCallback", "pCallback", inout, optionalIn),
                ("wmsdkidl.h", "IWMWriterPostView", "SetPostViewCallback", "pvContext", inout, ParameterAttributes.In),
                ("wmsdkidl.h", "IWMDRMTranscryptor", "Initialize", "pbLicenseRequestMsg", inout, ParameterAttributes.In),
                ("strmif.h", "IAMTVAudio", "UnRegisterNotificationCallBack", "pNotify", inout, optionalIn),
                ("strmif.h", "IGraphConfigCallback", "Reconfigure", "pvContext", inout, ParameterAttributes.In),
                ("strmif.h", "ICodecAPI", "SetAllSettingsWithNotify", "__MIDL__ICodecAPI0002", inout, optionalIn),
                ("mfidl.h", "IMFVideoSampleAllocatorNotifyEx", "NotifyPrune",
                    "__MIDL__IMFVideoSampleAllocatorNotifyEx0000", inout, optionalIn),
            };
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string prefix = @"
#define STDMETHODCALLTYPE __stdcall
#include <rpcsal.h>
typedef long HRESULT;
typedef unsigned short WORD;
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef void* PVOID;
typedef unsigned long long UINT64;
typedef unsigned char BYTE;
typedef unsigned short* BSTR;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
struct IUnknown {};
struct IStream {};
struct IMFSample {};
struct ID3D12Fence {};
struct IWMWriterPostViewCallback {};
struct INSSBuffer {};
struct IWMStatusCallback {};
struct IAMTunerNotification {};
";
            foreach (var type in new[] { ("d3d12.h", "D3D12_COMMAND_QUEUE_PROCESS_PRIORITY"),
                ("d3d12.h", "D3D12_COMMAND_QUEUE_GLOBAL_PRIORITY"), ("wmsdkidl.h", "WMT_VERSION") })
            {
                Match declaration = Regex.Match(File.ReadAllText(Path.Combine(headers, type.Item1)),
                    @"typedef\s+enum " + type.Item2 + @"\s*\{[^}]+\}\s*" + type.Item2 + @";");
                Assert.IsTrue(declaration.Success, type.Item2);
                prefix += declaration.Value + "\n";
            }
            var signatures = new Dictionary<string, byte[]>();
            var parameters = new Dictionary<string, (ParameterAttributes Flags, string[] Facts)>();
            foreach (bool production in new[] { false, true })
            {
                string declarations = prefix;
                foreach (var group in cases.GroupBy(c => (c.Header, c.Owner)))
                {
                    string text = production ? File.ReadAllText(Path.Combine(headers, group.Key.Header)) :
                        Encoding.Latin1.GetString(ProductionSalLocalHeaderTests.ReadBeforeCutover(
                            "RecompiledIdlHeaders/um/" + group.Key.Header));
                    string definition = AudioComOutputHeaderTests.Definition(text, group.Key.Owner);
                    Match uuid = Regex.Match(definition, @"MIDL_INTERFACE\(""(?<uuid>[^""]+)""\)");
                    Assert.IsTrue(uuid.Success, group.Key.Owner);
                    declarations += "struct __declspec(uuid(\"" + uuid.Groups["uuid"].Value + "\")) " +
                        group.Key.Owner + " : public IUnknown {\n";
                    foreach (string method in group.Select(c => c.Method).Distinct())
                    {
                        Match declaration = Regex.Match(definition, @"virtual HRESULT STDMETHODCALLTYPE " + method + @"\s*\([^;]+;");
                        Assert.IsTrue(declaration.Success, group.Key.Owner + "::" + method);
                        declarations += declaration.Value + "\n";
                    }
                    declarations += "};\n";
                }
                using var fixture = new LastErrorHeaderTests.Fixture();
                var result = fixture.Scrape(declarations, target, sal: true,
                    extraSettings: "--include-directory\n" + SharedHeaders() + "\n");
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string output = fixture.Emit();
                Console.WriteLine("RAW_OUTPUT production-local-sal." + target + "." +
                    (production ? "production" : "held") + ".winmd " + Convert.ToBase64String(File.ReadAllBytes(output)));
                using var pe = new PEReader(File.OpenRead(output));
                var reader = pe.GetMetadataReader();
                var found = new HashSet<string>();
                foreach (var method in reader.MethodDefinitions.Select(reader.GetMethodDefinition))
                {
                    string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                    if (!cases.Any(c => c.Owner == owner)) continue;
                    string selector = owner + "::" + reader.GetString(method.Name);
                    if (!production) signatures.Add(selector, reader.GetBlobBytes(method.Signature));
                    else CollectionAssert.AreEqual(signatures[selector], reader.GetBlobBytes(method.Signature), selector);
                    foreach (var parameter in method.GetParameters().Select(reader.GetParameter).Where(p => p.SequenceNumber != 0))
                    {
                        string name = reader.GetString(parameter.Name);
                        string key = selector + "::" + name;
                        var facts = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                            .Select(a => ParameterHeaderTests.AttributeName(reader, a) + ":" + Convert.ToHexString(reader.GetBlobBytes(a.Value))).ToArray();
                        var expected = cases.Where(c => c.Owner + "::" + c.Method + "::" + c.Parameter == key).ToArray();
                        if (expected.Length == 1)
                        {
                            Assert.IsTrue(found.Add(key));
                            Assert.AreEqual(production ? expected[0].After : expected[0].Before, parameter.Attributes, key);
                            Console.WriteLine("LOCAL_SAL_FINAL " + key + " " + (production ? "production" : "held") +
                                " flags=" + parameter.Attributes + " facts=" + string.Join(",", facts));
                            if (key == "IWMDRMTranscryptor::Initialize::pbLicenseRequestMsg" && production)
                            {
                                AssertCount(reader, parameter, "NativeArrayInfoAttribute", "CountParamIndex", 2, shortValue: true);
                                CollectionAssert.AreEqual(new[] { "NativeArrayInfoAttribute" },
                                    parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                                        .Select(a => ParameterHeaderTests.AttributeName(reader, a)).ToArray(), key);
                            }
                            else CollectionAssert.AreEqual(Array.Empty<string>(), facts, key);
                        }
                        if (!production) parameters.Add(key, (parameter.Attributes, facts));
                        else if (expected.Length == 0)
                        {
                            Assert.AreEqual(parameters[key].Flags, parameter.Attributes, key);
                            CollectionAssert.AreEqual(parameters[key].Facts, facts, key);
                        }
                    }
                }
                CollectionAssert.AreEquivalent(cases.Select(c => c.Owner + "::" + c.Method + "::" + c.Parameter).ToArray(), found.ToArray());
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        public void NativeRetval_BindsExactParametersInMethodsVtablesAndCallbacks(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string common = @"
typedef long (__stdcall *Callback)(
    /* [in] */ _In_ const long* input,
    /* [retval][out] */ _Out_ long** result);
struct Vtable {
    long (__stdcall *Value)(void* self, /* [out] */ _Out_ long* event,
        /* [retval][out] */ _Out_ long* result);
};
";
            string method = language == "c++" ? @"
struct __declspec(uuid(""64abea93-aac7-4f3b-a257-976deba89f04"")) IExample {
    virtual long __stdcall Value(/* [out] */ _Out_ long* neighbor,
        /* [retval][out] */ _Out_ long* result) = 0;
};" : "";
            var result = fixture.Scrape(common + method, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine(File.ReadAllText(fixture.GeneratedSource));
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var marked = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .SelectMany(m => m.GetParameters().Select(reader.GetParameter))
                .Where(p => p.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(a => ParameterHeaderTests.AttributeName(reader, a) == "RetValAttribute")).ToArray();
            Assert.AreEqual(language == "c++" ? 3 : 2, marked.Length,
                "Each real callback/vtable Invoke and C++ method receives its own native fact.");
            Assert.IsTrue(marked.All(p => reader.GetString(p.Name) == "result"));
            foreach (var parameter in marked)
            {
                Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes);
                ParameterHeaderTests.AssertAttribute(reader, parameter, "RetValAttribute");
                CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 },
                    reader.GetBlobBytes(reader.GetCustomAttribute(parameter.GetCustomAttributes().Single()).Value));
            }
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Any(t => reader.GetString(t.Name).Contains("_WIN32META_", StringComparison.Ordinal)));
            var vtable = NativeTypedefHeaderTests.Type(reader, "Vtable");
            Assert.AreEqual(TypeAttributes.SequentialLayout, vtable.Attributes & TypeAttributes.LayoutMask);
            Assert.IsFalse(vtable.Attributes.HasFlag(TypeAttributes.Interface));
            var slot = reader.GetFieldDefinition(vtable.GetFields().Single());
            Assert.AreEqual("Value", reader.GetString(slot.Name));
            var slotType = reader.GetBlobReader(slot.Signature);
            slotType.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.TypeHandle, slotType.ReadSignatureTypeCode());
            var handle = slotType.ReadTypeHandle();
            Assert.AreEqual(HandleKind.TypeReference, handle.Kind);
            var reference = reader.GetTypeReference((TypeReferenceHandle)handle);
            Assert.AreEqual("Vtable_Value", reader.GetString(reference.Name));
            Assert.AreEqual("Test", reader.GetString(reference.Namespace));
            var delegateType = NativeTypedefHeaderTests.Type(reader, "Vtable_Value");
            Assert.AreEqual("Vtable_Value", reader.GetString(delegateType.Name));
            AssertCallingConvention(reader, delegateType, target.StartsWith("i686", StringComparison.Ordinal) ? 1 : 2);
            var invoke = delegateType.GetMethods().Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Invoke");
            CollectionAssert.AreEqual(new[] { "self", "event", "result" }, invoke.GetParameters()
                .Select(reader.GetParameter).Where(p => p.SequenceNumber != 0).Select(p => reader.GetString(p.Name)).ToArray());
            var signature = reader.GetBlobReader(invoke.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(3, signature.ReadCompressedInteger());
            foreach (var code in new[] { SignatureTypeCode.Int32, SignatureTypeCode.Pointer, SignatureTypeCode.Void,
                SignatureTypeCode.Pointer, SignatureTypeCode.Int32, SignatureTypeCode.Pointer, SignatureTypeCode.Int32 })
                Assert.AreEqual(code, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
            var callbackType = NativeTypedefHeaderTests.Type(reader, "Callback");
            AssertCallingConvention(reader, callbackType, 1);
            var callback = callbackType.GetMethods().Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Invoke");
            signature = reader.GetBlobReader(callback.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(2, signature.ReadCompressedInteger());
            foreach (var code in new[] { SignatureTypeCode.Int32, SignatureTypeCode.Pointer, SignatureTypeCode.Int32,
                SignatureTypeCode.Pointer, SignatureTypeCode.Pointer, SignatureTypeCode.Int32 })
                Assert.AreEqual(code, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        internal static void AssertCallingConvention(MetadataReader reader, TypeDefinition type, int expected)
        {
            var attribute = type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(a => ParameterHeaderTests.AttributeName(reader, a) == "UnmanagedFunctionPointerAttribute");
            var blob = reader.GetBlobReader(attribute.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual(expected, blob.ReadInt32(), reader.GetString(type.Name));
            Assert.AreEqual((ushort)0, blob.ReadUInt16());
            Assert.AreEqual(0, blob.RemainingBytes);
        }

        [TestMethod]
        public void Retval_NeighboringCommentsAndLiteralsDoNotSelectAParameter()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"/* [retval] */ long __stdcall Value(
    /* ordinary prose [retval] */ _Out_ long* neighbor,
    _Out_ long* result);
typedef long (__stdcall *Callback)(_Out_ long* value);
long __stdcall Trailing(_Out_ long* neighbor /* [retval][out] */, _Out_ long* result);
", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute)
                .Any(a => ParameterHeaderTests.AttributeName(reader, a) == "RetValAttribute"));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void RawRpc_PreservesPartialCapacityDirectionAndIndependentStorageOptionality(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"#include <rpcsal.h>
long __stdcall Example(
    __RPC__inout_ecount_part_opt(capacity, *written) unsigned char* buffer,
    unsigned long capacity,
    __RPC__inout unsigned long* written,
    __RPC__deref_out_opt void** resultNullable,
    __RPC__deref_opt_inout_opt void** storageOptional,
    __RPC__in_ecount(3) const long* fixedInput,
    _Out_writes_bytes_to_opt_(byteCapacity, *byteWritten) void* bytes,
    unsigned long byteCapacity, _Out_ unsigned long* byteWritten);", target, sal: true,
                extraSettings: "--include-directory\n" + SharedHeaders() + "\n");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine(File.ReadAllText(fixture.GeneratedSource));
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            var parameters = method.GetParameters().Select(reader.GetParameter).Where(p => p.SequenceNumber != 0).ToArray();
            Console.WriteLine(string.Join("\n", parameters.Select(p => $"{p.SequenceNumber} {reader.GetString(p.Name)} {p.Attributes}")));
            CollectionAssert.AreEqual(new[] { ParameterAttributes.In | ParameterAttributes.Out | ParameterAttributes.Optional,
                ParameterAttributes.In, ParameterAttributes.In | ParameterAttributes.Out, ParameterAttributes.Out,
                ParameterAttributes.In | ParameterAttributes.Out | ParameterAttributes.Optional, ParameterAttributes.In,
                ParameterAttributes.Out | ParameterAttributes.Optional, ParameterAttributes.In, ParameterAttributes.Out },
                parameters.Select(p => p.Attributes).ToArray());
            AssertCount(reader, parameters[0], "NativeArrayInfoAttribute", "CountParamIndex", 1, shortValue: true);
            AssertCount(reader, parameters[5], "NativeArrayInfoAttribute", "CountConst", 3, shortValue: false);
            AssertCount(reader, parameters[6], "MemorySizeAttribute", "BytesParamIndex", 7, shortValue: true);
            Assert.IsFalse(parameters[3].Attributes.HasFlag(ParameterAttributes.Optional),
                "RPC deref_out_opt permits a null result, not a null storage pointer.");
            Assert.IsFalse(parameters.SelectMany(p => p.GetCustomAttributes()).Select(reader.GetCustomAttribute)
                .Any(a => ParameterHeaderTests.AttributeName(reader, a) is "RetValAttribute" or "ComOutPtrAttribute"));
            Console.WriteLine("Final counts select capacity (parameter 1), fixedInput constant 3, and byteCapacity (parameter 7). " +
                "Native initialized extents *written and *byteWritten remain in SAL; the final schema has no independent extent field.");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void DoubleNull_ReturnFieldAndTypedefUsesKeepNativeStorage(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
_NullNull_terminated_ typedef const wchar_t* NAMES;
typedef NAMES NAMES_ALIAS;
_NullNull_terminated_ const wchar_t* __stdcall Direct(void);
NAMES_ALIAS __stdcall Aliased(void);
long __stdcall Use(NAMES_ALIAS names, const wchar_t* neighbor);
struct Record { _NullNull_terminated_ const wchar_t* direct; NAMES_ALIAS names; const wchar_t* neighbor; };
", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine(File.ReadAllText(fixture.GeneratedSource));
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "Direct", "Aliased" })
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(m => reader.GetString(m.Name) == name);
                var parameter = method.GetParameters().Select(reader.GetParameter).Single();
                Assert.AreEqual(0, parameter.SequenceNumber);
                ParameterHeaderTests.AssertAttribute(reader, parameter, "NullNullTerminatedAttribute");
                CollectionAssert.AreEqual(new byte[] { 0x0f, 0x07 }, reader.GetBlobBytes(method.Signature).TakeLast(2).ToArray());
            }
            var use = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Use").GetParameters().Select(reader.GetParameter)
                .Where(p => p.SequenceNumber != 0).ToArray();
            ParameterHeaderTests.AssertAttribute(reader, use[0], "NullNullTerminatedAttribute");
            Assert.IsFalse(use[1].GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Any(a => ParameterHeaderTests.AttributeName(reader, a) == "NullNullTerminatedAttribute"));
            foreach (var field in NativeTypedefHeaderTests.Type(reader, "Record").GetFields().Select(reader.GetFieldDefinition))
            {
                var attributes = field.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                Assert.AreEqual(reader.GetString(field.Name) == "neighbor" ? 0 : 1,
                    attributes.Count(a => ParameterHeaderTests.AttributeName(reader, a) == "NullNullTerminatedAttribute"));
                CollectionAssert.AreEqual(new byte[] { 0x06, 0x0f, 0x07 }, reader.GetBlobBytes(field.Signature));
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NamedCarriers_ResolveOnlyTheExactNativeTargetsAndDisappear(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string header = @"
enum Option { First = 1 };
struct Record { unsigned short field; unsigned short neighbor; };
struct __declspec(uuid(""64abea93-aac7-4f3b-a257-976deba89f04"")) IExample {
    virtual long __stdcall Value(_In_ const long* retained,
        _Out_writes_to_opt_(capacity, *written) long* buffer,
        unsigned long capacity, _Out_ unsigned long* written,
        /* [retval][out] */ _Out_ long* result) = 0;
    virtual long __stdcall Neighbor(_In_ const long* retained) = 0;
};
_Win32_AnnotateMethod_(IExample::Value, _Win32_PreserveResult_);
_Win32_AnnotateParameter_(IExample::Value, retained, _Win32_Retained_);
_Win32_AnnotateParameter_(IExample::Value, buffer, _Win32_ContainsInteriorPointers_);
_Win32_AnnotateField_(Record::field, _Win32_AssociatedEnum_(Option));
";
            var result = fixture.Scrape(header, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine(File.ReadAllText(fixture.GeneratedSource));
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Value");
            Assert.IsTrue(method.ImplAttributes.HasFlag(MethodImplAttributes.PreserveSig));
            var parameters = method.GetParameters().Select(reader.GetParameter).Where(p => p.SequenceNumber != 0).ToArray();
            ParameterHeaderTests.AssertAttribute(reader, parameters[0], "RetainedAttribute");
            ParameterHeaderTests.AssertAttribute(reader, parameters[0], "ConstAttribute");
            ParameterHeaderTests.AssertAttribute(reader, parameters[1], "ContainsInteriorPointersAttribute");
            AssertCount(reader, parameters[1], "NativeArrayInfoAttribute", "CountParamIndex", 2, shortValue: true);
            ParameterHeaderTests.AssertAttribute(reader, parameters[4], "RetValAttribute");
            CollectionAssert.AreEqual(new[] { ParameterAttributes.In, ParameterAttributes.Out | ParameterAttributes.Optional,
                ParameterAttributes.In, ParameterAttributes.Out, ParameterAttributes.Out }, parameters.Select(p => p.Attributes).ToArray());
            var neighbor = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Neighbor");
            Assert.IsFalse(neighbor.ImplAttributes.HasFlag(MethodImplAttributes.PreserveSig));
            var fields = NativeTypedefHeaderTests.Type(reader, "Record").GetFields().Select(reader.GetFieldDefinition).ToArray();
            var association = reader.GetCustomAttribute(fields[0].GetCustomAttributes().Single());
            Assert.AreEqual("AssociatedEnumAttribute", ParameterHeaderTests.AttributeName(reader, association));
            var blob = reader.GetBlobReader(association.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual("Option", blob.ReadSerializedString());
            Assert.AreEqual((ushort)0, blob.ReadUInt16());
            Assert.AreEqual(0, blob.RemainingBytes);
            Assert.AreEqual(0, fields[1].GetCustomAttributes().Count);
            Assert.IsFalse(reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Any(f => reader.GetString(f.Name).StartsWith("_WIN32META_", StringComparison.Ordinal)));
            Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute)
                .Any(a => ParameterHeaderTests.AttributeName(reader, a) is "NativeAnnotationAttribute" or "CppAttributeList"));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void MethodCarrier_PreservesExistingTypedProducerTransport(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
typedef void* RESOURCE;
int __stdcall Release(RESOURCE resource);
struct __declspec(uuid(""64abea93-aac7-4f3b-a257-976deba89f04"")) IExample {
    virtual RESOURCE __stdcall Create(void) = 0;
};
_Win32_AnnotateMethod_(IExample::Create, _Win32_RAIIFree_(Release, (RESOURCE)-1, 0));
", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Create");
            var returned = method.GetParameters().Select(reader.GetParameter).Single();
            Assert.AreEqual(0, returned.SequenceNumber);
            var facts = returned.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
            var cleanup = facts.Single(a => ParameterHeaderTests.AttributeName(reader, a) == "RAIIFreeAttribute");
            var blob = reader.GetBlobReader(cleanup.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual("Release", blob.ReadSerializedString());
            Assert.AreEqual((ushort)0, blob.ReadUInt16());
            Assert.AreEqual(0, blob.RemainingBytes);
            var sentinels = facts.Where(a => ParameterHeaderTests.AttributeName(reader, a) == "InvalidHandleValueAttribute")
                .Select(a =>
                {
                    var value = reader.GetBlobReader(a.Value);
                    Assert.AreEqual((ushort)1, value.ReadUInt16());
                    long sentinel = value.ReadInt64();
                    Assert.AreEqual((ushort)0, value.ReadUInt16());
                    Assert.AreEqual(0, value.RemainingBytes);
                    return sentinel;
                }).ToArray();
            CollectionAssert.AreEqual(new long[] { -1, 0 }, sentinels);
        }

        [TestMethod]
        public void OverloadedNamedCarrier_IsRejectedByNativeCompiler()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
struct IExample { virtual long Value(long value) = 0; virtual long Value(long* value) = 0; };
_Win32_AnnotateMethod_(IExample::Value, _Win32_PreserveResult_);
", sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "overloaded");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("_Win32_AnnotateParameter_(IExample::Value, missing, _Win32_Retained_);", "No native parameter")]
        [DataRow("_Win32_AnnotateMethod_(IExample::Value, _Win32_Retained_);", "parameter declaration")]
        [DataRow("_Win32_AnnotateField_(Record::field, _Win32_Retained_);", "parameter declaration")]
        [DataRow("_Win32_AnnotateMethod_(IExample::Value, _Win32_PreserveResult_);\n_Win32_AnnotateMethod_(IExample::Value, _Win32_PreserveResult_);", "Duplicate")]
        [DataRow("_Win32_AnnotateParameter_(IExample::Value, value, _Win32_Retval_);", "pointer parameter")]
        [DataRow("_Win32_AnnotateField_(Record::field, _Win32_AssociatedEnum_(Missing));", "existing native enum")]
        [DataRow("_Win32_AnnotateMethod_(IExample::Missing, _Win32_PreserveResult_);", "no member named")]
        [DataRow("_Win32_AnnotateField_(IExample::Value, _Win32_AssociatedEnum_(Option));", "non-static record field")]
        [DataRow("_Win32_AnnotateField_(Record::field, _Win32_AssociatedEnum_(Option));\n_Win32_AnnotateField_(Record::field, _Win32_AssociatedEnum_(Option));", "enum association")]
        public void InvalidNamedCarriers_AreSourceLocated(string carrier, string diagnostic)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("enum Option { First = 1 }; struct Record { long field; };\n" +
                "struct IExample { virtual long __stdcall Value(long value) = 0; };\n" + carrier, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, diagnostic);
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        internal static string SharedHeaders() => Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
            "generation", "WinSDK", "RecompiledIdlHeaders", "shared");

        internal static void AssertCount(MetadataReader reader, Parameter parameter, string attribute, string field, int value, bool shortValue)
        {
            var fact = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(a => ParameterHeaderTests.AttributeName(reader, a) == attribute);
            var blob = reader.GetBlobReader(fact.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual((byte)0x53, blob.ReadByte());
            Assert.AreEqual(shortValue ? (byte)0x06 : (byte)0x08, blob.ReadByte());
            Assert.AreEqual(field, blob.ReadSerializedString());
            Assert.AreEqual(value, shortValue ? blob.ReadInt16() : blob.ReadInt32());
            Assert.AreEqual(0, blob.RemainingBytes);
        }
    }
}
