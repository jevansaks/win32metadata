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
    public class ShellComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/ShellComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the remaining shell output migration fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static string ReadOriginal(string path, bool checkoutBytes = false)
        {
            string root = LastErrorHeaderTests.RepositoryRoot();
            string parent = PreRetirementCommit(root);
            var result = checkoutBytes
                ? LastErrorHeaderTests.Run("git", "-C", root, "cat-file", "--filters", parent + ":generation/WinSDK/" + path)
                : LastErrorHeaderTests.Run("git", "-C", root, "show", parent + ":generation/WinSDK/" + path);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            return result.Output;
        }

        internal static readonly (string Owner, string[] Selectors)[] Owners =
        {
            ("ShObjIdl_core.h", new[] { "SHCreateAssociationRegistration::ppv", "SHCreateDefaultExtractIcon::ppv",
                "SHAssocEnumHandlersForProtocolByApplication::enumHandlers", "IShellMenu::GetShellFolder::ppv" }),
            ("ShObjIdl.h", new[] { "INameSpaceTreeControlEvents::OnBeforeContextMenu::ppv",
                "INameSpaceTreeControlEvents::OnAfterContextMenu::ppv" }),
            ("ShlObj_core.h", new[] { "SHCoCreateInstance::ppv", "SHCreateDataObject::ppv",
                "SHCreateDefaultContextMenu::ppv", "SHCreateFileExtractIconW::ppv" }),
            ("shellapi.h", new[] { "AssocCreateForClasses::ppv", "SHGetImageList::ppvObj" }),
            ("Shlwapi.h", new[] { "AssocCreate::ppv", "IQueryAssociations::GetEnum::ppvOut" }),
            ("ShlObj.h", new[] { "IDockingWindowFrame::FindToolbar::ppv" }),
            ("shdeprecated.h", new[] { "IBrowserService2::CreateBrowserPropSheetExt::ppv" }),
        };
        internal static readonly string[] Selectors = Owners.SelectMany(owner => owner.Selectors).ToArray();

        [TestMethod]
        public void CompleteShellInventory_RetiresOnlySixteenExactRecords()
        {
            string original = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = original.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(185, records.Length);
            Assert.AreEqual(16, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(MediaFoundationComOutputHeaderTests.RetireRecords(DirectXComOutputHeaderTests.RetireRecords(string.Join("\n", original.Split('\n').Where(line =>
                !Selectors.Any(selector => line == selector + "=[ComOutPtr]")))))))), current,
                "Preserve all independent sidecars exactly.");
            CollectionAssert.AreEqual(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(ServiceComOutputHeaderTests.RetireRecords(MediaFoundationComOutputHeaderTests.RetireRecords(DirectXComOutputHeaderTests.RetireRecords(string.Join("\n", records.Where(line =>
                !Selectors.Any(selector => line == selector + "=[ComOutPtr]"))) + "\n"))))).Split('\n')
                    .Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray(),
                current.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray(),
                "Retire only the sixteen exact records from the complete remaining corpus.");
            CollectionAssert.AreEqual(AudioComOutputHeaderTests.RetireRecords(ClrComOutputHeaderTests.RetireRecords(original)).Split('\n').Where(IndependentContract).ToArray(),
                current.Split('\n').Where(IndependentContract).ToArray(), "Preserve independent Out and result contracts.");
            Assert.AreEqual(88, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));

            var found = new HashSet<string>(StringComparer.Ordinal);
            var files = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(), "ls-tree", "-r",
                "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--", "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, files.ExitCode, files.Output);
            string[] traversal = File.ReadAllLines(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "Partitions", "Shell", "settings.rsp"));
            foreach (string path in traversal.Where(line => line.StartsWith("<IncludeRoot>/", StringComparison.Ordinal)))
            {
                string originalPath = files.Output.Split('\n').Select(line => line.TrimEnd('\r')).Single(line =>
                    line.Equals("generation/WinSDK/RecompiledIdlHeaders/" + path.Substring("<IncludeRoot>/".Length),
                        StringComparison.OrdinalIgnoreCase));
                string text = ReadOriginal(originalPath.Substring("generation/WinSDK/".Length));
                foreach (string record in records)
                {
                    string selector = record.Split('=')[0];
                    string[] parts = selector.Split("::");
                    if (parts.Length == 2 ? Regex.IsMatch(text, FunctionPattern(selector)) :
                        Regex.IsMatch(InterfaceDefinition(text, parts[0]), MethodPattern(parts[1])))
                        found.Add(selector);
                }
            }
            string[] retained = { "StgMakeUniqueName::ppv", "IUnknown_GetSite::ppv",
                "IUnknown_QueryService::ppvOut", "QISearch::ppv", "HlinkCreateFromMoniker::ppvObj",
                "HlinkCreateFromString::ppvObj", "HlinkCreateFromData::ppvObj", "HlinkClone::ppvObj",
                "HlinkCreateBrowseContext::ppvObj", "HlinkCreateExtensionServices::ppvObj", "HlinkResolveShortcut::ppvObj" };
            CollectionAssert.AreEquivalent(Selectors.Concat(retained).ToArray(), found.ToArray(),
                "Audit every remaining record against every selected shipped shell owner, not a name prefix.");
            foreach (string selector in retained) StringAssert.Contains(current, selector + "=[ComOutPtr]\n");
            StringAssert.Contains(File.ReadAllText(Header("shellapi.h")),
                "AssocCreateForClasses(_In_reads_(cClasses) const ASSOCIATIONELEMENT *rgClasses, ULONG cClasses, _In_ REFIID riid, _COM_Outptr_ void **ppv);");
            StringAssert.Contains(File.ReadAllText(Header("ShlObj_core.h")),
                "SHCreateFileExtractIconA(_In_ LPCSTR pszFile, _In_ DWORD dwFileAttributes, _In_ REFIID riid, _Outptr_ void **ppv);");
            string sdkUm = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0", "um");
            foreach (string owner in new[] { "ShlObj.idl", "ShlObj_core.idl", "Shlwapi.idl", "shellapi.idl", "shdeprecated.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(sdkUm, owner)), "Post-MIDL only for genuinely header-only owners: " + owner);

            static bool IndependentContract(string line) => line.Contains("[Out]", StringComparison.Ordinal) ||
                line.Contains("[CanReturn", StringComparison.Ordinal);
        }

        [TestMethod]
        public void NativeEdits_OnlyChangeSelectedSalBytesAndAllInheritedCopies()
        {
            foreach (string owner in new[] { "ShObjIdl.idl", "ShObjIdl.h", "ShlObj.h", "shdeprecated.h" })
            {
                string original = ReadOriginal("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(PatchOwner(owner, original)),
                    File.ReadAllBytes(Header(owner)), "No encoding, newline or unrelated changes: " + owner);
            }
            foreach (var owner in Owners)
            {
                string original = ReadOriginal("RecompiledIdlHeaders/um/" + owner.Owner);
                string patched = File.ReadAllText(Header(owner.Owner));
                foreach (string selector in owner.Selectors)
                {
                    string[] parts = selector.Split("::");
                    string pattern = parts.Length == 2 ? FunctionPattern(selector) : MethodPattern(parts[1]);
                    int count = parts.Length == 2 || owner.Owner == "ShlObj.h" || owner.Owner == "Shlwapi.h" ? 1 :
                        owner.Owner == "shdeprecated.h" ? 4 : 2;
                    var declarations = Regex.Matches(patched, pattern);
                    Assert.AreEqual(count, declarations.Count, selector);
                    foreach (Match declaration in declarations)
                        StringAssert.Contains(declaration.Value, "_COM_Outptr_", selector);
                    Assert.IsFalse(original.Contains(parts[0] + (parts.Length == 3 ? "_" + parts[1] : "") + "_Proxy(",
                        StringComparison.Ordinal), selector);
                }
            }
            StringAssert.Contains(Regex.Match(File.ReadAllText(Header("shdeprecated.h")),
                @"HRESULT \( STDMETHODCALLTYPE \*GetShellFolder \)\([^;]+;").Value, "_COM_Outptr_  void **ppv");
        }

        internal static string PatchOwner(string owner, string text)
        {
            string header = owner.EndsWith(".idl", StringComparison.OrdinalIgnoreCase) ? Path.ChangeExtension(owner, ".h") : owner;
            string[] selectors = Owners.Single(entry => entry.Owner.Equals(header, StringComparison.OrdinalIgnoreCase)).Selectors;
            foreach (string selector in selectors)
            {
                string[] parts = selector.Split("::");
                string pattern = parts.Length == 2 ? FunctionPattern(selector, idl: owner.EndsWith(".idl", StringComparison.OrdinalIgnoreCase)) :
                    owner.EndsWith(".idl", StringComparison.OrdinalIgnoreCase) ? @"HRESULT " + parts[1] + @"\([^;]+;" :
                    MethodPattern(parts[1]);
                text = Regex.Replace(text, pattern, match => parts.Length == 2 ?
                    Regex.Replace(match.Value, @"(?<!_COM)_Outptr_", "_COM_Outptr_") :
                    Regex.Replace(match.Value, @"_Outptr_(?=""\)\] void\*\* ppv|""\)\] void \*\*ppv|  void \*\*ppv\b| void \*\*ppv(?:Out)?\b)", "_COM_Outptr_"));
            }
            if (owner == "shdeprecated.h")
                text = Regex.Replace(text, @"HRESULT \( STDMETHODCALLTYPE \*GetShellFolder \)\([^;]+;",
                    match => match.Value.Replace("_Outptr_  void **ppv", "_COM_Outptr_  void **ppv"));
            return text;
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedNativeDeclarations_HeaderOnlyPreservesEveryFinalMetadataByte(string target)
            => AssertShippedDeclarations(target, Path.GetDirectoryName(Header("ShObjIdl.h"))!);

        internal static void AssertShippedDeclarations(string target, string headers)
        {
            const string prefix = @"
#define SHSTDAPI HRESULT __stdcall
#define STDAPI HRESULT __stdcall
#define LWSTDAPI HRESULT __stdcall
#define STDMETHODCALLTYPE __stdcall
#define MIDL_INTERFACE(iid) struct __declspec(uuid(iid))
#define DECLARE_INTERFACE_IID_(name, base, iid) struct __declspec(uuid(iid)) name : base
#define STDMETHOD(name) virtual HRESULT __stdcall name
#define THIS_
#define PURE = 0
typedef long HRESULT;
typedef unsigned long DWORD;
typedef unsigned long ULONG;
typedef unsigned int UINT;
typedef int BOOL;
typedef unsigned short WCHAR;
typedef const WCHAR* PCWSTR;
typedef const WCHAR* LPCWSTR;
struct GUID { unsigned long Data1; unsigned short Data2, Data3; unsigned char Data4[8]; };
typedef GUID CLSID;
typedef const GUID& REFIID;
struct ITEMIDLIST { unsigned short cb; };
typedef ITEMIDLIST* PIDLIST_ABSOLUTE;
typedef const ITEMIDLIST* PCIDLIST_ABSOLUTE;
typedef const ITEMIDLIST* PCUITEMID_CHILD;
typedef const PCUITEMID_CHILD* PCUITEMID_CHILD_ARRAY;
struct ASSOCIATIONELEMENT { int unused; };
struct DEFCONTEXTMENU { int unused; };
typedef unsigned long ASSOCF;
enum ASSOCENUM { ASSOCENUM_NONE = 0 };
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
struct IOleWindow : IUnknown {};
struct IBrowserService : IUnknown {};
struct IShellItem : IUnknown {};
struct IDataObject : IUnknown {};
struct IContextMenu : IUnknown {};
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            string mappings = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "libMappings.rsp"));
            string imports = "--with-librarypath\n" + string.Join("\n", Selectors.Where(selector => selector.Split("::").Length == 2)
                .Select(selector => mappings.Split('\n').Single(line => line.StartsWith(selector.Split("::")[0] + "=", StringComparison.Ordinal)).Trim()));
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
                    foreach (var group in owner.Selectors.GroupBy(selector => selector.Split("::")[0]))
                    {
                        if (group.First().Split("::").Length == 2)
                        {
                            var matches = Regex.Matches(text, FunctionPattern(group.First()));
                            Assert.AreEqual(1, matches.Count, group.Key);
                            declarations += matches[0].Value + "\n";
                            continue;
                        }
                        string definition = InterfaceDefinition(text, group.Key);
                        Assert.AreNotEqual("", definition, group.Key);
                        declarations += definition.Substring(0, definition.IndexOf('{') + 1) + "\n";
                        foreach (string selector in group)
                        {
                            var matches = Regex.Matches(definition, MethodPattern(selector.Split("::")[1]));
                            Assert.AreEqual(1, matches.Count, selector);
                            declarations += matches[0].Value + "\n";
                        }
                        declarations += "};\n";
                    }
                }
                return declarations;
            }
        }

        private static string Header(string owner) => Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
            "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner);

        private static string FunctionPattern(string selector, bool idl = false) =>
            (idl ? @"cpp_quote\(""" : @"(?m)^[ \t]*") + @"(?:SHSTDAPI|STDAPI|LWSTDAPI)\s+" + Regex.Escape(selector.Split("::")[0]) + @"\s*\([^;]+;";

        private static string MethodPattern(string method) =>
            @"(?:virtual HRESULT STDMETHODCALLTYPE " + Regex.Escape(method) + @"\s*\(|HRESULT \( STDMETHODCALLTYPE \*" +
            Regex.Escape(method) + @" \)\s*\(|STDMETHOD\s*\(" + Regex.Escape(method) + @"\)\s*\()[^;]+;";

        private static string InterfaceDefinition(string text, string type) => Regex.Match(text,
            @"(?:MIDL_INTERFACE\(""[^""]+""\)\s+" + Regex.Escape(type) + @"\s*:\s*public\s+\w+|" +
            @"DECLARE_INTERFACE_IID_\(\s*" + Regex.Escape(type) + @",[^)]+\))\s*\{[\s\S]+?\n\s*\};").Value;
    }
}
