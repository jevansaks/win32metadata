using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using MetadataUtils;
using Xunit;

namespace Windows.Win32.Tests
{
    public class HandleProducerPilotTests
    {
        [Fact]
        public void RealSdkProducersHaveRawSignaturesAndSiteSpecificOwnership()
        {
            string path = TestCommon.TestUtils.Win32WinmdPath;
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (var fact in new[] {
                (Name: "OpenPrinterW", Sequence: 2, Cleanup: "ClosePrinter", Invalid: new long[] { -1, 0 }),
                (Name: "LocalAlloc", Sequence: 0, Cleanup: "LocalFree", Invalid: new long[] { 0 }) })
            {
                var methods = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Where(method => reader.GetString(method.Name) == fact.Name).ToArray();
                Assert.NotEmpty(methods);
                foreach (var method in methods)
                {
                    var parameter = Assert.Single(method.GetParameters().Select(reader.GetParameter)
                        .Where(parameter => parameter.SequenceNumber == fact.Sequence));
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                        .Where(attribute => AttributeName(attribute) is "RAIIFreeAttribute" or "InvalidHandleValueAttribute")
                        .ToArray();
                    Assert.Equal(new[] { "RAIIFreeAttribute" }.Concat(fact.Invalid.Select(_ => "InvalidHandleValueAttribute")),
                        attributes.Select(AttributeName));
                    for (int index = 0; index < attributes.Length; index++)
                    {
                        var blob = reader.GetBlobReader(attributes[index].Value);
                        Assert.Equal((ushort)1, blob.ReadUInt16());
                        if (index == 0) Assert.Equal(fact.Cleanup, blob.ReadSerializedString());
                        else Assert.Equal(fact.Invalid[index - 1], blob.ReadInt64());
                        Assert.Equal((ushort)0, blob.ReadUInt16());
                        Assert.Equal(0, blob.RemainingBytes);
                    }
                    Assert.DoesNotContain(method.GetCustomAttributes().Select(reader.GetCustomAttribute),
                        attribute => AttributeName(attribute) is "RAIIFreeAttribute" or "InvalidHandleValueAttribute");
                }
            }

            foreach (string name in new[] { "GetProcessHeap", "ClosePrinter", "LocalFree" })
            {
                var methods = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Where(method => reader.GetString(method.Name) == name).ToArray();
                Assert.NotEmpty(methods);
                foreach (var method in methods)
                    Assert.DoesNotContain(method.GetCustomAttributes().Concat(method.GetParameters()
                        .Select(reader.GetParameter).SelectMany(parameter => parameter.GetCustomAttributes()))
                        .Select(reader.GetCustomAttribute), attribute => AttributeName(attribute) is
                            "RAIIFreeAttribute" or "InvalidHandleValueAttribute" or "DoNotReleaseAttribute" or
                            "CanReturnErrorsAsSuccessAttribute" or "NativeAnnotationAttribute");
            }

            var invalidValueType = Assert.Single(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Where(type => reader.GetString(type.Namespace) == "Windows.Win32.Foundation.Metadata" &&
                    reader.GetString(type.Name) == "InvalidHandleValueAttribute"));
            var usage = Assert.Single(invalidValueType.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Where(attribute => AttributeName(attribute) == "AttributeUsageAttribute"));
            var usageBlob = reader.GetBlobReader(usage.Value);
            Assert.Equal((ushort)1, usageBlob.ReadUInt16());
            Assert.Equal((int)(AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.ReturnValue),
                usageBlob.ReadInt32());
            Assert.Equal((ushort)2, usageBlob.ReadUInt16());
            foreach (string name in new[] { "AllowMultiple", "Inherited" })
            {
                Assert.Contains((CustomAttributeNamedArgumentKind)usageBlob.ReadByte(),
                    new[] { CustomAttributeNamedArgumentKind.Field, CustomAttributeNamedArgumentKind.Property });
                Assert.Equal((byte)SignatureTypeCode.Boolean, usageBlob.ReadByte());
                Assert.Equal(name, usageBlob.ReadSerializedString());
                Assert.True(usageBlob.ReadBoolean());
            }
            Assert.Equal(0, usageBlob.RemainingBytes);

            var types = DecompilerUtils.LoadDecompilerTypeSystem(path);
            var projectedMethods = types.MainModule.TypeDefinitions.SelectMany(type => type.Methods).ToArray();
            foreach (var site in new[] { ("OpenPrinterW", 1, "Windows.Win32.Foundation.HANDLE*"),
                ("ClosePrinter", 0, "Windows.Win32.Foundation.HANDLE"),
                ("GetProcessHeap", -1, "System.Void*"), ("LocalAlloc", -1, "System.Void*"),
                ("LocalFree", -1, "System.Void*"), ("LocalFree", 0, "System.Void*") })
            {
                var methods = projectedMethods.Where(method => method.Name == site.Item1).ToArray();
                Assert.NotEmpty(methods);
                foreach (var method in methods)
                    Assert.Equal(site.Item3, (site.Item2 < 0 ? method.ReturnType : method.Parameters[site.Item2].Type).ReflectionName);
            }

            string AttributeName(CustomAttribute attribute)
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
}
