using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class SdkPatchTests
    {
        private const string Version = "10.0.28000.2705";

        [TestMethod]
        public void EmptySelection_DoesNotRequireOrModifyInputs()
        {
            using var fixture = new Fixture();
            var result = fixture.Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase pre-midl -HeaderRoot missing -SdkVersion other");
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [DataTestMethod]
        [DataRow("\n", false, false)]
        [DataRow("\r\n", false, false)]
        [DataRow("\r\n", true, false)]
        [DataRow("\r\n", false, true)]
        public void SelectedPhase_PreservesBytesAndRejectsDoubleApplication(string newline, bool bom, bool legacy)
        {
            using var fixture = new Fixture();
            byte[] prefix = bom ? Encoding.UTF8.GetPreamble() : legacy ? new byte[] { 0x81, 0xE9 } : Array.Empty<byte>();
            byte[] before = prefix.Concat(Encoding.ASCII.GetBytes("prefix" + newline + "before" + newline + "suffix" + newline)).ToArray();
            string target = fixture.Header("proof.h", before);
            fixture.Patch("post-midl/proof.h.patch", "um/proof.h");
            fixture.Apply("pre-midl", "post-midl/proof.h.patch", success: true);
            CollectionAssert.AreEqual(before, File.ReadAllBytes(target));
            fixture.Apply("post-midl", "post-midl/proof.h.patch", success: true);
            byte[] expected = prefix.Concat(Encoding.ASCII.GetBytes("prefix" + newline + "after" + newline + "suffix" + newline)).ToArray();
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(target));
            fixture.Apply("post-midl", "post-midl/proof.h.patch", success: false);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(target));
        }

        [DataTestMethod]
        [DataRow("version")]
        [DataRow("phase")]
        [DataRow("traversal")]
        [DataRow("outside")]
        [DataRow("owner")]
        [DataRow("missing")]
        [DataRow("extra-file")]
        [DataRow("rename")]
        [DataRow("duplicate")]
        [DataRow("mixed-newlines")]
        [DataRow("utf16")]
        [DataRow("post-idl")]
        [DataRow("malformed")]
        public void InvalidSelectionOrTarget_FailsWithoutWriting(string failure)
        {
            using var fixture = new Fixture();
            string target = fixture.Header("proof.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            string selection = "post-midl/proof.h.patch";
            string path = fixture.Patch(selection, "um/proof.h");
            string version = Version;
            switch (failure)
            {
                case "version": version = "10.0.26100.7705"; break;
                case "phase": selection = "unknown/proof.h.patch"; break;
                case "traversal": File.WriteAllText(path, File.ReadAllText(path).Replace("um/proof.h", "um/../proof.h")); break;
                case "outside": File.WriteAllText(path, File.ReadAllText(path).Replace("generation/WinSDK/RecompiledIdlHeaders/", "")); break;
                case "owner": File.WriteAllText(path, File.ReadAllText(path).Replace("um/proof.h", "um/other.h")); break;
                case "missing": File.Delete(target); break;
                case "extra-file": File.AppendAllText(path, File.ReadAllText(path).Substring(File.ReadAllText(path).IndexOf("diff --git", StringComparison.Ordinal))); break;
                case "rename": File.WriteAllText(path, File.ReadAllText(path).Replace("@@ ", "new file mode 100644\n@@ ")); break;
                case "duplicate": selection += "','post-midl/proof.h.patch"; break;
                case "mixed-newlines": File.WriteAllText(target, "prefix\r\nbefore\nsuffix\r\n"); break;
                case "utf16": File.WriteAllText(target, "prefix\nbefore\nsuffix\n", Encoding.Unicode); break;
                case "post-idl": selection = "post-midl/proof.idl.patch"; fixture.Patch(selection, "um/proof.idl"); break;
                case "malformed": File.AppendAllText(path, "@@ invalid hunk\n"); break;
                default: Assert.Fail(failure); break;
            }
            byte[]? before = File.Exists(target) ? File.ReadAllBytes(target) : null;
            fixture.Apply("post-midl", selection, success: false, version);
            if (before != null) CollectionAssert.AreEqual(before, File.ReadAllBytes(target));
            else Assert.IsFalse(File.Exists(target));
        }

        [TestMethod]
        public void FailedHunk_DoesNotPartiallyApplyPhase()
        {
            using var fixture = new Fixture();
            string first = fixture.Header("first.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            string second = fixture.Header("second.h", Encoding.ASCII.GetBytes("prefix\nstale\nsuffix\n"));
            fixture.Patch("post-midl/first.h.patch", "um/first.h");
            fixture.Patch("post-midl/second.h.patch", "um/second.h");
            fixture.Apply("post-midl", "post-midl/first.h.patch','post-midl/second.h.patch", success: false);
            Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(first));
            Assert.AreEqual("prefix\nstale\nsuffix\n", File.ReadAllText(second));
        }

        [DataTestMethod]
        [DataRow("pre-midl")]
        [DataRow("post-midl")]
        public void RegeneratedHeaders_AreRejectedUsingCompleteInputsBeforeAnyWrites(string phase)
        {
            using var fixture = new Fixture();
            string source = Path.Combine(fixture.Path, "sdk");
            Directory.CreateDirectory(Path.Combine(source, "um"));
            foreach (string owner in new[] { "parent", "child" })
            {
                File.WriteAllText(Path.Combine(source, "um", owner + ".idl"),
                    owner == "parent" ? "import \"child.idl\";\n" : "typedef int Child;\n");
                File.WriteAllText(Path.Combine(source, "um", owner + ".h"), "/* File created by MIDL compiler */\n");
            }
            string first = fixture.Header("direct.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            string generated = fixture.Header("child.h", File.ReadAllBytes(Path.Combine(source, "um", "child.h")));
            fixture.Patch(phase + "/direct.h.patch", "um/direct.h");
            fixture.Patch(phase + "/child.h.patch", "um/child.h");
            var result = fixture.Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase {Quote(phase)} " +
                $"-HeaderRoot {Quote(fixture.Headers)} -MidlInputRoot {Quote(source)} -SdkVersion {Quote(Version)} " +
                $"-PatchRoot {Quote(fixture.Patches)} -SelectedPatches @('{phase}/direct.h.patch','{phase}/child.h.patch')");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "authoritative IDL um/child.idl");
            Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(first));
            Assert.AreEqual("/* File created by MIDL compiler */\n", File.ReadAllText(generated));
        }

        [DataTestMethod]
        [DataRow("winsync", false, true)]
        [DataRow("filterpipeline", false, true)]
        [DataRow("wincrypt", true, true)]
        [DataRow("direct", true, false)]
        public void DirectHeaders_RemainAllowedDespiteHistoricalBannerOrExcludedIdl(string owner, bool idl, bool banner)
        {
            using var fixture = new Fixture();
            string before = (banner ? "/* File created by MIDL compiler */" : "prefix") + "\nbefore\nsuffix\n";
            string target = fixture.Header(owner + ".h", Encoding.ASCII.GetBytes(before));
            if (idl) fixture.Header(owner + ".idl", Encoding.ASCII.GetBytes("typedef int Direct;\n"));
            fixture.Patch("post-midl/" + owner + ".h.patch", "um/" + owner + ".h");
            fixture.Apply("post-midl", "post-midl/" + owner + ".h.patch", success: true);
            Assert.AreEqual(before.Replace("\nbefore\n", "\nafter\n"), File.ReadAllText(target));
        }

        [TestMethod]
        public void CapturedOutputMap_RejectsRenamedHeadersWithoutGuessingOwnerBasename()
        {
            using var fixture = new Fixture();
            string target = fixture.Header("renamed.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            fixture.Patch("post-midl/renamed.h.patch", "um/renamed.h");
            var result = fixture.Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase post-midl " +
                $"-HeaderRoot {Quote(fixture.Headers)} -SdkVersion {Quote(Version)} -PatchRoot {Quote(fixture.Patches)} " +
                "-SelectedPatches @('post-midl/renamed.h.patch') " +
                "-MidlOutputs @(@{ Idl='shared/authoritative.idl'; Header='UM/RENAMED.H' })");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "shared/authoritative.idl");
            Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(target));
        }

        [TestMethod]
        public void SelectedSdk_ClassificationDistinguishesActualOutputsFromHistoricalBanners()
        {
            using var fixture = new Fixture();
            var result = fixture.Run(
                ". " + Quote(Script("CommonUtils.ps1")) + "; " +
                "$sdk = Get-NugetPropsProperty -name PkgMicrosoft_Windows_SDK_CPP -projectName ClangSharpSourceToWinmdTests.csproj; " +
                "$include = Join-Path $sdk 'c\\include\\10.0.28000.0'; " +
                "if (!(Test-Path -LiteralPath $include)) { throw \"Required SDK inputs missing: $include\" }; " +
                "$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) + " -HeaderRoot $include); " +
                "foreach ($owner in 'objidl', 'objidlbase') { " +
                "if (@($outputs | Where-Object { $_.Header -ieq \"um/$owner.h\" }).Count -ne 1) { throw \"Missing actual output: $owner\" } }; " +
                "foreach ($owner in 'winsync', 'filterpipeline', 'sapi51', 'sapi53', 'imapi', 'IE12Plugin', 'Ole2', 'OleCtl', 'coml2api', 'wincrypt') { " +
                "if (!(Test-Path -LiteralPath (Join-Path $include \"um\\$owner.h\"))) { throw \"Expected direct header missing: $owner\" }; " +
                "if ($outputs.Header -icontains \"um/$owner.h\") { throw \"Incorrectly classified direct header: $owner\" } }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [TestMethod]
        public void IncludedIdlFact_SurvivesActualMidlInMultipleQueuedOwners()
        {
            using var fixture = new Fixture();
            fixture.Header("child.idl", Encoding.ASCII.GetBytes("cpp_quote(\"// prefix\")\ncpp_quote(\"// before\")\ncpp_quote(\"// suffix\")\n"));
            fixture.Header("imported.idl", Encoding.ASCII.GetBytes("typedef int Imported;\n"));
            foreach (string owner in new[] { "parent", "second" })
            {
                fixture.Header(owner + ".idl", Encoding.ASCII.GetBytes(
                    "import \"imported.idl\";\n#include \"child.idl\"\n" +
                    "[uuid(431fcd36-4281-4fa2-b194-895046acde58)]\ninterface " + owner + " { void Method([in] Imported value); }\n"));
                fixture.Header(owner + ".h", Encoding.ASCII.GetBytes("/* File created by MIDL compiler */\n"));
            }
            string patch = fixture.Patch("pre-midl/child.idl.patch", "um/child.idl");
            File.WriteAllText(patch, File.ReadAllText(patch)
                .Replace("-before\n+after\n suffix\n", "-cpp_quote(\"// before\")\n+cpp_quote(\"// after\")\n cpp_quote(\"// suffix\")\n"));
            fixture.Apply("pre-midl", "pre-midl/child.idl.patch", success: true);
            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            var result = fixture.Run(
                ". " + Quote(Script("CommonUtils.ps1")) + "; " +
                "$sdk = Get-NugetPropsProperty -name PkgMicrosoft_Windows_SDK_CPP -projectName ClangSharpSourceToWinmdTests.csproj; " +
                "$midl = Join-Path $sdk 'c\\bin\\10.0.28000.0\\x86\\midl.exe'; " +
                "if (!(Test-Path -LiteralPath $midl)) { throw \"Required SDK MIDL missing: $midl\" }; " +
                "$env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); " +
                "$inputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) + " -HeaderRoot " + Quote(fixture.Headers) + " -IncludeUnqueued); " +
                "foreach ($input in $inputs) { & " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) +
                " (Join-Path " + Quote(fixture.Headers) + " $input.Idl) (Join-Path " + Quote(converted) + " ([IO.Path]::GetFileName($input.Idl))) }; " +
                "$outputs = @($inputs | Where-Object { $_.Header }); if ($outputs.Count -ne 2) { throw 'Incorrect owner queue' }; " +
                "foreach ($output in $outputs) { & $midl (Join-Path " + Quote(converted) + " ([IO.Path]::GetFileName($output.Idl))) " +
                "/out " + Quote(converted) + " /header (Join-Path " + Quote(fixture.Headers) + " $output.Header) " +
                "/no_warn /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8 /I " + Quote(converted) +
                "; if ($LASTEXITCODE -ne 0) { throw \"MIDL failed: $LASTEXITCODE\" } }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            foreach (string owner in new[] { "parent", "second" })
                StringAssert.Contains(File.ReadAllText(Path.Combine(fixture.Headers, "um", owner + ".h")), "// after");
        }

        [TestMethod]
        public void ReparseTarget_IsRejectedWithoutFollowingLink()
        {
            using var fixture = new Fixture();
            string outside = Path.Combine(fixture.Path, "outside");
            Directory.CreateDirectory(outside);
            string target = Path.Combine(outside, "proof.h");
            File.WriteAllText(target, "prefix\nbefore\nsuffix\n");
            string link = Path.Combine(fixture.Headers, "um");
            var result = fixture.Run($"New-Item -ItemType Junction -Path {Quote(link)} -Target {Quote(outside)} | Out-Null");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            try
            {
                fixture.Patch("post-midl/proof.h.patch", "um/proof.h");
                fixture.Apply("post-midl", "post-midl/proof.h.patch", success: false);
                Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(target));
            }
            finally { Directory.Delete(link); }
        }

        [TestMethod]
        public void ShippedProofs_OnlyAddCommentsAndPreMidlSurvivesConversion()
        {
            using var fixture = new Fixture();
            string sdk = Path.Combine(Root(), "generation", "WinSDK");
            foreach (string owner in new[] { "amstream.idl", "Uxtheme.h" })
            {
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(sdk, "RecompiledIdlHeaders", "um", owner)));
            }
            string proofs = Path.Combine(Root(), "tests", "ClangSharpSourceToWinmdTests", "SdkPatchFixtures");
            string selection = "pre-midl/amstream.idl.patch','post-midl/Uxtheme.h.patch";
            fixture.Apply("pre-midl", selection, true, patchRoot: proofs);
            string idl = Path.Combine(fixture.Headers, "um", "amstream.idl");
            string converted = Path.Combine(fixture.Path, "converted.idl");
            var result = fixture.Run($"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(idl)} {Quote(converted)}");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(File.ReadAllText(converted), "cpp_quote(\"// [win32metadata sdk patch] Metadata marker for OUTPUT_STATE enum\")");
            fixture.Apply("post-midl", selection, true, patchRoot: proofs);
            foreach (string owner in new[] { "amstream.idl", "Uxtheme.h" })
            {
                string original = File.ReadAllText(Path.Combine(sdk, "RecompiledIdlHeaders", "um", owner)).Replace("\r\n", "\n");
                string[] lines = File.ReadAllLines(Path.Combine(fixture.Headers, "um", owner));
                string[] added = lines.Where(line => line.Contains("[win32metadata sdk patch]", StringComparison.Ordinal)).ToArray();
                Assert.AreEqual(1, added.Length);
                Assert.IsTrue(added[0].StartsWith("//", StringComparison.Ordinal) ||
                    added[0].StartsWith("cpp_quote(\"//", StringComparison.Ordinal));
                Assert.AreEqual(original, string.Join("\n", lines.Where(line => !line.Contains("[win32metadata sdk patch]", StringComparison.Ordinal))) + "\n");
            }
        }

        [TestMethod]
        public void ParameterPilot_AllSelectedOwnersReplayByteForByte()
        {
            using var fixture = new Fixture();
            var owners = new[]
            {
                ("post-midl/winhttp.h.patch", "um", "winhttp.h"),
                ("post-midl/combaseapi.h.patch", "um", "combaseapi.h"),
                ("post-midl/objbase.h.patch", "um", "objbase.h"),
                ("post-midl/fileapi.h.patch", "um", "fileapi.h"),
                ("pre-midl/ObjIdl.Idl.patch", "um", "ObjIdl.Idl"),
                ("pre-midl/ObjIdlbase.Idl.patch", "um", "ObjIdlbase.Idl"),
                ("pre-midl/dxgi.idl.patch", "shared", "dxgi.idl"),
            };
            string copy = ". " + Quote(Script("CommonUtils.ps1")) + "; $sdk = Get-WinSdkCppPkgPath; ";
            foreach (var owner in owners)
                copy += "$destination = " + Quote(System.IO.Path.Combine(fixture.Headers, owner.Item2, owner.Item3)) +
                    "; New-Item -ItemType Directory -Force (Split-Path $destination) | Out-Null; " +
                    "Copy-Item -LiteralPath (Join-Path $sdk " +
                    Quote("c\\include\\10.0.28000.0\\" + owner.Item2 + "\\" + owner.Item3) + ") -Destination $destination; ";
            var copied = fixture.Run(copy);
            Assert.AreEqual(0, copied.ExitCode, copied.Output);
            string selections = string.Join("','", owners.Select(owner => owner.Item1));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("pre-midl", selections, true, patchRoot: patches);
            fixture.Apply("post-midl", selections, true, patchRoot: patches);
            foreach (var owner in owners)
                CollectionAssert.AreEqual(File.ReadAllBytes(System.IO.Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", owner.Item2, owner.Item3)),
                    File.ReadAllBytes(System.IO.Path.Combine(fixture.Headers, owner.Item2, owner.Item3)), owner.Item1);
            StringAssert.Contains(File.ReadAllText(Script("RecompileIdlFilesForScraping.ps1")),
                "$SelectedPatches = Get-SdkHeaderPatchSelections");
        }

        [TestMethod]
        public void IngestionHooks_BindCopiedInputsAndFinalOutputs()
        {
            string script = File.ReadAllText(Script("RecompileIdlFilesForScraping.ps1"));
            int pre = script.IndexOf("-Phase pre-midl -HeaderRoot $recompiledIdlHeadersDir", StringComparison.Ordinal);
            int post = script.IndexOf("-Phase post-midl -HeaderRoot $recompiledIdlHeadersDir", StringComparison.Ordinal);
            Assert.IsTrue(pre > script.IndexOf("Copy-Item \"$windowsWin32ProjectRoot\\AdditionalHeaders", StringComparison.Ordinal));
            Assert.IsTrue(pre < script.IndexOf("& $PSScriptRoot\\ConvertMidlAttributesToSalAnnotations.ps1", StringComparison.Ordinal));
            Assert.IsTrue(script.IndexOf("$midlOutputs = @($midlInputs", StringComparison.Ordinal) < pre);
            StringAssert.Contains(script, "$midlOutputs | ForEach-Object -ThrottleLimit");
            StringAssert.Contains(script, "Copy-Item $outputHeader $destination");
            Assert.AreEqual(2, script.Split("-MidlOutputs $midlOutputs").Length - 1);
            Assert.IsTrue(post > script.LastIndexOf("Copy-Item $outputHeader", StringComparison.Ordinal));
            Assert.IsTrue(post > script.LastIndexOf("git restore", StringComparison.Ordinal));
            Assert.AreEqual(2, script.Split("-SdkVersion ([IO.Path]::GetFileName($cppPkgPath)) -SelectedPatches $SelectedPatches").Length - 1);
            StringAssert.Contains(File.ReadAllText(Script("UpdateSDK.ps1")),
                ". \"$PSScriptRoot\\RecompileIdlFilesForScraping.ps1\" -SelectedPatches $SelectedPatches");
        }

        [TestMethod]
        public void CiLane_IsParallelSelectiveAndEnforcedByExistingRequiredGate()
        {
            string workflow = File.ReadAllText(Path.Combine(Root(), ".github", "workflows", "pr-validation.yml")).Replace("\r\n", "\n");
            string job = Regex.Match(workflow, @"^  sdk-patch-tests:\n(?<job>.*?)(?=^  \S)", RegexOptions.Multiline | RegexOptions.Singleline).Groups["job"].Value;
            StringAssert.Contains(job, "needs: [changes]");
            StringAssert.Contains(job, "if: needs.changes.outputs.sdk_patches_needed == 'true'");
            StringAssert.Contains(job, "--filter 'FullyQualifiedName~ClangSharpSourceToWinmdTests.SdkPatchTests'");
            string project = File.ReadAllText(Path.Combine(Root(), "tests", "ClangSharpSourceToWinmdTests", "ClangSharpSourceToWinmdTests.csproj"));
            StringAssert.Contains(project, "<PackageReference Include=\"Microsoft.Windows.SDK.CPP\" Version=\"" + Version + "\"");
            StringAssert.Contains(project, "GeneratePathProperty=\"true\" IncludeAssets=\"none\" PrivateAssets=\"all\"");
            Assert.IsFalse(job.Contains("dotnet restore BuildTools", StringComparison.Ordinal));
            Assert.IsFalse(job.Contains("continue-on-error:", StringComparison.Ordinal));
            StringAssert.Contains(File.ReadAllText(Script("TestTooling.ps1")),
                "'FullyQualifiedName!~ClangSharpSourceToWinmdTests.SdkPatchTests'");
            StringAssert.Contains(workflow, "const sdkPatchesNeeded = fileListIncomplete || changedPaths.some");
            StringAssert.Contains(workflow, "file.previous_filename, file.filename");
            foreach (string path in new[] { "scripts/ApplySDKPatches.ps1", "scripts/UpdateSDK.ps1",
                "scripts/RecompileIdlFilesForScraping.ps1", "scripts/Get-SDKMidlOutputs.ps1", "generation/WinSDK/patches/",
                "tests/ClangSharpSourceToWinmdTests/SdkPatchFixtures/" })
                StringAssert.Contains(workflow, $"'{path}'");
            StringAssert.Contains(workflow, "needs: [changes, workflow-tests, tooling-tests, scrape, build-test, api-diff, sdk-patch-tests]");

            Match block = Regex.Match(workflow,
                @"name: Require SDK tests for ingestion changes.*?        run: \|\n(?<script>(?:          [^\n]*\n)+)",
                RegexOptions.Singleline);
            Assert.IsTrue(block.Success);
            string script = Regex.Replace(block.Groups["script"].Value, "^          ", "", RegexOptions.Multiline);
            var cases = new[]
            {
                new { needed = "true", result = "success", fails = false },
                new { needed = "false", result = "skipped", fails = false },
                new { needed = "true", result = "failure", fails = true },
                new { needed = "true", result = "skipped", fails = true },
                new { needed = "true", result = "cancelled", fails = true },
                new { needed = "true", result = "", fails = true },
                new { needed = "", result = "success", fails = true },
                new { needed = "false", result = "success", fails = true },
                new { needed = "false", result = "", fails = true }
            };
            using var fixture = new Fixture();
            var result = fixture.Run(
                $"$gate = [scriptblock]::Create({Quote(script)}); $cases = {Quote(JsonSerializer.Serialize(cases))} | ConvertFrom-Json; " +
                "foreach ($case in $cases) { " +
                "$env:SDK_PATCHES_NEEDED = $case.needed; " +
                "$env:NEEDS = @{ 'sdk-patch-tests' = @{ result = $case.result } } | ConvertTo-Json; " +
                "$failed = $false; try { & $gate } catch { $failed = $true }; " +
                "if ($failed -ne $case.fails) { throw \"Unexpected SDK gate outcome: $($case | ConvertTo-Json -Compress)\" } }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        private static string Root()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !File.Exists(System.IO.Path.Combine(directory.FullName, "DoAll.ps1"))) directory = directory.Parent;
            return directory?.FullName ?? throw new InvalidOperationException("Repository root not found.");
        }

        private static string Script(string name) => System.IO.Path.Combine(Root(), "scripts", name);
        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";

        private sealed class Fixture : IDisposable
        {
            public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "sdk-patch-tests-" + Guid.NewGuid().ToString("N"));
            public string Headers => System.IO.Path.Combine(Path, "headers");
            public string Patches => System.IO.Path.Combine(Path, "patches");

            public Fixture() { Directory.CreateDirectory(Headers); Directory.CreateDirectory(Patches); }
            public string Header(string owner, byte[] bytes)
            {
                string file = System.IO.Path.Combine(Headers, "um", owner);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(file)!);
                File.WriteAllBytes(file, bytes);
                return file;
            }

            public string Patch(string selection, string target)
            {
                string file = System.IO.Path.Combine(Patches, selection.Replace('/', System.IO.Path.DirectorySeparatorChar));
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(file)!);
                string path = "generation/WinSDK/RecompiledIdlHeaders/" + target;
                File.WriteAllText(file, $"# SDK-Version: {Version}\ndiff --git a/{path} b/{path}\n--- a/{path}\n+++ b/{path}\n@@ -2,2 +2,2 @@\n-before\n+after\n suffix\n");
                return file;
            }

            public void Apply(string phase, string selection, bool success, string version = Version, string? patchRoot = null)
            {
                var result = Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase {Quote(phase)} -HeaderRoot {Quote(Headers)} " +
                    $"-SdkVersion {Quote(version)} -PatchRoot {Quote(patchRoot ?? Patches)} -SelectedPatches @('{selection}')");
                Assert.AreEqual(success, result.ExitCode == 0, result.Output);
            }

            public (int ExitCode, string Output) Run(string command)
            {
                var info = new ProcessStartInfo("pwsh") { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, WorkingDirectory = Path };
                foreach (string argument in new[] { "-NoProfile", "-NonInteractive", "-Command", "$ErrorActionPreference = 'Stop'; " + command }) info.ArgumentList.Add(argument);
                using var process = Process.Start(info) ?? throw new InvalidOperationException("PowerShell did not start.");
                var output = process.StandardOutput.ReadToEndAsync();
                var error = process.StandardError.ReadToEndAsync();
                if (!process.WaitForExit(60000)) { process.Kill(true); throw new TimeoutException("SDK patch fixture timed out."); }
                return (process.ExitCode, output.GetAwaiter().GetResult() + error.GetAwaiter().GetResult());
            }

            public void Dispose() => Directory.Delete(Path, recursive: true);
        }
    }
}
