using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class SdkPatchTests
    {
        private const string Version = "10.0.28000.2705";

        [TestMethod]
        public void MixedMidl_ConvertsOtherParametersWithoutChangingAuthoredAnnotationsOrNonCode()
        {
            using var fixture = new Fixture();
            var cases = new (string Input, string Expected)[]
            {
                (@"HRESULT F([in] int a, [out, annotation(""_COM_Outptr_"")] void** b);",
                 @"HRESULT F([in][annotation(""_In_"")] int a, [out, annotation(""_COM_Outptr_"")] void** b);"),
                (@"HRESULT F([in, annotation(""_In_opt_"")] int* a, [out] int* b);",
                 @"HRESULT F([in, annotation(""_In_opt_"")] int* a, [out][annotation(""_Out_"")] int* b);"),
                (@"HRESULT F([in][annotation (""_In_opt_"")] int* a, [out] int* b);",
                 @"HRESULT F([in][annotation (""_In_opt_"")] int* a, [out][annotation(""_Out_"")] int* b);"),
                (@"HRESULT F([annotation(""_Out_writes_(n)"")] [out, size_is(n)] int* a, [in] int n);",
                 @"HRESULT F([annotation(""_Out_writes_(n)"")] [out, size_is(n)] int* a, [in][annotation(""_In_"")] int n);"),
                (@"HRESULT F([in] [out] int* a, [out, iid_is(iid)] void** b);",
                 @"HRESULT F([in, out][annotation(""_Inout_"")] int* a, [out, iid_is(iid)][annotation(""_COM_Outptr_"")] void** b);"),
                (@"HRESULT F([in]/* keep [out] */[out] int* a);",
                 @"HRESULT F([in,/* keep [out] */out][annotation(""_Inout_"")] int* a);"),
                (@"HRESULT F([in, defaultvalue(""[out][in]"")] int a);",
                 @"HRESULT F([in, defaultvalue(""[out][in]"")][annotation(""_In_"")] int a);"),
                (@"HRESULT F([annotation(""text \""[out][in]\"" // /* annotation("")] int a, [in] int b);",
                 @"HRESULT F([annotation(""text \""[out][in]\"" // /* annotation("")] int a, [in][annotation(""_In_"")] int b);"),
                (@"const char* s = ""[out][in] annotation(\""x\"")""; HRESULT F([out] int* a);",
                 @"const char* s = ""[out][in] annotation(\""x\"")""; HRESULT F([out][annotation(""_Out_"")] int* a);"),
                ("// [in][out] annotation(\"ignored\")\nHRESULT F([in] int a);",
                 "// [in][out] annotation(\"ignored\")\nHRESULT F([in][annotation(\"_In_\")] int a);"),
                ("/* [in]\n[out] annotation(\"ignored\") */ HRESULT F([in] int a);",
                 "/* [in]\n[out] annotation(\"ignored\") */ HRESULT F([in][annotation(\"_In_\")] int a);"),
                (@"HRESULT F([in] int a); // [out][in] annotation(""ignored"")",
                 @"HRESULT F([in][annotation(""_In_"")] int a); // [out][in] annotation(""ignored"")"),
                (@"cpp_quote (""HRESULT F([out] int* p);"")",
                 @"cpp_quote (""HRESULT F([out] int* p);"")"),
                (@"  #pragma message(""[in]"")", @"  #pragma message(""[in]"")"),
                (@"[uuid(""00000000-0000-0000-0000-000000000000"")] interface F;",
                 @"[uuid(""00000000-0000-0000-0000-000000000000"")] interface F;"),
                (@"HRESULT F([string, unique] wchar_t* p, int a[4]);",
                 @"HRESULT F([string, unique] wchar_t* p, int a[4]);"),
                (@"HRESULT F([out, size_is(counts[index])] int* p);",
                 @"HRESULT F([out, size_is(counts[index])][annotation(""_Out_writes_(counts[index])"")] int* p);"),
                (@"const char c = '['; HRESULT F([out] int* p);",
                 @"const char c = '['; HRESULT F([out][annotation(""_Out_"")] int* p);"),
                ("HRESULT F([out, annotation(\"text\\\n[in][out]\")] int* a, [in] int b);",
                 "HRESULT F([out, annotation(\"text\\\n[in][out]\")] int* a, [in][annotation(\"_In_\")] int b);"),
                (@"HRESULT F([in] int a, [out, annotation /* keep */ (""_Out_"")] int* b);",
                 @"HRESULT F([in][annotation(""_In_"")] int a, [out, annotation /* keep */ (""_Out_"")] int* b);"),
                (@"HRESULT F([in] [out, size_is(counts[x][y])] int* p);",
                 @"HRESULT F([in, out, size_is(counts[x][y])][annotation(""_Inout_updates_(counts[x][y])"")] int* p);"),
                (@"HRESULT F([in, out, unique, size_is(capacity), length_is(*written)] byte* p);",
                 @"HRESULT F([in, out, unique, size_is(capacity), length_is(*written)][annotation(""_Inout_updates_to_opt_(capacity,*written)"")] byte* p);"),
                (@"HRESULT F([in, unique, size_is(cbReserved1)] byte *reserved1);",
                 @"HRESULT F([in, unique, size_is(cbReserved1)][annotation(""_In_reads_opt_(cbReserved1)"")] byte *reserved1);"),
                (@"HRESULT F([out, unique, size_is(capacity), length_is(*written)] byte* p);",
                 @"HRESULT F([out, unique, size_is(capacity), length_is(*written)][annotation(""_Out_writes_to_opt_(capacity,*written)"")] byte* p);"),
                (@"HRESULT F([in, unique] int* p, [out, unique] int* q);",
                 @"HRESULT F([in, unique][annotation(""_In_opt_"")] int* p, [out, unique][annotation(""_Out_opt_"")] int* q);"),
                (@"HRESULT F([out, unique, iid_is(iid)] void** p);",
                 @"HRESULT F([out, unique, iid_is(iid)][annotation(""_COM_Outptr_opt_"")] void** p);"),
                (@"HRESULT F([out, retval] int* p, [in] int n);",
                 @"HRESULT F([out, retval][annotation(""_Out_"")] int* p, [in][annotation(""_In_"")] int n);"),
                (@"HRESULT F([out, retval, iid_is(iid)] void** p);",
                 @"HRESULT F([out, retval, iid_is(iid)][annotation(""_COM_Outptr_"")] void** p);"),
                (@"HRESULT F([out, retval, unique, iid_is(iid)] void** p);",
                 @"HRESULT F([out, retval, unique, iid_is(iid)][annotation(""_COM_Outptr_opt_"")] void** p);"),
                (@"HRESULT F([out, retval, unique, size_is(capacity), length_is(*written)] byte* p);",
                 @"HRESULT F([out, retval, unique, size_is(capacity), length_is(*written)][annotation(""_Out_writes_to_opt_(capacity,*written)"")] byte* p);"),
                (@"HRESULT F([in, unique, annotation(""_In_"")] int* p, [out] int* q);",
                 @"HRESULT F([in, unique, annotation(""_In_"")] int* p, [out][annotation(""_Out_"")] int* q);"),
                (@"HRESULT F([annotation(""_In_"")][annotation(""_Out_"")][in, out] int* p);",
                 @"HRESULT F([annotation(""_In_"")][annotation(""_Out_"")][in, out] int* p);"),
                (@"HRESULT F([out, retval, annotation(""_Out_"")] int* p); // retval must remain native",
                 @"HRESULT F([out, retval, annotation(""_Out_"")] int* p); // retval must remain native"),
                ("HRESULT F([in]\n[annotation(\"_In_opt_\")] int* p, [out] int* q);",
                 "HRESULT F([in]\n[annotation(\"_In_opt_\")] int* p, [out][annotation(\"_Out_\")] int* q);"),
                ("HRESULT F([in,\n out, unique] int* p);",
                 "HRESULT F([in,\n out, unique][annotation(\"_Inout_opt_\")] int* p);"),
                ("HRESULT F([in]\n/* [out] */[annotation(\"_In_opt_\")] int* p);",
                 "HRESULT F([in]\n/* [out] */[annotation(\"_In_opt_\")] int* p);"),
                ("HRESULT F([out,\n annotation(\"_Out_opt_\")] int* p, [in] int n);",
                 "HRESULT F([out,\n annotation(\"_Out_opt_\")] int* p, [in][annotation(\"_In_\")] int n);"),
            };
            string command = "";
            for (int i = 0; i < cases.Length; i++)
            {
                string input = fixture.Header($"case{i}.idl", Encoding.UTF8.GetBytes(cases[i].Input));
                string output = Path.Combine(fixture.Path, $"case{i}.converted.idl");
                string repeated = Path.Combine(fixture.Path, $"case{i}.repeated.idl");
                command += $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(input)} {Quote(output)}; " +
                    $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(output)} {Quote(repeated)}; ";
            }
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            for (int i = 0; i < cases.Length; i++)
            {
                string output = Path.Combine(fixture.Path, $"case{i}.converted.idl");
                Assert.AreEqual(cases[i].Expected.Replace("\n", Environment.NewLine) + Environment.NewLine,
                    File.ReadAllText(output), $"Mixed MIDL case {i}");
                CollectionAssert.AreEqual(File.ReadAllBytes(output),
                    File.ReadAllBytes(Path.Combine(fixture.Path, $"case{i}.repeated.idl")), $"Idempotence case {i}");
            }
        }

        [TestMethod]
        public void MixedMidl_RealRegbagPreservesInputsAndComOutputThroughMidlAndEveryFinalTarget()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            byte[] original = File.ReadAllBytes(Path.Combine(include, "um", "regbag.idl"));
            CollectionAssert.AreEqual(PropertyComOutputHeaderTests.ReadOriginalBytes(
                "RecompiledIdlHeaders/um/regbag.idl"), original);
            string source = fixture.Header("regbag.idl", original);
            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            var pending = new Queue<string>(new[] { "regbag.idl" });
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string command = "";
            while (pending.Count > 0)
            {
                string name = pending.Dequeue();
                if (!seen.Add(name)) continue;
                string input = name == "regbag.idl" ? source :
                    new[] { "um", "shared", "winrt" }.Select(area => Path.Combine(include, area, name)).Single(File.Exists);
                foreach (Match import in Regex.Matches(File.ReadAllText(input),
                    @"(?im)^\s*(?:import\s+|#include\s*)[""<]([^"">]+\.idl)["">]"))
                    pending.Enqueue(import.Groups[1].Value);
                command += $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(input)} {Quote(Path.Combine(converted, name))}; ";
            }
            string header = Path.Combine(fixture.Headers, "um", "regbag.h");
            string setup = ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            string midl = "& " +
                Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, "regbag.idl")) + " /out " + Quote(converted) +
                " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            string mixed = Encoding.Latin1.GetString(original).Replace("[out] LPVOID *ppBag",
                "[out, annotation(\"_COM_Outptr_\")] LPVOID *ppBag", StringComparison.Ordinal);
            Assert.AreNotEqual(Encoding.Latin1.GetString(original), mixed);
            Assert.IsFalse(mixed.Contains("annotation(\"_In_\")", StringComparison.Ordinal),
                "The pilot must not use the four explicit input workarounds.");
            string mixedSource = Path.Combine(fixture.Path, "mixed.idl");
            string originalHeader = Path.Combine(fixture.Path, "original.h");
            File.WriteAllBytes(mixedSource, Encoding.Latin1.GetBytes(mixed));
            var result = fixture.Run(setup + command + midl +
                $"Copy-Item -LiteralPath {Quote(header)} -Destination {Quote(originalHeader)}; " +
                $"Copy-Item -LiteralPath {Quote(mixedSource)} -Destination {Quote(source)} -Force; " +
                $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(source)} " +
                $"{Quote(Path.Combine(converted, "regbag.idl"))}; " + midl);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string before = Encoding.Latin1.GetString(File.ReadAllBytes(originalHeader));
            string outputParameter = @"_Out_(?<suffix>\s+LPVOID\s*\*ppBag)";
            Assert.AreEqual(2, Regex.Matches(before, outputParameter).Count, "C++ method and C vtable slot.");
            CollectionAssert.AreEqual(Encoding.Latin1.GetBytes(Regex.Replace(before, outputParameter, "_COM_Outptr_${suffix}")),
                File.ReadAllBytes(header), "Every regenerated header byte except the two output SAL tokens.");
            string after = Encoding.Latin1.GetString(File.ReadAllBytes(header));
            const string prefix = @"
#define STDMETHODCALLTYPE __stdcall
#define MIDL_INTERFACE(iid) struct __declspec(uuid(iid))
typedef long HRESULT;
typedef unsigned long DWORD;
typedef void* HKEY;
typedef const wchar_t* LPCOLESTR;
typedef void* LPVOID;
struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };
typedef const GUID& REFIID;
struct IUnknown { virtual HRESULT __stdcall QueryInterface(REFIID iid, _COM_Outptr_ void** p) = 0; };
";
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
            {
                using var emission = new LastErrorHeaderTests.Fixture();
                var legacy = new Dictionary<string, string> { ["ICreatePropBagOnRegKey::Create::ppBag"] = "[ComOutPtr]" };
                result = emission.Scrape(prefix + Definition(before), target, sal: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] expected = File.ReadAllBytes(emission.Emit(legacy));
                result = emission.Scrape(prefix + Definition(after), target, sal: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string winmd = emission.Emit();
                CollectionAssert.AreEqual(expected, File.ReadAllBytes(winmd), target);
                Console.WriteLine("RAW_OUTPUT regbag." + target + ".legacy.winmd " + Convert.ToBase64String(expected));
                Console.WriteLine("RAW_OUTPUT regbag." + target + ".header.winmd " + Convert.ToBase64String(File.ReadAllBytes(winmd)));
                using (var pe = new PEReader(File.OpenRead(winmd)))
                {
                    var reader = pe.GetMetadataReader();
                    var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(method => reader.GetString(method.Name) == "Create");
                    var parameters = method.GetParameters().Select(reader.GetParameter)
                        .Where(parameter => parameter.SequenceNumber != 0).ToArray();
                    Assert.AreEqual(6, parameters.Length);
                    CollectionAssert.AreEqual(new[] { "hkey", "subkey", "ulOptions", "samDesired", "iid", "ppBag" },
                        parameters.Select(parameter => reader.GetString(parameter.Name)).ToArray());
                    Assert.IsTrue(parameters.Take(4).All(parameter => parameter.Attributes == ParameterAttributes.In));
                    Assert.AreEqual(ParameterAttributes.Out, parameters[5].Attributes);
                    Assert.AreEqual(MethodImplAttributes.IL, method.ImplAttributes & MethodImplAttributes.PreserveSig);
                    ParameterHeaderTests.AssertAttribute(reader, parameters[5], "ComOutPtrAttribute");
                    var attribute = reader.GetCustomAttribute(parameters[5].GetCustomAttributes().Single());
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(attribute.Value));
                    CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 },
                        reader.GetBlobBytes(method.Signature).TakeLast(3).ToArray());
                }
                CollectionAssert.AreEqual(expected, File.ReadAllBytes(emission.Emit(legacy)), "Matching redundant legacy: " + target);
            }

            static string Definition(string text)
            {
                Match definition = Regex.Match(text,
                    @"MIDL_INTERFACE\(""[^""]+""\)\s+ICreatePropBagOnRegKey\s*:\s*public IUnknown\s*\{[\s\S]+?\n\s*\};");
                Assert.IsTrue(definition.Success);
                return definition.Value;
            }
        }

        [TestMethod]
        public void MixedMidl_StockSdkCompilesOptionalCountsAndNativeRetvalWithoutPrivateSal()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string nativeSource = "import \"unknwn.idl\";\n" +
                "[object, uuid(64abea93-aac7-4f3b-a257-976deba89f04), pointer_default(unique)]\n" +
                "interface IProbe : IUnknown {\n" +
                "HRESULT Counted([in, out, unique, size_is(capacity), length_is(*written)] byte* buffer, " +
                "[in] unsigned long capacity, [in, out] unsigned long* written);\n" +
                "HRESULT Value([out, retval] long* result);\n" +
                "HRESULT Object([in] REFIID iid, [out, retval, iid_is(iid)] void** result);\n" +
                "[local] HRESULT LocalValue([out, retval] long* result);\n" +
                "};\n";
            string input = fixture.Header("probe.idl", Encoding.UTF8.GetBytes(nativeSource));
            string converted = Path.Combine(fixture.Path, "converted.idl");
            string resultHeader = Path.Combine(fixture.Headers, "um", "probe.h");
            string regbag = File.ReadAllText(Path.Combine(include, "um", "regbag.idl"));
            string mixedRegbag = regbag.Replace("[in] LPCOLESTR subkey", "[in, unique] LPCOLESTR subkey", StringComparison.Ordinal)
                .Replace("[out] LPVOID *ppBag", "[out, annotation(\"_COM_Outptr_\")] LPVOID *ppBag", StringComparison.Ordinal);
            Assert.AreNotEqual(regbag, mixedRegbag);
            string regbagInput = fixture.Header("regbag.idl", Encoding.UTF8.GetBytes(mixedRegbag));
            string regbagConverted = Path.Combine(fixture.Path, "regbag.converted.idl");
            string regbagHeader = Path.Combine(fixture.Headers, "um", "regbag.h");
            string command = $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(input)} {Quote(converted)}; " +
                ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x64); ";
            foreach (string mode in new[] { "without-local", "native", "converted" })
            {
                if (mode == "converted")
                    command += "Copy-Item -LiteralPath " + Quote(converted) + " -Destination " + Quote(input) + " -Force; ";
                command += "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(input) + " /out " + Quote(fixture.Path) + " /header " + Quote(resultHeader) +
                    " /proxy " + Quote(Path.Combine(fixture.Path, "probe_p.c")) +
                    " /iid " + Quote(Path.Combine(fixture.Path, "probe_i.c")) +
                    " /no_stamp /nologo /no_settings_comment /sal " + (mode == "without-local" ? "" : "/sal_local ") +
                    "/amd64 /target NT100 /Zp8" +
                    " /I " + Quote(Path.Combine(include, "um")) + " /I " + Quote(Path.Combine(include, "shared")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; " +
                    "Copy-Item -LiteralPath " + Quote(resultHeader) + " -Destination " +
                    Quote(Path.Combine(fixture.Path, mode + ".h")) + "; " +
                    "Copy-Item -LiteralPath " + Quote(Path.Combine(fixture.Path, "probe_p.c")) + " -Destination " +
                    Quote(Path.Combine(fixture.Path, mode + ".p.c")) + "; ";
            }
            command += $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(regbagInput)} {Quote(regbagConverted)}; " +
                "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(regbagConverted) + " /out " + Quote(fixture.Path) + " /header " + Quote(regbagHeader) +
                " /no_stamp /nologo /no_settings_comment /sal /sal_local /amd64 /target NT100 /Zp8" +
                " /I " + Quote(Path.Combine(include, "um")) + " /I " + Quote(Path.Combine(include, "shared")) +
                "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine("MIDL NATIVE/CONVERTED PROBE OUTPUT:\n" + result.Output);
            string transformed = File.ReadAllText(converted);
            StringAssert.Contains(transformed, "[in, out, unique, size_is(capacity), length_is(*written)]" +
                "[annotation(\"_Inout_updates_to_opt_(capacity,*written)\")]");
            StringAssert.Contains(transformed, "[out, retval][annotation(\"_Out_\")] long* result");
            StringAssert.Contains(transformed, "[out, retval, iid_is(iid)][annotation(\"_COM_Outptr_\")] void** result");
            Assert.IsFalse(transformed.Contains("_Out_retval_", StringComparison.Ordinal));
            Assert.IsFalse(transformed.Contains("_COM_Outptr_retval_", StringComparison.Ordinal));
            string nativeHeader = File.ReadAllText(Path.Combine(fixture.Path, "native.h"));
            string changedHeader = File.ReadAllText(resultHeader);
            string withoutLocalHeader = File.ReadAllText(Path.Combine(fixture.Path, "without-local.h"));
            StringAssert.Contains(nativeHeader, "__RPC__inout_ecount_part_opt(capacity, *written)");
            StringAssert.Contains(changedHeader, "_Inout_updates_to_opt_(capacity,*written)");
            Assert.AreEqual(2, Regex.Matches(nativeHeader,
                @"__RPC__inout_ecount_part_opt\(capacity, \*written\)\s+byte \*buffer").Count, "C++ and C-vtable count/optionality.");
            Assert.AreEqual(2, Regex.Matches(changedHeader,
                @"_Inout_updates_to_opt_\(capacity,\*written\)\s+byte \*buffer").Count, "C++ and C-vtable count/optionality.");
            const string localValuePattern = @"virtual\s+(?:/\*[^*]*\*/\s+)?HRESULT STDMETHODCALLTYPE LocalValue\([\s\S]+?=\s*0;";
            Assert.IsFalse(Regex.Match(withoutLocalHeader, localValuePattern)
                .Value.Contains("__RPC__out", StringComparison.Ordinal));
            StringAssert.Contains(Regex.Match(nativeHeader, localValuePattern).Value, "__RPC__out");
            foreach (string header in new[] { nativeHeader, changedHeader })
            {
                Assert.AreEqual(6, Regex.Matches(header, @"\[retval\]\[out\]").Count, "All three retval methods in C++ and C vtables.");
            }
            Assert.AreEqual(2, Regex.Matches(changedHeader, @"_COM_Outptr_\s+void \*\*result").Count,
                "Standard COM output SAL must survive in C++ and C vtables alongside native retval comments.");
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(fixture.Path, "native.p.c")),
                File.ReadAllBytes(Path.Combine(fixture.Path, "converted.p.c")), "Complete same-path RPC proxy bytes.");
            Console.WriteLine("NATIVE IDL:\n" + nativeSource + "\nCONVERTED IDL:\n" + transformed +
                "\nWITHOUT SAL_LOCAL HEADER:\n" + withoutLocalHeader +
                "\nNATIVE HEADER:\n" + nativeHeader + "\nCONVERTED HEADER:\n" + changedHeader +
                "\nNATIVE RPC PROXY:\n" + File.ReadAllText(Path.Combine(fixture.Path, "native.p.c")) +
                "\nCONVERTED RPC PROXY:\n" + File.ReadAllText(Path.Combine(fixture.Path, "converted.p.c")) +
                "\nSELECTED SDK MIXED REGBAG IDL:\n" + mixedRegbag +
                "\nSELECTED SDK CONVERTED REGBAG IDL:\n" + File.ReadAllText(regbagConverted) +
                "\nSELECTED SDK REGBAG HEADER:\n" + File.ReadAllText(regbagHeader));
            StringAssert.Contains(File.ReadAllText(regbagConverted), "[in, unique][annotation(\"_In_opt_\")] LPCOLESTR subkey");
            Assert.AreEqual(2, Regex.Matches(File.ReadAllText(regbagHeader), @"_In_opt_\s+LPCOLESTR subkey").Count);
            Console.WriteLine("RAW_OUTPUT original.idl " + Convert.ToBase64String(Encoding.UTF8.GetBytes(nativeSource)));
            foreach (string file in new[] { converted, Path.Combine(fixture.Path, "without-local.h"),
                Path.Combine(fixture.Path, "native.h"), Path.Combine(fixture.Path, "converted.h"),
                Path.Combine(fixture.Path, "native.p.c"), Path.Combine(fixture.Path, "converted.p.c"),
                regbagInput, regbagConverted, regbagHeader })
                Console.WriteLine("RAW_OUTPUT " + Path.GetFileName(file) + " " + Convert.ToBase64String(File.ReadAllBytes(file)));

            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#include <windows.h>\n#include \"probe.h\"\n#include \"regbag.h\"\n" +
                "#ifdef _Out_retval_\n#error Private retval SAL must not be required\n#endif\n" +
                "#ifdef _COM_Outptr_retval_\n#error Private COM retval SAL must not be required\n#endif\n");
            command = ". " + Quote(Script("CommonUtils.ps1")) + "; ";
            foreach (string arch in new[] { "x86", "x64", "arm64" })
            {
                command += "$vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                    "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; ";
                foreach (string mode in new[] { "native", "converted" })
                    command += "Copy-Item -LiteralPath " + Quote(Path.Combine(fixture.Path, mode + ".h")) +
                        " -Destination " + Quote(resultHeader) + " -Force; foreach ($language in '/TC', '/TP') { " +
                        "& $compiler /nologo /Zs $language " + Quote(source) + " " + Quote("/I" + Path.GetDirectoryName(resultHeader)) +
                        " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                        " " + Quote("/I" + Path.Combine(include, "shared")) +
                        " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                        "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; }; ";
            }
            result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine("STOCK MSVC OUTPUT:\n" + result.Output);
            Console.WriteLine("Stock MSVC: original/converted headers compile in C/C++ on x86/x64/ARM64 (12 syntax checks).");
            string invalid = Path.Combine(fixture.Path, "nonstock.c");
            File.WriteAllText(invalid, "#include <windows.h>\nHRESULT Bad(_Out_retval_ long* result);\n");
            result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch x64 -HostArch x64; & (Join-Path $vc 'cl.exe') /nologo /Zs /TC " +
                Quote(invalid) + " \"/I$(Join-Path $vc '..\\..\\..\\include')\" " +
                Quote("/I" + Path.Combine(include, "um")) + " " + Quote("/I" + Path.Combine(include, "shared")) +
                " " + Quote("/I" + Path.Combine(include, "ucrt")) + "; exit $LASTEXITCODE");
            Assert.AreNotEqual(0, result.ExitCode, "The old private SAL spelling must not accidentally resolve in stock SDK headers.");
            StringAssert.Contains(result.Output, "syntax error");
            Console.WriteLine("EXPECTED STOCK MSVC REJECTION:\n" + result.Output);

            using var emission = new LastErrorHeaderTests.Fixture();
            const string prefix = "#define STDMETHODCALLTYPE __stdcall\n#define MIDL_INTERFACE(iid) struct __declspec(uuid(iid))\n" +
                "#include <rpcsal.h>\ntypedef long HRESULT;\ntypedef unsigned char byte;\n" +
                "struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };\n" +
                "typedef const GUID& REFIID;\nstruct IUnknown {};\n";
            string value = Regex.Match(changedHeader, @"virtual HRESULT STDMETHODCALLTYPE Value\([\s\S]+?=\s*0;").Value;
            string counted = Regex.Match(changedHeader, @"virtual HRESULT STDMETHODCALLTYPE Counted\([\s\S]+?=\s*0;").Value;
            string comOutput = Regex.Match(changedHeader, @"virtual HRESULT STDMETHODCALLTYPE Object\([\s\S]+?=\s*0;").Value;
            string nativeComOutput = Regex.Match(nativeHeader, @"virtual HRESULT STDMETHODCALLTYPE Object\([\s\S]+?=\s*0;").Value;
            Assert.IsTrue(value.Length > 0);
            Assert.IsTrue(counted.Length > 0);
            Assert.IsTrue(comOutput.Length > 0);
            Assert.IsTrue(nativeComOutput.Length > 0);
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
            {
                string wrapper = prefix + "MIDL_INTERFACE(\"64abea93-aac7-4f3b-a257-976deba89f04\") IProbe : IUnknown { ";
                string settings = "--include-directory\n" + Path.Combine(include, "shared") + "\n";
                result = emission.Scrape(wrapper + nativeComOutput + " };", target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] nativeCom = File.ReadAllBytes(emission.Emit(new Dictionary<string, string>
                    { ["IProbe::Object::result"] = "[ComOutPtr]" }));
                result = emission.Scrape(wrapper + comOutput + " };", target, sal: true, extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                byte[] convertedCom = File.ReadAllBytes(emission.Emit());
                CollectionAssert.AreEqual(nativeCom, convertedCom,
                    "Native retval/iid_is plus legacy COM versus standard COM SAL with native RetVal: " + target);
                Console.WriteLine("RAW_OUTPUT com-retval." + target + ".native-legacy-com.winmd " + Convert.ToBase64String(nativeCom));
                Console.WriteLine("RAW_OUTPUT com-retval." + target + ".standard-com.winmd " + Convert.ToBase64String(convertedCom));
                result = emission.Scrape(prefix + "MIDL_INTERFACE(\"64abea93-aac7-4f3b-a257-976deba89f04\") IProbe : IUnknown { " +
                    counted + value + comOutput + " };", target, sal: true,
                    extraSettings: settings);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string path = emission.Emit();
                byte[] nativeRetval = File.ReadAllBytes(path);
                Console.WriteLine("RAW_OUTPUT retval." + target + ".native-reader.winmd " + Convert.ToBase64String(nativeRetval));
                using (var pe = new PEReader(File.OpenRead(path)))
                {
                    var reader = pe.GetMetadataReader();
                    var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(method => reader.GetString(method.Name) == "Value");
                    var parameter = method.GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == "result");
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes);
                    ParameterHeaderTests.AssertAttribute(reader, parameter, "RetValAttribute");
                    var retval = reader.GetCustomAttribute(parameter.GetCustomAttributes().Single());
                    CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, reader.GetBlobBytes(retval.Value));
                    method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(method => reader.GetString(method.Name) == "Counted");
                    parameter = method.GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == "buffer");
                    Assert.AreEqual(ParameterAttributes.In | ParameterAttributes.Out | ParameterAttributes.Optional, parameter.Attributes);
                    method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(method => reader.GetString(method.Name) == "Object");
                    parameter = method.GetParameters().Select(reader.GetParameter)
                        .Single(parameter => reader.GetString(parameter.Name) == "result");
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes);
                    ParameterHeaderTests.AssertAttribute(reader, parameter, "ComOutPtrAttribute");
                    ParameterHeaderTests.AssertAttribute(reader, parameter, "RetValAttribute");
                    CollectionAssert.AreEqual(new byte[] { 0x0f, 0x0f, 0x01 },
                        reader.GetBlobBytes(method.Signature).TakeLast(3).ToArray());
                }
                byte[] withRetval = File.ReadAllBytes(emission.Emit(new Dictionary<string, string>
                    { ["IProbe::Value::result"] = "[RetVal]" }));
                Console.WriteLine("RAW_OUTPUT retval." + target + ".legacy-retained.winmd " + Convert.ToBase64String(withRetval));
                CollectionAssert.AreEqual(nativeRetval, withRetval,
                    "Matching legacy RetVal must be redundant with the authoritative native marker.");
            }
            Console.WriteLine("Native [retval] comments reach final RetValAttribute on all three targets.");
        }

        [TestMethod]
        public void DxgiRetval_ReplaysAuthoritativeOwnerThroughProductionMidl()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string original = Path.Combine(include, "shared", "dxgi.idl");
            fixture.Header("dxgi.idl", File.ReadAllBytes(original), "shared");
            fixture.Apply("pre-midl", "pre-midl/dxgi.idl.patch", true,
                patchRoot: Path.Combine(Root(), "generation", "WinSDK", "patches"), midlInputRoot: include);
            string input = Path.Combine(fixture.Headers, "shared", "dxgi.idl");
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(MidlHeaderTests.SharedHeaders(), "dxgi.idl")),
                File.ReadAllBytes(input));
            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            var pending = new Queue<string>(new[] { "dxgi.idl" });
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string command = "";
            while (pending.Count > 0)
            {
                string name = pending.Dequeue();
                if (!seen.Add(name)) continue;
                string source = name == "dxgi.idl" ? input : new[] { "um", "shared", "winrt" }
                    .Select(area => Path.Combine(include, area, name)).Single(File.Exists);
                foreach (Match import in Regex.Matches(File.ReadAllText(source),
                    @"(?im)^\s*(?:import\s+|#include\s*)[""<]([^"">]+\.idl)["">]"))
                    pending.Enqueue(import.Groups[1].Value);
                command += $"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(source)} " +
                    $"{Quote(Path.Combine(converted, name))}; ";
            }
            string header = Path.Combine(fixture.Headers, "shared", "dxgi.h");
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); & " +
                Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, "dxgi.idl")) + " /out " + Quote(converted) + " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Console.WriteLine("RAW_OUTPUT dxgi.regenerated.h " + Convert.ToBase64String(File.ReadAllBytes(header)));
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(MidlHeaderTests.SharedHeaders(), "dxgi.h")), File.ReadAllBytes(header));
            Assert.AreEqual(3, Regex.Matches(File.ReadAllText(header),
                @"/\* \[annotation\]\[retval\]\[out\] \*/\s+_Out_\s+DXGI_ADAPTER_DESC \*pDesc").Count);
            Assert.IsFalse(File.ReadAllText(header).Contains("_Win32_Retval_", StringComparison.Ordinal));
        }

        [TestMethod]
        public void VartypeSdkField_PreMidlCarrierKeepsNativeUInt16DistinctFromVarenum()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string original = File.ReadAllText(Path.Combine(include, "um", "oaidl.Idl"));
            string input = fixture.Header("oaidl.Idl", Encoding.UTF8.GetBytes(original));
            string header = Path.Combine(fixture.Headers, "um", "oaidl.h");
            File.Copy(Path.Combine(MidlHeaderTests.SharedHeaders(), "win32metadata_annotations.h"),
                Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            const string carrier = "_WIN32META_ANNOTATE_FIELD_(TYPEDESC::vt, _Win32_AssociatedEnum_(VARENUM));";
            string command = ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x64); ";
            foreach (string mode in new[] { "native", "associated" })
            {
                string source = Path.Combine(fixture.Path, mode + ".idl");
                File.WriteAllText(source, "#include <win32metadata_annotations.h>\n" + original +
                    (mode == "associated" ? "\n_Win32_AnnotateField_(TYPEDESC::vt, _Win32_AssociatedEnum_(VARENUM))\n" : ""));
                command += "Copy-Item -LiteralPath " + Quote(source) + " -Destination " + Quote(input) + " -Force; & " +
                    Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(input) + " /out " + Quote(fixture.Path) + " /header " + Quote(header) +
                    " /no_stamp /nologo /no_settings_comment /DMIDL_PASS=1 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(fixture.Headers) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(header) +
                    " -Destination " + Quote(Path.Combine(fixture.Path, mode + ".h")) + "; ";
            }
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string before = File.ReadAllText(Path.Combine(fixture.Path, "native.h"));
            string after = File.ReadAllText(header);
            const string pattern = @"typedef struct tagTYPEDESC\s*\{[\s\S]+?\}\s*TYPEDESC;";
            Match record = Regex.Match(before, pattern);
            Assert.IsTrue(record.Success);
            Assert.AreEqual(record.Value, Regex.Match(after, pattern).Value, "Every native union/field declaration byte.");
            StringAssert.Contains(after, carrier);
            string wtypes = File.ReadAllText(Path.Combine(include, "shared", "wtypes.h"));
            string sourcePrefix = EnumNativeSdkTests.VartypeDeclarations(before, wtypes);
            Assert.IsTrue(sourcePrefix.Contains("VT_EMPTY", StringComparison.Ordinal));
            var legacy = new Dictionary<string, string> { ["TYPEDESC::vt"] = "[AssociatedEnum(\"VARENUM\")]" };
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
            {
                using var emission = new LastErrorHeaderTests.Fixture();
                result = emission.Scrape(sourcePrefix, target, sal: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                string output = emission.Emit();
                using (var pe = new PEReader(File.OpenRead(output)))
                {
                    var reader = pe.GetMetadataReader();
                    var field = NativeTypedefHeaderTests.Type(reader, "TYPEDESC").GetFields()
                        .Select(reader.GetFieldDefinition).Single(field => reader.GetString(field.Name) == "vt");
                    Assert.AreEqual(0, EnumAssociationHeaderTests.StringAttributes(reader,
                        field.GetCustomAttributes(), "AssociatedEnumAttribute").Length);
                }
                byte[] expected = File.ReadAllBytes(emission.Emit(legacy));
                result = emission.Scrape(sourcePrefix + carrier, target, sal: true);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                output = emission.Emit();
                CollectionAssert.AreEqual(expected, File.ReadAllBytes(output), "Native field width plus association, not enum substitution.");
                using (var pe = new PEReader(File.OpenRead(output)))
                {
                    var reader = pe.GetMetadataReader();
                    var field = NativeTypedefHeaderTests.Type(reader, "TYPEDESC").GetFields()
                        .Select(reader.GetFieldDefinition).Single(field => reader.GetString(field.Name) == "vt");
                    EnumAssociationHeaderTests.AssertSignature(reader, field.Signature, 0, SignatureTypeCode.UInt16);
                    CollectionAssert.AreEqual(new[] { "VARENUM" }, EnumAssociationHeaderTests.StringAttributes(reader,
                        field.GetCustomAttributes(), "AssociatedEnumAttribute"));
                    var storage = NativeTypedefHeaderTests.Type(reader, "VARENUM").GetFields()
                        .Select(reader.GetFieldDefinition).Single(field => reader.GetString(field.Name) == "value__");
                    EnumAssociationHeaderTests.AssertSignature(reader, storage.Signature, 0, SignatureTypeCode.Int32);
                }
                CollectionAssert.AreEqual(expected, File.ReadAllBytes(emission.Emit(legacy)));
                Console.WriteLine("RAW_OUTPUT vartype-field." + target + ".winmd " + Convert.ToBase64String(expected));
            }
            foreach (string file in new[] { "native.idl", "associated.idl", "native.h", "associated.h" })
                Console.WriteLine("RAW_OUTPUT vartype-field." + file + " " +
                    Convert.ToBase64String(File.ReadAllBytes(Path.Combine(fixture.Path, file))));
        }

        [TestMethod]
        public void NamedMidlCarriers_PreserveNativeRpcSalAndAbiAndReachExactFinalTargets()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string helpers = fixture.Headers;
            File.Copy(Path.Combine(MidlHeaderTests.SharedHeaders(), "win32metadata_annotations.h"),
                Path.Combine(helpers, "win32metadata_annotations.h"));
            const string idl = @"
                #include <win32metadata_annotations.h>
                import ""unknwn.idl"";
                cpp_quote(""#include <win32metadata_annotations.h>"")
                typedef enum PROBE_OPTION { First = 1 } PROBE_OPTION;
                typedef struct PROBE_RECORD { unsigned short field; unsigned short neighbor; } PROBE_RECORD;
                [object, uuid(64abea93-aac7-4f3b-a257-976deba89f04), pointer_default(unique)]
                interface IProbe : IUnknown {
                    HRESULT Counted([in, out, unique, size_is(capacity), length_is(*written)] byte* buffer,
                        [in] unsigned long capacity, [in, out] unsigned long* written);
                    HRESULT Value([out, retval] long* result);
                    HRESULT Object([in] REFIID iid, [out, retval, iid_is(iid)] void** result);
                    [local] HRESULT LocalValue([out, retval] long* result);
                    HRESULT ArrayValue([in] unsigned long capacity, [out] unsigned long* written,
                        [out, retval, size_is(capacity), length_is(*written)] byte* result);
                };
                ";
            const string carriers = @"

                _Win32_AnnotateMethod_(IProbe::LocalValue, _Win32_PreserveResult_)
                _Win32_AnnotateParameter_(IProbe::Counted, buffer, _Win32_Retained_)
                _Win32_AnnotateParameter_(IProbe::Counted, buffer, _Win32_ContainsInteriorPointers_)
                _Win32_AnnotateField_(PROBE_RECORD::field, _Win32_AssociatedEnum_(PROBE_OPTION))
                ";
            string input = fixture.Header("probe.idl", Encoding.UTF8.GetBytes(idl));
            string header = Path.Combine(fixture.Headers, "um", "probe.h");
            string command = ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x64); ";
            foreach (string mode in new[] { "baseline", "carriers" })
            {
                string source = Path.Combine(fixture.Path, mode + ".idl");
                File.WriteAllText(source, idl + (mode == "carriers" ? carriers : ""));
                command += "Copy-Item -LiteralPath " + Quote(source) + " -Destination " + Quote(input) + " -Force; " +
                    "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(input) + " /out " + Quote(fixture.Path) + " /header " + Quote(header) +
                    " /proxy " + Quote(Path.Combine(fixture.Path, "probe_p.c")) +
                    " /iid " + Quote(Path.Combine(fixture.Path, "probe_i.c")) +
                    " /no_stamp /nologo /no_settings_comment /DMIDL_PASS=1 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(helpers) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; " +
                    "Copy-Item -LiteralPath " + Quote(header) + " -Destination " + Quote(Path.Combine(fixture.Path, mode + ".h")) + "; " +
                    "Copy-Item -LiteralPath " + Quote(Path.Combine(fixture.Path, "probe_p.c")) +
                    " -Destination " + Quote(Path.Combine(fixture.Path, mode + ".p.c")) + "; ";
            }
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string baseline = File.ReadAllText(Path.Combine(fixture.Path, "baseline.h"));
            string changed = File.ReadAllText(header);
            string methodPattern = @"MIDL_INTERFACE\(""[^""]+""\)\s+IProbe\s*:\s*public IUnknown\s*\{[\s\S]+?\n\s*\};";
            string vtablePattern = @"typedef struct IProbeVtbl\s*\{[\s\S]+?\}\s*IProbeVtbl;";
            foreach (string pattern in new[] { methodPattern, vtablePattern,
                @"typedef\s+enum PROBE_OPTION[\s\S]+?PROBE_OPTION;", @"typedef struct PROBE_RECORD[\s\S]+?PROBE_RECORD;" })
            {
                Assert.IsTrue(Regex.IsMatch(baseline, pattern), pattern);
                Assert.AreEqual(Regex.Match(baseline, pattern).Value, Regex.Match(changed, pattern).Value,
                    "Carriers must preserve every generated native declaration byte, including local SAL and both count expressions.");
            }
            StringAssert.Contains(changed, "__RPC__inout_ecount_part_opt(capacity, *written)");
            StringAssert.Contains(changed, "__RPC__out_ecount_part(capacity, *written)");
            StringAssert.Contains(changed, "_WIN32META_ANNOTATE_METHOD_(IProbe::LocalValue");
            StringAssert.Contains(changed, "_WIN32META_ANNOTATE_PARAMETER_(IProbe::Counted");
            StringAssert.Contains(changed, "_WIN32META_ANNOTATE_FIELD_(PROBE_RECORD::field");
            string Tokens(string path) => Regex.Replace(Regex.Replace(File.ReadAllText(path),
                @"/\*[\s\S]*?\*/|//[^\r\n]*", ""), @"\s+", "");
            Assert.AreEqual(Tokens(Path.Combine(fixture.Path, "baseline.p.c")),
                Tokens(Path.Combine(fixture.Path, "carriers.p.c")), "Complete noncomment RPC proxy tokens; not runtime RPC proof.");
            foreach (string file in new[] { "baseline.idl", "carriers.idl", "baseline.h", "carriers.h", "baseline.p.c", "carriers.p.c" })
                Console.WriteLine("RAW_OUTPUT named-carriers." + file + " " +
                    Convert.ToBase64String(File.ReadAllBytes(Path.Combine(fixture.Path, file))));

            string native = Path.Combine(fixture.Path, "native.c");
            string obj = Path.Combine(fixture.Path, "native.obj");
            File.WriteAllText(native, @"
                #include <windows.h>
                #include ""probe.h""
                typedef char RecordLayout[(sizeof(PROBE_RECORD) == 4) ? 1 : -1];
                #if !defined(__cplusplus) || defined(CINTERFACE)
                typedef char SlotLayout[(sizeof(IProbeVtbl) == 8 * sizeof(void*)) ? 1 : -1];
                #endif
                #if defined(__cplusplus) && !defined(CINTERFACE)
                HRESULT NativeCall(IProbe* self, long* result) { return self->Value(result); }
                #else
                HRESULT NativeCall(IProbe* self, long* result) { return self->lpVtbl->Value(self, result); }
                #endif
                ");
            foreach (string arch in new[] { "x86", "x64", "arm64" })
            {
                command = ". " + Quote(Script("CommonUtils.ps1")) + "; " +
                    "$vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; ";
                foreach (string language in new[] { "TC", "TP", "CINTERFACE" })
                    foreach (string mode in new[] { "baseline", "carriers", "metadata" })
                        command += "Copy-Item -LiteralPath " +
                            Quote(Path.Combine(fixture.Path, (mode == "baseline" ? "baseline" : "carriers") + ".h")) +
                            " -Destination " + Quote(header) + " -Force; & (Join-Path $vc 'cl.exe') /nologo /c " +
                            (language == "CINTERFACE" ? "/TP /DCINTERFACE" : "/" + language) + " " + Quote(native) + " " + Quote("/Fo" + obj) +
                            " " + Quote("/I" + Path.GetDirectoryName(header)) + " " + Quote("/I" + helpers) +
                            " \"/I$(Join-Path $vc '..\\..\\..\\include')\" " +
                            string.Join(" ", new[] { "um", "shared", "ucrt" }.Select(area => Quote("/I" + Path.Combine(include, area)))) +
                            (mode == "metadata" ? " /DWIN32METADATA" : "") +
                            "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                            " -Destination " + Quote(Path.Combine(fixture.Path, arch + language + mode + ".obj")) + "; ";
                result = fixture.Run(command);
                Assert.AreEqual(0, result.ExitCode, result.Output);
                Console.WriteLine(result.Output);
            }
            foreach (string arch in new[] { "x86", "x64", "arm64" })
                foreach (string language in new[] { "TC", "TP", "CINTERFACE" })
                {
                    var objects = new[] { "baseline", "carriers", "metadata" }.Select(mode =>
                        File.ReadAllBytes(Path.Combine(fixture.Path, arch + language + mode + ".obj"))).ToArray();
                    foreach (byte[] bytes in objects)
                    {
                        Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                        Array.Clear(bytes, 4, 4);
                    }
                    CollectionAssert.AreEqual(objects[0], objects[1]);
                    CollectionAssert.AreEqual(objects[1], objects[2]);
                }

            const string prefix = @"
                #define STDMETHODCALLTYPE __stdcall
                #define MIDL_INTERFACE(iid) struct __declspec(uuid(iid))
                #define DECLSPEC_XFGVIRT(iface, method)
                #define BEGIN_INTERFACE
                #define END_INTERFACE
                #include <rpcsal.h>
                typedef long HRESULT;
                typedef unsigned long ULONG;
                typedef unsigned char byte;
                typedef struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; } GUID;
                #ifdef __cplusplus
                typedef const GUID& REFIID;
                struct IUnknown {};
                #else
                typedef const GUID* REFIID;
                #endif
                #if !defined(__cplusplus) || defined(CINTERFACE)
                typedef struct IProbe { void* lpVtbl; } IProbe;
                #endif
                ";
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                foreach (string mode in new[] { "c++", "c", "cinterface" })
                {
                    using var emission = new LastErrorHeaderTests.Fixture();
                    string language = mode == "c" ? "c" : "c++";
                    bool methodsOnly = mode == "c++";
                    string declarations = Regex.Match(changed, @"typedef\s+enum PROBE_OPTION[\s\S]+?PROBE_OPTION;").Value + "\n" +
                        Regex.Match(changed, @"typedef struct PROBE_RECORD[\s\S]+?PROBE_RECORD;").Value + "\n" +
                        Regex.Match(changed, methodsOnly ? methodPattern : vtablePattern).Value + "\n" +
                        string.Join("\n", changed.Split('\n').Where(line => line.StartsWith("_WIN32META_ANNOTATE_", StringComparison.Ordinal)));
                    result = emission.Scrape(prefix + "\n" + declarations, target, sal: true, language: language,
                        extraSettings: "--include-directory\n" + Path.Combine(include, "shared") + "\n" +
                            (mode == "cinterface" ? "--additional\n-DCINTERFACE\n" : ""));
                    Assert.AreEqual(0, result.ExitCode, result.Output);
                    string output = emission.Emit();
                    Console.WriteLine("RAW_OUTPUT named-carriers." + target + "." + mode + ".winmd " +
                        Convert.ToBase64String(File.ReadAllBytes(output)));
                    using var pe = new PEReader(File.OpenRead(output));
                    var reader = pe.GetMetadataReader();
                    var methods = reader.MethodDefinitions.Select(reader.GetMethodDefinition).ToArray();
                    Assert.AreEqual(4, methods.SelectMany(m => m.GetParameters()).Select(reader.GetParameter)
                        .Count(p => p.GetCustomAttributes().Select(reader.GetCustomAttribute)
                            .Any(a => ParameterHeaderTests.AttributeName(reader, a) == "RetValAttribute")));
                    var array = methodsOnly ? methods.Single(m => reader.GetString(m.Name) == "ArrayValue") :
                        NativeTypedefHeaderTests.Type(reader, "IProbeVtbl_ArrayValue").GetMethods().Select(reader.GetMethodDefinition)
                            .Single(m => reader.GetString(m.Name) == "Invoke");
                    var parameter = array.GetParameters().Select(reader.GetParameter).Single(p => reader.GetString(p.Name) == "result");
                    ParameterHeaderTests.AssertAttribute(reader, parameter, "RetValAttribute");
                    MidlHeaderTests.AssertCount(reader, parameter, "NativeArrayInfoAttribute", "CountParamIndex",
                        methodsOnly ? 0 : 1, shortValue: true);
                    Assert.AreEqual(ParameterAttributes.Out, parameter.Attributes);
                    var field = NativeTypedefHeaderTests.Type(reader, "PROBE_RECORD").GetFields().Select(reader.GetFieldDefinition).First();
                    if (language == "c++")
                        Assert.AreEqual("AssociatedEnumAttribute", ParameterHeaderTests.AttributeName(reader,
                            reader.GetCustomAttribute(field.GetCustomAttributes().Single())));
                    else
                        Assert.AreEqual(0, field.GetCustomAttributes().Count, "C has no C++ qualified-member carrier transport.");
                    if (methodsOnly)
                    {
                        var local = methods.Single(m => reader.GetString(m.Name) == "LocalValue");
                        Assert.IsTrue(local.ImplAttributes.HasFlag(MethodImplAttributes.PreserveSig));
                        var counted = methods.Single(m => reader.GetString(m.Name) == "Counted");
                        parameter = counted.GetParameters().Select(reader.GetParameter).Single(p => reader.GetString(p.Name) == "buffer");
                        ParameterHeaderTests.AssertAttribute(reader, parameter, "RetainedAttribute");
                        ParameterHeaderTests.AssertAttribute(reader, parameter, "ContainsInteriorPointersAttribute");
                        MidlHeaderTests.AssertCount(reader, parameter, "NativeArrayInfoAttribute", "CountParamIndex", 1, shortValue: true);
                    }
                    else
                    {
                        var objectCallback = NativeTypedefHeaderTests.Type(reader, "IProbeVtbl_Object").GetMethods()
                            .Select(reader.GetMethodDefinition).Single(m => reader.GetString(m.Name) == "Invoke");
                        var iid = objectCallback.GetParameters().Select(reader.GetParameter).Single(p => reader.GetString(p.Name) == "iid");
                        ParameterHeaderTests.AssertAttribute(reader, iid, "ConstAttribute");
                        Assert.AreEqual(ParameterAttributes.In, iid.Attributes);
                    }
                    Assert.IsFalse(reader.TypeDefinitions.Select(reader.GetTypeDefinition)
                        .Any(t => reader.GetString(t.Name).Contains("_WIN32META_", StringComparison.Ordinal)));
                }
        }

        [TestMethod]
        public void EmptySelection_DoesNotRequireOrModifyInputs()
        {
            using var fixture = new Fixture();
            var result = fixture.Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase pre-midl -HeaderRoot missing -SdkVersion other");
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [DataTestMethod]
        [DataRow("\n", false, false)]
        [DataRow("\r\n", false, false)]
        [DataRow("\r\n", true, false)]
        [DataRow("\r\n", false, true)]
        public void SelectedPhase_PreservesBytesAndRejectsDoubleApplication(string newline, bool bom, bool legacy)
        {
            using var fixture = new Fixture();
            byte[] prefix = bom ? Encoding.UTF8.GetPreamble() : legacy ? new byte[] { 0x81, 0xE9 } : Array.Empty<byte>();
            byte[] before = prefix.Concat(Encoding.ASCII.GetBytes("prefix" + newline + "before" + newline + "suffix" + newline)).ToArray();
            string target = fixture.Header("proof.h", before);
            fixture.Patch("post-midl/proof.h.patch", "um/proof.h");
            fixture.Apply("pre-midl", "post-midl/proof.h.patch", success: true);
            CollectionAssert.AreEqual(before, File.ReadAllBytes(target));
            fixture.Apply("post-midl", "post-midl/proof.h.patch", success: true);
            byte[] expected = prefix.Concat(Encoding.ASCII.GetBytes("prefix" + newline + "after" + newline + "suffix" + newline)).ToArray();
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(target));
            fixture.Apply("post-midl", "post-midl/proof.h.patch", success: false);
            CollectionAssert.AreEqual(expected, File.ReadAllBytes(target));
        }

        [DataTestMethod]
        [DataRow("version")]
        [DataRow("phase")]
        [DataRow("traversal")]
        [DataRow("outside")]
        [DataRow("owner")]
        [DataRow("missing")]
        [DataRow("extra-file")]
        [DataRow("rename")]
        [DataRow("duplicate")]
        [DataRow("mixed-newlines")]
        [DataRow("utf16")]
        [DataRow("post-idl")]
        [DataRow("malformed")]
        public void InvalidSelectionOrTarget_FailsWithoutWriting(string failure)
        {
            using var fixture = new Fixture();
            string target = fixture.Header("proof.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            string selection = "post-midl/proof.h.patch";
            string path = fixture.Patch(selection, "um/proof.h");
            string version = Version;
            switch (failure)
            {
                case "version": version = "10.0.26100.7705"; break;
                case "phase": selection = "unknown/proof.h.patch"; break;
                case "traversal": File.WriteAllText(path, File.ReadAllText(path).Replace("um/proof.h", "um/../proof.h")); break;
                case "outside": File.WriteAllText(path, File.ReadAllText(path).Replace("generation/WinSDK/RecompiledIdlHeaders/", "")); break;
                case "owner": File.WriteAllText(path, File.ReadAllText(path).Replace("um/proof.h", "um/other.h")); break;
                case "missing": File.Delete(target); break;
                case "extra-file": File.AppendAllText(path, File.ReadAllText(path).Substring(File.ReadAllText(path).IndexOf("diff --git", StringComparison.Ordinal))); break;
                case "rename": File.WriteAllText(path, File.ReadAllText(path).Replace("@@ ", "new file mode 100644\n@@ ")); break;
                case "duplicate": selection += "','post-midl/proof.h.patch"; break;
                case "mixed-newlines": File.WriteAllText(target, "prefix\r\nbefore\nsuffix\r\n"); break;
                case "utf16": File.WriteAllText(target, "prefix\nbefore\nsuffix\n", Encoding.Unicode); break;
                case "post-idl": selection = "post-midl/proof.idl.patch"; fixture.Patch(selection, "um/proof.idl"); break;
                case "malformed": File.AppendAllText(path, "@@ invalid hunk\n"); break;
                default: Assert.Fail(failure); break;
            }
            byte[]? before = File.Exists(target) ? File.ReadAllBytes(target) : null;
            fixture.Apply("post-midl", selection, success: false, version);
            if (before != null) CollectionAssert.AreEqual(before, File.ReadAllBytes(target));
            else Assert.IsFalse(File.Exists(target));
        }

        [TestMethod]
        public void FailedHunk_DoesNotPartiallyApplyPhase()
        {
            using var fixture = new Fixture();
            string first = fixture.Header("first.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            string second = fixture.Header("second.h", Encoding.ASCII.GetBytes("prefix\nstale\nsuffix\n"));
            fixture.Patch("post-midl/first.h.patch", "um/first.h");
            fixture.Patch("post-midl/second.h.patch", "um/second.h");
            fixture.Apply("post-midl", "post-midl/first.h.patch','post-midl/second.h.patch", success: false);
            Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(first));
            Assert.AreEqual("prefix\nstale\nsuffix\n", File.ReadAllText(second));
        }

        [DataTestMethod]
        [DataRow("pre-midl")]
        [DataRow("post-midl")]
        public void RegeneratedHeaders_AreRejectedUsingCompleteInputsBeforeAnyWrites(string phase)
        {
            using var fixture = new Fixture();
            string source = Path.Combine(fixture.Path, "sdk");
            Directory.CreateDirectory(Path.Combine(source, "um"));
            foreach (string owner in new[] { "parent", "child" })
            {
                File.WriteAllText(Path.Combine(source, "um", owner + ".idl"),
                    owner == "parent" ? "import \"child.idl\";\n" : "typedef int Child;\n");
                File.WriteAllText(Path.Combine(source, "um", owner + ".h"), "/* File created by MIDL compiler */\n");
            }
            string first = fixture.Header("direct.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            string generated = fixture.Header("child.h", File.ReadAllBytes(Path.Combine(source, "um", "child.h")));
            fixture.Patch(phase + "/direct.h.patch", "um/direct.h");
            fixture.Patch(phase + "/child.h.patch", "um/child.h");
            var result = fixture.Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase {Quote(phase)} " +
                $"-HeaderRoot {Quote(fixture.Headers)} -MidlInputRoot {Quote(source)} -SdkVersion {Quote(Version)} " +
                $"-PatchRoot {Quote(fixture.Patches)} -SelectedPatches @('{phase}/direct.h.patch','{phase}/child.h.patch')");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "authoritative IDL um/child.idl");
            Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(first));
            Assert.AreEqual("/* File created by MIDL compiler */\n", File.ReadAllText(generated));
        }

        [DataTestMethod]
        [DataRow("winsync", false, true)]
        [DataRow("filterpipeline", false, true)]
        [DataRow("wincrypt", true, true)]
        [DataRow("direct", true, false)]
        public void DirectHeaders_RemainAllowedDespiteHistoricalBannerOrExcludedIdl(string owner, bool idl, bool banner)
        {
            using var fixture = new Fixture();
            string before = (banner ? "/* File created by MIDL compiler */" : "prefix") + "\nbefore\nsuffix\n";
            string target = fixture.Header(owner + ".h", Encoding.ASCII.GetBytes(before));
            if (idl) fixture.Header(owner + ".idl", Encoding.ASCII.GetBytes("typedef int Direct;\n"));
            fixture.Patch("post-midl/" + owner + ".h.patch", "um/" + owner + ".h");
            fixture.Apply("post-midl", "post-midl/" + owner + ".h.patch", success: true);
            Assert.AreEqual(before.Replace("\nbefore\n", "\nafter\n"), File.ReadAllText(target));
        }

        [TestMethod]
        public void CapturedOutputMap_RejectsRenamedHeadersWithoutGuessingOwnerBasename()
        {
            using var fixture = new Fixture();
            string target = fixture.Header("renamed.h", Encoding.ASCII.GetBytes("prefix\nbefore\nsuffix\n"));
            fixture.Patch("post-midl/renamed.h.patch", "um/renamed.h");
            var result = fixture.Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase post-midl " +
                $"-HeaderRoot {Quote(fixture.Headers)} -SdkVersion {Quote(Version)} -PatchRoot {Quote(fixture.Patches)} " +
                "-SelectedPatches @('post-midl/renamed.h.patch') " +
                "-MidlOutputs @(@{ Idl='shared/authoritative.idl'; Header='UM/RENAMED.H' })");
            Assert.AreNotEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(result.Output, "shared/authoritative.idl");
            Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(target));
        }

        [TestMethod]
        public void SelectedSdk_ClassificationDistinguishesActualOutputsFromHistoricalBanners()
        {
            using var fixture = new Fixture();
            var result = fixture.Run(
                ". " + Quote(Script("CommonUtils.ps1")) + "; " +
                "$sdk = Get-NugetPropsProperty -name PkgMicrosoft_Windows_SDK_CPP -projectName ClangSharpSourceToWinmdTests.csproj; " +
                "$include = Join-Path $sdk 'c\\include\\10.0.28000.0'; " +
                "if (!(Test-Path -LiteralPath $include)) { throw \"Required SDK inputs missing: $include\" }; " +
                "$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) + " -HeaderRoot $include); " +
                "foreach ($owner in 'objidl', 'objidlbase') { " +
                "if (@($outputs | Where-Object { $_.Header -ieq \"um/$owner.h\" }).Count -ne 1) { throw \"Missing actual output: $owner\" } }; " +
                "foreach ($owner in 'winsync', 'filterpipeline', 'sapi51', 'sapi53', 'imapi', 'IE12Plugin', 'Ole2', 'OleCtl', 'coml2api', 'wincrypt') { " +
                "if (!(Test-Path -LiteralPath (Join-Path $include \"um\\$owner.h\"))) { throw \"Expected direct header missing: $owner\" }; " +
                "if ($outputs.Header -icontains \"um/$owner.h\") { throw \"Incorrectly classified direct header: $owner\" } }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        [TestMethod]
        public void IncludedIdlFact_SurvivesActualMidlInMultipleQueuedOwners()
        {
            using var fixture = new Fixture();
            fixture.Header("child.idl", Encoding.ASCII.GetBytes("cpp_quote(\"// prefix\")\ncpp_quote(\"// before\")\ncpp_quote(\"// suffix\")\n"));
            fixture.Header("imported.idl", Encoding.ASCII.GetBytes("typedef int Imported;\n"));
            foreach (string owner in new[] { "parent", "second" })
            {
                fixture.Header(owner + ".idl", Encoding.ASCII.GetBytes(
                    "import \"imported.idl\";\n#include \"child.idl\"\n" +
                    "[uuid(431fcd36-4281-4fa2-b194-895046acde58)]\ninterface " + owner + " { void Method([in] Imported value); }\n"));
                fixture.Header(owner + ".h", Encoding.ASCII.GetBytes("/* File created by MIDL compiler */\n"));
            }
            string patch = fixture.Patch("pre-midl/child.idl.patch", "um/child.idl");
            File.WriteAllText(patch, File.ReadAllText(patch)
                .Replace("-before\n+after\n suffix\n", "-cpp_quote(\"// before\")\n+cpp_quote(\"// after\")\n cpp_quote(\"// suffix\")\n"));
            fixture.Apply("pre-midl", "pre-midl/child.idl.patch", success: true);
            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            var result = fixture.Run(
                ". " + Quote(Script("CommonUtils.ps1")) + "; " +
                "$sdk = Get-NugetPropsProperty -name PkgMicrosoft_Windows_SDK_CPP -projectName ClangSharpSourceToWinmdTests.csproj; " +
                "$midl = Join-Path $sdk 'c\\bin\\10.0.28000.0\\x86\\midl.exe'; " +
                "if (!(Test-Path -LiteralPath $midl)) { throw \"Required SDK MIDL missing: $midl\" }; " +
                "$env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); " +
                "$inputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) + " -HeaderRoot " + Quote(fixture.Headers) + " -IncludeUnqueued); " +
                "foreach ($input in $inputs) { & " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) +
                " (Join-Path " + Quote(fixture.Headers) + " $input.Idl) (Join-Path " + Quote(converted) + " ([IO.Path]::GetFileName($input.Idl))) }; " +
                "$outputs = @($inputs | Where-Object { $_.Header }); if ($outputs.Count -ne 2) { throw 'Incorrect owner queue' }; " +
                "foreach ($output in $outputs) { & $midl (Join-Path " + Quote(converted) + " ([IO.Path]::GetFileName($output.Idl))) " +
                "/out " + Quote(converted) + " /header (Join-Path " + Quote(fixture.Headers) + " $output.Header) " +
                "/no_warn /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8 /I " + Quote(converted) +
                "; if ($LASTEXITCODE -ne 0) { throw \"MIDL failed: $LASTEXITCODE\" } }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            foreach (string owner in new[] { "parent", "second" })
                StringAssert.Contains(File.ReadAllText(Path.Combine(fixture.Headers, "um", owner + ".h")), "// after");
        }

        [TestMethod]
        public void ReparseTarget_IsRejectedWithoutFollowingLink()
        {
            using var fixture = new Fixture();
            string outside = Path.Combine(fixture.Path, "outside");
            Directory.CreateDirectory(outside);
            string target = Path.Combine(outside, "proof.h");
            File.WriteAllText(target, "prefix\nbefore\nsuffix\n");
            string link = Path.Combine(fixture.Headers, "um");
            var result = fixture.Run($"New-Item -ItemType Junction -Path {Quote(link)} -Target {Quote(outside)} | Out-Null");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            try
            {
                fixture.Patch("post-midl/proof.h.patch", "um/proof.h");
                fixture.Apply("post-midl", "post-midl/proof.h.patch", success: false);
                Assert.AreEqual("prefix\nbefore\nsuffix\n", File.ReadAllText(target));
            }
            finally { Directory.Delete(link); }
        }

        [TestMethod]
        public void ShippedProofs_OnlyAddCommentsAndPreMidlSurvivesConversion()
        {
            using var fixture = new Fixture();
            string sdk = Path.Combine(Root(), "generation", "WinSDK");
            foreach (string owner in new[] { "amstream.idl", "Uxtheme.h" })
            {
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(sdk, "RecompiledIdlHeaders", "um", owner)));
            }
            string proofs = Path.Combine(Root(), "tests", "ClangSharpSourceToWinmdTests", "SdkPatchFixtures");
            string selection = "pre-midl/amstream.idl.patch','post-midl/Uxtheme.h.patch";
            fixture.Apply("pre-midl", selection, true, patchRoot: proofs);
            string idl = Path.Combine(fixture.Headers, "um", "amstream.idl");
            string converted = Path.Combine(fixture.Path, "converted.idl");
            var result = fixture.Run($"& {Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1"))} {Quote(idl)} {Quote(converted)}");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            StringAssert.Contains(File.ReadAllText(converted), "cpp_quote(\"// [win32metadata sdk patch] Metadata marker for OUTPUT_STATE enum\")");
            fixture.Apply("post-midl", selection, true, patchRoot: proofs);
            foreach (string owner in new[] { "amstream.idl", "Uxtheme.h" })
            {
                string original = File.ReadAllText(Path.Combine(sdk, "RecompiledIdlHeaders", "um", owner)).Replace("\r\n", "\n");
                string[] lines = File.ReadAllLines(Path.Combine(fixture.Headers, "um", owner));
                string[] added = lines.Where(line => line.Contains("[win32metadata sdk patch]", StringComparison.Ordinal)).ToArray();
                Assert.AreEqual(1, added.Length);
                Assert.IsTrue(added[0].StartsWith("//", StringComparison.Ordinal) ||
                    added[0].StartsWith("cpp_quote(\"//", StringComparison.Ordinal));
                Assert.AreEqual(original, string.Join("\n", lines.Where(line => !line.Contains("[win32metadata sdk patch]", StringComparison.Ordinal))) + "\n");
            }
        }

        [TestMethod]
        public void ParameterPilot_AllSelectedOwnersReplayByteForByte()
        {
            using var fixture = new Fixture();
            var owners = new[]
            {
                ("post-midl/winhttp.h.patch", "um", "winhttp.h"),
                ("post-midl/combaseapi.h.patch", "um", "combaseapi.h"),
                ("post-midl/objbase.h.patch", "um", "objbase.h"),
                ("post-midl/fileapi.h.patch", "um", "fileapi.h"),
                ("post-midl/errhandlingapi.h.patch", "um", "errhandlingapi.h"),
                ("post-midl/handleapi.h.patch", "um", "handleapi.h"),
                ("post-midl/HtmlHelp.h.patch", "um", "HtmlHelp.h"),
                ("post-midl/webauthnplugin.h.patch", "um", "webauthnplugin.h"),
                ("pre-midl/ObjIdl.Idl.patch", "um", "ObjIdl.Idl"),
                ("pre-midl/ObjIdlbase.Idl.patch", "um", "ObjIdlbase.Idl"),
                ("pre-midl/dxgi.idl.patch", "shared", "dxgi.idl"),
            };
            string inputRoot = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string copy = "$include = " + Quote(inputRoot) + "; " +
                "$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) + " -HeaderRoot $include); " +
                "foreach ($owner in @(@{ Idl='um/ObjIdl.Idl'; Header='um/ObjIdl.h' }, " +
                "@{ Idl='um/ObjIdlbase.Idl'; Header='um/ObjIdlbase.h' }, " +
                "@{ Idl='shared/dxgi.idl'; Header='shared/dxgi.h' })) { " +
                "if (@($outputs | Where-Object { $_.Idl -ieq $owner.Idl -and $_.Header -ieq $owner.Header }).Count -ne 1) " +
                "{ throw \"Missing generated pilot owner: $($owner.Idl)\" } }; " +
                "foreach ($header in 'um/winhttp.h', 'um/combaseapi.h', 'um/objbase.h', 'um/fileapi.h') { " +
                "if ($outputs.Header -icontains $header) { throw \"Pilot direct header is regenerated: $header\" } }; ";
            foreach (var owner in owners)
                copy += "$destination = " + Quote(System.IO.Path.Combine(fixture.Headers, owner.Item2, owner.Item3)) +
                    "; New-Item -ItemType Directory -Force (Split-Path $destination) | Out-Null; " +
                    "Copy-Item -LiteralPath (Join-Path $include " +
                    Quote(owner.Item2 + "\\" + owner.Item3) + ") -Destination $destination; ";
            var copied = fixture.Run(copy);
            Assert.AreEqual(0, copied.ExitCode, copied.Output);
            string selections = string.Join("','", owners.Select(owner => owner.Item1));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: inputRoot);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: inputRoot);
            foreach (var owner in owners)
                CollectionAssert.AreEqual(File.ReadAllBytes(System.IO.Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", owner.Item2, owner.Item3)),
                    File.ReadAllBytes(System.IO.Path.Combine(fixture.Headers, owner.Item2, owner.Item3)), owner.Item1);
            StringAssert.Contains(File.ReadAllText(Script("RecompileIdlFilesForScraping.ps1")),
                "$SelectedPatches = Get-SdkHeaderPatchSelections");
        }

        [DataTestMethod]
        [DataRow("ObjIdl", false)]
        [DataRow("ObjIdlbase", false)]
        [DataRow("ObjIdlbase", true)]
        public void ExplicitAvailability_IdlSourceReplaysOnRealSelectedSdkOutput(string owner, bool authoredResult)
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = System.IO.Path.Combine(sdk, "c", "include", "10.0.28000.0");
            fixture.Header(owner + ".Idl", File.ReadAllBytes(System.IO.Path.Combine(include, "um", owner + ".Idl")));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("pre-midl", "pre-midl/" + owner + ".Idl.patch", true, patchRoot: patches, midlInputRoot: include);
            if (owner == "ObjIdl")
            {
                fixture.Header("ObjIdlbase.Idl", File.ReadAllBytes(System.IO.Path.Combine(include, "um", "ObjIdlbase.Idl")));
                fixture.Apply("pre-midl", "pre-midl/ObjIdlbase.Idl.patch", true, patchRoot: patches, midlInputRoot: include);
            }
            if (authoredResult)
            {
                string ownerPath = System.IO.Path.Combine(fixture.Headers, "um", owner + ".Idl");
                string original = File.ReadAllText(ownerPath);
                string annotated = original.Replace("[local]\r\n    HRESULT Read(",
                    "[local, annotation(\"_Win32_PreserveResult_\")]\r\n    HRESULT Read(");
                Assert.AreNotEqual(original, annotated, "Author only the real ISequentialStream::Read fixture.");
                File.WriteAllText(ownerPath, annotated);
            }
            string converted = System.IO.Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string idl = System.IO.Path.Combine(converted, owner.ToLowerInvariant() + ".idl");
            string header = System.IO.Path.Combine(fixture.Headers, "um", owner.ToLowerInvariant() + ".h");
            if (owner == "ObjIdl")
            {
                var conversion = fixture.Run("& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(System.IO.Path.Combine(fixture.Headers, "um", "ObjIdlbase.Idl")) + " " +
                    Quote(System.IO.Path.Combine(converted, "objidlbase.idl")));
                Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            }
            foreach (string dependency in new[] { "unknwnbase.idl", "wtypesbase.idl", "unknwn.idl", "wtypes.idl" })
            {
                var conversion = fixture.Run("& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(System.IO.Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency)) + " " +
                    Quote(System.IO.Path.Combine(converted, dependency)));
                Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            }
            var result = fixture.Run("& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                Quote(System.IO.Path.Combine(fixture.Headers, "um", owner + ".Idl")) + " " + Quote(idl) + "; " +
                ". " + Quote(Script("CommonUtils.ps1")) + "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); " +
                "& " + Quote(System.IO.Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(idl) + " /out " + Quote(converted) + " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(System.IO.Path.Combine(include, "um")) +
                " /I " + Quote(System.IO.Path.Combine(include, "shared")) +
                " /I " + Quote(System.IO.Path.Combine(include, "winrt")) +
                " /I " + Quote(System.IO.Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            Assert.IsFalse(File.Exists(System.IO.Path.Combine(patches, "post-midl", owner + ".h.patch")));
            if (authoredResult)
            {
                string generated = File.ReadAllText(header);
                int marker = generated.IndexOf("_Win32_PreserveResult_", StringComparison.Ordinal);
                Assert.IsTrue(marker >= 0, "MIDL must preserve the authored result marker.");
                string method = generated.Substring(generated.LastIndexOf("virtual", marker, StringComparison.Ordinal));
                method = method.Substring(0, method.IndexOf(" = 0;", StringComparison.Ordinal) + " = 0;".Length);
                using var resultFixture = new LastErrorHeaderTests.Fixture();
                var scraped = resultFixture.Scrape("typedef long HRESULT; typedef unsigned long ULONG;\n" +
                    "#define STDMETHODCALLTYPE __stdcall\n" +
                    "struct __declspec(uuid(\"0c733a30-2a1c-11ce-ade5-00aa0044773d\")) ISequentialStream {\n" +
                    method + "\n};", sal: true);
                Assert.AreEqual(0, scraped.ExitCode, method + "\n" + scraped.Output);
                ResultHeaderTests.AssertResult(resultFixture.Emit(), "Read", true);
                return;
            }
            string expectedHeader = System.IO.Path.Combine(Root(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "um", owner + ".h");
            AssertRegeneratedHeader(owner + ".h", header);
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                SupportedOSHeaderTests.AssertMidlAvailability(File.ReadAllText(header), File.ReadAllText(expectedHeader), target);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ExplicitAvailability_SelectedSdkNativeBuildIsANoOp(string arch, string language, bool urlmonFirst)
        {
            using var fixture = new Fixture();
            string include = System.IO.Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            foreach (string owner in new[] { "winhttp.h", "errhandlingapi.h", "handleapi.h", "combaseapi.h", "objbase.h",
                "HtmlHelp.h", "webauthnplugin.h", "coml2api.h", "Ole2.h", "OleCtl.h" })
                fixture.Header(owner, File.ReadAllBytes(System.IO.Path.Combine(include, "um", owner)));
            string patches = System.IO.Path.Combine(Root(), "generation", "WinSDK", "patches");
            fixture.Apply("post-midl", string.Join("','", new[] { "winhttp.h", "errhandlingapi.h", "handleapi.h",
                "combaseapi.h", "objbase.h", "HtmlHelp.h", "webauthnplugin.h", "coml2api.h", "Ole2.h", "OleCtl.h" }
                .Select(owner => "post-midl/" + owner + ".patch")), true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "ObjIdl.h", "ObjIdlbase.h", "Unknwnbase.h", "Unknwn.h", "servprov.h", "ocidl.h", "urlmon.h" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)));
            File.Copy(System.IO.Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders",
                "shared", "win32metadata_annotations.h"), System.IO.Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            File.Copy(System.IO.Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"),
                System.IO.Path.Combine(fixture.Headers, "sal.h"));
            string source = System.IO.Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" + (urlmonFirst ? "#include <urlmon.h>\n" : "") +
                "#include <windows.h>\n#include <winhttp.h>\n#include <objbase.h>\n#include <ObjIdlbase.h>\n" +
                "#include <ole2.h>\n#include <olectl.h>\n#include <coml2api.h>\n#include <servprov.h>\n" +
                "#include <HtmlHelp.h>\n#include <webauthn.h>\n#include <webauthnplugin.h>\n#include <urlmon.h>\n" +
                "HWND (WINAPI *html_signature)(HWND, LPCWSTR, UINT, DWORD_PTR) = &HtmlHelpW;\n" +
                "HRESULT (WINAPI *plugin_signature)(REFCLSID, AUTHENTICATOR_STATE*) = &WebAuthNPluginGetAuthenticatorState;\n" +
                "DWORD (WINAPI *error_signature)(void) = &GetLastError;\n" +
                "BOOL (WINAPI *winhttp_signature)(void) = &WinHttpCheckPlatform;\n" +
                "DWORD (WINAPI *com_signature)(void) = &CoGetCurrentProcess;\n" +
                "HRESULT (WINAPI *storage_create)(const WCHAR*, DWORD, DWORD, DWORD, STGOPTIONS*, PSECURITY_DESCRIPTOR, REFIID, void**) = &StgCreateStorageEx;\n" +
                "HRESULT (WINAPI *storage_open)(const WCHAR*, DWORD, DWORD, DWORD, STGOPTIONS*, PSECURITY_DESCRIPTOR, REFIID, void**) = &StgOpenStorageEx;\n" +
                "HRESULT (WINAPI *ole_default)(REFCLSID, LPUNKNOWN, REFIID, LPVOID*) = &OleCreateDefaultHandler;\n" +
                "HRESULT (WINAPI *ole_embedding)(REFCLSID, LPUNKNOWN, DWORD, LPCLASSFACTORY, REFIID, LPVOID*) = &OleCreateEmbeddingHelper;\n" +
                "HRESULT (WINAPI *ole_stream)(LPSTREAM, REFIID, LPVOID*) = &OleLoadFromStream;\n" +
                "HRESULT (WINAPI *ole_font)(LPFONTDESC, REFIID, LPVOID*) = &OleCreateFontIndirect;\n" +
                "HRESULT (WINAPI *ole_picture)(LPPICTDESC, REFIID, BOOL, LPVOID*) = &OleCreatePictureIndirect;\n" +
                "HRESULT (WINAPI *ole_load)(LPSTREAM, LONG, BOOL, REFIID, LPVOID*) = &OleLoadPicture;\n" +
                "HRESULT (WINAPI *ole_path)(LPOLESTR, LPUNKNOWN, DWORD, OLE_COLOR, REFIID, LPVOID*) = &OleLoadPicturePath;\n" +
                "HRESULT (STDMETHODCALLTYPE *unknown_proxy)(IUnknown*, REFIID, void**) = &IUnknown_QueryInterface_Proxy;\n" +
                "HRESULT (WINAPI *url_activation)(REFCLSID, LPCWSTR, DWORD, DWORD, LPCWSTR, LPBINDCTX, DWORD, LPVOID, REFIID, LPVOID*) = &CoGetClassObjectFromURL;\n" +
                "HRESULT __stdcall BindHostCall(IBindHost* host, IMoniker* moniker, IBindCtx* context, IBindStatusCallback* callback, REFIID riid, void** output) {\n" +
                "#ifdef __cplusplus\nreturn host->MonikerBindToObject(moniker, context, callback, riid, output) + host->MonikerBindToStorage(moniker, context, callback, riid, output);\n" +
                "#else\nreturn IBindHost_MonikerBindToObject(host, moniker, context, callback, riid, output) + IBindHost_MonikerBindToStorage(host, moniker, context, callback, riid, output);\n" +
                "#endif\n}\n" +
                "HRESULT __stdcall FactoryCall(IClassFactory2* factory, IServiceProvider* provider, REFIID riid, void** output) {\n" +
                "#ifdef __cplusplus\nreturn factory->CreateInstanceLic(0, 0, riid, 0, output) + provider->QueryService(riid, riid, output);\n" +
                "#else\nreturn IClassFactory2_CreateInstanceLic(factory, 0, 0, riid, 0, output) + IServiceProvider_QueryService(provider, riid, riid, output);\n" +
                "#endif\n}\n");
            string obj = System.IO.Path.Combine(fixture.Path, "native.obj");
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { & $compiler /nologo /c " + language + " " + Quote(source) +
                " " + Quote("/Fo" + obj) + " $(if ($mode -ne 'baseline') { " +
                Quote("/I" + System.IO.Path.Combine(fixture.Headers, "um")) + "; " + Quote("/I" + fixture.Headers) +
                " }) \"/I$vcInclude\" " + Quote("/I" + System.IO.Path.Combine(include, "um")) +
                " " + Quote("/I" + System.IO.Path.Combine(include, "shared")) +
                " " + Quote("/I" + System.IO.Path.Combine(include, "ucrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[] ordinary = File.ReadAllBytes(System.IO.Path.Combine(fixture.Path, "ordinary.obj"));
            byte[] metadata = File.ReadAllBytes(System.IO.Path.Combine(fixture.Path, "metadata.obj"));
            byte[] baseline = File.ReadAllBytes(System.IO.Path.Combine(fixture.Path, "baseline.obj"));
            Array.Clear(baseline, 4, 4);
            Array.Clear(ordinary, 4, 4);
            Array.Clear(metadata, 4, 4);
            CollectionAssert.AreEqual(baseline, ordinary);
            CollectionAssert.AreEqual(ordinary, metadata);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ShellItemOutput_NativeBuildIsANoOp(string arch, string language, bool shellFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string parent = ShellItemComOutputHeaderTests.PreRetirementCommit(Root());
            foreach (string owner in new[] { "ShObjIdl.h", "ShObjIdl_core.h", "ShlObj.h", "ShlObj_core.h" })
            {
                var original = LastErrorHeaderTests.Run("git", "-C", Root(), "cat-file", "--filters",
                    parent + ":generation/WinSDK/RecompiledIdlHeaders/um/" + owner);
                Assert.AreEqual(0, original.ExitCode, original.Output);
                Assert.IsTrue(original.Output.All(character => character <= 127), owner);
                fixture.Header(owner, Encoding.ASCII.GetBytes(original.Output));
            }
            foreach (string owner in new[] { "propsys.h", "shellapi.h", "Shlwapi.h" })
            {
                string original = PropertyComOutputHeaderTests.ReadOriginal("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                fixture.Header(owner, Encoding.ASCII.GetBytes(original));
            }
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (shellFirst ? "#include <shlobj.h>\n#include <urlmon.h>\n" : "#include <windows.h>\n#include <urlmon.h>\n") +
                "#include <shobjidl.h>\n#include <shlobj.h>\n#include <propsys.h>\n#include <shellapi.h>\n#include <shlwapi.h>\n" +
                "HRESULT (WINAPI *item_idlist)(PCIDLIST_ABSOLUTE, REFIID, void**) = &SHCreateItemFromIDList;\n" +
                "HRESULT (WINAPI *item_name)(PCWSTR, IBindCtx*, REFIID, void**) = &SHCreateItemFromParsingName;\n" +
                "HRESULT (WINAPI *item_parent)(PCIDLIST_ABSOLUTE, IShellFolder*, PCUITEMID_CHILD, REFIID, void**) = &SHCreateItemWithParent;\n" +
                "HRESULT (WINAPI *item_relative)(IShellItem*, PCWSTR, IBindCtx*, REFIID, void**) = &SHCreateItemFromRelativeName;\n" +
                "HRESULT (WINAPI *item_known)(REFKNOWNFOLDERID, DWORD, PCWSTR, REFIID, void**) = &SHCreateItemInKnownFolder;\n" +
                "HRESULT (WINAPI *item_object)(IUnknown*, REFIID, void**) = &SHGetItemFromObject;\n" +
                "HRESULT (WINAPI *item_data)(IDataObject*, DATAOBJ_GET_ITEM_FLAGS, REFIID, void**) = &SHGetItemFromDataObject;\n" +
                "HRESULT (WINAPI *array_data)(IDataObject*, REFIID, void**) = &SHCreateShellItemArrayFromDataObject;\n" +
                "HRESULT (WINAPI *array_item)(IShellItem*, REFIID, void**) = &SHCreateShellItemArrayFromShellItem;\n" +
                "HRESULT (WINAPI *known_folder)(REFKNOWNFOLDERID, KNOWN_FOLDER_FLAG, HANDLE, REFIID, void**) = &SHGetKnownFolderItem;\n" +
                "HRESULT (WINAPI *bind_parent)(PCIDLIST_ABSOLUTE, REFIID, void**, PCUITEMID_CHILD*) = &SHBindToParent;\n" +
                "HRESULT (WINAPI *bind_folder)(IShellFolder*, PCUIDLIST_RELATIVE, REFIID, void**, PCUITEMID_CHILD*) = &SHBindToFolderIDListParent;\n" +
                "HRESULT (WINAPI *bind_folder_ex)(IShellFolder*, PCUIDLIST_RELATIVE, IBindCtx*, REFIID, void**, PCUITEMID_CHILD*) = &SHBindToFolderIDListParentEx;\n" +
                "HRESULT (WINAPI *bind_object)(IShellFolder*, PCUIDLIST_RELATIVE, IBindCtx*, REFIID, void**) = &SHBindToObject;\n" +
                "HRESULT (WINAPI *ps_memory)(REFIID, void**) = &PSCreateMemoryPropertyStore;\n" +
                "HRESULT (WINAPI *ps_delayed)(GETPROPERTYSTOREFLAGS, IDelayedPropertyStoreFactory*, const DWORD*, DWORD, REFIID, void**) = &PSCreateDelayedMultiplexPropertyStore;\n" +
                "HRESULT (WINAPI *ps_multiplex)(IUnknown**, DWORD, REFIID, void**) = &PSCreateMultiplexPropertyStore;\n" +
                "HRESULT (WINAPI *ps_changes)(const PROPERTYKEY*, const PKA_FLAGS*, const PROPVARIANT*, UINT, REFIID, void**) = &PSCreatePropertyChangeArray;\n" +
                "HRESULT (WINAPI *ps_change)(PKA_FLAGS, REFPROPERTYKEY, REFPROPVARIANT, REFIID, void**) = &PSCreateSimplePropertyChange;\n" +
                "HRESULT (WINAPI *ps_description)(REFPROPERTYKEY, REFIID, void**) = &PSGetPropertyDescription;\n" +
                "HRESULT (WINAPI *ps_description_name)(LPCWSTR, REFIID, void**) = &PSGetPropertyDescriptionByName;\n" +
                "HRESULT (WINAPI *ps_handler)(IUnknown*, BOOL, REFIID, void**) = &PSGetItemPropertyHandler;\n" +
                "HRESULT (WINAPI *ps_handler_create)(IUnknown*, BOOL, IUnknown*, REFIID, void**) = &PSGetItemPropertyHandlerWithCreateObject;\n" +
                "HRESULT (WINAPI *ps_enumerate)(PROPDESC_ENUMFILTER, REFIID, void**) = &PSEnumeratePropertyDescriptions;\n" +
                "HRESULT (WINAPI *ps_description_list)(LPCWSTR, REFIID, void**) = &PSGetPropertyDescriptionListFromString;\n" +
                "HRESULT (WINAPI *ps_storage)(IPropertySetStorage*, DWORD, REFIID, void**) = &PSCreatePropertyStoreFromPropertySetStorage;\n" +
                "HRESULT (WINAPI *ps_object)(IUnknown*, DWORD, REFIID, void**) = &PSCreatePropertyStoreFromObject;\n" +
                "HRESULT (WINAPI *ps_adapter)(IPropertyStore*, REFIID, void**) = &PSCreateAdapterFromPropertyStore;\n" +
                "HRESULT (WINAPI *ps_system)(REFIID, void**) = &PSGetPropertySystem;\n" +
                "HRESULT (WINAPI *ps_bag)(IPropertyBag*, LPCWSTR, REFIID, void**) = &PSPropertyBag_ReadUnknown;\n" +
                "HRESULT (WINAPI *ps_idlist)(PCIDLIST_ABSOLUTE, GETPROPERTYSTOREFLAGS, REFIID, void**) = &SHGetPropertyStoreFromIDList;\n" +
                "HRESULT (WINAPI *ps_path)(PCWSTR, IBindCtx*, GETPROPERTYSTOREFLAGS, REFIID, void**) = &SHGetPropertyStoreFromParsingName;\n" +
                "HRESULT (WINAPI *ps_window)(HWND, REFIID, void**) = &SHGetPropertyStoreForWindow;\n" +
                "HRESULT (WINAPI *ps_view)(PCIDLIST_ABSOLUTE, PCWSTR, DWORD, REFIID, void**) = &SHGetViewStatePropertyBag;\n" +
                "HRESULT __stdcall TransferCall(ITransferSource* source, ITransferDestination* destination, IShellItem* item, REFIID riid, void** output, void** resources) {\n" +
                "#ifdef __cplusplus\nreturn source->OpenItem(item, 0, riid, output) + destination->CreateItem(0, 0, 0, 0, riid, output, riid, resources);\n" +
                "#else\nreturn ITransferSource_OpenItem(source, item, 0, riid, output) + ITransferDestination_CreateItem(destination, 0, 0, 0, 0, riid, output, riid, resources);\n" +
                "#endif\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            // Use identical include paths for old/new headers; MSVC orders UUID COMDATs by header identity.
            string replacements = string.Join("; ", new[] { "ShObjIdl_core.h", "ShlObj_core.h",
                "propsys.h", "shellapi.h", "Shlwapi.h" }.Select(owner =>
                "Copy-Item -LiteralPath " + Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [TestMethod]
        public void ShellItemOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShObjIdl.idl", "ShlObj_core.h", "ShlObj.h",
                "propsys.idl", "shellapi.h", "Shlwapi.h", "shdeprecated.h" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(include, "um", owner)));
            const string selections = "pre-midl/ShObjIdl_core.idl.patch','post-midl/ShlObj_core.h.patch'," +
                "'pre-midl/propsys.idl.patch','post-midl/shellapi.h.patch','post-midl/Shlwapi.h.patch'," +
                "'pre-midl/ShObjIdl.idl.patch','post-midl/ShlObj.h.patch','post-midl/shdeprecated.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "ShObjIdl_core.idl", "ShObjIdl.idl", "ShlObj_core.h", "ShlObj.h",
                "propsys.idl", "shellapi.h", "Shlwapi.h", "shdeprecated.h" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl",
                "msxml.idl", "propidlbase.idl", "propidl.idl", "StructuredQueryCondition.idl", "shtypes.idl",
                "comcat.idl", "propsys.idl", "ObjectArray.idl", "shobjidl_core.idl", "docobj.idl", "prsht.idl", "shobjidl.idl" })
            {
                string input = dependency == "shobjidl_core.idl" || dependency == "shobjidl.idl" || dependency == "propsys.idl" ? Path.Combine(fixture.Headers, "um", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            foreach (string owner in new[] { "propsys", "shobjidl_core", "shobjidl" })
            {
                string header = Path.Combine(fixture.Headers, "um", owner + ".h");
                command += "& " +
                Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, owner + ".idl")) + " /out " + Quote(converted) +
                " /header " + Quote(header) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            }
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertRegeneratedHeaders(new[] { "propsys.h", "ShObjIdl_core.h", "ShObjIdl.h" }
                .Select(owner => (owner, Path.Combine(fixture.Headers, "um", owner))));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
            {
                ShellItemComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
                PropertyComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
                ShellComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
            }
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ShellOutput_NativeBuildIsANoOp(string arch, string language, bool shellFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = ShellComOutputHeaderTests.Owners.Select(owner => owner.Owner).Append("propsys.h").ToArray();
            foreach (string owner in owners)
            {
                string original = ShellComOutputHeaderTests.ReadOriginal("RecompiledIdlHeaders/um/" + owner, checkoutBytes: true);
                Assert.IsTrue(original.All(character => character <= 127), owner);
                fixture.Header(owner, Encoding.ASCII.GetBytes(original));
            }
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (shellFirst ? "#include <shlobj.h>\n#include <urlmon.h>\n" : "#include <windows.h>\n#include <urlmon.h>\n") +
                "#include <shobjidl.h>\n#include <shlobj.h>\n#include <propsys.h>\n#include <shellapi.h>\n#include <shlwapi.h>\n#include <shdeprecated.h>\n" +
                "HRESULT (WINAPI *association)(REFIID, void**) = &SHCreateAssociationRegistration;\n" +
                "HRESULT (WINAPI *icon)(REFIID, void**) = &SHCreateDefaultExtractIcon;\n" +
                "HRESULT (WINAPI *handlers)(PCWSTR, REFIID, void**) = &SHAssocEnumHandlersForProtocolByApplication;\n" +
                "HRESULT (WINAPI *activate)(PCWSTR, const CLSID*, IUnknown*, REFIID, void**) = &SHCoCreateInstance;\n" +
                "HRESULT (WINAPI *data)(PCIDLIST_ABSOLUTE, UINT, PCUITEMID_CHILD_ARRAY, IDataObject*, REFIID, void**) = &SHCreateDataObject;\n" +
                "HRESULT (WINAPI *context)(const DEFCONTEXTMENU*, REFIID, void**) = &SHCreateDefaultContextMenu;\n" +
                "HRESULT (WINAPI *file_icon)(LPCWSTR, DWORD, REFIID, void**) = &SHCreateFileExtractIconW;\n" +
                "HRESULT (WINAPI *classes)(const ASSOCIATIONELEMENT*, ULONG, REFIID, void**) = &AssocCreateForClasses;\n" +
                "HRESULT (WINAPI *images)(int, REFIID, void**) = &SHGetImageList;\n" +
                "HRESULT (WINAPI *assoc)(CLSID, REFIID, void**) = &AssocCreate;\n" +
                "HRESULT __stdcall ShellCalls(INameSpaceTreeControlEvents* tree, IShellMenu* menu, IQueryAssociations* assoc,\n" +
                "IDockingWindowFrame* dock, IBrowserService2* browser, IBrowserService3* browser3, IBrowserService4* browser4,\n" +
                "ITrackShellMenu* track, IShellItem* item, IContextMenu* context, REFIID riid, void** output, DWORD* flags, PIDLIST_ABSOLUTE* pidl) {\n" +
                "#ifdef __cplusplus\n" +
                "return tree->OnBeforeContextMenu(item, riid, output) + tree->OnAfterContextMenu(item, context, riid, output) +\n" +
                "menu->GetShellFolder(flags, pidl, riid, output) + assoc->GetEnum(0, ASSOCENUM_NONE, 0, riid, output) +\n" +
                "dock->FindToolbar(0, riid, output) + browser->CreateBrowserPropSheetExt(riid, output) +\n" +
                "browser3->CreateBrowserPropSheetExt(riid, output) + browser4->CreateBrowserPropSheetExt(riid, output) + track->GetShellFolder(flags, pidl, riid, output);\n" +
                "#else\n" +
                "return INameSpaceTreeControlEvents_OnBeforeContextMenu(tree, item, riid, output) + INameSpaceTreeControlEvents_OnAfterContextMenu(tree, item, context, riid, output) +\n" +
                "IShellMenu_GetShellFolder(menu, flags, pidl, riid, output) + assoc->lpVtbl->GetEnum(assoc, 0, ASSOCENUM_NONE, 0, riid, output) +\n" +
                "dock->lpVtbl->FindToolbar(dock, 0, riid, output) + IBrowserService2_CreateBrowserPropSheetExt(browser, riid, output) +\n" +
                "IBrowserService3_CreateBrowserPropSheetExt(browser3, riid, output) + IBrowserService4_CreateBrowserPropSheetExt(browser4, riid, output) + ITrackShellMenu_GetShellFolder(track, flags, pidl, riid, output);\n" +
                "#endif\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(owner =>
                "Copy-Item -LiteralPath " + Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [TestMethod]
        public void CoreComOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            foreach (string owner in new[] { "coml2api.h", "Ole2.h", "OleCtl.h",
                "ocidl.Idl", "ServProv.Idl", "Unknwnbase.Idl", "UrlMon.Idl" })
                fixture.Header(owner, File.ReadAllBytes(Path.Combine(include, "um", owner)));
            string selections = "post-midl/coml2api.h.patch','post-midl/Ole2.h.patch','post-midl/OleCtl.h.patch'," +
                "'pre-midl/ocidl.Idl.patch','pre-midl/ServProv.Idl.patch','pre-midl/Unknwnbase.Idl.patch','pre-midl/UrlMon.Idl.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (string owner in new[] { "coml2api.h", "Ole2.h", "OleCtl.h",
                "ocidl.Idl", "ServProv.Idl", "Unknwnbase.Idl", "UrlMon.Idl" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)),
                    File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(converted, "win32metadata_annotations.h"));
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl" })
            {
                string patched = Path.Combine(fixture.Headers, "um", dependency);
                string input = File.Exists(patched) ? patched :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            foreach (string owner in new[] { "unknwnbase", "unknwn", "servprov", "urlmon", "ocidl" })
                command += "& " + Quote(Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, owner + ".idl")) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", owner + ".h")) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                    " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertRegeneratedHeaders(new[] { "Unknwnbase.h", "Unknwn.h", "servprov.h", "urlmon.h", "ocidl.h" }
                .Select(owner => (owner, Path.Combine(fixture.Headers, "um", owner))));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
            {
                ComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
                UrlMonComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
            }
        }

        [TestMethod]
        public void DirectXOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string d3d = Direct3DPackagePath();
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string midlInputs = Path.Combine(fixture.Path, "sdk");
            foreach (string folder in new[] { "um", "shared" })
            {
                string destination = Path.Combine(midlInputs, folder);
                Directory.CreateDirectory(destination);
                foreach (string file in Directory.GetFiles(Path.Combine(include, folder)))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            foreach (string file in Directory.GetFiles(Path.Combine(d3d, "build", "native", "include")))
                File.Copy(file, Path.Combine(midlInputs, Path.GetFileNameWithoutExtension(file) == "dxgiformat" ? "shared" : "um",
                    Path.GetFileName(file)), overwrite: true);
            string[] inputs = DirectXComOutputHeaderTests.Owners.Select(owner => owner.Path)
                .Where(path => path != "um/d3d10.h" && path != "um/d3d12.h" && path != "um/Presentation.h")
                .Concat(new[] { "um/d3d10.idl", "um/d3d12.idl", "um/Presentation.idl" }).ToArray();
            foreach (string path in inputs)
            {
                string source = Path.Combine(path.StartsWith("winrt/", StringComparison.Ordinal) ? include : midlInputs, path);
                byte[] original = File.ReadAllBytes(source);
                CollectionAssert.AreEqual(Encoding.ASCII.GetBytes(DirectXComOutputHeaderTests.ReadOriginal(
                    "RecompiledIdlHeaders/" + path, checkoutBytes: true)), original,
                    "The reachable introduction parent must match the actual shipped native owner: " + path);
                fixture.Header(Path.GetFileName(path), original, Path.GetDirectoryName(path)!);
            }
            string selections = "pre-midl/d3d10.idl.patch','pre-midl/d3d12.idl.patch','pre-midl/Presentation.idl.patch'," +
                "'post-midl/d2d1.h.patch','post-midl/d3dcompiler.h.patch','post-midl/d3d9on12.h.patch'," +
                "'post-midl/d3d9.h.patch','post-midl/dcomp.h.patch','post-midl/dxcapi.h.patch'," +
                "'post-midl/windows.ui.composition.interop.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            foreach (string path in inputs)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", path)), File.ReadAllBytes(Path.Combine(fixture.Headers, path)), path);
            foreach (string path in new[] { "um/d2d1.idl", "shared/d3d9.idl", "um/d3d9on12.idl", "um/d3dcompiler.idl",
                "um/dcomp.idl", "um/dxcapi.idl", "um/dwrite.idl", "winrt/windows.ui.composition.interop.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(path.StartsWith("winrt/", StringComparison.Ordinal) ? include : midlInputs, path)),
                    "Post-MIDL only for true header-only owners: " + path);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "oleidl.idl", "servprov.idl", "urlmon.idl", "ocidl.idl",
                "dxgicommon.idl", "dxgiformat.idl", "dxgitype.idl", "dxgi.idl", "dxgi1_2.idl", "dxgi1_3.idl",
                "dxgi1_4.idl", "dxgi1_5.idl", "dxgi1_6.idl", "d3dcommon.idl", "d3d10.idl", "d3d10_1.idl",
                "PresentationTypes.idl", "Presentation.idl", "d3d12.idl" })
            {
                string patched = Path.Combine(fixture.Headers, "um", dependency);
                string input = File.Exists(patched) ? patched : Path.Combine(midlInputs,
                    dependency.StartsWith("wtypes", StringComparison.Ordinal) || dependency.StartsWith("dxgi", StringComparison.Ordinal) ?
                        "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            command += ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            command += "$graphicsOutputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(midlInputs) + "); ";
            foreach (string owner in new[] { "d3d10", "d3d10_1", "d3d12", "Presentation" })
            {
                command += "$outputs = @($graphicsOutputs | Where-Object { $_.Idl -ieq " + Quote("um/" + owner + ".idl") +
                    " }); if ($outputs.Count -ne 1 -or $outputs[0].Header -ine " + Quote("um/" + owner + ".h") +
                    ") { throw 'Selected graphics owner is not queued by the production MIDL input inventory.' }; ";
                command += "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, owner.ToLowerInvariant() + ".idl")) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", owner.ToLowerInvariant() + ".h")) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) +
                    " /I " + Quote(Path.Combine(midlInputs, "um")) + " /I " + Quote(Path.Combine(midlInputs, "shared")) +
                    " /I " + Quote(Path.Combine(include, "winrt")) + " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            }
            command += "$header = " + Quote(Path.Combine(fixture.Headers, "um", "d3d10_1.h")) +
                "; (Get-Content $header -Raw).Replace('#include \"d3d10.h\"' + \"`r`n\", '') | Set-Content $header";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertRegeneratedHeaders(new[] { "d3d10.h", "d3d10_1.h", "d3d12.h", "Presentation.h" }
                .Select(owner => (owner, Path.Combine(fixture.Headers, "um", owner))));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                DirectXComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void DirectXOutput_NativeBuildIsANoOp(string arch, string language, bool graphicsFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = DirectXComOutputHeaderTests.Owners.Select(owner => owner.Path).Append("um/d3d10_1.h").ToArray();
            foreach (string path in owners)
                fixture.Header(Path.GetFileName(path), Encoding.ASCII.GetBytes(DirectXComOutputHeaderTests.ReadOriginal(
                    "RecompiledIdlHeaders/" + path, checkoutBytes: true)), Path.GetDirectoryName(path)!);
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (graphicsFirst ? "#include <d3d10_1.h>\n#include <d2d1.h>\n" : "#include <windows.h>\n") +
                "#include <d3d10_1.h>\n#include <d3d10.h>\n#include <d3d9on12.h>\n#include <d3dcompiler.h>\n" +
                "#include <d2d1.h>\n#include <Presentation.h>\n" +
                "#ifdef __cplusplus\n#include <dcomp.h>\n#include <dwrite.h>\n#include <dxcapi.h>\n#include <windows.ui.composition.interop.h>\n#endif\n" +
                "HRESULT (WINAPI *d2d)(D2D1_FACTORY_TYPE, REFIID, const D2D1_FACTORY_OPTIONS*, void**) = &D2D1CreateFactory;\n" +
                "HRESULT (WINAPI *deserialize)(LPCVOID, SIZE_T, REFIID, void**) = &D3D12CreateRootSignatureDeserializer;\n" +
                "HRESULT (WINAPI *versioned)(LPCVOID, SIZE_T, REFIID, void**) = &D3D12CreateVersionedRootSignatureDeserializer;\n" +
                "HRESULT (WINAPI *reflect)(LPCVOID, SIZE_T, REFIID, void**) = &D3DReflect;\n" +
                "HRESULT (WINAPI *library)(LPCVOID, SIZE_T, REFIID, void**) = &D3DReflectLibrary;\n" +
                "HRESULT (WINAPI *presentation)(IUnknown*, REFIID, void**) = &CreatePresentationFactory;\n" +
                "#ifdef __cplusplus\n" +
                "HRESULT (WINAPI *composition)(IDXGIDevice*, REFIID, void**) = &DCompositionCreateDevice;\n" +
                "HRESULT (WINAPI *composition2)(IUnknown*, REFIID, void**) = &DCompositionCreateDevice2;\n" +
                "HRESULT (WINAPI *composition3)(IUnknown*, REFIID, void**) = &DCompositionCreateDevice3;\n" +
                "HRESULT (WINAPI *dwrite)(DWRITE_FACTORY_TYPE, REFIID, IUnknown**) = &DWriteCreateFactory;\n" +
                "HRESULT (__stdcall *dxc)(REFCLSID, REFIID, void**) = &DxcCreateInstance;\n" +
                "HRESULT (__stdcall *dxc2)(IMalloc*, REFCLSID, REFIID, void**) = &DxcCreateInstance2;\n" +
                "HRESULT NativeCalls(ID3D10Device* device, ID3D10Device1* device1, IDirect3DDevice9On12* bridge,\n" +
                "IDirect3DSurface9* surface, IDirect3DVolume9* volume, IDCompositionSurface* compSurface,\n" +
                "IDCompositionTexture* texture, IPresentationManager* presentation, IDxcUtils* utils,\n" +
                "IDxcContainerReflection* reflection, IDxcCompiler3* compiler, ID2D1Resource* resource,\n" +
                "ABI::Windows::UI::Composition::ICompositionTextureInterop* interop, REFIID iid, void** output,\n" +
                "UINT64* value, POINT* point, ID2D1Factory** factory) {\n" +
                "resource->GetFactory(factory);\n" +
                "return device->OpenSharedResource(0, iid, output) + device1->OpenSharedResource(0, iid, output) +\n" +
                "bridge->GetD3D12Device(iid, output) + bridge->UnwrapUnderlyingResource(0, 0, iid, output) +\n" +
                "surface->GetContainer(iid, output) + volume->GetContainer(iid, output) +\n" +
                "compSurface->BeginDraw(0, iid, output, point) + texture->GetAvailableFence(value, iid, output) +\n" +
                "presentation->GetPresentRetiringFence(iid, output) + interop->GetAvailableFence(value, iid, output) +\n" +
                "utils->CreateReflection(0, iid, output) + reflection->GetPartReflection(0, iid, output) +\n" +
                "compiler->Compile(0, 0, 0, 0, iid, output) + compiler->Disassemble(0, iid, output);\n}\n" +
                "HRESULT FactoryTemplates(REFIID iid, ID2D1Factory** factory, void** output) {\n" +
                "D2D1_FACTORY_OPTIONS options = {}; return D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, iid, output) +\n" +
                "D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, factory) + D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, options, factory);\n}\n" +
                "#else\n" +
                "HRESULT NativeCalls(ID3D10Device* device, ID3D10Device1* device1, IDirect3DDevice9On12* bridge,\n" +
                "IDirect3DSurface9* surface, IDirect3DVolume9* volume, IPresentationManager* presentation, REFIID iid, void** output) {\n" +
                "return ID3D10Device_OpenSharedResource(device, 0, iid, output) + ID3D10Device1_OpenSharedResource(device1, 0, iid, output) +\n" +
                "IDirect3DDevice9On12_GetD3D12Device(bridge, iid, output) + IDirect3DDevice9On12_UnwrapUnderlyingResource(bridge, 0, 0, iid, output) +\n" +
                "IDirect3DSurface9_GetContainer(surface, iid, output) + IDirect3DVolume9_GetContainer(volume, iid, output) +\n" +
                "IPresentationManager_GetPresentRetiringFence(presentation, iid, output);\n}\n#endif\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(path =>
                "Copy-Item -LiteralPath " + Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", path)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, path))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + Path.Combine(fixture.Headers, "shared")) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "winrt")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            NativeCoffAssertions.CodeDataAndLinkageEqual(outputs[0], outputs[1]);
            NativeCoffAssertions.CodeDataAndLinkageEqual(outputs[1], outputs[2]);
        }

        [TestMethod]
        public void MediaFoundationOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string direct3D = Direct3DPackagePath();
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string midlInputs = Path.Combine(fixture.Path, "sdk");
            foreach (string folder in new[] { "um", "shared" })
            {
                string destination = Path.Combine(midlInputs, folder);
                Directory.CreateDirectory(destination);
                foreach (string file in Directory.GetFiles(Path.Combine(include, folder)))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            foreach (string file in Directory.GetFiles(Path.Combine(Direct3DPackagePath(), "build", "native", "include")))
                File.Copy(file, Path.Combine(midlInputs, Path.GetFileNameWithoutExtension(file) == "dxgiformat" ? "shared" : "um",
                    Path.GetFileName(file)), overwrite: true);
            string[] inputs = MediaFoundationComOutputHeaderTests.IdlOwners.Append("mfapi.h").ToArray();
            foreach (string owner in inputs)
            {
                byte[] original = File.ReadAllBytes(Path.Combine(include, "um", owner));
                CollectionAssert.AreEqual(MediaFoundationComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/um/" + owner), original,
                    "Reachable fixture introduction parent matches selected SDK: " + owner);
                fixture.Header(owner, original);
            }
            Assert.IsFalse(File.Exists(Path.Combine(include, "um", "mfapi.idl")), "Only mfapi.h is header-only.");
            string selections = string.Join("','", MediaFoundationComOutputHeaderTests.IdlOwners
                .Select(owner => "pre-midl/" + owner + ".patch").Append("post-midl/mfapi.h.patch"));
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            foreach (string owner in inputs)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oleidl.idl", "oaidl.idl", "servprov.idl", "xmldom.idl", "xmldso.idl",
                "msxml.idl", "urlmon.idl", "ocidl.idl", "shtypes.idl", "propidlbase.idl", "propidl.idl",
                "StructuredQueryCondition.idl", "propsys.idl", "mediaobj.idl", "mfobjects.idl", "mftransform.idl",
                "mfidl.idl", "mfcaptureengine.idl", "mfmediaengine.idl", "mfsharingengine.idl", "mfreadwrite.idl",
                "dxgiformat.idl", "d3dcommon.idl", "D3D12MarkerApiEnums.idl", "d3d12.idl", "mfd3d12.idl",
                "devenum.idl", "axcore.idl", "icodecapi.idl", "axextend.idl", "dyngraph.idl", "VMRender.idl",
                "dvdif.idl", "strmif.idl", "evr.idl", "dxva2api.idl" })
            {
                string patched = Path.Combine(fixture.Headers, "um", dependency);
                string input = File.Exists(patched) ? patched :
                    dependency == "d3d12.idl" || dependency == "D3D12MarkerApiEnums.idl" ?
                    Path.Combine(direct3D, "build", "native", "include", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ||
                        dependency == "dxgiformat.idl" ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            var conversion = fixture.Run(command);
            Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            foreach (string owner in MediaFoundationComOutputHeaderTests.IdlOwners)
            {
                var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                    "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); & " +
                    Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, owner.ToLowerInvariant())) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", Path.ChangeExtension(owner, "h").ToLowerInvariant())) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                    " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE");
                Assert.AreEqual(0, result.ExitCode, result.Output);
            }
            AssertRegeneratedHeaders(MediaFoundationComOutputHeaderTests.IdlOwners.Select(owner =>
                (Path.ChangeExtension(owner, "h"), Path.Combine(fixture.Headers, "um", Path.ChangeExtension(owner, "h")))));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                MediaFoundationComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void MediaFoundationOutput_NativeBuildIsANoOp(string arch, string language, bool mediaFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = MediaFoundationComOutputHeaderTests.Owners.Select(owner => owner.Owner).ToArray();
            foreach (string owner in owners)
                fixture.Header(owner, Encoding.ASCII.GetBytes(MediaFoundationComOutputHeaderTests.ReadOriginal(
                    "RecompiledIdlHeaders/um/" + owner, checkoutBytes: true)));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            string declarations = "";
            foreach (var owner in MediaFoundationComOutputHeaderTests.Owners)
            {
                string text = File.ReadAllText(Path.Combine(fixture.Headers, "um", owner.Owner));
                foreach (string function in owner.Selectors.Where(selector => selector.Split("::").Length == 2)
                    .Select(selector => selector.Split("::")[0]).Distinct())
                {
                    string declaration = Regex.Match(text, MediaFoundationComOutputHeaderTests.FunctionPattern(function)).Value;
                    Assert.AreNotEqual("", declaration, function);
                    declarations += declaration + "\n";
                }
            }
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (mediaFirst ? "#include <mfapi.h>\n#include <mfidl.h>\n" : "#include <windows.h>\n") +
                "#include <d3d9.h>\n#include <d3d12.h>\n" +
                string.Join("\n", owners.Select(owner => "#include <" + owner + ">")) + "\n" +
                declarations +
                "HRESULT NativeCalls(IMFCaptureEngineClassFactory* capture, IMFImageSharingEngineClassFactory* sharing,\n" +
                "IMFReadWriteClassFactory* factory, IMFSourceReader* reader, IMFSourceReaderEx* readerEx,\n" +
                "IMFSinkWriter* writer, IMFSinkWriterEx* writerEx, IMFDXGIBuffer* buffer, IMFDXGIDeviceManager* manager,\n" +
                "IMFInputTrustAuthority* trust, IMFObjectReferenceStream* refs, IMFRemoteProxy* proxy,\n" +
                "IMFMediaEngineClassFactory4* media, IDirect3DDeviceManager9* manager9, REFIID iid, REFCLSID clsid,\n" +
                "void** output, IMFImageSharingEngine** engine, IMFAttributes** attributes) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(capture, CreateInstance, clsid, iid, output) + CALL(sharing, CreateInstanceFromUDN, 0, engine) +\n" +
                "CALL(factory, CreateInstanceFromURL, clsid, 0, 0, iid, output) + CALL(factory, CreateInstanceFromObject, clsid, 0, 0, iid, output) +\n" +
                "CALL(reader, GetServiceForStream, 0, iid, iid, output) + CALL(readerEx, GetServiceForStream, 0, iid, iid, output) +\n" +
                "CALL(writer, GetServiceForStream, 0, iid, iid, output) + CALL(writerEx, GetServiceForStream, 0, iid, iid, output) +\n" +
                "CALL(buffer, GetResource, iid, output) + CALL(buffer, GetUnknown, iid, iid, output) +\n" +
                "CALL(manager, GetVideoService, 0, iid, output) + CALL(manager9, GetVideoService, 0, iid, output) +\n" +
                "CALL(trust, GetDecrypter, iid, output) + CALL(refs, LoadReference, iid, output) +\n" +
                "CALL(proxy, GetRemoteHost, iid, output) + CALL(proxy, GetRemoteObject, iid, output) +\n" +
                "CALL(media, CreateContentDecryptionModuleFactory, 0, iid, output) +\n" +
                "MFCreateAttributes(attributes, 0) + MFCreateMediaExtensionActivate(0, 0, iid, output) +\n" +
                "MFCreateVideoSampleAllocatorEx(iid, output) + MFCreateD3D12SynchronizationObject(0, iid, output) +\n" +
                "MFCreateNetSchemePlugin(iid, output) + MFCreatePropertiesFromMediaType(0, iid, output) +\n" +
                "MFCreateStreamOnMFByteStreamEx(0, iid, output) + MFGetService(0, iid, iid, output) +\n" +
                "MFCreateVideoMixer(0, iid, iid, output) + MFCreateVideoMixerAndPresenter(0, 0, iid, output, iid, output) +\n" +
                "MFCreateVideoPresenter(0, iid, iid, output) + MFCreateVideoRenderer(iid, output) +\n" +
                "MFCreateVideoSampleAllocator(iid, output) + DXVA2CreateVideoService(0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            NativeCoffAssertions.CodeDataAndLinkageEqual(outputs[0], outputs[1]);
            NativeCoffAssertions.CodeDataAndLinkageEqual(outputs[1], outputs[2]);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ServiceOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner(string target)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string midlInputs = Path.Combine(fixture.Path, "sdk");
            foreach (string folder in new[] { "um", "shared" })
            {
                string destination = Path.Combine(midlInputs, folder);
                Directory.CreateDirectory(destination);
                foreach (string file in Directory.GetFiles(Path.Combine(include, folder)))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(midlInputs) + "); " +
                "foreach ($owner in 'ComSvcs.h', 'TxDtc.h') { " +
                "if ($outputs.Header -icontains \"um/$owner\") { throw \"Incorrectly classified direct service header: $owner\" } }; " +
                "$callobj = @($outputs | Where-Object { $_.Header -ieq 'um/callobj.h' }); " +
                "if ($callobj.Count -ne 1 -or $callobj[0].Idl -ine 'um/callobj.idl') { throw 'Missing authoritative callobj output' }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            foreach (string owner in new[] { "ComSvcs.h", "TxDtc.h", "callobj.idl" })
            {
                byte[] original = File.ReadAllBytes(Path.Combine(include, "um", owner));
                CollectionAssert.AreEqual(ServiceComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/um/" + owner), original, "Reachable fixture introduction parent matches selected SDK: " + owner);
                fixture.Header(owner, original);
            }
            foreach (string absent in new[] { "ComSvcs.idl", "autosvcs.idl", "TxDtc.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(include, "um", absent)), "Header-only shipped owner: " + absent);
            const string selections = "pre-midl/callobj.idl.patch','post-midl/ComSvcs.h.patch','post-midl/TxDtc.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: midlInputs);
            foreach (string owner in new[] { "ComSvcs.h", "TxDtc.h", "callobj.idl" })
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", owner)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", owner)), owner);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            string command = "";
            foreach (string dependency in new[] { "wtypesbase.idl", "wtypes.idl", "unknwnbase.idl", "unknwn.idl",
                "objidlbase.idl", "objidl.idl", "oaidl.idl", "callobj.idl" })
            {
                string input = dependency == "callobj.idl" ? Path.Combine(fixture.Headers, "um", dependency) :
                    Path.Combine(include, dependency.StartsWith("wtypes", StringComparison.Ordinal) ? "shared" : "um", dependency);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, dependency)) + "; ";
            }
            var conversion = fixture.Run(command);
            Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); & " +
                Quote(Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                " " + Quote(Path.Combine(converted, "callobj.idl")) + " /out " + Quote(converted) +
                " /header " + Quote(Path.Combine(fixture.Headers, "um", "callobj.h")) +
                " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /amd64 /target NT100 /Zp8" +
                " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) + "; exit $LASTEXITCODE");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "um", "callobj.h")), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", "callobj.h")));
            ServiceComOutputHeaderTests.AssertShippedDeclarations(target, Path.Combine(fixture.Headers, "um"));
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ServiceOutput_NativeBuildIsANoOp(string arch, string language, bool serviceFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string[] owners = ServiceComOutputHeaderTests.Owners.Select(owner => owner.Owner).ToArray();
            foreach (string owner in owners)
                fixture.Header(owner, ServiceComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/um/" + owner));
            foreach (string forwarder in new[] { "TxCtx.h", "Mtx.h", "Mtxdm.h" })
                fixture.Header(forwarder, File.ReadAllBytes(Path.Combine(include, "um", forwarder)));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (serviceFirst ? "#include <txctx.h>\n#include <callobj.h>\n" : "#include <windows.h>\n#include <txdtc.h>\n") +
                string.Join("\n", owners.Select(owner => "#include <" + owner + ">")) +
                "\n#include <mtx.h>\n#include <mtxdm.h>\n#include <txctx.h>\n#include <callobj.h>\n" +
                "HRESULT NativeCalls(ITransactionContextEx* context, ICreateWithTransactionEx* transaction,\n" +
                "ICreateWithTipTransactionEx* tip, ICreateWithLocalTransaction* local, IObjectContext* objectContext,\n" +
                "IComTrackingInfoCollection* tracking, IServicePool* pool, IGetDispenser* dispenser,\n" +
                "IResourceManager* manager, IResourceManager2* manager2, IResourceManagerRejoinable* rejoinable,\n" +
                "IResourceManagerFactory2* factory, REFCLSID clsid, REFIID iid, void** output) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(context, CreateInstance, clsid, iid, output) + CALL(transaction, CreateInstance, 0, clsid, iid, output) +\n" +
                "CALL(tip, CreateInstance, 0, clsid, iid, output) + CALL(local, CreateInstanceWithSysTx, 0, clsid, iid, output) +\n" +
                "CALL(objectContext, CreateInstance, clsid, iid, output) + CALL(tracking, Item, 0, iid, output) +\n" +
                "CALL(pool, GetObject, iid, output) + CALL(dispenser, GetDispenser, iid, output) +\n" +
                "CALL(manager, GetDistributedTransactionManager, iid, output) +\n" +
                "CALL(manager2, GetDistributedTransactionManager, iid, output) +\n" +
                "CALL(rejoinable, GetDistributedTransactionManager, iid, output) + CALL(factory, CreateEx, 0, 0, 0, iid, output) +\n" +
                "CoCreateActivity(0, iid, output) + MTSCreateActivity(iid, output) +\n" +
                "CoGetInterceptor(iid, 0, iid, output) + CoGetInterceptorFromTypeInfo(iid, 0, 0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", owner)) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", owner))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [DataTestMethod]
        [DataRow("i686-pc-windows-msvc")]
        [DataRow("x86_64-pc-windows-msvc")]
        [DataRow("aarch64-pc-windows-msvc")]
        public void ClrOutput_RealCopiedOwnersReplayEveryNativeByteThroughFinalEmission(string target)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(include) + "); " +
                "if ($outputs.Header -icontains 'winrt/rometadata.h') { throw 'The direct SDK owner is a queued MIDL output' }; " +
                "if (Select-String -LiteralPath " + Quote(Path.Combine(include, "winrt", "rometadata.h")) +
                " -SimpleMatch 'File created by MIDL compiler' -Quiet) { throw 'Generated SDK header cannot be directly patched' }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            foreach (var owner in ClrComOutputHeaderTests.Owners)
            {
                string file = Path.GetFileName(owner.Path);
                bool compatibility = owner.Path.StartsWith("um/", StringComparison.Ordinal);
                string input = compatibility ? Path.Combine(Root(), "generation", "WinSDK", "AdditionalHeaders", file) :
                    Path.Combine(include, owner.Path.Replace('/', Path.DirectorySeparatorChar));
                byte[] original = File.ReadAllBytes(input);
                CollectionAssert.AreEqual(ClrComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/" + owner.Path), original, "Reachable introduction parent matches actual SDK/compatibility input: " + owner.Path);
                if (compatibility)
                {
                    CollectionAssert.AreEqual(ClrComOutputHeaderTests.ReadOriginalBytes("AdditionalHeaders/" + file), original);
                    Assert.IsFalse(File.Exists(Path.Combine(include, "um", file)), "Do not replace an existing compatibility owner with a removed SDK header.");
                    StringAssert.Contains(System.Text.Encoding.Latin1.GetString(original), "File created by MIDL compiler");
                    Assert.IsFalse(File.Exists(Path.Combine(patches, "post-midl", file + ".patch")),
                        "Unresolved generated compatibility source must retain its sidecar, not acquire a post-header patch.");
                }
                foreach (string area in new[] { "um", "winrt" })
                    Assert.IsFalse(File.Exists(Path.Combine(include, area, Path.ChangeExtension(file, ".idl"))),
                        "No shipped IDL to patch or regenerate: " + file);
                string destination = Path.Combine(fixture.Headers, owner.Path.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.WriteAllBytes(destination, original);
            }
            const string selections = "post-midl/rometadata.h.patch";
            fixture.Apply("pre-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selections, true, patchRoot: patches, midlInputRoot: include);
            foreach (var owner in ClrComOutputHeaderTests.Owners)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders",
                    owner.Path.Replace('/', Path.DirectorySeparatorChar))),
                    File.ReadAllBytes(Path.Combine(fixture.Headers, owner.Path.Replace('/', Path.DirectorySeparatorChar))), owner.Path);
            ClrComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ClrOutput_NativeBuildIsANoOp(string arch, string language, bool hostingFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            foreach (var owner in ClrComOutputHeaderTests.Owners)
                fixture.Header(Path.GetFileName(owner.Path), ClrComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/" + owner.Path));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            // The unchanged compatibility header omits this forward declaration in C.
            File.WriteAllText(source, "#define COBJMACROS\n#define USE_DEPRECATED_CLR_API_WITHOUT_WARNING\n" +
                "#ifndef __cplusplus\ntypedef struct IManagedObject IManagedObject;\n#endif\n" +
                (hostingFirst ? "#include <mscoree.h>\n#include <metahost.h>\n#include <rometadata.h>\n" :
                    "#include <windows.h>\n#include <rometadata.h>\n#include <metahost.h>\n#include <mscoree.h>\n") +
                "HRESULT NativeCalls(IHostControl* host, ICLRControl* clr, REFCLSID clsid, REFIID iid, void** output) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(host, GetHostManager, iid, output) + CALL(clr, GetCLRManager, iid, output) +\n" +
                "CLRCreateInstance(clsid, iid, output) + MetaDataGetDispenser(clsid, iid, output) +\n" +
                "CorBindToCurrentRuntime(0, clsid, iid, output) + CorBindToRuntime(0, 0, clsid, iid, output) +\n" +
                "CorBindToRuntimeByCfg(0, 0, 0, clsid, iid, output) + CorBindToRuntimeEx(0, 0, 0, clsid, iid, output) +\n" +
                "CorBindToRuntimeHost(0, 0, 0, 0, 0, clsid, iid, output) + ClrCreateManagedInstance(0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", ClrComOutputHeaderTests.Owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", owner.Path.Replace('/', Path.DirectorySeparatorChar))) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", Path.GetFileName(owner.Path)))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " " + Quote("/I" + Path.Combine(Root(), "generation", "WinSDK", "AdditionalHeaders")) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [TestMethod]
        public void AudioOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(include) + "); " +
                "foreach ($header in 'um/mmdeviceapi.h','um/Audioclient.h','um/devicetopology.h'," +
                "'um/strmif.h','um/vidcap.h','um/wmsdkidl.h') { " +
                "if (@($outputs | Where-Object Header -ieq $header).Count -ne 1) { throw \"Missing production MIDL owner: $header\" } }; " +
                "foreach ($header in 'um/dsound.h','shared/ksproxy.h') { " +
                "if ($outputs.Header -icontains $header) { throw \"Direct owner is queued: $header\" }; " +
                "if (Select-String -LiteralPath (Join-Path " + Quote(include) + " $header) " +
                "-SimpleMatch 'File created by MIDL compiler' -Quiet) { throw \"Direct owner is generated: $header\" } }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            foreach (string path in AudioComOutputHeaderTests.Idls.Select(name => "um/" + name)
                .Concat(new[] { "um/dsound.h", "shared/ksproxy.h" }))
            {
                byte[] original = File.ReadAllBytes(Path.Combine(include, path.Replace('/', Path.DirectorySeparatorChar)));
                CollectionAssert.AreEqual(AudioComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/" + path), original, "Reachable fixture introduction parent matches selected SDK: " + path);
                string destination = Path.Combine(fixture.Headers, path.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.WriteAllBytes(destination, original);
            }
            foreach (string absent in new[] { "um/dsound.idl", "shared/ksproxy.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(include, absent.Replace('/', Path.DirectorySeparatorChar))));
            string selection = string.Join("','", AudioComOutputHeaderTests.Idls.Select(name => "pre-midl/" + name + ".patch")
                .Concat(new[] { "post-midl/dsound.h.patch", "post-midl/ksproxy.h.patch" }));
            foreach (var owner in AudioComOutputHeaderTests.Owners.Where(owner =>
                owner.Path != "um/dsound.h" && owner.Path != "shared/ksproxy.h"))
            {
                string file = Path.GetFileName(owner.Path);
                byte[] original = File.ReadAllBytes(Path.Combine(include, owner.Path.Replace('/', Path.DirectorySeparatorChar)));
                string destination = fixture.Header(file, original);
                fixture.Patch("post-midl/" + file + ".patch", owner.Path);
                var rejection = fixture.Run("& " + Quote(Script("ApplySDKPatches.ps1")) +
                    " -Phase post-midl -HeaderRoot " + Quote(fixture.Headers) +
                    " -SdkVersion " + Quote(Version) + " -PatchRoot " + Quote(fixture.Patches) +
                    " -MidlInputRoot " + Quote(include) + " -SelectedPatches " + Quote("post-midl/" + file + ".patch"));
                Assert.AreNotEqual(0, rejection.ExitCode, owner.Path);
                StringAssert.Contains(rejection.Output, "targets regenerated header " + owner.Path);
                CollectionAssert.AreEqual(original, File.ReadAllBytes(destination), "Rejected output patch must leave native bytes unchanged.");
            }
            fixture.Apply("pre-midl", selection, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selection, true, patchRoot: patches, midlInputRoot: include);
            foreach (string idl in AudioComOutputHeaderTests.Idls)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", idl)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", idl)), idl);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            var pending = new System.Collections.Generic.Queue<string>(AudioComOutputHeaderTests.Idls.Concat(new[] { "strmif.idl" }));
            var seen = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string command = "";
            while (pending.Count > 0)
            {
                string name = pending.Dequeue();
                if (!seen.Add(name)) continue;
                string input = AudioComOutputHeaderTests.Idls.Contains(name, StringComparer.OrdinalIgnoreCase) ?
                    Path.Combine(fixture.Headers, "um", name) :
                    new[] { "um", "shared", "winrt" }.Select(area => Path.Combine(include, area, name)).Single(File.Exists);
                foreach (Match match in Regex.Matches(File.ReadAllText(input),
                    @"(?im)^\s*(?:import\s+|#include\s*)[""<]([^"">]+\.idl)["">]"))
                    pending.Enqueue(match.Groups[1].Value);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, name)) + "; ";
            }
            Assert.AreEqual(33, seen.Count, "Bounded actual selected/imported/included IDL closure.");
            var conversion = fixture.Run(command);
            Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            command = ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            foreach (string name in new[] { "mmdeviceapi", "audioclient", "devicetopology", "strmif", "vidcap", "wmsdkidl" })
                command += "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, name + ".idl")) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", name + ".h")) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                    " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertAudioDirectionDifference(Path.Combine(fixture.Headers, "um", "Audioclient.h"));
            AssertRegeneratedHeaders(AudioComOutputHeaderTests.Owners.Select(owner =>
                (owner.Path, Path.Combine(fixture.Headers, owner.Path.Replace('/', Path.DirectorySeparatorChar)))));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                AudioComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
        }

        private static void AssertAudioDirectionDifference(string regenerated)
        {
            string original = Encoding.Latin1.GetString(ProductionSalLocalHeaderTests.ReadBeforeCutover(
                "RecompiledIdlHeaders/um/Audioclient.h"));
            string candidate = File.ReadAllText(regenerated);
            string prefix = "#define STDMETHODCALLTYPE __stdcall\n#include <rpcsal.h>\n" +
                "typedef long HRESULT; typedef unsigned int UINT32; typedef int BOOL;\n" +
                "struct GUID { unsigned long a; unsigned short b, c; unsigned char d[8]; };\n";
            prefix += Regex.Match(original, @"typedef /\* \[v1_enum\] \*/\s+enum AUDIO_EFFECT_STATE[\s\S]+?AUDIO_EFFECT_STATE;").Value + "\n";
            prefix += Regex.Match(original, @"typedef struct AUDIO_EFFECT\s*\{[\s\S]+?AUDIO_EFFECT;").Value + "\n";
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
            {
                byte[]? originalSignature = null;
                foreach (bool converted in new[] { false, true })
                {
                    string declaration = Regex.Match(converted ? candidate : original,
                        @"virtual HRESULT STDMETHODCALLTYPE GetAudioEffects\s*\([^;]+;").Value;
                    Assert.AreNotEqual("", declaration);
                    using var emission = new LastErrorHeaderTests.Fixture();
                    var result = emission.Scrape(prefix +
                        "struct __declspec(uuid(\"64abea93-aac7-4f3b-a257-976deba89f04\")) IAudioEffectsManager {\n" +
                        declaration + "\n};", target, sal: true,
                        extraSettings: "--include-directory\n" + MidlHeaderTests.SharedHeaders() + "\n");
                    Assert.AreEqual(0, result.ExitCode, result.Output);
                    string output = emission.Emit();
                    Console.WriteLine("RAW_OUTPUT audio-native-direction." + target + "." +
                        (converted ? "converted" : "tracked") + ".winmd " + Convert.ToBase64String(File.ReadAllBytes(output)));
                    using var pe = new PEReader(File.OpenRead(output));
                    var reader = pe.GetMetadataReader();
                    var method = reader.MethodDefinitions.Select(reader.GetMethodDefinition)
                        .Single(m => reader.GetString(m.Name) == "GetAudioEffects");
                    var parameter = method.GetParameters().Select(reader.GetParameter)
                        .Single(p => reader.GetString(p.Name) == "numEffects");
                    Assert.AreEqual(converted ? ParameterAttributes.Out : ParameterAttributes.In | ParameterAttributes.Out,
                        parameter.Attributes, "Existing native [out] changes final In|Out to Out only; this is not unchanged-output migration proof.");
                    Assert.AreEqual(0, parameter.GetCustomAttributes().Count);
                    if (converted)
                    {
                        Assert.IsNotNull(originalSignature);
                        CollectionAssert.AreEqual(originalSignature, reader.GetBlobBytes(method.Signature));
                    }
                    else originalSignature = reader.GetBlobBytes(method.Signature);
                }
            }
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void AudioOutput_NativeBuildIsANoOp(string arch, string language, bool audioFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            foreach (var owner in AudioComOutputHeaderTests.Owners)
                fixture.Header(Path.GetFileName(owner.Path), AudioComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/" + owner.Path));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n#ifdef __cplusplus\n#define __STREAMS__\n#endif\n" +
                (audioFirst ? "#include <mmdeviceapi.h>\n#include <audioclient.h>\n" : "#include <windows.h>\n") +
                "#include <mmsystem.h>\n#include <ks.h>\n#include <ksmedia.h>\n#include <strmif.h>\n" +
                "#include <mmdeviceapi.h>\n#include <audioclient.h>\n#include <devicetopology.h>\n#include <dsound.h>\n" +
                "#include <ksproxy.h>\n#include <vidcap.h>\n#include <wmsdkidl.h>\n" +
                "HRESULT NativeCalls(IMMDevice* device, IAudioClient* client, IAudioClient2* client2, IAudioClient3* client3,\n" +
                "IPart* part, IDirectSoundBuffer8* sound, IDirectSoundCaptureBuffer8* capture,\n" +
                "ICaptureGraphBuilder* graph, ICaptureGraphBuilder2* graph2, IDvdGraphBuilder* dvd,\n" +
                "IKsTopology* topology, IKsTopologyInfo* info, IWMReaderAccelerator* reader, REFIID iid, void** output) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(device, Activate, iid, 0, 0, output) + CALL(client, GetService, iid, output) +\n" +
                "CALL(client2, GetService, iid, output) + CALL(client3, GetService, iid, output) +\n" +
                "CALL(part, Activate, 0, iid, output) + CALL(sound, GetObjectInPath, iid, 0, iid, output) +\n" +
                "CALL(capture, GetObjectInPath, iid, 0, iid, output) + CALL(graph, FindInterface, 0, 0, iid, output) +\n" +
                "CALL(graph2, FindInterface, 0, 0, 0, iid, output) + CALL(dvd, GetDvdInterface, iid, output) +\n" +
                "CALL(topology, CreateNodeInstance, 0, 0, 0, 0, iid, output) + CALL(info, CreateNodeInstance, 0, iid, output) +\n" +
                "CALL(reader, GetCodecInterface, 0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", AudioComOutputHeaderTests.Owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", owner.Path.Replace('/', Path.DirectorySeparatorChar))) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", Path.GetFileName(owner.Path)))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [TestMethod]
        public void ImagingOutput_RealSdkPatchesAndMidlReplayEveryPhysicalOwner()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            string patches = Path.Combine(Root(), "generation", "WinSDK", "patches");
            string[] headerOnly = { "Sti.h", "wia_lh.h", "PhotoAcquire.h", "CommCtrl.h" };
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(include) + "); " +
                "foreach ($header in 'um/wincodecsdk.h','um/wincodec.h','um/commoncontrols.h') { " +
                "if (@($outputs | Where-Object Header -ieq $header).Count -ne 1) { throw \"Missing production MIDL owner: $header\" } }; " +
                "foreach ($header in 'um/Sti.h','um/wia_lh.h','um/PhotoAcquire.h','um/CommCtrl.h') { " +
                "if ($outputs.Header -icontains $header) { throw \"Direct SDK owner is queued: $header\" } }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            foreach (string name in ImagingComOutputHeaderTests.Idls.Concat(headerOnly))
            {
                byte[] original = File.ReadAllBytes(Path.Combine(include, "um", name));
                CollectionAssert.AreEqual(ImagingComOutputHeaderTests.ReadOriginalBytes(
                    "RecompiledIdlHeaders/um/" + name), original, "Reachable fixture introduction parent matches selected SDK: " + name);
                fixture.Header(name, original);
            }
            foreach (string absent in new[] { "sti.idl", "wia_lh.idl", "wia.idl", "PhotoAcquire.idl", "CommCtrl.idl" })
                Assert.IsFalse(File.Exists(Path.Combine(include, "um", absent)));
            string selection = string.Join("','", ImagingComOutputHeaderTests.Idls.Select(name => "pre-midl/" + name + ".patch")
                .Concat(headerOnly.Select(name => "post-midl/" + name + ".patch")));
            foreach (var owner in ImagingComOutputHeaderTests.Owners.Where(owner =>
                !headerOnly.Contains(Path.GetFileName(owner.Path), StringComparer.OrdinalIgnoreCase)))
            {
                string file = Path.GetFileName(owner.Path);
                byte[] original = File.ReadAllBytes(Path.Combine(include, owner.Path.Replace('/', Path.DirectorySeparatorChar)));
                string destination = fixture.Header(file, original);
                fixture.Patch("post-midl/" + file + ".patch", owner.Path);
                var rejection = fixture.Run("& " + Quote(Script("ApplySDKPatches.ps1")) +
                    " -Phase post-midl -HeaderRoot " + Quote(fixture.Headers) +
                    " -SdkVersion " + Quote(Version) + " -PatchRoot " + Quote(fixture.Patches) +
                    " -MidlInputRoot " + Quote(include) + " -SelectedPatches " + Quote("post-midl/" + file + ".patch"));
                Assert.AreNotEqual(0, rejection.ExitCode, owner.Path);
                StringAssert.Contains(rejection.Output, "targets regenerated header " + owner.Path);
                CollectionAssert.AreEqual(original, File.ReadAllBytes(destination), "Rejected output patch must leave native bytes unchanged.");
            }
            fixture.Apply("pre-midl", selection, true, patchRoot: patches, midlInputRoot: include);
            fixture.Apply("post-midl", selection, true, patchRoot: patches, midlInputRoot: include);
            foreach (string idl in ImagingComOutputHeaderTests.Idls)
                CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", "um", idl)), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", idl)), idl);

            string converted = Path.Combine(fixture.Path, "converted");
            Directory.CreateDirectory(converted);
            var pending = new System.Collections.Generic.Queue<string>(ImagingComOutputHeaderTests.Idls);
            var seen = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string command = "";
            while (pending.Count > 0)
            {
                string name = pending.Dequeue();
                if (!seen.Add(name)) continue;
                string input = ImagingComOutputHeaderTests.Idls.Contains(name, StringComparer.OrdinalIgnoreCase) ?
                    Path.Combine(fixture.Headers, "um", name) :
                    new[] { "um", "shared", "winrt" }.Select(area => Path.Combine(include, area, name)).Single(File.Exists);
                foreach (Match match in Regex.Matches(File.ReadAllText(input),
                    @"(?im)^\s*(?:import\s+|#include\s*)[""<]([^"">]+\.idl)["">]"))
                    pending.Enqueue(match.Groups[1].Value);
                command += "& " + Quote(Script("ConvertMidlAttributesToSalAnnotations.ps1")) + " " +
                    Quote(input) + " " + Quote(Path.Combine(converted, name)) + "; ";
            }
            Assert.AreEqual(22, seen.Count, "Bounded actual selected/imported/included IDL closure.");
            var conversion = fixture.Run(command);
            Assert.AreEqual(0, conversion.ExitCode, conversion.Output);
            command = ". " + Quote(Script("CommonUtils.ps1")) +
                "; $env:Path += ';' + (Get-VcDirPath -Arch x86 -HostArch x86); ";
            foreach (string name in new[] { "wincodecsdk", "wincodec", "commoncontrols" })
                command += "& " + Quote(Path.Combine(sdk, "c", "bin", "10.0.28000.0", "x86", "midl.exe")) +
                    " " + Quote(Path.Combine(converted, name + ".idl")) + " /out " + Quote(converted) +
                    " /header " + Quote(Path.Combine(fixture.Headers, "um", name + ".h")) +
                    " /no_warn /DUNICODE /D_UNICODE /DWINVER=0x0A00 /D_APISET_MINWIN_VERSION=0x010F /DNTDDI_VERSION=0x0A00000C" +
                    " /DBUILD_UMS_ENABLED=0 /DBUILD_WOW64_ENABLED=0 /DBUILD_ARM64X_ENABLED=0 /DEXECUTABLE_WRITES_SUPPORT=0" +
                    " /D_USE_DECLSPECS_FOR_SAL=1 /D_CONTROL_FLOW_GUARD_SVCTAB=1 /DMIDL_PASS=1 /D_AMD64_ /D_WIN64 /D_WCHAR_T_DEFINED" +
                    " /no_stamp /nologo /no_settings_comment /lcid 1033 /sal /sal_local /amd64 /target NT100 /Zp8" +
                    " /I " + Quote(converted) + " /I " + Quote(Path.Combine(include, "um")) +
                    " /I " + Quote(Path.Combine(include, "shared")) + " /I " + Quote(Path.Combine(include, "winrt")) +
                    " /I " + Quote(Path.Combine(Root(), "scripts", "inc")) +
                    "; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; ";
            var result = fixture.Run(command);
            Assert.AreEqual(0, result.ExitCode, result.Output);
            AssertRegeneratedHeaders(ImagingComOutputHeaderTests.Owners.Select(owner =>
                (owner.Path, Path.Combine(fixture.Headers, owner.Path.Replace('/', Path.DirectorySeparatorChar)))));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                ImagingComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void ImagingOutput_NativeBuildIsANoOp(string arch, string language, bool imagingFirst)
        {
            using var fixture = new Fixture();
            string include = Path.Combine(LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0");
            foreach (var owner in ImagingComOutputHeaderTests.Owners)
                fixture.Header(Path.GetFileName(owner.Path), ImagingComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/" + owner.Path));
            foreach (string forwarding in new[] { "Wia.h", "commctrl.inl" })
                fixture.Header(forwarding, ImagingComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/um/" + forwarding));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (imagingFirst ? "#include <wincodecsdk.h>\n#include <commctrl.h>\n#include <commoncontrols.h>\n" : "#include <windows.h>\n") +
                "#include <windows.h>\n#include <commctrl.h>\n#include <commoncontrols.h>\n#include <wincodecsdk.h>\n" +
                "#include <sti.h>\n#include <wia.h>\n#include <photoacquire.h>\n" +
                "HRESULT NativeCalls(IWICMetadataReaderInfo* reader, IWICBitmapDecoderInfo* decoder, IWICBitmapEncoderInfo* encoder,\n" +
                "IWICD3DTextureSource* texture, IWiaItem2* item, IPhotoAcquireSource* photo,\n" +
                "IImageList* images, IImageList2* images2, REFCLSID clsid, REFIID iid, void** output) {\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "return CALL(reader, CreateInstance, (IWICMetadataReader**)output) +\n" +
                "CALL(decoder, CreateInstance, (IWICBitmapDecoder**)output) + CALL(encoder, CreateInstance, (IWICBitmapEncoder**)output) +\n" +
                "CALL(texture, GetTexture, 0, 0, iid, output) + CALL(texture, GetTransformedTexture, 0, 0, 0, 0, WICBitmapTransformRotate0, 0, 0, iid, output) +\n" +
                "StiCreateInstanceW(0, 0, (IStillImageW**)output, 0) + CALL(item, GetExtension, 0, 0, iid, output) +\n" +
                "CALL(photo, BindToObject, iid, output) + ImageList_CoCreateInstance(clsid, 0, iid, output) +\n" +
                "ImageList_ReadEx(0, 0, iid, output) + HIMAGELIST_QueryInterface(0, iid, output) +\n" +
                "CALL(images, Clone, iid, output) + CALL(images, GetDragImage, 0, 0, iid, output) +\n" +
                "CALL(images, Merge, 0, 0, 0, 0, 0, iid, output) + CALL(images2, GetCallback, iid, output) +\n" +
                "CALL(images2, Clone, iid, output) + CALL(images2, GetDragImage, 0, 0, iid, output) +\n" +
                "CALL(images2, Merge, 0, 0, 0, 0, 0, iid, output);\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            string replacements = string.Join("; ", ImagingComOutputHeaderTests.Owners.Select(owner => "Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", owner.Path.Replace('/', Path.DirectorySeparatorChar))) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", Path.GetFileName(owner.Path)))));
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { " + replacements + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        [TestMethod]
        public void SyncOutput_RealSdkHeaderOnlyPatchReplaysEveryByteThroughFinalEmission()
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            var ownership = fixture.Run("$outputs = @(& " + Quote(Script("Get-SDKMidlOutputs.ps1")) +
                " -HeaderRoot " + Quote(include) + "); " +
                "if ($outputs.Header -icontains 'um/winsync.h') { throw 'Synchronization header is a queued output.' }");
            Assert.AreEqual(0, ownership.ExitCode, ownership.Output);
            byte[] original = File.ReadAllBytes(Path.Combine(include, "um", "winsync.h"));
            CollectionAssert.AreEqual(SyncComOutputHeaderTests.ReadOriginalBytes(
                "RecompiledIdlHeaders/um/winsync.h"), original, "Reachable fixture introduction parent matches the selected shipped SDK.");
            Assert.IsFalse(File.Exists(Path.Combine(include, "um", "winsync.idl")), "Actual header-only input; do not restore an IDL.");
            fixture.Header("winsync.h", original);
            fixture.Apply("post-midl", "post-midl/winsync.h.patch", true,
                patchRoot: Path.Combine(Root(), "generation", "WinSDK", "patches"), midlInputRoot: include);
            CollectionAssert.AreEqual(File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                "RecompiledIdlHeaders", "um", "winsync.h")), File.ReadAllBytes(Path.Combine(fixture.Headers, "um", "winsync.h")));
            foreach (string target in new[] { "i686-pc-windows-msvc", "x86_64-pc-windows-msvc", "aarch64-pc-windows-msvc" })
                SyncComOutputHeaderTests.AssertShippedDeclarations(target, fixture.Headers);
        }

        [DataTestMethod]
        [DataRow("x86", "/TC", false)]
        [DataRow("x64", "/TC", false)]
        [DataRow("arm64", "/TC", false)]
        [DataRow("x86", "/TP", false)]
        [DataRow("x64", "/TP", false)]
        [DataRow("arm64", "/TP", false)]
        [DataRow("x86", "/TC", true)]
        [DataRow("x64", "/TC", true)]
        [DataRow("arm64", "/TC", true)]
        [DataRow("x86", "/TP", true)]
        [DataRow("x64", "/TP", true)]
        [DataRow("arm64", "/TP", true)]
        public void SyncOutput_NativeBuildIsANoOp(string arch, string language, bool syncFirst)
        {
            using var fixture = new Fixture();
            string sdk = LastErrorHeaderTests.SdkPackagePath();
            string include = Path.Combine(sdk, "c", "include", "10.0.28000.0");
            fixture.Header("winsync.h", SyncComOutputHeaderTests.ReadOriginalBytes("RecompiledIdlHeaders/um/winsync.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "inc", "sal.h"), Path.Combine(fixture.Headers, "sal.h"));
            File.Copy(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "shared",
                "win32metadata_annotations.h"), Path.Combine(fixture.Headers, "win32metadata_annotations.h"));
            string source = Path.Combine(fixture.Path, "native.c");
            File.WriteAllText(source, "#define COBJMACROS\n" +
                (syncFirst ? "#include <winsync.h>\n" : "#include <windows.h>\n") +
                "#include <windows.h>\n#include <winsync.h>\n#include <syncregistration.h>\n" +
                "#include <synchronizationerrors.h>\n#include <syncregistrationerrors.h>\n" +
                "#ifdef __cplusplus\n#define CALL(object, method, ...) object->method(__VA_ARGS__)\n" +
                "#else\n#define CALL(object, method, ...) object->lpVtbl->method(object, __VA_ARGS__)\n#endif\n" +
                "HRESULT NativeCalls(IClockVector* clock, IFeedClockVector* feed, IRangeException* range,\n" +
                "ISingleItemException* single, IChangeUnitException* unit, ISyncKnowledge* knowledge,\n" +
                "IForgottenKnowledge* forgotten, ISyncKnowledge2* knowledge2, REFIID iid, void** output) {\n" +
                "return CALL(clock, GetClockVectorElements, iid, output) + CALL(feed, GetClockVectorElements, iid, output) +\n" +
                "CALL(range, GetClockVector, iid, output) + CALL(single, GetClockVector, iid, output) +\n" +
                "CALL(unit, GetClockVector, iid, output) + CALL(knowledge2, GetInspector, iid, output) +\n" +
                string.Join(" +\n", new[] { "knowledge", "forgotten", "knowledge2" }.SelectMany(owner => new[] {
                    "CALL(" + owner + ", GetScopeVector, iid, output)",
                    "CALL(" + owner + ", GetRangeExceptions, iid, output)",
                    "CALL(" + owner + ", GetSingleItemExceptions, iid, output)",
                    "CALL(" + owner + ", GetChangeUnitExceptions, iid, output)",
                    "CALL(" + owner + ", FindClockVectorForItem, 0, iid, output)",
                    "CALL(" + owner + ", FindClockVectorForChangeUnit, 0, 0, iid, output)" })) + ";\n}\n");
            string obj = Path.Combine(fixture.Path, "native.obj");
            var result = fixture.Run(". " + Quote(Script("CommonUtils.ps1")) +
                "; $vc = Get-VcDirPath -Arch " + Quote(arch) + " -HostArch x64; " +
                "$compiler = Join-Path $vc 'cl.exe'; $vcInclude = Join-Path $vc '..\\..\\..\\include'; " +
                "foreach ($mode in 'baseline', 'ordinary', 'metadata') { if ($mode -eq 'ordinary') { Copy-Item -LiteralPath " +
                Quote(Path.Combine(Root(), "generation", "WinSDK", "RecompiledIdlHeaders", "um", "winsync.h")) +
                " -Destination " + Quote(Path.Combine(fixture.Headers, "um", "winsync.h")) + " }; " +
                "& $compiler /nologo /c " + language + " " + Quote(source) + " " + Quote("/Fo" + obj) +
                " " + Quote("/I" + Path.Combine(fixture.Headers, "um")) + " " + Quote("/I" + fixture.Headers) +
                " \"/I$vcInclude\" " + Quote("/I" + Path.Combine(include, "um")) +
                " " + Quote("/I" + Path.Combine(include, "shared")) + " " + Quote("/I" + Path.Combine(include, "ucrt")) +
                " " + Quote("/I" + Path.Combine(include, "winrt")) +
                " /DWINVER=0x0A00 /D_WIN32_WINNT=0x0A00 /DNTDDI_VERSION=0x0A00000C" +
                " $(if ($mode -eq 'metadata') { '/DWIN32METADATA' }); " +
                "if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; Copy-Item -LiteralPath " + Quote(obj) +
                " -Destination (Join-Path " + Quote(fixture.Path) + " ($mode + '.obj')) }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            byte[][] outputs = new[] { "baseline", "ordinary", "metadata" }
                .Select(mode => File.ReadAllBytes(Path.Combine(fixture.Path, mode + ".obj"))).ToArray();
            foreach (byte[] bytes in outputs)
            {
                Assert.AreEqual(arch == "x86" ? 0x14c : arch == "x64" ? 0x8664 : 0xaa64, BitConverter.ToUInt16(bytes, 0));
                Array.Clear(bytes, 4, 4);
            }
            CollectionAssert.AreEqual(outputs[0], outputs[1]);
            CollectionAssert.AreEqual(outputs[1], outputs[2]);
        }

        private static string Direct3DPackagePath()
        {
            var result = LastErrorHeaderTests.Run("pwsh", "-NoProfile", "-NonInteractive", "-Command",
                ". " + Quote(Script("CommonUtils.ps1")) +
                "; Get-NugetPropsProperty -name PkgMicrosoft_Direct3D_D3D12 -projectName ClangSharpSourceToWinmdTests.csproj 6>$null");
            Assert.AreEqual(0, result.ExitCode, result.Output);
            string path = result.Output.Trim();
            Assert.AreEqual("1.619.3", Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar)));
            return path;
        }

        [TestMethod]
        public void IngestionHooks_BindCopiedInputsAndFinalOutputs()
        {
            string script = File.ReadAllText(Script("RecompileIdlFilesForScraping.ps1"));
            int pre = script.IndexOf("-Phase pre-midl -HeaderRoot $recompiledIdlHeadersDir", StringComparison.Ordinal);
            int post = script.IndexOf("-Phase post-midl -HeaderRoot $recompiledIdlHeadersDir", StringComparison.Ordinal);
            Assert.IsTrue(pre > script.IndexOf("Copy-Item \"$windowsWin32ProjectRoot\\AdditionalHeaders", StringComparison.Ordinal));
            Assert.IsTrue(pre < script.IndexOf("& $PSScriptRoot\\ConvertMidlAttributesToSalAnnotations.ps1", StringComparison.Ordinal));
            Assert.IsTrue(script.IndexOf("$midlOutputs = @($midlInputs", StringComparison.Ordinal) < pre);
            StringAssert.Contains(script, "$midlOutputs | ForEach-Object -ThrottleLimit");
            StringAssert.Contains(script, "Copy-Item $outputHeader $destination");
            Assert.AreEqual(2, script.Split("-MidlOutputs $midlOutputs").Length - 1);
            Assert.IsTrue(post > script.LastIndexOf("Copy-Item $outputHeader", StringComparison.Ordinal));
            Assert.IsTrue(post > script.LastIndexOf("git restore", StringComparison.Ordinal));
            Assert.AreEqual(2, script.Split("-SdkVersion ([IO.Path]::GetFileName($cppPkgPath)) -SelectedPatches $SelectedPatches").Length - 1);
            StringAssert.Contains(File.ReadAllText(Script("UpdateSDK.ps1")),
                ". \"$PSScriptRoot\\RecompileIdlFilesForScraping.ps1\" -SelectedPatches $SelectedPatches");
        }

        [TestMethod]
        public void CiLane_IsParallelSelectiveAndEnforcedByExistingRequiredGate()
        {
            string workflow = File.ReadAllText(Path.Combine(Root(), ".github", "workflows", "pr-validation.yml")).Replace("\r\n", "\n");
            string job = Regex.Match(workflow, @"^  sdk-patch-tests:\n(?<job>.*?)(?=^  \S)", RegexOptions.Multiline | RegexOptions.Singleline).Groups["job"].Value;
            StringAssert.Contains(job, "needs: [changes]");
            StringAssert.Contains(job, "if: needs.changes.outputs.sdk_patches_needed == 'true'");
            StringAssert.Contains(job, "--filter 'FullyQualifiedName~ClangSharpSourceToWinmdTests.SdkPatchTests'");
            string project = File.ReadAllText(Path.Combine(Root(), "tests", "ClangSharpSourceToWinmdTests", "ClangSharpSourceToWinmdTests.csproj"));
            StringAssert.Contains(project, "<PackageReference Include=\"Microsoft.Windows.SDK.CPP\" Version=\"" + Version + "\"");
            StringAssert.Contains(project, "GeneratePathProperty=\"true\" IncludeAssets=\"none\" PrivateAssets=\"all\"");
            Assert.IsFalse(job.Contains("dotnet restore BuildTools", StringComparison.Ordinal));
            Assert.IsFalse(job.Contains("continue-on-error:", StringComparison.Ordinal));
            StringAssert.Contains(File.ReadAllText(Script("TestTooling.ps1")),
                "'FullyQualifiedName!~ClangSharpSourceToWinmdTests.SdkPatchTests'");
            StringAssert.Contains(workflow, "const sdkPatchesNeeded = fileListIncomplete || changedPaths.some");
            StringAssert.Contains(workflow, "file.previous_filename, file.filename");
            foreach (string path in new[] { "scripts/ApplySDKPatches.ps1", "scripts/UpdateSDK.ps1",
                "scripts/RecompileIdlFilesForScraping.ps1", "scripts/Get-SDKMidlOutputs.ps1", "generation/WinSDK/patches/",
                "tests/ClangSharpSourceToWinmdTests/SdkPatchFixtures/" })
                StringAssert.Contains(workflow, $"'{path}'");
            StringAssert.Contains(workflow, "needs: [changes, workflow-tests, tooling-tests, scrape, build-test, api-diff, sdk-patch-tests]");

            Match block = Regex.Match(workflow,
                @"name: Require SDK tests for ingestion changes.*?        run: \|\n(?<script>(?:          [^\n]*\n)+)",
                RegexOptions.Singleline);
            Assert.IsTrue(block.Success);
            string script = Regex.Replace(block.Groups["script"].Value, "^          ", "", RegexOptions.Multiline);
            var cases = new[]
            {
                new { needed = "true", result = "success", fails = false },
                new { needed = "false", result = "skipped", fails = false },
                new { needed = "true", result = "failure", fails = true },
                new { needed = "true", result = "skipped", fails = true },
                new { needed = "true", result = "cancelled", fails = true },
                new { needed = "true", result = "", fails = true },
                new { needed = "", result = "success", fails = true },
                new { needed = "false", result = "success", fails = true },
                new { needed = "false", result = "", fails = true }
            };
            using var fixture = new Fixture();
            var result = fixture.Run(
                $"$gate = [scriptblock]::Create({Quote(script)}); $cases = {Quote(JsonSerializer.Serialize(cases))} | ConvertFrom-Json; " +
                "foreach ($case in $cases) { " +
                "$env:SDK_PATCHES_NEEDED = $case.needed; " +
                "$env:NEEDS = @{ 'sdk-patch-tests' = @{ result = $case.result } } | ConvertTo-Json; " +
                "$failed = $false; try { & $gate } catch { $failed = $true }; " +
                "if ($failed -ne $case.fails) { throw \"Unexpected SDK gate outcome: $($case | ConvertTo-Json -Compress)\" } }");
            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        private static void AssertRegeneratedHeader(string owner, string actual)
            => AssertRegeneratedHeaders(new[] { (owner, actual) });

        private static void AssertRegeneratedHeaders(IEnumerable<(string Owner, string Actual)> headers)
        {
            var outputs = headers.Select(header =>
            {
                string relative = header.Owner.Contains('/') ? header.Owner.Replace('/', Path.DirectorySeparatorChar) :
                    Path.Combine("um", header.Owner);
                return (header.Owner, header.Actual, Expected: File.ReadAllBytes(Path.Combine(Root(), "generation", "WinSDK",
                    "RecompiledIdlHeaders", relative)), Bytes: File.ReadAllBytes(header.Actual));
            }).ToArray();
            foreach (var output in outputs.Where(output => !output.Expected.SequenceEqual(output.Bytes)))
            {
                Console.WriteLine("RAW_OUTPUT sdk-replay." + Path.GetFileName(output.Owner) + ".expected " + Convert.ToBase64String(output.Expected));
                Console.WriteLine("RAW_OUTPUT sdk-replay." + Path.GetFileName(output.Owner) + ".actual " + Convert.ToBase64String(output.Bytes));
                string name = Path.ChangeExtension(Path.GetFileName(output.Actual), "idl");
                string converted = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(output.Actual)))!, "converted", name);
                if (File.Exists(converted))
                {
                    Console.WriteLine("RAW_OUTPUT sdk-replay." + name + ".converted " + Convert.ToBase64String(File.ReadAllBytes(converted)));
                    string native = Path.ChangeExtension(output.Actual, "idl");
                    if (!File.Exists(native))
                        native = new[] { "um", "shared", "winrt" }.Select(area => Path.Combine(
                            LastErrorHeaderTests.SdkPackagePath(), "c", "include", "10.0.28000.0", area, name)).Single(File.Exists);
                    Console.WriteLine("RAW_OUTPUT sdk-replay." + name + ".native " + Convert.ToBase64String(File.ReadAllBytes(native)));
                }
            }
            foreach (var output in outputs)
                CollectionAssert.AreEqual(output.Expected, output.Bytes, output.Owner);
        }

        private static string Root() => LastErrorHeaderTests.RepositoryRoot();

        private static string Script(string name) => System.IO.Path.Combine(Root(), "scripts", name);
        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";

        private sealed class Fixture : IDisposable
        {
            public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "sdk-patch-tests-" + Guid.NewGuid().ToString("N"));
            public string Headers => System.IO.Path.Combine(Path, "headers");
            public string Patches => System.IO.Path.Combine(Path, "patches");

            public Fixture() { Directory.CreateDirectory(Headers); Directory.CreateDirectory(Patches); }
            public string Header(string owner, byte[] bytes, string folder = "um")
            {
                string file = System.IO.Path.Combine(Headers, folder, owner);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(file)!);
                File.WriteAllBytes(file, bytes);
                return file;
            }

            public string Patch(string selection, string target)
            {
                string file = System.IO.Path.Combine(Patches, selection.Replace('/', System.IO.Path.DirectorySeparatorChar));
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(file)!);
                string path = "generation/WinSDK/RecompiledIdlHeaders/" + target;
                File.WriteAllText(file, $"# SDK-Version: {Version}\ndiff --git a/{path} b/{path}\n--- a/{path}\n+++ b/{path}\n@@ -2,2 +2,2 @@\n-before\n+after\n suffix\n");
                return file;
            }

            public void Apply(string phase, string selection, bool success, string version = Version, string? patchRoot = null,
                string? midlInputRoot = null)
            {
                var result = Run($"& {Quote(Script("ApplySDKPatches.ps1"))} -Phase {Quote(phase)} -HeaderRoot {Quote(Headers)} " +
                    $"-SdkVersion {Quote(version)} -PatchRoot {Quote(patchRoot ?? Patches)} -SelectedPatches @('{selection}')" +
                    (midlInputRoot == null ? "" : " -MidlInputRoot " + Quote(midlInputRoot)));
                Assert.AreEqual(success, result.ExitCode == 0, result.Output);
            }

            public (int ExitCode, string Output) Run(string command)
            {
                if (command.Contains("/sal", StringComparison.Ordinal))
                    Console.WriteLine("MIDL_COMMAND " + command);
                else if (command.Contains("ConvertMidlAttributesToSalAnnotations.ps1", StringComparison.Ordinal) ||
                    command.Contains("$compiler", StringComparison.Ordinal))
                    Console.WriteLine("SDK_COMMAND " + command);
                var info = new ProcessStartInfo("pwsh") { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, WorkingDirectory = Path };
                foreach (string argument in new[] { "-NoProfile", "-NonInteractive", "-Command", "$ErrorActionPreference = 'Stop'; " + command }) info.ArgumentList.Add(argument);
                using var process = Process.Start(info) ?? throw new InvalidOperationException("PowerShell did not start.");
                var output = process.StandardOutput.ReadToEndAsync();
                var error = process.StandardError.ReadToEndAsync();
                if (!process.WaitForExit(60000)) { process.Kill(true); throw new TimeoutException("SDK patch fixture timed out."); }
                return (process.ExitCode, output.GetAwaiter().GetResult() + error.GetAwaiter().GetResult());
            }

            public void Dispose()
            {
                foreach (string mode in new[] { "baseline", "ordinary", "metadata" })
                {
                    string file = System.IO.Path.Combine(Path, mode + ".obj");
                    if (File.Exists(file))
                        Console.WriteLine("RAW_OUTPUT native." + System.IO.Path.GetFileName(Path) + "." + mode +
                            ".obj " + Convert.ToBase64String(File.ReadAllBytes(file)));
                }
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
