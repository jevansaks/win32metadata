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
    public class ProducerHeaderTests
    {
        private const string Prefix = @"
typedef void* HANDLE;
typedef __INTPTR_TYPE__ LONG_PTR;
#define INVALID_HANDLE_VALUE ((HANDLE)(LONG_PTR)-1)
int __stdcall CloseThing(HANDLE value);
";

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void ProducerSites_ResolveTypedCleanupAndOrderedValues(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + @"
#define Failure 0x80000000UL
_Win32_RAIIFree_(CloseThing, 0, INVALID_HANDLE_VALUE, Failure, -1, 0)
HANDLE __stdcall Example(void);
int __stdcall Output(_Out_ _Win32_RAIIFree_Handle_(CloseThing) HANDLE* value);
HANDLE __stdcall Borrowed(void);
", target, language: language, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertFacts(reader, "Example", 0, "CloseThing", 0, -1, 2147483648, -1, 0);
            AssertFacts(reader, "Output", 1, "CloseThing", -1, 0);
            AssertNoOwnership(reader, "Borrowed");
            AssertNoOwnership(reader, "CloseThing");
            Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).SelectMany(type => type.GetCustomAttributes())
                .Select(reader.GetCustomAttribute).Any(attr => Name(reader, attr) is "RAIIFreeAttribute" or "InvalidHandleValueAttribute"));
            Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute)
                .Any(attr => Name(reader, attr) is "NativeAnnotationAttribute" or "CppAttributeList"));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", -1L)]
        [DataRow("x86_64-pc-windows-msvc", 4294967295L)]
        [DataRow("aarch64-pc-windows-msvc", 4294967295L)]
        public void PointerCasts_KeepTargetWidthAndNativeSignedness(string target, long expected)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix +
                "_Win32_RAIIFree_(CloseThing, (HANDLE)0xffffffffUL, (unsigned int)-1, (long)-1)\n" +
                "HANDLE __stdcall Example(void);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            AssertFacts(pe.GetMetadataReader(), "Example", 0, "CloseThing", expected, 4294967295, -1);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void HeaderOnlyAndMatchingLegacy_AreByteExact(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string declarations = "HANDLE __stdcall Example(void);\nint __stdcall Output(_Out_ HANDLE* value);";
            var remaps = new Dictionary<string, string>
            {
                ["Example::return"] = "[RAIIFree(\"CloseThing\")][InvalidHandleValue(-1L)]",
                ["Output::value"] = "[RAIIFree(\"CloseThing\")]",
            };
            var result = fixture.Scrape(Prefix + declarations, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] baseline = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(Prefix + declarations.Replace("HANDLE __stdcall Example",
                "_Win32_RAIIFree_(CloseThing, INVALID_HANDLE_VALUE) HANDLE __stdcall Example")
                .Replace("_Out_ HANDLE*", "_Out_ _Win32_RAIIFree_(CloseThing) HANDLE*"), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(fixture.Emit()));
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(fixture.Emit(remaps)));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void MethodReturnsAndOutputs_PreserveActualSequenceAndNoNeighborLeak(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + @"
struct __declspec(uuid(""a70c3c0d-eeba-4a63-b18e-df1a8bc45682"")) IExample {
    _Win32_RAIIFree_Handle_(CloseThing) virtual HANDLE __stdcall Return(void) = 0;
    virtual int __stdcall Output(_Out_ _Win32_RAIIFree_(CloseThing, 0) HANDLE* value) = 0;
    virtual HANDLE __stdcall Borrowed(void) = 0;
};", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertFacts(reader, "Return", 0, "CloseThing", -1, 0);
            AssertFacts(reader, "Output", 1, "CloseThing", 0);
            AssertNoOwnership(reader, "Borrowed");
        }

        [TestMethod]
        public void FirstDeclarationAndTransitiveRepeatedIncludes_EmitOneFact()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#pragma once\n" + Prefix + @"
_Win32_RAIIFree_Handle_(CloseThing) HANDLE __stdcall Example(void);
HANDLE __stdcall Example(void);
int __stdcall Output(_Out_ _Win32_RAIIFree_(CloseThing, 0) HANDLE* value);
int __stdcall Output(_Out_ HANDLE* value);", sal: true, repeatInclude: true, transitive: true,
                remap: "Renamed", extraSettings: "--remap\nCloseThing=RenamedCleanup\n");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertFacts(reader, "Renamed", 0, "RenamedCleanup", -1, 0);
            AssertFacts(reader, "Output", 1, "RenamedCleanup", 0);
        }

        [TestMethod]
        public void OverloadsAndExcludedCleanup_AreDiagnosedInsteadOfLeakingOwnership()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + @"struct IExample {
    virtual HANDLE __stdcall Method(int value) = 0;
    _Win32_RAIIFree_(CloseThing) virtual HANDLE __stdcall Method(void) = 0;
};", sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "overloaded");
            result = fixture.Scrape(Prefix + "_Win32_RAIIFree_(CloseThing) HANDLE __stdcall Example(void);",
                sal: true, extraSettings: "--exclude\nCloseThing");
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "excluded");
        }

        [TestMethod]
        public void RepeatedInvalidValues_DoNotChangeSingleValuedLegacyReplacement()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + "_Win32_RAIIFree_Handle_(CloseThing) HANDLE __stdcall Example(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit(new Dictionary<string, string>
            {
                ["Example::return"] = "[Documentation(\"first\")][Documentation(\"last\")]",
            })));
            var reader = pe.GetMetadataReader();
            AssertFacts(reader, "Example", 0, "CloseThing", -1, 0);
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Example");
            var documentation = method.GetParameters().Select(reader.GetParameter).Single(parameter => parameter.SequenceNumber == 0)
                .GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => Name(reader, attribute) == "DocumentationAttribute");
            var blob = reader.GetBlobReader(documentation.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual("last", blob.ReadSerializedString());
            Assert.AreEqual((ushort)0, blob.ReadUInt16());
            Assert.AreEqual(0, blob.RemainingBytes);
        }

        [TestMethod]
        public void NoAuthoredInvalidValues_CannotAcquireThemFromLegacy()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + "_Win32_RAIIFree_(CloseThing) HANDLE __stdcall Example(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<AggregateException>(() =>
                fixture.Emit(new Dictionary<string, string> { ["Example::return"] = "[InvalidHandleValue(0)]" }));
            StringAssert.Contains(error.Message, "conflicts with native InvalidHandleValue");
        }

        [DataTestMethod]
        [DataRow("_Win32_RAIIFree_(CloseThing) typedef HANDLE Alias;", "producer")]
        [DataRow("struct Value { _Win32_RAIIFree_(CloseThing) HANDLE value; };", "producer")]
        [DataRow("_Win32_RAIIFree_(CloseThing) HANDLE value;", "producer")]
        [DataRow("_Win32_RAIIFree_(CloseThing) void __stdcall Example(void);", "resource")]
        [DataRow("int __stdcall Example(_Win32_RAIIFree_(CloseThing) HANDLE* value);", "output")]
        [DataRow("int __stdcall Example(_In_ _Win32_RAIIFree_(CloseThing) HANDLE* value);", "output")]
        [DataRow("int __stdcall Example(_Out_ _Win32_RAIIFree_(CloseThing) int value);", "output")]
        [DataRow("_Win32_RAIIFree_(CloseThing) HANDLE __stdcall Example(void) { return 0; }", "body")]
        [DataRow("_Win32_RAIIFree_(Missing) HANDLE __stdcall Example(void);", "Missing")]
        [DataRow("int NotFunction;\n_Win32_RAIIFree_(NotFunction) HANDLE __stdcall Example(void);", "cleanup")]
        [DataRow("int Wrong(int);\n_Win32_RAIIFree_(Wrong) HANDLE __stdcall Example(void);", "resource type")]
        [DataRow("int Wrong(HANDLE, int);\n_Win32_RAIIFree_(Wrong) HANDLE __stdcall Example(void);", "exactly")]
        [DataRow("int Wrong(HANDLE, ...);\n_Win32_RAIIFree_(Wrong) HANDLE __stdcall Example(void);", "non-variadic")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.RAIIFree\"))) HANDLE __stdcall Example(void);", "typed cleanup")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.RAIIFree\", 8))) HANDLE __stdcall Example(void);", "sizeof")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.RAIIFree()\"))) HANDLE __stdcall Example(void);", "typed")]
        [DataRow("_Win32_RAIIFree_(CloseThing, 1.5) HANDLE __stdcall Example(void);", "integer")]
        [DataRow("_Win32_RAIIFree_(CloseThing, 0xffffffffffffffffULL) HANDLE __stdcall Example(void);", "Int64")]
        [DataRow("int variable;\n_Win32_RAIIFree_(CloseThing, variable) HANDLE __stdcall Example(void);", "constant")]
        [DataRow("int variable;\n_Win32_RAIIFree_(CloseThing, &variable) HANDLE __stdcall Example(void);", "unsupported")]
        [DataRow("_Win32_RAIIFree_(CloseThing) _Win32_RAIIFree_(CloseThing) HANDLE __stdcall Example(void);", "Duplicate")]
        [DataRow("HANDLE __stdcall Example(void);\n_Win32_RAIIFree_(CloseThing) HANDLE __stdcall Example(void);", "first native")]
        [DataRow("int __stdcall Example(_Out_ HANDLE* value);\nint __stdcall Example(_Out_ _Win32_RAIIFree_(CloseThing) HANDLE* value);", "first native")]
        [DataRow("HANDLE __stdcall _Win32_RAIIFree_(CloseThing) Example(void);", "precede")]
        [DataRow("HANDLE __stdcall Example(void) _Win32_RAIIFree_(CloseThing);", "precede")]
        public void UnsupportedFacts_AreSourceLocatedAndDoNotEmit(string declaration, string diagnostic)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + declaration, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, diagnostic);
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("[RAIIFree(\"Different\")]")]
        [DataRow("[-RAIIFree]")]
        [DataRow("[InvalidHandleValue(2)]")]
        [DataRow("[-InvalidHandleValue]")]
        public void LegacyConflicts_CannotReplaceProducerFacts(string remap)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix +
                "_Win32_RAIIFree_Handle_(CloseThing) HANDLE __stdcall Example(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<AggregateException>(() =>
                fixture.Emit(new Dictionary<string, string> { ["Example::return"] = remap }));
            StringAssert.Contains(error.Message, "conflicts with native");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void OrdinaryClang_HelpersAreNoOpsEvenForUndeclaredArguments(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Prefix + "HANDLE __stdcall Example(void);", target,
                metadata: false, language: language, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] baseline = File.ReadAllBytes(fixture.Emit());
            result = fixture.Scrape(Prefix +
                "_Win32_RAIIFree_(Undeclared, not_a_constant) _Win32_RAIIFree_Handle_(Missing) HANDLE __stdcall Example(void);",
                target, metadata: false, language: language, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(fixture.Emit()));
        }

        internal static void AssertFacts(MetadataReader reader, string methodName, int sequence, string cleanup, params long[] values)
        {
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == methodName);
            var site = method.GetParameters().Select(reader.GetParameter).Single(parameter => parameter.SequenceNumber == sequence);
            var attributes = site.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Where(attribute => Name(reader, attribute) is "RAIIFreeAttribute" or "InvalidHandleValueAttribute").ToArray();
            CollectionAssert.AreEqual(new[] { "RAIIFreeAttribute" }.Concat(values.Select(_ => "InvalidHandleValueAttribute")).ToArray(),
                attributes.Select(attribute => Name(reader, attribute)).ToArray());
            for (int index = 0; index < attributes.Length; index++)
            {
                var attribute = attributes[index];
                var constructor = reader.GetBlobReader(attribute.Constructor.Kind == HandleKind.MethodDefinition ?
                    reader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor).Signature :
                    reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Signature);
                constructor.ReadSignatureHeader();
                Assert.AreEqual(1, constructor.ReadCompressedInteger());
                Assert.AreEqual(SignatureTypeCode.Void, constructor.ReadSignatureTypeCode());
                Assert.AreEqual(index == 0 ? SignatureTypeCode.String : SignatureTypeCode.Int64, constructor.ReadSignatureTypeCode());
                Assert.AreEqual(0, constructor.RemainingBytes);
                var blob = reader.GetBlobReader(attribute.Value);
                Assert.AreEqual((ushort)1, blob.ReadUInt16());
                if (index == 0) Assert.AreEqual(cleanup, blob.ReadSerializedString());
                else Assert.AreEqual(values[index - 1], blob.ReadInt64());
                Assert.AreEqual((ushort)0, blob.ReadUInt16());
                Assert.AreEqual(0, blob.RemainingBytes);
            }
        }

        internal static void AssertNoOwnership(MetadataReader reader, string methodName)
        {
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == methodName);
            Assert.IsFalse(method.GetCustomAttributes().Concat(method.GetParameters()
                .Select(reader.GetParameter).SelectMany(parameter => parameter.GetCustomAttributes()))
                .Select(reader.GetCustomAttribute).Any(attribute =>
                    Name(reader, attribute) is "RAIIFreeAttribute" or "InvalidHandleValueAttribute"));
        }

        internal static string Name(MetadataReader reader, CustomAttribute attribute)
        {
            var type = attribute.Constructor.Kind == HandleKind.MethodDefinition ?
                reader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor).GetDeclaringType() :
                reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent;
            return type.Kind == HandleKind.TypeDefinition ?
                reader.GetString(reader.GetTypeDefinition((TypeDefinitionHandle)type).Name) :
                reader.GetString(reader.GetTypeReference((TypeReferenceHandle)type).Name);
        }
    }
}
