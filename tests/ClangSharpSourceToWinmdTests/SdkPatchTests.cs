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
                ("post-midl/errhandlingapi.h.patch", "um", "errhandlingapi.h"),
                ("post-midl/handleapi.h.patch", "um", "handleapi.h"),
                ("post-midl/HtmlHelp.h.patch", "um", "HtmlHelp.h"),
                ("post-midl/webauthnplugin.h.patch", "um", "webauthnplugin.h"),
                ("pre-midl/ObjIdl.Idl.patch", "um", "ObjIdl.Idl"),
                ("pre-midl/ObjIdlbase.Idl.patch", "um", "ObjIdlbase.Idl"),
                ("pre-midl/dxgi.idl.patch", "shared", "dxgi.idl"),
            };
            string inputRoot = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string copy = "$include = " + Quote(inputRoot) + "; " +
                "$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) + " -HeaderRoot $include); " +
                "foreach ($owner in @(@{ Idl='um/ObjIdl.Idl'; Header='um/ObjIdl.h' }, " +
                "@{ Idl='um/ObjIdlbase.Idl'; Header='um/ObjIdlbase.h' }, " +
                "@{ Idl='shared/dxgi.idl'; Header='shared/dxgi.h' })) { " +
                "if (@($outputs | Where-Object { $_.Idl -ieq $owner.Idl -and $_.Header -ieq $owner.Header }).Count -ne 1) " +
                "{ throw \"Missing generated pilot owner: $($owner.Idl)\" } }; " +
                "foreach ($header in 'um/winhttp.h', 'um/combaseapi.h', 'um/objbase.h', 'um/fileapi.h') { " +
                "if ($outputs.Header -icontains $header) { throw \"Pilot direct header is regenerated: $header\" } }; ";
            foreach (var owner in owners)
                copy += "$destination = " + Quote(System.IO.Path.Combine(fixture.Headers, owner.Item2, owner.Item3)) +
                    "; New-Item -ItemType Directory -Force (Split-Path $destination) | Out-Null; " +
                    "Copy-Item -LiteralPath (Join-Path $include " +
                    Quote(owner.Item2 + "\\" + owner.Item3) + ") -Destination $destination; ";
            var copied = fixture.Run(copy);
            Assert.AreEqual(0, copied.ExitCode, copied.Output);
            string selections = string.Join("','", owners.Select(owner => owner.Item1));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: inputRoot);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: inputRoot);
            foreach (var owner in owners)
                CollectionAssert.AreEqual(File.ReadAllBytes(System.IO.Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", owner.Item2, owner.Item3)),
                    File.ReadAllBytes(System.IO.Path.Combine(fixture.Headers, owner.Item2, owner.Item3)), owner.Item1);
            StringAssert.Contains(File.ReadAllText(Script("RecompileIdlFilesForScraping.ps1")),
                "$SelectedPatches = Get-SdkHeaderPatchSelections");
        }

        [DataTestMethod]
        [DataRow("ObjIdl", false)]
        [DataRow("ObjIdlbase", false)]
        [DataRow("ObjIdlbase", true)]
        public void ExplicitAvailability_IdlSourceReplaysOnRealSelectedSdkOutput(string owner, bool authoredResult)
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = System.IO.Path.Combine(sdk, "c", "include", "10.0.28000.0");
            fixture.Header(owner + ".Idl", File.ReadAllBytes(System.IO.Path.Combine(include, "um", owner + ".Idl")));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("pre-midl", "pre-midl/" + owner + ".Idl.patch", true, patchRoot: patches, midlInputRoot: include);
            if (owner == "ObjIdl")
            {
                fixture.Header("ObjIdlbase.Idl", File.ReadAllBytes(System.IO.Path.Combine(include, "um", "ObjIdlbase.Idl")));
                fixture.Apply("pre-midl", "pre-midl/ObjIdlbase.Idl.patch", true, patchRoot: patches, midlInputRoot: include);
            }
            if (authoredResult)
            {
                string ownerPath = System.IO.Path.Combine(fixture.Headers, "um", owner + ".Idl");
                string original = File.ReadAllText(ownerPath);
                string annotated = original.Replace("[local]\r\n    HRESULT Read(",
                    "[local, annotation(\"_Win32_PreserveResult_\")]\r\n    HRESULT Read(");
                Assert.AreNotEqual(original, annotated, "Author only the real ISequentialStream::Read fixture.");
                File.WriteAllText(ownerPath, annotated);
            }
            string converted = System.IO.Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string idl = System.IO.Path.Combine(converted, owner.ToLowerInvariant() + ".idl");
            string header = System.IO.Path.Combine(fixture.Headers, "um", owner.ToLowerInvariant() + ".h");
            if (owner == "ObjIdl")
            {
                var conversion = fixture.Run("& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(System.IO.Path.Combine(fixture.Headers, "um", "ObjIdlbase.Idl")) + " " +
                    Quote(System.IO.Path.Combine(converted, "objidlbase.idl")));
                Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            }
            foreach (string dependency in new[] { "unknwnbase.idl", "wtypesbase.idl", "unknwn.idl", "wtypes.idl" })
            {
                var conversion = fixture.Run("& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(System.IO.Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency)) + " " +
                    Quote(System.IO.Path.Combine(converted, dependency)));
                Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            }
            var result = fixture.Run("& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                Quote(System.IO.Path.Combine(fixture.Headers, "um", owner + ".Idl")) + " " + Quote(idl) + "; " +
                ". " + Quote(Script("CommonUtils.ps1")) + "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); " +
                "& " + Quote(System.IO.Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(idl) + " /out " + Quote(converted) + " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(System.IO.Path.Combine(include, "um")) +
                " /I " + Quote(System.IO.Path.Combine(include, "shared")) +
                " /I " + Quote(System.IO.Path.Combine(include, "winrt")) +
                " /I " + Quote(System.IO.Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Assert.IsFalse(File.Exists(System.IO.Path.Combine(patches, "post-midl", owner + ".h.patch")));
            if (authoredResult)
            {
                string generated = File.ReadAllText(header);
                int marker = generated.IndexOf("_Win32_PreserveResult_", StringComparison.Ordinal);
                Assert.IsTrue(marker >= 0, "MIDL must preserve the authored result marker.");
                string method = generated.Substring(generated.LastIndexOf("virtual", marker, StringComparison.Ordinal));
                method = method.Substring(0, method.IndexOf(" = 0;", StringComparison.Ordinal) + " = 0;".Length);
                using var resultFixture = new LastErrorHeaderTests.Fixture();
                var scraped = resultFixture.Scrape("typedef long HRESULT; typedef unsigned long ULONG;\n" +
                    "#define STDMETHODCALLTYPE __stdcall\n" +
                    "struct __declspec(uuid(\"0c733a30-2a1c-11ce-ade5-00aa0044773d\")) ISequentialStream {\n" +
                    method + "\n};", sal: true);
                Assert.AreEqual(0, scraped.ExitCode, method + "\n" + scraped.Output);
                ResultHeaderTests.AssertResult(resultFixture.Emit(), "Read", true);
                return;
            }
            string expectedHeader = System.IO.Path.Combine(Root(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "um", owner + ".h");
            var expectedLines = File.ReadAllLines(expectedHeader);
            var actualLines = File.ReadAllLines(header);
            string differences = string.Join("\n", expectedLines.Zip(actualLines, (expected, actual) => (expected, actual))
                .Where(pair => pair.expected != pair.actual).Take(3).Select(pair => pair.expected + "\nACTUAL: " + pair.actual));
            CollectionAssert.AreEqual(File.ReadAllBytes(expectedHeader), File.ReadAllBytes(header), differences);
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                SupportedOSHeaderTests.AssertMidlAvailability(File.ReadAllText(header), File.ReadAllText(expectedHeader), target);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ExplicitAvailability_SelectedSdkNativeBuildIsANoOp(string arch, string language, bool urlmonFirst)
        {
            using var fixture = new Fixture();
            string include = System.IO.Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            foreach (string owner in new[] { "winhttp.h", "errhandlingapi.h", "handleapi.h", "combaseapi.h", "objbase.h",
                "HtmlHelp.h", "webauthnplugin.h", "coml2api.h", "Ole2.h", "OleCtl.h" })
                fixture.Header(owner, File.ReadAllBytes(System.IO.Path.Combine(include, "um", owner)));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("post-midl", string.Join("','", new[] { "winhttp.h", "errhandlingapi.h", "handleapi.h",
                "combaseapi.h", "objbase.h", "HtmlHelp.h", "webauthnplugin.h", "coml2api.h", "Ole2.h", "OleCtl.h" }
                .Select(owner => "post-midl/" + owner + ".patch")), true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "ObjIdl.h", "ObjIdlbase.h", "Unknwnbase.h", "Unknwn.h", "servprov.h", "ocidl.h", "urlmon.h" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)));
            File.Copy(System.IO.Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders",
                "shared", "win32metadata_annotations.h"), System.IO.Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            File.Copy(System.IO.Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"),
                System.IO.Path.Combine(fixture.Headers, "sal.h"));
            string source = System.IO.Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" + (urlmonFirst ? "#include <urlmon.h>\n" : "") +
                "#include <windows.h>\n#include <winhttp.h>\n#include <objbase.h>\n#include <ObjIdlbase.h>\n" +
                "#include <ole2.h>\n#include <olectl.h>\n#include <coml2api.h>\n#include <servprov.h>\n" +
                "#include <HtmlHelp.h>\n#include <webauthn.h>\n#include <webauthnplugin.h>\n#include <urlmon.h>\n" +
                "HWND (WINAPI *html_signature)(HWND, LPCWSTR, UINT, DWORD_PTR) = &HtmlHelpW;\n" +
                "HRESULT (WINAPI *plugin_signature)(REFCLSID, AUTHENTICATOR_STATE*) = &WebAuthNPluginGetAuthenticatorState;\n" +
                "DWORD (WINAPI *error_signature)(void) = &GetLastError;\n" +
                "BOOL (WINAPI *winhttp_signature)(void) = &WinHttpCheckPlatform;\n" +
                "DWORD (WINAPI *com_signature)(void) = &CoGetCurrentProcess;\n" +
                "HRESULT (WINAPI *storage_create)(const WCHAR*, DWORD, DWORD, DWORD, STGOPTIONS*, PSECURITY_DESCRIPTOR, REFIID, void**) = &StgCreateStorageEx;\n" +
                "HRESULT (WINAPI *storage_open)(const WCHAR*, DWORD, DWORD, DWORD, STGOPTIONS*, PSECURITY_DESCRIPTOR, REFIID, void**) = &StgOpenStorageEx;\n" +
                "HRESULT (WINAPI *ole_default)(REFCLSID, LPUNKNOWN, REFIID, LPVOID*) = &OleCreateDefaultHandler;\n" +
                "HRESULT (WINAPI *ole_embedding)(REFCLSID, LPUNKNOWN, DWORD, LPCLASSFACTORY, REFIID, LPVOID*) = &OleCreateEmbeddingHelper;\n" +
                "HRESULT (WINAPI *ole_stream)(LPSTREAM, REFIID, LPVOID*) = &OleLoadFromStream;\n" +
                "HRESULT (WINAPI *ole_font)(LPFONTDESC, REFIID, LPVOID*) = &OleCreateFontIndirect;\n" +
                "HRESULT (WINAPI *ole_picture)(LPPICTDESC, REFIID, BOOL, LPVOID*) = &OleCreatePictureIndirect;\n" +
                "HRESULT (WINAPI *ole_load)(LPSTREAM, LONG, BOOL, REFIID, LPVOID*) = &OleLoadPicture;\n" +
                "HRESULT (WINAPI *ole_path)(LPOLESTR, LPUNKNOWN, DWORD, OLE_COLOR, REFIID, LPVOID*) = &OleLoadPicturePath;\n" +
                "HRESULT (STDMETHODCALLTYPE *unknown_proxy)(IUnknown*, REFIID, void**) = &IUnknown_QueryInterface_Proxy;\n" +
                "HRESULT (WINAPI *url_activation)(REFCLSID, LPCWSTR, DWORD, DWORD, LPCWSTR, LPBINDCTX, DWORD, LPVOID, REFIID, LPVOID*) = &CoGetClassObjectFromURL;\n" +
                "HRESULT __stdcall BindHostCall(IBindHost* host, IMoniker* moniker, IBindCtx* context, IBindStatusCallback* callback, REFIID riid, void** output) {\n" +
                "#ifdef __cplusplus\nreturn host->MonikerBindToObject(moniker, context, callback, riid, output) + host->MonikerBindToStorage(moniker, context, callback, riid, output);\n" +
                "#else\nreturn IBindHost_MonikerBindToObject(host, moniker, context, callback, riid, output) + IBindHost_MonikerBindToStorage(host, moniker, context, callback, riid, output);\n" +
                "#endif\n}\n" +
                "HRESULT __stdcall FactoryCall(IClassFactory2* factory, IServiceProvider* provider, REFIID riid, void** output) {\n" +
                "#ifdef __cplusplus\nreturn factory->CreateInstanceLic(0, 0, riid, 0, output) + provider->QueryService(riid, riid, output);\n" +
                "#else\nreturn IClassFactory2_CreateInstanceLic(factory, 0, 0, riid, 0, output) + IServiceProvider_QueryService(provider, riid, riid, output);\n" +
                "#endif\n}\n");
            string obj = System.IO.Path.Combine(fixture.Path, "native.obj");
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { & $compiler /nologo /c " + language + " " + Quote(source) +
                " " + Quote("/Fo" + obj) + " $(if ($mode -ne 'baseline') { " +
                Quote("/I" + System.IO.Path.Combine(fixture.Headers, "um")) + "; " + Quote("/I" + fixture.Headers) +
                " }) \"/I$vcInclude\" " + Quote("/I" + System.IO.Path.Combine(include, "um")) +
                " " + Quote("/I" + System.IO.Path.Combine(include, "shared")) +
                " " + Quote("/I" + System.IO.Path.Combine(include, "ucrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] ordinary = File.ReadAllBytes(System.IO.Path.Combine(fixture.Path, "ordinary.obj"));
            byte[] metadata = File.ReadAllBytes(System.IO.Path.Combine(fixture.Path, "metadata.obj"));
            byte[] baseline = File.ReadAllBytes(System.IO.Path.Combine(fixture.Path, "baseline.obj"));
            Array.Clear(baseline, 4, 4);
            Array.Clear(ordinary, 4, 4);
            Array.Clear(metadata, 4, 4);
            CollectionAssert.AreEqual(baseline, ordinary);
            CollectionAssert.AreEqual(ordinary, metadata);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ShellItemOutput_NativeBuildIsANoOp(string arch, string language, bool shellFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string parent = ShellItemComOutputHeaderTests.PreRetirementCommit(Root());
            foreach (string owner in new[] { "ShObjIdl.h", "ShObjIdl_core.h", "ShlObj.h", "ShlObj_core.h" })
            {
                var original = LastErrorHeaderTests.Run("git", "-C", Root(), "cat-file", "--filters",
                    parent + ":generation/WinSDK/RecompiledIdlHeaders/um/" + owner);
                Assert.AreEqual(0, original.ExitCode, original.Output);
                Assert.IsTrue(original.Output.All(character => character <= 127), owner);
                fixture.Header(owner, Encoding.ASCII.GetBytes(original.Output));
            }
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (shellFirst ? "#include <shlobj.h>\n#include <urlmon.h>\n" : "#include <windows.h>\n#include <urlmon.h>\n") +
                "#include <shobjidl.h>\n#include <shlobj.h>\n" +
                "HRESULT (WINAPI *item_idlist)(PCIDLIST_ABSOLUTE, REFIID, void**) = &SHCreateItemFromIDList;\n" +
                "HRESULT (WINAPI *item_name)(PCWSTR, IBindCtx*, REFIID, void**) = &SHCreateItemFromParsingName;\n" +
                "HRESULT (WINAPI *item_parent)(PCIDLIST_ABSOLUTE, IShellFolder*, PCUITEMID_CHILD, REFIID, void**) = &SHCreateItemWithParent;\n" +
                "HRESULT (WINAPI *item_relative)(IShellItem*, PCWSTR, IBindCtx*, REFIID, void**) = &SHCreateItemFromRelativeName;\n" +
                "HRESULT (WINAPI *item_known)(REFKNOWNFOLDERID, DWORD, PCWSTR, REFIID, void**) = &SHCreateItemInKnownFolder;\n" +
                "HRESULT (WINAPI *item_object)(IUnknown*, REFIID, void**) = &SHGetItemFromObject;\n" +
                "HRESULT (WINAPI *item_data)(IDataObject*, DATAOBJ_GET_ITEM_FLAGS, REFIID, void**) = &SHGetItemFromDataObject;\n" +
                "HRESULT (WINAPI *array_data)(IDataObject*, REFIID, void**) = &SHCreateShellItemArrayFromDataObject;\n" +
                "HRESULT (WINAPI *array_item)(IShellItem*, REFIID, void**) = &SHCreateShellItemArrayFromShellItem;\n" +
                "HRESULT (WINAPI *known_folder)(REFKNOWNFOLDERID, KNOWN_FOLDER_FLAG, HANDLE, REFIID, void**) = &SHGetKnownFolderItem;\n" +
                "HRESULT (WINAPI *bind_parent)(PCIDLIST_ABSOLUTE, REFIID, void**, PCUITEMID_CHILD*) = &SHBindToParent;\n" +
                "HRESULT (WINAPI *bind_folder)(IShellFolder*, PCUIDLIST_RELATIVE, REFIID, void**, PCUITEMID_CHILD*) = &SHBindToFolderIDListParent;\n" +
                "HRESULT (WINAPI *bind_folder_ex)(IShellFolder*, PCUIDLIST_RELATIVE, IBindCtx*, REFIID, void**, PCUITEMID_CHILD*) = &SHBindToFolderIDListParentEx;\n" +
                "HRESULT (WINAPI *bind_object)(IShellFolder*, PCUIDLIST_RELATIVE, IBindCtx*, REFIID, void**) = &SHBindToObject;\n" +
                "HRESULT __stdcall TransferCall(ITransferSource* source, ITransferDestination* destination, IShellItem* item, REFIID riid, void** output, void** resources) {\n" +
                "#ifdef __cplusplus\nreturn source->OpenItem(item, 0, riid, output) + destination->CreateItem(0, 0, 0, 0, riid, output, riid, resources);\n" +
                "#else\nreturn ITransferSource_OpenItem(source, item, 0, riid, output) + ITransferDestination_CreateItem(destination, 0, 0, 0, 0, riid, output, riid, resources);\n" +
                "#endif\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            // Use identical include paths for old/new headers; MSVC orders UUID COMDATs by header identity.
            string replacements = string.Join("; ", new[] { "ShObjIdl_core.h", "ShlObj_core.h" }.Select(owner =>
                "Copy-Item -LiteralPath " + Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShellItemOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner(string target)
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShlObj_core.h" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(include, "um", owner)));
            const string selections = "pre-midl/ShObjIdl_core.idl.patch','post-midl/ShlObj_core.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShlObj_core.h" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl",
                "msxml.idl", "propidlbase.idl", "propidl.idl", "StructuredQueryCondition.idl", "shtypes.idl",
                "comcat.idl", "propsys.idl", "ObjectArray.idl", "shobjidl_core.idl" })
            {
                string input = dependency == "shobjidl_core.idl" ? Path.Combine(fixture.Headers, "um", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            string header = Path.Combine(fixture.Headers, "um", "shobjidl_core.h");
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); & " +
                Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, "shobjidl_core.idl")) + " /out " + Quote(converted) +
                " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "um", "ShObjIdl_core.h")), File.ReadAllBytes(header));
            ShellItemComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
        }

        [TestMethod]
        public void CoreComOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            foreach (string owner in new[] { "coml2api.h", "Ole2.h", "OleCtl.h",
                "ocidl.Idl", "ServProv.Idl", "Unknwnbase.Idl", "UrlMon.Idl" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(include, "um", owner)));
            string selections = "post-midl/coml2api.h.patch','post-midl/Ole2.h.patch','post-midl/OleCtl.h.patch'," +
                "'pre-midl/ocidl.Idl.patch','pre-midl/ServProv.Idl.patch','pre-midl/Unknwnbase.Idl.patch','pre-midl/UrlMon.Idl.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "coml2api.h", "Ole2.h", "OleCtl.h",
                "ocidl.Idl", "ServProv.Idl", "Unknwnbase.Idl", "UrlMon.Idl" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)),
                    File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl" })
            {
                string patched = Path.Combine(fixture.Headers, "um", dependency);
                string input = File.Exists(patched) ? patched :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            foreach (string owner in new[] { "unknwnbase", "unknwn", "servprov", "urlmon", "ocidl" })
                command += "& " + Quote(Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, owner + ".idl")) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", owner + ".h")) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                    " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            foreach (string owner in new[] { "Unknwnbase.h", "Unknwn.h", "servprov.h", "urlmon.h", "ocidl.h" })
            {
                string expected = Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner);
                string actual = Path.Combine(fixture.Headers, "um", owner);
                string differences = string.Join("\n", File.ReadAllLines(expected).Zip(File.ReadAllLines(actual))
                    .Where(pair => pair.First != pair.Second).Take(3)
                    .Select(pair => "EXPECTED: " + pair.First + "\nACTUAL: " + pair.Second));
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)),
                    File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner + "\n" + differences);
            }
            ComOutputHeaderTests.AssertShippedDeclarations("x86_64-pc-windows-msvc", Path.Combine(fixture.Headers, "um"));
            UrlMonComOutputHeaderTests.AssertShippedDeclarations("x86_64-pc-windows-msvc", Path.Combine(fixture.Headers, "um"));
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

        private static string Root() => LastErrorHeaderTests.RepositoryRoot();

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

            public void Apply(string phase, string selection, bool success, string version = Version, string? patchRoot = null,
                string? midlInputRoot = null)
            {
                var result = Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase {Quote(phase)} -HeaderRoot {Quote(Headers)} " +
                    $"-SdkVersion {Quote(version)} -PatchRoot {Quote(patchRoot ?? Patches)} -SelectedPatches @('{selection}')" +
                    (midlInputRoot == null ? "" : " -MidlInputRoot " + Quote(midlInputRoot)));
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
