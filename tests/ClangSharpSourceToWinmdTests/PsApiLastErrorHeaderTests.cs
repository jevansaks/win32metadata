using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class PsApiLastErrorHeaderTests
    {
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow(1, "i686-pc-windows-msvc")]
        [DataRow(1, "x86_64-pc-windows-msvc")]
        [DataRow(1, "aarch64-pc-windows-msvc")]
        [DataRow(2, "i686-pc-windows-msvc")]
        [DataRow(2, "x86_64-pc-windows-msvc")]
        [DataRow(2, "aarch64-pc-windows-msvc")]
        public void PsApiCompleteOwners_PreserveNamedExportLastErrorAcrossVersions(int version, string target)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string header = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", "Psapi.h"));
            string winnt = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", "winnt.h"));
            string[] names = { "EnumProcesses", "EmptyWorkingSet", "QueryWorkingSet" };
            var declarations = Regex.Matches(header,
                @"(?m)^(?:#if PSAPI_VERSION <= 1\r?\n)?(?:_Win32_SetLastError_\r?\n)?(?:#endif\r?\n)?BOOL\r?\nWINAPI\r?\n(?:EnumProcesses|EmptyWorkingSet|QueryWorkingSet)\s*\([^;]*\);")
                .Cast<Match>().Select(match => match.Value).ToArray();
            Assert.AreEqual(names.Length, declarations.Length);
            string wrapper = "#include <windows.h>\n#include \"Psapi.h\"\n" + string.Join("\n", declarations);
            var compilerInclude = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; [IO.Path]::GetFullPath((Join-Path (Get-VcDirPath -Arch x64 -HostArch x64) '..\\..\\..\\include'))");
            Assert.AreEqual(0, compilerInclude.ExitCode, compilerInclude.Output);
            string settings = "--include-directory\n" + string.Join("\n",
                new[] { "um", "shared", "ucrt", "winrt" }.Select(folder => Path.Combine(include, folder))) +
                "\n" + compilerInclude.Output.Trim() +
                "\n--remap\nHANDLE__=void\n--additional\n-DPSAPI_VERSION=" + version +
                "\n-DWINVER=0x0A00\n-D_WIN32_WINNT=0x0A00\n-DNTDDI_VERSION=0x0A00000C";
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? expected = null;
            foreach (bool native in new[] { false, true })
            {
                string mode = "psapi-" + version + "-" + target + (native ? "-native" : "-legacy");
                var result = fixture.Scrape(native ? wrapper : wrapper.Replace("_Win32_SetLastError_", ""),
                    target, sal: true, libraryPath: version == 1 ? "PSAPI.dll" : "KERNEL32.dll",
                    extraSettings: settings + (native ? "" : "\n--with-setlasterror\n" + string.Join("\n", names)),
                    headers: new Dictionary<string, string>
                    {
                        ["Psapi.h"] = native ? header : header.Replace("_Win32_SetLastError_", ""),
                        ["winnt.h"] = winnt,
                    });
                SaveEvidence(Path.Combine(fixture.DirectoryPath, "Psapi.h"), mode);
                SaveEvidence(Path.Combine(fixture.DirectoryPath, "winnt.h"), mode);
                SaveEvidence(Path.Combine(fixture.DirectoryPath, "input.h"), mode);
                SaveEvidence(Path.Combine(fixture.DirectoryPath, "settings.rsp"), mode);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                SaveEvidence(fixture.GeneratedSource, mode);
                string output = fixture.Emit();
                SaveEvidence(output, mode);
                using var pe = new PEReader(File.OpenRead(output));
                var reader = pe.GetMetadataReader();
                var methods = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Single(type => reader.GetString(type.Name) == "Apis")
                    .GetMethods().Select(reader.GetMethodDefinition).ToArray();
                Assert.AreEqual(names.Length, methods.Length);
                foreach (string name in names)
                {
                    string exported = version == 1 ? name : "K32" + name;
                    var method = methods.Single(method => reader.GetString(method.Name) == exported);
                    var import = method.GetImport();
                    Assert.AreEqual(version == 1, import.Attributes.HasFlag(MethodImportAttributes.SetLastError), exported);
                    Assert.AreEqual(version == 1 ? "PSAPI.dll" : "KERNEL32.dll",
                        reader.GetString(reader.GetModuleReference(import.Module).Name));
                    Assert.AreEqual(exported, reader.GetString(import.Name));
                    Assert.AreEqual(name == "EmptyWorkingSet" ? 1 : 3,
                        method.GetParameters().Count(parameter => reader.GetParameter(parameter).SequenceNumber != 0));
                }
                byte[] bytes = File.ReadAllBytes(output);
                if (!native) expected = bytes;
                else CollectionAssert.AreEqual(expected!, bytes, "Complete legacy/native images must be identical.");
            }
        }

        private void SaveEvidence(string path, string mode)
        {
            string destination = Path.Combine(TestContext.ResultsDirectory!, mode + "-" + Path.GetFileName(path));
            File.Copy(path, destination, overwrite: true);
            TestContext.AddResultFile(destination);
        }
    }
}
