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
            foreach (string owner in new[] { "propsys.h", "shellapi.h", "Shlwapi.h" })
            {
                string original = PropertyComOutputHeaderTests.ReadOriginal("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                fixture.Header(owner, Encoding.ASCII.GetBytes(original));
            }
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (shellFirst ? "#include <shlobj.h>\n#include <urlmon.h>\n" : "#include <windows.h>\n#include <urlmon.h>\n") +
                "#include <shobjidl.h>\n#include <shlobj.h>\n#include <propsys.h>\n#include <shellapi.h>\n#include <shlwapi.h>\n" +
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
                "HRESULT (WINAPI *ps_memory)(REFIID, void**) = &PSCreateMemoryPropertyStore;\n" +
                "HRESULT (WINAPI *ps_delayed)(GETPROPERTYSTOREFLAGS, IDelayedPropertyStoreFactory*, const DWORD*, DWORD, REFIID, void**) = &PSCreateDelayedMultiplexPropertyStore;\n" +
                "HRESULT (WINAPI *ps_multiplex)(IUnknown**, DWORD, REFIID, void**) = &PSCreateMultiplexPropertyStore;\n" +
                "HRESULT (WINAPI *ps_changes)(const PROPERTYKEY*, const PKA_FLAGS*, const PROPVARIANT*, UINT, REFIID, void**) = &PSCreatePropertyChangeArray;\n" +
                "HRESULT (WINAPI *ps_change)(PKA_FLAGS, REFPROPERTYKEY, REFPROPVARIANT, REFIID, void**) = &PSCreateSimplePropertyChange;\n" +
                "HRESULT (WINAPI *ps_description)(REFPROPERTYKEY, REFIID, void**) = &PSGetPropertyDescription;\n" +
                "HRESULT (WINAPI *ps_description_name)(LPCWSTR, REFIID, void**) = &PSGetPropertyDescriptionByName;\n" +
                "HRESULT (WINAPI *ps_handler)(IUnknown*, BOOL, REFIID, void**) = &PSGetItemPropertyHandler;\n" +
                "HRESULT (WINAPI *ps_handler_create)(IUnknown*, BOOL, IUnknown*, REFIID, void**) = &PSGetItemPropertyHandlerWithCreateObject;\n" +
                "HRESULT (WINAPI *ps_enumerate)(PROPDESC_ENUMFILTER, REFIID, void**) = &PSEnumeratePropertyDescriptions;\n" +
                "HRESULT (WINAPI *ps_description_list)(LPCWSTR, REFIID, void**) = &PSGetPropertyDescriptionListFromString;\n" +
                "HRESULT (WINAPI *ps_storage)(IPropertySetStorage*, DWORD, REFIID, void**) = &PSCreatePropertyStoreFromPropertySetStorage;\n" +
                "HRESULT (WINAPI *ps_object)(IUnknown*, DWORD, REFIID, void**) = &PSCreatePropertyStoreFromObject;\n" +
                "HRESULT (WINAPI *ps_adapter)(IPropertyStore*, REFIID, void**) = &PSCreateAdapterFromPropertyStore;\n" +
                "HRESULT (WINAPI *ps_system)(REFIID, void**) = &PSGetPropertySystem;\n" +
                "HRESULT (WINAPI *ps_bag)(IPropertyBag*, LPCWSTR, REFIID, void**) = &PSPropertyBag_ReadUnknown;\n" +
                "HRESULT (WINAPI *ps_idlist)(PCIDLIST_ABSOLUTE, GETPROPERTYSTOREFLAGS, REFIID, void**) = &SHGetPropertyStoreFromIDList;\n" +
                "HRESULT (WINAPI *ps_path)(PCWSTR, IBindCtx*, GETPROPERTYSTOREFLAGS, REFIID, void**) = &SHGetPropertyStoreFromParsingName;\n" +
                "HRESULT (WINAPI *ps_window)(HWND, REFIID, void**) = &SHGetPropertyStoreForWindow;\n" +
                "HRESULT (WINAPI *ps_view)(PCIDLIST_ABSOLUTE, PCWSTR, DWORD, REFIID, void**) = &SHGetViewStatePropertyBag;\n" +
                "HRESULT __stdcall TransferCall(ITransferSource* source, ITransferDestination* destination, IShellItem* item, REFIID riid, void** output, void** resources) {\n" +
                "#ifdef __cplusplus\nreturn source->OpenItem(item, 0, riid, output) + destination->CreateItem(0, 0, 0, 0, riid, output, riid, resources);\n" +
                "#else\nreturn ITransferSource_OpenItem(source, item, 0, riid, output) + ITransferDestination_CreateItem(destination, 0, 0, 0, 0, riid, output, riid, resources);\n" +
                "#endif\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            // Use identical include paths for old/new headers; MSVC orders UUID COMDATs by header identity.
            string replacements = string.Join("; ", new[] { "ShObjIdl_core.h", "ShlObj_core.h",
                "propsys.h", "shellapi.h", "Shlwapi.h" }.Select(owner =>
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
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShObjIdl.idl", "ShlObj_core.h", "ShlObj.h",
                "propsys.idl", "shellapi.h", "Shlwapi.h", "shdeprecated.h" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(include, "um", owner)));
            const string selections = "pre-midl/ShObjIdl_core.idl.patch','post-midl/ShlObj_core.h.patch'," +
                "'pre-midl/propsys.idl.patch','post-midl/shellapi.h.patch','post-midl/Shlwapi.h.patch'," +
                "'pre-midl/ShObjIdl.idl.patch','post-midl/ShlObj.h.patch','post-midl/shdeprecated.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShObjIdl.idl", "ShlObj_core.h", "ShlObj.h",
                "propsys.idl", "shellapi.h", "Shlwapi.h", "shdeprecated.h" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl",
                "msxml.idl", "propidlbase.idl", "propidl.idl", "StructuredQueryCondition.idl", "shtypes.idl",
                "comcat.idl", "propsys.idl", "ObjectArray.idl", "shobjidl_core.idl", "docobj.idl", "prsht.idl", "shobjidl.idl" })
            {
                string input = dependency == "shobjidl_core.idl" || dependency == "shobjidl.idl" || dependency == "propsys.idl" ? Path.Combine(fixture.Headers, "um", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            foreach (string owner in new[] { "propsys", "shobjidl_core", "shobjidl" })
            {
                string header = Path.Combine(fixture.Headers, "um", owner + ".h");
                command += "& " +
                Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, owner + ".idl")) + " /out " + Quote(converted) +
                " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            }
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            foreach (string owner in new[] { "propsys.h", "ShObjIdl_core.h", "ShObjIdl.h" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);
            ShellItemComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
            PropertyComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
            ShellComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
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
        public void ShellOutput_NativeBuildIsANoOp(string arch, string language, bool shellFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = ShellComOutputHeaderTests.Owners.Select(owner => owner.Owner).Append("propsys.h").ToArray();
            foreach (string owner in owners)
            {
                string original = ShellComOutputHeaderTests.ReadOriginal("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                fixture.Header(owner, Encoding.ASCII.GetBytes(original));
            }
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (shellFirst ? "#include <shlobj.h>\n#include <urlmon.h>\n" : "#include <windows.h>\n#include <urlmon.h>\n") +
                "#include <shobjidl.h>\n#include <shlobj.h>\n#include <propsys.h>\n#include <shellapi.h>\n#include <shlwapi.h>\n#include <shdeprecated.h>\n" +
                "HRESULT (WINAPI *association)(REFIID, void**) = &SHCreateAssociationRegistration;\n" +
                "HRESULT (WINAPI *icon)(REFIID, void**) = &SHCreateDefaultExtractIcon;\n" +
                "HRESULT (WINAPI *handlers)(PCWSTR, REFIID, void**) = &SHAssocEnumHandlersForProtocolByApplication;\n" +
                "HRESULT (WINAPI *activate)(PCWSTR, const CLSID*, IUnknown*, REFIID, void**) = &SHCoCreateInstance;\n" +
                "HRESULT (WINAPI *data)(PCIDLIST_ABSOLUTE, UINT, PCUITEMID_CHILD_ARRAY, IDataObject*, REFIID, void**) = &SHCreateDataObject;\n" +
                "HRESULT (WINAPI *context)(const DEFCONTEXTMENU*, REFIID, void**) = &SHCreateDefaultContextMenu;\n" +
                "HRESULT (WINAPI *file_icon)(LPCWSTR, DWORD, REFIID, void**) = &SHCreateFileExtractIconW;\n" +
                "HRESULT (WINAPI *classes)(const ASSOCIATIONELEMENT*, ULONG, REFIID, void**) = &AssocCreateForClasses;\n" +
                "HRESULT (WINAPI *images)(int, REFIID, void**) = &SHGetImageList;\n" +
                "HRESULT (WINAPI *assoc)(CLSID, REFIID, void**) = &AssocCreate;\n" +
                "HRESULT __stdcall ShellCalls(INameSpaceTreeControlEvents* tree, IShellMenu* menu, IQueryAssociations* assoc,\n" +
                "IDockingWindowFrame* dock, IBrowserService2* browser, IBrowserService3* browser3, IBrowserService4* browser4,\n" +
                "ITrackShellMenu* track, IShellItem* item, IContextMenu* context, REFIID riid, void** output, DWORD* flags, PIDLIST_ABSOLUTE* pidl) {\n" +
                "#ifdef __cplusplus\n" +
                "return tree->OnBeforeContextMenu(item, riid, output) + tree->OnAfterContextMenu(item, context, riid, output) +\n" +
                "menu->GetShellFolder(flags, pidl, riid, output) + assoc->GetEnum(0, ASSOCENUM_NONE, 0, riid, output) +\n" +
                "dock->FindToolbar(0, riid, output) + browser->CreateBrowserPropSheetExt(riid, output) +\n" +
                "browser3->CreateBrowserPropSheetExt(riid, output) + browser4->CreateBrowserPropSheetExt(riid, output) + track->GetShellFolder(flags, pidl, riid, output);\n" +
                "#else\n" +
                "return INameSpaceTreeControlEvents_OnBeforeContextMenu(tree, item, riid, output) + INameSpaceTreeControlEvents_OnAfterContextMenu(tree, item, context, riid, output) +\n" +
                "IShellMenu_GetShellFolder(menu, flags, pidl, riid, output) + assoc->lpVtbl->GetEnum(assoc, 0, ASSOCENUM_NONE, 0, riid, output) +\n" +
                "dock->lpVtbl->FindToolbar(dock, 0, riid, output) + IBrowserService2_CreateBrowserPropSheetExt(browser, riid, output) +\n" +
                "IBrowserService3_CreateBrowserPropSheetExt(browser3, riid, output) + IBrowserService4_CreateBrowserPropSheetExt(browser4, riid, output) + ITrackShellMenu_GetShellFolder(track, flags, pidl, riid, output);\n" +
                "#endif\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(owner =>
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

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void DirectXOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner(string target)
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string d3d = Direct3DPackagePath();
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string midlInputs = Path.Combine(fixture.Path, "sdk");
            foreach (string folder in new[] { "um", "shared" })
            {
                string destination = Path.Combine(midlInputs, folder);
                Directory.CreateDirectory(destination);
                foreach (string file in Directory.GetFiles(Path.Combine(include, folder)))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            foreach (string file in Directory.GetFiles(Path.Combine(d3d, "build", "native", "include")))
                File.Copy(file, Path.Combine(midlInputs, Path.GetFileNameWithoutExtension(file) == "dxgiformat" ? "shared" : "um",
                    Path.GetFileName(file)), overwrite: true);
            string[] inputs = DirectXComOutputHeaderTests.Owners.Select(owner => owner.Path)
                .Where(path => path != "um/d3d10.h" && path != "um/d3d12.h" && path != "um/Presentation.h")
                .Concat(new[] { "um/d3d10.idl", "um/d3d12.idl", "um/Presentation.idl" }).ToArray();
            foreach (string path in inputs)
            {
                string source = Path.Combine(path.StartsWith("winrt/", StringComparison.Ordinal) ? include : midlInputs, path);
                byte[] original = File.ReadAllBytes(source);
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(DirectXComOutputHeaderTests.ReadOriginal(
                    "RecompiledIdlHeaders/" + path, checkoutBytes: true)), original,
                    "The reachable introduction parent must match the actual shipped native owner: " + path);
                fixture.Header(Path.GetFileName(path), original, Path.GetDirectoryName(path)!);
            }
            string selections = "pre-midl/d3d10.idl.patch','pre-midl/d3d12.idl.patch','pre-midl/Presentation.idl.patch'," +
                "'post-midl/d2d1.h.patch','post-midl/d3dcompiler.h.patch','post-midl/d3d9on12.h.patch'," +
                "'post-midl/d3d9.h.patch','post-midl/dcomp.h.patch','post-midl/dxcapi.h.patch'," +
                "'post-midl/windows.ui.composition.interop.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            foreach (string path in inputs)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", path)), File.ReadAllBytes(Path.Combine(fixture.Headers, path)), path);
            foreach (string path in new[] { "um/d2d1.idl", "shared/d3d9.idl", "um/d3d9on12.idl", "um/d3dcompiler.idl",
                "um/dcomp.idl", "um/dxcapi.idl", "um/dwrite.idl", "winrt/windows.ui.composition.interop.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(path.StartsWith("winrt/", StringComparison.Ordinal) ? include : midlInputs, path)),
                    "Post-MIDL only for true header-only owners: " + path);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl",
                "dxgicommon.idl", "dxgiformat.idl", "dxgitype.idl", "dxgi.idl", "dxgi1_2.idl", "dxgi1_3.idl",
                "dxgi1_4.idl", "dxgi1_5.idl", "dxgi1_6.idl", "d3dcommon.idl", "d3d10.idl", "d3d10_1.idl",
                "PresentationTypes.idl", "Presentation.idl", "d3d12.idl" })
            {
                string patched = Path.Combine(fixture.Headers, "um", dependency);
                string input = File.Exists(patched) ? patched : Path.Combine(midlInputs,
                    dependency.StartsWith("wtypes", StringComparison.Ordinal) || dependency.StartsWith("dxgi", StringComparison.Ordinal) ?
                        "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            command += "$graphicsOutputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(midlInputs) + "); ";
            foreach (string owner in new[] { "d3d10", "d3d10_1", "d3d12", "Presentation" })
            {
                command += "$outputs = @($graphicsOutputs | Where-Object { $_.Idl -ieq " + Quote("um/" + owner + ".idl") +
                    " }); if ($outputs.Count -ne 1 -or $outputs[0].Header -ine " + Quote("um/" + owner + ".h") +
                    ") { throw 'Selected graphics owner is not queued by the production MIDL input inventory.' }; ";
                command += "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, owner.ToLowerInvariant() + ".idl")) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", owner.ToLowerInvariant() + ".h")) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) +
                    " /I " + Quote(Path.Combine(midlInputs, "um")) + " /I " + Quote(Path.Combine(midlInputs, "shared")) +
                    " /I " + Quote(Path.Combine(include, "winrt")) + " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            }
            command += "$header = " + Quote(Path.Combine(fixture.Headers, "um", "d3d10_1.h")) +
                "; (Get-Content $header -Raw).Replace('#include \"d3d10.h\"' + \"`r`n\", '') | Set-Content $header";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            foreach (string owner in new[] { "d3d10.h", "d3d10_1.h", "d3d12.h", "Presentation.h" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);
            DirectXComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
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
        public void DirectXOutput_NativeBuildIsANoOp(string arch, string language, bool graphicsFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = DirectXComOutputHeaderTests.Owners.Select(owner => owner.Path).Append("um/d3d10_1.h").ToArray();
            foreach (string path in owners)
                fixture.Header(Path.GetFileName(path), Encoding.ASCII.GetBytes(DirectXComOutputHeaderTests.ReadOriginal(
                    "RecompiledIdlHeaders/" + path, checkoutBytes: true)), Path.GetDirectoryName(path)!);
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (graphicsFirst ? "#include <d3d10_1.h>\n#include <d2d1.h>\n" : "#include <windows.h>\n") +
                "#include <d3d10_1.h>\n#include <d3d10.h>\n#include <d3d9on12.h>\n#include <d3dcompiler.h>\n" +
                "#include <d2d1.h>\n#include <Presentation.h>\n" +
                "#ifdef __cplusplus\n#include <dcomp.h>\n#include <dwrite.h>\n#include <dxcapi.h>\n#include <windows.ui.composition.interop.h>\n#endif\n" +
                "HRESULT (WINAPI *d2d)(D2D1_FACTORY_TYPE, REFIID, const D2D1_FACTORY_OPTIONS*, void**) = &D2D1CreateFactory;\n" +
                "HRESULT (WINAPI *deserialize)(LPCVOID, SIZE_T, REFIID, void**) = &D3D12CreateRootSignatureDeserializer;\n" +
                "HRESULT (WINAPI *versioned)(LPCVOID, SIZE_T, REFIID, void**) = &D3D12CreateVersionedRootSignatureDeserializer;\n" +
                "HRESULT (WINAPI *reflect)(LPCVOID, SIZE_T, REFIID, void**) = &D3DReflect;\n" +
                "HRESULT (WINAPI *library)(LPCVOID, SIZE_T, REFIID, void**) = &D3DReflectLibrary;\n" +
                "HRESULT (WINAPI *presentation)(IUnknown*, REFIID, void**) = &CreatePresentationFactory;\n" +
                "#ifdef __cplusplus\n" +
                "HRESULT (WINAPI *composition)(IDXGIDevice*, REFIID, void**) = &DCompositionCreateDevice;\n" +
                "HRESULT (WINAPI *composition2)(IUnknown*, REFIID, void**) = &DCompositionCreateDevice2;\n" +
                "HRESULT (WINAPI *composition3)(IUnknown*, REFIID, void**) = &DCompositionCreateDevice3;\n" +
                "HRESULT (WINAPI *dwrite)(DWRITE_FACTORY_TYPE, REFIID, IUnknown**) = &DWriteCreateFactory;\n" +
                "HRESULT (__stdcall *dxc)(REFCLSID, REFIID, void**) = &DxcCreateInstance;\n" +
                "HRESULT (__stdcall *dxc2)(IMalloc*, REFCLSID, REFIID, void**) = &DxcCreateInstance2;\n" +
                "HRESULT NativeCalls(ID3D10Device* device, ID3D10Device1* device1, IDirect3DDevice9On12* bridge,\n" +
                "IDirect3DSurface9* surface, IDirect3DVolume9* volume, IDCompositionSurface* compSurface,\n" +
                "IDCompositionTexture* texture, IPresentationManager* presentation, IDxcUtils* utils,\n" +
                "IDxcContainerReflection* reflection, IDxcCompiler3* compiler, ID2D1Resource* resource,\n" +
                "ABI::Windows::UI::Composition::ICompositionTextureInterop* interop, REFIID iid, void** output,\n" +
                "UINT64* value, POINT* point, ID2D1Factory** factory) {\n" +
                "resource->GetFactory(factory);\n" +
                "return device->OpenSharedResource(0, iid, output) + device1->OpenSharedResource(0, iid, output) +\n" +
                "bridge->GetD3D12Device(iid, output) + bridge->UnwrapUnderlyingResource(0, 0, iid, output) +\n" +
                "surface->GetContainer(iid, output) + volume->GetContainer(iid, output) +\n" +
                "compSurface->BeginDraw(0, iid, output, point) + texture->GetAvailableFence(value, iid, output) +\n" +
                "presentation->GetPresentRetiringFence(iid, output) + interop->GetAvailableFence(value, iid, output) +\n" +
                "utils->CreateReflection(0, iid, output) + reflection->GetPartReflection(0, iid, output) +\n" +
                "compiler->Compile(0, 0, 0, 0, iid, output) + compiler->Disassemble(0, iid, output);\n}\n" +
                "HRESULT FactoryTemplates(REFIID iid, ID2D1Factory** factory, void** output) {\n" +
                "D2D1_FACTORY_OPTIONS options = {}; return D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, iid, output) +\n" +
                "D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, factory) + D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, options, factory);\n}\n" +
                "#else\n" +
                "HRESULT NativeCalls(ID3D10Device* device, ID3D10Device1* device1, IDirect3DDevice9On12* bridge,\n" +
                "IDirect3DSurface9* surface, IDirect3DVolume9* volume, IPresentationManager* presentation, REFIID iid, void** output) {\n" +
                "return ID3D10Device_OpenSharedResource(device, 0, iid, output) + ID3D10Device1_OpenSharedResource(device1, 0, iid, output) +\n" +
                "IDirect3DDevice9On12_GetD3D12Device(bridge, iid, output) + IDirect3DDevice9On12_UnwrapUnderlyingResource(bridge, 0, 0, iid, output) +\n" +
                "IDirect3DSurface9_GetContainer(surface, iid, output) + IDirect3DVolume9_GetContainer(volume, iid, output) +\n" +
                "IPresentationManager_GetPresentRetiringFence(presentation, iid, output);\n}\n#endif\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(path =>
                "Copy-Item -LiteralPath " + Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", path)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, path))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + Path.Combine(fixture.Headers, "shared")) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "winrt")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
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
        public void MediaFoundationOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner(string target)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string midlInputs = Path.Combine(fixture.Path, "sdk");
            foreach (string folder in new[] { "um", "shared" })
            {
                string destination = Path.Combine(midlInputs, folder);
                Directory.CreateDirectory(destination);
                foreach (string file in Directory.GetFiles(Path.Combine(include, folder)))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            foreach (string file in Directory.GetFiles(Path.Combine(Direct3DPackagePath(), "build", "native", "include")))
                File.Copy(file, Path.Combine(midlInputs, Path.GetFileNameWithoutExtension(file) == "dxgiformat" ? "shared" : "um",
                    Path.GetFileName(file)), overwrite: true);
            string[] inputs = MediaFoundationComOutputHeaderTests.IdlOwners.Append("mfapi.h").ToArray();
            foreach (string owner in inputs)
            {
                byte[] original = File.ReadAllBytes(Path.Combine(include, "um", owner));
                CollectionAssert.AreEqual(MediaFoundationComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/um/" + owner), original,
                    "Reachable fixture introduction parent matches selected SDK: " + owner);
                fixture.Header(owner, original);
            }
            Assert.IsFalse(File.Exists(Path.Combine(include, "um", "mfapi.idl")), "Only mfapi.h is header-only.");
            string selections = string.Join("','", MediaFoundationComOutputHeaderTests.IdlOwners
                .Select(owner => "pre-midl/" + owner + ".patch").Append("post-midl/mfapi.h.patch"));
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            foreach (string owner in inputs)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oleidl.idl", "oaidl.idl", "servprov.idl", "xmldom.idl", "xmldso.idl",
                "msxml.idl", "urlmon.idl", "ocidl.idl", "shtypes.idl", "propidlbase.idl", "propidl.idl",
                "StructuredQueryCondition.idl", "propsys.idl", "mediaobj.idl", "mfobjects.idl", "mftransform.idl",
                "mfidl.idl", "mfcaptureengine.idl", "mfmediaengine.idl", "mfsharingengine.idl", "mfreadwrite.idl",
                "dxgiformat.idl", "d3dcommon.idl", "D3D12MarkerApiEnums.idl", "d3d12.idl", "mfd3d12.idl",
                "devenum.idl", "axcore.idl", "icodecapi.idl", "axextend.idl", "dyngraph.idl", "VMRender.idl",
                "dvdif.idl", "strmif.idl", "evr.idl", "dxva2api.idl" })
            {
                string patched = Path.Combine(fixture.Headers, "um", dependency);
                string input = File.Exists(patched) ? patched :
                    dependency == "d3d12.idl" || dependency == "D3D12MarkerApiEnums.idl" ?
                    Path.Combine(Direct3DPackagePath(), "build", "native", "include", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ||
                        dependency == "dxgiformat.idl" ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            var conversion = fixture.Run(command);
            Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            foreach (string owner in MediaFoundationComOutputHeaderTests.IdlOwners)
            {
                var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                    "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); & " +
                    Quote(Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, owner.ToLowerInvariant())) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", Path.ChangeExtension(owner, "h").ToLowerInvariant())) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                    " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE");
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string header = Path.ChangeExtension(owner, "h");
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", header)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", header)), header);
            }
            MediaFoundationComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
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
        public void MediaFoundationOutput_NativeBuildIsANoOp(string arch, string language, bool mediaFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = MediaFoundationComOutputHeaderTests.Owners.Select(owner => owner.Owner).ToArray();
            foreach (string owner in owners)
                fixture.Header(owner, Encoding.ASCII.GetBytes(MediaFoundationComOutputHeaderTests.ReadOriginal(
                    "RecompiledIdlHeaders/um/" + owner, checkoutBytes: true)));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            string declarations = "";
            foreach (var owner in MediaFoundationComOutputHeaderTests.Owners)
            {
                string text = File.ReadAllText(Path.Combine(fixture.Headers, "um", owner.Owner));
                foreach (string function in owner.Selectors.Where(selector => selector.Split("::").Length == 2)
                    .Select(selector => selector.Split("::")[0]).Distinct())
                {
                    string declaration = Regex.Match(text, MediaFoundationComOutputHeaderTests.FunctionPattern(function)).Value;
                    Assert.AreNotEqual("", declaration, function);
                    declarations += declaration + "\n";
                }
            }
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (mediaFirst ? "#include <mfapi.h>\n#include <mfidl.h>\n" : "#include <windows.h>\n") +
                "#include <d3d9.h>\n#include <d3d12.h>\n" +
                string.Join("\n", owners.Select(owner => "#include <" + owner + ">")) + "\n" +
                declarations +
                "HRESULT NativeCalls(IMFCaptureEngineClassFactory* capture, IMFImageSharingEngineClassFactory* sharing,\n" +
                "IMFReadWriteClassFactory* factory, IMFSourceReader* reader, IMFSourceReaderEx* readerEx,\n" +
                "IMFSinkWriter* writer, IMFSinkWriterEx* writerEx, IMFDXGIBuffer* buffer, IMFDXGIDeviceManager* manager,\n" +
                "IMFInputTrustAuthority* trust, IMFObjectReferenceStream* refs, IMFRemoteProxy* proxy,\n" +
                "IMFMediaEngineClassFactory4* media, IDirect3DDeviceManager9* manager9, REFIID iid, REFCLSID clsid,\n" +
                "void** output, IMFImageSharingEngine** engine, IMFAttributes** attributes) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(capture, CreateInstance, clsid, iid, output) + CALL(sharing, CreateInstanceFromUDN, 0, engine) +\n" +
                "CALL(factory, CreateInstanceFromURL, clsid, 0, 0, iid, output) + CALL(factory, CreateInstanceFromObject, clsid, 0, 0, iid, output) +\n" +
                "CALL(reader, GetServiceForStream, 0, iid, iid, output) + CALL(readerEx, GetServiceForStream, 0, iid, iid, output) +\n" +
                "CALL(writer, GetServiceForStream, 0, iid, iid, output) + CALL(writerEx, GetServiceForStream, 0, iid, iid, output) +\n" +
                "CALL(buffer, GetResource, iid, output) + CALL(buffer, GetUnknown, iid, iid, output) +\n" +
                "CALL(manager, GetVideoService, 0, iid, output) + CALL(manager9, GetVideoService, 0, iid, output) +\n" +
                "CALL(trust, GetDecrypter, iid, output) + CALL(refs, LoadReference, iid, output) +\n" +
                "CALL(proxy, GetRemoteHost, iid, output) + CALL(proxy, GetRemoteObject, iid, output) +\n" +
                "CALL(media, CreateContentDecryptionModuleFactory, 0, iid, output) +\n" +
                "MFCreateAttributes(attributes, 0) + MFCreateMediaExtensionActivate(0, 0, iid, output) +\n" +
                "MFCreateVideoSampleAllocatorEx(iid, output) + MFCreateD3D12SynchronizationObject(0, iid, output) +\n" +
                "MFCreateNetSchemePlugin(iid, output) + MFCreatePropertiesFromMediaType(0, iid, output) +\n" +
                "MFCreateStreamOnMFByteStreamEx(0, iid, output) + MFGetService(0, iid, iid, output) +\n" +
                "MFCreateVideoMixer(0, iid, iid, output) + MFCreateVideoMixerAndPresenter(0, 0, iid, output, iid, output) +\n" +
                "MFCreateVideoPresenter(0, iid, iid, output) + MFCreateVideoRenderer(iid, output) +\n" +
                "MFCreateVideoSampleAllocator(iid, output) + DXVA2CreateVideoService(0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
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
        public void ServiceOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner(string target)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string midlInputs = Path.Combine(fixture.Path, "sdk");
            foreach (string folder in new[] { "um", "shared" })
            {
                string destination = Path.Combine(midlInputs, folder);
                Directory.CreateDirectory(destination);
                foreach (string file in Directory.GetFiles(Path.Combine(include, folder)))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(midlInputs) + "); " +
                "foreach ($owner in 'ComSvcs.h', 'TxDtc.h') { " +
                "if ($outputs.Header -icontains \"um/$owner\") { throw \"Incorrectly classified direct service header: $owner\" } }; " +
                "$callobj = @($outputs | Where-Object { $_.Header -ieq 'um/callobj.h' }); " +
                "if ($callobj.Count -ne 1 -or $callobj[0].Idl -ine 'um/callobj.idl') { throw 'Missing authoritative callobj output' }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            foreach (string owner in new[] { "ComSvcs.h", "TxDtc.h", "callobj.idl" })
            {
                byte[] original = File.ReadAllBytes(Path.Combine(include, "um", owner));
                CollectionAssert.AreEqual(ServiceComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/um/" + owner), original, "Reachable fixture introduction parent matches selected SDK: " + owner);
                fixture.Header(owner, original);
            }
            foreach (string absent in new[] { "ComSvcs.idl", "autosvcs.idl", "TxDtc.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(include, "um", absent)), "Header-only shipped owner: " + absent);
            const string selections = "pre-midl/callobj.idl.patch','post-midl/ComSvcs.h.patch','post-midl/TxDtc.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            foreach (string owner in new[] { "ComSvcs.h", "TxDtc.h", "callobj.idl" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "callobj.idl" })
            {
                string input = dependency == "callobj.idl" ? Path.Combine(fixture.Headers, "um", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            var conversion = fixture.Run(command);
            Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); & " +
                Quote(Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, "callobj.idl")) + " /out " + Quote(converted) +
                " /header " + Quote(Path.Combine(fixture.Headers, "um", "callobj.h")) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "um", "callobj.h")), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", "callobj.h")));
            ServiceComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
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
        public void ServiceOutput_NativeBuildIsANoOp(string arch, string language, bool serviceFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = ServiceComOutputHeaderTests.Owners.Select(owner => owner.Owner).ToArray();
            foreach (string owner in owners)
                fixture.Header(owner, ServiceComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/um/" + owner));
            foreach (string forwarder in new[] { "TxCtx.h", "Mtx.h", "Mtxdm.h" })
                fixture.Header(forwarder, File.ReadAllBytes(Path.Combine(include, "um", forwarder)));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (serviceFirst ? "#include <txctx.h>\n#include <callobj.h>\n" : "#include <windows.h>\n#include <txdtc.h>\n") +
                string.Join("\n", owners.Select(owner => "#include <" + owner + ">")) +
                "\n#include <mtx.h>\n#include <mtxdm.h>\n#include <txctx.h>\n#include <callobj.h>\n" +
                "HRESULT NativeCalls(ITransactionContextEx* context, ICreateWithTransactionEx* transaction,\n" +
                "ICreateWithTipTransactionEx* tip, ICreateWithLocalTransaction* local, IObjectContext* objectContext,\n" +
                "IComTrackingInfoCollection* tracking, IServicePool* pool, IGetDispenser* dispenser,\n" +
                "IResourceManager* manager, IResourceManager2* manager2, IResourceManagerRejoinable* rejoinable,\n" +
                "IResourceManagerFactory2* factory, REFCLSID clsid, REFIID iid, void** output) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(context, CreateInstance, clsid, iid, output) + CALL(transaction, CreateInstance, 0, clsid, iid, output) +\n" +
                "CALL(tip, CreateInstance, 0, clsid, iid, output) + CALL(local, CreateInstanceWithSysTx, 0, clsid, iid, output) +\n" +
                "CALL(objectContext, CreateInstance, clsid, iid, output) + CALL(tracking, Item, 0, iid, output) +\n" +
                "CALL(pool, GetObject, iid, output) + CALL(dispenser, GetDispenser, iid, output) +\n" +
                "CALL(manager, GetDistributedTransactionManager, iid, output) +\n" +
                "CALL(manager2, GetDistributedTransactionManager, iid, output) +\n" +
                "CALL(rejoinable, GetDistributedTransactionManager, iid, output) + CALL(factory, CreateEx, 0, 0, 0, iid, output) +\n" +
                "CoCreateActivity(0, iid, output) + MTSCreateActivity(iid, output) +\n" +
                "CoGetInterceptor(iid, 0, iid, output) + CoGetInterceptorFromTypeInfo(iid, 0, 0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
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
        public void ClrOutput_RealCopiedOwnersReplayEveryNativeByteThroughFinalEmission(string target)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(include) + "); " +
                "if ($outputs.Header -icontains 'winrt/rometadata.h') { throw 'The direct SDK owner is a queued MIDL output' }; " +
                "if (Select-String -LiteralPath " + Quote(Path.Combine(include, "winrt", "rometadata.h")) +
                " -SimpleMatch 'File created by MIDL compiler' -Quiet) { throw 'Generated SDK header cannot be directly patched' }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            foreach (var owner in ClrComOutputHeaderTests.Owners)
            {
                string file = Path.GetFileName(owner.Path);
                bool compatibility = owner.Path.StartsWith("um/", StringComparison.Ordinal);
                string input = compatibility ? Path.Combine(Root(), "generation", "WinSDK", "AdditionalHeaders", file) :
                    Path.Combine(include, owner.Path.Replace('/', Path.DirectorySeparatorChar));
                byte[] original = File.ReadAllBytes(input);
                CollectionAssert.AreEqual(ClrComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/" + owner.Path), original, "Reachable introduction parent matches actual SDK/compatibility input: " + owner.Path);
                if (compatibility)
                {
                    CollectionAssert.AreEqual(ClrComOutputHeaderTests.ReadOriginalBytes("AdditionalHeaders/" + file), original);
                    Assert.IsFalse(File.Exists(Path.Combine(include, "um", file)), "Do not replace an existing compatibility owner with a removed SDK header.");
                    StringAssert.Contains(System.Text.Encoding.Latin1.GetString(original), "File created by MIDL compiler");
                    Assert.IsFalse(File.Exists(Path.Combine(patches, "post-midl", file + ".patch")),
                        "Unresolved generated compatibility source must retain its sidecar, not acquire a post-header patch.");
                }
                foreach (string area in new[] { "um", "winrt" })
                    Assert.IsFalse(File.Exists(Path.Combine(include, area, Path.ChangeExtension(file, ".idl"))),
                        "No shipped IDL to patch or regenerate: " + file);
                string destination = Path.Combine(fixture.Headers, owner.Path.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.WriteAllBytes(destination, original);
            }
            const string selections = "post-midl/rometadata.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (var owner in ClrComOutputHeaderTests.Owners)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders",
                    owner.Path.Replace('/', Path.DirectorySeparatorChar))),
                    File.ReadAllBytes(Path.Combine(fixture.Headers, owner.Path.Replace('/', Path.DirectorySeparatorChar))), owner.Path);
            ClrComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
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
        public void ClrOutput_NativeBuildIsANoOp(string arch, string language, bool hostingFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            foreach (var owner in ClrComOutputHeaderTests.Owners)
                fixture.Header(Path.GetFileName(owner.Path), ClrComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/" + owner.Path));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            // The unchanged compatibility header omits this forward declaration in C.
            File.WriteAllText(source, "#define COBJMACROS\n#define USE_DEPRECATED_CLR_API_WITHOUT_WARNING\n" +
                "#ifndef __cplusplus\ntypedef struct IManagedObject IManagedObject;\n#endif\n" +
                (hostingFirst ? "#include <mscoree.h>\n#include <metahost.h>\n#include <rometadata.h>\n" :
                    "#include <windows.h>\n#include <rometadata.h>\n#include <metahost.h>\n#include <mscoree.h>\n") +
                "HRESULT NativeCalls(IHostControl* host, ICLRControl* clr, REFCLSID clsid, REFIID iid, void** output) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(host, GetHostManager, iid, output) + CALL(clr, GetCLRManager, iid, output) +\n" +
                "CLRCreateInstance(clsid, iid, output) + MetaDataGetDispenser(clsid, iid, output) +\n" +
                "CorBindToCurrentRuntime(0, clsid, iid, output) + CorBindToRuntime(0, 0, clsid, iid, output) +\n" +
                "CorBindToRuntimeByCfg(0, 0, 0, clsid, iid, output) + CorBindToRuntimeEx(0, 0, 0, clsid, iid, output) +\n" +
                "CorBindToRuntimeHost(0, 0, 0, 0, 0, clsid, iid, output) + ClrCreateManagedInstance(0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", ClrComOutputHeaderTests.Owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", owner.Path.Replace('/', Path.DirectorySeparatorChar))) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", Path.GetFileName(owner.Path)))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " " + Quote("/I" + Path.Combine(Root(), "generation", "WinSDK", "AdditionalHeaders")) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
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

        private static string Direct3DPackagePath()
        {
            var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". " + Quote(Script("CommonUtils.ps1")) +
                "; Get-NugetPropsProperty -name PkgMicrosoft_Direct3D_D3D12 -projectName ClangSharpSourceToWinmdTests.csproj 6>$null");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = result.Output.Trim();
            Assert.AreEqual("1.619.3", Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar)));
            return path;
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
            public string Header(string owner, byte[] bytes, string folder = "um")
            {
                string file = System.IO.Path.Combine(Headers, folder, owner);
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
