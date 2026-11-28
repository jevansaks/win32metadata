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
    public class FileSystemLastErrorHeaderTests
    {
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("fileapi.h", "i686-pc-windows-msvc")]
        [DataRow("fileapi.h", "x86_64-pc-windows-msvc")]
        [DataRow("fileapi.h", "aarch64-pc-windows-msvc")]
        [DataRow("WinBase.h", "i686-pc-windows-msvc")]
        [DataRow("WinBase.h", "x86_64-pc-windows-msvc")]
        [DataRow("WinBase.h", "aarch64-pc-windows-msvc")]
        public void FileSystemCompleteOwners_PreserveLastErrorAndExistingNativeFacts(string owner, string target)
        {
            TestContext context = TestContext;
            string root = LastErrorHeaderTests.RepositoryRoot();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] names = owner == "fileapi.h"
                ? new[] { "GetFileAttributesA", "GetFileAttributesW", "GetFileType", "QueryDosDeviceW" }
                : new[] { "QueryDosDeviceA", "FindFirstVolumeA", "FindFirstVolumeMountPointA", "FindFirstVolumeMountPointW" };
            string[] existing = owner == "fileapi.h"
                ? new[] { "DeleteFileA", "DeleteFileW" }
                : new[] { "CopyFileA", "CopyFileW", "MoveFileA", "MoveFileW" };
            string[] originalFacts = names.Concat(owner == "fileapi.h" ? new[] { "DeleteFile" } : new[] { "CopyFile", "MoveFile" }).ToArray();
            const string control = "AreFileApisANSI";
            const string library = "kernel32.dll";
            string[] allNames = names.Concat(existing).Append(control).ToArray();
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
                foreach (string file in new[] { "fileapi.h", "WinBase.h", "winnt.h" })
                {
                    if (native)
                        headers[file] = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", file));
                    else
                    {
                        var original = LastErrorHeaderTests.Run("git", "-C", root, "show",
                            "2b997fc06e67661bb3d1e28da7fa5e54d2e4cb88:generation/WinSDK/RecompiledIdlHeaders/um/" + file);
                        Assert.AreEqual(0, original.ExitCode, original.Output);
                        headers[file] = original.Output;
                    }
                }
                string selector = string.Join("|", allNames.Select(Regex.Escape));
                string[] declarations = new[] { headers["fileapi.h"], headers["WinBase.h"] }
                    .SelectMany(header => Regex.Matches(header,
                        @"(?m)^(?:(?:_Win32_[^\r\n]+|_Success_\([^\r\n]+\)|WINBASEAPI)\r?\n(?:\r?\n)*)*" +
                        @"(?:HANDLE|DWORD|BOOL)\r?\nWINAPI\r?\n(?:" + selector + @")\s*\([^;]*\);")
                        .Cast<Match>())
                    .Select(match => Regex.Replace(match.Value,
                        @"(?m)^_Win32_RAIIFree_\([^\r\n]+\)\r?\n", "")).ToArray();
                Assert.AreEqual(allNames.Length, declarations.Length, owner);
                // Keep the complete first-declaration context; do not replay cleanup carriers on late declarations.
                string wrapper = "#include <windows.h>\n" + string.Join("\n", declarations);
                string mode = nameof(FileSystemLastErrorHeaderTests) + "-" + owner + "-" + target + (native ? "-native" : "-legacy");
                var result = fixture.Scrape(wrapper, target, sal: true, libraryPath: library,
                    extraSettings: settings + (native ? "" : "\n--with-setlasterror\n" + string.Join("\n", originalFacts)),
                    headers: headers);
                foreach (string file in new[] { "fileapi.h", "WinBase.h", "winnt.h", "input.h", "settings.rsp" })
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
                    Assert.AreEqual(name != control, import.Attributes.HasFlag(MethodImportAttributes.SetLastError), name);
                }
                byte[] bytes = File.ReadAllBytes(output);
                if (!native) expected = bytes;
                else CollectionAssert.AreEqual(expected!, bytes, "The entire image, including existing native facts and the unchanged control, must be identical.");
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
