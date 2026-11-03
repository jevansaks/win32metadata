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
    public class ShellItemComOutputHeaderTests
    {
        internal const string Parent = "35495220f3c9a71126aa51c662a2b83cc37f18e2";
        internal static readonly string[] ItemFunctions =
        {
            "SHCreateItemFromIDList::ppv", "SHCreateItemFromParsingName::ppv",
            "SHCreateItemWithParent::ppvItem", "SHCreateItemFromRelativeName::ppv",
            "SHCreateItemInKnownFolder::ppv", "SHGetItemFromObject::ppv",
            "SHGetItemFromDataObject::ppv", "SHCreateShellItemArrayFromDataObject::ppv",
            "SHCreateShellItemArrayFromShellItem::ppv",
        };
        internal static readonly string[] BindingFunctions =
        {
            "SHGetKnownFolderItem::ppv", "SHBindToParent::ppv",
            "SHBindToFolderIDListParent::ppv", "SHBindToFolderIDListParentEx::ppv", "SHBindToObject::ppv",
        };
        internal static readonly string[] TransferMethods =
        {
            "ITransferSource::OpenItem::ppv", "ITransferDestination::CreateItem::ppvItem",
            "ITransferDestination::CreateItem::ppvResources",
        };
        internal static readonly string[] Selectors = ItemFunctions.Concat(BindingFunctions).Concat(TransferMethods).ToArray();

        [TestMethod]
        public void CompleteFamilyInventory_RetiresOnlySeventeenExactRecords()
        {
            string original = ReadParent("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = original.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(222, records.Length);
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            string expected = string.Join("\n", original.Split('\n')
                .Where(line => !Selectors.Any(selector => line == selector + "=[ComOutPtr]")));
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(expected, current);
            Assert.AreEqual(205, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));

            foreach (var owner in new[] { ("ShObjIdl_core.h", ItemFunctions), ("ShlObj_core.h", BindingFunctions) })
            {
                string text = ReadParent("RecompiledIdlHeaders/um/" + owner.Item1);
                foreach (string selector in owner.Item2)
                    Assert.AreEqual(1, Regex.Matches(text, FunctionPattern(selector)).Count, selector);
                string[] owned = records.Where(record =>
                {
                    string[] parts = record.Split('=')[0].Split("::");
                    if (parts.Length == 2) return Regex.IsMatch(text, FunctionPattern(record.Split('=')[0]));
                    Match definition = Regex.Match(text, @"#ifndef __" + Regex.Escape(parts[0]) +
                        @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" + Regex.Escape(parts[0]) + @"_INTERFACE_DEFINED__ \*/");
                    return definition.Success && Regex.IsMatch(definition.Value,
                        @"virtual HRESULT STDMETHODCALLTYPE " + Regex.Escape(parts[1]) + @"\([^;]+\b" + Regex.Escape(parts[2]) + @"\b");
                }).ToArray();
                string[] retained = owner.Item1 == "ShObjIdl_core.h" ? new[] {
                    "SHCreateAssociationRegistration::ppv", "SHCreateDefaultExtractIcon::ppv",
                    "SHAssocEnumHandlersForProtocolByApplication::enumHandlers", "IShellMenu::GetShellFolder::ppv",
                    "SHGetPropertyStoreFromIDList::ppv", "SHGetPropertyStoreFromParsingName::ppv" } : new[] {
                    "SHCoCreateInstance::ppv", "SHCreateDataObject::ppv", "SHCreateDefaultContextMenu::ppv",
                    "SHCreateFileExtractIconW::ppv", "StgMakeUniqueName::ppv" };
                CollectionAssert.AreEquivalent(owner.Item2.Concat(owner.Item1 == "ShObjIdl_core.h" ? TransferMethods : Array.Empty<string>())
                    .Concat(retained).Select(selector => selector + "=[ComOutPtr]").ToArray(), owned, owner.Item1);
            }
            foreach (string owner in new[] { "ShObjIdl.h", "ShlObj.h" })
            {
                string text = ReadParent("RecompiledIdlHeaders/um/" + owner);
                foreach (string selector in ItemFunctions.Concat(BindingFunctions))
                    Assert.AreEqual(0, Regex.Matches(text, FunctionPattern(selector)).Count, owner + ": " + selector);
            }
            string idl = ReadParent("RecompiledIdlHeaders/um/ShObjIdl_core.idl");
            foreach (string selector in ItemFunctions)
                Assert.AreEqual(1, Regex.Matches(idl, @"cpp_quote\(""" + FunctionPattern(selector)).Count, selector);
            foreach (string selector in BindingFunctions)
                Assert.IsFalse(idl.Contains(selector.Split("::")[0] + "(", StringComparison.Ordinal));
            string sdkUm = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0", "um");
            Assert.IsFalse(File.Exists(Path.Combine(sdkUm, "ShlObj_core.idl")), "Binding owner is genuinely header-only.");
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalBytes()
        {
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShObjIdl_core.h", "ShlObj_core.h" })
            {
                string original = ReadParent("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                string expected = original;
                foreach (string selector in owner == "ShlObj_core.h" ? BindingFunctions : ItemFunctions)
                {
                    string pattern = FunctionPattern(selector);
                    Match declaration = Regex.Match(expected, pattern);
                    Assert.IsTrue(declaration.Success, selector);
                    expected = expected.Replace(declaration.Value, declaration.Value.Replace("_Outptr_ void **", "_COM_Outptr_ void **"));
                }
                if (owner == "ShObjIdl_core.idl")
                {
                    foreach (string method in new[] { "OpenItem", "CreateItem" })
                    {
                        Match declaration = Regex.Match(expected, @"HRESULT " + method + @"\([^;]+;");
                        Assert.IsTrue(declaration.Success);
                        expected = expected.Replace(declaration.Value, declaration.Value.Replace(
                            "annotation(\"_Outptr_\")", "annotation(\"_COM_Outptr_\")"));
                    }
                }
                else if (owner == "ShObjIdl_core.h")
                {
                    foreach (string type in new[] { "ITransferSource", "ITransferDestination" })
                    {
                        Match definition = Regex.Match(expected, @"#ifndef __" + type +
                            @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" + type + @"_INTERFACE_DEFINED__ \*/");
                        Assert.IsTrue(definition.Success);
                        string changed = Regex.Replace(definition.Value, @"_Outptr_(  void \*\*ppv(?:Item|Resources)?\b)", "_COM_Outptr_$1");
                        Assert.AreEqual(type == "ITransferSource" ? 2 : 4,
                            Regex.Matches(changed, "_COM_Outptr_").Count - Regex.Matches(definition.Value, "_COM_Outptr_").Count);
                        expected = expected.Replace(definition.Value, changed);
                    }
                }
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(expected), File.ReadAllBytes(Path.Combine(
                    LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)),
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
#define SHSTDAPI HRESULT __stdcall
#define STDAPI HRESULT __stdcall
#define STDMETHODCALLTYPE __stdcall
#define MIDL_INTERFACE(iid) struct __declspec(uuid(iid))
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef unsigned __int64 ULONGLONG;
typedef unsigned short WCHAR;
typedef const WCHAR* PCWSTR;
typedef const WCHAR* LPCWSTR;
typedef void* HANDLE;
struct GUID { unsigned long Data1; unsigned short Data2, Data3; unsigned char Data4[8]; };
typedef const GUID& REFIID;
typedef const GUID& REFKNOWNFOLDERID;
struct ITEMIDLIST { unsigned short cb; };
typedef const ITEMIDLIST* PCIDLIST_ABSOLUTE;
typedef const ITEMIDLIST* PCUIDLIST_RELATIVE;
typedef const ITEMIDLIST* PCUITEMID_CHILD;
enum DATAOBJ_GET_ITEM_FLAGS { DOGIF_DEFAULT = 0 };
enum KNOWN_FOLDER_FLAG { KF_FLAG_DEFAULT = 0 };
typedef unsigned long TRANSFER_SOURCE_FLAGS;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IBindCtx : IUnknown {};
struct IShellFolder : IUnknown {};
struct IShellItem : IUnknown {};
struct IDataObject : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            string imports = "--with-librarypath\n" + string.Join("\n", ItemFunctions.Concat(BindingFunctions)
                .Select(selector => mappings.Split('\n').Single(line => line.StartsWith(selector.Split("::")[0] + "=", StringComparison.Ordinal)).Trim()));
            var result = fixture.Scrape(prefix + Declarations(legacy: true), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(legacy: false), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));

            string Declarations(bool legacy)
            {
                string declarations = "";
                foreach (var owner in new[] { ("ShObjIdl_core.h", ItemFunctions), ("ShlObj_core.h", BindingFunctions) })
                {
                    string text = legacy ? ReadParent("RecompiledIdlHeaders/um/" + owner.Item1) :
                        File.ReadAllText(Path.Combine(headers, owner.Item1));
                    foreach (string selector in owner.Item2)
                    {
                        var matches = Regex.Matches(text, FunctionPattern(selector));
                        Assert.AreEqual(1, matches.Count, selector);
                        declarations += matches[0].Value + "\n";
                    }
                    if (owner.Item1 != "ShObjIdl_core.h") continue;
                    foreach (var method in new[] { ("ITransferSource", "OpenItem"), ("ITransferDestination", "CreateItem") })
                    {
                        var definitions = Regex.Matches(text, @"MIDL_INTERFACE\(""[^""]+""\)\s+" + method.Item1 + @" : public IUnknown\s*\{");
                        Assert.AreEqual(1, definitions.Count);
                        var methods = Regex.Matches(text, @"virtual HRESULT STDMETHODCALLTYPE " + method.Item2 + @"\([^;]+?=\s*0;");
                        Assert.AreEqual(1, methods.Count);
                        declarations += definitions[0].Value + "\n" + methods[0].Value + "\n};\n";
                        Assert.IsFalse(text.Contains(method.Item1 + "_" + method.Item2 + "_Proxy(", StringComparison.Ordinal),
                            "Local transfer methods have no generated proxy to import.");
                    }
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
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                string methodSelector = method.Attributes.HasFlag(MethodAttributes.PinvokeImpl) ? name : owner + "::" + name;
                var parameters = method.GetParameters().Select(reader.GetParameter).Where(p => p.SequenceNumber != 0).ToArray();
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                Assert.AreEqual(parameters.Length, signature.ReadCompressedInteger());
                ReadType(ref signature);
                foreach (var parameter in parameters)
                {
                    int start = signature.Offset;
                    ReadType(ref signature);
                    string selector = methodSelector + "::" + reader.GetString(parameter.Name);
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector));
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    Assert.IsFalse(attributes.Any(a => ParameterHeaderTests.AttributeName(reader, a) == "RetValAttribute"));
                    var attribute = attributes.Single(a => ParameterHeaderTests.AttributeName(reader, a) == "ComOutPtrAttribute");
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attribute.Value), selector);
                    CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 },
                        reader.GetBlobBytes(method.Signature).Skip(start).Take(signature.Offset - start).ToArray(), selector);
                }
                Assert.AreEqual(0, signature.RemainingBytes);
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray());

            static void ReadType(ref BlobReader signature)
            {
                SignatureTypeCode type = signature.ReadSignatureTypeCode();
                if (type == SignatureTypeCode.Pointer || type == SignatureTypeCode.ByReference) ReadType(ref signature);
                else if (type == SignatureTypeCode.TypeHandle) signature.ReadTypeHandle();
                else Assert.IsTrue(type >= SignatureTypeCode.Void && type <= SignatureTypeCode.Object, type.ToString());
            }
        }

        private static string FunctionPattern(string selector) => @"(?:SHSTDAPI|STDAPI)\s+" +
            Regex.Escape(selector.Split("::")[0]) + @"\([^;]+;";

        private static string ReadParent(string path, bool checkoutBytes = false)
        {
            string[] arguments = checkoutBytes
                ? new[] { "-C", LastErrorHeaderTests.RepositoryRoot(), "cat-file", "--filters", Parent + ":generation/WinSDK/" + path }
                : new[] { "-C", LastErrorHeaderTests.RepositoryRoot(), "show", Parent + ":generation/WinSDK/" + path };
            var result = LastErrorHeaderTests.Run("git", arguments);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            return result.Output;
        }
    }
}
