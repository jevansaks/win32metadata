using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using ClangSharpSourceToWinmd;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class NativeTypedefSdkTests
    {
        private static readonly string[] Pilots = { "HRESULT", "NCRYPT_HANDLE", "NCRYPT_KEY_HANDLE" };
        private const string Foundation = "Windows.Win32.Foundation";
        private const string Cryptography = "Windows.Win32.Security.Cryptography";
        public TestContext TestContext { get; set; } = null!;

        private static string Parent()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/NativeTypedefSdkTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the native typedef fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static string ApplyAutoTypesDelta(string original)
        {
            int changed = 0;
            string result = Regex.Replace(original.Replace("\r\n", "\n"), @"(?ms)^  \{\n.*?^  \}", match =>
            {
                string name = Regex.Match(match.Value, "\"Name\": \"([^\"]+)\"").Groups[1].Value;
                if (!Pilots.Contains(name)) return match.Value;
                changed++;
                StringAssert.Contains(match.Value, "\"ValueType\": \"" + (name == "HRESULT" ? "int" : "UIntPtr") + "\"");
                StringAssert.Contains(match.Value, "\"NativeTypedef\": true");
                string updated = match.Value.Replace("\"ValueType\": \"" + (name == "HRESULT" ? "int" : "UIntPtr") + "\"",
                    "\"ValueType\": \"native\"", StringComparison.Ordinal)
                    .Replace(",\n    \"NativeTypedef\": true", "", StringComparison.Ordinal);
                if (name == "NCRYPT_KEY_HANDLE")
                    updated = updated.Replace("    \"AlsoUsableFor\": \"NCRYPT_HANDLE\",\n", "", StringComparison.Ordinal);
                return updated;
            });
            Assert.AreEqual(3, changed);
            return result;
        }

        [TestMethod]
        public void ExactParent_OnlyThreeIdentityRecordsAndOneRelationshipMove()
        {
            string parent = Parent();
            string original = Encoding.UTF8.GetString(PropertyComOutputHeaderTests.ReadOriginalBytes("autoTypes.json", parent));
            Assert.AreEqual(ApplyAutoTypesDelta(original), File.ReadAllText(Sdk("autoTypes.json")).Replace("\r\n", "\n"));
            foreach (string path in new[] { "emitter.settings.rsp", "scraper.settings.rsp", "functionPointerFixups.json",
                "RecompiledIdlHeaders/shared/wtypes.h", "RecompiledIdlHeaders/shared/basetsd.h",
                "RecompiledIdlHeaders/shared/minwindef.h", "RecompiledIdlHeaders/um/winnt.h",
                "RecompiledIdlHeaders/um/winhttp.h" })
                CollectionAssert.AreEqual(PropertyComOutputHeaderTests.ReadOriginalBytes(path, parent),
                    File.ReadAllBytes(Sdk(path)), "Independent native/legacy facts stay byte-exact: " + path);
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", parent, "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
        }

        [TestMethod]
        public void ActualAutoTypePreparation_SelectsOnlyCompilerBackedPilots()
        {
            string directory = Path.Combine(Path.GetTempPath(), "native-settings-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            try
            {
                string output = Path.Combine(directory, "settings.rsp");
                var task = new MetadataTasks.PrepSettingsForAutoTypes
                {
                    AutoTypes = new[] { new TaskItem(Sdk("autoTypes.json")) },
                    ScraperAutoTypesRsp = output,
                };
                Assert.IsTrue(task.Execute());
                string text = File.ReadAllText(output).Replace("\r\n", "\n");
                StringAssert.StartsWith(text, "--native-typedef\nHRESULT=Windows.Win32.Foundation\n" +
                    "NCRYPT_HANDLE=Windows.Win32.Security.Cryptography\nNCRYPT_KEY_HANDLE\n--remap\n");
                string remaps = text.Substring(text.IndexOf("--remap\n", StringComparison.Ordinal));
                foreach (string name in Pilots)
                    Assert.IsFalse(remaps.Contains(name + "=", StringComparison.Ordinal));
            }
            finally { Directory.Delete(directory, true); }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void SelectedSdkPatchReplay_EmitsRealStorageReferencesAndRelationship(string target)
        {
            string directory = Path.Combine(Path.GetTempPath(), "typedef-sdk-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path.Combine(directory, "um"));
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            try
            {
                File.Copy(Path.Combine(include, "um", "ncrypt.h"), Path.Combine(directory, "um", "ncrypt.h"));
                var apply = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                    "& " + Quote(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "ApplySDKPatches.ps1")) +
                    " -Phase post-midl -HeaderRoot " + Quote(directory) + " -MidlInputRoot " + Quote(include) +
                    " -SdkVersion '10.0.28000.2705' -SelectedPatches @('post-midl/ncrypt.h.patch')");
                Assert.AreEqual(0, apply.ExitCode, apply.Output);
                CollectionAssert.AreEqual(File.ReadAllBytes(Sdk("RecompiledIdlHeaders/um/ncrypt.h")),
                    File.ReadAllBytes(Path.Combine(directory, "um", "ncrypt.h")), "Complete native bytes, including CRLF/EOF.");

                using var fixture = new LastErrorHeaderTests.Fixture();
                string declarations = Declarations(include, directory);
                string settings = "--native-typedef\nHRESULT=Test\nNCRYPT_HANDLE=Test\nNCRYPT_KEY_HANDLE\n" +
                    "--remap\nULONG_PTR=UIntPtr\nSECURITY_STATUS=HRESULT\n--include-directory\n" + Path.Combine(include, "shared");
                var result = fixture.Scrape(declarations, target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                var namespaces = new Dictionary<string, string>
                {
                    ["HRESULT"] = Foundation, ["NCRYPT_HANDLE"] = Cryptography, ["NCRYPT_KEY_HANDLE"] = Cryptography,
                    ["NCryptFreeObject"] = Cryptography, ["NCryptDeleteKey"] = Cryptography,
                };
                string output = fixture.Emit(requiredNamespaces: namespaces);
                AssertFinal(output, legacy: false);
                byte[] native = File.ReadAllBytes(output);

                // Residual cleanup/invalid values stay legacy; they are not inferred from typedef identity.
                var items = AutoTypeHelpers.GetAutoTypesFromFiles(new[] { Sdk("autoTypes.json") })
                    .Where(item => Pilots.Contains(item.Name)).ToArray();
                using (var stream = File.Create(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "autotypes.cs")))
                    NativeTypedefStructsCreator.WriteToStream(namespaces, items, stream);
                output = fixture.Emit(requiredNamespaces: namespaces);
                AssertFinal(output, legacy: true);
                string proof = Path.Combine(this.TestContext.ResultsDirectory, "native-typedef-" + target + ".winmd");
                File.Copy(output, proof, true);
                this.TestContext.AddResultFile(proof);

                File.Delete(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "autotypes.cs"));
                result = fixture.Scrape(declarations.Replace("_Win32_AlsoUsableFor_(NCRYPT_HANDLE)", "", StringComparison.Ordinal),
                    target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                output = fixture.Emit(new Dictionary<string, string> { ["NCRYPT_KEY_HANDLE"] = "[AlsoUsableFor(\"NCRYPT_HANDLE\")]" }, namespaces);
                CollectionAssert.AreEqual(native, File.ReadAllBytes(output), "Exact native/legacy relationship parity on the same SDK declarations.");
                output = fixture.Emit(requiredNamespaces: namespaces);
                using var withoutAssociation = new PEReader(File.OpenRead(output));
                Assert.IsFalse(withoutAssociation.GetMetadataReader().CustomAttributes.Select(withoutAssociation.GetMetadataReader().GetCustomAttribute)
                    .Any(attribute => ParameterHeaderTests.AttributeName(withoutAssociation.GetMetadataReader(), attribute) == "AlsoUsableForAttribute"));
            }
            finally { Directory.Delete(directory, true); }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void RealSdkNativeCompiler_OrdinaryAndMetadataObjectsHaveIdenticalAbi(string arch, int pointerSize, string language)
        {
            string directory = Path.Combine(Path.GetTempPath(), "typedef-native-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            try
            {
                File.Copy(Sdk("RecompiledIdlHeaders/um/ncrypt.h"), Path.Combine(directory, "ncrypt.h"));
                File.Copy(Sdk("RecompiledIdlHeaders/shared/win32metadata_annotations.h"),
                    Path.Combine(directory, "win32metadata_annotations.h"));
                string source = Path.Combine(directory, "abi.c");
                File.WriteAllText(source, @"
#include <windows.h>
#include <ncrypt.h>
typedef char ResultWidth[(sizeof(HRESULT) == 4 && (HRESULT)-1 < 0) ? 1 : -1];
typedef char KeyWidth[(sizeof(NCRYPT_KEY_HANDLE) == " + pointerSize + @" && (NCRYPT_KEY_HANDLE)-1 > 0) ? 1 : -1];
typedef char SignedWidth[(sizeof(LPARAM) == sizeof(void*) && (LPARAM)-1 < 0) ? 1 : -1];
SECURITY_STATUS (WINAPI *releaseObject)(NCRYPT_HANDLE) = &NCryptFreeObject;
SECURITY_STATUS (WINAPI *deleteKey)(NCRYPT_KEY_HANDLE, DWORD) = &NCryptDeleteKey;
_Win32_AlsoUsableFor_(Undeclared) typedef ULONG_PTR NoopAlias;
_Win32_PreserveResult_ typedef HRESULT (WINAPI *ResultCallback)(_In_ void*);
typedef ResultCallback* LPResultCallback;
typedef char CallbackWidth[(sizeof(*((LPResultCallback)0)) == sizeof(void*)) ? 1 : -1];
");
                string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
                string script = ". " + Quote(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "CommonUtils.ps1")) +
                    "; Install-VsDevShell; $compiler = Join-Path (Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64) 'cl.exe'; " +
                    "foreach ($mode in 'ordinary','metadata') { $arguments = @('/nologo','/c','/Brepro','/WX'," +
                    Quote(language) + "," + Quote(source) + "," + Quote("/Fo" + Path.Combine(directory, "abi.obj")) +
                    "," + Quote("/I" + directory) + "," + Quote("/I" + Path.Combine(include, "um")) +
                    "," + Quote("/I" + Path.Combine(include, "shared")) + "," + Quote("/I" + Path.Combine(include, "ucrt")) +
                    "); if ($mode -eq 'metadata') { $arguments += '/DWIN32METADATA' }; & $compiler @arguments; " +
                    "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " +
                    Quote(Path.Combine(directory, "abi.obj")) + " -Destination (" + Quote(directory + "\\") + " + $mode + '.obj') }";
                var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command", script);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] ordinary = File.ReadAllBytes(Path.Combine(directory, "ordinary.obj"));
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(ordinary, 0));
                CollectionAssert.AreEqual(ordinary, File.ReadAllBytes(Path.Combine(directory, "metadata.obj")));
            }
            finally { Directory.Delete(directory, true); }
        }

        private static string Declarations(string include, string patched)
        {
            string winnt = File.ReadAllText(Path.Combine(include, "um", "winnt.h"));
            string ncrypt = File.ReadAllText(Path.Combine(patched, "um", "ncrypt.h"));
            string Scalar(string pattern) => Slice(winnt, pattern);
            return "#include <basetsd.h>\n#define WINAPI __stdcall\n" +
                Scalar(@"typedef long LONG;") + "\n" +
                Scalar(@"typedef _Return_type_success_\(return >= 0\) long HRESULT;") + "\n" +
                Slice(ncrypt, @"typedef LONG SECURITY_STATUS;") + "\ntypedef unsigned long DWORD;\n" +
                Slice(ncrypt, @"typedef ULONG_PTR NCRYPT_HANDLE;") + "\n" +
                Slice(ncrypt, @"(?:_Win32_AlsoUsableFor_\(NCRYPT_HANDLE\)\r?\n)?typedef ULONG_PTR NCRYPT_KEY_HANDLE;") + "\n" +
                Slice(ncrypt, @"SECURITY_STATUS\r?\nWINAPI\r?\nNCryptFreeObject\([^;]+;") + "\n" +
                Slice(ncrypt, @"SECURITY_STATUS\r?\nWINAPI\r?\nNCryptDeleteKey\([^;]+;");
        }

        private static string Slice(string source, string pattern)
        {
            var matches = Regex.Matches(source, pattern);
            Assert.AreEqual(1, matches.Count, pattern);
            return matches[0].Value;
        }

        private static void AssertFinal(string output, bool legacy)
        {
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            NativeTypedefHeaderTests.AssertStorage(reader, "HRESULT", SignatureTypeCode.Int32);
            NativeTypedefHeaderTests.AssertStorage(reader, "NCRYPT_HANDLE", SignatureTypeCode.UIntPtr);
            NativeTypedefHeaderTests.AssertStorage(reader, "NCRYPT_KEY_HANDLE", SignatureTypeCode.UIntPtr);
            var association = reader.GetBlobReader(NativeTypedefHeaderTests.Type(reader, "NCRYPT_KEY_HANDLE")
                .GetCustomAttributes().Select(reader.GetCustomAttribute).Single(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) == "AlsoUsableForAttribute").Value);
            Assert.AreEqual((ushort)1, association.ReadUInt16());
            Assert.AreEqual("NCRYPT_HANDLE", association.ReadSerializedString());
            Assert.AreEqual((ushort)0, association.ReadUInt16());
            Assert.AreEqual(0, association.RemainingBytes);
            foreach (var pair in new[] { ("NCryptFreeObject", "NCRYPT_HANDLE", 1), ("NCryptDeleteKey", "NCRYPT_KEY_HANDLE", 2) })
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(item => reader.GetString(item.Name) == pair.Item1);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(pair.Item3, signature.ReadCompressedInteger());
                NativeTypedefHeaderTests.AssertReference(reader, ref signature, "HRESULT", Foundation);
                NativeTypedefHeaderTests.AssertReference(reader, ref signature, pair.Item2, Cryptography);
                if (pair.Item3 == 2) Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
                Assert.AreEqual(0, signature.RemainingBytes);
                ProducerHeaderTests.AssertNoOwnership(reader, pair.Item1);
            }
            foreach (string name in Pilots)
            {
                Assert.AreEqual(name == "HRESULT" ? Foundation : Cryptography,
                    reader.GetString(NativeTypedefHeaderTests.Type(reader, name).Namespace));
                var attributes = NativeTypedefHeaderTests.Type(reader, name).GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                Assert.AreEqual(legacy && name != "HRESULT", attributes.Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) == "RAIIFreeAttribute"));
                Assert.AreEqual(legacy && name != "HRESULT", attributes.Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) == "InvalidHandleValueAttribute"));
                if (legacy && name != "HRESULT")
                {
                    var cleanup = reader.GetBlobReader(attributes.Single(attribute =>
                        ParameterHeaderTests.AttributeName(reader, attribute) == "RAIIFreeAttribute").Value);
                    Assert.AreEqual((ushort)1, cleanup.ReadUInt16());
                    Assert.AreEqual("NCryptFreeObject", cleanup.ReadSerializedString());
                    Assert.AreEqual((ushort)0, cleanup.ReadUInt16());
                    Assert.AreEqual(0, cleanup.RemainingBytes);
                    var invalid = reader.GetBlobReader(attributes.Single(attribute =>
                        ParameterHeaderTests.AttributeName(reader, attribute) == "InvalidHandleValueAttribute").Value);
                    Assert.AreEqual((ushort)1, invalid.ReadUInt16());
                    Assert.AreEqual(0L, invalid.ReadInt64());
                    Assert.AreEqual((ushort)0, invalid.ReadUInt16());
                    Assert.AreEqual(0, invalid.RemainingBytes);
                }
            }
            Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute).Any(attribute =>
                ParameterHeaderTests.AttributeName(reader, attribute) is "SupportedOSPlatformAttribute" or "NativeAnnotationAttribute" or "CppAttributeList"));
        }

        private static string Sdk(string path) => Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
            "generation", "WinSDK", path.Replace('/', Path.DirectorySeparatorChar));
        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";
    }
}
