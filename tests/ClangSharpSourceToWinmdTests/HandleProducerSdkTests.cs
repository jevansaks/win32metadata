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
            string current = File.ReadAllText(Sdk("emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(ApplySidecarDelta(original), current);
            Assert.AreEqual(NativeTypedefSdkTests.ApplyAutoTypesDelta(Encoding.UTF8.GetString(
                PropertyComOutputHeaderTests.ReadOriginalBytes("autoTypes.json", parent))),
                File.ReadAllText(Sdk("autoTypes.json")).Replace("\r\n", "\n"));
            foreach (string path in new[] { "scraper.settings.rsp", "functionPointerFixups.json",
                "RecompiledIdlHeaders/um/heapapi.h" })
                CollectionAssert.AreEqual(PropertyComOutputHeaderTests.ReadOriginalBytes(path, parent),
                    File.ReadAllBytes(Sdk(path)), "Unrelated legacy/native inputs must remain byte-exact: " + path);
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
                foreach (string owner in new[] { "WinBase.h", "winspool.h", "heapapi.h" })
                    File.Copy(Path.Combine(include, "um", owner), Path.Combine(directory, "um", owner));
                var apply = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                    "& " + Quote(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "ApplySDKPatches.ps1")) +
                    " -Phase post-midl -HeaderRoot " + Quote(directory) + " -MidlInputRoot " + Quote(include) +
                    " -SdkVersion '10.0.28000.2705' -SelectedPatches @('post-midl/WinBase.h.patch','post-midl/winspool.h.patch')");
                Assert.AreEqual(0, apply.ExitCode, apply.Output);
                foreach (string owner in new[] { "WinBase.h", "winspool.h", "heapapi.h" })
                    CollectionAssert.AreEqual(File.ReadAllBytes(Sdk("RecompiledIdlHeaders/um/" + owner)),
                        File.ReadAllBytes(Path.Combine(directory, "um", owner)), owner + ": complete native bytes, including EOF and CRLF");

                using var fixture = new LastErrorHeaderTests.Fixture();
                string declarations = Declarations(directory);
                string settings = "--include-directory\n" + Path.Combine(include, "shared") + "\n" + Path.Combine(include, "um");
                var result = fixture.Scrape(declarations, target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                var remaps = File.ReadAllLines(Sdk("emitter.settings.rsp"))
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
BOOL (WINAPI *openPrinter)(LPWSTR, void**, LPPRINTER_DEFAULTSW) = &OpenPrinterW;
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
            return @"
#include <specstrings.h>
#define WINAPI __stdcall
#define WINBASEAPI
#define DECLSPEC_ALLOCATOR
typedef void* HANDLE;
typedef HANDLE HLOCAL;
typedef HANDLE* LPHANDLE;
typedef unsigned short* LPWSTR;
typedef int BOOL;
typedef unsigned int UINT;
typedef __SIZE_TYPE__ SIZE_T;
typedef void VOID;
typedef struct _PRINTER_DEFAULTSW* LPPRINTER_DEFAULTSW;
" + Declaration(printer, "ClosePrinter", "BOOL") + "\n" +
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
                ["LocalAlloc"] = new[] { SignatureTypeCode.Pointer, SignatureTypeCode.Void, SignatureTypeCode.UInt32, SignatureTypeCode.UIntPtr },
            };
            foreach (var fact in signatures)
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == fact.Key);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(fact.Key == "LocalAlloc" ? 2 : fact.Key == "GetProcessHeap" ? 0 : 1, signature.ReadCompressedInteger());
                foreach (var code in fact.Value) Assert.AreEqual(code, signature.ReadSignatureTypeCode(), fact.Key);
                Assert.AreEqual(0, signature.RemainingBytes);
            }
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
