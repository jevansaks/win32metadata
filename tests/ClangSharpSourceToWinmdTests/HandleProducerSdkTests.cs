using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class HandleProducerSdkTests
    {
        private static readonly string[] ProjectionSites =
        {
            "OpenPrinterW::phPrinter", "ClosePrinter::hPrinter", "GetProcessHeap::return",
            "LocalAlloc::return", "LocalFree::return", "LocalFree::hMem",
        };

        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void PrinterClosure_OwnsOnlyOpenAndAddProducers(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(PrinterDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "OpenPrinterA", "OpenPrinterW", "OpenPrinter2A", "OpenPrinter2W" })
                ProducerHeaderTests.AssertFacts(reader, name, 2, "ClosePrinter", -1, 0);
            foreach (string name in new[] { "AddPrinterA", "AddPrinterW" })
                ProducerHeaderTests.AssertFacts(reader, name, 0, "ClosePrinter", -1, 0);
            ProducerHeaderTests.AssertNoOwnership(reader, "ClosePrinter");
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void PrinterClosure_PreservesNativeAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language, @"
#define _In_
#define _Out_
#define _In_opt_
#define _In_range_(...)
#define _When_(...)
#define _In_reads_bytes_(...)
" + PrinterDeclarations() + @"
BOOL (WINAPI *verifyOpen)(LPCSTR, LPHANDLE, LPPRINTER_DEFAULTSA) = &OpenPrinterA;
BOOL (WINAPI *verifyOpen2)(LPCWSTR, LPHANDLE, PPRINTER_DEFAULTSW, PPRINTER_OPTIONSW) = &OpenPrinter2W;
HANDLE (WINAPI *verifyAdd)(LPCWSTR, DWORD, LPBYTE) = &AddPrinterW;
");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void MappedViewCleanup_ConstPointeeRequiresAnExplicitContractDecision(string target)
        {
            string text = File.ReadAllText(Path.Combine(LastErrorHeaderTests.SdkPackagePath(),
                "c", "include", "10.0.28000.0", "um", "memoryapi.h"));
            string cleanup = Regex.Match(text, @"BOOL\s+WINAPI\s+UnmapViewOfFile\([^;]+;").Value;
            string producer = Regex.Match(text, @"LPVOID\s+WINAPI\s+MapViewOfFile\([^;]+;").Value;
            Assert.IsFalse(string.IsNullOrEmpty(cleanup));
            Assert.IsFalse(string.IsNullOrEmpty(producer));
            string dependencies = NextProducerDependencies +
                "\ntypedef __SIZE_TYPE__ SIZE_T;\ntypedef const void* LPCVOID;\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(dependencies + cleanup + producer, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            result = fixture.Scrape(dependencies + cleanup +
                "\n_Win32_RAIIFree_(UnmapViewOfFile, 0)\n" + producer, target, sal: true);
            TestContext.WriteLine(result.Output);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output,
                "RAIIFree cleanup must resolve to one declared non-member, non-variadic native function accepting exactly the produced resource type.");
        }

        private static string PrinterDeclarations()
        {
            string header = File.ReadAllText(Sdk("RecompiledIdlHeaders/um/winspool.h"));
            string[] names = { "ClosePrinter", "OpenPrinterA", "OpenPrinterW", "OpenPrinter2A",
                "OpenPrinter2W", "AddPrinterA", "AddPrinterW" };
            return NextProducerDependencies + @"
typedef HANDLE* LPHANDLE;
typedef const char* LPCSTR;
typedef const unsigned short* LPCWSTR;
typedef unsigned char* LPBYTE;
typedef void* LPPRINTER_DEFAULTSA;
typedef void* LPPRINTER_DEFAULTSW;
typedef void* PPRINTER_DEFAULTSA;
typedef void* PPRINTER_DEFAULTSW;
typedef void* PPRINTER_OPTIONSA;
typedef void* PPRINTER_OPTIONSW;
" + string.Join("\n", names.Select(name =>
                Regex.Match(header,
                    @"(?m)(?:^_Win32_RAIIFree_Handle_\([^\r\n]*\)\r?\n)?^\s*(?:BOOL|HANDLE)\s+WINAPI\s+" +
                    name + @"\([^;]+;").Value));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void HttpFilteringAndPrinterProducers_RetireOnlyProjectionHandles(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(NextProducerDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            ProducerHeaderTests.AssertFacts(reader, "HttpCreateRequestQueue", 5, "HttpCloseRequestQueue", 0);
            ProducerHeaderTests.AssertFacts(reader, "FwpmEngineOpen0", 5, "FwpmEngineClose0", -1, 0);
            ProducerHeaderTests.AssertFacts(reader, "FindFirstPrinterChangeNotification", 0,
                "FindClosePrinterChangeNotification", -1, 0);
            foreach (string name in new[] { "HttpCloseRequestQueue", "HttpCreateHttpHandle", "FwpmEngineClose0",
                "FindNextPrinterChangeNotification", "FindClosePrinterChangeNotification" })
                ProducerHeaderTests.AssertNoOwnership(reader, name);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void FilteringEnumerationCleanup_RequiresEngineContextBeyondOneArgumentContract(string target)
        {
            string declarations = NextProducerDependencies +
                CurrentProducerDeclaration("fwpmu.h", "FwpmFilterDestroyEnumHandle0") + "\n" +
                CurrentProducerDeclaration("fwpmu.h", "FwpmFilterCreateEnumHandle0");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(declarations, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            result = fixture.Scrape(declarations.Replace("_Out_ HANDLE* enumHandle",
                "_Out_ _Win32_RAIIFree_Handle_(FwpmFilterDestroyEnumHandle0) HANDLE* enumHandle",
                StringComparison.Ordinal), target, sal: true);
            TestContext.WriteLine(result.Output);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output,
                "RAIIFree cleanup must resolve to one declared non-member, non-variadic native function accepting exactly the produced resource type.");
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void HttpFilteringAndPrinterProducers_PreserveNativeAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language, @"
#define _In_
#define _Out_
#define _Inout_
#define _In_opt_
#define _Out_opt_
#define _Reserved_
" + NextProducerDeclarations() + @"
ULONG (WINAPI *verifyHttp)(HTTPAPI_VERSION, PCWSTR, PSECURITY_ATTRIBUTES, ULONG, PHANDLE) = &HttpCreateRequestQueue;
DWORD (WINAPI *verifyEngine)(const wchar_t*, UINT32, SEC_WINNT_AUTH_IDENTITY_W*, const FWPM_SESSION0*, HANDLE*) = &FwpmEngineOpen0;
HANDLE (WINAPI *verifyPrinter)(HANDLE, DWORD, DWORD, PVOID) = &FindFirstPrinterChangeNotification;
");
        }

        private const string NextProducerDependencies = @"
#define WINAPI __stdcall
#define HTTPAPI_LINKAGE __declspec(dllimport)
#define IN
#define OUT
#define OPTIONAL
typedef void* HANDLE;
typedef HANDLE* PHANDLE;
typedef int BOOL;
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef unsigned int UINT32;
#ifndef __cplusplus
typedef unsigned short wchar_t;
#endif
typedef const wchar_t* PCWSTR;
typedef void* PVOID;
typedef void* LPVOID;
typedef DWORD* PDWORD;
typedef struct { unsigned short major, minor; } HTTPAPI_VERSION;
typedef struct SECURITY_ATTRIBUTES { DWORD length; } SECURITY_ATTRIBUTES, *PSECURITY_ATTRIBUTES;
typedef struct SEC_WINNT_AUTH_IDENTITY_W { DWORD flags; } SEC_WINNT_AUTH_IDENTITY_W;
typedef struct FWPM_SESSION0 { DWORD flags; } FWPM_SESSION0;
typedef struct FWPM_FILTER_ENUM_TEMPLATE0 { DWORD flags; } FWPM_FILTER_ENUM_TEMPLATE0;
";

        private static string NextProducerDeclarations() => NextProducerDependencies + string.Join("\n", new[] {
            CurrentProducerDeclaration("http.h", "HttpCloseRequestQueue"),
            CurrentProducerDeclaration("http.h", "HttpCreateRequestQueue"),
            CurrentProducerDeclaration("http.h", "HttpCreateHttpHandle"),
            CurrentProducerDeclaration("fwpmu.h", "FwpmEngineClose0"),
            CurrentProducerDeclaration("fwpmu.h", "FwpmEngineOpen0"),
            CurrentProducerDeclaration("winspool.h", "FindClosePrinterChangeNotification"),
            CurrentProducerDeclaration("winspool.h", "FindFirstPrinterChangeNotification"),
            CurrentProducerDeclaration("winspool.h", "FindNextPrinterChangeNotification"),
        });

        private static string CurrentProducerDeclaration(string header, string name)
        {
            var match = Regex.Match(File.ReadAllText(Sdk("RecompiledIdlHeaders/um/" + header)),
                @"(?m)(?:^_Win32_RAIIFree_(?:Handle_)?\([^\r\n]*\)\r?\n)?^(?:HTTPAPI_LINKAGE\s+)?(?:DWORD|ULONG|BOOL|HANDLE)\s+WINAPI\s+" +
                name + @"\s*\([^;]+;");
            Assert.IsTrue(match.Success, name);
            return match.Value;
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void AvrtNativeProducers_ReplaceFictitiousTypesAndDistinguishJoinCleanup(string target)
        {
            string declarations = AvrtDeclarations();
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(declarations, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                foreach (string name in new[] { "AvSetMmThreadCharacteristicsA", "AvSetMmThreadCharacteristicsW",
                    "AvSetMmMaxThreadCharacteristicsA", "AvSetMmMaxThreadCharacteristicsW" })
                    ProducerHeaderTests.AssertFacts(reader, name, 0, "AvRevertMmThreadCharacteristics", 0);
                foreach (string name in new[] { "AvRtCreateThreadOrderingGroup", "AvRtCreateThreadOrderingGroupExA",
                    "AvRtCreateThreadOrderingGroupExW" })
                    ProducerHeaderTests.AssertFacts(reader, name, 1, "AvRtDeleteThreadOrderingGroup", 0);
                ProducerHeaderTests.AssertFacts(reader, "AvRtJoinThreadOrderingGroup", 1, "AvRtLeaveThreadOrderingGroup", 0);
                foreach (string name in new[] { "AvRevertMmThreadCharacteristics", "AvSetMmThreadPriority",
                    "AvRtWaitOnThreadOrderingGroup", "AvRtLeaveThreadOrderingGroup",
                    "AvRtDeleteThreadOrderingGroup", "AvQuerySystemResponsiveness" })
                    ProducerHeaderTests.AssertNoOwnership(reader, name);
                Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(type =>
                    reader.GetString(type.Name).StartsWith("AVRT_", StringComparison.Ordinal) &&
                    reader.GetString(type.Name).EndsWith("_HANDLE", StringComparison.Ordinal)));
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == "AvSetMmThreadPriority");
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(2, signature.ReadCompressedInteger());
                Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.TypeHandle, signature.ReadSignatureTypeCode());
                Assert.AreEqual("AVRT_PRIORITY", EnumHeaderTests.TypeName(reader, signature.ReadTypeHandle()));
                Assert.AreEqual(0, signature.RemainingBytes);
            }
            result = fixture.Scrape(Regex.Replace(declarations, @"_Win32_RAIIFree_\([^)]*\)", ""), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var original = new PEReader(File.OpenRead(fixture.Emit()));
            ProducerHeaderTests.AssertNoOwnership(original.GetMetadataReader(), "AvRtJoinThreadOrderingGroup");
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void AvrtNativeProducers_AnnotationsPreserveRawAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language, @"
#define _In_
#define _Out_
#define _Inout_
#define _In_opt_
#define _Success_(value)
" + AvrtDeclarations() + @"
HANDLE (WINAPI *verifyTask)(LPCWSTR, LPDWORD) = &AvSetMmThreadCharacteristicsW;
BOOL (WINAPI *verifyCreate)(PHANDLE, PLARGE_INTEGER, GUID*, PLARGE_INTEGER) = &AvRtCreateThreadOrderingGroup;
BOOL (WINAPI *verifyJoin)(PHANDLE, GUID*, BOOL) = &AvRtJoinThreadOrderingGroup;
BOOL (WINAPI *verifyLeave)(HANDLE) = &AvRtLeaveThreadOrderingGroup;
");
        }

        private static string AvrtDeclarations() => @"
#define WINAPI __stdcall
#define DECLSPEC_IMPORT __declspec(dllimport)
#define WINAPI_FAMILY_PARTITION(...) 1
typedef void* HANDLE;
typedef HANDLE* PHANDLE;
typedef int BOOL;
typedef unsigned long DWORD;
typedef DWORD* LPDWORD;
typedef unsigned long* PULONG;
typedef const char* LPCSTR;
typedef const unsigned short* LPCWSTR;
typedef struct { long long value; } LARGE_INTEGER, *PLARGE_INTEGER;
typedef struct { unsigned long a; unsigned short b, c; unsigned char d[8]; } GUID;
" + File.ReadAllText(Sdk("RecompiledIdlHeaders/um/avrt.h"))
            .Replace("#include <win32metadata_annotations.h>", "", StringComparison.Ordinal)
            .Replace("#include <winapifamily.h>", "", StringComparison.Ordinal);

        internal static string Parent()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/HandleProducerSdkTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the handle producer fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static string ApplySidecarDelta(string text) => text
            .Replace("GetProcessHeap::return=[DoNotRelease]\n",
                "GetProcessHeap::return=void*[-NativeTypeName]\nLocalAlloc::return=void*[-NativeTypeName]\nLocalFree::return=void*[-NativeTypeName]\nLocalFree::hMem=void*[-NativeTypeName]\n", StringComparison.Ordinal)
            .Replace("LocalFree=[CanReturnErrorsAsSuccess]\n", "", StringComparison.Ordinal)
            .Replace("ClosePrinter::hPrinter=PRINTER_HANDLE\n", "ClosePrinter::hPrinter=void*[-NativeTypeName]\n", StringComparison.Ordinal)
            .Replace("OpenPrinterW::phPrinter=PRINTER_HANDLE*\n", "OpenPrinterW::phPrinter=void**[-NativeTypeName]\n", StringComparison.Ordinal);

        [TestMethod]
        public void ExactParent_OnlySelectedProjectionSitesChangeAndLegacyTypesRemainUntouched()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string parent = Parent();
            string original = Encoding.Latin1.GetString(PropertyComOutputHeaderTests.ReadOriginalBytes("emitter.settings.rsp", parent))
                .Replace("\r\n", "\n");
            string current = FullHeaderMigrationExperimentTests.ReadRecoveryText("emitter.settings.rsp").Replace("\r\n", "\n");
            Assert.AreEqual(ApplySidecarDelta(original), current);
            Assert.AreEqual(NativeTypedefSdkTests.ApplyAutoTypesDelta(Encoding.UTF8.GetString(
                PropertyComOutputHeaderTests.ReadOriginalBytes("autoTypes.json", parent))),
                FullHeaderMigrationExperimentTests.ReadRecoveryText("autoTypes.json").Replace("\r\n", "\n"));
            foreach (string path in new[] { "scraper.settings.rsp", "functionPointerFixups.json",
                "RecompiledIdlHeaders/um/heapapi.h" })
                CollectionAssert.AreEqual(PropertyComOutputHeaderTests.ReadOriginalBytes(path, parent),
                    FullHeaderMigrationExperimentTests.ReadRecoveryBytes(path),
                    "Unrelated legacy/native inputs must remain byte-exact: " + path);
            var ancestry = LastErrorHeaderTests.Run("git", "-C", root, "merge-base", "--is-ancestor", parent, "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            string helper = File.ReadAllText(Sdk("RecompiledIdlHeaders/shared/win32metadata_annotations.h"));
            StringAssert.Contains(helper, "#define _Win32_RAIIFree_Handle_(cleanup) _Win32_RAIIFree_(cleanup, -1, 0)");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void SelectedSdkPatchReplay_PreservesAllBytesAndFinalRawProducerSites(string target)
        {
            string directory = Path.Combine(Path.GetTempPath(), "handle-sdk-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path.Combine(directory, "um"));
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            try
            {
                foreach (string owner in new[] { "WinBase.h", "winspool.h", "heapapi.h", "minwinbase.h" })
                    File.Copy(Path.Combine(include, "um", owner), Path.Combine(directory, "um", owner));
                var apply = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                    "& " + Quote(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "ApplySDKPatches.ps1")) +
                    " -Phase post-midl -HeaderRoot " + Quote(directory) + " -MidlInputRoot " + Quote(include) +
                    " -SdkVersion '10.0.28000.2705' -SelectedPatches @('post-midl/WinBase.h.patch','post-midl/winspool.h.patch','post-midl/heapapi.h.patch','post-midl/minwinbase.h.patch')");
                Assert.AreEqual(0, apply.ExitCode, apply.Output);
                foreach (string owner in new[] { "WinBase.h", "winspool.h", "heapapi.h", "minwinbase.h" })
                    CollectionAssert.AreEqual(File.ReadAllBytes(Sdk("RecompiledIdlHeaders/um/" + owner)),
                        File.ReadAllBytes(Path.Combine(directory, "um", owner)), owner + ": complete native bytes, including EOF and CRLF");

                using var fixture = new LastErrorHeaderTests.Fixture();
                string declarations = Declarations(directory);
                string settings = "--include-directory\n" + Path.Combine(include, "shared") + "\n" + Path.Combine(include, "um");
                var result = fixture.Scrape(declarations, target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                var remaps = FullHeaderMigrationExperimentTests.ReadRecoveryText("emitter.settings.rsp")
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => ProjectionSites.Contains(line.Split('=')[0]))
                    .ToDictionary(line => line.Split('=')[0], line => line.Substring(line.IndexOf('=') + 1));
                Assert.AreEqual(6, remaps.Count);
                string output = fixture.Emit(remaps);
                AssertFinalFacts(output);
                byte[] native = File.ReadAllBytes(output);

                // Matching legacy producer facts are representable after the applicability/schema change.
                remaps["OpenPrinterW::phPrinter"] = "void**[RAIIFree(\"ClosePrinter\")][InvalidHandleValue(-1L)][InvalidHandleValue(0L)]";
                remaps["LocalAlloc::return"] = "void*[RAIIFree(\"LocalFree\")][InvalidHandleValue(0L)]";
                CollectionAssert.AreEqual(native, File.ReadAllBytes(fixture.Emit(remaps)));
                result = fixture.Scrape(Regex.Replace(declarations, @"_Win32_RAIIFree_(?:Handle_)?\([^)]*\)", ""), target,
                    sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                CollectionAssert.AreEqual(native, File.ReadAllBytes(fixture.Emit(remaps)),
                    "The paired raw legacy fixture uses the same real SDK declarations, not invented API wrappers.");

                string proof = Path.Combine(this.TestContext.ResultsDirectory, "handle-producers-" + target + ".winmd");
                File.Copy(output, proof, true);
                this.TestContext.AddResultFile(proof);

                result = fixture.Scrape(Declarations(include), target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                using var original = new PEReader(File.OpenRead(fixture.Emit()));
                foreach (string name in new[] { "OpenPrinterW", "ClosePrinter", "LocalAlloc", "LocalFree", "GetProcessHeap" })
                    ProducerHeaderTests.AssertNoOwnership(original.GetMetadataReader(), name);
            }
            finally { Directory.Delete(directory, true); }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void RealSdkNativeCompiler_OrdinaryAndMetadataHaveIdenticalRawAbi(string arch, int pointerSize, string language)
        {
            string directory = Path.Combine(Path.GetTempPath(), "handle-native-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            try
            {
                foreach (string owner in new[] { "WinBase.h", "winspool.h" })
                    File.Copy(Sdk("RecompiledIdlHeaders/um/" + owner), Path.Combine(directory, owner));
                File.Copy(Sdk("RecompiledIdlHeaders/shared/win32metadata_annotations.h"),
                    Path.Combine(directory, "win32metadata_annotations.h"));
                string source = Path.Combine(directory, "abi.c");
                File.WriteAllText(source, @"
#include <windows.h>
#include <winspool.h>
typedef char PointerWidth[(sizeof(HANDLE) == " + pointerSize + @") ? 1 : -1];
typedef char LocalWidth[(sizeof(HLOCAL) == sizeof(void*)) ? 1 : -1];
typedef char OutputWidth[(sizeof(*((LPHANDLE)0)) == sizeof(void*)) ? 1 : -1];
BOOL (WINAPI *openPrinter)(LPCWSTR, void**, LPPRINTER_DEFAULTSW) = &OpenPrinterW;
BOOL (WINAPI *closePrinter)(void*) = &ClosePrinter;
void* (WINAPI *getHeap)(void) = &GetProcessHeap;
void* (WINAPI *localAlloc)(UINT, SIZE_T) = &LocalAlloc;
void* (WINAPI *localFree)(void*) = &LocalFree;
_Win32_RAIIFree_(Undeclared, not_a_constant)
_Win32_RAIIFree_Handle_(Missing)
HANDLE WINAPI Noop(void);
");
                string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
                string script = ". " + Quote(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "CommonUtils.ps1")) +
                    "; Install-VsDevShell; $compiler = Join-Path (Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64) 'cl.exe'; " +
                    "foreach ($mode in 'ordinary','metadata') { $arguments = @('/nologo','/c','/Brepro','/WX'," +
                    Quote(language) + "," + Quote(source) + "," + Quote("/Fo" + Path.Combine(directory, "abi.obj")) +
                    "," + Quote("/I" + directory) + "," + Quote("/I" + Path.Combine(include, "um")) +
                    "," + Quote("/I" + Path.Combine(include, "shared")) + "," + Quote("/I" + Path.Combine(include, "ucrt")) +
                    "); if ($mode -eq 'metadata') { $arguments += '/DWIN32METADATA' }; & $compiler @arguments; " +
                    "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " +
                    Quote(Path.Combine(directory, "abi.obj")) + " -Destination (" + Quote(directory + "\\") + " + $mode + '.obj') }";
                var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command", script);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] ordinary = File.ReadAllBytes(Path.Combine(directory, "ordinary.obj"));
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(ordinary, 0));
                CollectionAssert.AreEqual(ordinary, File.ReadAllBytes(Path.Combine(directory, "metadata.obj")));
                StringAssert.Contains(File.ReadAllText(Path.Combine(directory, "WinBase.h")),
                    "_Success_(return==0)\n_Ret_maybenull_\nHLOCAL\nWINAPI\nLocalFree(".Replace("\n", "\r\n"));
            }
            finally { Directory.Delete(directory, true); }
        }

        private static string Declarations(string include)
        {
            string printer = File.ReadAllText(Path.Combine(include, "um", "winspool.h"));
            string memory = File.ReadAllText(Path.Combine(include, "um", "WinBase.h"));
            string heap = File.ReadAllText(Path.Combine(include, "um", "heapapi.h"));
            string localFlags = string.Empty;
            if (memory.Contains("_Win32_AssociatedEnum_(LOCAL_ALLOC_FLAGS)", StringComparison.Ordinal))
            {
                string header = File.ReadAllText(Path.Combine(include, "um", "minwinbase.h"));
                var groups = Regex.Matches(header, @"(?m)^_Win32_FlagsEnum_\(LOCAL_ALLOC_FLAGS,[^\r\n]*\);");
                Assert.AreEqual(1, groups.Count, "The sliced LocalAlloc declaration requires its actual SDK enum dependency.");
                localFlags = string.Join("\n", Regex.Matches(header,
                    @"(?m)^#define (?:LMEM_[A-Z_]+|LHND|LPTR|NONZEROLHND|NONZEROLPTR)\b[^\r\n]*")
                    .Select(match => match.Value)) + "\n" + groups[0].Value + "\n";
            }
            return @"
#include <specstrings.h>
#define WINAPI __stdcall
#define WINBASEAPI
#define DECLSPEC_ALLOCATOR
typedef void* HANDLE;
typedef HANDLE HLOCAL;
typedef HANDLE* LPHANDLE;
typedef unsigned short* LPWSTR;
typedef const unsigned short* LPCWSTR;
typedef int BOOL;
typedef unsigned int UINT;
typedef __SIZE_TYPE__ SIZE_T;
typedef void VOID;
typedef struct _PRINTER_DEFAULTSW* LPPRINTER_DEFAULTSW;
" + localFlags + Declaration(printer, "ClosePrinter", "BOOL") + "\n" +
                Declaration(printer, "OpenPrinterW", "BOOL") + "\n" +
                Declaration(memory, "LocalFree", "WINBASEAPI") + "\n" +
                Declaration(memory, "LocalAlloc", "WINBASEAPI") + "\n" +
                Declaration(heap, "GetProcessHeap", "WINBASEAPI");
        }

        private static string Declaration(string header, string name, string prefix)
        {
            var declarations = Regex.Matches(header, @"(?m)^(?:_Win32_RAIIFree_\([^\r\n]*\)\r?\n)?" +
                prefix + @"\r?\n[^;{}]*?\b" + name + @"\s*\([^;{}]*\);")
                .Select(match => match.Value.Replace("\r\n", "\n")).Distinct().ToArray();
            Assert.AreEqual(1, declarations.Length, name);
            return declarations[0];
        }

        private static void AssertFinalFacts(string path)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            ProducerHeaderTests.AssertFacts(reader, "OpenPrinterW", 2, "ClosePrinter", -1, 0);
            ProducerHeaderTests.AssertFacts(reader, "LocalAlloc", 0, "LocalFree", 0);
            foreach (string name in new[] { "ClosePrinter", "LocalFree", "GetProcessHeap" })
                ProducerHeaderTests.AssertNoOwnership(reader, name);
            Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute)
                .Any(attribute => ProducerHeaderTests.Name(reader, attribute) is
                    "CanReturnErrorsAsSuccessAttribute" or "NativeAnnotationAttribute" or "DoNotReleaseAttribute"));
            var signatures = new Dictionary<string, SignatureTypeCode[]>
            {
                ["ClosePrinter"] = new[] { SignatureTypeCode.Int32, SignatureTypeCode.Pointer, SignatureTypeCode.Void },
                ["LocalFree"] = new[] { SignatureTypeCode.Pointer, SignatureTypeCode.Void, SignatureTypeCode.Pointer, SignatureTypeCode.Void },
                ["GetProcessHeap"] = new[] { SignatureTypeCode.Pointer, SignatureTypeCode.Void },
            };
            foreach (var fact in signatures)
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == fact.Key);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(fact.Key == "GetProcessHeap" ? 0 : 1, signature.ReadCompressedInteger());
                foreach (var code in fact.Value) Assert.AreEqual(code, signature.ReadSignatureTypeCode(), fact.Key);
                Assert.AreEqual(0, signature.RemainingBytes);
            }
            var localAlloc = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "LocalAlloc");
            var localSignature = reader.GetBlobReader(localAlloc.Signature);
            localSignature.ReadSignatureHeader();
            Assert.AreEqual(2, localSignature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Pointer, localSignature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Void, localSignature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.TypeHandle, localSignature.ReadSignatureTypeCode());
            Assert.AreEqual("LOCAL_ALLOC_FLAGS", EnumHeaderTests.TypeName(reader, localSignature.ReadTypeHandle()));
            Assert.AreEqual(SignatureTypeCode.UIntPtr, localSignature.ReadSignatureTypeCode());
            Assert.AreEqual(0, localSignature.RemainingBytes);
            var localFlags = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Single(type => reader.GetString(type.Name) == "LOCAL_ALLOC_FLAGS");
            var value = reader.GetFieldDefinition(localFlags.GetFields()
                .Single(handle => reader.GetString(reader.GetFieldDefinition(handle).Name) == "value__"));
            var storage = reader.GetBlobReader(value.Signature);
            storage.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.UInt32, storage.ReadSignatureTypeCode());
            Assert.AreEqual(0, storage.RemainingBytes);
            var open = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "OpenPrinterW");
            var openSignature = reader.GetBlobReader(open.Signature);
            openSignature.ReadSignatureHeader();
            Assert.AreEqual(3, openSignature.ReadCompressedInteger());
            foreach (var code in new[] { SignatureTypeCode.Int32, SignatureTypeCode.Pointer, SignatureTypeCode.UInt16,
                SignatureTypeCode.Pointer, SignatureTypeCode.Pointer, SignatureTypeCode.Void, SignatureTypeCode.Pointer,
                SignatureTypeCode.TypeHandle })
                Assert.AreEqual(code, openSignature.ReadSignatureTypeCode());
            openSignature.ReadTypeHandle();
            Assert.AreEqual(0, openSignature.RemainingBytes);
        }

        private static string Sdk(string path) => Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
            path.Replace('/', Path.DirectorySeparatorChar));
        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";
    }
}
