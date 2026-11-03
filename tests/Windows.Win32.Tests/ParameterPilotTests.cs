using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Xunit;

namespace Windows.Win32.Tests
{
    public class ParameterPilotTests
    {
        [Fact]
        public void RetiredSelectorsHaveTheirExactFactsInProductionMetadata()
        {
            using var pe = new PEReader(File.OpenRead(TestCommon.TestUtils.Win32WinmdPath));
            var reader = pe.GetMetadataReader();
            var records = new Dictionary<string, List<Parameter>>(StringComparer.Ordinal);
            foreach (var methodHandle in reader.MethodDefinitions)
            {
                var method = reader.GetMethodDefinition(methodHandle);
                string name = reader.GetString(method.Name);
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                foreach (var handle in method.GetParameters())
                {
                    var parameter = reader.GetParameter(handle);
                    if (parameter.SequenceNumber == 0) continue;
                    string selector = (method.Attributes.HasFlag(MethodAttributes.PinvokeImpl) ? name : owner + "::" + name) +
                        "::" + reader.GetString(parameter.Name);
                    if (!records.TryGetValue(selector, out var parameters)) records.Add(selector, parameters = new List<Parameter>());
                    parameters.Add(parameter);
                }
            }

            var groups = new[]
            {
                ("ComOutPtrAttribute", ParameterAttributes.Out, new[] {
                    "CoGetDefaultContext::ppv", "IAddrExclusionControl::GetCurrentAddrExclusionList::ppEnumerator",
                    "IAsyncManager::GetCallContext::pInterface", "IGlobalInterfaceTable::GetInterfaceFromGlobal::ppv",
                    "IInternalUnknown::QueryInternalInterface::ppv", "IMarshal::UnmarshalInterface::ppv",
                    "IRpcChannelBuffer3::GetCallContext::pInterface", "CreateDataCache::ppv", "BindMoniker::ppvResult",
                    "CoGetObject::ppv", "CoGetObjectContext::ppv", "CoGetClassObject::ppv", "CoGetCallContext::ppInterface",
                    "CoGetCancelObject::ppUnk", "IMoniker::BindToObject::ppvResult", "IMoniker::BindToStorage::ppvObj" }),
                ("ReservedAttribute", ParameterAttributes.In | ParameterAttributes.Optional, new[] {
                    "CoAllowSetForegroundWindow::lpvReserved", "CoDisableCallCancellation::pReserved",
                    "CoEnableCallCancellation::pReserved", "CoInitialize::pvReserved", "CoInitializeEx::pvReserved",
                    "CoInitializeSecurity::pReserved1", "CoInitializeSecurity::pReserved3" }),
                ("RetainedAttribute", ParameterAttributes.In | ParameterAttributes.Out, new[] {
                    "ReadFileEx::lpOverlapped", "WriteFileEx::lpOverlapped" }),
                ("RetValAttribute", ParameterAttributes.Out, new[] { "IDXGIAdapter::GetDesc::pDesc" }),
                ("ContainsInteriorPointersAttribute", ParameterAttributes.Out | ParameterAttributes.Optional, new[] {
                    "WinHttpQueryHeadersEx::pBuffer" }),
            };
            int checkedSelectors = 0;
            foreach (var group in groups)
                foreach (string selector in group.Item3)
                {
                    foreach (var parameter in GetParameters(selector))
                    {
                        Assert.Equal(group.Item2, parameter.Attributes);
                        var blob = GetAttribute(parameter, group.Item1);
                        Assert.Equal((ushort)1, blob.ReadUInt16());
                        Assert.Equal((ushort)0, blob.ReadUInt16());
                        Assert.Equal(0, blob.RemainingBytes);
                    }
                    checkedSelectors++;
                }

            foreach (var association in new[] {
                ("CoGetClassObject::dwClsContext", "CLSCTX"),
                ("CoInitializeEx::dwCoInit", "COINIT"),
                ("CoInitializeSecurity::dwCapabilities", "EOLE_AUTHENTICATION_CAPABILITIES"),
                ("CoRegisterClassObject::flags", "REGCLS"),
                ("CoSetProxyBlanket::dwCapabilities", "EOLE_AUTHENTICATION_CAPABILITIES"),
                ("IClientSecurity::QueryBlanket::pCapabilites", "EOLE_AUTHENTICATION_CAPABILITIES"),
                ("IClientSecurity::SetBlanket::dwCapabilities", "EOLE_AUTHENTICATION_CAPABILITIES"),
                ("IStorage::MoveElementTo::grfFlags", "STGMOVE"),
                ("IStream::Commit::grfCommitFlags", "STGC"),
                ("IStream::LockRegion::dwLockType", "LOCKTYPE"),
                ("IStream::Stat::grfStatFlag", "STATFLAG") })
            {
                foreach (var parameter in GetParameters(association.Item1))
                {
                    Assert.Equal(association.Item1.Contains("QueryBlanket", StringComparison.Ordinal) ?
                        ParameterAttributes.Out | ParameterAttributes.Optional : ParameterAttributes.In, parameter.Attributes);
                    var blob = GetAttribute(parameter, "AssociatedEnumAttribute");
                    Assert.Equal((ushort)1, blob.ReadUInt16());
                    Assert.Equal(association.Item2, blob.ReadSerializedString());
                    Assert.Equal((ushort)0, blob.ReadUInt16());
                    Assert.Equal(0, blob.RemainingBytes);
                }
                Assert.Contains(reader.TypeDefinitions.Select(reader.GetTypeDefinition),
                    type => reader.GetString(type.Name) == association.Item2 &&
                        type.GetFields().Select(reader.GetFieldDefinition).Any(field => reader.GetString(field.Name) == "value__"));
                checkedSelectors++;
            }
            Assert.Equal(38, checkedSelectors);

            List<Parameter> GetParameters(string selector)
            {
                Assert.True(records.TryGetValue(selector, out var parameters), "Missing production selector: " + selector);
                Assert.NotEmpty(parameters);
                return parameters;
            }

            BlobReader GetAttribute(Parameter parameter, string name)
            {
                var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                Assert.DoesNotContain(attributes, attribute => AttributeName(attribute) == "NativeAnnotationAttribute" ||
                    AttributeName(attribute) == "CppAttributeListAttribute");
                return reader.GetBlobReader(Assert.Single(attributes.Where(attribute => AttributeName(attribute) == name)).Value);
            }

            string AttributeName(CustomAttribute attribute)
            {
                var parent = attribute.Constructor.Kind == HandleKind.MethodDefinition ?
                    (EntityHandle)reader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor).GetDeclaringType() :
                    reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent;
                return parent.Kind == HandleKind.TypeDefinition ?
                    reader.GetString(reader.GetTypeDefinition((TypeDefinitionHandle)parent).Name) :
                    reader.GetString(reader.GetTypeReference((TypeReferenceHandle)parent).Name);
            }
        }
    }
}
