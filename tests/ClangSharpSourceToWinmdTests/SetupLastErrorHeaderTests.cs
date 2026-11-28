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
    public class SetupLastErrorHeaderTests
    {
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("SetupAPI.h", "i686-pc-windows-msvc")]
        [DataRow("SetupAPI.h", "x86_64-pc-windows-msvc")]
        [DataRow("SetupAPI.h", "aarch64-pc-windows-msvc")]
        [DataRow("newdev.h", "i686-pc-windows-msvc")]
        [DataRow("newdev.h", "x86_64-pc-windows-msvc")]
        [DataRow("newdev.h", "aarch64-pc-windows-msvc")]
        public void SetupCompleteOwners_PreserveLastErrorAndExistingControl(string owner, string target)
        {
            TestContext context = TestContext;
            string root = LastErrorHeaderTests.RepositoryRoot();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] names = owner == "newdev.h"
                ? new[] { "DiInstallDriverA", "DiUninstallDriverA" }
                : new[]
                {
                    "SetupDiCancelDriverInfoSearch", "SetupDiDestroyDeviceInfoList", "SetupGetFileCompressionInfoExW",
                };
            string control = owner == "newdev.h" ? "DiInstallDriverW" : "SetupOpenInfFileW";
            string library = owner == "newdev.h" ? "newdev.dll" : "setupapi.dll";
            var compilerInclude = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; [IO.Path]::GetFullPath((Join-Path (Get-VcDirPath -Arch x64 -HostArch x64) '..\\..\\..\\include'))");
            Assert.AreEqual(0, compilerInclude.ExitCode, compilerInclude.Output);
            string settings = "--include-directory\n" + string.Join("\n",
                new[] { "um", "shared", "ucrt", "winrt" }.Select(folder => Path.Combine(include, folder))) +
                "\n" + compilerInclude.Output.Trim() +
                "\n--remap\nHANDLE__=void\nHWND__=void" +
                "\n--additional\n-DWINVER=0x0A00\n-D_WIN32_WINNT=0x0A00\n-DNTDDI_VERSION=0x0A00000C";
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? expected = null;
            foreach (bool native in new[] { false, true })
            {
                var headers = new Dictionary<string, string>();
                foreach (string file in new[] { "SetupAPI.h", "newdev.h", "winnt.h" })
                {
                    if (native)
                        headers[file] = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", file));
                    else
                    {
                        var original = LastErrorHeaderTests.Run("git", "-C", root, "show",
                            "7422e923bc481b5694a05040e0746d9f39286cdc:generation/WinSDK/RecompiledIdlHeaders/um/" + file);
                        Assert.AreEqual(0, original.ExitCode, original.Output);
                        headers[file] = original.Output;
                    }
                }
                string selector = string.Join("|", names.Append(control).Select(Regex.Escape));
                string[] declarations = Regex.Matches(headers[owner],
                    @"(?m)^(?:_Win32_SetLastError_\r?\n)?(?:WINSETUPAPI\r?\n)?(?:BOOL|HDEVINFO|HINF)\r?\nWINAPI\r?\n(?:" +
                    selector + @")\s*\([^;]*\);").Cast<Match>().Select(match => match.Value).ToArray();
                Assert.AreEqual(names.Length + 1, declarations.Length, owner);
                string wrapper = "#include <windows.h>\n#include \"SetupAPI.h\"\n#include \"newdev.h\"\n" +
                    string.Join("\n", declarations);
                string mode = owner + "-" + target + (native ? "-native" : "-legacy");
                var result = fixture.Scrape(wrapper, target, sal: true, libraryPath: library,
                    extraSettings: settings + (native ? "" : "\n--with-setlasterror\n" + string.Join("\n", names)),
                    headers: headers);
                foreach (string file in new[] { "SetupAPI.h", "newdev.h", "winnt.h", "input.h", "settings.rsp" })
                    SaveEvidence(context, Path.Combine(fixture.DirectoryPath, file), mode);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                SaveEvidence(context, fixture.GeneratedSource, mode);
                string output = fixture.Emit();
                SaveEvidence(context, output, mode);
                using var pe = new PEReader(File.OpenRead(output));
                var reader = pe.GetMetadataReader();
                var methods = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Single(type => reader.GetString(type.Name) == "Apis").GetMethods()
                    .Select(reader.GetMethodDefinition).Where(method => !method.GetImport().Module.IsNil).ToArray();
                Assert.AreEqual(names.Length + 1, methods.Length);
                foreach (string name in names.Append(control))
                {
                    var method = methods.Single(method => reader.GetString(method.Name) == name);
                    var import = method.GetImport();
                    Assert.AreEqual(library, reader.GetString(reader.GetModuleReference(import.Module).Name));
                    Assert.AreEqual(name, reader.GetString(import.Name));
                    if (name != control)
                        Assert.IsTrue(import.Attributes.HasFlag(MethodImportAttributes.SetLastError), name);
                }
                byte[] bytes = File.ReadAllBytes(output);
                if (!native) expected = bytes;
                else CollectionAssert.AreEqual(expected!, bytes, "The entire owner image, including the existing control, must be identical.");
            }
        }

        private static void SaveEvidence(TestContext context, string path, string mode)
        {
            string destination = Path.Combine(context.ResultsDirectory!, mode + "-" + Path.GetFileName(path));
            File.Copy(path, destination, overwrite: true);
            context.AddResultFile(destination);
        }
    }
}
