using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class CallbackHeaderTests
    {
        private const string LegacySettings = "--exclude\nLPWINHTTP_STATUS_CALLBACK\n--remap\n" +
            "WINHTTP_STATUS_CALLBACK=WINHTTP_STATUS_CALLBACK\nLPWINHTTP_STATUS_CALLBACK=WINHTTP_STATUS_CALLBACK";

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void ShippedCallback_FinalWinmdKeepsSignatureConventionAndPointerDepth(
            string target, string language)
        {
            string header = ShippedDeclarations();
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, target, language: language, sal: true, extraSettings: LegacySettings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] legacy = File.ReadAllBytes(fixture.Emit());
            result = fixture.Scrape(header, target, language: language, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(legacy, File.ReadAllBytes(fixture.Emit()), "Exact retired-selector parity.");

            // The SDK does not use its LP alias in an API. Exercise that genuine
            // declaration separately without changing a shipped function signature.
            result = fixture.Scrape(header + "\nvoid __stdcall Probe(WINHTTP_STATUS_CALLBACK value, " +
                "LPWINHTTP_STATUS_CALLBACK address);", target, language: language, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var callbackHandle = reader.TypeDefinitions.Single(handle =>
                reader.GetString(reader.GetTypeDefinition(handle).Name) == "WINHTTP_STATUS_CALLBACK");
            var callback = reader.GetTypeDefinition(callbackHandle);
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(type =>
                reader.GetString(type.Name) == "LPWINHTTP_STATUS_CALLBACK"));
            var invoke = callback.GetMethods().Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Invoke");
            var signature = reader.GetBlobReader(invoke.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(5, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            AssertVoidPointer(ref signature);
            Assert.AreEqual(SignatureTypeCode.UIntPtr, signature.ReadSignatureTypeCode(), "DWORD_PTR is pointer-sized.");
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            AssertVoidPointer(ref signature);
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
            var convention = reader.GetBlobReader(callback.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => AttributeName(reader, attribute) == "UnmanagedFunctionPointerAttribute").Value);
            Assert.AreEqual((ushort)1, convention.ReadUInt16());
            Assert.AreEqual(1, convention.ReadInt32(), "The existing emitter represents Windows StdCall as Winapi.");
            Assert.AreEqual((ushort)0, convention.ReadUInt16());
            Assert.AreEqual(0, convention.RemainingBytes);

            var setter = Method(reader, "WinHttpSetStatusCallback");
            Assert.IsTrue(setter.GetImport().Attributes.HasFlag(MethodImportAttributes.CallingConventionWinApi));
            signature = reader.GetBlobReader(setter.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(4, signature.ReadCompressedInteger());
            AssertCallback(ref signature, callbackHandle);
            AssertVoidPointer(ref signature);
            AssertCallback(ref signature, callbackHandle);
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UIntPtr, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);

            signature = reader.GetBlobReader(Method(reader, "Probe").Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(2, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            AssertCallback(ref signature, callbackHandle);
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertCallback(ref signature, callbackHandle);
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void SameLevelAlias_UsesTheCompleteDefiningDelegate(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef int (__stdcall *CALLBACK)(unsigned short value);\n" +
                "typedef CALLBACK SAMECALLBACK;\nvoid __stdcall Example(CALLBACK value, SAMECALLBACK same);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var callback = reader.TypeDefinitions.Single(handle =>
                reader.GetString(reader.GetTypeDefinition(handle).Name) == "CALLBACK");
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(type =>
                reader.GetString(type.Name) == "SAMECALLBACK"));
            var invoke = reader.GetTypeDefinition(callback).GetMethods().Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Invoke");
            var signature = reader.GetBlobReader(invoke.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(1, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
            signature = reader.GetBlobReader(Method(reader, "Example").Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(2, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            AssertCallback(ref signature, callback);
            AssertCallback(ref signature, callback);
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        [TestMethod]
        public void AliasStorage_IsPreservedInReturnFieldsAndAdditionalPointers()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef int (__stdcall *CALLBACK)(unsigned short value);\n" +
                "typedef CALLBACK *LPCALLBACK;\nstruct State { LPCALLBACK address; };\n" +
                "LPCALLBACK __stdcall Example(LPCALLBACK* address);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var callback = reader.TypeDefinitions.Single(handle =>
                reader.GetString(reader.GetTypeDefinition(handle).Name) == "CALLBACK");
            var signature = reader.GetBlobReader(Method(reader, "Example").Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(1, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertCallback(ref signature, callback);
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertCallback(ref signature, callback);
            Assert.AreEqual(0, signature.RemainingBytes);
            var state = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Single(type => reader.GetString(type.Name) == "State");
            signature = reader.GetBlobReader(reader.GetFieldDefinition(state.GetFields().Single()).Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertCallback(ref signature, callback);
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        [DataTestMethod]
        [DataRow("--with-type\nLPWINHTTP_STATUS_CALLBACK=void*")]
        [DataRow("--with-type\nLPWINHTTP_STATUS_CALLBACK=WINHTTP_STATUS_CALLBACK")]
        public void ConflictingAliasStorage_IsNotSilentlyAccepted(string settings)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(ShippedDeclarations(), sal: true, extraSettings: settings);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "LPWINHTTP_STATUS_CALLBACK");
            StringAssert.Contains(result.Output, "conflicts with native storage");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void ConflictingAliasRemap_IsNotSilentlyAccepted()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(ShippedDeclarations(), sal: true,
                extraSettings: "--remap\nLPWINHTTP_STATUS_CALLBACK=Unrelated");
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "conflicts with native callback");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void MatchingAliasStorage_IsByteIdentical()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string header = ShippedDeclarations() + "\nvoid __stdcall Probe(LPWINHTTP_STATUS_CALLBACK address);";
            var result = fixture.Scrape(header, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            result = fixture.Scrape(header, sal: true,
                extraSettings: "--with-type\nLPWINHTTP_STATUS_CALLBACK=WINHTTP_STATUS_CALLBACK*");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit()));
        }

        [TestMethod]
        public void CdeclCallback_IsNotChangedToTheWindowsDefaultConvention()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef int (__cdecl *CALLBACK)(int value);\n" +
                "typedef CALLBACK *LPCALLBACK;\nvoid __stdcall Example(LPCALLBACK address);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var callback = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Single(type => reader.GetString(type.Name) == "CALLBACK");
            var convention = reader.GetBlobReader(callback.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => AttributeName(reader, attribute) == "UnmanagedFunctionPointerAttribute").Value);
            Assert.AreEqual((ushort)1, convention.ReadUInt16());
            Assert.AreEqual(2, convention.ReadInt32(), "Cdecl must not become Winapi.");
            Assert.AreEqual((ushort)0, convention.ReadUInt16());
            Assert.AreEqual(0, convention.RemainingBytes);
        }

        [DataTestMethod]
        [DataRow("_Win32_Retained_ typedef void (__stdcall *CALLBACK)(int value);", "parameter declaration")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.AlsoUsableFor()\"))) typedef void (__stdcall *CALLBACK)(int value);", "Unknown or malformed")]
        [DataRow("typedef void (__stdcall *CALLBACK)(int value,);", "error:")]
        public void MalformedOrMisplacedNativeFacts_FailBeforeBindingGeneration(string header, string diagnostic)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, diagnostic);
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void NativeCompiler_CallbackPointerDepthAndOrdinaryBuildRemainUnchanged(string arch, int pointerSize, string language)
        {
            string directory = Path.Combine(Path.GetTempPath(), "callback-abi-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            try
            {
                string source = Path.Combine(directory, "abi.c");
                File.WriteAllText(source, "#define _In_\n" + ShippedDeclarations() + @"
typedef char PointerWidth[(sizeof(DWORD_PTR) == " + pointerSize + @") ? 1 : -1];
VOID CALLBACK Implementation(HINTERNET handle, DWORD_PTR context, DWORD status, LPVOID information, DWORD length) {}
WINHTTP_STATUS_CALLBACK callback = &Implementation;
LPWINHTTP_STATUS_CALLBACK address = &callback;
VOID CALLBACK Caller(void) { (*address)(0, 0, 0, 0, 0); }
");
                string script = ". " + Quote(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "CommonUtils.ps1")) +
                    "; $compiler = Join-Path (Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64) 'cl.exe'; " +
                    "foreach ($mode in 'ordinary', 'metadata') { $arguments = @('/nologo', '/c', '/Brepro', '/WX', " +
                    Quote(language) + ", " + Quote(source) + ", " + Quote("/Fo" + Path.Combine(directory, "abi.obj")) +
                    "); if ($mode -eq 'metadata') { $arguments += '/DWIN32METADATA' }; & $compiler @arguments; " +
                    "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " +
                    Quote(Path.Combine(directory, "abi.obj")) + " -Destination (" + Quote(directory + "\\") + " + $mode + '.obj') }";
                var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command", script);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] ordinary = File.ReadAllBytes(Path.Combine(directory, "ordinary.obj"));
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(ordinary, 0));
                CollectionAssert.AreEqual(ordinary, File.ReadAllBytes(Path.Combine(directory, "metadata.obj")),
                    "Deterministic native objects are compared without timestamp or other masks.");
            }
            finally { Directory.Delete(directory, true); }
        }

        [TestMethod]
        public void ScalarTypedef_IsNotConflatedWithItsSeparateEnum()
        {
            string source = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "shared", "wtypes.h"));
            StringAssert.Contains(source, "typedef unsigned short VARTYPE;");
            StringAssert.Contains(source, "enum VARENUM");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef unsigned short VARTYPE;\nenum VARENUM { VT_EMPTY = 0 };\n" +
                "VARTYPE __stdcall Example(VARTYPE value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(type =>
                reader.GetString(type.Name) == "VARTYPE"), "Scalar identity is an explicit unsupported boundary.");
            var signature = reader.GetBlobReader(Method(reader, "Example").Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(1, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
            var enumType = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Single(type => reader.GetString(type.Name) == "VARENUM");
            var storage = enumType.GetFields().Select(reader.GetFieldDefinition)
                .Single(field => reader.GetString(field.Name) == "value__");
            signature = reader.GetBlobReader(storage.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
        }

        private static string ShippedDeclarations()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string script = ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; Join-Path (Get-WinSdkCppPkgPath 6>$null) 'c\\include\\10.0.28000.0\\um\\winhttp.h'";
            var located = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command", script);
            Assert.AreEqual(0, located.ExitCode, located.Output);
            string path = located.Output.Trim();
            StringAssert.Contains(path, "10.0.28000.2705");
            string shipped = File.ReadAllText(path);
            string current = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", "winhttp.h"));
            string callback = Slice(shipped, @"typedef\s+VOID\s+\(CALLBACK \* WINHTTP_STATUS_CALLBACK\)[\s\S]*?typedef WINHTTP_STATUS_CALLBACK \* LPWINHTTP_STATUS_CALLBACK;");
            string setter = Slice(shipped, @"WINHTTPAPI\s+WINHTTP_STATUS_CALLBACK\s+WINAPI\s+WinHttpSetStatusCallback[\s\S]*?;");
            Assert.AreEqual(callback, Slice(current, @"typedef\s+VOID\s+\(CALLBACK \* WINHTTP_STATUS_CALLBACK\)[\s\S]*?typedef WINHTTP_STATUS_CALLBACK \* LPWINHTTP_STATUS_CALLBACK;"));
            Assert.AreEqual(setter, Slice(current, @"WINHTTPAPI\s+WINHTTP_STATUS_CALLBACK\s+WINAPI\s+WinHttpSetStatusCallback[\s\S]*?;"));
            return @"#define CALLBACK __stdcall
#define WINAPI __stdcall
#define WINHTTPAPI
#define IN _In_
#define OPTIONAL
typedef void VOID;
typedef void* HINTERNET;
typedef void* LPVOID;
typedef unsigned long DWORD;
#ifdef _WIN64
typedef unsigned long long DWORD_PTR;
#else
typedef unsigned long DWORD_PTR;
#endif
" + callback + "\n" + setter;
        }

        private static string Slice(string source, string pattern)
        {
            var matches = Regex.Matches(source, pattern);
            Assert.AreEqual(1, matches.Count, pattern);
            return matches[0].Value;
        }

        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";

        private static MethodDefinition Method(MetadataReader reader, string name) =>
            reader.MethodDefinitions.Select(reader.GetMethodDefinition).Single(method => reader.GetString(method.Name) == name);

        private static void AssertVoidPointer(ref BlobReader signature)
        {
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
        }

        private static void AssertCallback(ref BlobReader signature, TypeDefinitionHandle callback)
        {
            Assert.AreEqual(SignatureTypeCode.TypeHandle, signature.ReadSignatureTypeCode());
            Assert.AreEqual(callback, signature.ReadTypeHandle());
        }

        private static string AttributeName(MetadataReader reader, CustomAttribute attribute)
        {
            var parent = attribute.Constructor.Kind == HandleKind.MethodDefinition ?
                (EntityHandle)reader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor).GetDeclaringType() :
                reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent;
            return parent.Kind == HandleKind.TypeDefinition ?
                reader.GetString(reader.GetTypeDefinition((TypeDefinitionHandle)parent).Name) :
                reader.GetString(reader.GetTypeReference((TypeReferenceHandle)parent).Name);
        }
    }
}
