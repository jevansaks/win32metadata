using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class EnumHeaderTests
    {
        private const string Constants = "#define Low 1\n#define High 0x80000000UL\n#define Both (Low | High)\n";
        private const string Groups = "_Win32_FlagsEnum_(Options, unsigned long, Low, High, Both);\n";
        private const string Legacy = "{\"items\":[{\"namespace\":\"Test\",\"name\":\"Options\",\"flags\":true,\"members\":[{\"name\":\"Low\"},{\"name\":\"High\"},{\"name\":\"Both\"}]}]}";

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void PackagedScraper_HasItsRuntimeDependencies(bool headerGroup)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string root = LastErrorHeaderTests.RepositoryRoot();
            string package = Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "package");
            var manifest = XDocument.Load(Path.Combine(root, "sources", "GeneratorSdk", "nuget", "BuildSdk.nuspec"));
            foreach (var file in manifest.Descendants("file").Where(file =>
                ((string)file.Attribute("target")!).TrimEnd('\\').Equals(@"tools\net10.0\win-x64", StringComparison.OrdinalIgnoreCase)))
            {
                string source = Path.Combine(AppContext.BaseDirectory, Path.GetRelativePath(
                    Path.Combine(root, "bin", "release", "net10.0"), Path.Combine(root, (string)file.Attribute("src")!)));
                string target = Path.Combine(package, (string)file.Attribute("target")!);
                Directory.CreateDirectory(target);
                string[] inputs = Directory.GetFiles(Path.GetDirectoryName(source)!, Path.GetFileName(source));
                Assert.IsTrue(inputs.Length > 0, "Missing package input: " + source);
                foreach (string input in inputs)
                    File.Copy(input, Path.Combine(target, Path.GetFileName(input)), true);
            }
            string scraper = Path.Combine(package, "tools", "net10.0", "win-x64", "Win32MetadataScraper.dll");
            var result = fixture.Scrape(Constants + (headerGroup ? Groups : "") +
                "int __stdcall Example(void);", sal: true, scraperPath: scraper);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string declarations = Decompile(fixture.Emit());
            StringAssert.Contains(declarations, "extern int Example");
            Assert.AreEqual(headerGroup, declarations.Contains("enum Options", StringComparison.Ordinal));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void HeaderOnlyAndMatchingLegacy_EmitExactFlagsAndUses(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Groups +
                "struct ExampleRecord { _Win32_AssociatedEnum_(Options) unsigned long option; };\n" +
                "int __stdcall Example(_Win32_AssociatedEnum_(Options) unsigned long option);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            string path = fixture.Emit();
            byte[] header = File.ReadAllBytes(path);
            string expected = Decompile(path);
            fixture.ScrapeConstants(Legacy);
            string actual = Decompile(fixture.Emit());
            Assert.IsTrue(expected == actual, "Missing: " + string.Join("\n", expected.Split('\n').Except(actual.Split('\n'))) +
                "\nAdded: " + string.Join("\n", actual.Split('\n').Except(expected.Split('\n'))));
            using var pe = new PEReader(new MemoryStream(header));
            var reader = pe.GetMetadataReader();
            var type = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Single(t => reader.GetString(t.Name) == "Options");
            var fields = type.GetFields().Select(reader.GetFieldDefinition).ToArray();
            var storage = reader.GetBlobReader(fields.Single(f => reader.GetString(f.Name) == "value__").Signature);
            storage.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.UInt32, storage.ReadSignatureTypeCode());
            CollectionAssert.AreEqual(new[] { 1u, 0x80000000u, 0x80000001u },
                fields.Where(f => !f.GetDefaultValue().IsNil).Select(f => reader.GetBlobReader(
                    reader.GetConstant(f.GetDefaultValue()).Value).ReadUInt32()).ToArray());
            Assert.AreEqual(1, type.GetCustomAttributes().Count);
            Assert.AreEqual("FlagsAttribute", AttributeName(reader, reader.GetCustomAttribute(type.GetCustomAttributes().Single())));
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(1, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.TypeHandle, signature.ReadSignatureTypeCode());
            Assert.AreEqual("Options", TypeName(reader, signature.ReadTypeHandle()));
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Any(t => reader.GetString(t.Name).Contains("Win32Metadata.", StringComparison.Ordinal)));
        }

        [DataTestMethod]
        [DataRow("signed char", "sbyte", "-128", SignatureTypeCode.SByte, "-128")]
        [DataRow("unsigned char", "byte", "255", SignatureTypeCode.Byte, "255")]
        [DataRow("short", "short", "-32768", SignatureTypeCode.Int16, "-32768")]
        [DataRow("unsigned short", "ushort", "65535", SignatureTypeCode.UInt16, "65535")]
        [DataRow("int", "int", "-2147483647 - 1", SignatureTypeCode.Int32, "-2147483648")]
        [DataRow("unsigned long", "uint", "0xffffffffUL", SignatureTypeCode.UInt32, "4294967295")]
        [DataRow("long long", "long", "(-9223372036854775807LL - 1)", SignatureTypeCode.Int64, "-9223372036854775808")]
        [DataRow("unsigned long long", "ulong", "0xffffffffffffffffULL", SignatureTypeCode.UInt64, "18446744073709551615")]
        public void NativeStorageAndSignedFlags_AreNotPromoted(string native, string managed, string value, SignatureTypeCode expected, string expectedValue)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#define Value (" + value + ")\n_Win32_Enum_(Scalar, " + native +
                ", Value);\n_Win32_FlagsEnum_(Flags, " + native + ", Value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            using var pe = new PEReader(new MemoryStream(File.ReadAllBytes(fixture.Emit())));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "Scalar", "Flags" })
            {
                var type = reader.TypeDefinitions.Select(reader.GetTypeDefinition).Single(t => reader.GetString(t.Name) == name);
                var field = type.GetFields().Select(reader.GetFieldDefinition).Single(f => reader.GetString(f.Name) == "value__");
                var signature = reader.GetBlobReader(field.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(expected, signature.ReadSignatureTypeCode(), managed);
                var member = type.GetFields().Select(reader.GetFieldDefinition).Single(f => reader.GetString(f.Name) == "Value");
                Assert.AreEqual((ConstantTypeCode)expected, reader.GetConstant(member.GetDefaultValue()).TypeCode);
                var constant = reader.GetBlobReader(reader.GetConstant(member.GetDefaultValue()).Value);
                object numeric = expected switch {
                    SignatureTypeCode.SByte => constant.ReadSByte(),
                    SignatureTypeCode.Byte => constant.ReadByte(),
                    SignatureTypeCode.Int16 => constant.ReadInt16(),
                    SignatureTypeCode.UInt16 => constant.ReadUInt16(),
                    SignatureTypeCode.Int32 => constant.ReadInt32(),
                    SignatureTypeCode.UInt32 => constant.ReadUInt32(),
                    SignatureTypeCode.Int64 => constant.ReadInt64(),
                    _ => constant.ReadUInt64(),
                };
                Assert.AreEqual(expectedValue, Convert.ToString(numeric, System.Globalization.CultureInfo.InvariantCulture));
                Assert.AreEqual(name == "Flags" ? 1 : 0, type.GetCustomAttributes().Count);
                if (name == "Flags")
                    Assert.AreEqual("FlagsAttribute", AttributeName(reader, reader.GetCustomAttribute(type.GetCustomAttributes().Single())));
            }
        }

        [DataTestMethod]
        [DataRow("_Win32_Enum_(Options, float, Low);", "native integer storage")]
        [DataRow("_Win32_Enum_(Options, unsigned long, Low, Low);", "unique constituent")]
        [DataRow("_Win32_Enum_(Options, unsigned long, Missing);", "Missing")]
        [DataRow("_Win32_Enum_(Options, unsigned long, Low + 1);", "constituent name")]
        [DataRow("typedef __attribute__((annotate(\"Win32Metadata.Enum(Low,High)\", Low))) unsigned long Options;", "constituent name")]
        [DataRow("typedef __attribute__((annotate(\"Win32Metadata.Enum(Low)\", Low, High))) unsigned long Options;", "constituent name")]
        [DataRow("typedef __attribute__((annotate(\"Win32Metadata.Enum(Low)\", Low), annotate(\"Win32Metadata.FlagsEnum(Low)\", Low))) unsigned long Options;", "conflicting")]
        [DataRow("_Win32_Enum_(Options, unsigned long, Low);\nint __stdcall Example(_Win32_AssociatedEnum_(Options) unsigned short value);", "storage conflicts")]
        [DataRow("namespace Scope { _Win32_Enum_(Options, unsigned long, Low); }", "unscoped")]
        [DataRow("#define Floating 1.5\n_Win32_Enum_(Options, unsigned long, Floating);", "integer constant")]
        public void InvalidGroups_FailAtNativeSource(string group, string error)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + group, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output.ToLowerInvariant(), error.ToLowerInvariant());
        }

        [TestMethod]
        public void EmptyConstituents_AreExplicitCompilerErrors()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("_Win32_Enum_(Options, unsigned long, );");
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "error: expected expression");
        }

        [DataTestMethod]
        [DataRow("\"flags\":false")]
        [DataRow("\"flags\":true,\"type\":\"ushort\"")]
        [DataRow("\"flags\":true,\"members\":[{\"name\":\"Low\",\"value\":\"2\"}]")]
        [DataRow("\"flags\":true,\"autoPopulate\":{\"filter\":\".*\",\"header\":\"input.h\"}")]
        public void ConflictingLegacyDefinition_CannotOverwriteHeader(string facts)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Groups, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<FormatException>(() =>
                fixture.ScrapeConstants("{\"items\":[{\"name\":\"Options\"," + facts + "}]}"));
            StringAssert.Contains(error.Message, "Options");
        }

        [TestMethod]
        public void LegacyOnly_StillEmitsWithoutHeaderFallback()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + "int __stdcall Example(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants(Legacy);
            using var pe = new PEReader(new MemoryStream(File.ReadAllBytes(fixture.Emit())));
            var reader = pe.GetMetadataReader();
            Assert.IsTrue(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(t => reader.GetString(t.Name) == "Options"));
            result = fixture.Scrape(Constants, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            using var emptyPe = new PEReader(File.OpenRead(fixture.Emit()));
            var emptyReader = emptyPe.GetMetadataReader();
            Assert.IsFalse(emptyReader.TypeDefinitions.Select(emptyReader.GetTypeDefinition).Any(t => emptyReader.GetString(t.Name) == "Options"));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedWinHttpSlices_HeaderOnlyMatchesRetiredGroupsAndUses(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string root = LastErrorHeaderTests.RepositoryRoot();
            string owner = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", "winhttp.h"));
            string[] names = { "WINHTTP_OPEN_REQUEST_FLAGS", "WINHTTP_ACCESS_TYPE" };
            string groups = string.Join("\n", names.Select(name =>
                Regex.Match(owner, @"_Win32_(?:FlagsEnum|Enum)_\(" + name + @",[\s\S]*?\);").Value));
            Assert.IsTrue(names.All(name => groups.Contains(name, StringComparison.Ordinal)));
            string[] members = Regex.Matches(groups, @"WINHTTP_(?:FLAG_[A-Z_]+|ACCESS_TYPE_[A-Z_]+)")
                .Select(match => match.Value).Distinct().ToArray();
            string defines = string.Join("\n", members.Select(member =>
                Regex.Match(owner, @"(?m)^#define " + member + @"\s+[^\r\n]*").Value));
            string record = Regex.Match(owner, @"typedef struct _WINHTTP_PROXY_INFO[\s\S]*?\} WINHTTP_PROXY_INFO,[^;]+;").Value;
            string functions = string.Join("\n", new[] { "WinHttpOpen", "WinHttpOpenRequest" }.Select(name =>
                Regex.Match(owner, @"WINHTTPAPI\s+HINTERNET\s+WINAPI\s+" + name + @"\s*\([\s\S]*?\);").Value));
            Assert.IsTrue(functions.Contains("dwAccessType", StringComparison.Ordinal));
            Assert.IsTrue(functions.Contains("_Win32_AssociatedEnum_(WINHTTP_OPEN_REQUEST_FLAGS)", StringComparison.Ordinal));
            string source = "#include <windows.h>\n#define WINHTTPAPI DECLSPEC_IMPORT\n" +
                Regex.Match(owner, @"typedef LPVOID HINTERNET;").Value + "\n" + defines + "\n" + groups + "\n" + record + "\n" + functions;
            string sdk = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            var vc = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; Get-VcDirPath -Arch x64 -HostArch x64");
            Assert.AreEqual(0, vc.ExitCode, vc.Output);
            string vcInclude = Path.GetFullPath(Path.Combine(vc.Output.Trim(), "..", "..", "..", "include"));
            string settings = "--include-directory\n" + string.Join("\n", new[] {
                Path.Combine(sdk, "um"), Path.Combine(sdk, "shared"), Path.Combine(sdk, "ucrt"), vcInclude });
            var result = fixture.Scrape(source, target, sal: true, extraSettings: settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            string expected = Decompile(fixture.Emit());
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/EnumHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the enum fixture.");
            var legacyJson = LastErrorHeaderTests.Run("git", "-C", root, "show",
                commits[0] + "^:generation/WinSDK/enums.json");
            Assert.AreEqual(0, legacyJson.ExitCode, legacyJson.Output);
            using var document = JsonDocument.Parse(legacyJson.Output);
            string retired = "{\"items\":[" + string.Join(",", document.RootElement.GetProperty("items")
                .EnumerateArray().Where(item => item.TryGetProperty("name", out var name) && names.Contains(name.GetString()))
                .Select(item => item.GetRawText())) + "]}";
            string legacySource = Regex.Replace(source.Replace(groups, ""), @"_Win32_AssociatedEnum_\([^)]+\)\s*", "");
            result = fixture.Scrape(legacySource, target, sal: true, extraSettings: settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var remaps = fixture.ScrapeConstants(retired);
            Assert.AreEqual(3, remaps.Count, "All three retired use sites must be exercised.");
            Assert.AreEqual(expected, Decompile(fixture.Emit(remaps)));
            string currentConfig = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "enums.json"));
            Assert.IsTrue(names.All(name => !currentConfig.Contains("\"" + name + "\"", StringComparison.Ordinal)));
        }

        [DataTestMethod]
        [DataRow("c")]
        [DataRow("c++")]
        public void HelpersAreNativeNoOpsAndTransitiveMetadataUsesRemainUnscoped(string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = "#pragma once\n" + Constants + Groups +
                "int __stdcall Example(_Win32_AssociatedEnum_(Options) unsigned long value);";
            var result = fixture.Scrape(source, metadata: false, language: language, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using (var pe = new PEReader(File.OpenRead(fixture.Emit())))
            {
                var reader = pe.GetMetadataReader();
                Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(t => reader.GetString(t.Name) == "Options"));
            }
            result = fixture.Scrape(source, language: language, sal: true, transitive: true, repeatInclude: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var metadata = new PEReader(File.OpenRead(fixture.Emit()));
            var metadataReader = metadata.GetMetadataReader();
            Assert.AreEqual(1, metadataReader.TypeDefinitions.Select(metadataReader.GetTypeDefinition)
                .Count(t => metadataReader.GetString(t.Name) == "Options"));
        }

        [TestMethod]
        public void GroupReturnsDirectAliasesAndPointerDepth_PreserveExactSignatures()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Groups + @"
enum class NativeScoped : unsigned short { NativeValue = 7 };
_Win32_AssociatedEnum_(Options) unsigned long __stdcall Associated(_Win32_AssociatedEnum_(Options) unsigned long* value);
Options __stdcall Direct(Options** value);
int __stdcall ConstInput(_Win32_AssociatedEnum_(Options) const unsigned long* value);
NativeScoped __stdcall Existing(NativeScoped value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string declarations = Decompile(fixture.Emit());
            StringAssert.Contains(declarations, "extern Options Associated");
            StringAssert.Contains(declarations, "Options* value");
            StringAssert.Contains(declarations, "extern Options Direct");
            StringAssert.Contains(declarations, "Options** value");
            StringAssert.Contains(declarations, "ConstInput");
            StringAssert.Contains(declarations, "enum NativeScoped : ushort");
            StringAssert.Contains(declarations, "NativeValue = 7");
        }

        [DataTestMethod]
        [DataRow("ushort")]
        [DataRow("[AssociatedEnum(\"Different\")]")]
        public void ConflictingUseSiteRemap_CannotOverrideHeaderGroup(string remap)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Groups +
                "int __stdcall Example(_Win32_AssociatedEnum_(Options) unsigned long value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<AggregateException>(() =>
                fixture.Emit(new Dictionary<string, string> { ["Example::value"] = remap }));
            StringAssert.Contains(error.Message, "conflicts with header enum group");
        }

        [TestMethod]
        public void MatchingLegacyAssociation_DoesNotDuplicateHeaderType()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Groups +
                "_Win32_AssociatedEnum_(Options) unsigned long __stdcall Example(_Win32_AssociatedEnum_(Options) unsigned long value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string expected = Decompile(fixture.Emit());
            var remaps = new Dictionary<string, string> {
                ["Example::value"] = "[AssociatedEnum(\"Options\")]",
                ["Example::return"] = "[AssociatedEnum(\"Options\")]",
            };
            Assert.AreEqual(expected, Decompile(fixture.Emit(remaps)));
        }

        [TestMethod]
        public void SharedConstituents_RemainExactInEveryGroupAndOtherConstants()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Groups + @"
#define SharedHigh High
#define SharedCombined Both
_Win32_Enum_(Other, unsigned long, High);
int __stdcall Example(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            string path = fixture.Emit();
            string header = Decompile(path);
            using var pe = new PEReader(new MemoryStream(File.ReadAllBytes(path)));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "SharedHigh", "SharedCombined" })
            {
                var field = reader.FieldDefinitions.Select(reader.GetFieldDefinition).Single(f => reader.GetString(f.Name) == name);
                Assert.IsFalse(field.GetDefaultValue().IsNil);
                Assert.AreEqual(name == "SharedHigh" ? 0x80000000u : 0x80000001u,
                    reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32());
            }
            Assert.AreEqual(2, reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Count(t => new[] { "Options", "Other" }.Contains(reader.GetString(t.Name))));
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Where(t => reader.GetString(t.Name) == "Apis").SelectMany(t => t.GetFields())
                .Select(reader.GetFieldDefinition).Any(f => new[] { "Low", "High", "Both" }.Contains(reader.GetString(f.Name))));
            fixture.ScrapeConstants(Legacy);
            Assert.AreEqual(header, Decompile(fixture.Emit()));
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void LegacyEnumReferencesAndManualExpressions_PreserveExistingBinding(bool headerGroup)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + (headerGroup ? Groups : "") +
                "int __stdcall Example(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string manual = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "manual", "WinSock.cs"));
            string pollin = Regex.Match(manual, @"public const ushort POLLIN = [^;]+;").Value;
            Assert.IsFalse(string.IsNullOrEmpty(pollin));
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "Legacy.enums.cs"), @"
using System;
using static Legacy.Foundation.Apis;
using static Legacy.Foundation.WAIT_EVENT;
namespace Legacy.Foundation {
    public enum WAIT_EVENT : uint { WAIT_OBJECT_0 = 0, WAIT_IO_COMPLETION = 192 }
    public static partial class Apis {
        public const uint WAIT_OBJECT_0 = 0;
        public const uint WAIT_IO_COMPLETION = 192;
        public const int StillPublic = 42;
    }
}
namespace Legacy.WinSock {
    public enum WSA_ERROR {
        WSA_WAIT_EVENT_0 = (int)(WAIT_OBJECT_0),
        WSA_WAIT_IO_COMPLETION = (int)(WAIT_IO_COMPLETION)
    }
    [Flags] public enum WSAPOLL_EVENT_FLAGS : short {
        POLLRDNORM = 0x0100, POLLRDBAND = 0x0200, POLLIN = (POLLRDNORM | POLLRDBAND)
    }
    public static partial class Apis { " + pollin + @" }
}");
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var types = reader.TypeDefinitions.Select(reader.GetTypeDefinition).ToArray();
            var error = types.Single(t => reader.GetString(t.Name) == "WSA_ERROR");
            CollectionAssert.AreEqual(new[] { 0, 192 }, error.GetFields().Select(reader.GetFieldDefinition)
                .Where(f => !f.GetDefaultValue().IsNil).Select(f => reader.GetBlobReader(
                    reader.GetConstant(f.GetDefaultValue()).Value).ReadInt32()).ToArray());
            var flags = types.Single(t => reader.GetString(t.Name) == "WSAPOLL_EVENT_FLAGS");
            var field = flags.GetFields().Select(reader.GetFieldDefinition).Single(f => reader.GetString(f.Name) == "POLLIN");
            Assert.AreEqual((short)0x0300, reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadInt16());
            var apiFields = types.Where(t => reader.GetString(t.Name) == "Apis").SelectMany(t => t.GetFields())
                .Select(reader.GetFieldDefinition).Select(f => reader.GetString(f.Name)).ToArray();
            Assert.IsTrue(apiFields.Contains("StillPublic"));
            Assert.IsFalse(apiFields.Any(name => name is "WAIT_OBJECT_0" or "WAIT_IO_COMPLETION" or "POLLIN"));
        }

        private static string Decompile(string path) =>
            string.Join("\n", CSharpSyntaxTree.ParseText(new CSharpDecompiler(path,
                new DecompilerSettings { ThrowOnAssemblyResolveErrors = false }).DecompileWholeModuleAsString())
                .GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .OrderBy(type => type.Identifier.ValueText, StringComparer.Ordinal)
                .Select(type => type.NormalizeWhitespace().ToFullString()));

        private static string TypeName(MetadataReader reader, EntityHandle handle) =>
            reader.GetString(handle.Kind == HandleKind.TypeDefinition ?
                reader.GetTypeDefinition((TypeDefinitionHandle)handle).Name :
                reader.GetTypeReference((TypeReferenceHandle)handle).Name);

        private static string AttributeName(MetadataReader reader, CustomAttribute attribute) =>
            TypeName(reader, attribute.Constructor.Kind == HandleKind.MethodDefinition ?
                reader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor).GetDeclaringType() :
                reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent);
    }
}
