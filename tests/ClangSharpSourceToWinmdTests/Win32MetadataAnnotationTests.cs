using System.Collections.Generic;
using ClangSharpSourceToWinmd;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class Win32MetadataAnnotationTests
    {
        private static string Clean(string source)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            var cleaned = MetadataSyntaxTreeCleaner.CleanSyntaxTree(
                tree,
                new Dictionary<string, string>(),
                new Dictionary<string, Dictionary<string, string>>(),
                new HashSet<string>(),
                new Dictionary<string, string>(),
                new Dictionary<string, string>(),
                new Dictionary<string, string>(),
                new HashSet<string>(),
                new HashSet<string>(),
                "test.cs");
            return cleaned.GetRoot().ToFullString();
        }

        [TestMethod]
        public void ImportLibraryAnnotation_OverridesEmptyDllImportName()
        {
            string source = @"
using System.Runtime.InteropServices;
namespace Windows.Win32.Test
{
    public static unsafe partial class Apis
    {
        [DllImport("""", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName(""HRESULT"")]
        [NativeAnnotation(""win32metadata:supported_os=windows5.1.2600"")]
        [NativeAnnotation(""win32metadata:import_library=P2PGRAPH.dll"")]
        public static extern int PeerGraphStartup(ushort wVersionRequested);
    }
}";
            string result = Clean(source);

            Assert.IsTrue(result.Contains("DllImport(\"P2PGRAPH.dll\""));
            Assert.IsTrue(result.Contains("[SupportedOSPlatform(\"windows5.1.2600\")]"));
        }

        [TestMethod]
        public void SetLastErrorAnnotation_SetsDllImportSetLastError()
        {
            string source = @"
using System.Runtime.InteropServices;
namespace Windows.Win32.Test
{
    public static unsafe partial class Apis
    {
        [DllImport(""kernel32.dll"", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName(""BOOL"")]
        [NativeAnnotation(""win32metadata:set_last_error"")]
        public static extern int SomeFunc(ushort x);
    }
}";
            string result = Clean(source);
            Assert.IsTrue(result.Contains("SetLastError=true") || result.Contains("SetLastError = true"), result);
        }

        [TestMethod]
        public void RaiiFreeAndInvalidHandleAnnotations_BecomeRealAttributesOnParameter()
        {
            string source = @"
namespace Windows.Win32.Test
{
    public static unsafe partial class Apis
    {
        [System.Runtime.InteropServices.DllImport(""bcrypt.dll"", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName(""NTSTATUS"")]
        public static extern int BCryptOpenAlgorithmProvider([NativeAnnotation(""win32metadata:invalid_handle=0"")] [NativeAnnotation(""win32metadata:raii_free=BCryptCloseAlgorithmProvider"")] void** phAlgorithm);
    }
}";
            string result = Clean(source);

            Assert.IsTrue(result.Contains("[InvalidHandleValue(0)]"));
            Assert.IsTrue(result.Contains("[RAIIFree(\"BCryptCloseAlgorithmProvider\")]"));
        }
    }
}
