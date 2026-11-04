using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;
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
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ServiceNoChange_RealSdkMacrosStayGlobalAcrossRelatedGroups(string target)
        {
            string constants = ServiceConstants();
            string root = LastErrorHeaderTests.RepositoryRoot();
            string[] names = { "SERVICE_START_TYPE", "SERVICE_ERROR" };
            using var json = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "enums.json")));
            foreach (string name in names)
            {
                var legacy = json.RootElement.GetProperty("items").EnumerateArray()
                    .Single(item => item.TryGetProperty("name", out var candidate) && candidate.GetString() == name);
                Assert.AreEqual(name == "SERVICE_START_TYPE" ? 5 : 4, legacy.GetProperty("members").GetArrayLength());
            }
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
            string proof = Path.Combine(TestContext.ResultsDirectory, name + ".winmd");
            File.Copy(path, proof, true);
            TestContext.AddResultFile(proof);
        }
    }
}
