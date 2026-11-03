using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using ClangSharpSourceToWinmd;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class LastErrorHeaderTests
    {
        private const string Declaration = "int __stdcall Example(void* buffer, unsigned long count);";

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void HeaderToWinmd_LegacyHeaderBothAndNeither(string target)
        {
            using var fixture = new Fixture();
            byte[]? legacyOutput = null;
            foreach (var mode in new[] { (Header: false, Legacy: true), (Header: true, Legacy: false),
                (Header: true, Legacy: true), (Header: false, Legacy: false) })
            {
                string header = (mode.Header ? "_Win32_SetLastError_\n" : "") + Declaration;
                var scrape = fixture.Scrape(header, target, mode.Legacy);
                Assert.AreEqual(0, scrape.ExitCode, scrape.Output);
                string winmd = fixture.Emit();
                using var pe = new PEReader(File.OpenRead(winmd));
                MetadataReader reader = pe.GetMetadataReader();
                MethodDefinition method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == "Example");
                MethodImport import = method.GetImport();
                Assert.AreEqual(mode.Header || mode.Legacy,
                    import.Attributes.HasFlag(MethodImportAttributes.SetLastError));
                Assert.AreEqual("test.dll", reader.GetString(reader.GetModuleReference(import.Module).Name));
                Assert.AreEqual("Example", reader.GetString(import.Name));
                Assert.IsTrue(import.Attributes.HasFlag(MethodImportAttributes.CallingConventionWinApi));
                Assert.AreEqual(2, method.GetParameters().Count);
                BlobReader signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(2, signature.ReadCompressedInteger());
                Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
                Assert.AreEqual(0, signature.RemainingBytes);
                byte[] output = File.ReadAllBytes(winmd);
                if (legacyOutput == null) legacyOutput = output;
                if (mode.Header)
                    CollectionAssert.AreEqual(legacyOutput, output,
                        "Header relocation must preserve the complete winmd byte-for-byte.");
            }
        }

        [DataTestMethod]
        [DataRow("Win32Metadata.SetLastError()")]
        [DataRow("Win32Metadata.SetLastError(true)")]
        [DataRow("Win32Metadata.SetLastError=false")]
        [DataRow("Win32Metadata.SetLastError(")]
        [DataRow("Win32Metadata.Unknown")]
        public void OwnedPayloads_RejectMalformedValuedAndUnknown(string payload)
        {
            using var fixture = new Fixture();
            var result = fixture.Scrape($"__attribute__((annotate(\"{payload}\")))\n{Declaration}");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:2:");
            StringAssert.Contains(result.Output, payload);
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("__attribute__((annotate(\"Win32Metadata.SetLastError\", 1))) " + Declaration)]
        [DataRow("int __stdcall _Win32_SetLastError_ Example(void* buffer, unsigned long count);")]
        [DataRow(Declaration + "\n_Win32_SetLastError_ int field;")]
        [DataRow("int __stdcall Example(_Win32_SetLastError_ void* buffer, unsigned long count);")]
        [DataRow("struct Value { _Win32_SetLastError_ int field; };")]
        [DataRow("_Win32_SetLastError_ typedef int Alias;")]
        [DataRow("int __stdcall Example(void* buffer, unsigned long count) _Win32_SetLastError_;")]
        [DataRow("_Win32_SetLastError_ int Example() { return 0; }")]
        public void WrongTargetsArgumentsAndPlacement_AreSourceLocatedErrors(string header)
        {
            using var fixture = new Fixture();
            var result = fixture.Scrape(header);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void RedeclarationsRepeatedIncludesAndRemaps_PreserveOneImport(bool remap)
        {
            using var fixture = new Fixture();
            var result = fixture.Scrape(Declaration + "\n_Win32_SetLastError_\n" + Declaration,
                remap: remap ? "Renamed" : null, repeatInclude: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var methods = ImportedMethods(reader);
            Assert.AreEqual(1, methods.Length);
            Assert.AreEqual(remap ? "Renamed" : "Example", reader.GetString(methods[0].Name));
            Assert.AreEqual("Example", reader.GetString(methods[0].GetImport().Name));
            Assert.IsTrue(methods[0].GetImport().Attributes.HasFlag(MethodImportAttributes.SetLastError));
        }

        [TestMethod]
        public void UnrelatedAnnotations_AreNotOwnedAndDoNotEnableLastError()
        {
            using var fixture = new Fixture();
            var result = fixture.Scrape("__attribute__((annotate(\"SetLastError\")))\n" +
                "__attribute__((annotate(\"ThirdParty.Unknown\", 1)))\n" + Declaration);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            Assert.IsFalse(ImportedMethods(reader).Single()
                .GetImport().Attributes.HasFlag(MethodImportAttributes.SetLastError));
        }

        [TestMethod]
        public void CHeader_UsesTheSameProductionTransport()
        {
            using var fixture = new Fixture();
            var legacy = fixture.Scrape(Declaration, legacy: true, language: "c");
            Assert.AreEqual(0, legacy.ExitCode, legacy.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            var header = fixture.Scrape("_Win32_SetLastError_\n" + Declaration, language: "c");
            Assert.AreEqual(0, header.ExitCode, header.Output);
            byte[] actual = File.ReadAllBytes(fixture.Emit());
            CollectionAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void OrdinaryBuildHelpers_AreNoOps(string target)
        {
            using var fixture = new Fixture();
            var baseline = fixture.Scrape(Declaration, target, metadata: false);
            Assert.AreEqual(0, baseline.ExitCode, baseline.Output);
            byte[] baselineOutput = File.ReadAllBytes(fixture.Emit());
            var result = fixture.Scrape("_Win32_SetLastError_\n" + Declaration, target, metadata: false);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string winmd = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(winmd));
            var reader = pe.GetMetadataReader();
            Assert.IsFalse(ImportedMethods(reader).Single()
                .GetImport().Attributes.HasFlag(MethodImportAttributes.SetLastError));
            CollectionAssert.AreEqual(baselineOutput, File.ReadAllBytes(winmd));
        }

        [TestMethod]
        public void WinHttpOwningPatch_ReplaysOnlyTheReviewedHeaderEdits()
        {
            string root = RepositoryRoot();
            const string legacyReference = "86ed37b2846440297afb087c4718acd1a1fb4ad8";
            var ancestry = Run("git", "-C", root, "merge-base", "--is-ancestor", legacyReference, "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, "The frozen legacy fixture must be reachable from the tested commit. " + ancestry.Output);
            string current = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", "winhttp.h"));
            var origin = Run("git", "-C", root, "show", $"{legacyReference}:generation/WinSDK/RecompiledIdlHeaders/um/winhttp.h");
            Assert.AreEqual(0, origin.ExitCode, origin.Output);
            string directory = Path.Combine(Path.GetTempPath(), "winhttp-patch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path.Combine(directory, "um"));
            try
            {
                string header = Path.Combine(directory, "um", "winhttp.h");
                File.WriteAllText(header, origin.Output, new UTF8Encoding(false));
                var apply = Run("pwsh", "-NoProfile", "-NonInteractive", "-File",
                    Path.Combine(root, "scripts", "ApplySDKPatches.ps1"),
                    "-Phase", "post-midl", "-HeaderRoot", directory, "-SdkVersion", "10.0.28000.2705",
                    "-SelectedPatches", "post-midl/winhttp.h.patch");
                Assert.AreEqual(0, apply.ExitCode, apply.Output);
                Assert.AreEqual(current.Replace("\r\n", "\n"), File.ReadAllText(header).Replace("\r\n", "\n"));
                Assert.AreEqual(3, current.Split("_Win32_SetLastError_").Length - 1);
                var legacy = Run("git", "-C", root, "show", $"{legacyReference}:generation/WinSDK/WithSetLastError.rsp");
                Assert.AreEqual(0, legacy.ExitCode, legacy.Output);
                var removed = new[] { "WinHttpReadData", "WinHttpReceiveResponse", "WinHttpSendRequest" };
                string expected = string.Join("\n", legacy.Output.Replace("\r\n", "\n").Split('\n')
                    .Where(line => !removed.Contains(line)));
                Assert.AreEqual(expected, File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "WithSetLastError.rsp")).Replace("\r\n", "\n"));
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
        public void NativeCompiler_OrdinaryAndMetadataDefinesPreserveObjectAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language);
        }

        [DataTestMethod]
        [DataRow("_Win32_SetLastError_\n_Win32_SetLastError_\n")]
        [DataRow("__attribute__((annotate(\"ThirdParty.Annotation\")))\n_Win32_SetLastError_\n")]
        public void RepeatedAndNeighboringAnnotations_DoNotDuplicateBehavior(string prefix)
        {
            using var fixture = new Fixture();
            var result = fixture.Scrape(prefix + Declaration);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            Assert.IsTrue(ImportedMethods(pe.GetMetadataReader()).Single()
                .GetImport().Attributes.HasFlag(MethodImportAttributes.SetLastError));
        }

        internal sealed class Fixture : IDisposable
        {
            private readonly string directory = Path.Combine(Path.GetTempPath(), "last-error-header-" + Guid.NewGuid().ToString("N"));
            public string GeneratedSource => Path.Combine(this.directory, "generated", "Test.cs");
            public string DiscoveredRemaps => File.ReadAllText(Path.Combine(this.directory, "remaps.rsp"));

            public Fixture()
            {
                Directory.CreateDirectory(this.directory);
                Directory.CreateDirectory(Path.GetDirectoryName(this.GeneratedSource)!);
                File.Copy(Path.Combine(RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders",
                    "shared", "win32metadata_annotations.h"), Path.Combine(this.directory, "win32metadata_annotations.h"));
                File.Copy(Path.Combine(RepositoryRoot(), "generation", "WinSDK", "inc", "sal.h"),
                    Path.Combine(this.directory, "sal.h"));
                File.Copy(Path.Combine(RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders",
                    "shared", "concurrencysal.h"), Path.Combine(this.directory, "concurrencysal.h"));
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(this.GeneratedSource)!, "Metadata.manual.cs"),
                    "namespace Test { public class NativeTypeNameAttribute : System.Attribute { public NativeTypeNameAttribute(string name) {} } }");
            }

            public (int ExitCode, string Output) Scrape(string header, string target = "x86_64-pc-windows-msvc",
                bool legacy = false, string? remap = null, bool repeatInclude = false, bool metadata = true, string language = "c++",
                bool sal = false, bool transitive = false, string extraSettings = "", string? includedHeader = null)
            {
                if (File.Exists(this.GeneratedSource)) File.Delete(this.GeneratedSource);
                if (sal)
                {
                    File.Copy(Path.Combine(RepositoryRoot(), "generation", "WinSDK", "manual", "Metadata.cs"),
                        Path.Combine(Path.GetDirectoryName(this.GeneratedSource)!, "Attributes.manual.cs"), true);
                    File.WriteAllText(Path.Combine(Path.GetDirectoryName(this.GeneratedSource)!, "Usings.manual.cs"),
                        "global using Windows.Win32.Foundation.Metadata;");
                }
                File.WriteAllText(Path.Combine(this.directory, "input.h"), "#include \"win32metadata_annotations.h\"\n" +
                    (sal ? "#include \"sal.h\"\n" : "") + (transitive ? "#include \"inner.h\"\n" : header));
                if (transitive) File.WriteAllText(Path.Combine(this.directory, "inner.h"), header);
                if (includedHeader != null)
                {
                    File.WriteAllText(Path.Combine(this.directory, "inner.h"), includedHeader);
                    File.WriteAllText(Path.Combine(this.directory, "input.h"),
                        "#include \"inner.h\"\n" + File.ReadAllText(Path.Combine(this.directory, "input.h")));
                }
                File.WriteAllText(Path.Combine(this.directory, "main.cpp"),
                    "#ifdef __cplusplus\nextern \"C\" {\n#endif\n#include \"input.h\"\n" +
                    (repeatInclude ? "#include \"input.h\"\n" : "") + "#ifdef __cplusplus\n}\n#endif\n");
                string rsp = Path.Combine(this.directory, "settings.rsp");
                File.WriteAllText(rsp, $@"--file
{Path.Combine(this.directory, "main.cpp")}
--output
{this.GeneratedSource}
--language
{language}
--std
{(language == "c" ? "c17" : "c++17")}
--namespace
Test
--include-directory
{this.directory}
--traverse
{Path.Combine(this.directory, transitive ? "inner.h" : "input.h")}
--libraryPath
test.dll
--methodClassName
Apis
--config
compatible-codegen
generate-cpp-attributes
--additional
-target
{target}
{(sal ? "-D_USE_DECLSPECS_FOR_SAL=1\n-D_PREFAST_=1" : "")}
-Wno-ignored-attributes
{(metadata ? "-DWIN32METADATA" : "")}
{(legacy ? "--with-setlasterror\nExample" : "")}
{(remap != null ? "--remap\nExample=" + remap : "")}
{extraSettings}
");
                string scraper = Path.Combine(AppContext.BaseDirectory,
                    RuntimeInformation.RuntimeIdentifier, "Win32MetadataScraper.dll");
                Assert.IsTrue(File.Exists(scraper), "Missing production RID scraper: " + scraper);
                return Run("dotnet", scraper, Path.Combine(this.directory, "remaps.rsp"), "@" + rsp);
            }

            public string Emit(Dictionary<string, string>? memberRemaps = null)
            {
                var compilation = ClangSharpSourceCompilation.Create(
                    Path.GetDirectoryName(this.GeneratedSource), "crossarch",
                    memberRemaps ?? new Dictionary<string, string>(), new Dictionary<string, Dictionary<string, string>>(),
                    Array.Empty<string>(), new Dictionary<string, string>(), new Dictionary<string, string>(),
                    new HashSet<string>(), Array.Empty<string>(), new Dictionary<string, string>(), new Dictionary<string, string>());
                var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
                Assert.AreEqual(0, errors.Length, string.Join(Environment.NewLine, errors.Select(d => d.ToString())));
                string output = Path.Combine(this.directory, "Test.winmd");
                var generator = ClangSharpSourceWinmdGenerator.GenerateWindmdForCompilation(compilation,
                    new Dictionary<string, string>(), new HashSet<string>(), new HashSet<string>(),
                    new Version(1, 0, 0, 0), output);
                Assert.IsTrue(generator.WroteWinmd, string.Join(Environment.NewLine, generator.GetDiagnostics()));
                return output;
            }

            public Dictionary<string, string> ScrapeConstants(string? enums = null)
            {
                string path = Path.Combine(this.directory, "enums.json");
                if (enums != null) File.WriteAllText(path, enums);
                var remaps = new Dictionary<string, string>();
                MetadataUtils.ConstantsScraper.ScrapeConstants(enums == null ? Array.Empty<string>() : new[] { path },
                    "Test", Path.GetDirectoryName(this.GeneratedSource)!, "using System;", new HashSet<string>(),
                    new Dictionary<string, string> { [Path.Combine(this.directory, "input.h")] = "Test" },
                    new Dictionary<string, string>(), remaps, new Dictionary<string, string>(),
                    new Dictionary<string, string>());
                return remaps;
            }

            public void AssertNativeAbi(string arch, int pointerSize, string language)
            {
                string source = Path.Combine(this.directory, "abi.c");
                File.WriteAllText(source, $@"#include ""win32metadata_annotations.h""
typedef char VerifyPointerSize[(sizeof(void*) == {pointerSize}) ? 1 : -1];
#ifdef __cplusplus
extern ""C"" {{
#endif
_Win32_SetLastError_
{Declaration}
#define Low 1
#define High 0x80000000UL
_Win32_Enum_(Access, unsigned long, Low);
_Win32_FlagsEnum_(Options, unsigned long, Low, High);
enum Option {{ One = 1 }};
_Win32_AssociatedEnum_(Option) int __stdcall ParameterAbi(
    _Win32_Retained_ void* buffer, _Win32_Retval_ int* result,
    _Win32_ContainsInteriorPointers_ void* interior, _Win32_AssociatedEnum_(Option) unsigned long option);
typedef int (__stdcall *ExpectedParameterSignature)(void*, int*, void*, unsigned long);
ExpectedParameterSignature parameterSignatureCheck = &ParameterAbi;
typedef int (__stdcall *ExpectedSignature)(void*, unsigned long);
ExpectedSignature signatureCheck = &Example;
int __stdcall Caller(void) {{ return Example(0, 3); }}
#ifdef __cplusplus
}}
#endif
");
                string script = "$ErrorActionPreference = 'Stop'; . " + Quote(Path.Combine(RepositoryRoot(), "scripts", "CommonUtils.ps1")) +
                    "; $compiler = Join-Path (Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64) 'cl.exe'; " +
                    "foreach ($mode in 'ordinary', 'metadata') { " +
                    "$arguments = @('/nologo', '/c', " + Quote(language) + ", " + Quote(source) +
                    ", " + Quote("/Fo" + Path.Combine(this.directory, "abi.obj")) + "); " +
                    "if ($mode -eq 'metadata') { $arguments += '/DWIN32METADATA' }; " +
                    "& $compiler @arguments; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; " +
                    "Copy-Item -LiteralPath " + Quote(Path.Combine(this.directory, "abi.obj")) +
                    " -Destination (" + Quote(this.directory + Path.DirectorySeparatorChar) + " + $mode + '.obj') }";
                var result = Run("pwsh", "-NoProfile", "-NonInteractive", "-Command", script);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] ordinary = File.ReadAllBytes(Path.Combine(this.directory, "ordinary.obj"));
                byte[] metadata = File.ReadAllBytes(Path.Combine(this.directory, "metadata.obj"));
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(ordinary, 0));
                // COFF timestamps are not part of the native ABI; compare every other byte.
                Array.Clear(ordinary, 4, 4);
                Array.Clear(metadata, 4, 4);
                CollectionAssert.AreEqual(ordinary, metadata);
            }

            public void Dispose() => Directory.Delete(this.directory, true);
        }

        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";

        private static MethodDefinition[] ImportedMethods(MetadataReader reader) =>
            reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Single(type => reader.GetString(type.Name) == "Apis")
                .GetMethods().Select(reader.GetMethodDefinition).ToArray();

        internal static string RepositoryRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !File.Exists(Path.Combine(directory.FullName, "BuildTools", "BuildTools.proj")))
                directory = directory.Parent;
            return directory?.FullName ?? throw new InvalidOperationException("Cannot locate repository root.");
        }

        internal static string SdkPackagePath()
        {
            string script = ". " + Quote(Path.Combine(RepositoryRoot(), "scripts", "CommonUtils.ps1")) +
                "; Get-NugetPropsProperty -name PkgMicrosoft_Windows_SDK_CPP " +
                "-projectName ClangSharpSourceToWinmdTests.csproj 6>$null";
            var result = Run("pwsh", "-NoProfile", "-NonInteractive", "-Command", script);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = result.Output.Trim();
            Assert.AreEqual("10.0.28000.2705", Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar)));
            Assert.IsTrue(Directory.Exists(path), "Missing test project's declared SDK package: " + path);
            return path;
        }

        internal static (int ExitCode, string Output) Run(string executable, params string[] arguments)
        {
            using var process = new Process { StartInfo = new ProcessStartInfo(executable) {
                UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true } };
            foreach (string argument in arguments) process.StartInfo.ArgumentList.Add(argument);
            process.Start();
            var stdout = process.StandardOutput.ReadToEndAsync();
            var stderr = process.StandardError.ReadToEndAsync();
            process.WaitForExit();
            return (process.ExitCode, stdout.GetAwaiter().GetResult() + stderr.GetAwaiter().GetResult());
        }
    }
}
