using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using MetadataUtils;
using Xunit;

namespace Windows.Win32.Tests
{
    public class LibraryMappingTests
    {
        [Fact]
        public void WinHttpStatusCallbackKeepsItsNativeTypedef()
        {
            var typeSystem = DecompilerUtils.LoadDecompilerTypeSystem(TestCommon.TestUtils.Win32WinmdPath);
            var apis = DecompilerUtils.GetSelfDefinedWinmdToplevelTypes(typeSystem)
                .Single(type => type.FullName == "Windows.Win32.Networking.WinHttp.Apis");
            var method = apis.Methods.Single(method => method.Name == "WinHttpSetStatusCallback");

            Assert.Equal("WINHTTP_STATUS_CALLBACK", method.ReturnType.Name);
            Assert.Equal("WINHTTP_STATUS_CALLBACK", method.Parameters[1].Type.Name);
        }

        [Theory]
        [InlineData("Windows.Win32.Data.HtmlHelp", "HtmlHelpA", "hhctrl.ocx")]
        [InlineData("Windows.Win32.Data.HtmlHelp", "HtmlHelpW", "hhctrl.ocx")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginGetAuthenticatorState", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAddAuthenticator", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAddAuthenticator2", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginFreeAddAuthenticatorResponse", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginRemoveAuthenticator", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginUpdateAuthenticatorDetails", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginUpdateAuthenticatorDetails2", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAuthenticatorAddCredentials", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAuthenticatorRemoveCredentials", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAuthenticatorRemoveAllCredentials", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAuthenticatorGetAllCredentials", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginAuthenticatorFreeCredentialDetailsArray", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginPerformUserVerification", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginPerformUserVerification2", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginFreeUserVerificationResponse", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginGetUserVerificationCount", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginGetUserVerificationPublicKey", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginGetOperationSigningPublicKey", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginFreePublicKeyResponse", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNEncodeMakeCredentialResponse", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNDecodeMakeCredentialRequest", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNFreeDecodedMakeCredentialRequest", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNDecodeGetAssertionRequest", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNFreeDecodedGetAssertionRequest", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNEncodeGetAssertionResponse", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginRegisterStatusChangeCallback", "webauthn.dll")]
        [InlineData("Windows.Win32.Security.Authentication.WebAuthn", "WebAuthNPluginUnregisterStatusChangeCallback", "webauthn.dll")]
        [InlineData("Windows.Win32.Devices.AllJoyn", "AllJoynCloseBusHandle", "MSAJApi.dll")]
        [InlineData("Windows.Win32.Devices.AllJoyn", "alljoyn_init", "MSAJApi.dll")]
        [InlineData("Windows.Win32.NetworkManagement.P2P", "PeerGroupStartup", "P2P.dll")]
        [InlineData("Windows.Win32.Media.MediaFoundation", "OPMXboxEnableHDCP", "OPMXbox.dll")]
        [InlineData("Windows.Win32.Media.MediaFoundation", "OPMXboxGetHDCPStatus", "OPMXbox.dll")]
        [InlineData("Windows.Win32.Media.MediaFoundation", "OPMXboxGetHDCPStatusAndType", "OPMXbox.dll")]
        public void SdkApisHaveDllImports(string expectedNamespace, string methodName, string expectedDll)
        {
            using var stream = File.OpenRead(TestCommon.TestUtils.Win32WinmdPath);
            using var peReader = new PEReader(stream);
            var reader = peReader.GetMetadataReader();
            var method = reader.MethodDefinitions
                .Select(reader.GetMethodDefinition)
                .Single(method => reader.StringComparer.Equals(method.Name, methodName));
            var declaringType = reader.GetTypeDefinition(method.GetDeclaringType());
            Assert.Equal(expectedNamespace, reader.GetString(declaringType.Namespace));

            var import = method.GetImport();
            Assert.False(import.Module.IsNil);
            Assert.Equal(methodName, reader.GetString(import.Name));
            Assert.Equal(expectedDll, reader.GetString(reader.GetModuleReference(import.Module).Name));
        }
    }
}
