using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using ClangSharpSourceToWinmd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class NativeTypedefHeaderTests
    {
        private const string Settings = "--native-typedef\nResult\nSigned\nUnsigned\nOpaque\n" +
            "--remap\nLONG_PTR=IntPtr\nULONG_PTR=UIntPtr";
        private const string Header = @"
#ifdef _WIN64
typedef long long LONG_PTR;
typedef unsigned long long ULONG_PTR;
#else
typedef long LONG_PTR;
typedef unsigned long ULONG_PTR;
#endif
typedef long Result;
typedef LONG_PTR Signed;
typedef ULONG_PTR Unsigned;
typedef struct Opaque__* Opaque;
typedef Signed Same;
typedef Same* LPSame;
_Win32_AlsoUsableFor_(Unsigned) typedef ULONG_PTR Key;
typedef unsigned short VARTYPE;
enum VARENUM { VT_EMPTY = 0 };
struct State { LPSame address; Key key; VARTYPE vt; };
Result __stdcall Example(Same value, LPSame address, Opaque opaque, Key key, Unsigned parent);
";

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void NativeDeclarations_KeepNominalReferencesStorageAndOnlyAuthoredAssociation(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Header, target, sal: true, language: language, extraSettings: Settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertStorage(reader, "Result", SignatureTypeCode.Int32);
            AssertStorage(reader, "Signed", SignatureTypeCode.IntPtr);
            AssertStorage(reader, "Unsigned", SignatureTypeCode.UIntPtr);
            AssertStorage(reader, "Key", SignatureTypeCode.UIntPtr);
            AssertStorage(reader, "Opaque", SignatureTypeCode.Pointer, SignatureTypeCode.Void);
            var key = Type(reader, "Key");
            var association = reader.GetBlobReader(key.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "AlsoUsableForAttribute").Value);
            Assert.AreEqual((ushort)1, association.ReadUInt16());
            Assert.AreEqual("Unsigned", association.ReadSerializedString());
            Assert.AreEqual((ushort)0, association.ReadUInt16());
            Assert.AreEqual(0, association.RemainingBytes);
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(item => reader.GetString(item.Name) == "Example");
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(5, signature.ReadCompressedInteger());
            AssertReference(reader, ref signature, "Result");
            AssertReference(reader, ref signature, "Signed");
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertReference(reader, ref signature, "Signed");
            AssertReference(reader, ref signature, "Opaque");
            AssertReference(reader, ref signature, "Key");
            AssertReference(reader, ref signature, "Unsigned");
            Assert.AreEqual(0, signature.RemainingBytes);
            var state = Type(reader, "State");
            var address = state.GetFields().Select(reader.GetFieldDefinition).Single(field => reader.GetString(field.Name) == "address");
            signature = reader.GetBlobReader(address.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertReference(reader, ref signature, "Signed");
            Assert.AreEqual(0, signature.RemainingBytes);
            var vt = state.GetFields().Select(reader.GetFieldDefinition).Single(field => reader.GetString(field.Name) == "vt");
            signature = reader.GetBlobReader(vt.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode(), "Native VARTYPE is never widened to VARENUM.");
            Assert.AreEqual(0, signature.RemainingBytes);
            foreach (string name in new[] { "Same", "LPSame", "LONG_PTR", "ULONG_PTR", "VARTYPE" })
                Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition).Any(type => reader.GetString(type.Name) == name));
            Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute).Any(attribute =>
                ParameterHeaderTests.AttributeName(reader, attribute) is "RAIIFreeAttribute" or "InvalidHandleValueAttribute" or
                "SupportedOSPlatformAttribute" or "NativeAnnotationAttribute" or "RetValAttribute"));
            Assert.AreEqual(1, reader.CustomAttributes.Select(reader.GetCustomAttribute).Count(attribute =>
                ParameterHeaderTests.AttributeName(reader, attribute) == "AlsoUsableForAttribute"));
        }

        [DataTestMethod]
        [DataRow("_Win32_AlsoUsableFor_(Target) int __stdcall Example(void);")]
        [DataRow("int __stdcall Example(_Win32_AlsoUsableFor_(Target) int value);")]
        [DataRow("typedef void (*Callback)(_Win32_AlsoUsableFor_(Target) int value);")]
        [DataRow("struct _Win32_AlsoUsableFor_(Target) Value { int field; };")]
        [DataRow("_Win32_AlsoUsableFor_(Missing) typedef unsigned long Alias;")]
        [DataRow("_Win32_AlsoUsableFor_(Alias) typedef unsigned long Alias;")]
        [DataRow("_Win32_AlsoUsableFor_(Target) typedef long Alias;")]
        [DataRow("_Win32_AlsoUsableFor_(unsigned long) typedef unsigned long Alias;")]
        [DataRow("_Win32_AlsoUsableFor_(Target) _Win32_AlsoUsableFor_(Target) typedef unsigned long Alias;")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.AlsoUsableFor()\"))) typedef unsigned long Alias;")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.AlsoUsableFor(Unknown::Alias)\"))) typedef unsigned long Alias;")]
        [DataRow("_Win32_AlsoUsableFor_(Target) typedef int (*Alias)(void);")]
        [DataRow("typedef _Win32_AlsoUsableFor_(Target) unsigned long Alias;")]
        public void InvalidAssociations_AreSourceLocatedBeforeOutput(string declaration)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef unsigned long Target;\n" + declaration, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void ExplicitConflictingDocumentation_IsNotDroppedFromALegacyPartial()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef long Result;", sal: true, extraSettings: "--native-typedef\nResult");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "autotypes.cs"),
                "using Windows.Win32.Foundation.Metadata; namespace Test { " +
                "[Documentation(\"test://explicit-conflict\")] public partial struct Result {} }");
            var compilation = ClangSharpSourceCompilation.Create(
                Path.GetDirectoryName(fixture.GeneratedSource), "crossarch",
                new Dictionary<string, string> { ["Result"] = "[Documentation(\"test://definition\")]" },
                new Dictionary<string, Dictionary<string, string>>(), Array.Empty<string>(), new Dictionary<string, string>(),
                new Dictionary<string, string>(), new HashSet<string>(), Array.Empty<string>(),
                new Dictionary<string, string>(), new Dictionary<string, string>());
            Assert.IsTrue(compilation.GetDiagnostics().Any(diagnostic =>
                diagnostic.Id == "CS0579" && diagnostic.GetMessage().Contains("Documentation", StringComparison.Ordinal)));
        }

        [DataTestMethod]
        [DataRow("typedef int Alias[4];", "")]
        [DataRow("typedef int (*Alias)(void);", "")]
        [DataRow("typedef int* Alias;", "")]
        [DataRow("typedef void* __ptr32 Alias;", "")]
        [DataRow("typedef struct { int field; } Alias;", "")]
        [DataRow("typedef unsigned long Alias;", "--exclude\nAlias")]
        [DataRow("typedef long Alias;", "--remap\nAlias=Unrelated")]
        [DataRow("typedef long Alias;", "--with-type\nAlias=long")]
        [DataRow("typedef unsigned long Target; _Win32_AlsoUsableFor_(Target) typedef unsigned long Alias;", "--exclude\nTarget")]
        public void UnsupportedSelectedShapesAndConflicts_AreNotSilentlyFlattened(string declaration, string settings)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(declaration, sal: true, extraSettings: "--native-typedef\nAlias\n" + settings);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void NativeNamespaceAndUsingChain_ResolveCompilerDeclarationsWithoutInventedNames()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef unsigned long Scalar;\n" +
                "namespace Native { extern \"C\" { typedef long Scalar; } using Same = Scalar; typedef Same* LP;\n" +
                "_Win32_AlsoUsableFor_(Scalar) typedef long Key; }\n" +
                "Native::Scalar __stdcall Example(Native::Same value, Native::LP* address, Native::Key key);",
                sal: true, extraSettings: "--native-typedef\nNative::Scalar");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertStorage(reader, "Scalar", SignatureTypeCode.Int32);
            AssertStorage(reader, "Key", SignatureTypeCode.Int32);
            var association = reader.GetBlobReader(Type(reader, "Key").GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "AlsoUsableForAttribute").Value);
            Assert.AreEqual((ushort)1, association.ReadUInt16());
            Assert.AreEqual("Scalar", association.ReadSerializedString(), "The typed operand binds the inner native declaration, not a same-named global typedef.");
            Assert.AreEqual((ushort)0, association.ReadUInt16());
            Assert.AreEqual(0, association.RemainingBytes);
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(item => reader.GetString(item.Name) == "Example");
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(3, signature.ReadCompressedInteger());
            AssertReference(reader, ref signature, "Scalar");
            AssertReference(reader, ref signature, "Scalar");
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertReference(reader, ref signature, "Scalar");
            AssertReference(reader, ref signature, "Key");
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        [TestMethod]
        public void TransitiveAlias_ReferencesTheOwningDeclarationWithoutEmittingAnotherType()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef NativeValue Same;\nSame __stdcall Example(Same* address);",
                includedHeader: "typedef long NativeValue;", sal: true, extraSettings: "--native-typedef\nNativeValue");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string declarations = File.ReadAllText(fixture.GeneratedSource);
            StringAssert.Contains(declarations, "NativeValue");
            Assert.IsFalse(File.Exists(Path.ChangeExtension(fixture.GeneratedSource, ".headerTypedefs.cs")));
            // The owning partition contributes its declaration; this partition only contributes references.
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "Owner.cs"),
                "namespace Test { [Windows.Win32.Foundation.Metadata.NativeTypedef] public struct NativeValue { public int Value; } }");
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertStorage(reader, "NativeValue", SignatureTypeCode.Int32);
            var signature = reader.GetBlobReader(reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Example").Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(1, signature.ReadCompressedInteger());
            AssertReference(reader, ref signature, "NativeValue");
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            AssertReference(reader, ref signature, "NativeValue");
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        [TestMethod]
        public void ExplicitNamespaceOwner_ProjectsSharedIncludeOnlyInTheExistingNamespace()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string header = "NativeValue __stdcall Example(NativeValue value);";
            foreach (string owner in new[] { "Test", "Different" })
            {
                var result = fixture.Scrape(header, includedHeader: "typedef long NativeValue;", sal: true,
                    extraSettings: "--native-typedef\nNativeValue=" + owner);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                Assert.AreEqual(owner == "Test", File.Exists(Path.ChangeExtension(fixture.GeneratedSource, ".headerTypedefs.cs")));
                StringAssert.Contains(File.ReadAllText(fixture.GeneratedSource), "NativeValue");
                if (owner == "Test")
                {
                    using var pe = new PEReader(File.OpenRead(fixture.Emit()));
                    var reader = pe.GetMetadataReader();
                    AssertStorage(reader, "NativeValue", SignatureTypeCode.Int32);
                    Assert.AreEqual("Test", reader.GetString(Type(reader, "NativeValue").Namespace));
                }
            }
        }

        [TestMethod]
        public void NativeResidualSettings_PreserveOnlyUnmigratedLegacyFacts()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Header, sal: true, extraSettings: Settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var stream = File.Create(Path.Combine(Path.GetDirectoryName(fixture.GeneratedSource)!, "autotypes.cs"));
            NativeTypedefStructsCreator.WriteToStream(new Dictionary<string, string>
                { ["Key"] = "Test", ["Close"] = "Test" }, new[] { new AutoType {
                    Name = "Key", Namespace = "Test", ValueType = "native", CloseApi = "Close", InvalidHandleValues = new long[] { 0 },
                } }, stream);
            stream.Dispose();
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            AssertStorage(reader, "Key", SignatureTypeCode.UIntPtr);
            var attributes = Type(reader, "Key").GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).OrderBy(name => name).ToArray();
            CollectionAssert.AreEqual(new[] { "AlsoUsableForAttribute", "InvalidHandleValueAttribute", "NativeTypedefAttribute", "RAIIFreeAttribute" }, attributes);
        }

        [TestMethod]
        public void LegacyAssociation_CannotOverwriteTheCompilerResolvedTarget()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(Header, sal: true, extraSettings: Settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] native = File.ReadAllBytes(fixture.Emit());
            CollectionAssert.AreEqual(native, File.ReadAllBytes(fixture.Emit(new Dictionary<string, string>
                { ["Key"] = "[AlsoUsableFor(\"Unsigned\")]" })));
            var error = Assert.ThrowsException<AggregateException>(() => fixture.Emit(new Dictionary<string, string>
                { ["Key"] = "[AlsoUsableFor(\"Different\")]" }));
            StringAssert.Contains(error.Message, "conflicts with native AlsoUsableFor");
            StringAssert.Contains(error.Message, "input.h:");
        }

        internal static TypeDefinition Type(MetadataReader reader, string name) =>
            reader.TypeDefinitions.Select(reader.GetTypeDefinition).Single(type => reader.GetString(type.Name) == name);

        internal static void AssertStorage(MetadataReader reader, string name, params SignatureTypeCode[] codes)
        {
            var type = Type(reader, name);
            var identity = reader.GetBlobReader(type.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "NativeTypedefAttribute").Value);
            Assert.AreEqual((ushort)1, identity.ReadUInt16());
            Assert.AreEqual((ushort)0, identity.ReadUInt16());
            Assert.AreEqual(0, identity.RemainingBytes);
            var field = reader.GetFieldDefinition(type.GetFields().Single());
            Assert.AreEqual("Value", reader.GetString(field.Name));
            var signature = reader.GetBlobReader(field.Signature);
            signature.ReadSignatureHeader();
            foreach (var code in codes) Assert.AreEqual(code, signature.ReadSignatureTypeCode(), name);
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        internal static void AssertReference(MetadataReader reader, ref BlobReader signature, string name, string ns = "Test")
        {
            Assert.AreEqual(SignatureTypeCode.TypeHandle, signature.ReadSignatureTypeCode(), name);
            var handle = signature.ReadTypeHandle();
            if (handle.Kind == HandleKind.TypeDefinition)
            {
                var definition = reader.GetTypeDefinition((TypeDefinitionHandle)handle);
                Assert.AreEqual(name, reader.GetString(definition.Name));
                Assert.AreEqual(ns, reader.GetString(definition.Namespace));
            }
            else
            {
                Assert.AreEqual(HandleKind.TypeReference, handle.Kind);
                var reference = reader.GetTypeReference((TypeReferenceHandle)handle);
                Assert.AreEqual(name, reader.GetString(reference.Name));
                Assert.AreEqual(ns, reader.GetString(reference.Namespace));
                Assert.AreEqual(HandleKind.ModuleDefinition, reference.ResolutionScope.Kind);
            }
        }
    }
}
