using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class SupportedOSHeaderTests
    {
        private const string Predecessor = "19ab9e3800c641060f47d0e8369227e83ced55f3";

        [TestMethod]
        public void LegacyHeaderBothAndNeither_PreserveExactFinalMetadata()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? expected = null;
            foreach (var mode in new[] { (Header: false, Legacy: true), (Header: true, Legacy: false),
                (Header: true, Legacy: true), (Header: false, Legacy: false) })
            {
                var result = fixture.Scrape((mode.Header ? "_Win32_SupportedOS_WindowsXP_\n" : "") +
                    "int __stdcall Example(void);", sal: true,
                    extraSettings: mode.Legacy ? "--with-attribute\nExample=SupportedOSPlatform(\"windows5.1.2600\")" : "");
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string output = fixture.Emit();
                using var pe = new PEReader(File.OpenRead(output));
                var reader = pe.GetMetadataReader();
                var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(method => reader.GetString(method.Name) == "Example");
                Assert.AreEqual(mode.Header || mode.Legacy ? "windows5.1.2600" : null,
                    Platform(reader, method.GetCustomAttributes().ToArray()));
                if (expected == null) expected = File.ReadAllBytes(output);
                if (mode.Header) CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            }
        }

        [DataTestMethod]
        [DataRow("Win32Metadata.SupportedOSPlatform")]
        [DataRow("Win32Metadata.SupportedOSPlatform(windows5.0)")]
        [DataRow("Win32Metadata.SupportedOSPlatform(\"windows11\")")]
        [DataRow("Win32Metadata.SupportedOSPlatform(\"windows5.0\", \"windows6.1\")")]
        public void MalformedPayloads_FailAtTheSource(string payload)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("__attribute__((annotate(\"" +
                payload.Replace("\"", "\\\"") + "\"))) int Example(void);", sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("_Win32_SupportedOS_WindowsXP_ _Win32_SupportedOS_Windows7_ int Example(void);")]
        [DataRow("_Win32_SupportedOS_WindowsXP_ int Example(void);\n_Win32_SupportedOS_Windows7_ int Example(void);")]
        [DataRow("struct _Win32_SupportedOS_WindowsXP_ Record;\nstruct _Win32_SupportedOS_Windows7_ Record { int value; };")]
        [DataRow("struct Interface { virtual _Win32_SupportedOS_WindowsXP_ _Win32_SupportedOS_Windows7_ int Method() = 0; };")]
        [DataRow("int Example(_Win32_SupportedOS_WindowsXP_ int* value);")]
        [DataRow("struct Record { _Win32_SupportedOS_WindowsXP_ int value; };")]
        [DataRow("typedef _Win32_SupportedOS_WindowsXP_ int Scalar;")]
        [DataRow("typedef int (*Callback)(int); typedef _Win32_SupportedOS_WindowsXP_ Callback Alias;")]
        [DataRow("struct Record { int value; }; typedef _Win32_SupportedOS_WindowsXP_ struct Record Alias;")]
        [DataRow("int Example(void);\n_Win32_SupportedOS_WindowsXP_ int Example(void);")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.SupportedOSPlatform(\\\"windows5.0\\\")\", 1))) int Example(void);")]
        public void ConflictingAndUnsupportedTargets_FailWithoutOutput(string header)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void ConflictingLegacyAvailability_IsNotSilentlyOverridden()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("_Win32_SupportedOS_WindowsXP_ int Example(void);", sal: true,
                extraSettings: "--with-attribute\nExample=SupportedOSPlatform(\"windows6.1\")");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            var error = Assert.ThrowsException<AggregateException>(() => fixture.Emit());
            StringAssert.Contains(error.ToString(), "Legacy SupportedOSPlatform conflicts");
        }

        [TestMethod]
        public void RepeatedDeclarationsAndAnnotations_EmitOneFact()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("_Win32_SupportedOS_WindowsXP_\n" +
                "_Win32_SupportedOS_WindowsXP_ int Example(void);\nint Example(void);", sal: true, repeatInclude: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Example");
            Assert.AreEqual("windows5.1.2600", Platform(reader, method.GetCustomAttributes().ToArray()));
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void NativeVersionGuardsWithoutAnnotations_DoNotInventAvailability(bool metadata)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#if _WIN32_WINNT >= 0x0601\nint Example(void);\n#endif\n" +
                (metadata ? "" : "_Win32_SupportedOS_Windows7_\n") + "int Unannotated(void);", sal: true,
                metadata: metadata, extraSettings: "--additional\n-D_WIN32_WINNT=0x0A00");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            foreach (var method in reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Where(method => reader.GetString(method.Name) is "Example" or "Unannotated"))
                Assert.IsNull(Platform(reader, method.GetCustomAttributes().ToArray()));
        }

        [TestMethod]
        public void OwningDeclarations_RetireOnlyExactlyMatchingSidecars()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string Legacy(string path)
            {
                var result = LastErrorHeaderTests.Run("git", "-C", root, "show", Predecessor + ":" + path);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                return result.Output.Replace("\r\n", "\n");
            }
            string helper = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders",
                "shared", "win32metadata_annotations.h"));
            var versions = Regex.Matches(helper,
                @"#define _Win32_SupportedOS_(\w+)_ _WIN32META_ANNOTATION_\(""SupportedOSPlatform\(\\""([^""]+)\\""\)""\)")
                .Cast<Match>().ToDictionary(match => match.Groups[1].Value, match => match.Groups[2].Value);
            Assert.AreEqual(20, versions.Count);
            var retired = new Dictionary<string, string>();
            string oldSidecar = Legacy("generation/WinSDK/supportedOS.rsp");
            var oldNames = Regex.Matches(oldSidecar, @"(?m)^(\w+)=").Cast<Match>()
                .Select(match => match.Groups[1].Value).ToHashSet(StringComparer.Ordinal);
            foreach (string owner in new[] { "winhttp.h", "errhandlingapi.h", "handleapi.h", "combaseapi.h", "objbase.h", "ObjIdlbase.h" })
            {
                string path = "generation/WinSDK/RecompiledIdlHeaders/um/" + owner;
                string current = File.ReadAllText(Path.Combine(root, path.Replace('/', Path.DirectorySeparatorChar))).Replace("\r\n", "\n");
                Assert.AreEqual(Legacy(path), Regex.Replace(current, @"(?m)^[ \t]*_Win32_SupportedOS_\w+_\n", "")
                    .Replace("#include <win32metadata_annotations.h>\n",
                        Legacy(path).Contains("#include <win32metadata_annotations.h>\n") ? "#include <win32metadata_annotations.h>\n" : ""));
                foreach (Match match in Regex.Matches(current, @"(?m)^[ \t]*_Win32_SupportedOS_(\w+)_\n([^;]+;)"))
                {
                    string declaration = match.Groups[2].Value;
                    string name;
                    if (owner == "ObjIdlbase.h")
                        name = Regex.Match(declaration, @"^\s*(\w+)\s*:").Groups[1].Value;
                    else
                    {
                        declaration = Regex.Replace(declaration, @"//[^\r\n]*|/\*.*?\*/", "", RegexOptions.Singleline);
                        name = Regex.Matches(declaration, @"\b(\w+)\s*\(").Cast<Match>()
                            .Select(call => call.Groups[1].Value).Where(oldNames.Contains).Single();
                    }
                    Assert.IsFalse(string.IsNullOrEmpty(name), match.Value);
                    string value = $"SupportedOSPlatform(\"{versions[match.Groups[1].Value]}\")";
                    if (retired.TryGetValue(name, out var existing)) Assert.AreEqual(existing, value, name);
                    retired[name] = value;
                }
            }
            Assert.AreEqual(178, retired.Count);
            foreach (var fact in retired)
                StringAssert.Contains(oldSidecar, fact.Key + "=" + fact.Value + "\n");
            string expected = string.Join("\n", oldSidecar.Split('\n').Where(line =>
                !retired.ContainsKey(line.Split('=')[0])));
            Assert.AreEqual(expected, File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "supportedOS.rsp")).Replace("\r\n", "\n"));
        }

        [DataTestMethod]
        [DataRow("winhttp.h", "WinHttpCheckPlatform", "windows5.1.2600")]
        [DataRow("errhandlingapi.h", "GetErrorMode", "windows6.0.6000")]
        [DataRow("combaseapi.h", "CoGetCurrentProcess", "windows5.0")]
        public void ShippedNativeDeclarations_ReachFinalWinmdWithoutSidecars(string owner, string name, string version)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string header = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner));
            var declaration = Regex.Matches(header, @"(?m)^_Win32_SupportedOS_\w+_\r?\n[^;]+;")
                .Cast<Match>().Single(match => Regex.IsMatch(match.Value, @"\b" + name + @"\s*\("));
            const string prefixes = @"
#define WINHTTPAPI __declspec(dllimport)
#define WINBASEAPI __declspec(dllimport)
#define WINOLEAPI_(type) __declspec(dllimport) type __stdcall
#define WINAPI __stdcall
typedef unsigned long DWORD;
typedef unsigned int UINT;
typedef int BOOL;
typedef void VOID;
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var legacy = fixture.Scrape(prefixes + Regex.Replace(declaration.Value,
                @"_Win32_SupportedOS_\w+_", ""), sal: true,
                extraSettings: $"--with-attribute\n{name}=SupportedOSPlatform(\"{version}\")");
            Assert.AreEqual(0, legacy.ExitCode, legacy.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            var result = fixture.Scrape(prefixes + declaration.Value, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == name);
            Assert.AreEqual(version, Platform(reader, method.GetCustomAttributes().ToArray()));
            Assert.IsFalse(File.ReadAllLines(Path.Combine(root, "generation", "WinSDK", "supportedOS.rsp"))
                .Any(line => line.StartsWith(name + "=", StringComparison.Ordinal)));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeUseSites_ReachFinalWinmd(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
_Win32_SupportedOS_WindowsXP_ int __stdcall Example(void);
struct _Win32_SupportedOS_WindowsVista_ Record { int value; };
enum _Win32_SupportedOS_Windows7_ Choice { First = 1 };
struct __declspec(uuid(""00000000-0000-0000-C000-000000000046"")) Interface {
    virtual _Win32_SupportedOS_Windows8_ int __stdcall Method(void) = 0;
};
typedef _Win32_SupportedOS_Windows81_ int (__stdcall *Callback)(int);
int __stdcall Unannotated(void);
", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            foreach (var expected in new[] { ("Example", "windows5.1.2600"), ("Record", "windows6.0.6000"),
                ("Choice", "windows6.1"), ("Method", "windows8.0"), ("Callback", "windows8.1") })
            {
                var attributes = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                    .Where(type => reader.GetString(type.Name) == expected.Item1)
                    .SelectMany(type => type.GetCustomAttributes())
                    .Concat(reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Where(method => reader.GetString(method.Name) == expected.Item1)
                        .SelectMany(method => method.GetCustomAttributes()));
                Assert.AreEqual(expected.Item2, Platform(reader, attributes.ToArray()), expected.Item1);
            }
            var unannotated = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Unannotated");
            Assert.IsNull(Platform(reader, unannotated.GetCustomAttributes().ToArray()));
            var interfaceMethod = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == "Method");
            Assert.AreEqual("Interface", reader.GetString(reader.GetTypeDefinition(interfaceMethod.GetDeclaringType()).Name));
        }

        private static string? Platform(MetadataReader reader, CustomAttributeHandle[] attributes)
        {
            var values = attributes.Select(reader.GetCustomAttribute).Where(attribute =>
            {
                var constructor = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
                var type = reader.GetTypeReference((TypeReferenceHandle)constructor.Parent);
                return reader.GetString(type.Name) == "SupportedOSPlatformAttribute";
            }).Select(attribute =>
            {
                var blob = reader.GetBlobReader(attribute.Value);
                Assert.AreEqual(1, blob.ReadUInt16());
                return blob.ReadSerializedString();
            }).ToArray();
            Assert.IsTrue(values.Length <= 1, "Availability must be non-repeatable.");
            return values.SingleOrDefault();
        }
    }
}
