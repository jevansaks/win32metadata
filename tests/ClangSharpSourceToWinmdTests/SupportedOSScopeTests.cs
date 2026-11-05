using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using ClangSharp.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class SupportedOSScopeTests
    {
        public TestContext TestContext { get; set; } = null!;

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void LexicalDefaultsAndExplicitOverrides_ReachEveryProjectedTarget(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
int Before(void);
#define CURRENT_OS Windows8
_Win32_SupportedOS_BEGIN_(CURRENT_OS)
int Outer(void);
_Win32_SupportedOS_BEGIN_(WindowsVista)
int InnerEarlier(void);
_Win32_SupportedOS_BEGIN_(Windows7)
int InnerLater(void);
_Win32_SupportedOS_END_()
int InnerRestored(void);
_Win32_SupportedOS_Windows2000_ int ExplicitEarlier(void);
_Win32_SupportedOS_19041_ int ExplicitLater(void);
struct ScopedRecord { int field; struct Nested { int field; }; enum NestedEnum { NestedFirst = 1 }; };
enum ScopedEnum { First = 1 };
typedef int (__stdcall *Callback)(int parameter);
typedef long NativeCode;
struct __declspec(uuid(""a86a87bd-88b6-46ce-a5eb-05b4462e1211"")) Interface {
    virtual int __stdcall Method(int parameter) = 0;
    virtual _Win32_SupportedOS_WindowsXP_ int __stdcall ExplicitMethod(void) = 0;
};
const int NoDefaultConstant = 2;
_Win32_SupportedOS_Windows7_ const int ExplicitConstant = 3;
_Win32_SupportedOS_END_()
int OuterAgain(void);
_Win32_SupportedOS_END_()
int After(void);
struct _Win32_SupportedOS_Windows7_ ContainerOnly { int field; struct Child { int field; }; };
struct _Win32_SupportedOS_Windows7_ __declspec(uuid(""09f41e39-ea73-439a-bce5-7dfd9ac10cc2"")) ContainerInterface {
    virtual int __stdcall NoInheritance(void) = 0;
};
typedef int (__stdcall *UnannotatedCallback)(int parameter);
", target, sal: true, extraSettings: "--native-typedef\nNativeCode");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            Save(path, "lexical-" + target + ".winmd");
            Save(fixture.GeneratedSource, "lexical-" + target + ".cs");
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "Outer", "OuterAgain" }) AssertMethod(reader, name, "windows8.0");
            foreach (string name in new[] { "InnerEarlier", "InnerRestored", "Method" }) AssertMethod(reader, name, "windows6.0.6000");
            AssertMethod(reader, "InnerLater", "windows6.1");
            AssertMethod(reader, "ExplicitEarlier", "windows5.0");
            AssertMethod(reader, "ExplicitLater", "windows10.0.19041");
            AssertMethod(reader, "ExplicitMethod", "windows5.1.2600");
            foreach (string name in new[] { "Before", "After", "NoInheritance" }) AssertMethod(reader, name, null);
            foreach (string name in new[] { "ScopedRecord", "ScopedEnum", "Callback", "NativeCode", "Interface", "Nested", "NestedEnum" })
                AssertType(reader, name, "windows6.0.6000");
            foreach (string name in new[] { "ContainerOnly", "ContainerInterface" }) AssertType(reader, name, "windows6.1");
            foreach (string name in new[] { "Child", "UnannotatedCallback" }) AssertType(reader, name, null);
            foreach (var field in reader.FieldDefinitions.Select(reader.GetFieldDefinition))
                Assert.AreEqual(reader.GetString(field.Name) == "ExplicitConstant" ? "windows6.1" : null,
                    Platform(reader, field.GetCustomAttributes()), reader.GetString(field.Name));
            foreach (var parameter in reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .SelectMany(method => method.GetParameters()).Select(reader.GetParameter))
                Assert.IsNull(Platform(reader, parameter.GetCustomAttributes()));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void CDeclarations_ApplyDefaultsOnlyToProjectedDeclarationTargets(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
_Win32_SupportedOS_BEGIN_(Windows7)
struct Record { int field; };
enum Choice { First = 1 };
typedef int (__stdcall *Callback)(int parameter);
typedef long NativeCode;
_Win32_SupportedOS_WindowsXP_ typedef int (__stdcall *ExplicitCallback)(int parameter);
int __stdcall Function(int value);
_Win32_SupportedOS_END_()
int After(void);
typedef int (__stdcall *AfterCallback)(int parameter);
", target, sal: true, language: "c", extraSettings: "--native-typedef\nNativeCode");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            Save(path, "c-declarations-" + target + ".winmd");
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "Record", "Choice", "Callback", "NativeCode" })
                AssertType(reader, name, "windows6.1");
            AssertType(reader, "ExplicitCallback", "windows5.1.2600");
            AssertType(reader, "AfterCallback", null);
            AssertMethod(reader, "Function", "windows6.1");
            AssertMethod(reader, "After", null);
            foreach (var field in reader.FieldDefinitions.Select(reader.GetFieldDefinition))
                Assert.IsNull(Platform(reader, field.GetCustomAttributes()));
            foreach (var parameter in reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .SelectMany(method => method.GetParameters()).Select(reader.GetParameter))
                Assert.IsNull(Platform(reader, parameter.GetCustomAttributes()));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void IncludesAndDeclarationMacros_ResolveAtTheirExpansionInstance(string target, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var headers = new Dictionary<string, string>
            {
                ["definitions.h"] = "#define DECLARE(name) int __stdcall name(void);\n" +
                    "#define DECLARE_EXPLICIT(name) _Win32_SupportedOS_Windows2000_ int __stdcall name(void);\n",
                ["leaf.h"] = "DECLARE(TransitiveNoLeak)\nDECLARE_EXPLICIT(TransitiveExplicit)\n",
                ["inner.h"] = "#include \"leaf.h\"\n" +
                    "_Win32_SupportedOS_BEGIN_(WindowsXP)\nDECLARE(IncludedOwnScope)\n_Win32_SupportedOS_END_()\n",
                ["repeated.h"] = "#if FIRST\n_Win32_SupportedOS_BEGIN_(Windows7)\nDECLARE(FirstInstance)\n_Win32_SupportedOS_END_()\n" +
                    "#else\nDECLARE(SecondInstanceNoLeak)\n_Win32_SupportedOS_BEGIN_(Windows81)\nDECLARE(SecondInstance)\n_Win32_SupportedOS_END_()\n#endif\n",
            };
            string input = "#include \"definitions.h\"\n_Win32_SupportedOS_BEGIN_(Windows8)\n" +
                "#include \"inner.h\"\nDECLARE(CallerDefault)\nDECLARE_EXPLICIT(CallerExplicit)\n" +
                "#define FIRST 1\n#include \"repeated.h\"\n#undef FIRST\n#define FIRST 0\n#include \"repeated.h\"\n" +
                "_Win32_SupportedOS_END_()\nDECLARE(After)\n";
            string traverse = string.Join("\n", headers.Keys.Select(name => Path.Combine(fixture.DirectoryPath, name)));
            var result = fixture.Scrape(input, target, sal: true, language: language, headers: headers,
                extraSettings: "--traverse\n" + traverse);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            AssertMethod(reader, "CallerDefault", "windows8.0");
            AssertMethod(reader, "IncludedOwnScope", "windows5.1.2600");
            AssertMethod(reader, "FirstInstance", "windows6.1");
            AssertMethod(reader, "SecondInstance", "windows8.1");
            foreach (string name in new[] { "TransitiveExplicit", "CallerExplicit" }) AssertMethod(reader, name, "windows5.0");
            foreach (string name in new[] { "After", "TransitiveNoLeak", "SecondInstanceNoLeak" }) AssertMethod(reader, name, null);
            Save(path, "includes-" + language + "-" + target + ".winmd");
        }

        [DataTestMethod]
        [DataRow("_Win32_SupportedOS_END_()", "no BEGIN")]
        [DataRow("_Win32_SupportedOS_BEGIN_(Windows7)\nint Example(void);", "Unclosed")]
        [DataRow("_Win32_SupportedOS_BEGIN_(NotReviewed)\n_Win32_SupportedOS_END_()", "input.h:")]
        [DataRow("_Win32_SupportedOS_BEGIN_(Windows7)\n#if 0\n_Win32_SupportedOS_END_()\n#endif", "Unclosed")]
        [DataRow("#if 0\n_Win32_SupportedOS_BEGIN_(Windows7)\n#endif\n_Win32_SupportedOS_END_()", "no BEGIN")]
        [DataRow("_Win32_SupportedOS_BEGIN_(Windows7)\n_Win32_SupportedOS_WindowsXP_ _Win32_SupportedOS_Windows8_ int Example(void);\n_Win32_SupportedOS_END_()", "Conflicting")]
        [DataRow("_Win32_SupportedOS_BEGIN_(Windows7)\nint Example(_Win32_SupportedOS_WindowsXP_ int value);\n_Win32_SupportedOS_END_()", "requires")]
        [DataRow("_Win32_SupportedOS_BEGIN_(Windows7)\nstruct Record { _Win32_SupportedOS_WindowsXP_ int field; };\n_Win32_SupportedOS_END_()", "requires")]
        [DataRow("struct Interface {\n_Win32_SupportedOS_BEGIN_(Windows7)\nvirtual int Method(int value) = 0;\n_Win32_SupportedOS_END_()\nvirtual int Method(double value) = 0;\n};", "overloaded")]
        [DataRow("struct Interface {\nvirtual _Win32_SupportedOS_Windows7_ int Method(int value) = 0;\nvirtual int Method(double value) = 0;\n};", "overloaded")]
        public void InvalidActivePaths_FailAtSourceWithoutOutput(string header, string diagnostic)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, diagnostic);
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c", false)]
        [DataRow("x86_64-pc-windows-msvc", "c", false)]
        [DataRow("aarch64-pc-windows-msvc", "c", false)]
        [DataRow("i686-pc-windows-msvc", "c++", false)]
        [DataRow("x86_64-pc-windows-msvc", "c++", false)]
        [DataRow("aarch64-pc-windows-msvc", "c++", false)]
        [DataRow("i686-pc-windows-msvc", "c++", true)]
        [DataRow("x86_64-pc-windows-msvc", "c++", true)]
        [DataRow("aarch64-pc-windows-msvc", "c++", true)]
        public void ConstParameterAvailability_IsRejectedBeforeGeneration(string target, string language, bool carrier)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string header = carrier
                ? "struct Interface { virtual int __stdcall Method(const int value) = 0; };\n" +
                    "_Win32_AnnotateParameter_(Interface::Method, value, _Win32_SupportedOS_WindowsXP_);"
                : "int Example(_Win32_SupportedOS_WindowsXP_ const int value);";
            var result = fixture.Scrape(header, target, sal: true, language: language);
            this.TestContext.WriteLine($"ExitCode={result.ExitCode}; GeneratedSource={File.Exists(fixture.GeneratedSource)}");
            this.TestContext.WriteLine(result.Output);
            if (File.Exists(fixture.GeneratedSource))
                Save(fixture.GeneratedSource, $"const-parameter-{target}-{language}-{carrier}.cs");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "requires");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", false)]
        [DataRow("x86_64-pc-windows-msvc", false)]
        [DataRow("aarch64-pc-windows-msvc", false)]
        [DataRow("i686-pc-windows-msvc", true)]
        [DataRow("x86_64-pc-windows-msvc", true)]
        [DataRow("aarch64-pc-windows-msvc", true)]
        public void ConstLocalAvailability_IsRejectedBeforeGeneration(string target, bool staticLocal)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("void LocalOwner(void) {\n_Win32_SupportedOS_WindowsXP_ " +
                (staticLocal ? "static " : "") + "const int LocalConstant = 1;\n}\nint Projected(void);",
                target, sal: true);
            this.TestContext.WriteLine($"ExitCode={result.ExitCode}; GeneratedSource={File.Exists(fixture.GeneratedSource)}");
            this.TestContext.WriteLine(result.Output);
            if (File.Exists(fixture.GeneratedSource))
                Save(fixture.GeneratedSource, $"local-constant-{target}-{staticLocal}.cs");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "requires");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ProjectedConstAvailability_PreservesFileMemberAndEnumConstants(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
_Win32_SupportedOS_BEGIN_(Windows8)
_Win32_SupportedOS_WindowsXP_ extern const int DeclaredConstant;
const int DeclaredConstant = 7;
_Win32_SupportedOS_Windows7_ const int FileConstant = 3;
struct Record {
    _Win32_SupportedOS_Windows7_ static const int MemberConstant = 4;
    static const int NoDefaultConstant = 6;
};
enum Choice { EnumConstant _Win32_SupportedOS_Windows7_ = 5 };
_Win32_SupportedOS_END_()
", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var expected = new Dictionary<string, string?>
            {
                ["FileConstant"] = "windows6.1",
                ["MemberConstant"] = "windows6.1",
                ["EnumConstant"] = "windows6.1",
                ["DeclaredConstant"] = "windows5.1.2600",
                ["NoDefaultConstant"] = null,
            };
            var fields = reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Where(field => expected.ContainsKey(reader.GetString(field.Name)))
                .ToDictionary(field => reader.GetString(field.Name));
            foreach (var fact in expected)
            {
                Assert.AreEqual(fact.Value, Platform(reader, fields[fact.Key].GetCustomAttributes()), fact.Key);
                Assert.IsFalse(fields[fact.Key].GetDefaultValue().IsNil, fact.Key);
            }
            Save(path, "projected-constants-" + target + ".winmd");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void UninitializedConstAvailability_IsRejectedBeforeGeneration(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("_Win32_SupportedOS_WindowsXP_ extern const int ExternalValue;", target, sal: true);
            this.TestContext.WriteLine($"ExitCode={result.ExitCode}; GeneratedSource={File.Exists(fixture.GeneratedSource)}");
            this.TestContext.WriteLine(result.Output);
            if (File.Exists(fixture.GeneratedSource))
                Save(fixture.GeneratedSource, "extern-constant-" + target + ".cs");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            StringAssert.Contains(result.Output, "requires");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [DataTestMethod]
        [DataRow("_Win32_SupportedOS_BEGIN_(Windows7)\n#include \"inner.h\"\n", "_Win32_SupportedOS_END_()")]
        [DataRow("#include \"inner.h\"\n_Win32_SupportedOS_END_()", "_Win32_SupportedOS_BEGIN_(Windows7)")]
        [DataRow("#define FIRST 1\n#include \"inner.h\"\n#undef FIRST\n#include \"inner.h\"",
            "#if FIRST\n_Win32_SupportedOS_BEGIN_(Windows7)\n#else\n_Win32_SupportedOS_END_()\n#endif")]
        [DataRow("#define FIRST 1\n#include \"inner.h\"",
            "#if FIRST\n_Win32_SupportedOS_BEGIN_(Windows7)\n#undef FIRST\n#include \"inner.h\"\n_Win32_SupportedOS_END_()\n#else\n_Win32_SupportedOS_END_()\n#endif")]
        public void CrossFileAndRepeatedIncludePairing_CannotPopAnotherInstance(string header, string inner)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(header, sal: true, headers: new Dictionary<string, string> { ["inner.h"] = inner });
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, ".h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void InactiveScopesAndUnknownTokens_DoNotAffectTheActivePath()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#if 0\n_Win32_SupportedOS_END_()\n_Win32_SupportedOS_BEGIN_(NotReviewed)\n#endif\n" +
                "_Win32_SupportedOS_BEGIN_(Windows7)\nint Inside(void);\n_Win32_SupportedOS_END_()\nint After(void);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            AssertMethod(pe.GetMetadataReader(), "Inside", "windows6.1");
            AssertMethod(pe.GetMetadataReader(), "After", null);
        }

        [TestMethod]
        public void PresumedFilenames_DoNotReplacePhysicalExpansionIdentity()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("#line 1 \"same.h\"\n_Win32_SupportedOS_BEGIN_(Windows7)\n" +
                "#include \"inner.h\"\nint Caller(void);\n_Win32_SupportedOS_END_()\n", sal: true,
                headers: new Dictionary<string, string> { ["inner.h"] = "#line 1 \"same.h\"\nint Included(void);\n" },
                extraSettings: "--traverse\n" + Path.Combine(fixture.DirectoryPath, "inner.h") +
                    "\n" + Path.Combine(fixture.DirectoryPath, "same.h"));
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            AssertMethod(pe.GetMetadataReader(), "Caller", "windows6.1");
            AssertMethod(pe.GetMetadataReader(), "Included", null);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void BareCallbackTypedefAndSelectedNativeAlias_HaveIndependentAvailability(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
_Win32_SupportedOS_BEGIN_(Windows7)
typedef int __stdcall Function(int value);
typedef Function* Callback;
_Win32_SupportedOS_Windows2000_ typedef unsigned long NativeValue;
typedef NativeValue Alias;
_Win32_SupportedOS_END_()
", target, sal: true, extraSettings: "--native-typedef\nNativeValue\nAlias");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(path));
            AssertType(pe.GetMetadataReader(), "Callback", "windows6.1");
            AssertType(pe.GetMetadataReader(), "NativeValue", "windows5.0");
            AssertType(pe.GetMetadataReader(), "Alias", "windows6.1");
            Save(path, "typedefs-" + target + ".winmd");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NamedMethodCarrier_IsExplicitRatherThanCarrierLexicalDefault(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
_Win32_SupportedOS_BEGIN_(Windows8)
struct __declspec(uuid(""ab0dc58b-f1cf-4fc7-a3bf-afed9bbf3c98"")) Interface {
    virtual int __stdcall Method(void) = 0;
    virtual int __stdcall Neighbor(void) = 0;
};
_Win32_SupportedOS_END_()
_Win32_SupportedOS_BEGIN_(Windows7)
_Win32_AnnotateMethod_(Interface::Method, _Win32_SupportedOS_WindowsXP_);
_Win32_SupportedOS_END_()
", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            AssertMethod(reader, "Method", "windows5.1.2600");
            AssertMethod(reader, "Neighbor", "windows8.0");
            AssertType(reader, "Interface", "windows8.0");
            Assert.IsFalse(reader.FieldDefinitions.Select(reader.GetFieldDefinition)
                .Any(field => reader.GetString(field.Name).Contains("_WIN32META_")));
            Save(path, "carrier-" + target + ".winmd");
        }

        [DataTestMethod]
        [DataRow("_Win32_SupportedOS_Windows7_", "_Win32_AnnotateMethod_(Interface::Method, _Win32_SupportedOS_WindowsXP_);")]
        [DataRow("", "_Win32_AnnotateMethod_(Interface::Method, _Win32_SupportedOS_WindowsXP_);\n_Win32_AnnotateMethod_(Interface::Method, _Win32_SupportedOS_Windows7_);")]
        [DataRow("", "_Win32_AnnotateParameter_(Interface::Method, value, _Win32_SupportedOS_WindowsXP_);")]
        public void InvalidAndConflictingCarriers_AreSourceLocated(string annotation, string carrier)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("struct Interface { virtual " + annotation +
                " int __stdcall Method(int value) = 0; };\n" + carrier, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void NativeAssertions_AreNotScopesAndNeverBecomeMetadata()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
static_assert(sizeof(int) == 4, ""native assertion"");
static_assert(1, ""Win32Metadata unrelated assertion"");
static_assert(1, ""Win32Metadata.SupportedOSBegin"");
_Win32_SupportedOS_BEGIN_(Windows7)
struct Record { static_assert(sizeof(int) == 4, ""native member assertion""); int field; };
int Inside(void);
_Win32_SupportedOS_END_()
int After(void);
", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = fixture.Emit();
            using var pe = new PEReader(File.OpenRead(path));
            AssertType(pe.GetMetadataReader(), "Record", "windows6.1");
            AssertMethod(pe.GetMetadataReader(), "Inside", "windows6.1");
            AssertMethod(pe.GetMetadataReader(), "After", null);
            foreach (var encoding in new[] { Encoding.UTF8, Encoding.Unicode })
            {
                string text = encoding.GetString(File.ReadAllBytes(path));
                Assert.IsFalse(text.Contains("SupportedOSBegin"));
                Assert.IsFalse(text.Contains("SupportedOSEnd"));
                Assert.IsFalse(text.Contains("native assertion"));
            }
            var failing = fixture.Scrape("static_assert(0, \"native failure must survive\");", sal: true);
            Assert.AreNotEqual(0, failing.ExitCode, failing.Output);
            StringAssert.Contains(failing.Output, "native failure must survive");
            StringAssert.Contains(failing.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        [TestMethod]
        public void FixedMenu_IsSharedByExplicitAndScopedAnnotations()
        {
            string helper = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "shared", "win32metadata_annotations.h"));
            var tokens = Regex.Matches(helper, @"(?m)^#define _WIN32META_OS_VERSION_(\w+) ""SupportedOSPlatform")
                .Cast<Match>().Select(match => match.Groups[1].Value).ToArray();
            Assert.AreEqual(20, tokens.Length);
            using var fixture = new LastErrorHeaderTests.Fixture();
            string explicitDeclarations = string.Join("\n", tokens.Select((token, i) =>
                "_Win32_SupportedOS_" + token + "_ int Menu" + i + "(void);"));
            var result = fixture.Scrape(explicitDeclarations, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            string scopes = string.Join("\n", tokens.Select((token, i) =>
                "_Win32_SupportedOS_BEGIN_(" + token + ")\nint Menu" + i + "(void);\n_Win32_SupportedOS_END_()"));
            result = fixture.Scrape(scopes, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit()));
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", "c")]
        [DataRow("x86_64-pc-windows-msvc", "c")]
        [DataRow("aarch64-pc-windows-msvc", "c")]
        [DataRow("i686-pc-windows-msvc", "c++")]
        [DataRow("x86_64-pc-windows-msvc", "c++")]
        [DataRow("aarch64-pc-windows-msvc", "c++")]
        public void ClangWithoutMetadata_ScopesAndExplicitAnnotationsAreNoOps(string target, string language)
        {
            const string declarations = @"
_Win32_SupportedOS_BEGIN_(Windows8)
struct _Win32_SupportedOS_Windows7_ Record { int field; };
enum Choice { First = 1 };
typedef int (__stdcall *Callback)(int parameter);
int __stdcall Function(int value);
_Win32_SupportedOS_END_()
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(declarations, target, sal: true, language: language, metadata: false);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            result = fixture.Scrape(Regex.Replace(declarations, @"_Win32_SupportedOS_(?:BEGIN_\(\w+\)|END_\(\)|\w+_)", ""),
                target, sal: true, language: language, metadata: false);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit()));
        }

        [DataTestMethod]
        [DataRow("x86", 4, "/TC")]
        [DataRow("x64", 8, "/TC")]
        [DataRow("arm64", 8, "/TC")]
        [DataRow("x86", 4, "/TP")]
        [DataRow("x64", 8, "/TP")]
        [DataRow("arm64", 8, "/TP")]
        public void NativeScopeMacros_PreserveLayoutsCallingConventionsSymbolsAndVtables(string arch, int pointerSize, string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            fixture.AssertNativeAbi(arch, pointerSize, language, @"
_Win32_SupportedOS_BEGIN_(Windows8)
typedef long NativeCode;
typedef int (__stdcall *ScopeCallback)(int);
struct ScopeRecord { ScopeCallback callback; long value; };
typedef char CheckScopeLayout[(sizeof(struct ScopeRecord) == 2 * sizeof(void*)) ? 1 : -1];
int __stdcall ScopeImplementation(int value) { return value + 1; }
ScopeCallback verifyScopeCallingConvention = &ScopeImplementation;
_Win32_SupportedOS_BEGIN_(Windows2000)
enum ScopeEnum { ScopeFirst = 1, ScopeLast = 7 };
_Win32_SupportedOS_END_()
#ifdef __cplusplus
struct ScopeInterface {
    virtual int __stdcall Method(int value) = 0;
};
struct ScopeImplementationClass : ScopeInterface {
    int __stdcall Method(int value) override { return value + 1; }
};
ScopeImplementationClass nativeVtable;
int __stdcall CallScopeVtable(ScopeInterface* value) { return value->Method(3); }
#else
struct ScopeVtable { ScopeCallback Method; };
struct ScopeVtable nativeVtable = { &ScopeImplementation };
int __stdcall CallScopeVtable(struct ScopeVtable* value) { return value->Method(3); }
#endif
_Win32_SupportedOS_END_()
");
            string prefix = "scope-native-" + arch + "-" + language.TrimStart('/');
            Save(fixture.OrdinaryNativeObject, prefix + "-ordinary.obj");
            Save(fixture.MetadataNativeObject, prefix + "-metadata.obj");
            Save(Path.Combine(fixture.DirectoryPath, "abi.c"), prefix + ".c");
            Save(Path.Combine(fixture.DirectoryPath, "win32metadata_annotations.h"), "win32metadata_annotations.h");
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", false)]
        [DataRow("x86_64-pc-windows-msvc", false)]
        [DataRow("aarch64-pc-windows-msvc", false)]
        [DataRow("i686-pc-windows-msvc", true)]
        [DataRow("x86_64-pc-windows-msvc", true)]
        [DataRow("aarch64-pc-windows-msvc", true)]
        public void CompleteSdkHeaderAndPchReuse_PreserveReviewedAvailabilityWithoutIncludeLeakage(string target, bool pch)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            string root = LastErrorHeaderTests.RepositoryRoot();
            string sdk = Path.Combine(root, "generation", "WinSDK");
            string shipped = File.ReadAllText(Path.Combine(sdk, "RecompiledIdlHeaders", "um", "handleapi.h"));
            int facts = Regex.Matches(shipped, @"(?m)^_Win32_SupportedOS_\w+_\r?$").Count;
            Assert.AreEqual(5, facts, "Factor only the five already reviewed availability facts, never version guards.");
            string scoped = Regex.Replace(shipped, @"(?m)^_Win32_SupportedOS_(\w+)_\r?\n([^;]+;)", match =>
                "_Win32_SupportedOS_BEGIN_(" + match.Groups[1].Value + ")\n" +
                match.Groups[2].Value + "\n_Win32_SupportedOS_END_()");
            Assert.AreEqual(5, Regex.Matches(scoped, "_Win32_SupportedOS_BEGIN_").Count);
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            var vc = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". '" + Path.Combine(root, "scripts", "CommonUtils.ps1").Replace("'", "''") +
                "'; [IO.Path]::GetFullPath((Join-Path (Get-VcDirPath -Arch x64 -HostArch x64) '..\\..\\..\\include'))");
            Assert.AreEqual(0, vc.ExitCode, vc.Output);
            string[] directories = { Path.Combine(sdk, "inc"), Path.Combine(sdk, "RecompiledIdlHeaders", "shared"),
                Path.Combine(sdk, "RecompiledIdlHeaders", "um"), Path.Combine(include, "shared"),
                Path.Combine(include, "um"), Path.Combine(include, "ucrt"), vc.Output.Trim() };
            string settings = "--include-directory\n" + string.Join("\n", directories) +
                "\n--traverse\n" + Path.Combine(fixture.DirectoryPath, "pilot.h") +
                "\n" + Path.Combine(fixture.DirectoryPath, "repeat.h") +
                "\n" + Path.Combine(fixture.DirectoryPath, "cached.h");
            var headers = new Dictionary<string, string>
            {
                ["pilot.h"] = shipped,
                ["repeat.h"] = "#if FIRST\n_Win32_SupportedOS_BEGIN_(Windows7)\nextern \"C\" int __stdcall FirstInstance(void);\n_Win32_SupportedOS_END_()\n" +
                    "#else\nextern \"C\" int __stdcall SecondInstance(void);\n#endif",
                ["cached.h"] = "#pragma once\n#include \"win32metadata_annotations.h\"\n#include \"sal.h\"\n#define _APISETHANDLE_\n#include <windows.h>\n#undef _APISETHANDLE_\n#include \"pilot.h\"\n" +
                    "#define FIRST 1\n#include \"repeat.h\"\n#undef FIRST\n",
            };
            const string input = "#include \"cached.h\"\n#define FIRST 0\n#include \"repeat.h\"\n" +
                "_Win32_SupportedOS_BEGIN_(Windows8)\nint __stdcall Caller(void);\n_Win32_SupportedOS_END_()\nint __stdcall After(void);";
            var result = fixture.Scrape(input, target, sal: true, headers: headers, extraSettings: settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string baseline = fixture.Emit();
            byte[] expected = File.ReadAllBytes(baseline);
            Save(baseline, "sdk-explicit-" + target + (pch ? "-pch-control" : "") + ".winmd");
            headers["pilot.h"] = scoped;
            foreach (var header in headers)
                File.WriteAllText(Path.Combine(fixture.DirectoryPath, header.Key), header.Value);
            if (pch)
            {
                string path = CreatePch(fixture, target, directories);
                settings += "\n--additional\n-include-pch\n" + path;
            }
            result = fixture.Scrape(input, target, sal: true, extraSettings: settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            Save(output, "sdk-scoped-" + target + (pch ? "-pch" : "") + ".winmd");
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            foreach (string name in new[] { "CloseHandle", "DuplicateHandle", "GetHandleInformation", "SetHandleInformation" })
                AssertMethod(reader, name, "windows5.0");
            AssertMethod(reader, "CompareObjectHandles", "windows10.0.10240");
            AssertMethod(reader, "FirstInstance", "windows6.1");
            AssertMethod(reader, "SecondInstance", null);
            AssertMethod(reader, "Caller", "windows8.0");
            AssertMethod(reader, "After", null);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc", false)]
        [DataRow("x86_64-pc-windows-msvc", false)]
        [DataRow("aarch64-pc-windows-msvc", false)]
        [DataRow("i686-pc-windows-msvc", true)]
        [DataRow("x86_64-pc-windows-msvc", true)]
        [DataRow("aarch64-pc-windows-msvc", true)]
        public void PchRootScopes_RetainTheirOwnExpansionInstance(string target, bool reuseRoot)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var headers = new Dictionary<string, string>
            {
                ["cached.h"] = (reuseRoot ? "" : "#pragma once\n") +
                    "#include \"win32metadata_annotations.h\"\n#include \"sal.h\"\n#if !defined(ROOT_SEEN)\n#define ROOT_SEEN\nextern \"C\" {\n_Win32_SupportedOS_BEGIN_(Windows7)\n" +
                    "int Cached(void);\n#define FIRST 1\n#include \"repeat.h\"\n#undef FIRST\n_Win32_SupportedOS_END_()\n}\n" +
                    "#else\nextern \"C\" int RootSecondUnscoped(void);\n#endif\n",
                ["repeat.h"] = "#if FIRST\nint FirstUnscoped(void);\n#else\nint SecondUnscoped(void);\n#endif\n",
            };
            const string input = "#include \"cached.h\"\n#define FIRST 0\n#include \"repeat.h\"\nint After(void);";
            string control = (reuseRoot ? "#include \"cached.h\"\n" : "") + input;
            string settings = "--traverse\n" + Path.Combine(fixture.DirectoryPath, "cached.h") +
                "\n" + Path.Combine(fixture.DirectoryPath, "repeat.h");
            var result = fixture.Scrape(control, target, sal: true, headers: headers, extraSettings: settings);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            Save(Path.Combine(fixture.DirectoryPath, "Test.winmd"), "pch-root-control-" + target + ".winmd");
            Save(fixture.GeneratedSource, "pch-root-control-" + target + ".cs");
            string pch = CreatePch(fixture, target, Array.Empty<string>());
            result = fixture.Scrape(input, target, sal: true,
                extraSettings: settings + "\n--additional\n-include-pch\n" + pch);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            Save(output, "pch-root-" + target + ".winmd");
            Save(fixture.GeneratedSource, "pch-root-" + target + ".cs");
            using var pe = new PEReader(File.OpenRead(output));
            AssertMethod(pe.GetMetadataReader(), "Cached", "windows6.1");
            foreach (string name in new[] { "FirstUnscoped", "SecondUnscoped", "After" })
                AssertMethod(pe.GetMetadataReader(), name, null);
            if (reuseRoot) AssertMethod(pe.GetMetadataReader(), "RootSecondUnscoped", null);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
        }

        [TestMethod]
        public void PchWithoutIncludeRecords_CannotConflateRepeatedPhysicalFiles()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var headers = new Dictionary<string, string>
            {
                ["cached.h"] = "#include \"win32metadata_annotations.h\"\n#define FIRST 1\n#include \"repeat.h\"\n#undef FIRST\n#include \"repeat.h\"\n",
                ["repeat.h"] = "#if FIRST\n_Win32_SupportedOS_BEGIN_(Windows7)\n#else\n_Win32_SupportedOS_END_()\n#endif\n",
            };
            var result = fixture.Scrape("int Setup(void);", sal: true, headers: headers);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string pch = CreatePch(fixture, "x86_64-pc-windows-msvc", Array.Empty<string>(), detailed: false);
            result = fixture.Scrape("int After(void);", sal: true, extraSettings: "--additional\n-include-pch\n" + pch);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "repeat.h:");
            StringAssert.Contains(result.Output, "preserve the detailed preprocessing record");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }

        private static string CreatePch(LastErrorHeaderTests.Fixture fixture, string target, IEnumerable<string> directories,
            bool detailed = true)
        {
            NativeLibrary.Load(Path.Combine(AppContext.BaseDirectory, RuntimeInformation.RuntimeIdentifier, "libclang.dll"));
            string path = Path.Combine(fixture.DirectoryPath, "cached.pch");
            var args = new List<string> { "-x", "c++-header", "-std=c++17", "-target", target,
                "-DWIN32METADATA", "-D_USE_DECLSPECS_FOR_SAL=1", "-D_PREFAST_=1", "-Wno-ignored-attributes",
                "-I" + fixture.DirectoryPath };
            args.AddRange(directories.Select(directory => "-I" + directory));
            using var index = CXIndex.Create(excludeDeclarationsFromPch: false, displayDiagnostics: false);
            var error = CXTranslationUnit.TryParse(index, Path.Combine(fixture.DirectoryPath, "cached.h"),
                args.ToArray(), ReadOnlySpan<CXUnsavedFile>.Empty,
                CXTranslationUnit_Flags.CXTranslationUnit_ForSerialization |
                (detailed ? CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord : 0) |
                CXTranslationUnit_Flags.CXTranslationUnit_IncludeAttributedTypes |
                CXTranslationUnit_Flags.CXTranslationUnit_VisitImplicitAttributes, out var unit);
            Assert.AreEqual(CXErrorCode.CXError_Success, error);
            using (unit)
            {
                for (uint i = 0; i < unit.NumDiagnostics; i++)
                {
                    using var diagnostic = unit.GetDiagnostic(i);
                    Assert.IsTrue(diagnostic.Severity < CXDiagnosticSeverity.CXDiagnostic_Error,
                        diagnostic.Format(CXDiagnostic.DefaultDisplayOptions).ToString());
                }
                Assert.AreEqual(CXSaveError.CXSaveError_None, unit.Save(path, CXSaveTranslationUnit_Flags.CXSaveTranslationUnit_None));
            }
            args[1] = "c++";
            args.AddRange(new[] { "-include-pch", path });
            using var consumerIndex = CXIndex.Create(excludeDeclarationsFromPch: false, displayDiagnostics: true);
            error = CXTranslationUnit.TryParse(consumerIndex, Path.Combine(fixture.DirectoryPath, "main.cpp"),
                args.ToArray(), ReadOnlySpan<CXUnsavedFile>.Empty,
                CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord, out var consumer);
            Assert.AreEqual(CXErrorCode.CXError_Success, error, "The same libclang must be able to consume its PCH.");
            consumer.Dispose();
            return path;
        }

        private static void AssertMethod(MetadataReader reader, string name, string? version)
        {
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(method => reader.GetString(method.Name) == name);
            Assert.AreEqual(version, Platform(reader, method.GetCustomAttributes()), name);
        }

        private static void AssertType(MetadataReader reader, string name, string? version)
        {
            var matches = reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                .Where(type => reader.GetString(type.Name) == name).ToArray();
            Assert.AreEqual(1, matches.Length, name + "; actual types: " + string.Join(",",
                reader.TypeDefinitions.Select(reader.GetTypeDefinition).Select(type => reader.GetString(type.Name))));
            var type = matches.Single();
            Assert.AreEqual(version, Platform(reader, type.GetCustomAttributes()), name);
        }

        private static string? Platform(MetadataReader reader, CustomAttributeHandleCollection attributes)
        {
            var values = attributes.Select(reader.GetCustomAttribute)
                .Where(attribute => ParameterHeaderTests.AttributeName(reader, attribute) == "SupportedOSPlatformAttribute")
                .Select(attribute =>
                {
                    var blob = reader.GetBlobReader(attribute.Value);
                    Assert.AreEqual((ushort)1, blob.ReadUInt16());
                    string? version = blob.ReadSerializedString();
                    Assert.AreEqual((ushort)0, blob.ReadUInt16());
                    Assert.AreEqual(0, blob.RemainingBytes);
                    return version;
                }).ToArray();
            Assert.IsTrue(values.Length <= 1, "Availability is not repeatable.");
            return values.SingleOrDefault();
        }

        private void Save(string path, string name)
        {
            string destination = Path.Combine(TestContext.ResultsDirectory, name);
            File.Copy(path, destination, true);
            TestContext.AddResultFile(destination);
        }
    }
}
