using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        public void IngestionHooks_BindCopiedInputsAndFinalOutputs()
        {
            string script = File.ReadAllText(Script("RecompileIdlFilesForScraping.ps1"));
            int pre = script.IndexOf("-Phase pre-midl -HeaderRoot $recompiledIdlHeadersDir", StringComparison.Ordinal);
            int post = script.IndexOf("-Phase post-midl -HeaderRoot $recompiledIdlHeadersDir", StringComparison.Ordinal);
            Assert.IsTrue(pre > script.IndexOf("Copy-Item \"$windowsWin32ProjectRoot\\AdditionalHeaders", StringComparison.Ordinal));
            Assert.IsTrue(pre < script.IndexOf("& $PSScriptRoot\\ConvertMidlAttributesToSalAnnotations.ps1", StringComparison.Ordinal));
            Assert.IsTrue(post > script.LastIndexOf("Copy-Item $outputHeader", StringComparison.Ordinal));
            Assert.IsTrue(post > script.LastIndexOf("git restore", StringComparison.Ordinal));
            Assert.AreEqual(2, script.Split("-SdkVersion ([IO.Path]::GetFileName($cppPkgPath)) -SelectedPatches $SelectedPatches").Length - 1);
            StringAssert.Contains(File.ReadAllText(Script("UpdateSDK.ps1")),
                ". \"$PSScriptRoot\\RecompileIdlFilesForScraping.ps1\" -SelectedPatches $SelectedPatches");
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
            private string Patches => System.IO.Path.Combine(Path, "patches");

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
