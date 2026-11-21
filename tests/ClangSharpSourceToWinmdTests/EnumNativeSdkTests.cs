using System;
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
    public class EnumNativeSdkTests
    {
        private const string Start = "_Win32_AssociatedConstant_(SERVICE_NO_CHANGE)\n" +
            "_Win32_Enum_(SERVICE_START_TYPE, unsigned long, SERVICE_BOOT_START, SERVICE_SYSTEM_START, " +
            "SERVICE_AUTO_START, SERVICE_DEMAND_START, SERVICE_DISABLED);\n";
        private const string Error = "_Win32_AssociatedConstant_(SERVICE_NO_CHANGE)\n" +
            "_Win32_Enum_(SERVICE_ERROR, unsigned long, SERVICE_ERROR_IGNORE, SERVICE_ERROR_NORMAL, SERVICE_ERROR_SEVERE, SERVICE_ERROR_CRITICAL);\n";
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void InstallerParameters_PreserveNativeStorageAndIndividualLegacyFacts(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = InstallerDeclarations();
            string legacySource = Regex.Replace(source,
                @"_Win32_AssociatedEnum_\((INSTALLMODE|INSTALLLOGMODE|MSIINSTALLCONTEXT|REINSTALLMODE)\)\s*", "");
            legacySource = Regex.Replace(legacySource, @"UINT WINAPI MsiDatabaseGenerateTransform[AW]\([^;]+;",
                match => match.Value.Replace("_Reserved_ ", "", StringComparison.Ordinal));
            var result = fixture.Scrape(legacySource, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(InstallerFacts()));
            result = fixture.Scrape(source, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var retainedReserved = InstallerFacts().Where(fact => fact.Value == "[Reserved]")
                .ToDictionary(fact => fact.Key, fact => fact.Value);
            string path = fixture.Emit(retainedReserved);
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                var methods = NativeTypedefHeaderTests.Type(reader, "Apis").GetMethods().Select(reader.GetMethodDefinition)
                    .ToDictionary(method => reader.GetString(method.Name));
                foreach (var fact in InstallerFacts())
                {
                    string[] selector = fact.Key.Split("::");
                    var parameter = methods[selector[0]].GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == selector[1]);
                    string[] associations = EnumAssociationHeaderTests.StringAttributes(reader,
                        parameter.GetCustomAttributes(), "AssociatedEnumAttribute");
                    if (fact.Value == "[Reserved]")
                    {
                        Assert.AreEqual(0, associations.Length);
                        Assert.AreEqual(1, parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                            .Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ReservedAttribute"));
                    }
                    else
                        CollectionAssert.AreEqual(new[] { fact.Value.Split('"')[1] }, associations, fact.Key);
                }
                foreach (string name in new[] { "MsiProvideAssemblyA", "MsiProvideAssemblyW" })
                {
                    var neighbor = methods[name].GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == "dwAssemblyInfo");
                    CollectionAssert.AreEqual(new[] { "MSIASSEMBLYINFO" },
                        EnumAssociationHeaderTests.StringAttributes(reader, neighbor.GetCustomAttributes(), "AssociatedEnumAttribute"));
                }
            }
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(path),
                "Sixteen native enum transfers plus four retained reserved-scalar sidecars must preserve every declaration and attribute.");
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), "installer-parameters-" + target + "-" + language);
            Save(fixture.GeneratedSource, "installer-parameters-" + target + "-" + language);
            Save(path, "installer-parameters-" + target + "-" + language);
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void InstallerParameters_AreNativeAbiNoOps(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language,
                "#define _In_\n#define _In_opt_\n#define _Out_opt_\n#define _Inout_opt_\n" +
                "#define _Out_writes_opt_(count)\n#define _Reserved_\n" + InstallerDeclarations() + @"
typedef UINT (WINAPI *ExpectedReinstall)(LPCSTR, DWORD);
typedef UINT (WINAPI *ExpectedTransform)(MSIHANDLE, MSIHANDLE, LPCWSTR, int, int);
ExpectedReinstall verifyReinstall = &MsiReinstallProductA;
ExpectedTransform verifyTransform = &MsiDatabaseGenerateTransformW;
typedef char VerifyScalarStorage[(sizeof(DWORD) == 4 && sizeof(int) == 4) ? 1 : -1];
");
            Save(fixture.OrdinaryNativeObject, "installer-ordinary-" + arch + "-" + language.TrimStart('/'));
            Save(fixture.MetadataNativeObject, "installer-metadata-" + arch + "-" + language.TrimStart('/'));
        }

        [TestMethod]
        public void InstallerOwningPatches_ReplayBothEntireShippedHeaders()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string headers = Path.Combine(fixture.DirectoryPath, "headers");
            Directory.CreateDirectory(Path.Combine(headers, "um"));
            foreach (string name in new[] { "Msi.h", "MsiQuery.h" })
            {
                string output = Path.Combine(headers, "um", name);
                File.Copy(Sdk("um", name), output);
                var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-File",
                    Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "ApplySDKPatches.ps1"),
                    "-Phase", "post-midl", "-HeaderRoot", headers, "-SdkVersion", "10.0.28000.2705",
                    "-MidlInputRoot", Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0"),
                    "-SelectedPatches", "post-midl/" + name + ".patch");
                Assert.AreEqual(0, result.ExitCode, result.Output);
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                    "generation", "WinSDK", "RecompiledIdlHeaders", "um", name)), File.ReadAllBytes(output));
                Save(output, "installer-whole-owner-" + name);
            }
        }

        private static Dictionary<string, string> InstallerFacts()
        {
            var facts = new Dictionary<string, string>();
            foreach (string suffix in new[] { "A", "W" })
            {
                facts.Add("MsiEnableLog" + suffix + "::dwLogMode", "[AssociatedEnum(\"INSTALLLOGMODE\")]");
                facts.Add("MsiEnumClientsEx" + suffix + "::dwContext", "[AssociatedEnum(\"MSIINSTALLCONTEXT\")]");
                foreach (string name in new[] { "MsiProvideAssembly", "MsiProvideComponent",
                    "MsiProvideQualifiedComponent", "MsiProvideQualifiedComponentEx" })
                    facts.Add(name + suffix + "::dwInstallMode", "[AssociatedEnum(\"INSTALLMODE\")]");
                facts.Add("MsiReinstallFeature" + suffix + "::dwReinstallMode", "[AssociatedEnum(\"REINSTALLMODE\")]");
                facts.Add("MsiReinstallProduct" + suffix + "::szReinstallMode", "[AssociatedEnum(\"REINSTALLMODE\")]");
                foreach (string name in new[] { "iReserved1", "iReserved2" })
                    facts.Add("MsiDatabaseGenerateTransform" + suffix + "::" + name, "[Reserved]");
            }
            return facts;
        }

        private static string InstallerDeclarations()
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string msi = File.ReadAllText(Path.Combine(headers, "Msi.h"));
            string query = File.ReadAllText(Path.Combine(headers, "MsiQuery.h"));
            string source = "#define WINAPI __stdcall\n#define _WIN32_MSI 500\n#define MAX_GUID_CHARS 38\n" +
                "typedef unsigned int UINT; typedef unsigned long DWORD; typedef DWORD* LPDWORD;\n" +
                "typedef char CHAR; typedef const CHAR* LPCSTR; typedef CHAR* LPSTR;\n" +
                "#ifdef __cplusplus\ntypedef wchar_t WCHAR;\n#else\ntypedef unsigned short WCHAR;\n#endif\n" +
                "typedef const WCHAR* LPCWSTR; typedef WCHAR* LPWSTR; typedef unsigned long MSIHANDLE;\n";
            foreach (string name in new[] { "INSTALLMESSAGE", "REINSTALLMODE", "INSTALLLOGMODE", "INSTALLMODE", "MSIINSTALLCONTEXT" })
                source += Slice(msi, @"typedef enum [^{}]+\{[^}]+\}\s*" + name + ";") + "\n";
            foreach (string name in new[] { "MSIASSEMBLYINFO_NETASSEMBLY", "MSIASSEMBLYINFO_WIN32ASSEMBLY" })
                source += Slice(msi, @"(?m)^#define " + name + @"[^\r\n]+") + "\n";
            source += Slice(msi, @"_Win32_Enum_\(MSIASSEMBLYINFO,[^;]+;") + "\n";
            foreach (string name in InstallerFacts().Keys.Select(selector => selector.Split("::")[0]).Distinct())
                source += Slice(name.StartsWith("MsiDatabase", StringComparison.Ordinal) ? query : msi,
                    @"UINT WINAPI " + name + @"\s*\([^;]+;") + "\n";
            return source;
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void ThemePropertyParameters_PreserveSignedStorageAndIndividualLegacyFacts(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = ThemePropertyDeclarations();
            string legacySource = Regex.Replace(source, @"_Win32_AssociatedEnum_\(THEME_PROPERTY_SYMBOL_ID\)\s*", "");
            var result = fixture.Scrape(legacySource, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(ThemePropertyFacts()));
            result = fixture.Scrape(source, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit(new Dictionary<string, string>
            {
                ["GetThemeSysColorBrush::iColorId"] = "[AssociatedEnum(\"THEME_PROPERTY_SYMBOL_ID\")]",
            });
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                var methods = NativeTypedefHeaderTests.Type(reader, "Apis").GetMethods().Select(reader.GetMethodDefinition)
                    .ToDictionary(method => reader.GetString(method.Name));
                foreach (var fact in ThemePropertyFacts())
                {
                    string[] selector = fact.Key.Split("::");
                    var method = methods[selector[0]];
                    var parameter = method.GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == selector[1]);
                    CollectionAssert.AreEqual(new[] { "THEME_PROPERTY_SYMBOL_ID" },
                        EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"),
                        fact.Key);
                    var signature = reader.GetBlobReader(method.Signature);
                    signature.ReadSignatureHeader();
                    Assert.IsTrue(signature.ReadCompressedInteger() >= parameter.SequenceNumber);
                    SkipThemeFixtureType(ref signature);
                    for (int sequence = 1; sequence < parameter.SequenceNumber; sequence++)
                        SkipThemeFixtureType(ref signature);
                    Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode(), fact.Key);
                }
                var group = NativeTypedefHeaderTests.Type(reader, "THEME_PROPERTY_SYMBOL_ID");
                var backing = group.GetFields().Select(reader.GetFieldDefinition)
                    .Single(field => reader.GetString(field.Name) == "value__");
                var storage = reader.GetBlobReader(backing.Signature);
                storage.ReadSignatureHeader();
                Assert.AreEqual(SignatureTypeCode.UInt32, storage.ReadSignatureTypeCode());
                var neighbor = methods["GetThemeBitmap"].GetParameters().Select(reader.GetParameter)
                    .Single(parameter => reader.GetString(parameter.Name) == "dwFlags");
                CollectionAssert.AreEqual(new[] { "GET_THEME_BITMAP_FLAGS" },
                    EnumAssociationHeaderTests.StringAttributes(reader, neighbor.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(path),
                "Fourteen native property associations and the retained system-color fact must preserve every declaration and attribute.");
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), "theme-properties-" + target + "-" + language);
            Save(fixture.GeneratedSource, "theme-properties-" + target + "-" + language);
            Save(path, "theme-properties-" + target + "-" + language);
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void ThemePropertyParameters_AreNativeAbiNoOps(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language,
                "#define _In_\n#define _In_opt_\n#define _Out_\n#define _Out_writes_(count)\n" +
                ThemePropertyDeclarations() + @"
typedef HRESULT (__stdcall *ExpectedThemeInt)(HTHEME, int, int, int, int*);
typedef HRESULT (__stdcall *ExpectedThemeSysInt)(HTHEME, int, int*);
ExpectedThemeInt verifyThemeInt = &GetThemeInt;
ExpectedThemeSysInt verifyThemeSysInt = &GetThemeSysInt;
typedef char VerifySignedInt[(sizeof(int) == 4 && (int)-1 < 0) ? 1 : -1];
");
            Save(fixture.OrdinaryNativeObject, "theme-properties-ordinary-" + arch + "-" + language.TrimStart('/'));
            Save(fixture.MetadataNativeObject, "theme-properties-metadata-" + arch + "-" + language.TrimStart('/'));
        }

        [TestMethod]
        public void ThemePropertyOwningPatches_ReplayBothEntireShippedHeaders()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string headers = Path.Combine(fixture.DirectoryPath, "headers");
            Directory.CreateDirectory(Path.Combine(headers, "um"));
            foreach (string name in new[] { "Uxtheme.h", "vssym32.h" })
            {
                string output = Path.Combine(headers, "um", name);
                File.Copy(Sdk("um", name), output);
                var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-File",
                    Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "scripts", "ApplySDKPatches.ps1"),
                    "-Phase", "post-midl", "-HeaderRoot", headers, "-SdkVersion", "10.0.28000.2705",
                    "-MidlInputRoot", Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0"),
                    "-SelectedPatches", "post-midl/" + name + ".patch");
                Assert.AreEqual(0, result.ExitCode, result.Output);
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                    "generation", "WinSDK", "RecompiledIdlHeaders", "um", name)), File.ReadAllBytes(output));
                Save(output, "theme-properties-whole-owner-" + name);
            }
        }

        private static Dictionary<string, string> ThemePropertyFacts()
        {
            var facts = new Dictionary<string, string>();
            foreach (string name in new[] { "GetThemeBitmap", "GetThemeBool", "GetThemeColor", "GetThemeEnumValue",
                "GetThemeFilename", "GetThemeInt", "GetThemeIntList", "GetThemeMargins", "GetThemeMetric", "GetThemePosition" })
                facts.Add(name + "::iPropId", "[AssociatedEnum(\"THEME_PROPERTY_SYMBOL_ID\")]");
            foreach (var selector in new[] { ("GetThemeSysBool", "iBoolId"), ("GetThemeSysColorBrush", "iColorId"),
                ("GetThemeSysFont", "iFontId"), ("GetThemeSysInt", "iIntId"), ("GetThemeSysString", "iStringId") })
                facts.Add(selector.Item1 + "::" + selector.Item2, "[AssociatedEnum(\"THEME_PROPERTY_SYMBOL_ID\")]");
            return facts;
        }

        private static string ThemePropertyDeclarations()
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string theme = File.ReadAllText(Path.Combine(headers, "Uxtheme.h"));
            string symbols = File.ReadAllText(Path.Combine(headers, "vssym32.h"));
            string windef = File.ReadAllText(Sdk("shared", "windef.h"));
            string wingdi = File.ReadAllText(Sdk("um", "wingdi.h"));
            string source = "#define THEMEAPI HRESULT __stdcall\n#define THEMEAPI_(type) type __stdcall\n" +
                "#define FAR\n#define NEAR\n" +
                "typedef long HRESULT; typedef long LONG; typedef int BOOL; typedef unsigned char BYTE;\n" +
                "typedef unsigned long DWORD; typedef unsigned long ULONG; typedef unsigned long COLORREF;\n" +
                "typedef void* HTHEME; typedef void* HDC; typedef void* HBRUSH; typedef void* HBITMAP;\n" +
                "#ifdef __cplusplus\ntypedef wchar_t WCHAR;\n#else\ntypedef unsigned short WCHAR;\n#endif\n" +
                "typedef WCHAR* LPWSTR;\n" +
                Slice(windef, @"typedef struct tagPOINT\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(windef, @"typedef struct tagRECT\s*\{[^}]+\}[^;]+;") + "\ntypedef const RECT* LPCRECT;\n" +
                Slice(wingdi, @"(?m)^#define LF_FACESIZE[^\r\n]+") + "\n" +
                Slice(wingdi, @"typedef struct tagLOGFONTW\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(theme, @"typedef struct _MARGINS\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(theme, @"(?m)^#define MAX_INTLIST_COUNT[^\r\n]+") + "\n" +
                Slice(theme, @"typedef struct _INTLIST\s*\{[^}]+\}[^;]+;") + "\n" +
                string.Join("\n", Regex.Matches(symbols, @"(?m)^#define TMT_\w+[^\r\n]+").Select(match => match.Value)) + "\n" +
                Slice(symbols, @"_Win32_Enum_\(THEME_PROPERTY_SYMBOL_ID,[^;]+;") + "\n" +
                Slice(theme, @"(?m)^#define GBF_DIRECT[\s\S]+?GBF_COPY\)") + "\n" +
                Slice(theme, @"_Win32_Enum_\(GET_THEME_BITMAP_FLAGS,[^;]+;") + "\n";
            foreach (string name in ThemePropertyFacts().Keys.Select(selector => selector.Split("::")[0]))
                source += Slice(theme, @"THEMEAPI(?:_\([^)]*\))?\s+" + name + @"\s*\([^;]+;") + "\n";
            return source;
        }

        private static void SkipThemeFixtureType(ref BlobReader signature)
        {
            SignatureTypeCode code;
            do code = signature.ReadSignatureTypeCode();
            while (code == SignatureTypeCode.Pointer);
            if (code == SignatureTypeCode.TypeHandle) signature.ReadTypeHandle();
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void CommonDialogConsumer_ResolvesItsNativeErrorDefinition(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders");
            string dialogs = File.ReadAllText(Path.Combine(headers, "um", "commdlg.h"));
            string declarations = "#define WINCOMMDLGAPI\n#define APIENTRY __stdcall\n#define VOID void\n" +
                "typedef unsigned long DWORD;\n" + Slice(dialogs, @"\A[\s\S]*?(?=/\*)") +
                Slice(dialogs, @"_Win32_AssociatedEnum_\(COMMON_DLG_ERRORS\)\s+WINCOMMDLGAPI DWORD APIENTRY CommDlgExtendedError\([^;]+;");
            var result = fixture.Scrape(declarations, target, sal: true,
                extraSettings: "--traverse\n" + Path.Combine(fixture.DirectoryPath, "cderr.h"),
                headers: new Dictionary<string, string>
                {
                    ["winapifamily.h"] = "",
                    ["cderr.h"] = File.ReadAllText(Path.Combine(headers, "shared", "cderr.h")),
                });
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), "commdlg-consumer-" + target + ".h");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            Save(path, "commdlg-consumer-" + target + ".winmd");
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "CommDlgExtendedError");
            var parameter = method.GetParameters().Select(reader.GetParameter)
                .Single(parameter => parameter.SequenceNumber == 0);
            CollectionAssert.AreEqual(new[] { "COMMON_DLG_ERRORS" },
                EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
            var type = NativeTypedefHeaderTests.Type(reader, "COMMON_DLG_ERRORS");
            EnumAssociationHeaderTests.AssertSignature(reader,
                type.GetFields().Select(reader.GetFieldDefinition)
                    .Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ThemeAndDialogGroups_PreserveMembersAndNativeIntegerUses(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(ThemeAndDialogDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            Save(path, "theme-dialog-groups-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            string errors = File.ReadAllText(Sdk("shared", "cderr.h"));
            var expectedErrors = Regex.Matches(errors, @"(?m)^#define (\w+)\s+(0x[0-9A-Fa-f]+)")
                .Cast<Match>().ToDictionary(match => match.Groups[1].Value,
                    match => Convert.ToUInt32(match.Groups[2].Value, 16));
            Assert.AreEqual(37, expectedErrors.Count);
            var expectedTheme = new Dictionary<string, uint>
            {
                ["ALLOW_NONCLIENT"] = 1, ["ALLOW_CONTROLS"] = 2,
                ["ALLOW_WEBCONTENT"] = 4, ["VALIDBITS"] = 7,
            };
            foreach (var group in new[] { ("COMMON_DLG_ERRORS", expectedErrors), ("SET_THEME_APP_PROPERTIES_FLAGS", expectedTheme) })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Item1);
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    fields.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
                CollectionAssert.AreEqual(group.Item2.Keys.ToArray(),
                    fields.Where(field => !field.GetDefaultValue().IsNil).Select(field => reader.GetString(field.Name)).ToArray());
                foreach (var field in fields.Where(field => !field.GetDefaultValue().IsNil))
                    Assert.AreEqual(group.Item2[reader.GetString(field.Name)],
                        reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32());
                Assert.AreEqual(group.Item1 == "SET_THEME_APP_PROPERTIES_FLAGS" ? 1 : 0,
                    type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                        .Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "FlagsAttribute"));
                Assert.AreEqual(0, type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ScopedEnumAttribute"));
            }
            foreach (string name in new[] { "CommDlgExtendedError", "GetThemeAppProperties", "SetThemeAppProperties" })
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == name);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(name == "SetThemeAppProperties" ? 1 : 0, signature.ReadCompressedInteger());
                Assert.AreEqual(name == "SetThemeAppProperties" ? SignatureTypeCode.Void : SignatureTypeCode.UInt32,
                    signature.ReadSignatureTypeCode());
                if (name == "SetThemeAppProperties") Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
                Assert.AreEqual(0, signature.RemainingBytes);
                var parameter = method.GetParameters().Select(reader.GetParameter)
                    .Single(parameter => parameter.SequenceNumber == (name == "SetThemeAppProperties" ? 1 : 0));
                CollectionAssert.AreEqual(new[] { name == "CommDlgExtendedError" ? "COMMON_DLG_ERRORS" : "SET_THEME_APP_PROPERTIES_FLAGS" },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Where(type => reader.GetString(type.Name) == "Apis").SelectMany(type => type.GetFields())
                .Select(reader.GetFieldDefinition).Any(field => expectedTheme.ContainsKey(reader.GetString(field.Name))));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void ThemeAndDialogGroups_PreserveNativeCAndCppAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language, "#define _In_\n" +
                ThemeAndDialogDeclarations().Replace("#include <win32metadata_annotations.h>",
                    "#include \"win32metadata_annotations.h\"", StringComparison.Ordinal) + @"
typedef DWORD (__stdcall *Getter)(void);
typedef void (__stdcall *Setter)(DWORD);
Getter themeGetter = &GetThemeAppProperties;
Getter dialogGetter = &CommDlgExtendedError;
Setter themeSetter = &SetThemeAppProperties;
typedef char VerifyThemeBits[(STAP_VALIDBITS == 7) ? 1 : -1];
typedef char VerifyDialogError[(FNERR_BUFFERTOOSMALL == 0x3003) ? 1 : -1];
");
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "theme-dialog-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void WinBioGroups_PreserveTypedefStorageMembersAndOutputPointers(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(WinBioDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "winbio-generated-" + target);
            string path = fixture.Emit();
            Save(path, "winbio-groups-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var group in new[]
            {
                ("WINBIO_POOL", new[] { "WINBIO_POOL_SYSTEM", "WINBIO_POOL_PRIVATE" }, new uint[] { 1, 2 }),
                ("WINBIO_COMPONENT", new[] { "WINBIO_COMPONENT_SENSOR", "WINBIO_COMPONENT_ENGINE", "WINBIO_COMPONENT_STORAGE" }, new uint[] { 1, 2, 3 }),
                ("WINBIO_SETTING_SOURCE", new[] { "WINBIO_SETTING_SOURCE_INVALID", "WINBIO_SETTING_SOURCE_DEFAULT", "WINBIO_SETTING_SOURCE_LOCAL", "WINBIO_SETTING_SOURCE_POLICY" }, new uint[] { 0, 1, 3, 2 }),
            })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Item1);
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    fields.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
                fields = fields.Where(field => !field.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(group.Item2, fields.Select(field => reader.GetString(field.Name)).ToArray());
                CollectionAssert.AreEqual(group.Item3, fields.Select(field =>
                    reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(attribute => new[] { "FlagsAttribute", "ScopedEnumAttribute" }.Contains(ParameterHeaderTests.AttributeName(reader, attribute))));
            }
            foreach (var method in reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Where(method => reader.GetString(method.Name).StartsWith("WinBio", StringComparison.Ordinal) ||
                    reader.GetString(method.Name).StartsWith("NativeSlot_", StringComparison.Ordinal)))
            {
                string name = reader.GetString(method.Name);
                bool output = name.StartsWith("WinBioGet", StringComparison.Ordinal);
                string group = output ? "WINBIO_SETTING_SOURCE" : name.Contains("ControlUnit", StringComparison.Ordinal) ? "WINBIO_COMPONENT" : "WINBIO_POOL";
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(output ? 2 : 1, signature.ReadCompressedInteger());
                Assert.AreEqual(output ? SignatureTypeCode.Void : SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
                if (output)
                {
                    Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                    Assert.AreEqual(SignatureTypeCode.Byte, signature.ReadSignatureTypeCode());
                    Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                }
                Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode(), name);
                Assert.AreEqual(0, signature.RemainingBytes);
                var parameter = method.GetParameters().Select(reader.GetParameter)
                    .Single(parameter => parameter.SequenceNumber == (output ? 2 : 1));
                CollectionAssert.AreEqual(new[] { group },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
                Assert.AreEqual(output ? ParameterAttributes.Out : ParameterAttributes.In,
                    parameter.Attributes & (ParameterAttributes.In | ParameterAttributes.Out));
            }
            Assert.AreEqual(7, reader.MethodDefinitions.Select(reader.GetMethodDefinition).Count(method =>
                reader.GetString(method.Name).StartsWith("WinBioGet", StringComparison.Ordinal) ||
                reader.GetString(method.Name).StartsWith("NativeSlot_", StringComparison.Ordinal)));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void WinBioGroups_PreserveNativeTypedefCAndCppAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language, "#define _In_\n#define _Out_\n" + WinBioDeclarations() + @"
typedef VOID (__stdcall *SettingGetter)(BOOLEAN*, PWINBIO_SETTING_SOURCE_TYPE);
SettingGetter enabled = &WinBioGetEnabledSetting;
SettingGetter logon = &WinBioGetLogonSetting;
SettingGetter domain = &WinBioGetDomainLogonSetting;
typedef int (__stdcall *PoolInput)(WINBIO_POOL_TYPE);
typedef int (__stdcall *ComponentInput)(WINBIO_COMPONENT);
PoolInput pool = &NativeSlot_WinBioOpenSession;
PoolInput asyncPool = &NativeSlot_WinBioAsyncOpenSession;
ComponentInput component = &NativeSlot_WinBioControlUnit;
ComponentInput privileged = &NativeSlot_WinBioControlUnitPrivileged;
typedef char VerifyWidths[(sizeof(WINBIO_COMPONENT) == 4 && sizeof(WINBIO_POOL_TYPE) == 4 && sizeof(WINBIO_SETTING_SOURCE_TYPE) == 4) ? 1 : -1];
typedef char VerifyReservedPoolControls[(WINBIO_POOL_UNKNOWN == 0 && WINBIO_POOL_UNASSIGNED == 3) ? 1 : -1];
");
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "winbio-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string WinBioDeclarations()
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders");
            string types = File.ReadAllText(Path.Combine(headers, "shared", "winbio_types.h"));
            string api = File.ReadAllText(Path.Combine(headers, "um", "WinBio.h"));
            string source = "#define VOID void\n#define WINAPI __stdcall\ntypedef unsigned long ULONG, ULONG32;\ntypedef unsigned char BOOLEAN;\n";
            foreach (string name in new[] { "WINBIO_POOL_TYPE", "WINBIO_COMPONENT", "WINBIO_SETTING_SOURCE_TYPE" })
                source += Slice(types, @"typedef \w+ " + name + @"[^\r\n]+;[\s\S]+?_Win32_Enum_\([^;]+;") + "\n";
            foreach (string method in new[] { "WinBioGetEnabledSetting", "WinBioGetLogonSetting", "WinBioGetDomainLogonSetting" })
                source += Slice(api, @"VOID WINAPI\s+" + method + @"\([^;]+;") + "\n";
            foreach (var item in new[]
            {
                ("WinBioOpenSession", "PoolType"), ("WinBioAsyncOpenSession", "PoolType"),
                ("WinBioControlUnit", "Component"), ("WinBioControlUnitPrivileged", "Component"),
            })
            {
                string declaration = Slice(api, @"HRESULT WINAPI\s+" + item.Item1 + @"\([^;]+;");
                string slot = Slice(declaration, @"_In_ _Win32_AssociatedEnum_\(\w+\) \w+ " + item.Item2);
                source += "int __stdcall NativeSlot_" + item.Item1 + "(" + slot + ");\n";
            }
            return source;
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "DbgHelp.h")]
        [DataRow("x86_64-pc-windows-msvc", "DbgHelp.h")]
        [DataRow("aarch64-pc-windows-msvc", "DbgHelp.h")]
        [DataRow("i686-pc-windows-msvc", "ImageHlp.h")]
        [DataRow("x86_64-pc-windows-msvc", "ImageHlp.h")]
        [DataRow("aarch64-pc-windows-msvc", "ImageHlp.h")]
        public void DebugLoadAndWaitGroups_PreserveNativeFlagsAndDuplicateHeaderDefinitions(string target, string header)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(DebugLoadAndWaitDeclarations(header), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, "{0}", result.Output);
            Save(fixture.GeneratedSource, "debug-flags-" + header + "-" + target);
            string path = fixture.Emit();
            Save(path, "debug-flags-" + header + "-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var group in new[]
            {
                ("SYM_LOAD_FLAGS", new[] { "SLMFLAG_NONE", "SLMFLAG_VIRTUAL", "SLMFLAG_ALT_INDEX", "SLMFLAG_NO_SYMBOLS" }, new uint[] { 0, 1, 2, 4 }),
                ("OPEN_THREAD_WAIT_CHAIN_SESSION_FLAGS", new[] { "WCT_ASYNC_OPEN_FLAG" }, new uint[] { 1 }),
            })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Item1);
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    fields.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
                fields = fields.Where(field => !field.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(group.Item2, fields.Select(field => reader.GetString(field.Name)).ToArray());
                CollectionAssert.AreEqual(group.Item3, fields.Select(field =>
                    reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.AreEqual(group.Item1 == "SYM_LOAD_FLAGS" ? 1 : 0,
                    type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                        .Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "FlagsAttribute"));
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ScopedEnumAttribute"));
            }
            foreach (string name in new[] { "SymLoadModuleEx", "SymLoadModuleExW", "OpenThreadWaitChainSession" })
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == name);
                int position = name == "OpenThreadWaitChainSession" ? 1 : 8;
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(position == 1 ? 2 : 8, signature.ReadCompressedInteger());
                SkipDebugType(ref signature);
                for (int i = 1; i < position; i++) SkipDebugType(ref signature);
                Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode(), name);
                var parameter = method.GetParameters().Select(reader.GetParameter).Single(p => p.SequenceNumber == position);
                CollectionAssert.AreEqual(new[] { position == 1 ? "OPEN_THREAD_WAIT_CHAIN_SESSION_FLAGS" : "SYM_LOAD_FLAGS" },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
                Assert.AreEqual(ParameterAttributes.In, parameter.Attributes & (ParameterAttributes.In | ParameterAttributes.Out));
            }
        }

        private static void SkipDebugType(ref BlobReader signature)
        {
            var code = signature.ReadSignatureTypeCode();
            if (code == SignatureTypeCode.Pointer) SkipDebugType(ref signature);
            else if (code == SignatureTypeCode.TypeHandle) signature.ReadTypeHandle();
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void DebugLoadAndWaitGroups_PreserveBothNativeCAndCppHeaderAbis(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            foreach (string header in new[] { "DbgHelp.h", "ImageHlp.h" })
            {
                fixture.AssertNativeAbi(arch, pointerSize, language,
                    "#define _In_\n#define _In_opt_\n" + DebugLoadAndWaitDeclarations(header) + @"
typedef DWORD64 (__stdcall *SymbolLoaderA)(HANDLE, HANDLE, PCSTR, PCSTR, DWORD64, DWORD, PMODLOAD_DATA, DWORD);
typedef DWORD64 (__stdcall *SymbolLoaderW)(HANDLE, HANDLE, PCWSTR, PCWSTR, DWORD64, DWORD, PMODLOAD_DATA, DWORD);
typedef HWCT (__stdcall *SessionOpener)(DWORD, PWAITCHAINCALLBACK);
SymbolLoaderA loaderA = &SymLoadModuleEx;
SymbolLoaderW loaderW = &SymLoadModuleExW;
SessionOpener opener = &OpenThreadWaitChainSession;
");
                foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
                {
                    string path = Path.Combine(TestContext.ResultsDirectory,
                        "debug-flags-" + header + "-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                    File.Copy(original, path, true);
                    TestContext.AddResultFile(path);
                }
            }
        }

        private static string DebugLoadAndWaitDeclarations(string header)
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string symbols = File.ReadAllText(Path.Combine(headers, header));
            string wait = File.ReadAllText(Path.Combine(headers, "wct.h"));
            return @"
#define IMAGEAPI __stdcall
#define WINAPI __stdcall
#define WINADVAPI
#define _Check_return_
typedef unsigned long DWORD, *LPDWORD;
typedef unsigned __int64 DWORD64;
typedef void *HANDLE, *HWCT;
typedef const char *PCSTR;
typedef const unsigned short *PCWSTR;
typedef struct _MODLOAD_DATA *PMODLOAD_DATA;
typedef struct _WAITCHAIN_NODE_INFO *PWAITCHAIN_NODE_INFO;
typedef int *LPBOOL;
#ifdef _WIN64
typedef unsigned __int64 DWORD_PTR;
#else
typedef unsigned long DWORD_PTR;
#endif
typedef void (__stdcall *PWAITCHAINCALLBACK)(HWCT, DWORD_PTR, DWORD, LPDWORD, PWAITCHAIN_NODE_INFO, LPBOOL);
" +
                Slice(symbols, @"(?m)^#define SLMFLAG_VIRTUAL[\s\S]+?^#endif") + "\n" +
                Slice(symbols, @"DWORD64\s+IMAGEAPI\s+SymLoadModuleEx\([^;]+;") + "\n" +
                Slice(symbols, @"DWORD64\s+IMAGEAPI\s+SymLoadModuleExW\([^;]+;") + "\n" +
                Slice(wait, @"#define WCT_ASYNC_OPEN_FLAG[\s\S]+?_Win32_Enum_\([^;]+;") + "\n" +
                Slice(wait, @"_Check_return_\s+WINADVAPI\s+HWCT WINAPI\s+OpenThreadWaitChainSession \([^;]+;");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void PdhPathAndSourceGroups_PreserveNativeFlagsAndEveryMember(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(PdhPathAndSourceDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, "{0}", result.Output);
            Save(fixture.GeneratedSource, "pdh-flags-" + target);
            string path = fixture.Emit();
            Save(path, "pdh-flags-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var group in new[]
            {
                ("PDH_PATH_FLAGS", new[] { "PDH_PATH_WBEM_RESULT", "PDH_PATH_WBEM_INPUT", "PDH_PATH_WBEM_NONE" }, new uint[] { 1, 2, 0 }),
                ("PDH_SELECT_DATA_SOURCE_FLAGS", new[] { "PDH_FLAGS_FILE_BROWSER_ONLY", "PDH_FLAGS_NONE" }, new uint[] { 1, 0 }),
            })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Item1);
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    fields.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
                fields = fields.Where(field => !field.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(group.Item2, fields.Select(field => reader.GetString(field.Name)).ToArray());
                CollectionAssert.AreEqual(group.Item3, fields.Select(field =>
                    reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute).Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
            }
            foreach (string name in new[] { "PdhMakeCounterPathW", "PdhMakeCounterPathA", "PdhSelectDataSourceW", "PdhSelectDataSourceA" })
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == name);
                bool pathFlags = name.StartsWith("PdhMake", StringComparison.Ordinal);
                int position = pathFlags ? 4 : 2;
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(4, signature.ReadCompressedInteger());
                SkipDebugType(ref signature);
                for (int i = 1; i < position; i++) SkipDebugType(ref signature);
                Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode(), name);
                var parameter = method.GetParameters().Select(reader.GetParameter).Single(p => p.SequenceNumber == position);
                CollectionAssert.AreEqual(new[] { pathFlags ? "PDH_PATH_FLAGS" : "PDH_SELECT_DATA_SOURCE_FLAGS" },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
                Assert.AreEqual(ParameterAttributes.In, parameter.Attributes & (ParameterAttributes.In | ParameterAttributes.Out));
            }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void PdhPathAndSourceGroups_PreserveNativeCAndCppAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language,
                "#define _In_\n#define _Inout_\n#define _Out_writes_opt_(x)\n#define _Inout_updates_(x)\n" +
                PdhPathAndSourceDeclarations() + @"
typedef PDH_STATUS (__stdcall *PathMakerW)(PPDH_COUNTER_PATH_ELEMENTS_W, LPWSTR, LPDWORD, DWORD);
typedef PDH_STATUS (__stdcall *PathMakerA)(PPDH_COUNTER_PATH_ELEMENTS_A, LPSTR, LPDWORD, DWORD);
typedef PDH_STATUS (__stdcall *SourceSelectorW)(HWND, DWORD, LPWSTR, LPDWORD);
typedef PDH_STATUS (__stdcall *SourceSelectorA)(HWND, DWORD, LPSTR, LPDWORD);
PathMakerW pathW = &PdhMakeCounterPathW;
PathMakerA pathA = &PdhMakeCounterPathA;
SourceSelectorW sourceW = &PdhSelectDataSourceW;
SourceSelectorA sourceA = &PdhSelectDataSourceA;
");
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "pdh-flags-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string PdhPathAndSourceDeclarations()
        {
            string pdh = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "Pdh.h"));
            return @"
#define PDH_FUNCTION PDH_STATUS __stdcall
typedef long PDH_STATUS;
typedef unsigned long DWORD, *LPDWORD;
typedef unsigned short *LPWSTR;
typedef char *LPSTR;
typedef void *HWND;
typedef struct _PDH_COUNTER_PATH_ELEMENTS_W *PPDH_COUNTER_PATH_ELEMENTS_W;
typedef struct _PDH_COUNTER_PATH_ELEMENTS_A *PPDH_COUNTER_PATH_ELEMENTS_A;
" +
                Slice(pdh, @"PDH_FUNCTION\s+PdhMakeCounterPathW\([^;]+;") + "\n" +
                Slice(pdh, @"PDH_FUNCTION\s+PdhMakeCounterPathA\([^;]+;") + "\n" +
                Slice(pdh, @"(?m)^#define PDH_PATH_WBEM_RESULT[\s\S]+?^#endif") + "\n" +
                Slice(pdh, @"(?m)^#define PDH_FLAGS_FILE_BROWSER_ONLY[\s\S]+?^#endif") + "\n" +
                Slice(pdh, @"PDH_FUNCTION\s+PdhSelectDataSourceW\([^;]+;") + "\n" +
                Slice(pdh, @"PDH_FUNCTION\s+PdhSelectDataSourceA\([^;]+;");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void GdiBoundsAndTransformGroups_PreserveNativeSlotsAndRecordLayout(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(GdiBoundsAndTransformDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, "{0}", result.Output);
            Save(fixture.GeneratedSource, "gdi-bounds-transform-" + target);
            string path = fixture.Emit();
            Save(path, "gdi-bounds-transform-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var group in new[]
            {
                ("SET_BOUNDS_RECT_FLAGS", new[] { "DCB_ACCUMULATE", "DCB_DISABLE", "DCB_ENABLE", "DCB_RESET" }, new uint[] { 2, 8, 4, 1 }),
                ("MODIFY_WORLD_TRANSFORM_MODE", new[] { "MWT_IDENTITY", "MWT_LEFTMULTIPLY", "MWT_RIGHTMULTIPLY" }, new uint[] { 1, 2, 3 }),
            })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Item1);
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    fields.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
                fields = fields.Where(field => !field.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(group.Item2, fields.Select(field => reader.GetString(field.Name)).ToArray());
                CollectionAssert.AreEqual(group.Item3, fields.Select(field =>
                    reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute).Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
            }
            foreach (var use in new[] { ("SetBoundsRect", "SET_BOUNDS_RECT_FLAGS"), ("ModifyWorldTransform", "MODIFY_WORLD_TRANSFORM_MODE") })
            {
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == use.Item1);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(3, signature.ReadCompressedInteger());
                for (int i = 0; i < 3; i++) SkipDebugType(ref signature);
                Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode(), use.Item1);
                var parameter = method.GetParameters().Select(reader.GetParameter).Single(p => p.SequenceNumber == 3);
                CollectionAssert.AreEqual(new[] { use.Item2 },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
                Assert.AreEqual(ParameterAttributes.In, parameter.Attributes & (ParameterAttributes.In | ParameterAttributes.Out));
            }
            var record = NativeTypedefHeaderTests.Type(reader, "EMRMODIFYWORLDTRANSFORM");
            var mode = record.GetFields().Select(reader.GetFieldDefinition).Single(field => reader.GetString(field.Name) == "iMode");
            EnumAssociationHeaderTests.AssertSignature(reader, mode.Signature, 0, SignatureTypeCode.UInt32);
            CollectionAssert.AreEqual(new[] { "MODIFY_WORLD_TRANSFORM_MODE" },
                EnumAssociationHeaderTests.StringAttributes(reader, mode.GetCustomAttributes(), "AssociatedEnumAttribute"));
            CollectionAssert.AreEqual(new[] { "emr", "xform", "iMode" },
                record.GetFields().Select(reader.GetFieldDefinition).Select(field => reader.GetString(field.Name)).ToArray());
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void GdiBoundsAndTransformGroups_PreserveNativeCAndCppAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language,
                "#define _In_\n#define _In_opt_\n" + GdiBoundsAndTransformDeclarations() + @"
typedef UINT (__stdcall *BoundsSetter)(HDC, CONST RECT*, UINT);
typedef BOOL (__stdcall *TransformModifier)(HDC, CONST XFORM*, DWORD);
BoundsSetter bounds = &SetBoundsRect;
TransformModifier transform = &ModifyWorldTransform;
typedef char CheckTransformRecordSize[(sizeof(EMRMODIFYWORLDTRANSFORM) == 36) ? 1 : -1];
typedef char CheckTransformModeOffset[(((unsigned __int64)&(((EMRMODIFYWORLDTRANSFORM*)0)->iMode)) == 32) ? 1 : -1];
typedef char CheckTransformModeWidth[(sizeof(((EMRMODIFYWORLDTRANSFORM*)0)->iMode) == 4) ? 1 : -1];
");
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "gdi-bounds-transform-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string GdiBoundsAndTransformDeclarations()
        {
            string gdi = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "wingdi.h"));
            return @"
#define WINGDIAPI
#define WINAPI __stdcall
#define FAR
#define CONST const
typedef unsigned int UINT;
typedef unsigned long DWORD;
typedef int BOOL;
typedef float FLOAT;
typedef void *HDC;
typedef struct tagRECT RECT;
" +
                Slice(gdi, @"(?m)^#define DCB_RESET[\s\S]+?_Win32_Enum_\(SET_BOUNDS_RECT_FLAGS[^;]+;") + "\n" +
                Slice(gdi, @"(?m)^#define MWT_IDENTITY[\s\S]+?_Win32_Enum_\(MODIFY_WORLD_TRANSFORM_MODE[^;]+;") + "\n" +
                Slice(gdi, @"typedef struct\s+tagXFORM\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(gdi, @"typedef struct tagEMR\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(gdi, @"typedef struct tagEMRMODIFYWORLDTRANSFORM\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(gdi, @"WINGDIAPI UINT\s+WINAPI SetBoundsRect\([^;]+;") + "\n" +
                Slice(gdi, @"WINGDIAPI BOOL WINAPI ModifyWorldTransform\([^;]+;");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void TbsCommandGroups_PreserveNativeTypedefStorageAndEveryMember(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(TbsCommandDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, "{0}", result.Output);
            Save(fixture.GeneratedSource, "tbs-command-" + target);
            string path = fixture.Emit();
            Save(path, "tbs-command-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var group in new[]
            {
                ("TBS_COMMAND_PRIORITY", new[] { "TBS_COMMAND_PRIORITY_LOW", "TBS_COMMAND_PRIORITY_NORMAL", "TBS_COMMAND_PRIORITY_SYSTEM", "TBS_COMMAND_PRIORITY_HIGH", "TBS_COMMAND_PRIORITY_MAX" }, new uint[] { 100, 200, 400, 300, 0x80000000 }),
                ("TBS_COMMAND_LOCALITY", new[] { "TBS_COMMAND_LOCALITY_ZERO", "TBS_COMMAND_LOCALITY_ONE", "TBS_COMMAND_LOCALITY_TWO", "TBS_COMMAND_LOCALITY_THREE", "TBS_COMMAND_LOCALITY_FOUR" }, new uint[] { 0, 1, 2, 3, 4 }),
            })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Item1);
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    fields.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
                fields = fields.Where(field => !field.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(group.Item2, fields.Select(field => reader.GetString(field.Name)).ToArray());
                CollectionAssert.AreEqual(group.Item3, fields.Select(field =>
                    reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute).Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
            }
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Tbsip_Submit_Command");
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(7, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            foreach (string group in new[] { "TBS_COMMAND_LOCALITY", "TBS_COMMAND_PRIORITY" })
            {
                Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode(), group);
                int position = group == "TBS_COMMAND_LOCALITY" ? 2 : 3;
                var parameter = method.GetParameters().Select(reader.GetParameter).Single(p => p.SequenceNumber == position);
                CollectionAssert.AreEqual(new[] { group },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
                Assert.AreEqual(ParameterAttributes.In, parameter.Attributes & (ParameterAttributes.In | ParameterAttributes.Out));
            }
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Byte, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Byte, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void TbsCommandGroups_PreserveNativeCAndCppAbi(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language,
                "#define _In_\n#define _Inout_\n#define _In_reads_bytes_(x)\n#define _Out_writes_bytes_(x)\n" +
                TbsCommandDeclarations() + @"
typedef UINT32 (__stdcall *SubmitCommand)(void*, UINT32, UINT32, const BYTE*, UINT32, BYTE*, UINT32*);
SubmitCommand submit = &Tbsip_Submit_Command;
typedef char CheckLocalityWidth[(sizeof(TBS_COMMAND_LOCALITY) == 4) ? 1 : -1];
typedef char CheckPriorityWidth[(sizeof(TBS_COMMAND_PRIORITY) == 4) ? 1 : -1];
typedef char CheckUnsignedPriorityMax[(TBS_COMMAND_PRIORITY_MAX == 0x80000000U) ? 1 : -1];
");
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "tbs-command-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string TbsCommandDeclarations()
        {
            string tbs = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "shared", "tbs.h"));
            return @"
#define WINAPI __stdcall
typedef unsigned int UINT32, *PUINT32;
typedef unsigned char BYTE, *PBYTE;
typedef const BYTE *PCBYTE;
typedef void *TBS_HCONTEXT;
typedef UINT32 TBS_RESULT;
" +
                Slice(tbs, @"(?m)^typedef UINT32 TBS_COMMAND_PRIORITY;") + "\n" +
                Slice(tbs, @"(?m)^typedef UINT32 TBS_COMMAND_LOCALITY;") + "\n" +
                Slice(tbs, @"(?m)^#define TBS_COMMAND_PRIORITY_LOW[\s\S]+?_Win32_Enum_\(TBS_COMMAND_PRIORITY[^;]+;") + "\n" +
                Slice(tbs, @"(?m)^#define TBS_COMMAND_LOCALITY_ZERO[\s\S]+?_Win32_Enum_\(TBS_COMMAND_LOCALITY[^;]+;") + "\n" +
                Slice(tbs, @"TBS_RESULT WINAPI\s+Tbsip_Submit_Command\([^;]+;");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void PanoseGroups_PreserveEveryByteFieldAndCompatibilitySentinel(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(PanoseDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, "{0}", result.Output);
            Save(fixture.GeneratedSource, "panose-" + target);
            string path = fixture.Emit();
            Save(path, "panose-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var fields = NativeTypedefHeaderTests.Type(reader, "PANOSE").GetFields().Select(reader.GetFieldDefinition).ToArray();
            var uses = new[]
            {
                ("bFamilyType", "PAN_FAMILY_TYPE", 6), ("bSerifStyle", "PAN_SERIF_STYLE", 16),
                ("bWeight", "PAN_WEIGHT", 12), ("bProportion", "PAN_PROPORTION", 10),
                ("bContrast", "PAN_CONTRAST", 10), ("bStrokeVariation", "PAN_STROKE_VARIATION", 9),
                ("bArmStyle", "PAN_ARM_STYLE", 12), ("bLetterform", "PAN_LETT_FORM", 16),
                ("bMidline", "PAN_MIDLINE", 14), ("bXHeight", "PAN_XHEIGHT", 8),
            };
            CollectionAssert.AreEqual(uses.Select(use => use.Item1).ToArray(),
                fields.Select(field => reader.GetString(field.Name)).ToArray());
            for (int i = 0; i < uses.Length; i++)
            {
                var use = uses[i];
                EnumAssociationHeaderTests.AssertSignature(reader, fields[i].Signature, 0, SignatureTypeCode.Byte);
                CollectionAssert.AreEqual(new[] { use.Item2 },
                    EnumAssociationHeaderTests.StringAttributes(reader, fields[i].GetCustomAttributes(), "AssociatedEnumAttribute"));
                var type = NativeTypedefHeaderTests.Type(reader, use.Item2);
                var members = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
                EnumAssociationHeaderTests.AssertSignature(reader,
                    members.Single(field => reader.GetString(field.Name) == "value__").Signature, 0, SignatureTypeCode.Byte);
                members = members.Where(field => !field.GetDefaultValue().IsNil).ToArray();
                Assert.IsFalse(members.Any(field => reader.GetString(field.Name).EndsWith("_INDEX", StringComparison.Ordinal)),
                    use.Item2 + " must not contain PANOSE field-position macros.");
                Assert.AreEqual(use.Item3, members.Length, use.Item2);
                Assert.AreEqual((byte)0, reader.GetBlobReader(reader.GetConstant(members[0].GetDefaultValue()).Value).ReadByte());
                Assert.AreEqual((byte)1, reader.GetBlobReader(reader.GetConstant(members[1].GetDefaultValue()).Value).ReadByte());
                StringAssert.EndsWith(reader.GetString(members[0].Name), "_ANY");
                StringAssert.EndsWith(reader.GetString(members[1].Name), "_NO_FIT");
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute).Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
            }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void PanoseGroups_PreserveNativeCAndCppRecordLayout(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = PanoseDeclarations() + "\ntypedef char CheckPanoseSize[(sizeof(PANOSE) == 10) ? 1 : -1];\n";
            string[] fields = { "bFamilyType", "bSerifStyle", "bWeight", "bProportion", "bContrast",
                "bStrokeVariation", "bArmStyle", "bLetterform", "bMidline", "bXHeight" };
            for (int i = 0; i < fields.Length; i++)
            {
                source += $"typedef char Check{fields[i]}Width[(sizeof(((PANOSE*)0)->{fields[i]}) == 1) ? 1 : -1];\n";
                source += $"typedef char Check{fields[i]}Offset[(((unsigned __int64)&(((PANOSE*)0)->{fields[i]})) == {i}) ? 1 : -1];\n";
            }
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "panose-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string PanoseDeclarations()
        {
            string gdi = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "wingdi.h"));
            return "typedef unsigned char BYTE;\n" +
                Slice(gdi, @"(?m)^#define PANOSE_COUNT[\s\S]+?^#define PAN_CULTURE_LATIN[^\r\n]+") + "\n" +
                Slice(gdi, @"typedef struct tagPANOSE\s*\{[^}]+\}[^;]+;") + "\n" +
                Slice(gdi, @"(?m)^#define PAN_ANY[\s\S]+?_Win32_Enum_\(PAN_XHEIGHT[^;]+;\r?\n#endif");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void DevModeDisplayGroups_PreserveBothNativeAnonymousRecordUses(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(DevModeDisplayDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "devmode-display-" + target);
            string path = fixture.Emit();
            Save(path, "devmode-display-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var use in new[]
            {
                ("dmDisplayOrientation", "DEVMODE_DISPLAY_ORIENTATION", new[] { "DMDO_DEFAULT", "DMDO_90", "DMDO_180", "DMDO_270" }),
                ("dmDisplayFixedOutput", "DEVMODE_DISPLAY_FIXED_OUTPUT", new[] { "DMDFO_DEFAULT", "DMDFO_STRETCH", "DMDFO_CENTER" }),
            })
            {
                var fields = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                    .Where(field => reader.GetString(field.Name) == use.Item1).ToArray();
                Assert.AreEqual(2, fields.Length, use.Item1);
                var owners = fields.Select(field =>
                {
                    EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
                    CollectionAssert.AreEqual(new[] { use.Item2 },
                        EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
                    var inner = reader.GetTypeDefinition(field.GetDeclaringType());
                    var union = reader.GetTypeDefinition(inner.GetDeclaringType());
                    return reader.GetString(reader.GetTypeDefinition(union.GetDeclaringType()).Name);
                }).OrderBy(name => name).ToArray();
                CollectionAssert.AreEqual(new[] { "DEVMODEA", "DEVMODEW" }, owners);
                var type = NativeTypedefHeaderTests.Type(reader, use.Item2);
                var members = type.GetFields().Select(reader.GetFieldDefinition)
                    .Where(field => !field.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(use.Item3, members.Select(field => reader.GetString(field.Name)).ToArray());
                CollectionAssert.AreEqual(Enumerable.Range(0, members.Length).Select(value => (uint)value).ToArray(),
                    members.Select(field => reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute).Any(attribute =>
                    ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
            }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void DevModeDisplayGroups_PreserveNativeCAndCppLayouts(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = DevModeDisplayDeclarations();
            foreach (var record in new[] { ("DEVMODEA", 156, 52), ("DEVMODEW", 220, 84) })
            {
                source += $"\ntypedef char Check{record.Item1}Size[(sizeof({record.Item1}) == {record.Item2}) ? 1 : -1];\n";
                foreach (var field in new[] { ("dmDisplayOrientation", record.Item3), ("dmDisplayFixedOutput", record.Item3 + 4) })
                {
                    source += $"typedef char Check{record.Item1}{field.Item1}Width[(sizeof((({record.Item1}*)0)->{field.Item1}) == 4) ? 1 : -1];\n";
                    source += $"typedef char Check{record.Item1}{field.Item1}Offset[(((unsigned __int64)&((({record.Item1}*)0)->{field.Item1})) == {field.Item2}) ? 1 : -1];\n";
                }
            }
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "devmode-display-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string DevModeDisplayDeclarations()
        {
            string gdi = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "wingdi.h"));
            return @"
#define WINVER 0x0A00
#define _WIN32_WINNT 0x0A00
#define CCHDEVICENAME 32
#define CCHFORMNAME 32
#define DUMMYSTRUCTNAME
#define DUMMYSTRUCTNAME2
#define DUMMYUNIONNAME
#define DUMMYUNIONNAME2
typedef unsigned char BYTE;
typedef unsigned short WORD;
typedef unsigned short WCHAR;
typedef unsigned long DWORD;
typedef struct { long x; long y; } POINTL;
" + Slice(gdi, @"typedef struct _devicemodeA\s*\{[\s\S]+?\} DEVMODEA[^;]+;") + "\n" +
                Slice(gdi, @"typedef struct _devicemodeW\s*\{[\s\S]+?\} DEVMODEW[^;]+;") + "\n" +
                Slice(gdi, @"(?m)^[ \t]*#if\(WINVER >= 0x0501\)\r?\n[ \t]*/\* DEVMODE dmDisplayOrientation[\s\S]+?^[ \t]*#endif[^\r\n]*");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ModemGroups_DistinguishCapabilityBitsFromNativeSettingValues(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(ModemDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "modem-" + target);
            string path = fixture.Emit();
            Save(path, "modem-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var use in new[]
            {
                ("MODEMDEVCAPS", "dwDialOptions", "MODEMDEVCAPS_DIAL_OPTIONS", true,
                    new[] { "DIALOPTION_BILLING", "DIALOPTION_DIALTONE", "DIALOPTION_QUIET" }, new uint[] { 64, 256, 128 }),
                ("MODEMDEVCAPS", "dwSpeakerVolume", "MODEMDEVCAPS_SPEAKER_VOLUME", true,
                    new[] { "MDMVOLFLAG_HIGH", "MDMVOLFLAG_LOW", "MDMVOLFLAG_MEDIUM" }, new uint[] { 4, 1, 2 }),
                ("MODEMDEVCAPS", "dwSpeakerMode", "MODEMDEVCAPS_SPEAKER_MODE", true,
                    new[] { "MDMSPKRFLAG_CALLSETUP", "MDMSPKRFLAG_DIAL", "MDMSPKRFLAG_OFF", "MDMSPKRFLAG_ON" }, new uint[] { 8, 2, 1, 4 }),
                ("MODEMSETTINGS", "dwSpeakerVolume", "MODEM_SPEAKER_VOLUME", false,
                    new[] { "MDMVOL_HIGH", "MDMVOL_LOW", "MDMVOL_MEDIUM" }, new uint[] { 2, 0, 1 }),
                ("MODEMSETTINGS", "dwSpeakerMode", "MODEMSETTINGS_SPEAKER_MODE", false,
                    new[] { "MDMSPKR_CALLSETUP", "MDMSPKR_DIAL", "MDMSPKR_OFF", "MDMSPKR_ON" }, new uint[] { 3, 1, 0, 2 }),
            })
            {
                var owner = NativeTypedefHeaderTests.Type(reader, use.Item1);
                var field = owner.GetFields().Select(reader.GetFieldDefinition)
                    .Single(candidate => reader.GetString(candidate.Name) == use.Item2);
                EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
                CollectionAssert.AreEqual(new[] { use.Item3 },
                    EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
                var type = NativeTypedefHeaderTests.Type(reader, use.Item3);
                var members = type.GetFields().Select(reader.GetFieldDefinition)
                    .Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(use.Item5, members.Select(member => reader.GetString(member.Name)).ToArray());
                CollectionAssert.AreEqual(use.Item6,
                    members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.AreEqual(use.Item4, type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "FlagsAttribute"));
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ScopedEnumAttribute"));
            }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void ModemGroups_PreserveNativeCAndCppLayouts(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = ModemDeclarations() + @"
typedef char CheckCapsSize[(sizeof(MODEMDEVCAPS) == 80) ? 1 : -1];
typedef char CheckSettingsSize[(sizeof(MODEMSETTINGS) == 48) ? 1 : -1];
";
            foreach (var use in new[]
            {
                ("MODEMDEVCAPS", "dwDialOptions", 44), ("MODEMDEVCAPS", "dwSpeakerVolume", 56),
                ("MODEMDEVCAPS", "dwSpeakerMode", 60), ("MODEMSETTINGS", "dwSpeakerVolume", 24),
                ("MODEMSETTINGS", "dwSpeakerMode", 28),
            })
            {
                source += $"typedef char Check{use.Item1}{use.Item2}Width[(sizeof((({use.Item1}*)0)->{use.Item2}) == 4) ? 1 : -1];\n";
                source += $"typedef char Check{use.Item1}{use.Item2}Offset[(((unsigned __int64)&((({use.Item1}*)0)->{use.Item2})) == {use.Item3}) ? 1 : -1];\n";
            }
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "modem-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string ModemDeclarations() =>
            "typedef unsigned long DWORD;\ntypedef unsigned char BYTE;\n" +
            File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "mcx.h"));

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void RestorePointGroups_AssociateEveryNativeRecordIncludingEx(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(RestorePointDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "restore-point-" + target);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "Char.manual.cs"),
                "namespace Test { [Windows.Win32.Foundation.Metadata.NativeTypedef] public struct CHAR { public sbyte Value; } }");
            string path = fixture.Emit();
            Save(path, "restore-point-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var use in new[]
            {
                ("dwRestorePtType", "RESTOREPOINTINFO_TYPE",
                    new[] { "APPLICATION_INSTALL", "APPLICATION_UNINSTALL", "DEVICE_DRIVER_INSTALL", "MODIFY_SETTINGS", "CANCELLED_OPERATION" },
                    new uint[] { 0, 1, 10, 12, 13 }),
                ("dwEventType", "RESTOREPOINTINFO_EVENT_TYPE",
                    new[] { "BEGIN_NESTED_SYSTEM_CHANGE", "BEGIN_SYSTEM_CHANGE", "END_NESTED_SYSTEM_CHANGE", "END_SYSTEM_CHANGE" },
                    new uint[] { 102, 100, 103, 101 }),
            })
            {
                foreach (string record in new[] { "RESTOREPOINTINFOA", "RESTOREPOINTINFOW", "RESTOREPOINTINFOEX" })
                {
                    var owner = NativeTypedefHeaderTests.Type(reader, record);
                    var field = owner.GetFields().Select(reader.GetFieldDefinition)
                        .Single(candidate => reader.GetString(candidate.Name) == use.Item1);
                    EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
                    CollectionAssert.AreEqual(new[] { use.Item2 },
                        EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
                }

                var type = NativeTypedefHeaderTests.Type(reader, use.Item2);
                var members = type.GetFields().Select(reader.GetFieldDefinition)
                    .Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
                CollectionAssert.AreEqual(use.Item3, members.Select(member => reader.GetString(member.Name)).ToArray());
                CollectionAssert.AreEqual(use.Item4,
                    members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
                Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
            }
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void RestorePointGroups_PreserveNativeCAndCppLayouts(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = RestorePointDeclarations();
            foreach (var record in new[]
            {
                ("RESTOREPOINTINFOA", 80, 0), ("RESTOREPOINTINFOW", 528, 0), ("RESTOREPOINTINFOEX", 532, 8),
            })
            {
                source += $"typedef char Check{record.Item1}Size[(sizeof({record.Item1}) == {record.Item2}) ? 1 : -1];\n";
                foreach (var field in new[] { ("dwEventType", record.Item3), ("dwRestorePtType", record.Item3 + 4) })
                {
                    source += $"typedef char Check{record.Item1}{field.Item1}Width[(sizeof((({record.Item1}*)0)->{field.Item1}) == 4) ? 1 : -1];\n";
                    source += $"typedef char Check{record.Item1}{field.Item1}Offset[(((unsigned __int64)&((({record.Item1}*)0)->{field.Item1})) == {field.Item2}) ? 1 : -1];\n";
                }
            }
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "restore-point-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string RestorePointDeclarations()
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "SrRestorePtApi.h"));
            return @"
typedef unsigned long DWORD;
typedef __int64 INT64;
typedef char CHAR;
typedef unsigned short WCHAR;
typedef struct { DWORD dwLowDateTime; DWORD dwHighDateTime; } FILETIME;
" + Slice(header, @"(?m)^#define MIN_EVENT[\s\S]+?^#define MAX_DESC_W[^\r\n]+") + "\n" +
                Slice(header, @"(?m)^#pragma pack\(push, srrestoreptapi_include\)[\s\S]+?^#pragma pack\(pop, srrestoreptapi_include\)") + "\n";
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void PeerDistGroups_AssociateStatusAndRetrievalVersionRanges(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(PeerDistDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "peerdist-" + target);
            string path = fixture.Emit();
            Save(path, "peerdist-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            const string group = "PEERDIST_RETRIEVAL_OPTIONS_CONTENTINFO_VERSION_VALUE";
            foreach (var use in new[]
            {
                ("PEERDIST_STATUS_INFO", "dwMinVer"), ("PEERDIST_STATUS_INFO", "dwMaxVer"),
                ("PEERDIST_RETRIEVAL_OPTIONS", "dwContentInfoMinVersion"),
                ("PEERDIST_RETRIEVAL_OPTIONS", "dwContentInfoMaxVersion"),
            })
            {
                var owner = NativeTypedefHeaderTests.Type(reader, use.Item1);
                var field = owner.GetFields().Select(reader.GetFieldDefinition)
                    .Single(candidate => reader.GetString(candidate.Name) == use.Item2);
                EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
                CollectionAssert.AreEqual(new[] { group },
                    EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }

            var type = NativeTypedefHeaderTests.Type(reader, group);
            var members = type.GetFields().Select(reader.GetFieldDefinition)
                .Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                "PEERDIST_RETRIEVAL_OPTIONS_CONTENTINFO_VERSION_1",
                "PEERDIST_RETRIEVAL_OPTIONS_CONTENTINFO_VERSION_2",
                "PEERDIST_RETRIEVAL_OPTIONS_CONTENTINFO_VERSION",
            }, members.Select(member => reader.GetString(member.Name)).ToArray());
            CollectionAssert.AreEqual(new uint[] { 1, 2, 2 },
                members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
            Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void PeerDistGroups_PreserveNativeCAndCppLayouts(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = PeerDistDeclarations() + @"
typedef char CheckStatusSize[(sizeof(PEERDIST_STATUS_INFO) == 16) ? 1 : -1];
typedef char CheckRetrievalSize[(sizeof(PEERDIST_RETRIEVAL_OPTIONS) == 16) ? 1 : -1];
";
            foreach (var use in new[]
            {
                ("PEERDIST_STATUS_INFO", "dwMinVer", 8), ("PEERDIST_STATUS_INFO", "dwMaxVer", 12),
                ("PEERDIST_RETRIEVAL_OPTIONS", "dwContentInfoMinVersion", 4),
                ("PEERDIST_RETRIEVAL_OPTIONS", "dwContentInfoMaxVersion", 8),
            })
            {
                source += $"typedef char Check{use.Item1}{use.Item2}Width[(sizeof((({use.Item1}*)0)->{use.Item2}) == 4) ? 1 : -1];\n";
                source += $"typedef char Check{use.Item1}{use.Item2}Offset[(((unsigned __int64)&((({use.Item1}*)0)->{use.Item2})) == {use.Item3}) ? 1 : -1];\n";
            }
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "peerdist-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string PeerDistDeclarations()
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "PeerDist.h"));
            return "typedef unsigned long DWORD;\n" +
                Slice(header, @"typedef enum\s*\{[^}]+\} PEERDIST_STATUS;") + "\n" +
                Slice(header, @"(?m)^#define PEERDIST_RETRIEVAL_OPTIONS_CONTENTINFO_VERSION\s[\s\S]+?^typedef const PEERDIST_STATUS_INFO[^;]+;") + "\n";
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void CertificateSelectionGroup_PreservesNativeCriterionAndSelectionBounds(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(CertificateSelectionDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "certificate-selection-" + target);
            string path = fixture.Emit();
            Save(path, "certificate-selection-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var owner = NativeTypedefHeaderTests.Type(reader, "CERT_SELECT_CRITERIA");
            var field = owner.GetFields().Select(reader.GetFieldDefinition)
                .Single(candidate => reader.GetString(candidate.Name) == "dwType");
            EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
            CollectionAssert.AreEqual(new[] { "CERT_SELECT_CRITERIA_TYPE" },
                EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
            var type = NativeTypedefHeaderTests.Type(reader, "CERT_SELECT_CRITERIA_TYPE");
            var members = type.GetFields().Select(reader.GetFieldDefinition)
                .Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                "CERT_SELECT_BY_ENHKEY_USAGE", "CERT_SELECT_BY_KEY_USAGE", "CERT_SELECT_BY_POLICY_OID",
                "CERT_SELECT_BY_PROV_NAME", "CERT_SELECT_BY_EXTENSION", "CERT_SELECT_BY_SUBJECT_HOST_NAME",
                "CERT_SELECT_BY_ISSUER_ATTR", "CERT_SELECT_BY_SUBJECT_ATTR", "CERT_SELECT_BY_ISSUER_NAME",
                "CERT_SELECT_BY_PUBLIC_KEY", "CERT_SELECT_BY_TLS_SIGNATURES",
            }, members.Select(member => reader.GetString(member.Name)).ToArray());
            CollectionAssert.AreEqual(Enumerable.Range(1, 11).Select(value => (uint)value).ToArray(),
                members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
            Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void CertificateSelectionGroup_PreservesNativeCAndCppLayouts(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = "#define _Field_size_(count)\n" + CertificateSelectionDeclarations() + $@"
typedef char CheckSize[(sizeof(CERT_SELECT_CRITERIA) == {8 + pointerSize}) ? 1 : -1];
typedef char CheckKindWidth[(sizeof(((CERT_SELECT_CRITERIA*)0)->dwType) == 4) ? 1 : -1];
typedef char CheckKindOffset[(((unsigned __int64)&(((CERT_SELECT_CRITERIA*)0)->dwType)) == 0) ? 1 : -1];
typedef char CheckCountOffset[(((unsigned __int64)&(((CERT_SELECT_CRITERIA*)0)->cPara)) == 4) ? 1 : -1];
typedef char CheckPointerOffset[(((unsigned __int64)&(((CERT_SELECT_CRITERIA*)0)->ppPara)) == 8) ? 1 : -1];
typedef char CheckWin32Last[(CERT_SELECT_LAST == 11) ? 1 : -1];
typedef char CheckWin32Maximum[(CERT_SELECT_MAX == 33) ? 1 : -1];
typedef char CheckSeparateWinRtCriteria[(CERT_SELECT_BY_ISSUER_DISPLAYNAME == 12 &&
    CERT_SELECT_BY_FRIENDLYNAME == 13 && CERT_SELECT_BY_THUMBPRINT == 14) ? 1 : -1];
";
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "certificate-selection-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string CertificateSelectionDeclarations()
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "wincrypt.h"));
            return "typedef unsigned long DWORD;\n" +
                Slice(header, @"(?m)^typedef struct _CERT_SELECT_CRITERIA[\s\S]+?^// Selection Flags[^\r\n]*") + "\n";
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void MixerComponentGroup_PreservesBothNativeLineFieldsAndAllComponentValues(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(MixerComponentDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "mixer-component-" + target);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "Char.manual.cs"),
                "namespace Test { [Windows.Win32.Foundation.Metadata.NativeTypedef] public struct CHAR { public sbyte Value; } }");
            string path = fixture.Emit();
            Save(path, "mixer-component-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (string ownerName in new[] { "MIXERLINEA", "MIXERLINEW" })
            {
                var owner = NativeTypedefHeaderTests.Type(reader, ownerName);
                var field = owner.GetFields().Select(reader.GetFieldDefinition)
                    .Single(candidate => reader.GetString(candidate.Name) == "dwComponentType");
                EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
                CollectionAssert.AreEqual(new[] { "MIXERLINE_COMPONENTTYPE" },
                    EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }
            var type = NativeTypedefHeaderTests.Type(reader, "MIXERLINE_COMPONENTTYPE");
            var members = type.GetFields().Select(reader.GetFieldDefinition)
                .Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                "MIXERLINE_COMPONENTTYPE_DST_DIGITAL", "MIXERLINE_COMPONENTTYPE_DST_HEADPHONES",
                "MIXERLINE_COMPONENTTYPE_DST_LINE", "MIXERLINE_COMPONENTTYPE_DST_MONITOR",
                "MIXERLINE_COMPONENTTYPE_DST_SPEAKERS", "MIXERLINE_COMPONENTTYPE_DST_TELEPHONE",
                "MIXERLINE_COMPONENTTYPE_DST_UNDEFINED", "MIXERLINE_COMPONENTTYPE_DST_VOICEIN",
                "MIXERLINE_COMPONENTTYPE_DST_WAVEIN", "MIXERLINE_COMPONENTTYPE_SRC_ANALOG",
                "MIXERLINE_COMPONENTTYPE_SRC_AUXILIARY", "MIXERLINE_COMPONENTTYPE_SRC_COMPACTDISC",
                "MIXERLINE_COMPONENTTYPE_SRC_DIGITAL", "MIXERLINE_COMPONENTTYPE_SRC_LINE",
                "MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE", "MIXERLINE_COMPONENTTYPE_SRC_PCSPEAKER",
                "MIXERLINE_COMPONENTTYPE_SRC_SYNTHESIZER", "MIXERLINE_COMPONENTTYPE_SRC_TELEPHONE",
                "MIXERLINE_COMPONENTTYPE_SRC_UNDEFINED", "MIXERLINE_COMPONENTTYPE_SRC_WAVEOUT",
            }, members.Select(member => reader.GetString(member.Name)).ToArray());
            CollectionAssert.AreEqual(new uint[] { 1, 5, 2, 3, 4, 6, 0, 8, 7, 4106, 4105, 4101,
                4097, 4098, 4099, 4103, 4100, 4102, 4096, 4104 },
                members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
            Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) is "FlagsAttribute" or "ScopedEnumAttribute"));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void MixerComponentGroup_PreservesNativePackedCAndCppLayouts(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = MixerComponentDeclarations() + $@"
typedef char CheckAnsiSize[(sizeof(MIXERLINEA) == {164 + pointerSize}) ? 1 : -1];
typedef char CheckWideSize[(sizeof(MIXERLINEW) == {276 + pointerSize}) ? 1 : -1];
typedef char CheckAnsiWidth[(sizeof(((MIXERLINEA*)0)->dwComponentType) == 4) ? 1 : -1];
typedef char CheckWideWidth[(sizeof(((MIXERLINEW*)0)->dwComponentType) == 4) ? 1 : -1];
typedef char CheckAnsiOffset[(((unsigned __int64)&(((MIXERLINEA*)0)->dwComponentType)) == {20 + pointerSize}) ? 1 : -1];
typedef char CheckWideOffset[(((unsigned __int64)&(((MIXERLINEW*)0)->dwComponentType)) == {20 + pointerSize}) ? 1 : -1];
typedef char CheckNativeRanges[(MIXERLINE_COMPONENTTYPE_DST_FIRST == 0 &&
    MIXERLINE_COMPONENTTYPE_DST_LAST == 8 && MIXERLINE_COMPONENTTYPE_SRC_FIRST == 4096 &&
    MIXERLINE_COMPONENTTYPE_SRC_LAST == 4106) ? 1 : -1];
";
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "mixer-component-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string MixerComponentDeclarations()
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string header = File.ReadAllText(Path.Combine(headers, "mmeapi.h"));
            string common = File.ReadAllText(Path.Combine(headers, "mmsyscom.h"));
            StringAssert.Contains(File.ReadAllText(Path.Combine(headers, "mmsystem.h")), "#include <pshpack1.h>");
            return @"
#define NEAR
#define FAR
typedef unsigned long DWORD;
typedef unsigned short WORD;
typedef unsigned short WCHAR;
typedef char CHAR;
#ifdef _WIN64
typedef unsigned __int64 DWORD_PTR;
#else
typedef unsigned long DWORD_PTR;
#endif
typedef unsigned int UINT;
#pragma pack(push, 1)
" + Slice(common, @"(?m)^typedef UINT\s+MMVERSION;[^\r\n]*") + "\n" +
                Slice(common, @"(?m)^#define MAXPNAMELEN[^\r\n]+") + "\n" +
                Slice(header, @"(?m)^#define MIXER_SHORT_NAME_CHARS[^\r\n]+") + "\n" +
                Slice(header, @"(?m)^#define MIXER_LONG_NAME_CHARS[^\r\n]+") + "\n" +
                Slice(header, @"typedef struct tagMIXERLINEA \{[\s\S]+?\} MIXERLINEA[^;]+;") + "\n" +
                Slice(header, @"typedef struct tagMIXERLINEW \{[\s\S]+?\} MIXERLINEW[^;]+;") + "\n#pragma pack(pop)\n" +
                Slice(header, @"(?m)^#define MIXERLINE_COMPONENTTYPE_DST_FIRST[\s\S]+?^#define MIXERLINE_COMPONENTTYPE_SRC_LAST[^\r\n]+") + "\n" +
                (Regex.Match(header, @"(?m)^_Win32_Enum_\(MIXERLINE_COMPONENTTYPE,[^\r\n]+").Value) + "\n";
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void XmlSignatureStatusGroup_PreservesNativeErrorFlagsAndExistingInfoAssociation(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(XmlSignatureStatusDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(fixture.GeneratedSource, "xml-signature-status-" + target);
            string path = fixture.Emit();
            Save(path, "xml-signature-status-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var owner = NativeTypedefHeaderTests.Type(reader, "CRYPT_XML_STATUS");
            foreach (var use in new[]
            {
                ("dwErrorStatus", "CRYPT_XML_STATUS_ERROR_STATUS"),
                ("dwInfoStatus", "CRYPT_XML_STATUS_INFO_STATUS"),
            })
            {
                var field = owner.GetFields().Select(reader.GetFieldDefinition)
                    .Single(candidate => reader.GetString(candidate.Name) == use.Item1);
                EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
                CollectionAssert.AreEqual(new[] { use.Item2 },
                    EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }
            var type = NativeTypedefHeaderTests.Type(reader, "CRYPT_XML_STATUS_ERROR_STATUS");
            var members = type.GetFields().Select(reader.GetFieldDefinition)
                .Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                "CRYPT_XML_STATUS_ERROR_NOT_RESOLVED", "CRYPT_XML_STATUS_ERROR_DIGEST_INVALID",
                "CRYPT_XML_STATUS_ERROR_NOT_SUPPORTED_ALGORITHM", "CRYPT_XML_STATUS_ERROR_NOT_SUPPORTED_TRANSFORM",
                "CRYPT_XML_STATUS_ERROR_SIGNATURE_INVALID", "CRYPT_XML_STATUS_ERROR_KEYINFO_NOT_PARSED",
            }, members.Select(member => reader.GetString(member.Name)).ToArray());
            CollectionAssert.AreEqual(new uint[] { 1, 2, 4, 8, 65536, 131072 },
                members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
            var attributes = type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray();
            Assert.IsTrue(attributes.Contains("FlagsAttribute"));
            Assert.IsFalse(attributes.Contains("ScopedEnumAttribute"));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void XmlSignatureStatusGroup_PreservesCompleteNativeCAndCppRecordAndMasks(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = XmlSignatureStatusDeclarations() + @"
typedef char CheckStatusSize[(sizeof(CRYPT_XML_STATUS) == 12) ? 1 : -1];
typedef char CheckSizeOffset[(((unsigned __int64)&(((CRYPT_XML_STATUS*)0)->cbSize)) == 0) ? 1 : -1];
typedef char CheckErrorOffset[(((unsigned __int64)&(((CRYPT_XML_STATUS*)0)->dwErrorStatus)) == 4) ? 1 : -1];
typedef char CheckInfoOffset[(((unsigned __int64)&(((CRYPT_XML_STATUS*)0)->dwInfoStatus)) == 8) ? 1 : -1];
typedef char CheckErrorWidth[(sizeof(((CRYPT_XML_STATUS*)0)->dwErrorStatus) == 4) ? 1 : -1];
typedef char CheckInfoWidth[(sizeof(((CRYPT_XML_STATUS*)0)->dwInfoStatus) == 4) ? 1 : -1];
typedef char CheckNoError[(CRYPT_XML_STATUS_NO_ERROR == 0) ? 1 : -1];
typedef char CheckNativeErrorMask[((CRYPT_XML_STATUS_ERROR_NOT_RESOLVED |
    CRYPT_XML_STATUS_ERROR_DIGEST_INVALID | CRYPT_XML_STATUS_ERROR_NOT_SUPPORTED_ALGORITHM |
    CRYPT_XML_STATUS_ERROR_NOT_SUPPORTED_TRANSFORM | CRYPT_XML_STATUS_ERROR_SIGNATURE_INVALID |
    CRYPT_XML_STATUS_ERROR_KEYINFO_NOT_PARSED) == 0x0003000F) ? 1 : -1];
typedef char CheckNativeAlgorithmBit[(CRYPT_XML_STATUS_ERROR_NOT_SUPPORTED_ALGORITHM == 4 &&
    (CRYPT_XML_STATUS_ERROR_NOT_SUPPORTED_ALGORITHM & CRYPT_XML_STATUS_ERROR_NOT_RESOLVED) == 0) ? 1 : -1];
typedef char CheckUnchangedHighInfoFlag[(CRYPT_XML_STATUS_OPENED_TO_ENCODE == 0x80000000) ? 1 : -1];
";
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "xml-signature-status-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void XmlTransformGroup_PreservesNativeFlagBitsAndUnsignedStorage(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(XmlTransformDeclarations(), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Save(Path.Combine(fixture.DirectoryPath, "input.h"), "xml-transform-" + target);
            Save(fixture.GeneratedSource, "xml-transform-" + target);
            string path = fixture.Emit();
            Save(path, "xml-transform-" + target);
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var owner = NativeTypedefHeaderTests.Type(reader, "CRYPT_XML_TRANSFORM_INFO");
            var field = owner.GetFields().Select(reader.GetFieldDefinition)
                .Single(candidate => reader.GetString(candidate.Name) == "dwFlags");
            EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
            CollectionAssert.AreEqual(new[] { "CRYPT_XML_TRANSFORM_FLAGS" },
                EnumAssociationHeaderTests.StringAttributes(reader, field.GetCustomAttributes(), "AssociatedEnumAttribute"));
            var type = NativeTypedefHeaderTests.Type(reader, "CRYPT_XML_TRANSFORM_FLAGS");
            var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
            EnumAssociationHeaderTests.AssertSignature(reader,
                fields.Single(candidate => reader.GetString(candidate.Name) == "value__").Signature, 0, SignatureTypeCode.UInt32);
            var members = fields.Where(candidate => !candidate.GetDefaultValue().IsNil).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                "CRYPT_XML_TRANSFORM_ON_STREAM", "CRYPT_XML_TRANSFORM_ON_NODESET", "CRYPT_XML_TRANSFORM_URI_QUERY_STRING",
            }, members.Select(member => reader.GetString(member.Name)).ToArray());
            CollectionAssert.AreEqual(new uint[] { 1, 2, 4 },
                members.Select(member => reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value).ReadUInt32()).ToArray());
            var attributes = type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray();
            Assert.IsTrue(attributes.Contains("FlagsAttribute"));
            Assert.IsFalse(attributes.Contains("ScopedEnumAttribute"));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void XmlTransformGroup_PreservesCompleteNativeCAndCppRecordAndMasks(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = XmlTransformDeclarations() + @"
typedef char CheckTransformSize[(sizeof(CRYPT_XML_TRANSFORM_INFO) == (sizeof(void*) == 4 ? 20 : 32)) ? 1 : -1];
typedef char CheckSizeOffset[(((unsigned __int64)&(((CRYPT_XML_TRANSFORM_INFO*)0)->cbSize)) == 0) ? 1 : -1];
typedef char CheckAlgorithmOffset[(((unsigned __int64)&(((CRYPT_XML_TRANSFORM_INFO*)0)->wszAlgorithm)) == (sizeof(void*) == 4 ? 4 : 8)) ? 1 : -1];
typedef char CheckBufferOffset[(((unsigned __int64)&(((CRYPT_XML_TRANSFORM_INFO*)0)->cbBufferSize)) == (sizeof(void*) == 4 ? 8 : 16)) ? 1 : -1];
typedef char CheckFlagsOffset[(((unsigned __int64)&(((CRYPT_XML_TRANSFORM_INFO*)0)->dwFlags)) == (sizeof(void*) == 4 ? 12 : 20)) ? 1 : -1];
typedef char CheckCallbackOffset[(((unsigned __int64)&(((CRYPT_XML_TRANSFORM_INFO*)0)->pfnCreateTransform)) == (sizeof(void*) == 4 ? 16 : 24)) ? 1 : -1];
typedef char CheckFlagsWidth[(sizeof(((CRYPT_XML_TRANSFORM_INFO*)0)->dwFlags) == 4) ? 1 : -1];
typedef char CheckNativeTransformMask[((CRYPT_XML_TRANSFORM_ON_STREAM | CRYPT_XML_TRANSFORM_ON_NODESET |
    CRYPT_XML_TRANSFORM_URI_QUERY_STRING) == 7) ? 1 : -1];
typedef char CheckNativeQueryStringBit[(CRYPT_XML_TRANSFORM_URI_QUERY_STRING == 4 &&
    (CRYPT_XML_TRANSFORM_URI_QUERY_STRING & (CRYPT_XML_TRANSFORM_ON_STREAM | CRYPT_XML_TRANSFORM_ON_NODESET)) == 0) ? 1 : -1];
";
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            Save(Path.Combine(fixture.DirectoryPath, "abi.c"), "xml-transform-" + arch + "-" + language.TrimStart('/'));
            foreach (var original in new[] { fixture.OrdinaryNativeObject, fixture.MetadataNativeObject })
            {
                string path = Path.Combine(TestContext.ResultsDirectory,
                    "xml-transform-" + arch + "-" + language.TrimStart('/') + "-" + Path.GetFileName(original));
                File.Copy(original, path, true);
                TestContext.AddResultFile(path);
            }
        }

        private static string XmlTransformDeclarations()
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "cryptxml.h"));
            return @"
#include ""sal.h""
#define CALLBACK __stdcall
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef long HRESULT;
typedef const unsigned short *LPCWSTR;
typedef struct _CRYPT_XML_ALGORITHM CRYPT_XML_ALGORITHM;
typedef struct _CRYPT_XML_DATA_PROVIDER CRYPT_XML_DATA_PROVIDER;
" + Slice(header, @"typedef\s+_Success_\( return == 0 \)\s+HRESULT\s+\(CALLBACK \*PFN_CRYPT_XML_CREATE_TRANSFORM\)[\s\S]+?;") + "\n" +
                Slice(header, @"typedef struct _CRYPT_XML_TRANSFORM_INFO\{[\s\S]+?\}CRYPT_XML_TRANSFORM_INFO[^;]+;") + "\n" +
                Slice(header, @"(?m)^#define CRYPT_XML_TRANSFORM_ON_STREAM[\s\S]+?^#define CRYPT_XML_TRANSFORM_URI_QUERY_STRING[^\r\n]+") + "\n" +
                Regex.Match(header, @"(?m)^_Win32_FlagsEnum_\(CRYPT_XML_TRANSFORM_FLAGS,[^\r\n]+").Value + "\n";
        }

        private static string XmlSignatureStatusDeclarations()
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "cryptxml.h"));
            return @"
typedef unsigned long DWORD;
typedef unsigned long ULONG;
" + Slice(header, @"typedef struct _CRYPT_XML_STATUS\{[\s\S]+?\}CRYPT_XML_STATUS[^;]+;") + "\n" +
                Slice(header, @"(?m)^#define CRYPT_XML_STATUS_NO_ERROR[\s\S]+?^#define CRYPT_XML_STATUS_OPENED_TO_ENCODE[^\r\n]+") + "\n" +
                string.Join("\n", Regex.Matches(header, @"(?m)^_Win32_[^\r\n]*CRYPT_XML_STATUS_INFO_STATUS[^\r\n]*")
                    .Select(match => match.Value)) + "\n" +
                Regex.Match(header, @"(?m)^_Win32_FlagsEnum_\(CRYPT_XML_STATUS_ERROR_STATUS,[^\r\n]+").Value + "\n";
        }

        private static string ThemeAndDialogDeclarations()
        {
            string headers = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders");
            string theme = File.ReadAllText(Path.Combine(headers, "um", "Uxtheme.h"));
            string dialogs = File.ReadAllText(Path.Combine(headers, "um", "commdlg.h"));
            return @"
#define THEMEAPI_(type) type __stdcall
#define WINCOMMDLGAPI
#define APIENTRY __stdcall
#define VOID void
typedef unsigned long DWORD;
" + File.ReadAllText(Path.Combine(headers, "shared", "cderr.h")) + "\n" +
                Slice(theme, @"(?m)^#define STAP_ALLOW_NONCLIENT[\s\S]+?^#endif") + "\n" +
                Slice(theme, @"_Win32_AssociatedEnum_\(SET_THEME_APP_PROPERTIES_FLAGS\)\s+THEMEAPI_\(DWORD\)\s+GetThemeAppProperties\([^;]+;") + "\n" +
                Slice(theme, @"THEMEAPI_\(void\)\s+SetThemeAppProperties\([^;]+;") + "\n" +
                Slice(dialogs, @"_Win32_AssociatedEnum_\(COMMON_DLG_ERRORS\)\s+WINCOMMDLGAPI DWORD APIENTRY CommDlgExtendedError\([^;]+;");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void StructuredStorageGroup_DocumentationSidecarNeedsMetadataNamespaceImport(string target)
        {
            string header = File.ReadAllText(Sdk("um", "coml2api.h"));
            var constants = Regex.Matches(header, @"(?m)^#define (STGM_\w+)\s+[^\r\n]+")
                .Cast<Match>().ToArray();
            Assert.AreEqual(18, constants.Length);
            string source = string.Join("\n", constants.Select(match => match.Value)) +
                "\n_Win32_FlagsEnum_(STGM, unsigned long, " +
                string.Join(", ", constants.Select(match => match.Groups[1].Value)) + ");\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            // SAL fixtures install a global metadata using absent from production enum files.
            File.Copy(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "manual", "Metadata.cs"),
                Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "Attributes.manual.cs"));
            var namespaces = new Dictionary<string, string> { ["STGM"] = "Windows.Win32.System.Com" };
            using (var pe = new PEReader(File.OpenRead(fixture.Emit(requiredNamespaces: namespaces))))
            {
                var reader = pe.GetMetadataReader();
                var type = NativeTypedefHeaderTests.Type(reader, "STGM");
                Assert.AreEqual(19, type.GetFields().Count);
                var storage = type.GetFields().Select(reader.GetFieldDefinition)
                    .Single(field => reader.GetString(field.Name) == "value__");
                EnumAssociationHeaderTests.AssertSignature(reader, storage.Signature, 0, SignatureTypeCode.UInt32);
            }
            var remaps = new Dictionary<string, string>
            {
                ["STGM"] = "[Documentation(\"https://learn.microsoft.com/windows/win32/Stg/stgm-constants\")]",
            };
            var error = Assert.ThrowsException<AssertFailedException>(() => fixture.Emit(remaps, namespaces));
            TestContext.WriteLine(error.Message);
            StringAssert.Contains(error.Message, "CS0246");
            StringAssert.Contains(error.Message, "Documentation");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void SnmpGroup_UnsignedEnumPreservesSignedNativeScalarAndPointerParameters(string target)
        {
            string header = File.ReadAllText(Sdk("um", "WinSnmp.h"));
            string[] members = { "SNMP_PDU_GET", "SNMP_PDU_GETNEXT", "SNMP_PDU_RESPONSE",
                "SNMP_PDU_SET", "SNMP_PDU_GETBULK", "SNMP_PDU_TRAP" };
            string constants = string.Join("\n", members.Select(name =>
                Slice(header, @"(?m)^#define " + name + @"\s+[^\r\n]+")));
            string declaration = Slice(header, @"HSNMP_PDU\s+SNMPAPI_CALL\s+SnmpCreatePdu[^;]+;");
            string annotated = declaration.Replace("IN smiINT PDU_type",
                "IN _Win32_AssociatedEnum_(SNMP_PDU_TYPE) smiINT PDU_type");
            Assert.AreNotEqual(declaration, annotated);
            string getter = Slice(header, @"SNMPAPI_STATUS\s+SNMPAPI_CALL\s+SnmpGetPduData[^;]+;");
            string annotatedGetter = getter.Replace("OUT smiLPINT PDU_type",
                "OUT _Win32_AssociatedEnum_(SNMP_PDU_TYPE) smiLPINT PDU_type");
            Assert.AreNotEqual(getter, annotatedGetter);
            string source = @"
#define FAR
#define IN
#define OUT
#define SNMPAPI_CALL __stdcall
#define ULONG_MAX 4294967295U
#define ASN_CONTEXT 0x80
#define ASN_CONSTRUCTOR 0x20
typedef void* HSNMP_SESSION;
typedef void* HSNMP_PDU;
typedef void* HSNMP_VBL;
" + Slice(header, @"(?s)#if ULONG_MAX == 4294967295U.*?#endif") + @"
typedef smiUINT32 SNMPAPI_STATUS;
typedef HSNMP_VBL* LPHSNMP_VBL;
" + constants + "\n";
            string group = "_Win32_Enum_(SNMP_PDU_TYPE, {0}, " + string.Join(", ", members) + ");\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source + string.Format(group, "unsigned long") + annotated + "\n" + annotatedGetter,
                target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var type = NativeTypedefHeaderTests.Type(reader, "SNMP_PDU_TYPE");
            var storage = type.GetFields().Select(reader.GetFieldDefinition)
                .Single(field => reader.GetString(field.Name) == "value__");
            EnumAssociationHeaderTests.AssertSignature(reader, storage.Signature, 0, SignatureTypeCode.UInt32);
            Assert.AreEqual(7, type.GetFields().Count);
            Assert.AreEqual(0, type.GetCustomAttributes().Count, "UInt32, nonflags and unscoped.");
            foreach (var method in reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Where(method => reader.GetString(method.Name) is "SnmpCreatePdu" or "SnmpGetPduData"))
            {
                var parameter = method.GetParameters().Select(reader.GetParameter)
                    .Single(parameter => reader.GetString(parameter.Name) == "PDU_type");
                CollectionAssert.AreEqual(new[] { "SNMP_PDU_TYPE" },
                    EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }
            string declarations = EnumHeaderTests.Decompile(output);
            StringAssert.Contains(declarations, "int PDU_type");
            StringAssert.Contains(declarations, "int* PDU_type");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void CloudPolicyAssociation_RequiresAWrapperStorageContract(string target)
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "cfapi.h"));
            string source = "typedef unsigned short USHORT;\n" +
                Slice(header, @"(?ms)^#define DEFINE_USHORT_ENUM\(ENUMTYPE\).*?^  \};") + "\n" +
                Slice(header, @"typedef enum CF_HYDRATION_POLICY_PRIMARY\s*\{[^}]+\} CF_HYDRATION_POLICY_PRIMARY;") + "\n" +
                Slice(header, @"typedef enum CF_HYDRATION_POLICY_MODIFIER\s*\{[^}]+\} CF_HYDRATION_POLICY_MODIFIER;") + @"
DEFINE_USHORT_ENUM(CF_HYDRATION_POLICY_PRIMARY);
DEFINE_USHORT_ENUM(CF_HYDRATION_POLICY_MODIFIER);
" + Slice(header, @"typedef struct CF_HYDRATION_POLICY\s*\{[^}]+\} CF_HYDRATION_POLICY;");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            result = fixture.Scrape(source + @"
_Win32_AnnotateField_(CF_HYDRATION_POLICY::Primary,
    _Win32_AssociatedEnum_(CF_HYDRATION_POLICY_PRIMARY));
", target, sal: true);
            TestContext.WriteLine(result.Output);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output,
                "AssociatedEnum requires integer or native enum storage, optionally through pointers.");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void InstallerAndIconBindings_PreserveSignedStorageAndGenuineEnums(string target)
        {
            string root = Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um");
            string installer = File.ReadAllText(Path.Combine(root, "Msi.h"));
            string controls = File.ReadAllText(Path.Combine(root, "CommCtrl.h"));
            string[] names = { "MsiConfigureProductA", "MsiConfigureProductW", "MsiConfigureProductExA", "MsiConfigureProductExW" };
            string source = @"
#define WINAPI __stdcall
#define WINCOMMCTRLAPI
typedef unsigned int UINT;
typedef int HRESULT;
typedef void* HINSTANCE;
typedef void* HICON;
typedef const char* LPCSTR;
typedef const unsigned short* LPCWSTR;
typedef LPCWSTR PCWSTR;
" + Slice(installer, @"typedef enum tagINSTALLLEVEL\s*\{[^}]+\} INSTALLLEVEL;") + "\n" +
                Slice(installer, @"typedef enum tagINSTALLSTATE\s*\{[^}]+\} INSTALLSTATE;") + "\n" +
                string.Join("\n", names.Select(name => Slice(installer, @"UINT WINAPI " + name + @"\([^;]+;"))) + "\n" +
                Slice(controls, @"enum _LI_METRIC\s*\{[^}]+\};") + "\n" +
                Slice(controls, @"WINCOMMCTRLAPI HRESULT WINAPI LoadIconMetric\([^;]+;");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            byte[] expected = File.ReadAllBytes(path);
            string declarations = EnumHeaderTests.Decompile(path);
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                Assert.AreEqual(4, NativeTypedefHeaderTests.Type(reader, "INSTALLLEVEL").GetFields().Count);
                Assert.AreEqual(3, NativeTypedefHeaderTests.Type(reader, "_LI_METRIC").GetFields().Count);
                foreach (string name in names.Append("LoadIconMetric"))
                {
                    string parameterName = name == "LoadIconMetric" ? "lims" : "iInstallLevel";
                    string enumName = name == "LoadIconMetric" ? "_LI_METRIC" : "INSTALLLEVEL";
                    var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(method => reader.GetString(method.Name) == name);
                    var parameter = method.GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == parameterName);
                    CollectionAssert.AreEqual(new[] { enumName },
                        EnumAssociationHeaderTests.StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
                    Assert.IsTrue(Regex.IsMatch(declarations, @"\b" + name + @"\([^;]*\bint " + parameterName + @"\b"), name);
                    if (name != "LoadIconMetric")
                        Assert.IsTrue(Regex.IsMatch(declarations, @"\b" + name + @"\([^;]*\bINSTALLSTATE eInstallState\b"), name);
                }
            }
            var remaps = names.ToDictionary(name => name + "::iInstallLevel", _ => "[AssociatedEnum(\"INSTALLLEVEL\")]");
            remaps["LoadIconMetric::lims"] = "[AssociatedEnum(\"_LI_METRIC\")]";
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void PathOptions_NativeIntegerUsesCarryAssociationsInsteadOfEnumSubstitution(string target)
        {
            string header = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um", "PathCch.h"));
            string enumeration = Slice(header, @"typedef enum PATHCCH_OPTIONS\s*\{[^}]+\} PATHCCH_OPTIONS;");
            string[] names = { "PathCchCanonicalizeEx", "PathCchCombineEx", "PathCchAppendEx",
                "PathAllocCombine", "PathAllocCanonicalize" };
            string declarations = string.Join("\n", names.Select(name =>
                Slice(header, @"WINPATHCCHAPI\s+HRESULT\s+APIENTRY\s+" + name + @"\([^;]+;")));
            string source = @"
#define WINPATHCCHAPI
#define APIENTRY __stdcall
typedef unsigned long ULONG;
typedef int HRESULT;
typedef __SIZE_TYPE__ size_t;
typedef unsigned short* PWSTR;
typedef const unsigned short* PCWSTR;
" + enumeration + "\n" + declarations;
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            byte[] expected = File.ReadAllBytes(path);
            string outputDeclarations = EnumHeaderTests.Decompile(path);
            Save(path, "path-options-" + target);
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                Assert.AreEqual(9, NativeTypedefHeaderTests.Type(reader, "PATHCCH_OPTIONS").GetFields().Count);
                foreach (string name in names)
                {
                    var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(method => reader.GetString(method.Name) == name);
                    var flags = method.GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == "dwFlags");
                    CollectionAssert.AreEqual(new[] { "PATHCCH_OPTIONS" },
                        EnumAssociationHeaderTests.StringAttributes(reader, flags.GetCustomAttributes(), "AssociatedEnumAttribute"),
                        outputDeclarations);
                    Assert.IsTrue(Regex.IsMatch(outputDeclarations, @"\b" + name + @"\([^;]*\buint dwFlags\b"), name);
                }
            }
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(names.ToDictionary(
                name => name + "::dwFlags", _ => "[AssociatedEnum(\"PATHCCH_OPTIONS\")]"))));
            var conflict = Assert.ThrowsException<AggregateException>(() =>
                fixture.Emit(new Dictionary<string, string> { ["PathCchAppendEx::dwFlags"] = "PATHCCH_OPTIONS" }));
            StringAssert.Contains(conflict.Message, "conflicts with native AssociatedEnum storage");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ServiceNoChange_RealSdkMacrosStayGlobalAcrossRelatedGroups(string target)
        {
            string constants = ServiceConstants();
            string root = LastErrorHeaderTests.RepositoryRoot();
            string[] names = { "SERVICE_START_TYPE", "SERVICE_ERROR" };
            string sidecar = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "emitter.settings.rsp"));
            var remaps = names.ToDictionary(name => name, name =>
                Slice(sidecar, @"(?m)^" + name + @"=(?<value>[^\r\n]+)").Split('=', 2)[1]);
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(constants + Start + Error, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            string path = fixture.Emit();
            AssertService(path);
            Save(path, "service-" + target + "-header");
            byte[] header = File.ReadAllBytes(path);
            CollectionAssert.AreEqual(header, File.ReadAllBytes(fixture.Emit(remaps)), "Matching legacy must not mask header-only output.");
        }

        private static void AssertService(string path)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var group in new[] {
                (Name: "SERVICE_START_TYPE", Members: new[] { "SERVICE_BOOT_START", "SERVICE_SYSTEM_START", "SERVICE_AUTO_START", "SERVICE_DEMAND_START", "SERVICE_DISABLED" }),
                (Name: "SERVICE_ERROR", Members: new[] { "SERVICE_ERROR_IGNORE", "SERVICE_ERROR_NORMAL", "SERVICE_ERROR_SEVERE", "SERVICE_ERROR_CRITICAL" }) })
            {
                var type = NativeTypedefHeaderTests.Type(reader, group.Name);
                CollectionAssert.AreEqual(new[] { "SERVICE_NO_CHANGE" },
                    EnumAssociationHeaderTests.StringAttributes(reader, type.GetCustomAttributes(), "AssociatedConstantAttribute"));
                Assert.AreEqual(1, type.GetCustomAttributes().Count, "Unscoped, not flags.");
                var fields = type.GetFields().Select(reader.GetFieldDefinition).ToDictionary(field => reader.GetString(field.Name));
                EnumAssociationHeaderTests.AssertSignature(reader, fields["value__"].Signature, 0, SignatureTypeCode.UInt32);
                CollectionAssert.AreEquivalent(group.Members, fields.Keys.Where(name => name != "value__").ToArray());
                for (int i = 0; i < group.Members.Length; i++)
                {
                    var value = reader.GetConstant(fields[group.Members[i]].GetDefaultValue());
                    Assert.AreEqual(ConstantTypeCode.UInt32, value.TypeCode);
                    Assert.AreEqual((uint)i, reader.GetBlobReader(value.Value).ReadUInt32());
                }
            }
            var loose = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Single(field => reader.GetString(field.Name) == "SERVICE_NO_CHANGE");
            Assert.AreEqual("Apis", reader.GetString(reader.GetTypeDefinition(loose.GetDeclaringType()).Name));
            var constant = reader.GetConstant(loose.GetDefaultValue());
            Assert.AreEqual(ConstantTypeCode.UInt32, constant.TypeCode);
            Assert.AreEqual(uint.MaxValue, reader.GetBlobReader(constant.Value).ReadUInt32());
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void CompilerTypedLooseConstants_KeepWidthsSignednessAndNativeDeprecationLayout(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = ServiceConstants() + "\n#define SIGNED_LOW ((short)0x8001)\n#define WIDE_HIGH (1ULL << 63)\n" +
                "_Win32_AssociatedConstant_(MAXLONGLONG) _Win32_AssociatedConstant_(SIGNED_LOW) _Win32_AssociatedConstant_(WIDE_HIGH)\n" +
                "_Win32_Enum_(WidthProbe, unsigned long, SERVICE_BOOT_START);\n" + NativeRecords();
            var result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            string path = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var expected in new[] { (Name: "MAXLONGLONG", Type: ConstantTypeCode.Int64, Value: long.MaxValue),
                (Name: "SIGNED_LOW", Type: ConstantTypeCode.Int16, Value: (long)-32767) })
            {
                var field = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                    .Single(field => reader.GetString(field.Name) == expected.Name);
                var constant = reader.GetConstant(field.GetDefaultValue());
                Assert.AreEqual(expected.Type, constant.TypeCode);
                var value = reader.GetBlobReader(constant.Value);
                Assert.AreEqual(expected.Value, expected.Type == ConstantTypeCode.Int64 ? value.ReadInt64() : value.ReadInt16());
            }
            var wide = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Single(field => reader.GetString(field.Name) == "WIDE_HIGH");
            var wideConstant = reader.GetConstant(wide.GetDefaultValue());
            Assert.AreEqual(ConstantTypeCode.UInt64, wideConstant.TypeCode);
            Assert.AreEqual(0x8000000000000000UL, reader.GetBlobReader(wideConstant.Value).ReadUInt64());
            var packed = NativeTypedefHeaderTests.Type(reader, "Packed");
            Assert.AreEqual(TypeAttributes.SequentialLayout, packed.Attributes & TypeAttributes.LayoutMask);
            Assert.AreEqual(1, packed.GetLayout().PackingSize);
            var fields = packed.GetFields().Select(reader.GetFieldDefinition).ToArray();
            CollectionAssert.AreEqual(new[] { "tag", "word", "small" }, fields.Select(field => reader.GetString(field.Name)).ToArray());
            EnumAssociationHeaderTests.AssertSignature(reader, fields[0].Signature, 0, SignatureTypeCode.Byte);
            EnumAssociationHeaderTests.AssertSignature(reader, fields[1].Signature, 0, SignatureTypeCode.UInt32);
            EnumAssociationHeaderTests.AssertSignature(reader, fields[2].Signature, 0, SignatureTypeCode.UInt16);
            var deprecated = NativeTypedefHeaderTests.Type(reader, "_PACKEDEVENTINFO");
            var obsolete = deprecated.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ObsoleteAttribute");
            var blob = reader.GetBlobReader(obsolete.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            StringAssert.Contains(blob.ReadSerializedString()!, "struct PACKEDEVENTINFO is deprecated");
            Assert.AreEqual(0, NativeTypedefHeaderTests.Type(reader, "Ordinary").GetCustomAttributes()
                .Select(reader.GetCustomAttribute).Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ObsoleteAttribute"));
            Save(path, "width-layout-deprecation-" + target);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeSdkScopedEnums_RepeatedMatchingLegacyFactsKeepOneAttribute(string target)
        {
            var declarations = Regex.Matches(File.ReadAllText(Sdk("um", "dxcore_interface.h")),
                @"enum class (?<name>DXCore\w+) : uint32_t\s*\{[^}]+\};").Cast<Match>().ToArray();
            Assert.AreEqual(10, declarations.Length);
            string source = "typedef unsigned int uint32_t;\n" + string.Join("\n", declarations.Select(match => match.Value));
            string sidecar = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp"));
            var remaps = declarations.Select(match => match.Groups["name"].Value).ToDictionary(name => name, name =>
                string.Concat(Regex.Matches(sidecar, @"(?m)^" + name + @"=(?<value>[^\r\n]+)")
                    .Cast<Match>().Select(match => match.Groups["value"].Value)));
            Assert.AreEqual("[ScopedEnum][ScopedEnum]", remaps["DXCoreHardwareTypeFilterFlags"]);
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine("RAW_OUTPUT dxcore-scoped." + target + ".h " +
                Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(source)));
            Console.WriteLine("RAW_OUTPUT dxcore-scoped." + target + ".cs " +
                Convert.ToBase64String(File.ReadAllBytes(fixture.GeneratedSource)));
            Console.WriteLine("RAW_OUTPUT dxcore-scoped." + target + ".legacy.rsp " +
                Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Join("\n",
                    remaps.Select(remap => remap.Key + "=" + remap.Value)))));
            string path = fixture.Emit();
            byte[] expected = File.ReadAllBytes(path);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)),
                "Actual repeated legacy scope records must not conflict with one compiler-owned fact.");
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var declaration in declarations)
            {
                string name = declaration.Groups["name"].Value;
                var type = NativeTypedefHeaderTests.Type(reader, name);
                var attributes = type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray();
                Assert.AreEqual(1, attributes.Count(attribute => attribute == "ScopedEnumAttribute"), name);
                Assert.AreEqual(0, attributes.Count(attribute => attribute == "FlagsAttribute"),
                    "Names and DEFINE_ENUM_FLAG_OPERATORS are not compiler flag_enum facts: " + name);
                var storage = type.GetFields().Select(reader.GetFieldDefinition)
                    .Single(field => reader.GetString(field.Name) == "value__");
                EnumAssociationHeaderTests.AssertSignature(reader, storage.Signature, 0, SignatureTypeCode.UInt32);
            }
            Save(path, "dxcore-scoped-" + target);
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void NativeSdkPilots_EnumHelpersAndAssociationsAreAbiNoOps(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = ServiceConstants() + Start + Error + NativeRecords() +
                VartypeDeclarations(File.ReadAllText(Sdk("um", "oaidl.h")), File.ReadAllText(Sdk("shared", "wtypes.h"))) + @"
_Win32_AnnotateField_(TYPEDESC::vt, _Win32_AssociatedEnum_(VARENUM));
struct VariantUse { _Win32_AssociatedEnum_(VARENUM) VARTYPE vt; VARTYPE neighbor; };
typedef char CheckPacked[(sizeof(struct Packed) == 7) ? 1 : -1];
typedef char CheckOrdinary[(sizeof(struct Ordinary) == 8) ? 1 : -1];
typedef char CheckVariant[(sizeof(struct VariantUse) == 4) ? 1 : -1];
typedef char CheckNoChange[(sizeof(SERVICE_NO_CHANGE) == 4 && SERVICE_NO_CHANGE > 0) ? 1 : -1];
typedef char CheckLongLong[(sizeof(MAXLONGLONG) == 8 && MAXLONGLONG > 0) ? 1 : -1];
VARTYPE __stdcall NativeVariant(_Win32_AssociatedEnum_(VARENUM) VARTYPE** value);
typedef VARTYPE (__stdcall *ExpectedVariant)(VARTYPE**);
ExpectedVariant verifyVariant = &NativeVariant;
" + $"typedef char CheckTypeDesc[(sizeof(TYPEDESC) == {pointerSize * 2} && sizeof(((TYPEDESC*)0)->vt) == 2) ? 1 : -1];\n";
            fixture.AssertNativeAbi(arch, pointerSize, language, source);
            string path = Path.Combine(TestContext.ResultsDirectory, "native-enum-" + arch + "-" + language.TrimStart('/') + ".obj");
            File.Copy(fixture.NativeObject, path, true);
            TestContext.AddResultFile(path);
        }

        internal static string VartypeDeclarations(string oaidl, string wtypes) =>
            Slice(wtypes, @"typedef unsigned short VARTYPE;") + "\n" +
            Slice(wtypes, @"enum VARENUM\s*\{[^}]+\}\s*;") + "\n" +
            "#define DUMMYUNIONNAME\n typedef unsigned long HREFTYPE;\n" +
            "typedef unsigned long ULONG; typedef long LONG; typedef unsigned short USHORT;\n" +
            Slice(oaidl, @"typedef struct tagSAFEARRAYBOUND\s*\{[^}]+\}\s*SAFEARRAYBOUND;") + "\n" +
            Slice(oaidl, @"typedef struct tagTYPEDESC\s*\{[\s\S]+?\}\s*TYPEDESC;") + "\n" +
            Slice(oaidl, @"typedef struct tagARRAYDESC\s*\{[^}]+\}\s*ARRAYDESC;") + "\n";

        private static string ServiceConstants()
        {
            string winnt = File.ReadAllText(Sdk("um", "winnt.h"));
            string winsvc = File.ReadAllText(Sdk("um", "winsvc.h"));
            return Slice(winsvc, @"(?m)^#define SERVICE_NO_CHANGE\s+[^\r\n]+") + "\n" +
                string.Join("\n", new[] { "SERVICE_BOOT_START", "SERVICE_SYSTEM_START", "SERVICE_AUTO_START",
                    "SERVICE_DEMAND_START", "SERVICE_DISABLED", "SERVICE_ERROR_IGNORE", "SERVICE_ERROR_NORMAL",
                    "SERVICE_ERROR_SEVERE", "SERVICE_ERROR_CRITICAL", "MAXLONGLONG" }
                    .Select(name => Slice(winnt, @"(?m)^#define " + name + @"\s+[^\r\n]+"))) + "\n";
        }

        private static string NativeRecords() => @"
typedef unsigned long DWORD;
struct
" + Slice(File.ReadAllText(Sdk("um", "winnt.h")),
                @"__declspec\(deprecated\(""struct PACKEDEVENTINFO[^""]+""\)\)\s*#endif\s*_PACKEDEVENTINFO\s*\{[^}]+\};")
                .Replace("#endif", "", StringComparison.Ordinal) + @"
#pragma pack(push, 1)
struct Packed { unsigned char tag; unsigned long word; unsigned short small; };
#pragma pack(pop)
struct Ordinary { unsigned char tag; unsigned long word; };
";

        private static string Sdk(string area, string name) =>
            Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0", area, name);

        private static string Slice(string text, string pattern)
        {
            Match match = Regex.Match(text, pattern);
            Assert.IsTrue(match.Success, pattern);
            return match.Value;
        }

        private void Save(string path, string name)
        {
            string proof = Path.Combine(TestContext.ResultsDirectory, name + Path.GetExtension(path));
            File.Copy(path, proof, true);
            TestContext.AddResultFile(proof);
        }
    }
}
