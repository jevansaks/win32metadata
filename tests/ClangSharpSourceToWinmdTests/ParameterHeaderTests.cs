using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class ParameterHeaderTests
    {
        [DataTestMethod]
        [DataRow("Win32Metadata.Retval(extra)", "int*", "Unknown")]
        [DataRow("Win32Metadata.AssociatedEnum(Missing)", "int*", "existing native enum")]
        [DataRow("Win32Metadata.AssociatedEnum(Option)", "float*", "integer or native enum storage")]
        [DataRow("Win32Metadata.Retained", "int", "pointer parameter")]
        [DataRow("Win32Metadata.ContainsInteriorPointers", "int*", "caller-owned output")]
        [DataRow("Win32Metadata.AssociatedEnum(Option,Other)", "int*", "Unknown or malformed")]
        public void InvalidOwnedFacts_KeepNativeSourceDiagnostic(string payload, string type, string diagnostic)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("enum Option { One = 1 };\nint __stdcall Example(__attribute__((annotate(\"" +
                payload + "\"))) " + type + " value);", sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, diagnostic);
            StringAssert.Contains(result.Output, "input.h:");
        }

        [TestMethod]
        public void ConflictingLegacyAssociation_CannotOverwriteNativeFact()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("enum Option { One = 1 };\nint __stdcall Example(" +
                "__attribute__((annotate(\"Win32Metadata.AssociatedEnum(Option)\"))) int value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<AggregateException>(() => fixture.Emit(
                new Dictionary<string, string> { ["Example::value"] = "[AssociatedEnum(\"Different\")]" }));
            StringAssert.Contains(error.Message, "Example::value");
            StringAssert.Contains(error.Message, "conflicts with native AssociatedEnum");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ComMethod_PreservesNativeOutPointerAndRetval(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"struct __declspec(uuid(""64abea93-aac7-4f3b-a257-976deba89f04"")) IExample {
    virtual int __stdcall Method(_COM_Outptr_ void** output, _Out_ _Win32_Retval_ int* value) = 0;
};", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Method");
            var parameters = method.GetParameters().Select(reader.GetParameter).ToArray();
            AssertAttribute(reader, parameters[0], "ComOutPtrAttribute");
            AssertAttribute(reader, parameters[1], "RetValAttribute");
            Assert.IsTrue(parameters.All(p => p.Attributes == ParameterAttributes.Out));
        }

        [TestMethod]
        public void RedeclarationsAndRepeatedIncludes_PreserveOneOwnedParameterFact()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#pragma once\nint __stdcall Example(_Win32_Retained_ int* value);\n" +
                "int __stdcall Example(int* value);", repeatInclude: true, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            AssertAttribute(reader, reader.GetParameter(method.GetParameters().Single()), "RetainedAttribute");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeComResultNullability_PreservesIndependentAtOptionality(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"int __stdcall Example(
    _COM_Outptr_result_maybenull_ void** resultOnly,
    _COM_Outptr_result_maybenull_ _At_(*storageOptional, _Out_opt_) void** storageOptional);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Example");
            var parameters = method.GetParameters().Select(reader.GetParameter).ToArray();
            CollectionAssert.AreEqual(new[] { ParameterAttributes.Out, ParameterAttributes.Out | ParameterAttributes.Optional },
                parameters.Select(parameter => parameter.Attributes).ToArray());
            foreach (var parameter in parameters) AssertAttribute(reader, parameter, "ComOutPtrAttribute");
            CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01, 0x0f, 0x0f, 0x01 },
                reader.GetBlobBytes(method.Signature).TakeLast(6).ToArray());
        }

        [TestMethod]
        public void LateOnlyParameterAnnotation_IsDiagnosedInsteadOfSilentlyLost()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("int __stdcall Example(int* value);\n" +
                "int __stdcall Example(_Win32_Retained_ int* value);", sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "first native declaration");
        }

        [DataTestMethod]
        [DataRow("c")]
        [DataRow("c++")]
        public void TransitiveOwnedHeader_UsesTheSameNativeTransport(string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#pragma once\nint __stdcall Example(_Win32_Retained_ int* value);",
                language: language, sal: true, repeatInclude: true, transitive: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            AssertAttribute(reader, reader.GetParameter(method.GetParameters().Single()), "RetainedAttribute");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ReservedPointer_HeaderOnlyAndMatchingLegacyRemainExact(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("int __stdcall Example(_In_opt_ void* reserved);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var remaps = new Dictionary<string, string> { ["Example::reserved"] = "[Reserved]" };
            byte[] legacy = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape("int __stdcall Example(_In_opt_ _Reserved_ void* reserved);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(legacy, File.ReadAllBytes(fixture.Emit()));
            CollectionAssert.AreEqual(legacy, File.ReadAllBytes(fixture.Emit(remaps)));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeSal_PreservesDirectionOptionalReservedComPointerAndByteCapacity(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"int __stdcall Example(
    _In_ const int* input,
    _Out_ int* output,
    _Inout_opt_ int* both,
    _Reserved_ void* reserved,
    _COM_Outptr_ void** object,
    _Out_writes_bytes_to_opt_(capacity, *written) void* buffer,
    unsigned long capacity,
    _Out_ unsigned long* written);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            var parameters = method.GetParameters().Select(reader.GetParameter).ToArray();
            CollectionAssert.AreEqual(new[] { ParameterAttributes.In, ParameterAttributes.Out,
                ParameterAttributes.In | ParameterAttributes.Out | ParameterAttributes.Optional,
                ParameterAttributes.Optional, ParameterAttributes.Out, ParameterAttributes.Out | ParameterAttributes.Optional,
                ParameterAttributes.In, ParameterAttributes.Out }, parameters.Select(p => p.Attributes).ToArray(),
                string.Join(", ", parameters.Select(p => p.Attributes)));
            AssertAttribute(reader, parameters[0], "ConstAttribute");
            AssertAttribute(reader, parameters[3], "ReservedAttribute");
            AssertAttribute(reader, parameters[4], "ComOutPtrAttribute");
            AssertAttribute(reader, parameters[5], "MemorySizeAttribute");
            var memory = reader.GetBlobReader(parameters[5].GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(a => AttributeName(reader, a) == "MemorySizeAttribute").Value);
            Assert.AreEqual((ushort)1, memory.ReadUInt16());
            Assert.AreEqual((ushort)1, memory.ReadUInt16());
            Assert.AreEqual((byte)0x53, memory.ReadByte(), "Named field");
            Assert.AreEqual((byte)0x06, memory.ReadByte(), "Int16");
            Assert.AreEqual("BytesParamIndex", memory.ReadSerializedString());
            Assert.AreEqual((short)6, memory.ReadInt16());
            Assert.AreEqual(0, memory.RemainingBytes);
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(8, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
            foreach (var type in new[] { SignatureTypeCode.Int32, SignatureTypeCode.Int32,
                SignatureTypeCode.Int32, SignatureTypeCode.Void })
            {
                Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(type, signature.ReadSignatureTypeCode());
            }
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ReturnAssociation_UsesSequenceZeroAndStringConstructor(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("enum Option { One = 1 };\n_Win32_AssociatedEnum_(Option) " +
                "unsigned long __stdcall Example(int value);", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            var parameters = method.GetParameters().Select(reader.GetParameter).ToArray();
            var returnParameter = parameters.Single(p => p.SequenceNumber == 0);
            var association = reader.GetCustomAttribute(returnParameter.GetCustomAttributes().Single());
            Assert.AreEqual("AssociatedEnumAttribute", AttributeName(reader, association));
            var constructor = reader.GetBlobReader(association.Constructor.Kind == HandleKind.MethodDefinition ?
                reader.GetMethodDefinition((MethodDefinitionHandle)association.Constructor).Signature :
                reader.GetMemberReference((MemberReferenceHandle)association.Constructor).Signature);
            constructor.ReadSignatureHeader();
            Assert.AreEqual(1, constructor.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Void, constructor.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.String, constructor.ReadSignatureTypeCode());
            Assert.AreEqual(0, constructor.RemainingBytes);
            var blob = reader.GetBlobReader(association.Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual("Option", blob.ReadSerializedString());
            Assert.AreEqual((ushort)0, blob.ReadUInt16());
            Assert.AreEqual(0, blob.RemainingBytes);
            Assert.AreEqual(ParameterAttributes.None, returnParameter.Attributes);
            Assert.AreEqual(ParameterAttributes.In, parameters.Single(p => p.SequenceNumber == 1).Attributes);
        }

        [TestMethod]
        public void NativeCountsAndDoubleNull_KeepUnitsAndNoOwnedFallback()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"int __stdcall Example(
    _Out_writes_(capacity) _Post_ _NullNull_terminated_ wchar_t* names,
    unsigned long capacity, void* ordinary, unsigned long long wide);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Example");
            var parameters = method.GetParameters().Select(reader.GetParameter).ToArray();
            AssertAttribute(reader, parameters[0], "NullNullTerminatedAttribute");
            AssertAttribute(reader, parameters[0], "NativeArrayInfoAttribute");
            var blob = reader.GetBlobReader(parameters[0].GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Single(a => AttributeName(reader, a) == "NativeArrayInfoAttribute").Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual((ushort)1, blob.ReadUInt16());
            Assert.AreEqual((byte)0x53, blob.ReadByte());
            Assert.AreEqual((byte)0x06, blob.ReadByte());
            Assert.AreEqual("CountParamIndex", blob.ReadSerializedString());
            Assert.AreEqual((short)1, blob.ReadInt16());
            Assert.AreEqual(0, blob.RemainingBytes);
            Assert.AreEqual(ParameterAttributes.In | ParameterAttributes.Out, parameters[2].Attributes);
            Assert.AreEqual(ParameterAttributes.In, parameters[3].Attributes);
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(4, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt16, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt32, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            Assert.AreEqual(SignatureTypeCode.UInt64, signature.ReadSignatureTypeCode());
            Assert.AreEqual(0, signature.RemainingBytes);
            Assert.IsFalse(parameters.SelectMany(p => p.GetCustomAttributes()).Select(reader.GetCustomAttribute)
                .Any(a => new[] { "MemorySizeAttribute", "RetValAttribute", "RetainedAttribute",
                    "ComOutPtrAttribute", "ContainsInteriorPointersAttribute", "AssociatedEnumAttribute" }
                    .Contains(AttributeName(reader, a))));
        }

        [DataTestMethod]
        [DataRow("RetVal", "RetValAttribute")]
        [DataRow("Retained", "RetainedAttribute")]
        [DataRow("ContainsInteriorPointers", "ContainsInteriorPointersAttribute")]
        [DataRow("AssociatedEnum(Option)", "AssociatedEnumAttribute")]
        public void OwnedParameterFacts_HeaderOnlyMatchesLegacy(string payload, string attribute)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string declaration = "enum Option { One = 1 };\nint __stdcall Example(_Out_ int* value);";
            if (payload == "ContainsInteriorPointers") declaration = declaration.Replace("_Out_", "_Out_writes_(2)");
            var legacy = fixture.Scrape(declaration, sal: true);
            Assert.AreEqual(0, legacy.ExitCode, legacy.Output);
            string arguments = payload.StartsWith("AssociatedEnum") ? "(\"Option\")" : "";
            var remaps = new Dictionary<string, string> { ["Example::value"] = "[" + attribute.Replace("Attribute", "") + arguments + "]" };
            byte[] expected = File.ReadAllBytes(fixture.Emit(remaps));
            var header = fixture.Scrape(declaration.Replace("int* value", "__attribute__((annotate(\"Win32Metadata." + payload + "\"))) int* value"), sal: true);
            Assert.AreEqual(0, header.ExitCode, header.Output);
            string output = fixture.Emit();
            using (var pe = new PEReader(File.OpenRead(output)))
            {
                var reader = pe.GetMetadataReader();
                AssertAttribute(reader, reader.GetParameter(reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(m => reader.GetString(m.Name) == "Example").GetParameters().Single()), attribute);
            }
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));
        }

        internal static void AssertAttribute(MetadataReader reader, Parameter parameter, string name)
        {
            var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Where(a => AttributeName(reader, a) == name).ToArray();
            Assert.AreEqual(1, attributes.Length, name);
            var blob = reader.GetBlobReader(attributes[0].Value);
            Assert.AreEqual((ushort)1, blob.ReadUInt16(), "Custom attribute prolog");
        }

        internal static string AttributeName(MetadataReader reader, CustomAttribute attribute)
        {
            if (attribute.Constructor.Kind == HandleKind.MethodDefinition)
                return reader.GetString(reader.GetTypeDefinition(reader.GetMethodDefinition(
                    (MethodDefinitionHandle)attribute.Constructor).GetDeclaringType()).Name);
            var parent = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent;
            return reader.GetString(reader.GetTypeReference((TypeReferenceHandle)parent).Name);
        }
    }
}
