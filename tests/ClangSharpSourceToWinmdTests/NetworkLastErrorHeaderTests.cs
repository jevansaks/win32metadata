using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class NetworkLastErrorHeaderTests
    {
        private const string QualifiedParent = "33797f414fd9f3448db2711cfda627297d1a884e";
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("WinSock2.h", "i686-pc-windows-msvc")]
        [DataRow("WinSock2.h", "x86_64-pc-windows-msvc")]
        [DataRow("WinSock2.h", "aarch64-pc-windows-msvc")]
        [DataRow("winsock.h", "i686-pc-windows-msvc")]
        [DataRow("winsock.h", "x86_64-pc-windows-msvc")]
        [DataRow("winsock.h", "aarch64-pc-windows-msvc")]
        public void NetworkCompleteOwners_PreserveImportsLastErrorAndExistingControl(string owner, string target)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string header = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner));
            var original = LastErrorHeaderTests.Run("git", "-C", root, "show",
                QualifiedParent + ":generation/WinSDK/RecompiledIdlHeaders/um/" + owner);
            Assert.AreEqual(0, original.ExitCode, original.Output);
            string[] names = owner == "WinSock2.h"
                ? new[] { "WSACreateEvent", "WSAGetLastError", "inet_addr", "WSAIsBlocking" }
                : new[] { "socket", "WSAGetLastError", "inet_addr", "WSAIsBlocking" };
            string declarations = string.Join("\n", names.Select(name => Declaration(header, owner, name)));
            string wrapper = "#include \"" + owner + "\"\n" + declarations;
            var compilerInclude = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; [IO.Path]::GetFullPath((Join-Path (Get-VcDirPath -Arch x64 -HostArch x64) '..\\..\\..\\include'))");
            Assert.AreEqual(0, compilerInclude.ExitCode, compilerInclude.Output);
            string settings = "--include-directory\n" + string.Join("\n",
                new[] { "um", "shared", "ucrt", "winrt" }.Select(folder => Path.Combine(include, folder))) +
                "\n" + compilerInclude.Output.Trim() +
                "\n--remap\nHANDLE__=void\n--additional\n-DWINVER=0x0A00\n-D_WIN32_WINNT=0x0A00\n-DNTDDI_VERSION=0x0A00000C";
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? originalLegacy = null;
            byte[]? expected = null;
            foreach (string contract in new[] { "legacy-original", "legacy-corrected", "native" })
            {
                bool native = contract == "native";
                string mode = owner + "-" + target + "-" + contract;
                string input = native ? wrapper : "#include \"" + owner + "\"\n" +
                    string.Join("\n", names.Select(name => Declaration(original.Output, owner, name)));
                var result = fixture.Scrape(input, target, sal: true, libraryPath: "WS2_32.dll",
                    extraSettings: settings + (native ? "" : "\n--with-setlasterror\n" +
                        string.Join("\n", names.Where(name => name != "WSAIsBlocking" &&
                            (name != "WSAGetLastError" || contract == "legacy-original")))),
                    headers: new Dictionary<string, string> { [owner] = native ? header : original.Output });
                Save(Path.Combine(fixture.DirectoryPath, owner), mode);
                Save(Path.Combine(fixture.DirectoryPath, "input.h"), mode);
                Save(Path.Combine(fixture.DirectoryPath, "settings.rsp"), mode);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                Save(fixture.GeneratedSource, mode);
                string output = fixture.Emit();
                Save(output, mode);
                using var pe = new PEReader(File.OpenRead(output));
                var reader = pe.GetMetadataReader();
                var methods = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Single(type => reader.GetString(type.Name) == "Apis").GetMethods()
                    .Select(reader.GetMethodDefinition).ToArray();
                Assert.AreEqual(names.Length, methods.Length);
                foreach (string name in names)
                {
                    var method = methods.Single(method => reader.GetString(method.Name) == name);
                    var import = method.GetImport();
                    Assert.AreEqual(name == "WSAGetLastError" ? contract == "legacy-original" :
                        name != "WSAIsBlocking" || owner == "WinSock2.h",
                        import.Attributes.HasFlag(MethodImportAttributes.SetLastError), name);
                    Assert.AreEqual("WS2_32.dll", reader.GetString(reader.GetModuleReference(import.Module).Name), name);
                    Assert.AreEqual(name, reader.GetString(import.Name), name);
                    Assert.AreEqual(name == "socket" ? 3 : name == "inet_addr" ? 1 : 0,
                        method.GetParameters().Count(parameter => reader.GetParameter(parameter).SequenceNumber != 0), name);
                }
                byte[] bytes = File.ReadAllBytes(output);
                if (contract == "legacy-original") originalLegacy = bytes;
                else if (contract == "legacy-corrected")
                {
                    Assert.AreEqual(originalLegacy!.Length, bytes.Length);
                    using var originalPe = new PEReader(new MemoryStream(originalLegacy));
                    int stampOffset = originalPe.PEHeaders.CoffHeaderStartOffset + 4;
                    byte[] mvid = originalPe.GetMetadataReader()
                        .GetGuid(originalPe.GetMetadataReader().GetModuleDefinition().Mvid).ToByteArray();
                    int[] guidOffsets = Enumerable.Range(0, originalLegacy.Length - mvid.Length + 1)
                        .Where(index => originalLegacy.AsSpan(index, mvid.Length).SequenceEqual(mvid)).ToArray();
                    Assert.AreEqual(1, guidOffsets.Length);
                    int guidOffset = guidOffsets[0];
                    int[] offsets = Enumerable.Range(0, bytes.Length)
                        .Where(index => originalLegacy[index] != bytes[index] &&
                            (index < stampOffset || index >= stampOffset + 4) &&
                            (index < guidOffset || index >= guidOffset + 16)).ToArray();
                    Assert.AreEqual(1, offsets.Length, "Only the getter's import flag may change.");
                    int offset = offsets[0];
                    Assert.AreEqual(0x40, originalLegacy[offset] ^ bytes[offset]);
                    Assert.AreEqual(0x40, originalLegacy[offset] & 0x40);
                    Assert.AreEqual(0, bytes[offset] & 0x40);
                    byte[] restoredIdentity = RewriteContentId(originalLegacy, guidOffset, stampOffset);
                    CollectionAssert.AreEqual(originalLegacy, restoredIdentity, "Original deterministic identity must verify.");
                    byte[] correctedImage = originalLegacy.ToArray();
                    correctedImage[offset] ^= 0x40;
                    correctedImage = RewriteContentId(correctedImage, guidOffset, stampOffset);
                    CollectionAssert.AreEqual(correctedImage, bytes,
                        "The entire image must equal the exact getter correction with recomputed deterministic identity.");
                    string proof = Path.Combine(fixture.DirectoryPath, "flag-correction.json");
                    File.WriteAllText(proof, JsonSerializer.Serialize(new
                    {
                        method = "WSAGetLastError", owner, target, offset,
                        originalByte = originalLegacy[offset], correctedByte = bytes[offset],
                        stampOffset, guidOffset,
                        originalSetLastError = true, correctedSetLastError = false,
                        allOtherPayloadBytesIdentical = true, fullRecomputedImageEqual = true
                    }));
                    Save(proof, mode);
                    expected = bytes;
                }
                else CollectionAssert.AreEqual(expected!, bytes, "Complete corrected legacy/native images must be identical.");
            }
        }

        private static byte[] RewriteContentId(byte[] image, int guidOffset, int stampOffset)
        {
            byte[] result = image.ToArray();
            Array.Clear(result, guidOffset, 16);
            Array.Clear(result, stampOffset, 4);
            var id = BlobContentId.FromHash(SHA256.HashData(result));
            id.Guid.ToByteArray().CopyTo(result, guidOffset);
            BitConverter.GetBytes(id.Stamp).CopyTo(result, stampOffset);
            return result;
        }

        private static string Declaration(string header, string owner, string name)
        {
            string normalized = header.Replace("\r\n", "\n");
            string pattern = owner == "WinSock2.h"
                ? @"(?m)^(?:_Win32_SetLastError_\n)?(?:_WINSOCK_DEPRECATED_BY\(""[^""]*""\)\n)?WINSOCK_API_LINKAGE\n(?:_Must_inspect_result_\n)?[^\n;]+\nWSAAPI\n" +
                    Regex.Escape(name) + @"\s*\([^;]*\);"
                : @"(?m)^(?:_Win32_SetLastError_\n)?[^\n;#/]*\bPASCAL\s+FAR\s+" + Regex.Escape(name) + @"\s*\([^;]*\);";
            var matches = Regex.Matches(normalized, pattern);
            Assert.AreEqual(1, matches.Count, owner + "::" + name);
            return matches[0].Value;
        }

        private void Save(string path, string mode)
        {
            string destination = Path.Combine(TestContext.ResultsDirectory!, mode + "-" + Path.GetFileName(path));
            File.Copy(path, destination, overwrite: true);
            TestContext.AddResultFile(destination);
        }
    }
}
