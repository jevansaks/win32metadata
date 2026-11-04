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
    public class SyncComOutputHeaderTests
    {
        internal static string PreRetirementCommit(string root)
        {
            var introduction = LastErrorHeaderTests.Run("git", "-C", root, "log", "--diff-filter=A",
                "--format=%H", "HEAD", "--", "tests/ClangSharpSourceToWinmdTests/SyncComOutputHeaderTests.cs");
            Assert.AreEqual(0, introduction.ExitCode, introduction.Output);
            string[] commits = introduction.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(1, commits.Length, "Expected one reachable introduction of the synchronization output fixture.");
            var parent = LastErrorHeaderTests.Run("git", "-C", root, "rev-parse", commits[0] + "^");
            Assert.AreEqual(0, parent.ExitCode, parent.Output);
            return parent.Output.Trim();
        }

        internal static byte[] ReadOriginalBytes(string path) => PropertyComOutputHeaderTests.ReadOriginalBytes(
            path, PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()));

        internal static string ReadOriginal(string path) => Encoding.Latin1.GetString(ReadOriginalBytes(path));

        internal static readonly string[] Selectors =
        {
            "IChangeUnitException::GetClockVector::ppUnk",
            "IClockVector::GetClockVectorElements::ppiEnumClockVector",
            "IRangeException::GetClockVector::ppUnk",
            "ISingleItemException::GetClockVector::ppUnk",
            "ISyncKnowledge::FindClockVectorForChangeUnit::ppUnk",
            "ISyncKnowledge::FindClockVectorForItem::ppUnk",
            "ISyncKnowledge::GetChangeUnitExceptions::ppUnk",
            "ISyncKnowledge::GetRangeExceptions::ppUnk",
            "ISyncKnowledge::GetScopeVector::ppUnk",
            "ISyncKnowledge::GetSingleItemExceptions::ppUnk",
            "ISyncKnowledge2::GetInspector::ppiInspector",
        };

        internal static string RetireRecords(string text)
        {
            foreach (string selector in Selectors)
                text = text.Replace(selector + "=[ComOutPtr]\n", "", StringComparison.Ordinal);
            return HandleProducerSdkTests.ApplySidecarDelta(text);
        }

        [TestMethod]
        public void CompleteTraversal_AuditsAllSeventyThreeParentRecordsAndIndependentFacts()
        {
            string parent = ReadOriginal("emitter.settings.rsp").Replace("\r\n", "\n");
            string[] records = parent.Split('\n').Where(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)).ToArray();
            Assert.AreEqual(73, records.Length);
            Assert.AreEqual(11, Selectors.Length);
            Assert.IsFalse(records.Any(record => record.Split('=')[0].Contains('*')));
            foreach (string selector in Selectors) CollectionAssert.Contains(records, selector + "=[ComOutPtr]");
            string current = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "emitter.settings.rsp")).Replace("\r\n", "\n");
            Assert.AreEqual(RetireRecords(parent), current, "Preserve the complete independent sidecar exactly.");
            CollectionAssert.AreEqual(RetireRecords(parent).Split('\n').Where(Contract).ToArray(),
                current.Split('\n').Where(Contract).ToArray());
            Assert.AreEqual(62, current.Split('\n').Count(line => line.Contains("[ComOutPtr]", StringComparison.Ordinal)));
            foreach (string selector in ClrComOutputHeaderTests.RetainedSelectors)
                StringAssert.Contains(current, selector + "=" +
                    (selector == ClrComOutputHeaderTests.CombinedSelector ? "[Out]" : "") + "[ComOutPtr]\n");
            var ancestry = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "merge-base", "--is-ancestor", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "HEAD");
            Assert.AreEqual(0, ancestry.ExitCode, ancestry.Output);
            var inventory = LastErrorHeaderTests.Run("git", "-C", LastErrorHeaderTests.RepositoryRoot(),
                "ls-tree", "-r", "--name-only", PreRetirementCommit(LastErrorHeaderTests.RepositoryRoot()), "--",
                "generation/WinSDK/RecompiledIdlHeaders");
            Assert.AreEqual(0, inventory.ExitCode, inventory.Output);
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string include in File.ReadAllLines(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "Partitions", "WinSync", "settings.rsp"))
                .Where(line => line.StartsWith("<IncludeRoot>/", StringComparison.Ordinal)))
                paths.Add(include.Substring("<IncludeRoot>/".Length));
            string main = File.ReadAllText(Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "Partitions", "WinSync", "main.cpp"));
            foreach (Match include in Regex.Matches(main, @"#include <([^>]+)>"))
            {
                string path = "um/" + include.Groups[1].Value;
                if (inventory.Output.Split('\n').Any(line => line.TrimEnd('\r').Equals(
                    "generation/WinSDK/RecompiledIdlHeaders/" + path, StringComparison.OrdinalIgnoreCase)))
                    paths.Add(path);
            }
            CollectionAssert.AreEquivalent(new[] { "um/winsync.h", "um/syncregistration.h",
                "um/synchronizationerrors.h", "um/syncregistrationerrors.h" }, paths.ToArray());
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (string path in paths)
            {
                string text = ReadOriginal("RecompiledIdlHeaders/" + path);
                foreach (string record in records)
                {
                    string selector = record.Split('=')[0];
                    string[] parts = selector.Split("::");
                    if (parts.Length == 3 && Regex.IsMatch(AudioComOutputHeaderTests.Definition(text, parts[0]),
                        MethodPattern(parts[1])))
                        found.Add(selector);
                    else if (parts.Length == 2 && Regex.IsMatch(text,
                        @"\b" + Regex.Escape(parts[0]) + @"\s*\([^;{}]*\b" + Regex.Escape(parts[1]) + @"\b[^;{}]*\);"))
                        found.Add(selector);
                }
                if (path != "um/winsync.h")
                    CollectionAssert.AreEqual(ReadOriginalBytes("RecompiledIdlHeaders/" + path),
                        File.ReadAllBytes(Path.Combine(LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK",
                            "RecompiledIdlHeaders", path.Replace('/', Path.DirectorySeparatorChar))), path);
            }
            CollectionAssert.AreEquivalent(Selectors, found.ToArray(), "Complete physically owning traversal, not interface-name prefixes.");

            static bool Contract(string line) => line.Contains("[ComOutPtr]", StringComparison.Ordinal) ||
                line.Contains("[Out]", StringComparison.Ordinal) || line.Contains("[CanReturn", StringComparison.Ordinal) ||
                line == DirectXComOutputHeaderTests.DWriteSelector + "=void**";
        }

        [TestMethod]
        public void NativeEdits_OnlyAddThirtyFiveSalTokensAcrossAllPhysicalCopies()
        {
            byte[] original = ReadOriginalBytes("RecompiledIdlHeaders/um/winsync.h");
            string before = Encoding.Latin1.GetString(original);
            string expected = PatchHeader(before);
            CollectionAssert.AreEqual(Encoding.Latin1.GetBytes(expected), File.ReadAllBytes(Path.Combine(
                LastErrorHeaderTests.RepositoryRoot(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", "winsync.h")));
            Assert.AreEqual(35, Regex.Matches(expected, "_COM_Outptr_").Count - Regex.Matches(before, "_COM_Outptr_").Count);
            Assert.AreEqual(before, expected.Replace("/* [iid_is][out] */ _COM_Outptr_ void **",
                "/* [iid_is][out] */ void **", StringComparison.Ordinal));
        }

        internal static string PatchHeader(string text)
        {
            foreach (string selector in Selectors)
            {
                string[] parts = selector.Split("::");
                string[] scopes = parts[0] switch
                {
                    "IClockVector" => new[] { "IClockVector", "IFeedClockVector" },
                    "ISyncKnowledge" => new[] { "ISyncKnowledge", "IForgottenKnowledge", "ISyncKnowledge2" },
                    _ => new[] { parts[0] },
                };
                int count = 0;
                foreach (string scope in scopes)
                    text = Regex.Replace(text, @"#ifndef __" + scope + @"_INTERFACE_DEFINED__[\s\S]+?#endif\s+/\* __" +
                        scope + @"_INTERFACE_DEFINED__ \*/", definition => Regex.Replace(definition.Value,
                            @"(?:virtual HRESULT STDMETHODCALLTYPE " + parts[1] +
                            @"\s*\(|HRESULT\s*\(\s*STDMETHODCALLTYPE\s*\*" + parts[1] + @"\s*\)\s*\()[^;]+;",
                            method =>
                            {
                                string token = "/* [iid_is][out] */ void **" + parts[2];
                                Assert.AreEqual(1, Regex.Matches(method.Value, Regex.Escape(token)).Count, selector);
                                count++;
                                return method.Value.Replace(token, "/* [iid_is][out] */ _COM_Outptr_ void **" + parts[2],
                                    StringComparison.Ordinal);
                            }));
                Assert.AreEqual(parts[0] == "ISyncKnowledge" ? 4 : parts[0] == "IClockVector" ? 3 : 2, count, selector);
            }
            return text;
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ShippedNativeDeclarations_PreserveEveryFinalMetadataByte(string target) =>
            AssertShippedDeclarations(target, Path.Combine(LastErrorHeaderTests.RepositoryRoot(),
                "generation", "WinSDK", "RecompiledIdlHeaders"));

        internal static void AssertShippedDeclarations(string target, string headers)
        {
            const string prefix = @"
#define STDMETHODCALLTYPE __stdcall
typedef long HRESULT;
typedef unsigned char BYTE;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID riid, _COM_Outptr_ void** ppvObject) = 0; };
";
            using var fixture = new LastErrorHeaderTests.Fixture();
            var remaps = Selectors.ToDictionary(selector => selector, _ => "[ComOutPtr]", StringComparer.Ordinal);
            var result = fixture.Scrape(prefix + Declarations(true), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] baseline = File.ReadAllBytes(fixture.Emit(remaps));
            result = fixture.Scrape(prefix + Declarations(false), target, sal: true);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string output = fixture.Emit();
            AssertFinalFacts(output);
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(output));
            CollectionAssert.AreEqual(baseline, File.ReadAllBytes(fixture.Emit(remaps)));

            string Declarations(bool legacy)
            {
                string text = legacy ? ReadOriginal("RecompiledIdlHeaders/um/winsync.h") :
                    File.ReadAllText(Path.Combine(headers, "um", "winsync.h"));
                string declarations = "";
                foreach (var group in Selectors.GroupBy(selector => selector.Split("::")[0]))
                {
                    string definition = AudioComOutputHeaderTests.Definition(text, group.Key);
                    Match uuid = Regex.Match(definition, @"MIDL_INTERFACE\(""(?<uuid>[^""]+)""\)");
                    Assert.IsTrue(uuid.Success, group.Key);
                    declarations += "struct __declspec(uuid(\"" + uuid.Groups["uuid"].Value + "\")) " + group.Key + " : public IUnknown {\n";
                    foreach (string selector in group)
                    {
                        var matches = Regex.Matches(definition, MethodPattern(selector.Split("::")[1]));
                        Assert.AreEqual(1, matches.Count, selector);
                        declarations += matches[0].Value + "\n";
                    }
                    declarations += "};\n";
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
                string owner = reader.GetString(reader.GetTypeDefinition(method.GetDeclaringType()).Name);
                string name = reader.GetString(method.Name);
                var signature = reader.GetBlobReader(method.Signature);
                signature.ReadSignatureHeader();
                var parameters = method.GetParameters().Select(reader.GetParameter).Where(parameter => parameter.SequenceNumber != 0).ToArray();
                Assert.AreEqual(parameters.Length, signature.ReadCompressedInteger());
                ReadType(ref signature);
                foreach (var parameter in parameters)
                {
                    int start = signature.Offset;
                    ReadType(ref signature);
                    string selector = owner + "::" + name + "::" + reader.GetString(parameter.Name);
                    if (!Selectors.Contains(selector)) continue;
                    Assert.IsTrue(found.Add(selector), selector);
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes, selector);
                    Assert.AreEqual(MethodImplAttributes.IL, method.ImplAttributes & MethodImplAttributes.PreserveSig, selector);
                    var attributes = parameter.GetCustomAttributes().Select(reader.GetCustomAttribute).ToArray();
                    CollectionAssert.AreEqual(new[] { "ComOutPtrAttribute" },
                        attributes.Select(attribute => ParameterHeaderTests.AttributeName(reader, attribute)).ToArray(), selector);
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attributes.Single().Value), selector);
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

        private static string MethodPattern(string name) =>
            @"virtual HRESULT STDMETHODCALLTYPE " + Regex.Escape(name) + @"\s*\([^;]+;";
    }
}
