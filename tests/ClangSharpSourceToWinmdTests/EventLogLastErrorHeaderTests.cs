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
    public class EventLogLastErrorHeaderTests
    {
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("winevt.h", "i686-pc-windows-msvc")]
        [DataRow("winevt.h", "x86_64-pc-windows-msvc")]
        [DataRow("winevt.h", "aarch64-pc-windows-msvc")]
        [DataRow("WinBase.h", "i686-pc-windows-msvc")]
        [DataRow("WinBase.h", "x86_64-pc-windows-msvc")]
        [DataRow("WinBase.h", "aarch64-pc-windows-msvc")]
        public void EventLogCompleteOwners_PreserveLastErrorAndControls(string owner, string target)
        {
            TestContext context = TestContext;
            string root = LastErrorHeaderTests.RepositoryRoot();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] names = owner == "winevt.h"
                ? new[] { "EvtCreateBookmark", "EvtCreateRenderContext", "EvtQuery", "EvtOpenEventMetadataEnum", "EvtNextEventMetadata" }
                : new[] { "OpenEventLogA", "OpenEventLogW", "OpenBackupEventLogA", "OpenBackupEventLogW", "RegisterEventSourceA", "RegisterEventSourceW" };
            string control = owner == "winevt.h" ? "EvtGetExtendedStatus" : "CloseEventLog";
            string[] preserved = owner == "winevt.h"
                ? new[] { "EvtClose", "EvtNext", "EvtNextChannelPath", "EvtSaveChannelConfig", "EvtNextPublisherId", "EvtUpdateBookmark" }
                : System.Array.Empty<string>();
            string[] allNames = names.Concat(preserved).Append(control).ToArray();
            string library = owner == "winevt.h" ? "wevtapi.dll" : "advapi32.dll";
            var compilerInclude = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; [IO.Path]::GetFullPath((Join-Path (Get-VcDirPath -Arch x64 -HostArch x64) '..\\..\\..\\include'))");
            Assert.AreEqual(0, compilerInclude.ExitCode, compilerInclude.Output);
            string settings = "--include-directory\n" +
                Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um") + "\n" +
                Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "shared") + "\n" + string.Join("\n",
                new[] { "um", "shared", "ucrt", "winrt" }.Select(folder => Path.Combine(include, folder))) +
                "\n" + compilerInclude.Output.Trim() +
                "\n--remap\nHANDLE__=void\nHWND__=void" +
                "\n--additional\n-DWINVER=0x0A00\n-D_WIN32_WINNT=0x0A00";
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? expected = null;
            var implementationFlags = new Dictionary<string, MethodImplAttributes>();
            foreach (bool native in new[] { false, true })
            {
                var headers = new Dictionary<string, string>();
                foreach (string file in new[] { "winevt.h", "WinBase.h", "winnt.h" })
                {
                    if (native)
                        headers[file] = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", file));
                    else
                    {
                        var original = LastErrorHeaderTests.Run("git", "-C", root, "show",
                            "d7ccbeea131c32f9a54ea803e4c420041ed7c410:generation/WinSDK/RecompiledIdlHeaders/um/" + file);
                        Assert.AreEqual(0, original.ExitCode, original.Output);
                        headers[file] = original.Output;
                    }
                }
                string selector = string.Join("|", allNames.Select(Regex.Escape));
                string[] declarations = Regex.Matches(headers[owner],
                    @"(?m)^(?:(?:_Win32_[^\r\n]+|_Success_\([^\r\n]+\)|WINADVAPI)\r?\n(?:\r?\n)*)*" +
                    @"(?:EVT_HANDLE|HANDLE|DWORD|BOOL)\r?\nWINAPI\r?\n(?:" + selector + @")\s*\([^;]*\);")
                    .Cast<Match>().Select(match => Regex.Replace(match.Value,
                        @"(?m)^_Win32_RAIIFree_\([^\r\n]+\)\r?\n", "")).ToArray();
                Assert.AreEqual(allNames.Length, declarations.Length, owner);
                // Do not replay an unrelated producer carrier on a late comparison declaration.
                // Both paths retain the complete native owner and its existing cleanup annotations.
                string wrapper = "#include <windows.h>\n#include \"winevt.h\"\n" + string.Join("\n", declarations);
                string mode = owner + "-" + target + (native ? "-native" : "-legacy");
                var result = fixture.Scrape(wrapper, target, sal: true, libraryPath: library,
                    extraSettings: settings + (native ? "" : "\n--with-setlasterror\n" + string.Join("\n", names)),
                    headers: headers);
                foreach (string file in new[] { "winevt.h", "WinBase.h", "winnt.h", "input.h", "settings.rsp" })
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
                Assert.AreEqual(allNames.Length, methods.Length);
                foreach (string name in allNames)
                {
                    var method = methods.Single(method => reader.GetString(method.Name) == name);
                    var import = method.GetImport();
                    Assert.AreEqual(library, reader.GetString(reader.GetModuleReference(import.Module).Name));
                    Assert.AreEqual(name, reader.GetString(import.Name));
                    Assert.IsTrue(method.Attributes.HasFlag(MethodAttributes.PinvokeImpl), name);
                    if (!native) implementationFlags.Add(name, method.ImplAttributes);
                    else Assert.AreEqual(implementationFlags[name], method.ImplAttributes, name);
                    Assert.AreEqual(name != "EvtGetExtendedStatus",
                        import.Attributes.HasFlag(MethodImportAttributes.SetLastError), name);
                }
                byte[] bytes = File.ReadAllBytes(output);
                if (!native) expected = bytes;
                else CollectionAssert.AreEqual(expected!, bytes, "The entire owner image, including the unchanged control, must be identical.");
            }
        }

        private static void SaveEvidence(TestContext context, string path, string mode)
        {
            string directory = Path.Combine(context.ResultsDirectory!, mode);
            Directory.CreateDirectory(directory);
            string destination = Path.Combine(directory, Path.GetFileName(path));
            File.Copy(path, destination, overwrite: false);
            context.AddResultFile(destination);
        }
    }
}
