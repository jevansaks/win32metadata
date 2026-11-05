using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class EnumAssociationHeaderTests
    {
        private const string Constants = "#define FIRST 1\n#define SECOND 2\n#define LOOSE 0xffffffffUL\n#define OTHER 0x80000000UL\n";
        private const string Associations = "_Win32_AssociatedConstant_(LOOSE) _Win32_AssociatedConstant_(OTHER) ";
        private const string Groups = "_Win32_Enum_(First, unsigned long, FIRST);\n_Win32_FlagsEnum_(Second, unsigned long, SECOND);\n";
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void RepeatedLooseConstants_NativeAndGroupEnumsKeepGlobalsAndLegacyCompatibility(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? expected = null;
            string? expectedDeclarations = null;
            foreach (var mode in new[] { (Header: true, Legacy: false), (Header: true, Legacy: true),
                (Header: false, Legacy: true), (Header: false, Legacy: false) })
            {
                string source = Constants + (mode.Header ? Groups.Replace("_Win32_", Associations + "_Win32_") : Groups) +
                    "enum " + (mode.Header ? Associations : "") + "Native { NATIVE_VALUE = 3 };\n";
                var result = fixture.Scrape(source, target, sal: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                fixture.ScrapeConstants();
                var remaps = new Dictionary<string, string>();
                if (mode.Legacy)
                    foreach (string name in new[] { "First", "Second", "Native" })
                        remaps.Add(name, "[AssociatedConstant(\"LOOSE\")][AssociatedConstant(\"OTHER\")]");
                string output = fixture.Emit(remaps);
                Save(output, "loose-" + target + "-" + mode.Header + "-" + mode.Legacy);
                using var pe = new PEReader(File.OpenRead(output));
                var reader = pe.GetMetadataReader();
                foreach (string name in new[] { "First", "Second", "Native" })
                {
                    var type = NativeTypedefHeaderTests.Type(reader, name);
                    var values = StringAttributes(reader, type.GetCustomAttributes(), "AssociatedConstantAttribute");
                    CollectionAssert.AreEqual(mode.Header || mode.Legacy ? new[] { "LOOSE", "OTHER" } : Array.Empty<string>(), values,
                        name + " " + mode + ": " + string.Join(",", values));
                    Assert.IsFalse(type.GetFields().Select(reader.GetFieldDefinition)
                        .Any(field => reader.GetString(field.Name) is "LOOSE" or "OTHER"));
                    Assert.IsFalse(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                        .Any(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ScopedEnumAttribute"));
                }
                foreach (string name in new[] { "LOOSE", "OTHER" })
                {
                    var field = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                        .Single(field => reader.GetString(field.Name) == name);
                    Assert.AreEqual("Apis", reader.GetString(reader.GetTypeDefinition(field.GetDeclaringType()).Name));
                    var value = reader.GetConstant(field.GetDefaultValue());
                    Assert.AreEqual(ConstantTypeCode.UInt32, value.TypeCode);
                    Assert.AreEqual(name == "LOOSE" ? uint.MaxValue : 0x80000000u,
                        reader.GetBlobReader(value.Value).ReadUInt32());
                }
                if (mode.Header || mode.Legacy)
                {
                    if (expected == null)
                    {
                        expected = File.ReadAllBytes(output);
                        expectedDeclarations = EnumHeaderTests.Decompile(output);
                    }
                    else if (mode.Header) CollectionAssert.AreEqual(expected, File.ReadAllBytes(output), mode.ToString());
                    else Assert.AreEqual(expectedDeclarations, EnumHeaderTests.Decompile(output), mode.ToString());
                }
            }
        }

        [DataTestMethod]
        [DataRow("[AssociatedConstant(\"DIFFERENT\")]")]
        [DataRow("[AssociatedConstant(\"LOOSE\")]")]
        [DataRow("[AssociatedConstant(\"LOOSE\")][AssociatedConstant(\"LOOSE\")]")]
        [DataRow("[-AssociatedConstant]")]
        public void ConflictingLegacyLooseAssociations_CannotReplaceNativeSet(string legacy)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + Associations + "_Win32_Enum_(First, unsigned long, FIRST);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<AggregateException>(() =>
                fixture.Emit(new Dictionary<string, string> { ["First"] = legacy }));
            StringAssert.Contains(error.Message, "conflicts with native AssociatedConstant");
        }

        [DataTestMethod]
        [DataRow("_Win32_AssociatedConstant_(LOOSE) int value;", "native enum")]
        [DataRow("_Win32_AssociatedConstant_(LOOSE) typedef unsigned long Alias;", "native enum")]
        [DataRow("_Win32_AssociatedConstant_(LOOSE) _Win32_AssociatedConstant_(LOOSE) _Win32_Enum_(First, unsigned long, FIRST);", "Duplicate")]
        [DataRow("_Win32_AssociatedConstant_(FIRST) _Win32_Enum_(First, unsigned long, FIRST);", "loose global")]
        [DataRow("_Win32_AssociatedConstant_(Missing) _Win32_Enum_(First, unsigned long, FIRST);", "Missing")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.AssociatedConstant()\", LOOSE))) typedef unsigned long First;", "malformed")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.AssociatedConstant(LOOSE)\"))) typedef unsigned long First;", "compiler-evaluated")]
        [DataRow("#define FLOATING 1.5\n_Win32_AssociatedConstant_(FLOATING) _Win32_Enum_(First, unsigned long, FIRST);", "integer constant")]
        [DataRow("enum Existing { VALUE = 1 }; enum _Win32_AssociatedConstant_(VALUE) Native { NATIVE = 2 };", "existing enumerator")]
        public void InvalidLooseAssociations_FailAtNativeSource(string declaration, string diagnostic)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Constants + declaration, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, diagnostic);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void FieldAndDeepPointerAssociations_PreserveStorageAndMatchingLegacy(string target, string language)
        {
            const string source = @"
enum Existing { VALUE = 1 };
#define GROUP_VALUE 0x80000000UL
_Win32_FlagsEnum_(NativeGroup, unsigned long, GROUP_VALUE);
typedef unsigned short Small;
struct Record {
    _Win32_AssociatedEnum_(Existing) Small value;
    _Win32_AssociatedEnum_(Existing) const Small** pointer;
    _Win32_AssociatedEnum_(Existing) signed short** signedPointer;
    Small absent;
    enum Existing genuine;
};
struct GroupRecord { _Win32_AssociatedEnum_(NativeGroup) const unsigned long** pointer; };
_Win32_AssociatedEnum_(Existing) Small** __stdcall Example(_Win32_AssociatedEnum_(Existing) Small*** value);
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            byte[] expected = File.ReadAllBytes(path);
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                var fields = NativeTypedefHeaderTests.Type(reader, "Record").GetFields()
                    .Select(reader.GetFieldDefinition).ToDictionary(field => reader.GetString(field.Name));
                foreach (string name in new[] { "value", "pointer", "signedPointer", "absent", "genuine" })
                    CollectionAssert.AreEqual(name is "value" or "pointer" or "signedPointer" ? new[] { "Existing" } : Array.Empty<string>(),
                        StringAttributes(reader, fields[name].GetCustomAttributes(), "AssociatedEnumAttribute"));
                AssertSignature(reader, fields["value"].Signature, 0, SignatureTypeCode.UInt16);
                AssertSignature(reader, fields["pointer"].Signature, 2, SignatureTypeCode.UInt16);
                AssertSignature(reader, fields["signedPointer"].Signature, 2, SignatureTypeCode.Int16);
                AssertSignature(reader, fields["absent"].Signature, 0, SignatureTypeCode.UInt16);
                var groupField = NativeTypedefHeaderTests.Type(reader, "GroupRecord").GetFields()
                    .Select(reader.GetFieldDefinition).Single();
                var groupSignature = reader.GetBlobReader(groupField.Signature);
                groupSignature.ReadSignatureHeader();
                for (int i = 0; i < 2; i++) Assert.AreEqual(SignatureTypeCode.Pointer, groupSignature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.TypeHandle, groupSignature.ReadSignatureTypeCode());
                Assert.AreEqual("NativeGroup", EnumHeaderTests.TypeName(reader, groupSignature.ReadTypeHandle()));
                Assert.AreEqual(0, groupSignature.RemainingBytes);
                Assert.AreEqual(0, StringAttributes(reader, groupField.GetCustomAttributes(), "AssociatedEnumAttribute").Length);
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == "Example");
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(1, signature.ReadCompressedInteger());
                for (int i = 0; i < 2; i++) Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode());
                for (int i = 0; i < 3; i++) Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode());
                Assert.AreEqual(0, signature.RemainingBytes);
                foreach (var parameter in method.GetParameters().Select(reader.GetParameter))
                    CollectionAssert.AreEqual(new[] { "Existing" },
                        StringAttributes(reader, parameter.GetCustomAttributes(), "AssociatedEnumAttribute"));
            }
            var remaps = new Dictionary<string, string> {
                ["Record::value"] = "[AssociatedEnum(\"Existing\")]",
                ["Record::pointer"] = "[AssociatedEnum(\"Existing\")]",
                ["Record::signedPointer"] = "[AssociatedEnum(\"Existing\")]",
                ["GroupRecord::pointer"] = "NativeGroup**",
                ["Example::return"] = "[AssociatedEnum(\"Existing\")]",
                ["Example::value"] = "[AssociatedEnum(\"Existing\")]",
            };
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));
            Save(path, "field-pointers-" + target + "-" + language.Replace("+", "p"));
            foreach (string conflict in new[] { "[AssociatedEnum(\"Different\")]", "[-AssociatedEnum]", "Existing" })
            {
                var error = Assert.ThrowsException<AggregateException>(() =>
                    fixture.Emit(new Dictionary<string, string> { ["Record::value"] = conflict }));
                StringAssert.Contains(error.Message, "conflicts with native AssociatedEnum");
            }
            foreach (string selector in new[] { "Example::return", "Example::value" })
            {
                var error = Assert.ThrowsException<AggregateException>(() =>
                    fixture.Emit(new Dictionary<string, string> { [selector] = "Existing" }));
                StringAssert.Contains(error.Message, "conflicts with native AssociatedEnum storage");
            }
            result = fixture.Scrape(source.Replace("_Win32_AssociatedEnum_(Existing) ", "", StringComparison.Ordinal),
                target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)), "Legacy-only field/return/parameter associations.");
        }

        [DataTestMethod]
        [DataRow("struct Record { _Win32_AssociatedEnum_(Existing) enum Existing value; };")]
        [DataRow("int __stdcall Example(_Win32_AssociatedEnum_(Existing) enum Existing** value);")]
        [DataRow("_Win32_AssociatedEnum_(Existing) enum Existing __stdcall Example(void);")]
        public void GenuineEnumUses_DoNotAcceptRedundantAssociations(string use)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("enum Existing { VALUE = 1 };\n" + use, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "redundant");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void CompilerEnumFacts_NativeScopeAndFlagsDoNotScopeSyntheticBags(string target)
        {
            const string source = @"
#define GROUP_VALUE 1
#define GROUP_FLAG 2
_Win32_Enum_(Group, unsigned long, GROUP_VALUE);
_Win32_FlagsEnum_(GroupFlags, unsigned long, GROUP_FLAG);
enum Ordinary { ORDINARY_VALUE = 0 };
enum __attribute__((flag_enum)) { ANONYMOUS_FLAG = 1 };
enum class NativeScoped : unsigned short;
enum class NativeScoped : unsigned short { NativeValue = 7 };
typedef enum __attribute__((flag_enum)) _NativeFlags { NATIVE_LOW = 1, NATIVE_HIGH = 2 } NativeFlags;
enum class __attribute__((flag_enum)) NativeBoth : unsigned long { Low = 1, High = 2 };
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(source, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            byte[] expected = File.ReadAllBytes(path);
            using (var pe = new PEReader(File.OpenRead(path)))
            {
                var reader = pe.GetMetadataReader();
                foreach (var fact in new[] {
                    (Name: "NativeScoped", Scoped: true, Flags: false),
                    (Name: "NativeFlags", Scoped: false, Flags: true),
                    (Name: "NativeBoth", Scoped: true, Flags: true),
                    (Name: "Group", Scoped: false, Flags: false),
                    (Name: "GroupFlags", Scoped: false, Flags: true),
                    (Name: "Ordinary", Scoped: false, Flags: false) })
                {
                    var attributes = NativeTypedefHeaderTests.Type(reader, fact.Name).GetCustomAttributes()
                        .Select(reader.GetCustomAttribute).Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray();
                    Assert.AreEqual(fact.Scoped ? 1 : 0, attributes.Count(name => name == "ScopedEnumAttribute"), fact.Name);
                    Assert.AreEqual(fact.Flags ? 1 : 0, attributes.Count(name => name == "FlagsAttribute"), fact.Name);
                }
                Assert.AreEqual(1, reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                    .Count(field => reader.GetString(field.Name) == "ANONYMOUS_FLAG"));
            }
            var legacy = new Dictionary<string, string> {
                ["NativeScoped"] = "[ScopedEnum]",
                ["NativeFlags"] = "[Flags]",
                ["NativeBoth"] = "[Flags][ScopedEnum]",
                ["GroupFlags"] = "[Flags]",
            };
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(legacy)), "Matching native/helper and legacy facts deduplicate.");
            foreach (string suffix in new[] { "", "Attribute" })
            {
                var repeated = new Dictionary<string, string> {
                    ["NativeScoped"] = "[ScopedEnum][ScopedEnum" + suffix + "]",
                    ["NativeFlags"] = "[Flags][Flags" + suffix + "]",
                    ["NativeBoth"] = "[ScopedEnum][Flags][ScopedEnum" + suffix + "][Flags" + suffix + "]",
                    ["GroupFlags"] = "[Flags][Flags" + suffix + "]",
                };
                CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(repeated)),
                    "Repeated equivalent parameterless enum markers retain one native/helper fact.");
            }
            Save(path, "native-enum-facts-" + target);
            foreach (var conflict in new[] { (Name: "NativeScoped", Value: "[-ScopedEnum]"),
                (Name: "NativeScoped", Value: "[-ScopedEnumAttribute]"),
                (Name: "NativeScoped", Value: "[ScopedEnum(1)]"),
                (Name: "NativeScoped", Value: "[ScopedEnum][ScopedEnumAttribute(1)]"),
                (Name: "NativeScoped", Value: "[ScopedEnum][-ScopedEnum]"),
                (Name: "NativeFlags", Value: "[-Flags]"), (Name: "NativeFlags", Value: "[-FlagsAttribute]"),
                (Name: "NativeFlags", Value: "[Flags][Flags(1)]"), (Name: "GroupFlags", Value: "[-Flags]") })
            {
                var error = Assert.ThrowsException<AggregateException>(() =>
                    fixture.Emit(new Dictionary<string, string> { [conflict.Name] = conflict.Value }));
                StringAssert.Contains(error.Message, "legacy remap conflicts with native");
            }
            result = fixture.Scrape(source, target, sal: true, extraSettings:
                "--with-attribute\nNativeScoped=ScopedEnum\nNativeFlags=Flags\n_NativeFlags=Flags\nNativeBoth=ScopedEnum\nNativeBoth=Flags");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit()), "Matching legacy scraper attributes deduplicate.");
            result = fixture.Scrape(source.Replace("enum class", "enum", StringComparison.Ordinal)
                .Replace("__attribute__((flag_enum))", "", StringComparison.Ordinal), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            legacy["NativeBoth"] = "[ScopedEnum][Flags]";
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(legacy,
                new Dictionary<string, string> { ["Flags"] = "System" })), "Legacy-only native enum facts stay compatible.");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c", "static const")]
        [DataRow("x86_64-pc-windows-msvc", "c", "static const")]
        [DataRow("aarch64-pc-windows-msvc", "c", "static const")]
        [DataRow("i686-pc-windows-msvc", "c++", "constexpr")]
        [DataRow("x86_64-pc-windows-msvc", "c++", "constexpr")]
        [DataRow("aarch64-pc-windows-msvc", "c++", "constexpr")]
        public void NativeLooseConstantDeclarations_AreNotDuplicatedOrMovedIntoEnums(string target, string language, string qualifier)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("__declspec(deprecated(\"original global\")) " + qualifier +
                " unsigned long LOOSE_NATIVE = 0x80000000UL;\n" +
                "enum _Win32_AssociatedConstant_(LOOSE_NATIVE) Native { NATIVE_VALUE = 1 };",
                target, sal: true, language: language);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            fixture.ScrapeConstants();
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var field = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Single(field => reader.GetString(field.Name) == "LOOSE_NATIVE");
            Assert.AreEqual("Apis", reader.GetString(reader.GetTypeDefinition(field.GetDeclaringType()).Name));
            AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt32);
            Assert.AreEqual(ConstantTypeCode.UInt32, reader.GetConstant(field.GetDefaultValue()).TypeCode);
            Assert.AreEqual(0x80000000u, reader.GetBlobReader(reader.GetConstant(field.GetDefaultValue()).Value).ReadUInt32());
            Assert.AreEqual(1, field.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "ObsoleteAttribute"));
            CollectionAssert.AreEqual(new[] { "LOOSE_NATIVE" }, StringAttributes(reader,
                NativeTypedefHeaderTests.Type(reader, "Native").GetCustomAttributes(), "AssociatedConstantAttribute"));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void LooseHelpers_TransitiveRepeatedIncludesAndOrdinaryClangPreserveAbsence(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string source = "#pragma once\n" + Constants + Associations + "_Win32_Enum_(First, unsigned long, FIRST);\n";
            foreach (bool metadata in new[] { false, true })
            {
                var result = fixture.Scrape(source, target, sal: true, language: language, metadata: metadata,
                    transitive: true, repeatInclude: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                using var pe = new PEReader(File.OpenRead(fixture.Emit()));
                var reader = pe.GetMetadataReader();
                Assert.AreEqual(metadata ? 1 : 0, reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Count(type => reader.GetString(type.Name) == "First"));
                Assert.AreEqual(metadata ? 2 : 0, reader.CustomAttributes.Select(reader.GetCustomAttribute)
                    .Count(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "AssociatedConstantAttribute"));
                Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Any(type => reader.GetString(type.Name).Contains("_WIN32META_", StringComparison.Ordinal)));
            }
        }

        internal static string[] StringAttributes(MetadataReader reader, CustomAttributeHandleCollection handles, string name) =>
            handles.Select(reader.GetCustomAttribute)
                .Where(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == name)
                .Select(attribute => {
                    var blob = reader.GetBlobReader(attribute.Value);
                    Assert.AreEqual((ushort)1, blob.ReadUInt16());
                    string value = blob.ReadSerializedString()!;
                    Assert.AreEqual((ushort)0, blob.ReadUInt16());
                    Assert.AreEqual(0, blob.RemainingBytes);
                    return value;
                }).ToArray();

        internal static void AssertSignature(MetadataReader reader, BlobHandle handle, int pointers, SignatureTypeCode storage)
        {
            var signature = reader.GetBlobReader(handle);
            signature.ReadSignatureHeader();
            for (int i = 0; i < pointers; i++)
                Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(storage, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        private void Save(string path, string name)
        {
            string proof = Path.Combine(TestContext.ResultsDirectory, name + ".winmd");
            File.Copy(path, proof, true);
            TestContext.AddResultFile(proof);
        }
    }
}
