using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinmdUtilsProgram;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class MigrationQualificationTests
    {
        private static readonly string Hash = new string('a', 64);
        private static readonly string CandidateSha = new string('b', 40);
        private static readonly string[] Dependencies =
            { "changes", "workflow-tests", "tooling-tests", "scrape", "build-test", "qualification", "api-diff" };

        [TestMethod]
        public void RepeatedEmission_OnlyMvidAndTimestampAreNormalized()
        {
            using var fixture = new CrossArchMetadataTests.MetadataFixture();
            const string source = @"
public enum Mode { First = 1, Second = 2 }
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
public struct Value { public int Number; public Mode Mode; }
public static class Apis {
    [System.Runtime.InteropServices.DllImport(""kernel32.dll"", ExactSpelling = true, SetLastError = true)]
    public static extern int Example(int value);
}";
            var outputs = new List<byte[]>();
            string evidence = Path.Combine(RepositoryRoot(), "bin", "emission-evidence");
            Directory.CreateDirectory(evidence);
            for (int index = 0; index < 3; index++)
            {
                byte[] bytes = File.ReadAllBytes(fixture.Generate(source, source, source, new[] { "Mode" }));
                outputs.Add(bytes);
                File.WriteAllBytes(Path.Combine(evidence, $"emission-{index}.winmd"), bytes);
            }

            var comparisons = outputs.Skip(1).Select(bytes => MigrationQualification.Compare(outputs[0], bytes)).ToArray();
            File.WriteAllText(Path.Combine(evidence, "repeat-emission.json"),
                JsonSerializer.Serialize(comparisons, MigrationQualification.JsonOptions));
            Assert.IsTrue(comparisons.All(result => result.Equal));
            Assert.AreNotEqual(comparisons[0].Reference.Mvid, comparisons[0].Candidate.Mvid);
            Assert.AreNotEqual(comparisons[0].Reference.Sha256, comparisons[0].Candidate.Sha256);

            byte[] timestampOnly = (byte[])outputs[0].Clone();
            timestampOnly[comparisons[0].Reference.TimestampOffset] ^= 1;
            Assert.IsTrue(MigrationQualification.Compare(outputs[0], timestampOnly).Equal);
            string changed = source.Replace("First = 1", "First = 3");
            byte[] semanticChange = File.ReadAllBytes(fixture.Generate(changed, changed, changed, new[] { "Mode" }));
            Assert.IsFalse(MigrationQualification.Compare(outputs[0], semanticChange).Equal);

            var identity = comparisons[0].Reference;
            for (int offset = 0; offset < outputs[0].Length; offset++)
            {
                if ((offset >= identity.MvidOffset && offset < identity.MvidOffset + 16) ||
                    (offset >= identity.TimestampOffset && offset < identity.TimestampOffset + 4)) continue;
                byte[] changedByte = (byte[])outputs[0].Clone();
                changedByte[offset] ^= 1;
                try
                {
                    Assert.IsFalse(MigrationQualification.Compare(outputs[0], changedByte).Equal,
                        $"Unobserved byte outside normalized fields: {offset}");
                }
                catch (Exception error) when (error is BadImageFormatException || error is ArgumentException ||
                    error is InvalidOperationException || error is OverflowException)
                {
                    // Malformed metadata must be rejected, never normalized into equality.
                }
            }
        }

        [TestMethod]
        public void PerformanceBudgets_ExactBoundariesAndConfirmation()
        {
            JsonObject data = Receipt();
            Candidate(data)["seconds"] = 110;
            Candidate(data)["peakWorkingSetBytes"] = 1150;
            Assert.IsTrue(Evaluate(data).Passed);
            foreach (var metric in new[] { ("seconds", 110.001), ("peakWorkingSetBytes", 1150.001) })
            {
                data = Receipt();
                Candidate(data)[metric.Item1] = metric.Item2;
                Assert.IsFalse(Evaluate(data).Passed);
                data["pairs"]!.AsArray().Add(data["pairs"]![0]!.DeepClone());
                Assert.IsFalse(Evaluate(data).Passed);
                data["pairs"]!.AsArray().Add(data["pairs"]![0]!.DeepClone());
                Assert.IsFalse(Evaluate(data).Passed);
            }

            data = Receipt();
            Candidate(data)["seconds"] = 111;
            data["pairs"]!.AsArray().Add(Receipt()["pairs"]![0]!.DeepClone());
            Assert.IsFalse(Evaluate(data).Passed, "A mixed two-pair result must remain non-green.");
            data["pairs"]!.AsArray().Add(Receipt()["pairs"]![0]!.DeepClone());
            Assert.IsTrue(Evaluate(data).Passed, "The third pair resolves the majority without ratcheting the baseline.");
        }

        [TestMethod]
        public void MigrationInputs_MayDifferButOutputsAndCumulativeBudgetsMayNot()
        {
            JsonObject data = Receipt();
            data["candidate"]!["inputHash"] = new string('c', 64);
            Assert.IsTrue(Evaluate(data).Passed, "Source relocation is not semantic drift.");
            data["pairs"]![0]!["migrationDrift"]!["equal"] = false;
            Assert.IsFalse(Evaluate(data).Passed);
            data = Receipt();
            data["pairs"]![0]!["control"]!["seconds"] = 109;
            Candidate(data)["seconds"] = 118;
            Assert.IsFalse(Evaluate(data).Passed, "Two individually acceptable increments must not ratchet A/C.");
            data = Receipt();
            data["pairs"]![0]!["control"]!["seconds"] = 90;
            Candidate(data)["seconds"] = 100;
            Assert.IsFalse(Evaluate(data).Passed, "A tool improvement must not hide a B/C migration regression.");
            data = Receipt();
            data["pairs"]![0]!["control"]!["seconds"] = 111;
            Assert.IsFalse(Evaluate(data).Passed, "A faster C must not hide an A/B tool regression.");
        }

        [TestMethod]
        public void ActiveInputs_ObserveHeadersConfigurationAndSupplementalPartitionsNotInactiveAssets()
        {
            using var directory = new TemporaryDirectory();
            string script = LoadQualificationFunctions() + $@"
$base = '{Quote(directory.Path)}'
$build = @{{ Directory = $base; Headers = ""$base\headers""; Includes = ""$base\inc"" }}
$configuration = @{{ Properties = @{{ ObjDir = ""$base\obj""; EnumsJson = ""$base\enums.json""; ScraperHeader = ""$base\scraper.header.txt""; ConstantsScraperHeader = ""$base\ConstantsScraper.header.txt"" }}; Items = @{{ ScraperRsp = @(@{{ FullPath = ""$base\generation\WinSDK\scraper.settings.rsp"" }}) }} }}
foreach ($tree in 'headers', 'inc', 'generation\WinSDK\Partitions\Supplemental', 'sources\GeneratorSdk\tools\assets\scraper', 'sources\GeneratorSdk\sdk', 'generation\WinSDK\patches', 'tests\fixtures') {{
  New-Item -ItemType Directory -Force ""$base\$tree"" | Out-Null
}}
foreach ($file in 'generation\WinSDK\Windows.Win32.proj', 'sources\GeneratorSdk\sdk\sdk.props', 'sources\GeneratorSdk\sdk\sdk.targets', 'Directory.Build.props', 'Directory.Build.targets', 'enums.json', 'scraper.header.txt', 'ConstantsScraper.header.txt', 'generation\WinSDK\scraper.settings.rsp', 'headers\input.h', 'inc\input.h', 'generation\WinSDK\Partitions\Supplemental\main.cpp', 'generation\WinSDK\Partitions\Supplemental\settings.rsp') {{
  'original' | Set-Content ""$base\$file""
}}
$original = @(Get-ActiveInputEntries $build $configuration) -join ""`n""
foreach ($file in 'headers\input.h', 'inc\input.h', 'generation\WinSDK\scraper.settings.rsp', 'generation\WinSDK\Partitions\Supplemental\settings.rsp') {{
  'changed' | Set-Content ""$base\$file""
  if ((@(Get-ActiveInputEntries $build $configuration) -join ""`n"") -eq $original) {{ throw ""Unobserved active input: $file"" }}
  'original' | Set-Content ""$base\$file""
}}
'inactive' | Set-Content ""$base\generation\WinSDK\patches\unselected.h.patch""
'fixture' | Set-Content ""$base\tests\fixtures\annotation.h""
if ((@(Get-ActiveInputEntries $build $configuration) -join ""`n"") -ne $original) {{ throw 'Inactive assets changed generation input hash.' }}
Remove-Item ""$base\headers\input.h""
if ((@(Get-ActiveInputEntries $build $configuration) -join ""`n"") -eq $original) {{ throw 'Deleted active header unobserved.' }}
Remove-Item ""$base\generation\WinSDK\scraper.settings.rsp""
try {{ Get-ActiveInputEntries $build $configuration; throw 'Missing RSP accepted.' }} catch {{
  if ($_.Exception.Message -eq 'Missing RSP accepted.') {{ throw }}
}}
Write-Output 'Active input checks passed.'
";
            var result = Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script });
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [TestMethod]
        public void FrozenProject_EvaluatesCandidateToolsAndFrozenControlPaths()
        {
            using var directory = new TemporaryDirectory();
            foreach (string relative in new[] { "Directory.Build.props", "Directory.Build.targets",
                "sources/GeneratorSdk/sdk/sdk.props", "sources/GeneratorSdk/sdk/sdk.targets", "generation/WinSDK/Windows.Win32.proj" })
            {
                var content = Run("git", new[] { "show", $"{MigrationQualification.ReferenceSha}:{relative}" });
                Assert.AreEqual(0, content.ExitCode, content.Output);
                string path = Path.Combine(directory.Path, relative.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllText(path, content.Output);
            }

            var restore = Run("dotnet", new[] { "restore", Path.Combine(directory.Path, "generation", "WinSDK", "Windows.Win32.proj"),
                "--configfile", Path.Combine(RepositoryRoot(), "nuget.config"), "--verbosity", "quiet" }, timeoutSeconds: 180);
            Assert.AreEqual(0, restore.ExitCode, restore.Output);
            string script = LoadQualificationFunctions() + $@"
$ResultsDirectory = '{Quote(directory.Path)}'
$build = @{{ Directory = '{Quote(directory.Path)}'; Project = '{Quote(Path.Combine(directory.Path, "generation", "WinSDK", "Windows.Win32.proj"))}'; Tools = '{Quote(Path.Combine(RepositoryRoot(), "bin", "Release", "net10.0"))}'; Headers = '{Quote(Path.Combine(directory.Path, "generation", "WinSDK", "RecompiledIdlHeaders"))}'; Includes = '{Quote(Path.Combine(directory.Path, "generation", "WinSDK", "inc"))}'; SdkRoot = '{Quote(Path.Combine(directory.Path, "sdk-origin"))}' }}
$configuration = Get-GenerationConfiguration $build control
if ($configuration.Properties.ToolsBinDir -ne $build.Tools -or $configuration.Properties.SdkIncRoot -ne $build.Headers) {{ throw 'Control not isolated.' }}
foreach ($group in $configuration.Items.Values) {{
  foreach ($item in $group) {{
    if (!$item.FullPath.StartsWith($build.Directory + '\', [StringComparison]::OrdinalIgnoreCase)) {{ throw 'Control consumed candidate configuration.' }}
  }}
}}
$arguments = Get-GenerationArguments $build ""$ResultsDirectory\obj"" ""$ResultsDirectory\Windows.Win32.winmd""
if ($arguments -contains '-p:PartitionFilter=WinHttp' -or $arguments -contains '-p:SkipScraping=true') {{ throw 'Incomplete production generation.' }}
$build.Directory = '{Quote(RepositoryRoot())}'
$build.Project = ""$($build.Directory)\generation\WinSDK\Windows.Win32.proj""
$build.Headers = ""$($build.Directory)\generation\WinSDK\RecompiledIdlHeaders""
$build.Includes = ""$($build.Directory)\generation\WinSDK\inc""
$current = Get-GenerationConfiguration $build candidate
if ($current.Properties.SdkIncRoot -ne $build.Headers -or $current.Properties.EnumsJson -ne ""$($build.Directory)\generation\WinSDK\enums.json"") {{ throw 'Default C did not use legacy inputs.' }}
$build.Headers = ""$ResultsDirectory\explicit-patched-result""
$current = Get-GenerationConfiguration $build patched
if ($current.Properties.SdkIncRoot -ne $build.Headers) {{ throw 'Explicit C header selection ignored.' }}
";
            var result = Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script });
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [TestMethod]
        public void RealEmission_NormalVerbosityRecordsTheSelectedToolCommand()
        {
            using var directory = new TemporaryDirectory();
            using var fixture = new CrossArchMetadataTests.MetadataFixture();
            string source = Path.GetDirectoryName(fixture.Generate("",
                "public struct Value { public int Number; }", ""))!;
            string project = Path.Combine(directory.Path, "Emission.proj");
            File.WriteAllText(project, @"<Project>
  <UsingTask TaskName=""MetadataTasks.EmitWinmd"" AssemblyFile=""$(TaskBinDir)\MetadataTasks.dll"" />
  <Target Name=""EmitWinmd"">
    <EmitWinmd ToolsBinDir=""$(ToolsBinDir)"" EmitterSourceDir=""$(SourceDir)""
      WinmdVersion=""1.0.0.0"" OutputWinmd=""$(OutputWinmd)"" Win32WinmdBinDir=""$(ToolsBinDir)""
      MSBuildProjectDirectory=""$(MSBuildProjectDirectory)"" Libs="""" />
  </Target>
</Project>");
            string tools = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            string output = Path.Combine(directory.Path, "Windows.Win32.winmd");
            var result = Run("dotnet", new[] { "msbuild", project, "-t:EmitWinmd", "-verbosity:normal",
                $"-p:TaskBinDir={tools}", $"-p:ToolsBinDir={tools}", $"-p:SourceDir={source}", $"-p:OutputWinmd={output}" });
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Assert.IsTrue(new FileInfo(output).Length > 0);
            StringAssert.Contains(result.Output, Path.Combine(tools, "ClangSharpSourceToWinmd.dll"));
            StringAssert.Contains(File.ReadAllText(Path.Combine(RepositoryRoot(), "scripts", "TestMigrationQualification.ps1")),
                "'-t:EmitWinmd', '-verbosity:normal'");
        }

        [DataTestMethod]
        [DataRow("sdkHash")]
        [DataRow("toolHash")]
        public void SharedRootFingerprints_ReuseOnlyWithinBoundaryAndRejectPostRunChanges(string changedField)
        {
            using var directory = new TemporaryDirectory();
            string script = LoadQualificationFunctions() + $@"
$base = '{Quote(directory.Path)}'
$sdk = ""$base\sdk""
$tools = ""$base\tools""
New-Item -ItemType Directory -Force ""$sdk\include"", $tools | Out-Null
'sdk header' | Set-Content ""$sdk\include\A.h""
'sdk file' | Set-Content ""$sdk\z.h""
'tool binary' | Set-Content ""$tools\tool.dll""
$script:reads = 0
function Get-FileHash {{
  param([string]$Algorithm, [string]$LiteralPath)
  $script:reads++
  Microsoft.PowerShell.Utility\Get-FileHash -Algorithm $Algorithm -LiteralPath $LiteralPath
}}
$pre = @{{}}
$sdkFingerprint = Get-RootFingerprint $sdk @('z.h', 'include/A.h') $pre
$toolFingerprint = Get-RootFingerprint $tools @('tool.dll') $pre
foreach ($side in 'reference', 'control', 'candidate') {{
  $sharedSdk = Get-RootFingerprint ""$sdk\."" @('include\A.h', 'z.h') $pre
  $sharedTool = Get-RootFingerprint $tools @('tool.dll') $pre
  $sharedSdk.Hashes.Keys | Sort-Object | ForEach-Object {{
    ""SDK/$_ $($sharedSdk.Hashes[$_])""
  }} | Set-Content ""$base\$side-sdk.sha256""
  ""tool.dll $($sharedTool.Hashes['tool.dll'])"" | Set-Content ""$base\$side-tools.sha256""
}}
if ($script:reads -ne 3) {{ throw 'Shared roots were hashed more than once at the pre boundary.' }}
foreach ($side in 'control', 'candidate') {{
  foreach ($kind in 'sdk', 'tools') {{
    if ((Get-Content ""$base\reference-$kind.sha256"" -Raw) -cne (Get-Content ""$base\$side-$kind.sha256"" -Raw)) {{
      throw 'Independent per-side manifests changed shared content.'
    }}
  }}
}}
$before = @{{ sourceSha = 'source'; toolSourceSha = 'tools'; inputHash = 'inputs'; toolchainHash = 'chain'; sdkHash = $sdkFingerprint.Hashes['include/A.h']; toolHash = $toolFingerprint.Hashes['tool.dll'] }}
$post = @{{}}
$unchanged = $before.Clone()
$unchanged.sdkHash = (Get-RootFingerprint $sdk @('z.h', 'include/A.h') $post).Hashes['include/A.h']
$unchanged.toolHash = (Get-RootFingerprint $tools @('tool.dll') $post).Hashes['tool.dll']
Assert-UnchangedReceipt $before $unchanged candidate
if ($script:reads -ne 6) {{ throw 'Post boundary did not freshly fingerprint both roots.' }}
$changedPath = if ('{changedField}' -eq 'sdkHash') {{ ""$sdk\include\A.h"" }} else {{ ""$tools\tool.dll"" }}
'changed' | Set-Content $changedPath
$nextBoundary = @{{}}
$changed = $before.Clone()
$changed.sdkHash = (Get-RootFingerprint $sdk @('z.h', 'include/A.h') $nextBoundary).Hashes['include/A.h']
$changed.toolHash = (Get-RootFingerprint $tools @('tool.dll') $nextBoundary).Hashes['tool.dll']
try {{
  Assert-UnchangedReceipt $before $changed candidate
  throw 'Post-run content change accepted.'
}} catch {{
  if (!$_.Exception.Message.Contains('candidate {changedField}')) {{ throw }}
}}
Write-Output 'Shared boundary and post-run checks passed.'
";
            var result = Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script });
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [TestMethod]
        public void SharedRootFingerprints_DoNotAliasDifferentRootsOrConflictingFileLists()
        {
            using var directory = new TemporaryDirectory();
            string script = LoadQualificationFunctions() + $@"
$base = '{Quote(directory.Path)}'
foreach ($name in 'first', 'second') {{
  New-Item -ItemType Directory ""$base\$name"" | Out-Null
  $name | Set-Content ""$base\$name\header.h""
}}
$boundary = @{{}}
$first = Get-RootFingerprint ""$base\first"" @('header.h') $boundary
$second = Get-RootFingerprint ""$base\second"" @('header.h') $boundary
if ($boundary.Count -ne 2 -or $first.Hashes['header.h'] -eq $second.Hashes['header.h']) {{
  throw 'Different physical roots shared a fingerprint.'
}}
try {{
  Get-RootFingerprint ""$base\first"" @('header.h', 'extra.h') $boundary
  throw 'Conflicting shared-root file list accepted.'
}} catch {{
  if (!$_.Exception.Message.StartsWith('Conflicting file lists')) {{ throw }}
}}
try {{
  Get-RootFingerprint ""$base\first"" @('..\second\header.h') $boundary
  throw 'File outside root accepted.'
}} catch {{
  if (!$_.Exception.Message.Contains('outside its physical root')) {{ throw }}
}}
Write-Output 'Distinct root and file-list checks passed.'
";
            var result = Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script });
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [DataTestMethod]
        [DataRow("referenceSha")]
        [DataRow("reference")]
        [DataRow("sourceSha")]
        [DataRow("inputHash")]
        [DataRow("toolchainHash")]
        [DataRow("control")]
        [DataRow("toolHash")]
        [DataRow("sdkHash")]
        [DataRow("migrationDrift")]
        [DataRow("sdkVersion")]
        [DataRow("clangSharpVersion")]
        [DataRow("generationFailure")]
        [DataRow("missingOutput")]
        [DataRow("mismatchedOutput")]
        [DataRow("comparisonFailure")]
        [DataRow("drift")]
        [DataRow("missingTime")]
        [DataRow("missingMemory")]
        [DataRow("missingPhase")]
        [DataRow("emptyPairs")]
        public void Qualification_FailsClosed(string failure)
        {
            JsonObject data = Receipt();
            switch (failure)
            {
                case "referenceSha": data["referenceSha"] = CandidateSha; break;
                case "reference": data["reference"] = null; break;
                case "sourceSha": data["reference"]!["sourceSha"] = CandidateSha; break;
                case "inputHash": data["control"]!["inputHash"] = new string('c', 64); break;
                case "toolchainHash": data["candidate"]!["toolchainHash"] = new string('c', 64); break;
                case "control": data["control"] = null; break;
                case "toolHash": data["control"]!["toolHash"] = new string('c', 64); break;
                case "sdkHash": data["candidate"]!["sdkHash"] = new string('c', 64); break;
                case "migrationDrift": data["pairs"]![0]!["migrationDrift"]!["equal"] = false; break;
                case "sdkVersion": data["sdkVersion"] = "latest"; break;
                case "clangSharpVersion": data["clangSharpVersion"] = "latest"; break;
                case "generationFailure": Candidate(data)["exitCode"] = 1; break;
                case "missingOutput": Candidate(data).Remove("outputHash"); break;
                case "mismatchedOutput": Candidate(data)["outputHash"] = new string('c', 64); break;
                case "comparisonFailure": data["pairs"]![0]!.AsObject().Remove("drift"); break;
                case "drift": data["pairs"]![0]!["drift"]!["equal"] = false; break;
                case "missingTime": Candidate(data).Remove("seconds"); break;
                case "missingMemory": Candidate(data)["memorySamples"] = 0; break;
                case "missingPhase": Candidate(data)["endedAt"] = "invalid"; break;
                case "emptyPairs": data["pairs"] = new JsonArray(); break;
                default: Assert.Fail("Unknown failure case."); break;
            }

            Assert.IsFalse(Evaluate(data).Passed, failure);
        }

        [TestMethod]
        public void Evidence_BindsSourcesOutputsAndRawSamples()
        {
            using var directory = new TemporaryDirectory();
            using var fixture = new CrossArchMetadataTests.MetadataFixture();
            byte[] first = File.ReadAllBytes(fixture.Generate("", "public struct Value { public int Number; }", ""));
            byte[] second = File.ReadAllBytes(fixture.Generate("", "public struct Value { public int Number; }", ""));
            var drift = MigrationQualification.Compare(first, second);
            var migrationDrift = MigrationQualification.Compare(second, second);
            JsonObject data = Receipt();
            data["passed"] = true;
            data["pairs"]![0]!["drift"] = JsonSerializer.SerializeToNode(drift, MigrationQualification.JsonOptions);
            data["pairs"]![0]!["migrationDrift"] = JsonSerializer.SerializeToNode(migrationDrift, MigrationQualification.JsonOptions);
            File.WriteAllBytes(Path.Combine(directory.Path, "pair-1-reference.winmd"), first);
            File.WriteAllBytes(Path.Combine(directory.Path, "pair-1-control.winmd"), second);
            File.WriteAllBytes(Path.Combine(directory.Path, "pair-1-candidate.winmd"), second);
            foreach (string side in new[] { "reference", "control", "candidate" })
            {
                foreach (var entry in new[] { ("inputs.sha256", "inputHash"), ("toolchain.txt", "toolchainHash"),
                    ("sdk.sha256", "sdkHash"), ("tools.sha256", "toolHash") })
                {
                    string file = Path.Combine(directory.Path, $"{side}-{entry.Item1}");
                    File.WriteAllText(file, "bound inputs");
                    data[side]![entry.Item2] = MigrationQualification.Hash(File.ReadAllBytes(file));
                }

                JsonObject sample = data["pairs"]![0]![side]!.AsObject();
                sample["outputHash"] = side == "reference" ? drift.Reference.Sha256 : drift.Candidate.Sha256;
                foreach (var entry in new[] { (".binlog", "binlogHash"), (".log", "logHash"), ("-memory.csv", "memoryLogHash") })
                {
                    string file = Path.Combine(directory.Path, $"pair-1-{side}{entry.Item1}");
                    File.WriteAllText(file, "raw evidence");
                    sample[entry.Item2] = MigrationQualification.Hash(File.ReadAllBytes(file));
                }

                File.WriteAllText(Path.Combine(directory.Path, $"pair-1-{side}-sample.json"), sample.ToJsonString());
            }

            File.WriteAllText(Path.Combine(directory.Path, "receipt.json"), data.ToJsonString());
            Assert.IsTrue(MigrationQualification.VerifyEvidence(directory.Path, CandidateSha).Passed);
            Assert.ThrowsException<InvalidDataException>(() =>
                MigrationQualification.VerifyEvidence(directory.Path, new string('c', 40)));
            string candidate = Path.Combine(directory.Path, "pair-1-candidate.winmd");
            File.Delete(candidate);
            Assert.ThrowsException<FileNotFoundException>(() => MigrationQualification.VerifyEvidence(directory.Path));
            File.WriteAllBytes(candidate, second);
            string control = Path.Combine(directory.Path, "pair-1-control.winmd");
            File.Delete(control);
            Assert.ThrowsException<FileNotFoundException>(() => MigrationQualification.VerifyEvidence(directory.Path));
            File.WriteAllBytes(control, second);
            data["pairs"]![0]!["candidate"]!["outputHash"] = new string('c', 64);
            File.WriteAllText(Path.Combine(directory.Path, "receipt.json"), data.ToJsonString());
            Assert.ThrowsException<InvalidDataException>(() => MigrationQualification.VerifyEvidence(directory.Path));
            data["pairs"]![0]!["candidate"]!["outputHash"] = drift.Candidate.Sha256;
            File.WriteAllText(Path.Combine(directory.Path, "receipt.json"), data.ToJsonString());
            File.WriteAllText(Path.Combine(directory.Path, "pair-1-candidate-memory.csv"), "tampered");
            Assert.ThrowsException<InvalidDataException>(() => MigrationQualification.VerifyEvidence(directory.Path));
            File.Delete(Path.Combine(directory.Path, "receipt.json"));
            Assert.ThrowsException<FileNotFoundException>(() => MigrationQualification.VerifyEvidence(directory.Path));
        }

        [TestMethod]
        public void ComparisonAndEvidenceCommands_FailWithInvalidInputs()
        {
            using var directory = new TemporaryDirectory();
            string invalid = Path.Combine(directory.Path, "invalid.winmd");
            File.WriteAllText(invalid, "not metadata");
            Assert.AreNotEqual(0, Run("dotnet", new[] { Cli(), "compareBinary", "--first", invalid,
                "--second", invalid, "--output", Path.Combine(directory.Path, "drift.json") }).ExitCode);
            Assert.AreNotEqual(0, Run("dotnet", new[] { Cli(), "verifyMigrationEvidence", "--directory", directory.Path }).ExitCode);
        }

        [TestMethod]
        public void Workflow_CheckNamesTriggersDependenciesAndRetention()
        {
            string root = RepositoryRoot();
            string workflow = File.ReadAllText(Path.Combine(root, ".github", "workflows", "pr-validation.yml")).Replace("\r\n", "\n");
            StringAssert.Contains(workflow, "on:\n  pull_request:\n  push:\n");
            StringAssert.Contains(workflow, "branches: [main, feature/header-annotations]");
            Assert.IsFalse(workflow.Contains("paths-ignore:"));
            foreach (var entry in new[]
            {
                ("tooling-tests", "Tooling tests"), ("migration", "Migration qualification"),
                ("workflow-tests", "Workflow regression tests"), ("build-test", "'Build, test, package'")
            })
            {
                StringAssert.Contains(Job(workflow, entry.Item1), $"    name: {entry.Item2}\n");
            }

            string tooling = Job(workflow, "tooling-tests");
            Assert.IsFalse(Regex.IsMatch(tooling, @"^    (needs|if):", RegexOptions.Multiline));
            StringAssert.Contains(tooling, "fetch-depth: 0");
            StringAssert.Contains(tooling, "lfs: true");
            StringAssert.Contains(tooling, @".\scripts\TestTooling.ps1");
            StringAssert.Contains(Job(workflow, "workflow-tests"), "node --test .github/tests/pr-api-diff-comment.test.cjs");
            Assert.AreEqual(1, Regex.Matches(workflow, @"run: node ").Count, "Node is reserved for the existing API-comment tests.");
            StringAssert.Contains(Job(workflow, "changes"), "context.repo.owner === 'jevansaks'");
            string migration = Job(workflow, "migration");
            StringAssert.Contains(migration, "needs: [changes, workflow-tests, tooling-tests, scrape, build-test, qualification, api-diff, sdk-patch-tests]");
            StringAssert.Contains(migration, "if: always() && github.repository == 'jevansaks/win32metadata'");
            StringAssert.Contains(migration, "verifyMigrationEvidence --directory evidence");
            StringAssert.Contains(migration, "fetch-depth: 0");
            string build = Job(workflow, "build-test");
            StringAssert.Contains(build, "if: always()");
            StringAssert.Contains(build, "needs: [changes, scrape]");
            Assert.IsFalse(build.Contains("needs.qualification"));
            foreach (string dependency in new[] { "changes", "scrape" })
                StringAssert.Contains(build, $"needs.{dependency}.result != 'success'");
            foreach (string script in new[] { "BuildMetadataBin", "DoPackages", "DoSamples", "DoTests" })
                StringAssert.Contains(build, $@".\scripts\{script}.ps1");
            StringAssert.Contains(build, @".\scripts\DoPackages.ps1 -skipInstallTools");
            StringAssert.Contains(build, @".\scripts\DoSamples.ps1 -skipInstallTools");
            StringAssert.Contains(build, @".\scripts\DoTests.ps1 -SkipInstallTools");
            foreach (string script in new[] { "DoPackages", "DoSamples", "DoTests" })
                StringAssert.Matches(build, new Regex(@"run: \|\n          \. \.\\scripts\\CommonUtils\.ps1\n          Install-VsDevShell\n          \.\\scripts\\" + script + @"\.ps1 -[sS]kipInstallTools\n"),
                    "Each Actions step has a fresh shell and must initialize VS even when reusing binaries.");
            Assert.IsFalse(build.Contains("BuildMetadataBin.ps1 -assetsScrapedSeparately -skipInstallTools"),
                "Metadata emission must prepare the tools before later steps reuse them.");
            foreach (string artifact in new[] { "build-winmd", "build_logs", "full-test-results", "NuGetPackages" })
                StringAssert.Contains(build, $"name: {artifact}\n");
            Assert.IsFalse(build.Contains("Download frozen fork reference"));
            string comparison = Job(workflow, "api-diff");
            StringAssert.Contains(comparison, "needs: [changes, build-test, qualification]");
            StringAssert.Contains(comparison, "    if: always()\n");
            StringAssert.Contains(comparison, "name: Documentation-only change\n");
            StringAssert.Contains(comparison, "if: needs.changes.outputs.build_needed != 'true'\n");
            StringAssert.Matches(build, new Regex(@"name: Retain emitted winmd for comparison\n        uses: [^\n]+\n        if: always\(\) && needs.changes.outputs.build_needed == 'true'\n"));
            StringAssert.Contains(comparison, "name: build-winmd\n");
            StringAssert.Contains(comparison, "name: migration-evidence\n");
            StringAssert.Contains(comparison, "verifyMigrationEvidence --directory bin\\baseline-artifact");
            StringAssert.Contains(comparison, "name: Generate API surface diff");
            StringAssert.Contains(comparison, "name: winmd\n");
            StringAssert.Contains(comparison, "name: api-diff\n");
            StringAssert.Contains(comparison, "retention-days: 90");
            StringAssert.Contains(Job(workflow, "scrape"), "arch: [x64, x86, arm64]");
            foreach (string arch in new[] { "x64", "x86", "arm64" })
                StringAssert.Contains(build, $"name: generated_{arch}");
            StringAssert.Contains(Job(workflow, "qualification"), $"ref: {MigrationQualification.ReferenceSha}");
            StringAssert.Contains(Job(workflow, "qualification"), "path: .qualification/control");
            StringAssert.Contains(Job(workflow, "qualification"), "-ControlDirectory .qualification\\control");
            foreach (string name in new[] { "tooling-tests", "qualification" })
            {
                StringAssert.Matches(Job(workflow, name), new Regex(@"name: Retain [^\n]+\n        if: always\(\)"));
                StringAssert.Contains(Job(workflow, name), "retention-days: 30");
                Assert.IsFalse(Job(workflow, name).Contains("continue-on-error:"));
            }

            foreach (string name in Dependencies.Concat(new[] { "migration" }))
                StringAssert.Matches(Job(workflow, name), new Regex(@"timeout-minutes: \d+"));
            string upload = File.ReadAllText(Path.Combine(root, ".github", "actions", "upload-artifact-with-retry", "action.yml"));
            Assert.AreEqual(2, Regex.Matches(upload, "if-no-files-found: error").Count);
            string owners = File.ReadAllText(Path.Combine(root, ".github", "CODEOWNERS"));
            StringAssert.Contains(owners, "/.github/ @jevansaks");
            StringAssert.Contains(owners, "/sources/WinmdUtils/MigrationQualification.cs @jevansaks");
        }

        [TestMethod]
        public void ComparisonDependencies_FailClosedWithoutBlockingIndependentBuild()
        {
                string workflow = File.ReadAllText(Path.Combine(RepositoryRoot(), ".github", "workflows", "pr-validation.yml")).Replace("\r\n", "\n");
                string block = Job(workflow, "api-diff");
                Match match = Regex.Match(block, @"name: Require comparison dependencies.*?        run: \|\n(?<script>(?:          [^\n]*\n)+)", RegexOptions.Singleline);
                Assert.IsTrue(match.Success);
                string script = Regex.Replace(match.Groups["script"].Value, "^          ", "", RegexOptions.Multiline);
                var results = new[] { "changes", "build-test", "qualification" }.ToDictionary(name => name, _ => new { result = "success" });
                Assert.AreEqual(0, Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script },
                    new Dictionary<string, string> { ["NEEDS"] = JsonSerializer.Serialize(results), ["IS_FORK"] = "true" }).ExitCode);
                foreach (string dependency in results.Keys)
                {
                    foreach (string state in new[] { "skipped", "failure", "cancelled", "missing" })
                    {
                        var changed = new Dictionary<string, object>();
                        foreach (string name in results.Keys)
                        {
                            if (name == dependency && state == "missing") continue;
                            changed[name] = new { result = name == dependency ? state : "success" };
                        }

                        Assert.AreNotEqual(0, Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script },
                            new Dictionary<string, string> { ["NEEDS"] = JsonSerializer.Serialize(changed), ["IS_FORK"] = "true" }).ExitCode,
                            $"{dependency}: {state}");
                    }
                }

                results["qualification"] = new { result = "skipped" };
                Assert.AreEqual(0, Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script },
                    new Dictionary<string, string> { ["NEEDS"] = JsonSerializer.Serialize(results), ["IS_FORK"] = "false" }).ExitCode,
                    "Upstream keeps its existing non-migration comparison.");
        }

        [TestMethod]
        public void WorkflowAggregate_RejectsEveryMissingSkippedCancelledOrFailedDependency()
        {
            string workflow = File.ReadAllText(Path.Combine(RepositoryRoot(), ".github", "workflows", "pr-validation.yml")).Replace("\r\n", "\n");
            string block = Job(workflow, "migration");
            Match match = Regex.Match(block, @"name: Require every qualification dependency.*?        run: \|\n(?<script>(?:          [^\n]*\n)+)", RegexOptions.Singleline);
            Assert.IsTrue(match.Success);
            string script = Regex.Replace(match.Groups["script"].Value, "^          ", "", RegexOptions.Multiline);
            var results = Dependencies.ToDictionary(name => name, _ => new { result = "success" });
            Assert.AreEqual(0, Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script },
                new Dictionary<string, string> { ["NEEDS"] = JsonSerializer.Serialize(results) }).ExitCode);
            foreach (string dependency in Dependencies)
            {
                foreach (string state in new[] { "skipped", "failure", "cancelled", "missing" })
                {
                    var changed = new Dictionary<string, object>();
                    foreach (string name in Dependencies)
                    {
                        if (name == dependency && state == "missing") continue;
                        changed[name] = new { result = name == dependency ? state : "success" };
                    }

                    Assert.AreNotEqual(0, Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script },
                        new Dictionary<string, string> { ["NEEDS"] = JsonSerializer.Serialize(changed) }).ExitCode,
                        $"{dependency}: {state}");
                }
            }
        }

        [DataTestMethod]
        [DataRow(1, 1, 0, 1, false)]
        [DataRow(0, 0, 0, 0, false)]
        [DataRow(0, 2, 1, 1, false)]
        [DataRow(0, 2, 0, 1, false)]
        [DataRow(0, 2, 0, 2, true)]
        public void ToolingEntryPoint_ValidatesExitAndActualTrxCounts(int exitCode, int executed, int failed, int passed, bool expectedSuccess)
        {
            using var directory = new TemporaryDirectory();
            string script = $@"
function dotnet {{
  $results = $args[([Array]::IndexOf($args, '--results-directory') + 1)]
  $logger = $args[([Array]::IndexOf($args, '--logger') + 1)]
  $file = $logger.Replace('trx;LogFileName=', '')
  '<TestRun><ResultSummary outcome=""Completed""><Counters executed=""{executed}"" failed=""{failed}"" passed=""{passed}"" /></ResultSummary></TestRun>' | Set-Content (Join-Path $results $file)
  $global:LASTEXITCODE = {exitCode}
}}
& '{Quote(Path.Combine(RepositoryRoot(), "scripts", "TestTooling.ps1"))}' -ResultsDirectory '{Quote(directory.Path)}'
";
            var result = Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script });
            Assert.AreEqual(expectedSuccess, result.ExitCode == 0, result.Output);
            Assert.IsTrue(new FileInfo(Path.Combine(directory.Path, "tooling.log")).Length > 0);
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void FailingRealGeneration_RemovesStaleOutputAndRetainsFailureEvidence(bool missingOutput)
        {
            using var directory = new TemporaryDirectory();
            string project = Path.Combine(directory.Path, "generation", "WinSDK");
            Directory.CreateDirectory(project);
            string evidence = Path.Combine(directory.Path, "evidence");
            Directory.CreateDirectory(evidence);
            File.WriteAllText(Path.Combine(project, "Failure.proj"),
                missingOutput ? "<Project><Target Name=\"EmitWinmd\" /></Project>" :
                "<Project><Target Name=\"EmitWinmd\"><Error Text=\"Intentional qualification generation failure\" /></Target></Project>");
            string buildDirectory = Path.Combine(evidence, "build-1-reference");
            Directory.CreateDirectory(Path.Combine(buildDirectory, "obj", "generated"));
            string output = Path.Combine(buildDirectory, "Windows.Win32.winmd");
            File.WriteAllText(output, "stale output");
            string script = $@"
$ErrorActionPreference = 'Stop'
$errors = $null
$ast = [Management.Automation.Language.Parser]::ParseFile('{Quote(Path.Combine(RepositoryRoot(), "scripts", "TestMigrationQualification.ps1"))}', [ref]$null, [ref]$errors)
$definition = $ast.Find({{ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Measure-Generation' }}, $true)
Invoke-Expression $definition.Extent.Text
$arguments = $ast.Find({{ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Get-GenerationArguments' }}, $true)
Invoke-Expression $arguments.Extent.Text
$ResultsDirectory = '{Quote(evidence)}'
$build = @{{ Directory = '{Quote(directory.Path)}'; Project = '{Quote(Path.Combine(project, "Failure.proj"))}'; Tools = '{Quote(directory.Path)}'; Headers = '{Quote(directory.Path)}'; Includes = '{Quote(directory.Path)}' }}
Measure-Generation $build reference 1
";
            Assert.AreNotEqual(0, Run("pwsh", new[] { "-NoProfile", "-NonInteractive", "-Command", script }).ExitCode);
            Assert.IsFalse(File.Exists(output));
            using var sample = JsonDocument.Parse(File.ReadAllText(Path.Combine(evidence, "pair-1-reference-sample.json")));
            if (missingOutput) Assert.AreEqual(0, sample.RootElement.GetProperty("exitCode").GetInt32());
            else Assert.AreNotEqual(0, sample.RootElement.GetProperty("exitCode").GetInt32());
            Assert.AreEqual(JsonValueKind.Null, sample.RootElement.GetProperty("outputHash").ValueKind);
            if (!missingOutput)
                StringAssert.Contains(File.ReadAllText(Path.Combine(evidence, "pair-1-reference.log")),
                    "Intentional qualification generation failure");
        }

        private static JsonObject Receipt()
        {
            JsonObject Sample() => new JsonObject
            {
                ["exitCode"] = 0, ["seconds"] = 100, ["peakWorkingSetBytes"] = 1000, ["memorySamples"] = 10,
                ["outputHash"] = Hash, ["startedAt"] = "2026-11-01T00:00:00Z", ["endedAt"] = "2026-11-01T00:01:40Z"
            };
            return new JsonObject
            {
                ["referenceSha"] = MigrationQualification.ReferenceSha, ["candidateSha"] = CandidateSha,
                ["sdkVersion"] = "10.0.28000.2705", ["clangSharpVersion"] = "21.1.8.4",
                ["reference"] = InputReceipt(MigrationQualification.ReferenceSha, MigrationQualification.ReferenceSha),
                ["control"] = InputReceipt(MigrationQualification.ReferenceSha, CandidateSha),
                ["candidate"] = InputReceipt(CandidateSha, CandidateSha),
                ["pairs"] = new JsonArray(new JsonObject
                {
                    ["reference"] = Sample(), ["control"] = Sample(), ["candidate"] = Sample(),
                    ["drift"] = new JsonObject
                    {
                        ["equal"] = true, ["reference"] = new JsonObject { ["sha256"] = Hash },
                        ["candidate"] = new JsonObject { ["sha256"] = Hash }
                    },
                    ["migrationDrift"] = new JsonObject
                    {
                        ["equal"] = true, ["reference"] = new JsonObject { ["sha256"] = Hash },
                        ["candidate"] = new JsonObject { ["sha256"] = Hash }
                    }

                })
            };
        }

        private static JsonObject InputReceipt(string source, string tools) => new JsonObject
        {
            ["sourceSha"] = source, ["toolSourceSha"] = tools, ["inputHash"] = Hash,
            ["toolchainHash"] = Hash, ["sdkHash"] = Hash, ["toolHash"] = Hash
        };

        private static JsonObject Candidate(JsonObject receipt) => receipt["pairs"]![0]!["candidate"]!.AsObject();
        private static string LoadQualificationFunctions() => $@"
$ErrorActionPreference = 'Stop'
$errors = $null
$ast = [Management.Automation.Language.Parser]::ParseFile('{Quote(Path.Combine(RepositoryRoot(), "scripts", "TestMigrationQualification.ps1"))}', [ref]$null, [ref]$errors)
if ($errors) {{ throw ($errors | Out-String) }}
foreach ($definition in $ast.FindAll({{ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] }}, $true)) {{
  Invoke-Expression $definition.Extent.Text
}}
";
        private static MigrationQualification.Verdict Evaluate(JsonObject receipt) =>
            MigrationQualification.Evaluate(JsonSerializer.SerializeToElement(receipt));
        private static string Job(string workflow, string name) =>
            Regex.Match(workflow, $@"^  {Regex.Escape(name)}:\n(?<body>.*?)(?=^  [a-z-]+:\n|\z)",
                RegexOptions.Multiline | RegexOptions.Singleline).Groups["body"].Value;
        private static string Quote(string value) => value.Replace("'", "''");
        private static string Cli() => Path.Combine(AppContext.BaseDirectory, "WinmdUtils.dll");

        private static string RepositoryRoot()
        {
            for (DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory); directory != null; directory = directory.Parent)
            {
                if (File.Exists(Path.Combine(directory.FullName, "scripts", "TestTooling.ps1")))
                    return directory.FullName;
            }

            throw new DirectoryNotFoundException("Cannot locate qualification workflow/test scripts.");
        }

        private static (int ExitCode, string Output) Run(string executable, string[] arguments,
            Dictionary<string, string>? environment = null, int timeoutSeconds = 60)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo(executable)
            {
                UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true,
                WorkingDirectory = RepositoryRoot()
            };
            foreach (string argument in arguments) process.StartInfo.ArgumentList.Add(argument);
            if (environment != null)
                foreach (var entry in environment) process.StartInfo.Environment[entry.Key] = entry.Value;
            Assert.IsTrue(process.Start());
            var stdout = process.StandardOutput.ReadToEndAsync();
            var stderr = process.StandardError.ReadToEndAsync();
            if (!process.WaitForExit(timeoutSeconds * 1000))
            {
                process.Kill(true);
                Assert.Fail($"Qualification test child exceeded {timeoutSeconds} seconds: {executable}");
            }

            return (process.ExitCode, stdout.GetAwaiter().GetResult() + stderr.GetAwaiter().GetResult());
        }

        private sealed class TemporaryDirectory : IDisposable
        {
            public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "MigrationQualificationTests-" + Guid.NewGuid().ToString("N"));
            public TemporaryDirectory() => Directory.CreateDirectory(this.Path);
            public void Dispose() => Directory.Delete(this.Path, true);
        }
    }
}
