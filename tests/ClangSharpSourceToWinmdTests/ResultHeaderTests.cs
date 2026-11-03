using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class ResultHeaderTests
    {
        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedFunction_HeaderLegacyBothAndNeither(string target)
        {
            string header = Owner("objbase.h");
            string declaration = Regex.Match(header, @"_Check_return_ WINOLEAPI  CoInitialize\([^;]+;").Value;
            Assert.IsFalse(string.IsNullOrEmpty(declaration));
            const string types = "#define WINOLEAPI __declspec(dllimport) HRESULT __stdcall\n" +
                "typedef long HRESULT;\ntypedef void* LPVOID;\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            byte[]? expected = null;
            foreach (var mode in new[] { (Header: false, Legacy: true), (Header: true, Legacy: false),
                (Header: true, Legacy: true), (Header: false, Legacy: false) })
            {
                var result = fixture.Scrape(types + (mode.Header ? "_Win32_PreserveResult_\n" : "") +
                    declaration, target, sal: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string output = fixture.Emit(mode.Legacy ? new Dictionary<string, string>
                    { ["CoInitialize"] = "[PreserveSig]" } : null);
                AssertResult(output, "CoInitialize", mode.Header || mode.Legacy);
                if (expected == null) expected = File.ReadAllBytes(output);
                if (mode.Header) CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedComMethod_PreservesOnlyAuthoredResult(string target)
        {
            string header = Owner("ObjIdlbase.h");
            string Method(string name)
            {
                string method = Regex.Match(header, @"virtual /\* \[local\] \*/ HRESULT STDMETHODCALLTYPE " +
                    name + @"\([\s\S]*?\) = 0;").Value;
                Assert.IsFalse(string.IsNullOrEmpty(method), name);
                return method;
            }
            const string prefix = "typedef long HRESULT; typedef unsigned long ULONG;\n" +
                "#define STDMETHODCALLTYPE __stdcall\n" +
                "struct __declspec(uuid(\"0c733a30-2a1c-11ce-ade5-00aa0044773d\")) ISequentialStream {\n";
            using var fixture = new LastErrorHeaderTests.Fixture();
            string read = Method("Read"), write = Method("Write");
            var result = fixture.Scrape(prefix + read + write + "};", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(new Dictionary<string, string>
                { ["ISequentialStream::Read"] = "[PreserveSig]" }));
            result = fixture.Scrape(prefix + "_Win32_PreserveResult_\n" + read + write + "};", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertResult(output, "Read", true);
            AssertResult(output, "Write", false);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(new Dictionary<string, string>
                { ["ISequentialStream::Read"] = "[PreserveSig]" })));
        }

        [TestMethod]
        public void ResultAndNativeRetval_RemainIndependent()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"struct __declspec(uuid(""64abea93-aac7-4f3b-a257-976deba89f04"")) IExample {
_Win32_PreserveResult_ virtual int __stdcall Method(_Out_ _Win32_Retval_ int* value) = 0;
};", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertResult(output, "Method", true);
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Method");
            ParameterHeaderTests.AssertAttribute(reader, reader.GetParameter(method.GetParameters().Single()), "RetValAttribute");
        }

        [DataTestMethod]
        [DataRow("Win32Metadata.PreserveResult()")]
        [DataRow("Win32Metadata.PreserveResult(true)")]
        [DataRow("Win32Metadata.PreserveResult=false")]
        public void MalformedResult_IsSourceLocated(string payload)
        {
            Reject("__attribute__((annotate(\"" + payload + "\"))) int __stdcall Example(void);");
        }

        [DataTestMethod]
        [DataRow("_Win32_PreserveResult_ typedef int Alias;")]
        [DataRow("_Win32_PreserveResult_ typedef int (__stdcall *Callback)(void);")]
        [DataRow("struct _Win32_PreserveResult_ Value { int field; };")]
        [DataRow("enum _Win32_PreserveResult_ Value { One = 1 };")]
        [DataRow("struct Value { _Win32_PreserveResult_ int field; };")]
        [DataRow("int __stdcall Example(_Win32_PreserveResult_ int* value);")]
        [DataRow("int __stdcall _Win32_PreserveResult_ Example(void);")]
        [DataRow("int __stdcall Example(void) _Win32_PreserveResult_;")]
        [DataRow("_Win32_PreserveResult_ int Example(void) { return 0; }")]
        [DataRow("_Win32_PreserveResult_ _Win32_PreserveResult_ int __stdcall Example(void);")]
        [DataRow("__attribute__((annotate(\"Win32Metadata.PreserveResult\", 1))) int __stdcall Example(void);")]
        [DataRow("int __stdcall Example(void);\n_Win32_PreserveResult_ int __stdcall Example(void);")]
        [DataRow("struct Value { virtual int __stdcall _Win32_PreserveResult_ Method(void) = 0; };")]
        public void WrongSitesArgumentsDuplicatesAndLateDeclarations_Fail(string declaration) => Reject(declaration);

        [DataTestMethod]
        [DataRow("_Win32_Retained_ int __stdcall Example(void);")]
        [DataRow("_Win32_ContainsInteriorPointers_ int __stdcall Example(void);")]
        [DataRow("struct Value { _Win32_Retained_ int* field; };")]
        [DataRow("struct Value { _Win32_ContainsInteriorPointers_ int* field; };")]
        [DataRow("int __stdcall Example(_Win32_Retained_ _Win32_Retained_ int* value);")]
        [DataRow("int __stdcall Example(_Out_writes_(2) _Win32_ContainsInteriorPointers_ _Win32_ContainsInteriorPointers_ int* value);")]
        [DataRow("int __stdcall Example(__attribute__((annotate(\"Win32Metadata.Retained=true\"))) int* value);")]
        [DataRow("int __stdcall Example(__attribute__((annotate(\"Win32Metadata.ContainsInteriorPointers()\"))) int* value);")]
        public void LifetimeMarkers_KeepStrictPlacementAndValuelessContracts(string declaration) => Reject(declaration);

        [TestMethod]
        public void CoreOwnerInventory_IsExhaustedAndSupportAddsNoProductionFacts()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (string owner in new[] { "winhttp.h", "errhandlingapi.h", "handleapi.h",
                "combaseapi.h", "objbase.h", "ObjIdl.h", "ObjIdlbase.h" })
            {
                string header = Owner(owner);
                Assert.IsFalse(header.Contains("_Win32_PreserveResult_", StringComparison.Ordinal), owner);
                foreach (Match match in Regex.Matches(header, @"\b(\w+)\s*\("))
                    names.Add(match.Groups[1].Value);
                foreach (Match match in Regex.Matches(header, @"\b(I\w+)\s*:"))
                    names.Add(match.Groups[1].Value);
            }
            foreach (string sidecar in new[] { "emitter.settings.rsp", "scraper.settings.rsp",
                "Partitions/WinHttp/settings.rsp", "Partitions/Foundation/settings.rsp", "Partitions/Com/settings.rsp" })
            {
                string content = File.ReadAllText(Path.Combine(root, "generation", "WinSDK",
                    sidecar.Replace('/', Path.DirectorySeparatorChar)));
                Assert.IsFalse(content.Contains("PreserveSig", StringComparison.Ordinal), sidecar);
                Assert.IsFalse(content.Contains("PreserveResult", StringComparison.Ordinal), sidecar);
                foreach (string line in content.Split('\n').Where(line => line.Contains("[Retained]") ||
                    line.Contains("[ContainsInteriorPointers]")))
                {
                    string selector = line.Split('=')[0];
                    Assert.IsFalse(selector.Contains('*'), "A wildcard must not silently resupply a retired marker: " + line);
                    Assert.IsFalse(names.Contains(selector.Split(new[] { "::" }, StringSplitOptions.None)[0]), line);
                }
            }
            Assert.AreEqual(1, Regex.Matches(Owner("winhttp.h"), "_Win32_ContainsInteriorPointers_").Count);
            Assert.AreEqual(2, Regex.Matches(Owner("fileapi.h"), "_Win32_Retained_").Count);
        }

        [DataTestMethod]
        [DataRow("c")]
        [DataRow("c++")]
        public void RedeclarationsAndOrdinaryBuilds_KeepAbsenceAndOneResult(string language)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string declaration = "#pragma once\n_Win32_PreserveResult_ int __stdcall Example(void);\n" +
                "int __stdcall Example(void);";
            foreach (bool metadata in new[] { false, true })
            {
                var result = fixture.Scrape(declaration, metadata: metadata, language: language,
                    sal: true, repeatInclude: true, transitive: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                AssertResult(fixture.Emit(), "Example", metadata);
            }
        }

        internal static void AssertResult(string output, string name, bool preserved)
        {
            using var pe = new PEReader(File.OpenRead(output));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == name);
            Assert.AreEqual(preserved, method.ImplAttributes.HasFlag(MethodImplAttributes.PreserveSig), name);
            Assert.AreEqual(0, method.GetCustomAttributes().Count, "PreserveSig is an implementation flag, not a custom attribute.");
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            signature.ReadCompressedInteger();
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode(), "Native HRESULT remains intact.");
        }

        private static string Owner(string name) => File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
            "generation", "WinSDK", "RecompiledIdlHeaders", "um", name));

        private static void Reject(string declaration)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(declaration, sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }
    }
}
