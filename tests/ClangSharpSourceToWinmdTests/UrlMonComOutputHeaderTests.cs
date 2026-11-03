using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class UrlMonComOutputHeaderTests
    {
        private static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/UrlMonComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the URLMon migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }
        internal static readonly string[] Selectors =
        {
            "CoGetClassObjectFromURL::ppv",
            "IBindHost::MonikerBindToObject::ppvObj",
            "IBindHost::MonikerBindToStorage::ppvObj",
        };

        [TestMethod]
        public void CompletePhysicalOwnerInventory_RetiresOnlyThreeExactRecords()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string original = ReadParent("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = original.Split('\n')
                .Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(225, records.Length);
            foreach (string owner in new[] { "UrlMon.Idl", "urlmon.h" })
            {
                string text = ReadParent("RecompiledIdlHeaders/um/" + owner);
                string[] owned = records.Where(record =>
                    Regex.IsMatch(text, @"\b" + Regex.Escape(record.Split("::")[0]) + @"\b")).ToArray();
                CollectionAssert.AreEquivalent(Selectors.Select(selector => selector + "=[ComOutPtr]").ToArray(), owned);
                Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            }
            string expected = string.Join("\n", original.Split('\n')
                .Where(line => !Selectors.Any(selector => line == selector + "=[ComOutPtr]")));
            string current = File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "emitter.settings.rsp"))
                .Replace("\r\n", "\n");
            Assert.AreEqual(expected, current);
            Assert.AreEqual(222, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeExactSalBytes()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            foreach (string owner in new[] { "UrlMon.Idl", "urlmon.h" })
            {
                string original = ReadParent("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                string output = owner.EndsWith(".Idl", StringComparison.Ordinal)
                    ? "[out]   void                **ppvObj"
                    : "_Out_  void **ppvObj";
                Assert.AreEqual(owner.EndsWith(".Idl", StringComparison.Ordinal) ? 2 : 6,
                    Regex.Matches(original, Regex.Escape(output)).Count, owner);
                string expected = original.Replace("_Outptr_ LPVOID * ppv", "_COM_Outptr_ LPVOID * ppv")
                    .Replace(output, owner.EndsWith(".Idl", StringComparison.Ordinal)
                        ? "[out, annotation(\"_COM_Outptr_\")]   void                **ppvObj"
                        : "_COM_Outptr_  void **ppvObj");
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(expected),
                    File.ReadAllBytes(Path.Combine(root, "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)),
                    "No encoding, newline, or unrelated native changes: " + owner);
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedNativeDeclarations_HeaderOnlyPreservesEveryFinalMetadataByte(string target)
            => AssertShippedDeclarations(target, Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders", "um"));

        internal static void AssertShippedDeclarations(string target, string headers)
        {
            const string prefix = @"
#define STDAPI HRESULT __stdcall
#define STDMETHODCALLTYPE __stdcall
#define MIDL_INTERFACE(iid) struct __declspec(uuid(iid))
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef unsigned short WCHAR;
struct GUID { unsigned long Data1; unsigned short Data2, Data3; unsigned char Data4[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFCLSID;
typedef const WCHAR* LPCWSTR;
typedef WCHAR* LPOLESTR;
typedef void* LPVOID;
struct IUnknown {
    virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0;
    virtual ULONG __stdcall AddRef() = 0;
    virtual ULONG __stdcall Release() = 0;
};
struct IBindCtx : IUnknown {};
typedef IBindCtx* LPBINDCTX;
struct IMoniker : IUnknown {};
struct IBindStatusCallback : IUnknown {};
";
            string original = ReadParent("RecompiledIdlHeaders/um/urlmon.h");
            string current = File.ReadAllText(Path.Combine(headers, "urlmon.h"));
            string legacy = Declarations(original);
            string annotated = Declarations(current);
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            using var fixture = new LastErrorHeaderTests.Fixture();
            const string imports = "--with-librarypath\nCoGetClassObjectFromURL=urlmon.dll";
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            StringAssert.Contains(mappings, "CoGetClassObjectFromURL=urlmon.dll");
            Assert.IsFalse(mappings.Contains("IBindHost_MonikerBindTo", StringComparison.Ordinal));
            Assert.IsFalse(File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappingsManual.rsp")).Contains("IBindHost_MonikerBindTo", StringComparison.Ordinal));
            var result = fixture.Scrape(prefix + legacy, target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + annotated, target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));

            static string Declarations(string text)
            {
                Match activation = Regex.Match(text, @"STDAPI CoGetClassObjectFromURL\([^;]+;");
                Assert.IsTrue(activation.Success);
                var interfaces = Regex.Matches(text,
                    @"MIDL_INTERFACE\(""[^""]+""\)\s+IBindHost : public IUnknown\s*\{[^}]+\};");
                Assert.AreEqual(1, interfaces.Count, "Exactly one complete guarded C++ definition, not the forward.");
                string declarations = activation.Value + "\n" + interfaces[0].Value + "\n";
                foreach (string method in new[] { "MonikerBindToObject", "MonikerBindToStorage" })
                {
                    var proxies = Regex.Matches(text, @"HRESULT STDMETHODCALLTYPE IBindHost_" + method + @"_Proxy\([^;]+;");
                    Assert.AreEqual(1, proxies.Count);
                    declarations += proxies[0].Value + "\n";
                }
                return declarations;
            }
        }

        private static void AssertFinalFacts(string path)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (var handle in reader.MethodDefinitions)
            {
                var method = reader.GetMethodDefinition(handle);
                string name = reader.GetString(method.Name);
                Assert.IsFalse(name.StartsWith("IBindHost_MonikerBindTo", StringComparison.Ordinal),
                    "Native COM proxies must stay outside the projected API surface.");
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                string methodSelector = method.Attributes.HasFlag(MethodAttributes.PinvokeImpl) ? name : owner + "::" + name;
                var parameters = method.GetParameters().Select(reader.GetParameter)
                    .Where(parameter => parameter.SequenceNumber != 0).ToArray();
                foreach (var parameter in parameters)
                {
                    string selector = methodSelector + "::" + reader.GetString(parameter.Name);
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector));
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    Assert.IsFalse(attributes.Any(a => ParameterHeaderTests.AttributeName(reader, a) == "RetValAttribute"));
                    var attribute = attributes.Single(a => ParameterHeaderTests.AttributeName(reader, a) == "ComOutPtrAttribute");
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attribute.Value), selector);
                    var signature = reader.GetBlobReader(method.Signature);
                    signature.ReadSignatureHeader();
                    Assert.AreEqual(name == "CoGetClassObjectFromURL" ? 10 : 5, parameters.Length, selector);
                    Assert.AreEqual(parameters.Length, signature.ReadCompressedInteger());
                    Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
                    // Every selected output is the final parameter: two pointer indirections to void.
                    byte[] bytes = reader.GetBlobBytes(method.Signature);
                    CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 }, bytes.TakeLast(3).ToArray(), selector);
                }
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray());
        }

        private static string ReadParent(string path, bool checkoutBytes = false)
        {
            string parent = PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot());
            string[] arguments = checkoutBytes
                ? new[] { "-C", LastErrorHeaderTests.RepositoryRoot(), "cat-file", "--filters", parent + ":generation/WinSDK/" + path }
                : new[] { "-C", LastErrorHeaderTests.RepositoryRoot(), "show", parent + ":generation/WinSDK/" + path };
            var result = LastErrorHeaderTests.Run("git", arguments);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            return result.Output;
        }
    }
}
