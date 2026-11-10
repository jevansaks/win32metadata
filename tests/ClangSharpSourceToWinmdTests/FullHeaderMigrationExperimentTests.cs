using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class FullHeaderMigrationExperimentTests
    {
        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void FileApiProducer_RequiresCleanupDeclarationBeforeTypedOperand(string target)
        {
            string native = Sdk("um", "fileapi.h");
            string producer = Slice(native, @"WINBASEAPI\s+HANDLE\s+WINAPI\s+FindFirstVolumeW\s*\([^;]+;");
            string cleanup = Slice(native, @"WINBASEAPI\s+BOOL\s+WINAPI\s+FindVolumeClose\s*\([^;]+;");
            const string types = "#define WINBASEAPI\n#define WINAPI __stdcall\n" +
                "typedef int BOOL; typedef unsigned long DWORD; typedef void* HANDLE; typedef wchar_t* LPWSTR;\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(types + "_Win32_RAIIFree_(FindVolumeClose)\n" + producer,
                target, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "undeclared identifier 'FindVolumeClose'");
            result = fixture.Scrape(types + cleanup + "\n_Win32_RAIIFree_(FindVolumeClose)\n" + producer,
                target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(item => reader.GetString(item.Name) == "FindFirstVolumeW");
            var returned = method.GetParameters().Select(reader.GetParameter)
                .Single(item => item.SequenceNumber == 0);
            Assert.AreEqual(1, returned.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Count(item => ParameterHeaderTests.AttributeName(reader, item) == "RAIIFreeAttribute"));
        }

        [TestMethod]
        public void OldAmsiCommentCarrier_DoesNotReplaceCurrentAvailabilityTransport()
        {
            string declaration = Slice(Sdk("um", "amsi.h"),
                @"STDAPI_\(VOID\)\s+AmsiUninitialize\s*\([^;]+;");
            const string types = "#define STDAPI_(type) type __stdcall\n" +
                "typedef void VOID; typedef struct HAMSICONTEXT__* HAMSICONTEXT;\n";
            const string comment =
                "/* Win32Metadata: function ::AmsiUninitialize => SupportedOSPlatform(\"windows10.0.10240\") */\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(types + comment + declaration, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertAvailability(fixture.Emit(), expected: false);
            result = fixture.Scrape(types + "_Win32_SupportedOS_10240_\n" + declaration, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertAvailability(fixture.Emit(), expected: true);
        }

        [TestMethod]
        public void OldGrGlobalGroup_ProjectsPointerSentinelAsIncorrectUInt32()
        {
            string sentinel = Slice(Sdk("um", "winuser.h"),
                @"(?m)^#define GR_GLOBAL\s+\(\(HANDLE\)-2\)[^\r\n]*");
            string group = Slice(ExperimentInput(),
                @"typedef _WIN32META_ANNOTATION_\(""Enum\(GR_GLOBAL\)"",\s*\(unsigned int\)-2\)\s*unsigned int GET_GUI_RESOURCES_FLAGS;");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("typedef void* HANDLE;\n" + sentinel + "\n" + group, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var field = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Single(item => reader.GetString(item.Name) == "GR_GLOBAL");
            var constant = reader.GetConstant(field.GetDefaultValue());
            Assert.AreEqual(ConstantTypeCode.UInt32, constant.TypeCode);
            Assert.AreEqual(0xfffffffeU, reader.GetBlobReader(constant.Value).ReadUInt32());
        }

        [TestMethod]
        public void GenuineHandleIdentity_StillNeedsExplicitNominalProjectionPolicy()
        {
            string handle = Slice(Sdk("um", "winnt.h"), @"typedef void \*HANDLE;");
            string borrowed = Slice(Sdk("um", "processthreadsapi.h"),
                @"WINBASEAPI\s+HANDLE\s+WINAPI\s+GetCurrentProcess\s*\([^;]+;");
            string input = "#define WINBASEAPI\n#define WINAPI __stdcall\ntypedef void VOID;\n" + handle + "\n" + borrowed;
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(input, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using (var pe = new PEReader(File.OpenRead(fixture.Emit())))
            {
                var reader = pe.GetMetadataReader();
                Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Any(item => reader.GetString(item.Name) == "HANDLE"));
            }
            result = fixture.Scrape(input, sal: true, extraSettings: "--native-typedef\nHANDLE");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using (var pe = new PEReader(File.OpenRead(fixture.Emit())))
            {
                var reader = pe.GetMetadataReader();
                Assert.IsTrue(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Any(item => reader.GetString(item.Name) == "HANDLE"));
                Assert.IsFalse(reader.CustomAttributes.Select(reader.GetCustomAttribute)
                    .Any(item => ParameterHeaderTests.AttributeName(reader, item) == "RAIIFreeAttribute"));
            }
        }

        [TestMethod]
        public void ServiceErrorGroup_RequiresConstantFromItsActualOwner()
        {
            string group = Slice(ExperimentInput(),
                @"_Win32_AssociatedConstant_\(SERVICE_NO_CHANGE\)\s+_Win32_Enum_\(SERVICE_ERROR,[^;]+;");
            string values = string.Join("\n", Regex.Matches(Sdk("um", "winnt.h"),
                @"(?m)^#define SERVICE_ERROR_\w+\s+[^\r\n]+").Select(match => match.Value));
            string sentinel = Slice(Sdk("um", "winsvc.h"), @"(?m)^#define SERVICE_NO_CHANGE\s+[^\r\n]+");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(values + "\n" + group, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "undeclared identifier 'SERVICE_NO_CHANGE'");
            result = fixture.Scrape(values + "\n" + sentinel + "\n" + group, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
        }

        [TestMethod]
        public void RegionToGenericGdiHandle_StillConflictsAfterDeclarationOrderIsRepaired()
        {
            string macro = Slice(Sdk("um", "winnt.h"),
                @"(?m)^#define DECLARE_HANDLE\(name\) struct name##__\{int unused;\}; typedef struct name##__ \*name");
            string region = Slice(Sdk("shared", "minwindef.h"), @"DECLARE_HANDLE\(HRGN\);");
            string generic = Slice(Sdk("shared", "windef.h"), @"typedef void NEAR\* HGDIOBJ;");
            string association = Slice(ExperimentInput(), @"_Win32_AlsoUsableFor_\(HGDIOBJ\) typedef HRGN HRGN;");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(macro + "\n" + region + "\n" + association, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "undeclared identifier 'HGDIOBJ'");
            result = fixture.Scrape("#define NEAR\n" + macro + "\n" + region + "\n" +
                generic + "\n" + association, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "AlsoUsableFor target 'HGDIOBJ' has conflicting native storage.");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void RegionProducer_RejectsGenuineGenericGdiCleanupDespiteEarlierDeclaration(string target)
        {
            string macro = Slice(Sdk("um", "winnt.h"),
                @"(?m)^#define DECLARE_HANDLE\(name\) struct name##__\{int unused;\}; typedef struct name##__ \*name");
            string region = Slice(Sdk("shared", "minwindef.h"), @"DECLARE_HANDLE\(HRGN\);");
            string generic = Slice(Sdk("shared", "windef.h"), @"typedef void NEAR\* HGDIOBJ;");
            string native = Sdk("um", "wingdi.h");
            string cleanup = Slice(native, @"WINGDIAPI\s+BOOL\s+WINAPI\s+DeleteObject\s*\([^;]+;");
            string producer = Slice(native, @"WINGDIAPI\s+HRGN\s+WINAPI\s+CreateRectRgn\s*\([^;]+;");
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(
                "#define NEAR\n#define WINGDIAPI\n#define WINAPI __stdcall\ntypedef int BOOL;\n" +
                macro + "\n" + region + "\n" + generic + "\n" + cleanup +
                "\n_Win32_RAIIFree_(DeleteObject, -1, 0)\n" + producer, target, sal: true);
            Assert.AreNotEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "accepting exactly the produced resource type");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        private static string ExperimentInput() =>
            File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "tests", "ClangSharpSourceToWinmdTests", "FullHeaderMigrationExperimentFixtures.h"));

        private static string Sdk(string directory, string name) =>
            File.ReadAllText(Path.Combine(LastErrorHeaderTests.SdkPackagePath(),
                "c", "include", "10.0.28000.0", directory, name));

        private static string Slice(string source, string expression)
        {
            var matches = Regex.Matches(source, expression);
            Assert.AreEqual(1, matches.Count, expression);
            return matches[0].Value;
        }

        private static void AssertAvailability(string path, bool expected)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(item => reader.GetString(item.Name) == "AmsiUninitialize");
            Assert.AreEqual(expected, method.GetCustomAttributes().Select(reader.GetCustomAttribute)
                .Any(item => ParameterHeaderTests.AttributeName(reader, item) == "SupportedOSPlatformAttribute"));
        }
    }
}
