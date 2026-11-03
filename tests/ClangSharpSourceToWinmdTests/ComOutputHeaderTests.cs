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
    public class ComOutputHeaderTests
    {
        private static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/ComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the COM migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }
        internal static readonly string[] Selectors =
        {
            "StgCreateStorageEx::ppObjectOpen", "StgOpenStorageEx::ppObjectOpen",
            "OleCreateDefaultHandler::lplpObj", "OleCreateEmbeddingHelper::lplpObj",
            "OleLoadFromStream::ppvObj", "OleCreateFontIndirect::lplpvObj",
            "OleCreatePictureIndirect::lplpvObj", "OleLoadPicture::lplpvObj",
            "OleLoadPicturePath::ppvRet", "IClassFactory2::CreateInstanceLic::ppvObj",
            "IServiceProvider::QueryService::ppvObject", "IUnknown_QueryInterface_Proxy::ppvObject",
        };

        [TestMethod]
        public void CompleteCoreOwnerInventory_RetiresOnlyTheTwelveExactRecords()
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            var old = LastErrorHeaderTests.Run("git", "-C", root, "show",
                PreRetirementCommit(root) + ":generation/WinSDK/emitter.settings.rsp");
            Assert.AreEqual(0, old.ExitCode, old.Output);
            string original = old.Output.Replace("\r\n", "\n");
            Assert.AreEqual(237, original.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            foreach (string selector in Selectors) StringAssert.Contains(original, selector + "=[ComOutPtr]\n");
            string expected = DirectXComOutputHeaderTests.RetireRecords(string.Join("\n", original.Split('\n')
                .Where(line => !Selectors.Concat(UrlMonComOutputHeaderTests.Selectors).Concat(ShellItemComOutputHeaderTests.Selectors)
                    .Concat(PropertyComOutputHeaderTests.Selectors)
                    .Concat(ShellComOutputHeaderTests.Selectors)
                    .Any(selector => line == selector + "=[ComOutPtr]"))));
            CollectionAssert.AreEqual(MediaFoundationComOutputHeaderTests.RetireRecords(expected).Split('\n'),
                File.ReadAllText(Path.Combine(root, "generation", "WinSDK", "emitter.settings.rsp"))
                    .Replace("\r\n", "\n").Split('\n'));
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
            string root = LastErrorHeaderTests.RepositoryRoot();
            string parent = PreRetirementCommit(root);
            string prefix = @"
#define WINOLEAPI HRESULT __stdcall
#define WINOLECTLAPI HRESULT __stdcall
#define STDMETHODCALLTYPE __stdcall
#define IN
#define OUT
#define FAR
#define __RPC_FAR
#define __RPC__in _In_
#define __RPC__deref_out _Outptr_
#define __RPC__deref_out_opt _Outptr_result_maybenull_
typedef long HRESULT;
typedef unsigned long DWORD;
typedef int BOOL;
typedef unsigned short WCHAR;
typedef unsigned long OLE_COLOR;
typedef long LONG;
struct GUID { unsigned long Data1; unsigned short Data2, Data3; unsigned char Data4[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFGUID;
typedef const GUID& REFCLSID;
typedef void* LPVOID;
typedef void* PVOID;
typedef WCHAR* BSTR;
typedef WCHAR* LPOLESTR;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
typedef IUnknown* LPUNKNOWN;
struct IStream : IUnknown {};
typedef IStream* LPSTREAM;
struct IClassFactory : IUnknown {};
typedef IClassFactory* LPCLASSFACTORY;
struct FONTDESC { unsigned int size; };
typedef FONTDESC* LPFONTDESC;
struct PICTDESC { unsigned int size; };
typedef PICTDESC* LPPICTDESC;
struct STGOPTIONS { unsigned short usVersion, reserved; unsigned long ulSectorSize; const WCHAR* pwcsTemplateFile; };
typedef void* PSECURITY_DESCRIPTOR;
";
            string declarations = "";
            string legacyDeclarations = "";
            foreach (var owner in new[] {
                ("coml2api.h", new[] { "StgCreateStorageEx", "StgOpenStorageEx" }),
                ("Ole2.h", new[] { "OleCreateDefaultHandler", "OleCreateEmbeddingHelper", "OleLoadFromStream" }),
                ("OleCtl.h", new[] { "OleCreateFontIndirect", "OleCreatePictureIndirect", "OleLoadPicture", "OleLoadPicturePath" }) })
            {
                string text = File.ReadAllText(Path.Combine(headers, owner.Item1));
                string original = ReadParent(owner.Item1);
                foreach (string name in owner.Item2)
                {
                    Match declaration = Regex.Match(text, @"WINOLE(?:CTL)?API\s+" + name + @"\s*\([^;]+;");
                    Assert.IsTrue(declaration.Success, owner.Item1 + ": " + name);
                    declarations += declaration.Value + "\n";
                    Match old = Regex.Match(original, @"WINOLE(?:CTL)?API\s+" + name + @"\s*\([^;]+;");
                    Assert.IsTrue(old.Success);
                    legacyDeclarations += old.Value + "\n";
                }
            }
            foreach (var method in new[] { ("ocidl.h", "IClassFactory2", "CreateInstanceLic"),
                ("servprov.h", "IServiceProvider", "QueryService") })
            {
                string text = File.ReadAllText(Path.Combine(headers, method.Item1));
                // Both physical IServiceProvider C++ variants must carry the same fact.
                var methods = Regex.Matches(text, @"virtual[^;{}]*\b" + method.Item3 + @"\s*\([^;]+?=\s*0;");
                Assert.AreEqual(method.Item1 == "servprov.h" ? 2 : 1, methods.Count, method.Item1);
                foreach (Match native in methods) StringAssert.Contains(native.Value, "_COM_Outptr_");
                declarations += "struct __declspec(uuid(\"64abea93-aac7-4f3b-a257-976deba89f04\")) " +
                    method.Item2 + " { " + methods[0].Value + " };\n";
                Match old = Regex.Match(ReadParent(method.Item1), @"virtual[^;{}]*\b" + method.Item3 + @"\s*\([^;]+?=\s*0;");
                Assert.IsTrue(old.Success);
                legacyDeclarations += "struct __declspec(uuid(\"64abea93-aac7-4f3b-a257-976deba89f04\")) " +
                    method.Item2 + " { " + old.Value + " };\n";
            }
            foreach (string owner in new[] { "Unknwn.h", "Unknwnbase.h" })
            {
                string text = File.ReadAllText(Path.Combine(headers, owner));
                var proxies = Regex.Matches(text, @"HRESULT STDMETHODCALLTYPE IUnknown_QueryInterface_Proxy\([^;]+;");
                Assert.AreEqual(2, proxies.Count, owner);
                foreach (Match native in proxies) StringAssert.Contains(native.Value, "_COM_Outptr_");
                if (owner == "Unknwn.h")
                {
                    declarations += proxies[0].Value + "\n";
                    legacyDeclarations += Regex.Match(ReadParent(owner),
                        @"HRESULT STDMETHODCALLTYPE IUnknown_QueryInterface_Proxy\([^;]+;").Value + "\n";
                }
            }
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            var result = fixture.Scrape(prefix + legacyDeclarations, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + declarations, target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));

            string ReadParent(string owner)
            {
                var result = LastErrorHeaderTests.Run("git", "-C", root, "show",
                    parent + ":generation/WinSDK/RecompiledIdlHeaders/um/" + owner);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                return result.Output;
            }
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void NativeSalVariants_DoNotInferComFromNamesIidsOrBorrowedPointers(string target)
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape(@"
int __stdcall Variants(
    _COM_Outptr_ void** required,
    _COM_Outptr_result_maybenull_ void** nullableResult,
    _COM_Outptr_opt_ void** optionalStorage,
    _COM_Outptr_opt_result_maybenull_ void** optionalBoth,
    _COM_Outptr_ void*** deep,
    _Outptr_ void** ppvObject,
    _In_ const void* riid,
    _In_ void** borrowed,
    void** unmarked);
_COM_Outptr_ void** __stdcall WrongSite(void);
", target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            using var pe = new PEReader(File.OpenRead(fixture.Emit()));
            var reader = pe.GetMetadataReader();
            var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "Variants");
            var parameters = method.GetParameters().Select(reader.GetParameter).ToArray();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i < 5) ParameterHeaderTests.AssertAttribute(reader, parameters[i], "ComOutPtrAttribute");
                else Assert.IsFalse(parameters[i].GetCustomAttributes().Select(reader.GetCustomAttribute)
                    .Any(a => ParameterHeaderTests.AttributeName(reader, a) == "ComOutPtrAttribute"));
            }
            CollectionAssert.AreEqual(new[] { ParameterAttributes.Out, ParameterAttributes.Out,
                ParameterAttributes.Out | ParameterAttributes.Optional, ParameterAttributes.Out | ParameterAttributes.Optional,
                ParameterAttributes.Out, ParameterAttributes.Out, ParameterAttributes.In, ParameterAttributes.In,
                ParameterAttributes.In | ParameterAttributes.Out }, parameters.Select(p => p.Attributes).ToArray());
            var signature = reader.GetBlobReader(method.Signature);
            signature.ReadSignatureHeader();
            Assert.AreEqual(9, signature.ReadCompressedInteger());
            Assert.AreEqual(SignatureTypeCode.Int32, signature.ReadSignatureTypeCode());
            for (int i = 0; i < 9; i++)
            {
                for (int depth = 0; depth < (i == 4 ? 3 : i == 6 ? 1 : 2); depth++)
                    Assert.AreEqual(SignatureTypeCode.Pointer, signature.ReadSignatureTypeCode());
                Assert.AreEqual(SignatureTypeCode.Void, signature.ReadSignatureTypeCode());
            }
            Assert.AreEqual(0, signature.RemainingBytes);
            var wrong = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                .Single(m => reader.GetString(m.Name) == "WrongSite");
            Assert.IsFalse(wrong.GetParameters().Select(reader.GetParameter).SelectMany(p => p.GetCustomAttributes())
                .Select(reader.GetCustomAttribute).Any(a => ParameterHeaderTests.AttributeName(reader, a) == "ComOutPtrAttribute"));
        }

        internal static void AssertFinalFacts(string path)
        {
            using var pe = new PEReader(File.OpenRead(path));
            var reader = pe.GetMetadataReader();
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (var handle in reader.MethodDefinitions)
            {
                var method = reader.GetMethodDefinition(handle);
                string name = reader.GetString(method.Name);
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                foreach (var parameter in method.GetParameters().Select(reader.GetParameter))
                {
                    string selector = (method.Attributes.HasFlag(MethodAttributes.PinvokeImpl) ? name : owner + "::" + name) +
                        "::" + reader.GetString(parameter.Name);
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    ParameterHeaderTests.AssertAttribute(reader, parameter, "ComOutPtrAttribute");
                    var attribute = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute)
                        .Single(a => ParameterHeaderTests.AttributeName(reader, a) == "ComOutPtrAttribute");
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attribute.Value), selector);
                }
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray());
        }

        [TestMethod]
        public void NativeSalComposition_PreservesExplicitInputAndMatchingLegacyDeduplicates()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("int __stdcall Example(_In_ _COM_Outptr_ void** value);", sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit());
            using (var pe = new PEReader(File.OpenRead(fixture.Emit(
                new Dictionary<string, string> { ["Example::value"] = "[ComOutPtr]" }))))
            {
                var reader = pe.GetMetadataReader();
                var parameter = reader.GetParameter(reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                    .Single(m => reader.GetString(m.Name) == "Example").GetParameters().Single());
                Assert.AreEqual(ParameterAttributes.In | ParameterAttributes.Out, parameter.Attributes);
                ParameterHeaderTests.AssertAttribute(reader, parameter, "ComOutPtrAttribute");
            }
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit()));
        }

        [TestMethod]
        public void MalformedNativeSal_IsACompilerFailureNotAnInventedOwnedFact()
        {
            using var fixture = new LastErrorHeaderTests.Fixture();
            var result = fixture.Scrape("int __stdcall Example(_COM_Outptr_(unexpected) void** value);", sal: true);
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "input.h:");
            Assert.IsFalse(File.Exists(fixture.GeneratedSource));
        }
    }
}
