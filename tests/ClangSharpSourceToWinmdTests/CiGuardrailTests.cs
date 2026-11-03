using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class CiGuardrailTests
    {
        private static readonly string[] Dependencies =
            { "changes", "workflow-tests", "tooling-tests", "scrape", "build-test", "api-diff" };

        [TestMethod]
        public void Workflow_OrdinaryGenerationChecksAndRetention()
        {
            string root = RepositoryRoot();
            string workflow = Workflow();
            StringAssert.Contains(workflow, "on:\n  pull_request:\n  push:\n");
            StringAssert.Contains(workflow, "branches: [main, feature/header-annotations]");
            Assert.IsFalse(workflow.Contains("paths-ignore:"));
            foreach (var entry in new[]
            {
                ("tooling-tests", "Tooling tests"), ("migration", "Migration qualification"),
                ("workflow-tests", "Workflow regression tests"), ("build-test", "'Build, test, package'")
            })
                StringAssert.Contains(Job(workflow, entry.Item1), $"    name: {entry.Item2}\n");

            Assert.IsFalse(workflow.Contains("TestMigrationQualification"));
            Assert.IsFalse(workflow.Contains("migration-evidence"));
            Assert.IsFalse(workflow.Contains("  qualification:"));
            string aggregate = Job(workflow, "migration");
            StringAssert.Contains(aggregate, "needs: [changes, workflow-tests, tooling-tests, scrape, build-test, api-diff, sdk-patch-tests]");
            StringAssert.Contains(aggregate, "if: always() && github.repository == 'jevansaks/win32metadata'");
            Assert.IsFalse(aggregate.Contains("dotnet"));
            Assert.IsFalse(aggregate.Contains("actions/checkout"));

            string tooling = Job(workflow, "tooling-tests");
            Assert.IsFalse(Regex.IsMatch(tooling, @"^    (needs|if):", RegexOptions.Multiline));
            StringAssert.Contains(tooling, "fetch-depth: 0");
            StringAssert.Contains(tooling, "lfs: true");
            StringAssert.Contains(tooling, @".\scripts\TestTooling.ps1");
            StringAssert.Contains(tooling, "retention-days: 30");
            StringAssert.Contains(Job(workflow, "workflow-tests"), "node --test .github/tests/pr-api-diff-comment.test.cjs");
            Assert.AreEqual(1, Regex.Matches(workflow, @"run: node ").Count);
            StringAssert.Contains(Job(workflow, "changes"), "context.repo.owner === 'jevansaks'");
            StringAssert.Contains(Job(workflow, "scrape"), "arch: [x64, x86, arm64]");

            string build = Job(workflow, "build-test");
            StringAssert.Contains(build, "needs: [changes, scrape]");
            foreach (string name in new[] { "changes", "scrape" })
                StringAssert.Contains(build, $"needs.{name}.result != 'success'");
            StringAssert.Contains(build, @".\scripts\BuildMetadataBin.ps1 -assetsScrapedSeparately");
            foreach (string script in new[] { "DoPackages", "DoSamples", "DoTests" })
                StringAssert.Matches(build, new Regex(@"run: \|\n          \. \.\\scripts\\CommonUtils\.ps1\n          Install-VsDevShell\n          \.\\scripts\\" + script + @"\.ps1 -[sS]kipInstallTools\n"),
                    "Each reused-tools step needs its own VS environment.");
            foreach (string artifact in new[] { "build-winmd", "build_logs", "full-test-results", "NuGetPackages" })
                StringAssert.Contains(build, $"name: {artifact}\n");
            foreach (string arch in new[] { "x64", "x86", "arm64" })
                StringAssert.Contains(build, $"name: generated_{arch}");

            string comparison = Job(workflow, "api-diff");
            StringAssert.Contains(comparison, "needs: [changes, build-test]");
            StringAssert.Contains(comparison, "    if: always()\n");
            StringAssert.Contains(comparison, "name: Target baseline and API comparison");
            StringAssert.Contains(comparison, "context.payload.pull_request.base.sha");
            StringAssert.Contains(comparison, "owner: 'microsoft', repo: 'win32metadata', branch: 'main'");
            StringAssert.Contains(comparison, "Get-Win32MetadataLastReleaseWinmdPath");
            StringAssert.Contains(comparison, "Baseline SHA256:");
            StringAssert.Contains(comparison, "Candidate SHA256:");
            StringAssert.Contains(comparison, "baseline-source.txt");
            StringAssert.Contains(comparison, "comparison-inputs.txt");
            StringAssert.Contains(comparison, "retention-days: 90");
            StringAssert.Contains(comparison, "name: Documentation-only change");
            foreach (string name in Dependencies.Concat(new[] { "migration" }))
            {
                StringAssert.Matches(Job(workflow, name), new Regex(@"timeout-minutes: \d+"));
                Assert.IsFalse(Job(workflow, name).Contains("continue-on-error:"));
            }
            string upload = File.ReadAllText(Path.Combine(root, ".github", "actions", "upload-artifact-with-retry", "action.yml"));
            Assert.AreEqual(2, Regex.Matches(upload, "if-no-files-found: error").Count);
        }

        [TestMethod]
        public void Aggregate_RejectsEveryMissingSkippedCancelledOrFailedDependency()
        {
            AssertDependencies("migration", "Require every CI dependency", Dependencies);
        }

        [TestMethod]
        public void Comparison_RejectsFailedBuildOrChangeDetection()
        {
            AssertDependencies("api-diff", "Require comparison dependencies", new[] { "changes", "build-test" });
        }

        [DataTestMethod]
        [DataRow("missing", false)]
        [DataRow("dump", false)]
        [DataRow("incomplete", false)]
        [DataRow("diff", false)]
        [DataRow("changes", true)]
        public void ApiComparison_RejectsMissingInputsAndCommandFailures(string scenario, bool expectedSuccess)
        {
            using var directory = new TemporaryDirectory();
            string bin = Path.Combine(directory.Path, "bin");
            Directory.CreateDirectory(Path.Combine(bin, "baseline-artifact"));
            File.WriteAllText(Path.Combine(bin, "baseline-artifact", "Windows.Win32.winmd"), "baseline fixture");
            if (scenario != "missing")
                File.WriteAllText(Path.Combine(bin, "Windows.Win32.winmd"), "candidate fixture");
            File.WriteAllText(Path.Combine(bin, "baseline-source.txt"), "fixture target commit");
            string script = $@"
$env:GITHUB_OUTPUT = '{Quote(Path.Combine(directory.Path, "outputs.txt"))}'
function dotnet {{
  if ('{scenario}' -eq 'dump') {{ $global:LASTEXITCODE = 1; return }}
  $output = $args[([Array]::IndexOf($args, '--output') + 1)]
  if ('{scenario}' -eq 'incomplete') {{ '// ERROR decompiling fixture' | Set-Content $output }}
  else {{ 'fixture API dump' | Set-Content $output }}
  $global:LASTEXITCODE = 0
}}
function cmd {{
  '+changed' | Set-Content bin\api-diff.patch
  $global:LASTEXITCODE = if ('{scenario}' -eq 'diff') {{ 2 }} else {{ 1 }}
}}
" + Script(Job(Workflow(), "api-diff"), "Generate API surface diff");
            var result = Run(script, workingDirectory: directory.Path);
            Assert.AreEqual(expectedSuccess, result.ExitCode == 0, result.Output);
        }

        private static void AssertDependencies(string job, string step, string[] dependencies)
        {
            string script = Script(Job(Workflow(), job), step);
            var results = dependencies.ToDictionary(name => name, _ => new { result = "success" });
            Assert.AreEqual(0, Run(script, new Dictionary<string, string>
                { ["NEEDS"] = JsonSerializer.Serialize(results) }).ExitCode);
            foreach (string dependency in dependencies)
                foreach (string state in new[] { "skipped", "failure", "cancelled", "missing" })
                {
                    var changed = new Dictionary<string, object>();
                    foreach (string name in dependencies)
                    {
                        if (name == dependency && state == "missing") continue;
                        changed[name] = new { result = name == dependency ? state : "success" };
                    }
                    Assert.AreNotEqual(0, Run(script, new Dictionary<string, string>
                        { ["NEEDS"] = JsonSerializer.Serialize(changed) }).ExitCode, $"{dependency}: {state}");
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
            var result = Run(script);
            Assert.AreEqual(expectedSuccess, result.ExitCode == 0, result.Output);
            Assert.IsTrue(new FileInfo(Path.Combine(directory.Path, "tooling.log")).Length > 0);
        }

        private static string Workflow() => File.ReadAllText(Path.Combine(RepositoryRoot(), ".github", "workflows", "pr-validation.yml")).Replace("\r\n", "\n");
        private static string Job(string workflow, string name) =>
            Regex.Match(workflow, $@"^  {Regex.Escape(name)}:\n(?<body>.*?)(?=^  [a-z-]+:\n|\z)",
                RegexOptions.Multiline | RegexOptions.Singleline).Groups["body"].Value;
        private static string Script(string block, string step)
        {
            Match stepMatch = Regex.Match(block, $@"^      - name: {Regex.Escape(step)}\n(?<step>.*?)(?=^      - |\z)",
                RegexOptions.Multiline | RegexOptions.Singleline);
            Assert.IsTrue(stepMatch.Success);
            Match match = Regex.Match(stepMatch.Groups["step"].Value, @"        run: \|\n(?<script>.*)\z", RegexOptions.Singleline);
            Assert.IsTrue(match.Success);
            return Regex.Replace(match.Groups["script"].Value, "^          ", "", RegexOptions.Multiline);
        }
        private static string Quote(string value) => value.Replace("'", "''");
        private static string RepositoryRoot()
        {
            for (DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory); directory != null; directory = directory.Parent)
                if (File.Exists(Path.Combine(directory.FullName, "scripts", "TestTooling.ps1"))) return directory.FullName;
            throw new DirectoryNotFoundException("Cannot locate CI scripts.");
        }
        private static (int ExitCode, string Output) Run(string script, Dictionary<string, string>? environment = null, string? workingDirectory = null)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("pwsh")
            {
                UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true,
                WorkingDirectory = workingDirectory ?? RepositoryRoot()
            };
            foreach (string argument in new[] { "-NoProfile", "-NonInteractive", "-Command", script })
                process.StartInfo.ArgumentList.Add(argument);
            if (environment != null)
                foreach (var entry in environment) process.StartInfo.Environment[entry.Key] = entry.Value;
            Assert.IsTrue(process.Start());
            var stdout = process.StandardOutput.ReadToEndAsync();
            var stderr = process.StandardError.ReadToEndAsync();
            if (!process.WaitForExit(60000))
            {
                process.Kill(true);
                Assert.Fail("CI test child exceeded one minute.");
            }
            return (process.ExitCode, stdout.GetAwaiter().GetResult() + stderr.GetAwaiter().GetResult());
        }
        private sealed class TemporaryDirectory : IDisposable
        {
            public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "CiGuardrailTests-" + Guid.NewGuid().ToString("N"));
            public TemporaryDirectory() => Directory.CreateDirectory(this.Path);
            public void Dispose() => Directory.Delete(this.Path, true);
        }
    }
}
