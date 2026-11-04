using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class PropertyComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/PropertyComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the property output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }
        internal static readonly string[] PropertyFunctions =
        {
            "PSCreateAdapterFromPropertyStore::ppv", "PSCreateDelayedMultiplexPropertyStore::ppv",
            "PSCreateMultiplexPropertyStore::ppv", "PSCreatePropertyChangeArray::ppv",
            "PSCreatePropertyStoreFromObject::ppv", "PSCreatePropertyStoreFromPropertySetStorage::ppv",
            "PSCreateSimplePropertyChange::ppv", "PSEnumeratePropertyDescriptions::ppv",
            "PSGetItemPropertyHandler::ppv", "PSGetItemPropertyHandlerWithCreateObject::ppv",
            "PSGetPropertyDescription::ppv", "PSGetPropertyDescriptionByName::ppv",
            "PSGetPropertyDescriptionListFromString::ppv", "PSGetPropertySystem::ppv",
            "PSPropertyBag_ReadUnknown::ppv", "PSCreateMemoryPropertyStore::ppv",
        };
        internal static readonly string[] ItemFunctions =
        {
            "SHGetPropertyStoreFromIDList::ppv", "SHGetPropertyStoreFromParsingName::ppv",
        };
        internal static readonly (string Owner, string[] Selectors)[] Owners =
        {
            ("propsys.h", PropertyFunctions),
            ("ShObjIdl_core.h", ItemFunctions),
            ("shellapi.h", new[] { "SHGetPropertyStoreForWindow::ppv" }),
            ("Shlwapi.h", new[] { "SHGetViewStatePropertyBag::ppv" }),
        };
        internal static readonly string[] Selectors = Owners.SelectMany(owner => owner.Selectors).ToArray();

        [TestMethod]
        public void CompleteOwnerInventory_RetiresOnlyTwentyExactRecords()
        {
            string original = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string parent = PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot());
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", parent, "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, "The pre-retirement fixture must be reachable in a clean branch fetch. " + ancestry.Output);
            string[] records = original.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(205, records.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(MediaFoundationComOutputHeaderTests.RetireRecords(DirectXComOutputHeaderTests.RetireRecords(string.Join("\n", original.Split('\n').Where(line =>
                !Selectors.Concat(ShellComOutputHeaderTests.Selectors).Any(selector => line == selector + "=[ComOutPtr]"))))))))), current,
                "Preserve all independent sidecars exactly.");
            CollectionAssert.AreEqual(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(MediaFoundationComOutputHeaderTests.RetireRecords(DirectXComOutputHeaderTests.RetireRecords(string.Join("\n", records.Where(line => !Selectors.Concat(ShellComOutputHeaderTests.Selectors)
                .Any(selector => line == selector + "=[ComOutPtr]"))) + "\n")))))).Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray(),
                current.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray(),
                "Retire only the twenty exact records from the complete remaining corpus.");
            CollectionAssert.AreEqual(ImagingComOutputHeaderTests.RetireRecords(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(original))).Split('\n').Where(IndependentContract).ToArray(),
                current.Split('\n').Where(IndependentContract).ToArray(), "Preserve independent Out and result contracts.");
            Assert.AreEqual(73, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));

            var retained = new Dictionary<string, string[]>
            {
                ["propsys.h"] = Array.Empty<string>(),
                ["ShObjIdl_core.h"] = new[] { "SHCreateAssociationRegistration::ppv", "SHCreateDefaultExtractIcon::ppv",
                    "SHAssocEnumHandlersForProtocolByApplication::enumHandlers", "IShellMenu::GetShellFolder::ppv" },
                ["shellapi.h"] = new[] { "AssocCreateForClasses::ppv", "SHGetImageList::ppvObj" },
                ["Shlwapi.h"] = new[] { "AssocCreate::ppv", "IUnknown_GetSite::ppv",
                    "IUnknown_QueryService::ppvOut", "QISearch::ppv" },
            };
            foreach (var owner in Owners)
            {
                string text = ReadOriginal("RecompiledIdlHeaders/um/" + owner.Owner);
                string[] owned = records.Where(record =>
                {
                    string[] parts = record.Split('=')[0].Split("::");
                    if (parts.Length == 2) return Regex.IsMatch(text, FunctionPattern(record.Split('=')[0]));
                    Match definition = Regex.Match(text, @"#ifndef __" + Regex.Escape(parts[0]) +
                        @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" + Regex.Escape(parts[0]) + @"_INTERFACE_DEFINED__ \*/");
                    return definition.Success && Regex.IsMatch(definition.Value,
                        @"virtual HRESULT STDMETHODCALLTYPE " + Regex.Escape(parts[1]) + @"\([^;]+\b" + Regex.Escape(parts[2]) + @"\b");
                }).ToArray();
                CollectionAssert.AreEquivalent(owner.Selectors.Concat(retained[owner.Owner])
                    .Select(selector => selector + "=[ComOutPtr]").ToArray(), owned, owner.Owner);
                foreach (string selector in owner.Selectors)
                    Assert.AreEqual(1, Regex.Matches(text, FunctionPattern(selector)).Count, selector);
            }
            foreach (string owner in new[] { "ShObjIdl.h", "ShlObj_core.h", "ShlObj.h",
                "PropIdl.h", "PropIdlBase.h", "propvarutil.h", "propapi.h" })
            {
                string text = ReadOriginal("RecompiledIdlHeaders/um/" + owner);
                foreach (string selector in Selectors)
                    Assert.AreEqual(0, Regex.Matches(text, FunctionPattern(selector)).Count, owner + ": " + selector);
            }
            foreach (var owner in new[] { ("propsys.idl", PropertyFunctions), ("ShObjIdl_core.idl", ItemFunctions) })
            {
                string idl = ReadOriginal("RecompiledIdlHeaders/um/" + owner.Item1);
                foreach (string selector in owner.Item2)
                    Assert.AreEqual(1, Regex.Matches(idl, @"cpp_quote\(""" + FunctionPattern(selector)).Count, selector);
            }
            string sdkUm = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0", "um");
            foreach (string owner in new[] { "shellapi.idl", "Shlwapi.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(sdkUm, owner)), "Genuinely header-only owner: " + owner);

            static bool IndependentContract(string line) => line.Contains("[Out]", StringComparison.Ordinal) ||
                line.Contains("[CanReturn", StringComparison.Ordinal);
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalBytes()
        {
            foreach (string owner in new[] { "propsys.idl", "propsys.h", "shellapi.h", "Shlwapi.h" })
            {
                string original = ReadOriginal("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                string[] selectors = owner.StartsWith("propsys", StringComparison.Ordinal) ? PropertyFunctions :
                    Owners.Single(entry => entry.Owner == owner).Selectors;
                string expected = original;
                foreach (string selector in selectors)
                {
                    string pattern = owner.EndsWith(".idl", StringComparison.Ordinal) ?
                        @"cpp_quote\(""" + FunctionPattern(selector) : FunctionPattern(selector);
                    Match declaration = Regex.Match(expected, pattern);
                    Assert.IsTrue(declaration.Success, selector);
                    Assert.AreEqual(1, Regex.Matches(declaration.Value, "_Outptr_").Count, selector);
                    expected = expected.Replace(declaration.Value, declaration.Value.Replace("_Outptr_", "_COM_Outptr_"));
                }
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(owner.StartsWith("propsys", StringComparison.Ordinal) ?
                    expected : ShellComOutputHeaderTests.PatchOwner(owner, expected)), File.ReadAllBytes(Path.Combine(
                    LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)),
                    "No encoding, newline or unrelated native changes: " + owner);
            }
            // The shell family's byte audit includes both adjacent property functions in both native owners.
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
#define PSSTDAPI HRESULT __stdcall
#define SHSTDAPI HRESULT __stdcall
#define LWSTDAPI HRESULT __stdcall
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned int UINT;
typedef int BOOL;
typedef unsigned short WCHAR;
typedef const WCHAR* PCWSTR;
typedef const WCHAR* LPCWSTR;
typedef void* HWND;
struct GUID { unsigned long Data1; unsigned short Data2, Data3; unsigned char Data4[8]; };
typedef const GUID& REFIID;
struct PROPERTYKEY { GUID fmtid; DWORD pid; };
typedef const PROPERTYKEY& REFPROPERTYKEY;
struct PROPVARIANT { unsigned short vt; };
typedef const PROPVARIANT& REFPROPVARIANT;
struct ITEMIDLIST { unsigned short cb; };
typedef const ITEMIDLIST* PCIDLIST_ABSOLUTE;
enum GETPROPERTYSTOREFLAGS { GPS_DEFAULT = 0 };
enum PKA_FLAGS { PKA_SET = 0 };
enum PROPDESC_ENUMFILTER { PDEF_ALL = 0 };
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IPropertyStore : IUnknown {};
struct IDelayedPropertyStoreFactory : IUnknown {};
struct IPropertySetStorage : IUnknown {};
struct IPropertyBag : IUnknown {};
struct IBindCtx : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            string imports = "--with-librarypath\n" + string.Join("\n", Selectors.Select(selector =>
                mappings.Split('\n').Single(line => line.StartsWith(selector.Split("::")[0] + "=", StringComparison.Ordinal)).Trim()));
            var result = fixture.Scrape(prefix + Declarations(legacy: true), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] expected = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(legacy: false), target, sal: true, libraryPath: "", extraSettings: imports);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            ShellItemComOutputHeaderTests.AssertFinalFacts(output, Selectors);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(fixture.Emit(remaps)));

            string Declarations(bool legacy)
            {
                string declarations = "";
                foreach (var owner in Owners)
                {
                    string text = legacy ? ReadOriginal("RecompiledIdlHeaders/um/" + owner.Owner) :
                        File.ReadAllText(Path.Combine(headers, owner.Owner));
                    foreach (string selector in owner.Selectors)
                    {
                        var matches = Regex.Matches(text, FunctionPattern(selector));
                        Assert.AreEqual(1, matches.Count, selector);
                        declarations += matches[0].Value + "\n";
                        Assert.IsFalse(text.Contains(selector.Split("::")[0] + "_Proxy(", StringComparison.Ordinal));
                    }
                }
                return declarations;
            }
        }

        private static string FunctionPattern(string selector) => @"(?:PSSTDAPI|SHSTDAPI|LWSTDAPI|STDAPI)\s+" +
            Regex.Escape(selector.Split("::")[0]) + @"\([^;]+;";

        internal static byte[] ReadOriginalBytes(string path, string? revision = null)
        {
            var info = new System.Diagnostics.ProcessStartInfo("git")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            foreach (string argument in new[] { "-C", LastErrorHeaderTests.RepositoryRoot(), "cat-file",
                "--filters", (revision ?? PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot())) + ":generation/WinSDK/" + path })
                info.ArgumentList.Add(argument);
            using var process = System.Diagnostics.Process.Start(info) ?? throw new InvalidOperationException("Git did not start.");
            using var output = new MemoryStream();
            var error = process.StandardError.ReadToEndAsync();
            process.StandardOutput.BaseStream.CopyTo(output);
            Assert.IsTrue(process.WaitForExit(30000), "Git native fixture timed out.");
            Assert.AreEqual(0, process.ExitCode, error.GetAwaiter().GetResult());
            return output.ToArray();
        }

        internal static string ReadOriginal(string path, bool checkoutBytes = false)
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
