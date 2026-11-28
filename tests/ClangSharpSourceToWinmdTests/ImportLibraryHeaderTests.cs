using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class ImportLibraryHeaderTests
    {
        private const string Predecessor = "c16762d41f1bd641ff8c4389ebf16ee50747129f";
        private const string Declaration = "int __stdcall Example(void* buffer, unsigned long count);";
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "webauthn.dll")]
        [DataRow("x86_64-pc-windows-msvc", "webauthn.dll")]
        [DataRow("aarch64-pc-windows-msvc", "webauthn.dll")]
        [DataRow("i686-pc-windows-msvc", "hhctrl.ocx")]
        [DataRow("x86_64-pc-windows-msvc", "hhctrl.ocx")]
        [DataRow("aarch64-pc-windows-msvc", "hhctrl.ocx")]
        public void HeaderOverridesScanAndDefault_PreservingCompleteFinalMetadata(string target, string module)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var baseline = fixture.Scrape(Declaration, target, sal: true,
                extraSettings: "--with-librarypath\nExample=" + module);
            Assert.AreEqual(0, baseline.ExitCode, baseline.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            foreach (string mapping in new[] { "", module, "incorrect.dll" })
            {
                var result = fixture.Scrape($"_Win32_ImportLibrary_(\"{module}\")\n{Declaration}", target, sal: true,
                    extraSettings: mapping.Length == 0 ? "" : "--with-librarypath\nExample=" + mapping);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string output = fixture.Emit();
                CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
                AssertImport(output, "Example", "Example", module);
            }
        }

        [DataTestMethod]
        [DataRow(false, "c")]
        [DataRow(true, "c")]
        [DataRow(false, "c++")]
        [DataRow(true, "c++")]
        public void EarlierHeaderRepeatedDeclarationsAndRemaps_PreserveTheNativeEntrypoint(bool remap, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string name = remap ? "Renamed" : "Example";
            var baseline = fixture.Scrape(Declaration, remap: remap ? name : null, language: language, sal: true,
                extraSettings: $"--with-librarypath\n{name}=hhctrl.ocx");
            Assert.AreEqual(0, baseline.ExitCode, baseline.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            var result = fixture.Scrape("_Win32_ImportLibrary_(\"hhctrl.ocx\")\n" + Declaration,
                includedHeader: Declaration, repeatInclude: true, remap: remap ? name : null, language: language,
                sal: true, extraSettings: $"--with-librarypath\n{name}=incorrect.dll");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            AssertImport(output, name, "Example", "hhctrl.ocx");
        }

        [DataTestMethod]
        [DataRow("Win32Metadata.ImportLibrary")]
        [DataRow("Win32Metadata.ImportLibrary()")]
        [DataRow("Win32Metadata.ImportLibrary(webauthn.dll)")]
        [DataRow("Win32Metadata.ImportLibrary(\"\")")]
        [DataRow("Win32Metadata.ImportLibrary(\"a.dll\", \"b.dll\")")]
        [DataRow("Win32Metadata.ImportLibrary(\"a.dll\")trailing")]
        [DataRow("Win32Metadata.ImportLibrary(\"../a.dll\")")]
        [DataRow("Win32Metadata.ImportLibrary(\"a b.dll\")")]
        public void MalformedModules_AreSourceLocatedFailures(string payload)
        {
            Reject("__attribute__((annotate(\"" + payload.Replace("\"", "\\\"") + "\"))) " + Declaration);
        }

        [DataTestMethod]
        [DataRow("_Win32_ImportLibrary_(\"a.dll\") _Win32_ImportLibrary_(\"b.dll\") " + Declaration)]
        [DataRow("_Win32_ImportLibrary_(\"a.dll\") " + Declaration + "\n_Win32_ImportLibrary_(\"b.dll\") " + Declaration)]
        [DataRow("__attribute__((annotate(\"Win32Metadata.ImportLibrary(\\\"a.dll\\\")\", 1))) " + Declaration)]
        [DataRow("int __stdcall _Win32_ImportLibrary_(\"a.dll\") Example(void);")]
        [DataRow("int Example(void) _Win32_ImportLibrary_(\"a.dll\");")]
        [DataRow("_Win32_ImportLibrary_(\"a.dll\") int Example(void) { return 0; }")]
        [DataRow("int Example(_Win32_ImportLibrary_(\"a.dll\") int* value);")]
        [DataRow("struct Record { _Win32_ImportLibrary_(\"a.dll\") int value; };")]
        [DataRow("struct _Win32_ImportLibrary_(\"a.dll\") Record { int value; };")]
        [DataRow("_Win32_ImportLibrary_(\"a.dll\") typedef int (*Callback)(int);")]
        [DataRow("struct Interface { virtual _Win32_ImportLibrary_(\"a.dll\") int Method(void) = 0; };")]
        public void ConflictsArgumentsTargetsAndPlacement_FailWithoutOutput(string header) => Reject(header);

        [TestMethod]
        public void EqualAnnotationsAndUnrelatedAttributes_PreserveAllExistingFacts()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string neighbors = "_Win32_SetLastError_ _Win32_SupportedOS_WindowsXP_\n";
            var baseline = fixture.Scrape(neighbors + Declaration, sal: true,
                extraSettings: "--with-librarypath\nExample=hhctrl.ocx");
            Assert.AreEqual(0, baseline.ExitCode, baseline.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            var result = fixture.Scrape("_Win32_ImportLibrary_(\"hhctrl.ocx\")\n" +
                "_Win32_ImportLibrary_(\"hhctrl.ocx\")\n" + neighbors + Declaration, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit()));
        }

        [TestMethod]
        public void UnannotatedImports_StillUseScanThenConfiguredDefault()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Declaration + "\nint __stdcall Unmapped(void);",
                extraSettings: "--with-librarypath\nExample=scanned.dll");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertImport(output, "Example", "Example", "scanned.dll");
            AssertImport(output, "Unmapped", "Unmapped", "test.dll");
        }

        [TestMethod]
        public void CompleteOwnerFamilies_RetireOnlyMatchingManualRecords()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var ancestry = LastErrorHeaderTests.Run("git", "-C", root, "merge-base", "--is-ancestor", Predecessor, "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, "The selected SDK legacy fixture must be reachable from the tested commit. " + ancestry.Output);
            var legacy = LastErrorHeaderTests.Run("git", "-C", root, "show",
                Predecessor + ":generation/WinSDK/libMappingsManual.rsp");
            Assert.AreEqual(0, legacy.ExitCode, legacy.Output);
            string old = legacy.Output.Replace("\r\n", "\n");
            var retired = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var owner in new[] { ("webauthnplugin.h", 27), ("HtmlHelp.h", 2) })
            {
                string path = "generation/WinSDK/RecompiledIdlHeaders/um/" + owner.Item1;
                string header = FullHeaderMigrationExperimentTests.ReadRecoveryText(
                    "RecompiledIdlHeaders/um/" + owner.Item1).Replace("\r\n", "\n");
                var original = LastErrorHeaderTests.Run("git", "-C", root, "show", Predecessor + ":" + path);
                Assert.AreEqual(0, original.ExitCode, original.Output);
                string stripped = Regex.Replace(header, @"_Win32_ImportLibrary_\(""[^""]+""\)(?:\n| )", "")
                    .Replace("#include <win32metadata_annotations.h>\n", "")
                    .Replace("// WebAuthnPlugin.dll/lib does not ship; these APIs are exported by webauthn.dll.\n", "");
                Assert.AreEqual(original.Output.Replace("\r\n", "\n"), stripped);
                var declarations = Regex.Matches(header,
                    @"_Win32_ImportLibrary_\(""([^""]+)""\)\s+(?:HRESULT|void|HWND)\s+(?:WINAPI\s+)(\w+)\(");
                Assert.AreEqual(owner.Item2, declarations.Count, owner.Item1);
                foreach (Match declaration in declarations)
                    retired.Add(declaration.Groups[2].Value, declaration.Groups[1].Value);
            }
            Assert.AreEqual(29, retired.Count);
            foreach (var fact in retired) StringAssert.Contains(old, fact.Key + "=" + fact.Value + "\n");
            string expected = string.Join("\n", old.Split('\n').Where(line => !retired.ContainsKey(line.Split('=')[0]) &&
                !line.StartsWith("# webauthnplugin.h APIs", StringComparison.Ordinal) &&
                !line.StartsWith("# The documentation lists WebAuthnPlugin", StringComparison.Ordinal) &&
                !line.StartsWith("# or in Windows; LoadLibrary", StringComparison.Ordinal)));
            Assert.AreEqual(expected, FullHeaderMigrationExperimentTests.ReadRecoveryText("libMappingsManual.rsp")
                .Replace("\r\n", "\n"));
            string inferred = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "libMappings.rsp"));
            foreach (string name in retired.Keys)
                Assert.IsFalse(Regex.IsMatch(inferred, @"(?m)^" + name + "="), name);
        }

        internal static void AssertImport(string output, string name, string entrypoint, string module)
        {
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == name);
            Assert.IsFalse(method.GetImport().Module.IsNil, name);
            Assert.AreEqual(entrypoint, reader.GetString(method.GetImport().Name), name);
            Assert.AreEqual(module, reader.GetString(reader.GetModuleReference(method.GetImport().Module).Name), name);
        }

        [DataTestMethod]
        [DataRow("webauthnplugin.h", "webauthn.dll", 27, "i686-pc-windows-msvc")]
        [DataRow("webauthnplugin.h", "webauthn.dll", 27, "x86_64-pc-windows-msvc")]
        [DataRow("webauthnplugin.h", "webauthn.dll", 27, "aarch64-pc-windows-msvc")]
        [DataRow("HtmlHelp.h", "hhctrl.ocx", 2, "i686-pc-windows-msvc")]
        [DataRow("HtmlHelp.h", "hhctrl.ocx", 2, "x86_64-pc-windows-msvc")]
        [DataRow("HtmlHelp.h", "hhctrl.ocx", 2, "aarch64-pc-windows-msvc")]
        public void CompleteShippedOwner_EmitsFinalImportsWithoutAnyMappingSidecar(
            string owner, string module, int count, string target)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string header = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner));
            var names = Regex.Matches(header,
                @"_Win32_ImportLibrary_\(""[^""]+""\)\s+(?:HRESULT|void|HWND)\s+WINAPI\s+(\w+)\(")
                .Cast<Match>().Select(match => match.Groups[1].Value).ToArray();
            Assert.AreEqual(count, names.Length);
            string prefix = "#include <windows.h>\n";
            System.Collections.Generic.Dictionary<string, string>? headers = null;
            if (owner == "webauthnplugin.h")
                prefix += File.ReadAllText(Path.Combine(include, "um", "webauthn.h")) + "\n";
            else
            {
                headers = new() { [owner] = header };
                prefix += "#include \"" + owner + "\"\n";
                header = string.Join("\n", Regex.Matches(header,
                    @"_Win32_ImportLibrary_\(""[^""]+""\) HWND\s+WINAPI\s+\w+\([^;]+;")
                    .Cast<Match>().Select(match => match.Value));
            }
            var compilerInclude = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; [IO.Path]::GetFullPath((Join-Path (Get-VcDirPath -Arch x64 -HostArch x64) '..\\..\\..\\include'))");
            Assert.AreEqual(0, compilerInclude.ExitCode, compilerInclude.Output);
            string settings = "--include-directory\n" + string.Join("\n",
                new[] { "um", "shared", "ucrt", "winrt" }.Select(folder => Path.Combine(include, folder))) +
                "\n" + compilerInclude.Output.Trim() +
                "\n--remap\nHWND__=void\n--additional\n-DWINVER=0x0A00\n-D_WIN32_WINNT=0x0A00\n-DNTDDI_VERSION=0x0A00000C";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var legacyHeaders = headers?.ToDictionary(pair => pair.Key,
                pair => Regex.Replace(pair.Value, @"_Win32_ImportLibrary_\(""[^""]+""\)", ""));
            var legacy = fixture.Scrape(prefix + Regex.Replace(header, @"_Win32_ImportLibrary_\(""[^""]+""\)", ""),
                target, sal: true, extraSettings: settings + "\n--with-librarypath\n" +
                string.Join("\n", names.Select(name => name + "=" + module)), headers: legacyHeaders);
            if (legacyHeaders != null)
                Save(Path.Combine(fixture.DirectoryPath, owner), owner + "-" + target + "-baseline-owner");
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), owner + "-" + target + "-baseline");
            Save(Path.Combine(fixture.DirectoryPath, "settings.rsp"), owner + "-" + target + "-baseline");
            Assert.AreEqual(0, legacy.ExitCode, legacy.Output);
            Save(fixture.GeneratedSource, owner + "-" + target + "-baseline");
            string expectedOutput = fixture.Emit();
            byte[] expected = File.ReadAllBytes(expectedOutput);
            Save(expectedOutput, owner + "-" + target + "-baseline");
            var result = fixture.Scrape(prefix + header, target, sal: true, extraSettings: settings, headers: headers);
            if (headers != null)
                Save(Path.Combine(fixture.DirectoryPath, owner), owner + "-" + target + "-native-owner");
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), owner + "-" + target + "-native");
            Save(Path.Combine(fixture.DirectoryPath, "settings.rsp"), owner + "-" + target + "-native");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, owner + "-" + target + "-native");
            string output = fixture.Emit();
            Save(output, owner + "-" + target + "-native");
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            using (var pe = new PEReader(File.OpenRead(output)))
            {
                var reader = pe.GetMetadataReader();
                var imports = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Where(method => !method.GetImport().Module.IsNil).Select(method => reader.GetString(method.Name)).ToArray();
                foreach (string name in names) CollectionAssert.Contains(imports, name);
                Assert.AreEqual(count, imports.Count(name => names.Contains(name)));
            }
            foreach (string name in names) AssertImport(output, name, name, module);
        }

        private void Save(string path, string name)
        {
            string destination = Path.Combine(TestContext.ResultsDirectory!, name + "-" + Path.GetFileName(path));
            File.Copy(path, destination, overwrite: true);
            TestContext.AddResultFile(destination);
        }

        private static void Reject(string header)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }
    }
}
