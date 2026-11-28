using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class StageOneEnumNativeSdkTests
    {
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void NativeAssociationOwners_PreserveByteGroupsDwordStorageAndReturnFacts(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = Declarations();
            var result = fixture.Scrape(Regex.Replace(source, @"_Win32_AssociatedEnum_\(\w+\)\s*", ""),
                target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(Facts()));
            result = fixture.Scrape(source, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var methods = NativeTypedefHeaderTests.Type(reader, "Apis").GetMethods().Select(reader.GetMethodDefinition)
                .ToDictionary(method => reader.GetString(method.Name));
            foreach (var fact in Facts())
            {
                string[] selector = fact.Key.Split("::");
                var method = methods[selector[0]];
                var parameter = method.GetParameters().Select(reader.GetParameter)
                    .Single(parameter => selector[1] == "return" ? parameter.SequenceNumber == 0 :
                        reader.GetString(parameter.Name) == selector[1]);
                CollectionAssert.AreEqual(new[] { fact.Value.Split('"')[1] },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"),
                    fact.Key);
            }
            foreach (string name in new[] { "FONT_OUTPUT_PRECISION", "FONT_QUALITY" })
            {
                var backing = NativeTypedefHeaderTests.Type(reader, name).GetFields().Select(reader.GetFieldDefinition)
                    .Single(field => reader.GetString(field.Name) == "value__");
                var signature = reader.GetBlobReader(backing.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(SignatureTypeCode.Byte, signature.ReadSignatureTypeCode(), name);
            }
            foreach (string name in new[] { "CreateFontA", "CreateFontW" })
            {
                var signature = reader.GetBlobReader(methods[name].Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(14, signature.ReadCompressedInteger());
                Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
                for (int sequence = 1; sequence <= 13; sequence++)
                    Assert.AreEqual(sequence <= 5 ? SignatureTypeCode.Int32 : SignatureTypeCode.UInt32,
                        signature.ReadSignatureTypeCode(), name + " parameter " + sequence);
            }
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), "association-" + target + "-" + language);
            Save(fixture.GeneratedSource, "association-" + target + "-" + language);
            Save(output, "association-" + target + "-" + language);
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void NativeAssociationOwners_AreNativeAbiNoOps(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language,
                "#define _In_\n#define _In_opt_\n#define _Out_writes_(count)\n" + Declarations() + @"
typedef HFONT (WINAPI *ExpectedFont)(int,int,int,int,int,DWORD,DWORD,DWORD,DWORD,DWORD,DWORD,DWORD,DWORD,LPCSTR);
typedef PCHAR (IMAGEAPI *ExpectedHome)(DWORD,PSTR,size_t);
typedef int (WINAPI *ExpectedMode)(HDC,int);
ExpectedFont fontStorage = &CreateFontA;
ExpectedHome homeStorage = &SymGetHomeDirectory;
ExpectedMode modeStorage = &SetBkMode;
typedef char VerifyNativeDword[(sizeof(DWORD) == 4 && sizeof(int) == 4) ? 1 : -1];
");
            Save(fixture.OrdinaryNativeObject, "association-ordinary-" + arch + "-" + language.TrimStart('/'));
            Save(fixture.MetadataNativeObject, "association-metadata-" + arch + "-" + language.TrimStart('/'));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeTaskDialogCallback_PreservesItsNamedAliasAndMessageStorage(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "CommCtrl.h"));
            string source = "#define CALLBACK __stdcall\n" +
                "typedef long HRESULT; typedef unsigned int UINT; typedef void* HWND;\n" +
                "#ifdef _WIN64\ntypedef unsigned long long WPARAM; typedef long long LPARAM; typedef long long LONG_PTR;\n" +
                "#else\ntypedef unsigned int WPARAM; typedef long LPARAM; typedef long LONG_PTR;\n#endif\n" +
                Slice(header, @"typedef enum _TASKDIALOG_NOTIFICATIONS\s*\{[^}]+\}\s*TASKDIALOG_NOTIFICATIONS;") + "\n" +
                Slice(header, @"typedef HRESULT \(CALLBACK \*PFTASKDIALOGCALLBACK\)\([^;]+;");
            var result = fixture.Scrape(Regex.Replace(source, @"_Win32_AssociatedEnum_\(TASKDIALOG_NOTIFICATIONS\)\s*", ""),
                target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(new Dictionary<string, string> {
                ["PFTASKDIALOGCALLBACK::msg"] = "[AssociatedEnum(\"TASKDIALOG_NOTIFICATIONS\")]",
            }));
            result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var invoke = NativeTypedefHeaderTests.Type(reader, "PFTASKDIALOGCALLBACK").GetMethods()
                .Select(reader.GetMethodDefinition).Single(method => reader.GetString(method.Name) == "Invoke");
            var message = invoke.GetParameters().Select(reader.GetParameter)
                .Single(parameter => reader.GetString(parameter.Name) == "msg");
            CollectionAssert.AreEqual(new[] { "TASKDIALOG_NOTIFICATIONS" },
                EnumAssociationHeaderTests.StringAttributes(reader, message.GetCustomAttributes(), "AssociatedEnumAttribute"));
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), "task-dialog-" + target);
            Save(fixture.GeneratedSource, "task-dialog-" + target);
            Save(output, "task-dialog-" + target);
        }

        [DataTestMethod]
        [DataRow("post-midl", "um", "CommCtrl.h")]
        [DataRow("post-midl", "um", "DbgHelp.h")]
        [DataRow("post-midl", "um", "ImageHlp.h")]
        [DataRow("post-midl", "um", "HtmlHelp.h")]
        [DataRow("post-midl", "um", "ShlObj_core.h")]
        [DataRow("post-midl", "um", "Snmp.h")]
        [DataRow("post-midl", "um", "Winldap.h")]
        [DataRow("post-midl", "um", "avrfsdk.h")]
        [DataRow("post-midl", "um", "azroles.h")]
        [DataRow("post-midl", "um", "dwmapi.h")]
        [DataRow("post-midl", "um", "wingdi.h")]
        [DataRow("post-midl", "um", "wpcapi.h")]
        [DataRow("pre-midl", "shared", "dxgi.idl")]
        [DataRow("pre-midl", "shared", "dxgi1_3.idl")]
        [DataRow("pre-midl", "shared", "dxgi1_4.idl")]
        [DataRow("pre-midl", "shared", "dxgi1_5.idl")]
        [DataRow("pre-midl", "shared", "dxgi1_6.idl")]
        [DataRow("pre-midl", "um", "ShObjIdl_core.idl")]
        public void AssociationOwningPatches_ReplayEntirePinnedSdkInput(string phase, string area, string owner)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string sdk = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string headers = Path.Combine(fixture.DirectoryPath, "headers");
            Directory.CreateDirectory(Path.Combine(headers, area));
            string output = Path.Combine(headers, area, owner);
            File.Copy(Path.Combine(sdk, area, owner), output);
            var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-File",
                Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "ApplySDKPatches.ps1"),
                "-Phase", phase, "-HeaderRoot", headers, "-SdkVersion", "10.0.28000.2705",
                "-MidlInputRoot", sdk, "-SelectedPatches", phase + "/" + owner + ".patch");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", area, owner)), File.ReadAllBytes(output));
            Save(output, "association-whole-owner-" + owner);
        }

        [TestMethod]
        public void AssociationGeneratedOwners_ReplayCompleteImportAndIncludeClosure()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string root = LastErrorHeaderTests.RepositoryRoot();
            string headers = Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders");
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string converted = Path.Combine(fixture.DirectoryPath, "converted");
            Directory.CreateDirectory(converted);
            File.Copy(Path.Combine(headers, "shared", "win32metadata_annotations.h"),
                Path.Combine(converted, "win32metadata_annotations.h"));
            string[] owners = { "shared/dxgi.idl", "shared/dxgi1_2.idl", "shared/dxgi1_3.idl",
                "shared/dxgi1_4.idl", "shared/dxgi1_5.idl", "shared/dxgi1_6.idl", "shared/dxgi1_7.idl",
                "um/ShObjIdl_core.idl", "um/ShObjIdl.idl" };
            var pending = new Queue<string>(owners);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string command = "";
            while (pending.Count > 0)
            {
                string relative = pending.Dequeue();
                if (!seen.Add(relative)) continue;
                string source = new[] { headers, include }.Select(directory => Path.Combine(directory, relative)).First(File.Exists);
                foreach (Match statement in Regex.Matches(File.ReadAllText(source),
                    @"(?m)^\s*(?:import\b[^;]*;|#\s*include\s*[<""][^>""]+\.idl[>""])"))
                    foreach (Match dependency in Regex.Matches(statement.Value, @"[<""]([^>""]+\.idl)[>""]"))
                    {
                        string name = dependency.Groups[1].Value;
                        string path = new[] { "um", "shared", "winrt" }.Select(area => Path.Combine(area, name))
                            .First(path => File.Exists(Path.Combine(headers, path)) || File.Exists(Path.Combine(include, path)));
                        pending.Enqueue(path);
                    }
                command += "& " + Quote(Path.Combine(root, "scripts", "ConvertMidlAttributesToSalAnnotations.ps1")) +
                    " " + Quote(source) + " " + Quote(Path.Combine(converted, Path.GetFileName(relative).ToLowerInvariant())) + "; ";
            }
            command += ". " + Quote(Path.Combine(root, "scripts", "CommonUtils.ps1")) + "; Install-VsDevShell; " +
                "$env:INCLUDE = " + Quote(string.Join(";", new[] { converted, Path.Combine(headers, "um"),
                    Path.Combine(headers, "shared"), Path.Combine(headers, "winrt"), Path.Combine(root, "scripts", "inc") })) +
                " + ';' + $env:INCLUDE; ";
            foreach (string owner in owners)
            {
                string output = Path.Combine(converted, Path.GetFileName(Path.ChangeExtension(owner, ".h")).ToLowerInvariant());
                command += "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, Path.GetFileName(owner).ToLowerInvariant())) + " /out " + Quote(converted) +
                    " /header " + Quote(output) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(headers, "um")) +
                    " /I " + Quote(Path.Combine(headers, "shared")) + " /I " + Quote(Path.Combine(headers, "winrt")) +
                    " /I " + Quote(Path.Combine(root, "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            }
            string script = Path.Combine(fixture.DirectoryPath, "replay.ps1");
            File.WriteAllText(script, "$ErrorActionPreference = 'Stop'; " + command);
            var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-File", script);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Assert.IsFalse(Regex.IsMatch(result.Output, @"(?i)Processing .*Windows Kits\\10"), result.Output);
            foreach (string owner in owners)
            {
                string relative = Path.ChangeExtension(owner, ".h");
                string output = Path.Combine(converted, Path.GetFileName(relative).ToLowerInvariant());
                Save(output, "association-regenerated-" + Path.GetFileName(owner));
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(headers, relative)), File.ReadAllBytes(output), owner);
            }
        }

        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";

        private static Dictionary<string, string> Facts()
        {
            var facts = new Dictionary<string, string> {
                ["GetBkMode::return"] = "[AssociatedEnum(\"BACKGROUND_MODE\")]",
                ["SetBkMode::mode"] = "[AssociatedEnum(\"BACKGROUND_MODE\")]",
            };
            foreach (string suffix in new[] { "A", "W" })
            {
                facts.Add("CreateFont" + suffix + "::iOutPrecision", "[AssociatedEnum(\"FONT_OUTPUT_PRECISION\")]");
                facts.Add("CreateFont" + suffix + "::iQuality", "[AssociatedEnum(\"FONT_QUALITY\")]");
            }
            foreach (string suffix in new[] { "", "W" })
            {
                facts.Add("SymGetHomeDirectory" + suffix + "::type", "[AssociatedEnum(\"IMAGEHLP_HD_TYPE\")]");
                facts.Add("SymGetSymbolFile" + suffix + "::Type", "[AssociatedEnum(\"IMAGEHLP_SF_TYPE\")]");
            }
            return facts;
        }

        private static string Declarations()
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string gdi = File.ReadAllText(Path.Combine(headers, "wingdi.h"));
            string debug = File.ReadAllText(Path.Combine(headers, "ImageHlp.h"));
            string source = "#define WINGDIAPI\n#define WINAPI __stdcall\n#define IMAGEAPI __stdcall\n" +
                "typedef unsigned long DWORD; typedef int BOOL;\n" +
                "typedef void* HANDLE; typedef void* HFONT; typedef void* HDC;\n" +
                "typedef char CHAR; typedef CHAR* PCHAR; typedef CHAR* PSTR; typedef const CHAR* PCSTR; typedef const CHAR* LPCSTR;\n" +
                "typedef unsigned short WCHAR; typedef WCHAR* PWSTR; typedef const WCHAR* PCWSTR; typedef const WCHAR* LPCWSTR;\n" +
                "#ifdef _WIN64\ntypedef unsigned long long size_t;\n#else\ntypedef unsigned int size_t;\n#endif\n";
            foreach (string name in new[] { "FONT_OUTPUT_PRECISION", "FONT_QUALITY", "BACKGROUND_MODE" })
            {
                string group = Slice(gdi, @"_Win32_Enum_\(" + name + @",[^;]+;");
                foreach (string constant in group[(group.IndexOf(',') + 1)..].Split(',').Skip(1))
                {
                    string symbol = constant.Trim().TrimEnd(';', ')');
                    source += Slice(gdi, @"(?m)^#define\s+" + symbol + @"\s+[^\r\n]+") + "\n";
                }
                source += group + "\n";
            }
            foreach (string name in new[] { "IMAGEHLP_HD_TYPE", "IMAGEHLP_SF_TYPE" })
                source += Slice(debug, @"typedef enum\s*\{[^}]+\}\s*" + name + ";") + "\n";
            foreach (string name in new[] { "CreateFontA", "CreateFontW", "SetBkMode" })
                source += Slice(gdi, @"WINGDIAPI\s+(?:HFONT|int)\s+WINAPI\s+" + name + @"\s*\([^;]+;") + "\n";
            source += Slice(gdi, @"_Win32_AssociatedEnum_\(BACKGROUND_MODE\)\s+WINGDIAPI\s+int\s+WINAPI\s+GetBkMode\s*\([^;]+;") + "\n";
            foreach (string name in new[] { "SymGetHomeDirectory", "SymGetHomeDirectoryW", "SymGetSymbolFile", "SymGetSymbolFileW" })
                source += Slice(debug, @"(?:PCHAR|PWSTR|BOOL)\s+IMAGEAPI\s+" + name + @"\s*\([^;]+;") + "\n";
            return source;
        }

        private static string Slice(string source, string pattern)
        {
            var matches = Regex.Matches(source, pattern);
            Assert.AreEqual(1, matches.Count, pattern);
            return matches[0].Value;
        }

        private void Save(string path, string name)
        {
            string destination = Path.Combine(TestContext.TestResultsDirectory!, name + "-" + Path.GetFileName(path));
            File.Copy(path, destination, overwrite: true);
            TestContext.AddResultFile(destination);
        }
    }
}
