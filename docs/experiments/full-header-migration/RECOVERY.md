# Working-first recovery and sidecar reduction

**Current accepted cohort:** [27 DRT DLL overrides](#drt-import-overrides)
above preserved `4f645a80c85fa1e69c3a11c703b5c0384ee4dec0`, following
the 23 file/memory/PE groups and the previous 562 groups above `85444f25`.
The broader qualification also restores BOOL's legacy storage after exposing
bitfield backing-type changes; COLORREF's native selection remains.
Its actual WinMD is `6751041E...`; the complete 35,379-type dump is
`1F4F3839...`, byte-identical to accepted structural checkpoint 6d44.
Earlier authorized fictitious-handle/AssociatedEnum changes remain completely
accounted for relative to the original parent; no arbitrary API loss is
accepted. The dated quantities later in this document describe preserved
historical checkpoints, not the current totals.

| Inventory / units | Recovery | Accepted current | Net retired |
|---|---:|---:|---:|
| Original parameter attribute atoms | 770 | 361 | 409 |
| All RSP value rows | 146,789 | 134,597 | 12,192 |
| Allowed documentation URL rows | 90,502 | 90,502 | 0 (outside backlog) |
| Other RSP rows, mixed configuration/API units | 56,287 | 44,095 | 12,192 |
| Emitter `memberRemap` rows | 2,295 | 1,592 | 703 |
| LastError rows | 3,362 | 468 | 2,894 |
| Availability rows | 17,070 | 7,883 | 9,187 |
| Enum JSON records, mixed definitions/additional uses | 1,070 | 410 | 660 |
| autoTypes records | 444 | 436 | 8 |
| Function-pointer fixup records | 27 | 27 | 0 |

| Remaining category | Measured accepted quantity | Examination state |
|---|---:|---|
| Definition-bearing enum records | 386 | 585 further whole definitions accepted across nine cohorts. The new 24-group file/memory/PE trial retains 23, restoring IMAGE_OPTIONAL_HEADER_MAGIC; the actual WinInet definition dependency is qualified rather than deferred. Earlier restored closures remain explicit. |
| Additional-use enum records / selector rows | 23 / 55 | Exact retained FILE/REG/SNMP/process, STGM, COM/MIDL and Cloud closures; observed boundaries are in [PROBLEMS.md](PROBLEMS.md). |
| Genuine CLR flags request | 1 record | Native enum use override already redundant/retired; flags effect still unqualified. |
| Native type/storage/ownership inputs | 436 records | 424 legacy-storage / 12 native selections; genuine types remain. COLORREF storage and its redundant NativeTypedef flag are retired, not its record. BOOL/BOOLEAN and six DiagnosticDataQuery selections/producer closures remain restored; thirteen contextual cleanup records are separate boundaries, not all 436 records. |
| Parameter atoms | 361 | Individually inventoried; nine CLR selectors explicitly excepted. Two actual font/theme association controls isolate storage-policy conflicts on all three targets; this does not establish failure of all related selectors. |
| DLL mappings | 26,376 scan-derived rows; 373 explicit overrides | The explicit rows are 358 manual + 14 partition + 1 global. Twenty-seven shipped-DRT overrides moved into headers; all 145 peer mappings remain pending retained-header ownership qualification. Import scanning remains the specified default. |
| Other RSP operational/API rows | Exact file/option TSV | Parser/global/routing configuration is separate from semantic retirement; do not remove it to inflate counts. |

**Allowed external metadata:** the user explicitly permits all 90,502
documentation URL mappings to remain in `documentationMappings.rsp` for now.
They are separate from the annotation-retirement backlog and are not migrated
or removed here. No such exemption was granted to DLL mappings or other API
metadata; their unchanged state in these bounded cohorts is not an allowance.
The 44,095 non-documentation rows are not 44,095 semantic annotations:
26,376 are the checked-in scan-derived library table, 1,950 are parser/reference
configuration, and 15,769 are mixed API/policy/parser-review rows, including
3,081 namespace rows. The last category is neither distinct facts nor disjoint
from the parameter-atom inventory. The specification makes import scanning
the default; scan-derived rows are not 26,376 per-function header overrides.

The user redirected this experiment from broad sidecar retirement to a working
generation anchor. The diagnostic experiment remains in commit
`88991baf654ff0d89855a7005873addd92a76859`; its findings are not withdrawn.

The recovery restores the complete qualified parent **generation input set**
from `60188deb6ed2c22b14a5df028897b3a941a5d2aa`, without reverting tooling,
rewriting history, or changing lower stack layers. This conservative batch keeps
only the already-qualified parent header pilots. **No additional broad-migration
header coverage is claimed.** All 737 broad patches and their sidecar retirements
are deferred until they can be enabled in small batches that each emit.

## DRT import overrides

The 172-row peer/DRT preflight finds no scanned-library fallback for any
selected function. The accepted subset moves **27 exact mappings** into
the actual shipped drt.h declarations: sixteen drt.dll, nine drtprov.dll
and two drttransport.dll. All prototypes, existing SAL/availability facts
and DLL spellings remain unchanged; the production scraper carries each
native ImportLibrary annotation. libMappingsManual.rsp drops exactly those
27 rows, from 385 to 358. The 26,376 scan-derived rows remain untouched.

The other **145 P2P/P2PGRAPH mappings are retained**, not declared unsupported
by the reader. p2p.h is absent from the pinned SDK and has no current IDL or
AdditionalHeaders owner. SDK preparation explicitly restores this deprecated
header from origin/main after resetting the generated-header directory;
editing only this branch's regenerated header would therefore lose the
annotations. The authoring attempt stopped before any writes when its SDK
origin was missing. No fake 28000 patch, older-SDK copy or preparation-tool
change was introduced. A separately qualified compatibility-header source
and provenance decision is needed before claiming that retirement.

The required two-owner P2p/Identity refresh takes 1m06.06s and emission
4m47.96s. The actual candidate is byte-identical to the accepted file/PE
candidate (`6751041E...`), not just comparer-equivalent: the unmasked parent
comparison is zero and complete raw output remains `1F4F3839...`.
Integrity passes 54/54 and import controls 38/38; all 352 SDK patches replay
exactly and the changed native header preserves its parent byte chain.
Enum, autoTypes, fixup and original parameter-atom counts do not change.

## File, memory and PE subdivision

The 24-group trial first exposed a real definition dependency:
WinInet/WinProg see the InternetCrackUrl association, but not its
WIN_HTTP_CREATE_URL_FLAGS definition in winhttp.h. A metadata-Clang-only
include of the actual owning winhttp.h at WinInet's declaration boundary
resolves the parse on all targets without copied constants or new helper
syntax. Ordinary SDK compilation does not gain that include. The resulting
whole-union output, not merely that successful parse, qualifies the dependency.

The next emission exposed a distinct problem: relocating architecture-split
IMAGE_OPTIONAL_HEADER_MAGIC into Backup.headerEnums.modified.cs leaves
SupportedArchitecture without its metadata namespace import (CS0246).
The native IMAGE_NT_OPTIONAL_HDR_MAGIC operand is 267 on x86 and 523 on
x64/ARM64; the complete definition/use closure remains restored. Neither
the missing import nor architecture-specific output is hidden or patched
in generated files.

The accepted **23 groups / 307 members / 84 active selectors** remove
23 JSON records and add 23 namespace routing rows. Five selectors have
alternate native declarations/inline forms, so the exact source contains
**89 association carriers**, not 89 distinct API uses. Each alternate
prototype is checked against its unchanged parent after removing only the
new association annotations. The old SetNamedPipeHandleState::dwPipeMode
selector is absent from both native declaration and baseline argument;
its removal is one inactive selector, not migrated API coverage.
Cumulative retirement above 85444 is **585 definitions / 5,260 members /
1,146 active uses**, with thirteen inactive selectors separate.

The actual WinMD is
`6751041E87BE494E6FBD84962368F185A8EAB27490A2AA3A7E272E6C46DA2A86`
(24,174,592 bytes). The unmasked comparison against actual b482 is zero;
the complete 35,379-type dump remains exact `1F4F3839...`. Integrity passes
54/54, all 352 SDK patches replay exactly, and all 78 loaded native owners
preserve BOMs/non-ASCII byte chains.

Import-library controls initially report 37 pass / 1 failure: the historical
whole-header delta assertion observes unrelated accepted HtmlHelp enum
annotations. Only that historical control's header and manual-RSP inputs
now use the preserved recovery fixture; all assertions and the separate
current-header complete-owner emission controls remain. The corrected
suite passes 38/38. A concurrent test-build copy-lock failure is separately
retained; the successful run builds only the changed test project after
integrity exits, without rebuilding unchanged dependencies.

## Security and system subdivision

Of 102 preflighted groups, MENU_ITEM_FLAGS was deferred before source writes
because its OLEVERB field requires an owning-IDL closure. The actual 101-group
trial found an unavailable CONFIGFLAG_NETBOOT_CARD operand and RASENTRYA/W
macro-expanded field-carrier expressions. The configuration-flags closure
is restored; using the SDK's actual injected tagRASENTRYA/W record names
corrects the carrier input without changing the reader.

The remaining 100 groups parsed across 65 owner/consumer partitions, but
ENUM_PROCESS_MODULES_EX_FLAGS and its four LIST_MODULES_* members were
duplicated across PsApi1/PsApi2 at emission. Restoring that complete closure
allowed a 99-group output, whose full comparison then exposed:

- Four additional dependent globals, including NCRYPT_RNG_OPERATION.
- Five lost MODIFIERKEYS_FLAGS interface associations/nominal typings. These
  qualified method selectors are active IDL uses, not inactive free functions.
- Two CryptXML native values differing from their legacy JSON values.
- Ten integer bitfield backing fields changing to the genuine BOOL wrapper.

Seven enum closures and BOOL's original `int` / `NativeTypedef: true` selection
were restored. A fresh 65-partition evaluation of the resulting 92 groups
removed every difference except NCRYPT_RNG_OPERATION. Its native definition is
an alias of BCRYPT_RNG_OPERATION, so the complete BCRYPT_OPERATION closure
also remains restored alongside NCRYPT_OPERATION. No added global is masked.

The accepted **91 groups / 511 members / 179 active uses** remove exactly
91 JSON records and add 91 namespace routing rows. There are zero inactive
selectors in this cohort. Cumulative additional definition retirement above
85444 is **562 groups / 4,953 members / 1,062 active uses**, with twelve earlier
inactive selectors still separate. The RSP count rises by 91 routing rows;
these are configuration, not new semantic annotations or a retirement claim.

The actual WinMD is
`4E3243C8E09F01732AAC577C080A0DC370A2D07CB620A153DD11B31DE21FA70C`
(24,174,592 bytes). Its unmasked comparison against the actual 2f099 output
reports no differences, and its complete raw declaration dump is the same
`1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA`.
Integrity passes 54/54 and native SDK controls pass 23/23. All 352 selected
post-MIDL patches replay to the exact consumed headers; 60 loaded native
owners preserve their parent byte chain, BOMs and non-ASCII lines.

The final dependency subdivision reuses the completed 92-group raw cache:
all other native sources differ only in nonsemantic include-guard labels.
Foundation, Identity and Security.Cryptography are refreshed for the restored
BCrypt closure, followed by fresh production emission and full comparison.
No generated RSP is edited, nor is a partial or failed candidate accepted.

Focused controls additionally reproduce BOOL's MENUBARINFO bitfield backing
change on all three targets (six successful scrapes). Actual CreateFontW
and GetThemeInt native controls pass on all three targets; their associated
byte FONT_CHARSET and unsigned THEME_PROPERTY_SYMBOL_ID variants each reject
storage conflicts on all three targets. Only those two exact parameter
selectors are individually reproduced; all production parameter facts remain.
The current contract requires associated storage to match its group.

### Generation cost qualification

The retained full recovery run took 12m32.63s, versus 14m46.57s scraping plus
4m45.44s emission (19m32.01s) for the later fresh complete 91-group run:
an observed **6m59.38s / 55.7% increase**. These are not controlled matched
runs and do not establish attribution to annotation processing. The recorded
command/concurrency/cache context remains evidence, not a new benchmark:
the recovery used MaxDegreeOfParallelism 8 on 32 processors. The shared-RPC
filtered cycle took 6m10.90s, C-header64 took 6m45.59s, and this final
three-partition dependency refresh/emission took 1m06.55s + 4m43.83s.
Filtered refreshes are not comparable to full generation. Broad-generation
cost remains a qualification concern; no timeout, concurrency or tooling
change is used to conceal it.

| Retained evaluation | Scope / cache | Scrape/constants | Emission | Total |
|---|---|---:|---:|---:|
| Recovery | Complete all-architecture, empty output | Combined pipeline | Included | 12m32.63s |
| Earlier fresh full91 | Complete all-architecture, empty output | 14m46.57s | 4m45.44s | 19m32.01s |
| Security/system92 subdivision | 65 owner/consumer partitions, existing raw cache | 5m12.93s | 4m53.90s | 10m06.83s |
| Accepted security/system91 | 3 dependent partitions, qualified 92 raw cache | 1m06.55s | 4m43.83s | 5m50.38s |
| Accepted file/memory/PE23 | 17 owner/consumer partitions, accepted security/system91 raw cache | 2m25.07s | 4m55.34s | 7m20.41s |
| Accepted DRT27 import overrides | P2p/Identity, accepted file/PE23 raw cache | 1m06.06s | 4m47.96s | 5m54.02s |

These are retained required evaluations, not a separate benchmark loop.
Future accepted checkpoints retain both stage times and scope/cache context;
an upward trend in comparable runs needs attention even while the user
accepts the current observed cost.

## Shared RPC and security subdivision

The actual 13-group shared-header trial parsed, but could not emit:
FACILITY_CODE retirement exposed CS0229/CS0119 ambiguity/type-expression
errors for FACILITY_OPC and other FACILITY_* references in dependent HRESULT
constants. The failed source and compiler log remain preserved; no WinMD or
equivalence is claimed for that union. The full FACILITY_CODE closure,
157 members, and its original winerror.h/recipe state are restored.

The other twelve groups now own 84 members and 41 active associations in
shared/rpcdce.h and shared/sspi.h: RPC QoS capabilities/identity, HTTP
flags/authentication target, binding-handle options, security credentials,
export-context/package options, and ISC/ASC request/high flags. All twelve
JSON records are deleted, with twelve namespace routing rows added; routing
rows are not retired annotations. There are no inactive selectors in this
cohort. Cumulative additional definition retirement above 85444 is now
471 groups / 4,442 members / 883 active uses, with twelve earlier inactive
selectors still counted separately.

Production scraping/emission, the unmasked parent comparison and the entire
35,379-type raw dump preserve all API facts. The actual candidate is
`FF7172753D5A07B6BEC7CA6C6695F0B4B17C12A0BC7CAEB0350E3146808D5F24`;
integrity passes 54/54. All 348 selected post-MIDL patches replay to the
exact consumed headers; both changed owners preserve their original BOM
and non-ASCII lines. No production tooling changes or generated-RSP edits
are used.

## Native primitive storage subdivision

The three-selection BOOL/BOOLEAN/COLORREF trial emitted, but its complete
unmasked comparison changed two genuine callback types from `Byte*` to
`BOOLEAN*`: `CrediUnmarshalandDecodeStringFn.Invoke::IsFailureFatal` and
`PKSEC_DEREFERENCE_LIST_ENTRY.Invoke::Delete`. This is direct native-alias
propagation, not evidence that BOOLEAN is fictitious, nor the earlier global
callback aggregation failure. BOOLEAN's original byte storage and
`NativeTypedef: true` remain restored.

The then-retained two-selection union replaces BOOL's `int` and COLORREF's
`uint` storage with `native`, removing only their redundant `NativeTypedef`
flags. Actual native declarations still produce both genuine types. No
autoTypes record, parameter atom or RSP row is removed by this cohort.
Production emission and its unmasked parent comparison preserve the entire
35,379-type declaration dump byte-for-byte (`1F4F3839...`); the actual new
WinMD is `CE20171725434A9B37CBDE44A3982860CCFA2D9A26F66965E065860459F9D56C`.
Integrity passes 54/54. All 23 native SDK controls pass after binding the
eleven-selection control to its preserved fc60 input; its assertions remain
unchanged. The earlier failed control runs are retained, not counted as passes.

This was a bounded four-partition qualification. The later security/system
refresh exposes BOOL bitfield backing-type changes outside that coverage;
the current accepted union restores BOOL, retaining only COLORREF from
this two-selection trial. The historical checkpoint and its evidence remain.

## Retained sidecars

These are the counts retained by this conservative working set, **not measured
minimums**:

| JSON input | Qualified parent | Red experiment | Recovery |
|---|---:|---:|---:|
| `autoTypes.json` | 444 | 8 | 444 |
| `enums.json` items | 1,070 | 0 | 1,070 |
| `functionPointerFixups.json` | 27 | 27 | 27 |

All 69 parent manual C# inputs are retained. Across 332 RSP files, the recovery
retains all **146,789** parent non-comment value records, including 90,502
documentation mappings and 26,376 import-library mappings/configuration rows;
these are not 146,789 semantic annotations. The red experiment retained 35,900
total RSP rows; its 3,320 explicitly inventoried bridges were only a subset.
Relative to the red experiment, 110,913 records are
restored and 24 experiment-only records are removed. A value record is one
nonempty value under its option section, not necessarily one distinct API fact.

| RSP file | Retained records | Restored relative to red |
|---|---:|---:|
| `documentationMappings.rsp` | 90,502 | 90,502 |
| `libMappings.rsp` | 26,376 | 0 |
| `supportedOS.rsp` | 17,070 | 13,935 |
| `WithSetLastError.rsp` | 3,362 | 3,362 |
| `emitter.settings.rsp` | 2,503 | 2,065 |
| `ConstantsScraper.settings.rsp` | 614 | 465 |
| `libMappingsManual.rsp` | 385 | 382 |
| `scraper.settings.rsp` | 720 | 66 |

[recovery-sidecar-counts.tsv](recovery-sidecar-counts.tsv) provides the complete
parent/red/recovery retained, restored, and removed counts **by file and option**.
The enabled [patch list](recovery-enabled-patches.txt) contains the unchanged
parent's 37 post-MIDL and 23 selected pre-MIDL patches. The generation subtree is
exactly parent tree `95ee9dc0c79ba9b191f8080d458b788a9b2ea366`.

The operand-ordering failures cannot be repaired by RSP metadata alone; the broad
incompatible header additions are therefore disabled with the rest of the broad
input batch, not accepted through weaker parsing. Generic GDI policy, missing
owned facts, legacy comment transport, and native-MIDL gaps remain follow-up
work. No known-difference masks, skipped assertions, parser relaxations, or
timeout increases are introduced.

Three repros that previously read enabled experimental headers now read frozen
source snippets in `FullHeaderMigrationExperimentFixtures.h`. Their exact
non-whitespace source tokens were checked against the preserved experiment
commit. All semantic assertions remain; changing production input policy no
longer removes the repro's input.

## Newly generated output and API comparison

The existing production `ScrapeHeaders` / `ScrapeConstants` / `EmitWinmd`
pipeline succeeded on all architectures in **12 minutes 32.63 seconds**, with
zero errors and eight warnings. It used a new empty `ObjDir`; no stale generated
sources or parent WinMD were substituted for this output.

| Output | Identity |
|---|---|
| Local run | `working-recovery`, 2026-11-10 18:43:40Z through 18:56:14Z |
| New WinMD | `Windows.Win32.winmd`, 24,177,152 bytes |
| New WinMD SHA-256 | `18996DF3292B4035F3A0512882985BA5A1A69703346E767A97DE23125A6C35E9` |
| Qualified parent reused, not rebuilt | ordinary run `42133942944`, attempt 1 |
| Parent WinMD SHA-256 | `78B3D19EEB7793FB953F2FEFAEE029081520466452F6DA83747674C14A6DE3A2` |
| Both complete API dump SHA-256 values | `C18E31F0C0BFF9FEC72477DD4D539766806E4B282A2ADD1C285E5555F99C4D36` |
| Measured API declaration differences | **0** |

`WinmdUtils compare` exits zero with no known-difference file. The complete
sorted API dumps are byte-identical with both files named `Windows.Win32.winmd`;
no declaration filtering or text normalization is used. Comparing a differently
named copy initially showed only the dump's source-filename comment. The new
binary is not byte-identical to the qualified parent, and is not represented
as such.

The output is retained in the producing session's
`files\working-recovery\Windows.Win32.winmd`, with `production.log`,
`production.binlog`, generated sources, TRX files, and unmasked comparison
outputs. A copy of the **new** output is also at workspace
`bin\Windows.Win32.winmd` for existing integrity tests.

The production command was:

```powershell
. scripts\CommonUtils.ps1
Install-VsDevShell
dotnet build generation\WinSDK -c Release -t:EmitWinmd --no-restore -p:ScanArch=crossarch -p:ObjDir=<new-output>\obj -p:OutputWinmd=<new-output>\Windows.Win32.winmd -p:MaxProcessors=8
```

The build warnings describe existing cross-partition remap visibility and
partitions with no cross-architecture differences. They were not hidden or
"fixed" by restoring unsupported enum/handle annotations. This is a conservative
working anchor, not proof that the broad header migration has become compatible.

## Selected checks

| Suite | Passed | Failed | Skipped |
|---|---:|---:|---:|
| `MetadataUtils.Tests` | 17 | 0 | 0 |
| `Win32MetadataScraperTests` | 52 | 0 | 0 |
| `ClangSharpSourceToWinmdTests`, existing non-SDK selector | 702 | 0 | 0 |
| `Windows.Win32.Tests`, against the new output | 54 | 0 | 0 |

All eleven migration repros/controls remain in the passing emitter suite.
The existing non-SDK selector is
`FullyQualifiedName!~ClangSharpSourceToWinmdTests.SdkPatchTests`; the separate SDK
patch-validation suite was not rerun. This reports local production generation
and local test results, not a recovery CI result. The failed automatic run
`42691434230` belongs to the preserved red experiment at `88991baf`, not this
working input set.

## Incremental reduction: first parameter checkpoint

The user subsequently authorized measured retirement above the working recovery
`a9a5406f7a52b144094d2d6c9972907bdaa2540d`. The recovery quantities above remain
the historical conservative anchor, not the current reduction target.

The first accepted parameter batch removes **395 atomic parameter facts** and
**332 complete value rows** from `emitter.settings.rsp` (2,503 to 2,171 rows).
Across all parameter remaps, 770 attribute atoms become **375**, and 694
attribute-bearing rows become 362. A bracket containing two attributes counts
as two atoms, not one. Enum associations, cleanup policy, and negative fidelity
overrides are included in those 375 remaining atoms; they are not all SAL gaps.
Documentation, library mappings, and parser configuration are unchanged.

| Native parameter family | Anchor atoms | Removed | Retained |
|---|---:|---:|---:|
| `ContainsInteriorPointers` | 116 | 116 | 0 |
| `Const` | 93 | 92 | 1 |
| `Optional` | 82 | 53 | 29 |
| `In` | 67 | 57 | 10 |
| `ComOutPtr` | 61 | 11 | 50 |
| `Reserved` | 33 | 16 | 17 |
| `Out` | 26 | 19 | 7 |
| `Retained` | 17 | 17 | 0 |
| `NativeArrayInfo` | 15 | 1 | 14 |
| `RetVal` | 14 | 13 | 1 |
| `MemorySize` | 2 | 0 | 2 |
| **Subtotal** | **526** | **395** | **131** |

The nine accepted CLR exception selectors retain ten atoms within that subtotal.
The other 244 remaining parameter atoms are enum associations, ownership/freeing
policy, negative overrides, and two conditional-return facts. No entire parameter
file is retained as an unexplained exception.

Changes use native SAL, native const input types, and the current
`ContainsInteriorPointers`, `Retained`, and retval contracts. DXGI retval changes
are authored in three pre-MIDL IDLs and carried through actual MIDL outputs.
No reader, helper, converter, or CI changes are part of this batch.

The whole-category deletion trial emitted but lost API facts and was rejected.
Owner/fact-closed subdivision then rejected native Reserved/Optional changes
that also changed direction or optionality, retaining their exact old remaps.
Whole-owner WMI and TSF regeneration re-exposed unrelated native `[unique]`
optionality; those two owners were backed out rather than accepting unrelated
API changes. A TSF `_Out_opt_` input also displaced its existing array extent;
it was not accepted as a successful optionality migration.

Actual cross-architecture scraping/emission, followed by refreshing the four
changed rollback partitions and re-emitting the complete union, succeeded.
The new output at `files\sidecar-reduction\parameter-accepted\Windows.Win32.winmd`
has SHA-256
`18996DF3292B4035F3A0512882985BA5A1A69703346E767A97DE23125A6C35E9`.
The existing complete, unmasked comparer exits zero; the entire declaration dump
is byte-identical to `C18E31F0C0BFF9FEC72477DD4D539766806E4B282A2ADD1C285E5555F99C4D36`.
This is a newly emitted checkpoint, not a copied reference. It is not a claim
that all parameter metadata has moved, nor a mathematical minimum. Further
one-for-one batches and enum/handle trials follow above this checkpoint.

## Accepted parameter and function-prefix reduction

The next accepted union removes **407 of 770 parameter attribute atoms**,
leaving **363**. Twelve additional `ComOutPtr` transports survive subdivision:
seven in four independently clean native-IDL owners and five in directly
consumed declarations. Regenerating the other COM owners changed unrelated
`[unique]` optionality or lost existing `ComOutPtr`/direction/Optional facts;
those complete owner closures were restored, not counted as migrated.

| Response-file input | Recovery rows | Accepted retained | Removed |
|---|---:|---:|---:|
| `emitter.settings.rsp`, all options | 2,503 | 2,159 | 344 |
| `WithSetLastError.rsp` | 3,362 | 468 | 2,894 |
| `supportedOS.rsp` | 17,070 | 7,883 | 9,187 |
| **All RSP value rows** | **146,789** | **134,364** | **12,425** |

Documentation (90,502), automatic library mappings/configuration (26,376),
manual library mappings, and parser/global configuration are unchanged.
The [per-file/option inventory](reduced-sidecar-counts.tsv) and
[individual retained parameter facts](retained-parameter-facts.tsv) distinguish
the nine CLR exception selectors, explicit negative overrides, enum
associations, cleanup/lifetime contracts, and unsuccessful positive-annotation
input trials. There are 119 remaining positive parameter atoms, including the
ten CLR atoms, and 244 other parameter atoms; these are not 363 independent
reader defects.

The prefix trial initially retired 2,935 LastError and 9,749 availability rows.
Native prefixes were relocated before complete declarations, export/SAL
prefixes, and complete preprocessor continuations. Original BOM positions and
legacy bytes were preserved. Seven owner closures then required 391 availability
rows restored: incomplete overload sets, inline/unprojected functions, and a
flattened alias had no independently projected target. A shared `WSARecvEx`
prefix had to precede its deprecation attribute and return-type conditional.

The first emitting prefix union was **rejected** after the complete comparer
reported 73 discrepancies. PSAPI/K32, CM/CMP, and DDQuery/GdiEntry macro aliases
received unintended facts; other functions lost facts; a Direct2D enum became
loose constants. Exact failing facts and the conservative alias/Direct2D owner
closures were backed out. The Direct2D mechanism is not established by this
experiment and is not reported as a demonstrated reader bug. No unexpected API
change was accepted or masked.

After refreshing the 14 affected production partitions (1 minute 17.85 seconds)
and emitting the **complete** existing generated union (4 minutes 47.05 seconds),
the unmasked comparison exits zero. The newly emitted
`files\sidecar-reduction\prefix-accepted\Windows.Win32.winmd` is 24,177,152 bytes,
SHA-256 `18996DF3292B4035F3A0512882985BA5A1A69703346E767A97DE23125A6C35E9`;
its complete declaration dump remains
`C18E31F0C0BFF9FEC72477DD4D539766806E4B282A2ADD1C285E5555F99C4D36`.
The original full production and rejected comparison are retained alongside it.
This is the working boundary before bounded enum/handle trials, not a fresh
reference build or a claim that the remaining sidecars are minimal.

Historical whole-corpus/whole-header assertions now read the reachable frozen
recovery inputs. The assertions themselves remain intact; live semantic,
patch-replay, and native ABI checks still exercise current input. The printing
ABI fixture explicitly models the current `LPCWSTR` pointee constness rather
than hardcoding its former mutable spelling.

## Final accepted enum/handle union

The bounded native-enum cohort attempted 34 definitions. Five complete groups
were restored after native-input, emitter, or API failures; two further
type/name closures were deferred before attempting native generation. The
accepted union moves **29 definitions with 274 members** into ten owned headers,
using the current unscoped enum/flags contracts. It removes those exact JSON
definitions and adds **29 namespace bridges**, not 29 additional API facts.

The eight new native selections are `BCRYPT_HANDLE`, `BCRYPT_ALG_HANDLE`,
`BCRYPT_KEY_HANDLE`, `BCRYPT_HASH_HANDLE`, `BCRYPT_SECRET_HANDLE`,
`NCRYPT_PROV_HANDLE`, `NCRYPT_HASH_HANDLE`, and `NCRYPT_SECRET_HANDLE`.
Eight legacy storage fields, eight `NativeTypedef` fields, and five
`AlsoUsableFor` fields are replaced by native storage/identity/associations.
The association marker precedes the complete typedef, as in the qualified
existing native-key pilot; the first attempted after-`typedef` placement was
an input-authoring error and was corrected, not classified as a reader defect.
All explicit selection/identity records, cleanup, and invalid-value policy
remain. Five explicit namespace fields preserve the original alias owners.

| Input/fact category | Recovery | Final retained | Removed or replaced |
|---|---:|---:|---:|
| Parameter attribute atoms | 770 | 363 | 407 |
| Positive parameter atoms within that total | 526 | 119 | 407 |
| `emitter.settings.rsp`, all value rows | 2,503 | 2,159 | 344 |
| `WithSetLastError.rsp`, `--with-setlasterror` | 3,362 | 468 | 2,894 |
| `supportedOS.rsp`, `--with-attribute` | 17,070 | 7,883 | 9,187 |
| `requiredNamespacesForNames.rsp`, `--requiredNamespaceForName` | 2,462 | 2,491 | **29 added bridges** |
| **All RSP value rows** | **146,789** | **134,393** | **12,396 net removed** |
| `enums.json` definitions | 1,070 | 1,041 | 29 definitions / 274 members |
| `autoTypes.json` records | 444 | 444 | No record retirement |
| Legacy storage fields within `autoTypes.json` | 441 | 433 | 8 native replacements |
| Native selections within `autoTypes.json` | 3 | 11 | **8 selections added** |
| `NativeTypedef` fields within `autoTypes.json` | 418 | 410 | 8 native replacements |
| `AlsoUsableFor` fields within `autoTypes.json` | 15 | 10 | 5 native replacements |
| `CloseApi` / `InvalidHandleValues` fields | 153 / 231 | 153 / 231 | 0 |
| `functionPointerFixups.json` records | 27 | 27 | 0 |

These mixed units must not be summed as a semantic-fact count. The current
[file/option inventory](reduced-sidecar-counts.tsv) covers every RSP file, and
the [retained parameter inventory](retained-parameter-facts.tsv) labels all 363
remaining atoms individually. Documentation (90,502), automatic library
mappings/configuration (26,376), manual mappings, parser/global configuration,
and 69 manual C# inputs are unchanged. The final recipe selects **339 post-MIDL
and 28 pre-MIDL patches**, a precise set of parameter/prefix/enum/alias changes,
not blanket re-enablement of the original 737-patch experiment.

### Actual production and complete API accounting

The fresh complete cross-architecture 31-enum/eight-alias production run
`enum-handle-final-31` emitted in **17m32.75s**, but was **rejected**: two
`verrsrc.h` groups disappeared and five members became loose constants.
Restoring only those two groups and their native owner, refreshing production
`Fs` plus the constants stage (**59.52s**), and emitting the complete generated
union (**4m53.61s**) produced the accepted 29-group/eight-alias output.
The rest of the fresh production sources were retained; no reference rebuild,
parent-output substitution, or successful-compilation-only acceptance occurred.

| Final output | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\enum-handle-accepted\Windows.Win32.winmd` |
| Size | 24,177,152 bytes |
| SHA-256 | `3A228811B533CCB03A965AFBB77B55380C784943FDD7A5CD7E43FB0087B09F4E` |
| Reused qualified parent | `78B3D19EEB7793FB953F2FEFAEE029081520466452F6DA83747674C14A6DE3A2` |
| Complete final declaration dump SHA-256 | `CD5C0E0EF48C1784D5C6ADAFE8BCF8BE6FDB3F69D7CB779BFF65E712E1A96FCC` |
| Unmasked comparer against that exact parent | Exit **0**, no unknown differences |

The complete dump is **not byte-identical** to C18. Its exhaustive raw diff has
exactly five hunks: `AlsoUsableFor` moves after `InvalidHandleValue` and
`NativeTypedef` on `BCRYPT_ALG_HANDLE`, `BCRYPT_HASH_HANDLE`,
`BCRYPT_KEY_HANDLE`, `BCRYPT_SECRET_HANDLE`, and `NCRYPT_PROV_HANDLE`.
Native transport changes attribute emission order; the same five association
values and all other declaration text remain. No type, member, signature,
storage, namespace, or attribute fact changes. The raw diff is retained as
`enum-handle-accepted-api.diff`; no sorting/normalization, filters, or known-diff
mask was used to force equality. Parameter/prefix-only checkpoints remain
byte-identical to C18.

All **339/339** selected post-MIDL patches replay through the unchanged strict
production patcher to byte-exact current headers. The final binary-safe audit
preserves raw non-ASCII byte sequences and introduces no replacement bytes in
335 changed native owners. Fresh utility, scraper, and new-output integrity
runs pass **17, 52, and 54** cases, respectively.

The complete existing non-SDK emitter run initially passed 689 and failed 13
of 702 cases. Its failures were historical snapshots, incomplete live replay
selection, and SDK/PCH pilot controls coupled to migrating dependencies.
After making the controls self-contained and including the actual heap patch
in live replay, a 24-case affected run passed 18 and failed six; the final six
SDK/PCH cases then all passed on x86/x64/ARM64. Every original failing case has
an actual successful corrected rerun. This is coverage across full and targeted
runs, **not a claim of a subsequent single green 702-case run**.
The scope control retains the five frozen reviewed handle-header facts, real
SDK-package dependencies, the current real helper, complete byte equality, and
every scope/leakage assertion. Other current-input production/ABI/replay tests
remain live. No assertion was removed, weakened, or skipped.

The logs, binlogs, generated sources, TRX files, full raw dumps/diff, rejected
trials, and exact API accounting are retained in the producing session's
`files\sidecar-reduction`. The new accepted output is also copied to workspace
`bin\Windows.Win32.winmd`. Tooling/helper/converter/CI trees and all lower stack
layers remain unchanged. The separate full SDK-validation job was not rerun;
its original budget/cost limitation remains unresolved, without a timeout raise.

## Residual boundaries and narrow next steps

These are input/policy boundaries unless a self-contained current-reader repro
establishes otherwise. Compilation is not evidence that a replacement preserved
the API; unsuccessful native-input trials retain their exact sidecar facts.

| Boundary | Demonstrated evidence | Narrow follow-up |
|---|---|---|
| Parameter direction, Reserved and optionality | Native Reserved implies Optional; some input annotations also added In. Whole WMI/TSF and several COM owners re-exposed unrelated native unique-pointer facts. | Author fact-closed native IDL/header changes, retaining unrelated projection overrides; compare complete method/owner contracts before changing SAL/converter behavior. |
| Remaining parameter facts | 363 atoms remain: 119 positive atoms (ten in the nine accepted CLR exception selectors), 117 enum bindings, 56 negative overrides, 69 cleanup/lifetime facts, and two conditional-return facts. | Use the individual retained-fact inventory and its baseline/native-trial arguments. Definition-only enum or handle identity migration does not replace use-site enum binding or cleanup policy. Additional CLR callbacks are not silently included in the nine-selector exception. |
| Macro-renamed exports and prefix target closure | Native PSAPI/K32, CM/CMP and DDQuery/GdiEntry declarations changed which projected export received the fact. Inline/flattened targets and incomplete overload sets rejected annotation binding. | Add focused export-name/overload/use-site controls to the relevant prefix reader layer before retiring those precise restored rows; distinguish projection policy from stale annotation placement. |
| Direct2D enum-shape discrepancy | The rejected prefix union lost one enum and surfaced its eight members as loose constants. Restoring the conservative Direct2D input closure recovered complete equality. | Isolate the responsible native input before attributing it to a reader or expanding a lower layer. |
| Native enum operands | `SETUP_DI_DEVICE_CONFIGURATION_FLAGS` needs absent `CONFIGFLAG_NETBOOT_CARD`; evaluating SDK `WTS_SECURITY_CURRENT_USER_ACCESS` fails because its expression lacks OR operators. | Keep these two complete legacy groups until authoritative native operands are established. Do not invent a native value or resurrect a removed SDK surface to make generation green. |
| Native enum type/name closure | `DEVPROPTYPE` already has native typedef/storage/self-cast coupling; `CFE_UNDERLINE` collides with a public macro. | Qualify those complete type/use-site closures separately rather than accepting arbitrary storage, namespace or member loss. |
| Native enum/constants integration | Native `FACILITY_CODE` parses, but composed HRESULT constants reach emission with conflicting `Apis.FACILITY_*` type/value aliases (`CS0229`/`CS0119`, 1,062 repeated diagnostics). | Keep this exact JSON group; isolate the enum/constant composition boundary in a small fixture before changing the producer/emitter layer. These are not 1,062 independent bugs. |
| Version-resource enum projection | The first emitting bounded cohort lost `VS_FIXEDFILEINFO_FILE_TYPE` and `VS_FIXEDFILEINFO_FILE_SUBTYPE`; five subtype values appeared as loose constants. | Both exact groups and their `verrsrc.h` owner are restored. Isolate traversal/constant ownership before assigning a reader cause; the other 29 groups remain accepted. |
| Genuine handles versus ownership | Eight compiler-backed alias selections can replace eight storage and eight NativeTypedef fields plus five association fields. Explicit native selection, cleanup and invalid-value policy remain separate. | Expand genuine-alias cohorts with exact ABI/API controls; do not infer generic GDI cleanup or pointer-sentinel enum policy from native typedef identity. |
| Native-MIDL and validation coverage | The preserved original current-converter returned-pointer/outer-extent repros remain. Unique and split-line QueryStatus controls still distinguish non-reproducing concerns. | Use those current converter method pairs for narrow fixes, not the old source migration script. Full SDK-validation cost remains unestablished; no assertion skip or job-budget increase is justified by this experiment. |

This is a measured working reduction, not a mathematical-minimum claim.
Unattempted enum/handle closures are not classified as unsupported readers.
Documentation, library mappings, global/parser configuration, and the 69 manual
generation inputs remain separate from semantic annotation retirement.

## Producer ownership and enum use-site migration

This bounded batch follows the user's clarification that fictitious handle
removal and enum-to-`AssociatedEnum` use-site changes are intended API changes,
not reasons to reject a migration. It builds above the preserved working
`fc60db128ac08f4009737debc40cd4515e4a837a` checkpoint. Production readers,
helper, native-MIDL converter, CI, and all lower stack layers are unchanged.

### Exact retired facts and native replacements

`avrt.h` now owns the complete two-record projection-only handle closure:

| Retired synthetic type | Native producer replacement |
|---|---|
| `AVRT_TASK_HANDLE` | Four `AvSetMm[Max]ThreadCharacteristicsA/W` returns carry `_Win32_RAIIFree_(AvRevertMmThreadCharacteristics, 0)`. |
| `AVRT_THREAD_ORDERING_GROUP_HANDLE` | Three `AvRtCreateThreadOrderingGroup[ExA/ExW]::Context` outputs carry `_Win32_RAIIFree_(AvRtDeleteThreadOrderingGroup, 0)`; `AvRtJoinThreadOrderingGroup::Context` carries `_Win32_RAIIFree_(AvRtLeaveThreadOrderingGroup, 0)`. |

Both legacy records explicitly had `NativeTypedef: false`. Their two
`autoTypes.json` records and all 14 corresponding emitter `memberRemap` rows
are removed. The APIs use their genuine native `HANDLE`/`HANDLE*` types instead.
Borrowers and cleanup functions acquire no producer-site ownership metadata.
The real `AVRT_PRIORITY` enum remains enum-typed. The global genuine `HANDLE`
record and its legacy policy are not retired by this local closure.

The Join output deliberately no longer inherits the synthetic type's incorrect
Delete policy: [Leave consumes a Join context](https://learn.microsoft.com/en-us/windows/win32/api/avrt/nf-avrt-avrtleavethreadorderinggroup);
[Delete consumes a parent-created group](https://learn.microsoft.com/en-us/windows/win32/api/avrt/nf-avrt-avrtdeletethreadorderinggroup).
[Task handles use Revert](https://learn.microsoft.com/en-us/windows/win32/api/avrt/nf-avrt-avsetmmthreadcharacteristicsw).
The native patch moves the three complete existing cleanup declarations,
including SAL, LastError, availability and partition guards, before producers
so typed operands resolve. It introduces no shadow prototypes or typedefs.
Invalid value zero is retained from the exact retired records.

`PathCch.h` now places `_Win32_AssociatedEnum_(PATHCCH_OPTIONS)` on `dwFlags`
in `PathCchCanonicalizeEx`, `PathCchCombineEx`, `PathCchAppendEx`,
`PathAllocCombine`, and `PathAllocCanonicalize`. The corresponding
`enums.json` binding-only record and its five uses are removed. These native
parameters are `ULONG`; the output now correctly carries `uint` plus
`AssociatedEnum("PATHCCH_OPTIONS")`, rather than the old forced enum type.
The genuine `PATHCCH_OPTIONS` definition and all eight members remain.
This is **not a thirtieth migrated enum definition**.

| Retained input | Preserved `fc60` | Current |
|---|---:|---:|
| Total RSP value rows | 134,393 | 134,379 |
| Emitter `memberRemap` rows | 1,951 | 1,937 |
| `autoTypes.json` records | 444 | 442 |
| `enums.json` records | 1,041 | 1,040 |
| `functionPointerFixups.json` records | 27 | 27 |
| Remaining parameter attribute atoms | 363 | 363 |

The full file/option inventory remains
[reduced-sidecar-counts.tsv](reduced-sidecar-counts.tsv). Cumulatively,
12,410 of the original 146,789 RSP value rows are removed; 407 of 770 parameter
attribute atoms are retired. This phase removes type remaps and JSON uses,
not additional parameter-attribute RSP atoms. It preserves all earlier
29-definition/274-member and eight genuine-alias migrations. The retained
autoTypes fields include 431 legacy storage entries, 11 native selections,
410 `NativeTypedef` fields, ten associations, 151 cleanup fields and
229 invalid-value fields. These are conservative retained facts, not a
minimum or a claim that the remaining records are unsupported.

### Actual production output and complete API accounting

The full cross-architecture `Threading` and `Shell` production partitions and
constants refreshed in **1m20.22s**. Complete union emission then succeeded in
**4m51.58s**, reusing the preserved checkpoint's other generated partitions,
not its WinMD. No full reference build or hosted CI rerun was performed.

| Actual new output | Identity |
|---|---|
| WinMD | `files\sidecar-reduction\structural-candidate\Windows.Win32.winmd` |
| Size | 24,177,152 bytes |
| SHA-256 | `A7A81FE6B1AE95E1F08AE6BD1D504F4FDC456502A432179EFED76D548A0FFFFF` |
| Complete raw declaration dump SHA-256 | `7F64C4FA510A84E2C4A2F807552931B2301A9FE0604CA794BB901977087E73D4` |
| Actual unmasked comparer against `fc60` and qualified parent | Exit **-1**, reflecting the intended structural differences |

The complete raw diff contains exactly two removed synthetic type blocks,
14 AVRT signature substitutions, eight producer ownership/invalid-value
groups, five PATHCCH native-storage/association changes, and declaration-order
movement of the three cleanup methods. Exact before/after blocks for all
19 changed methods are checked against these source facts; removing those
individually checked blocks and the exact two retired types leaves all
remaining declaration text equal. There are **zero unmatched changes**.
Against the original qualified parent, the previously recorded five harmless
`AlsoUsableFor` attribute-order changes also remain. Direction, extents,
optionality, pointer depth, native enum definitions, imports, LastError,
availability, and documentation otherwise remain unchanged.

The original unmasked comparer results and full dumps/diff are retained.
`structural-api-accounting.json` records exact old/new blocks and native
owners; this accounting does not change the comparer exit, filter APIs,
introduce a known-difference file, or relax acceptance for unmatched loss.

All **174** directly related ownership/enum controls pass, including the
12 new x86/x64/ARM64 semantic and C/C++ ABI cases. All **54** integrity cases
pass against the actual new output copied to `bin\Windows.Win32.winmd`.
All **339/339** selected post-MIDL patches replay strictly to byte-exact
current headers, and the binary-safe audit preserves non-ASCII bytes in
335 changed native owners. Recipe selections remain 339 post-MIDL and
28 pre-MIDL patches; only the two owning post-MIDL assets changed here.

No new reader blocker was demonstrated by this bounded batch. Its initial
new-fixture failures came from reading the unannotated SDK package rather
than the current production header and from missing ABI-only include/SAL
setup; those fixture inputs were corrected without weakening assertions.
The complete production parse, semantic controls and exact API accounting
then passed. The earlier residual input/reader boundaries remain; unattempted
handle/enum closures are not reclassified as failures. Future cohorts should
continue separating genuine native identity from producer ownership and
genuine enum definitions from native-integer use-site associations.

## HTTP, filtering, printer and signed-enum cohorts

These cohorts build above preserved
`5e7a63a0831bc565896e9150df238b6f895d9fa3`, not a regenerated reference.
They retire projection-only records/use-site overrides rather than merely
changing genuine native alias storage fields.

### Three exact native handle closures

| Retired record | Native owner and producer | Retired emitter remaps |
|---|---|---:|
| `HTTP_REQUEST_QUEUE_HANDLE` | `http.h`: `HttpCreateRequestQueue::RequestQueueHandle` carries `_Win32_RAIIFree_(HttpCloseRequestQueue, 0)` on native `PHANDLE`. | 2 |
| `FWPM_ENGINE_HANDLE` | `fwpmu.h`: `FwpmEngineOpen0::engineHandle` carries `_Win32_RAIIFree_Handle_(FwpmEngineClose0)` on native `HANDLE*`. | 185 |
| `FINDPRINTERCHANGENOTIFICATION_HANDLE` | `winspool.h`: `FindFirstPrinterChangeNotification` carries `_Win32_RAIIFree_Handle_(FindClosePrinterChangeNotification)` on its native `HANDLE` return. | 3 |

All three records explicitly had `NativeTypedef: false`, but that flag alone
was not the classification: the actual owned declarations use `HANDLE` or
`PHANDLE`, not these synthetic names. All 185 engine selectors were checked
against their native `HANDLE engineHandle` declarations in the same header.
Cleanup functions retain their exact native signatures and existing SAL;
the complete declarations move before producers for operand visibility.
No new one-argument shim, shadow declaration or typedef is introduced.
The exact retired invalid-value policies remain at producers: HTTP zero;
engine and printer notification minus one and zero.

There are **190 removed remap rows but 189 live signature sites**.
`FwpmConnectionSubscriptionsGet0::engineHandle` is an inactive legacy selector:
its native declaration exists, but no method is emitted in either preserved
5e7 or the new candidate, and neither current import mapping file contains it.
Removing this dangling row is not claimed as migrated API coverage or a newly
lost API. Full accounting explicitly requires it to remain absent in both
outputs; its broader projection/export cause is not repaired here.

Legacy `HttpCreateHttpHandle` is a distinct control: the native header specifies
`CloseHandle`, not `HttpCloseRequestQueue`. It receives no newly authored
ownership metadata from the request-queue cohort. The genuine global `HANDLE`
policy and the still-retained `PRINTER_HANDLE` closure are unchanged.

### Five signed native-integer enum bindings

The four `MsiConfigureProduct[Ex]A/W::iInstallLevel` sites in `Msi.h` now carry
`_Win32_AssociatedEnum_(INSTALLLEVEL)` on native `int`. `CommCtrl.h` places
`_Win32_AssociatedEnum_(_LI_METRIC)` on `LoadIconMetric::lims`, also native
`int`. The exact two JSON binding-only records and five uses are deleted.
Output parameters are now `int` plus `AssociatedEnum`, not forced enum types.
The real three-member `INSTALLLEVEL`, two-member `_LI_METRIC`, and genuine
enum-typed `INSTALLSTATE eInstallState` parameters remain unchanged.
No additional enum definition is counted as retired.

| Retained input | Preserved 5e7 | Current |
|---|---:|---:|
| All RSP value rows | 134,379 | 134,189 |
| Emitter value rows / `memberRemap` rows | 2,145 / 1,937 | 1,955 / 1,747 |
| `autoTypes.json` records | 442 | 439 |
| `enums.json` records | 1,040 | 1,038 |
| `functionPointerFixups.json` records | 27 | 27 |
| Parameter atoms retired / retained | 407 / 363 | 407 / 363 |

Cumulatively, **12,600** original RSP value rows are removed net. The current
total includes the **90,502 allowed documentation mappings**; the other
43,687 retained RSP rows are not granted an exemption by that allowance.
File/option counts remain in the existing TSV. Retained autoTypes fields are
428 legacy storage entries, 11 native selections, 410 `NativeTypedef` fields,
ten associations, 148 cleanup fields and 226 invalid-value fields.
There are still 29 migrated definitions/274 members, eight genuine alias
selections, and now three deleted enum-binding JSON records/ten native
integer associations across the two structural checkpoints.

### Accepted new output

Complete cross-architecture `HttpServer`, `WindowsFilteringPlatform`,
`Printing`, `Controls` and `Setup` partitions plus constants refreshed in
**1m34.80s**, then the complete union emitted in **5m05.67s**, preserving the
checkpoint's other generated partitions. Six existing remap warnings and one
cross-architecture warning remain; there were no production errors.

| Actual new output | Identity |
|---|---|
| WinMD | `files\sidecar-reduction\next-structural-candidate\Windows.Win32.winmd` |
| Size | 24,176,640 bytes |
| SHA-256 | `122409C9D5CA95F1B791ED6A0846CBA25CDA1D178524F49A2F0B5FF610487EA7` |
| Complete raw declaration dump SHA-256 | `A4714E01E62224F5F1F0B3A3F25C19AE99B0279DA130E7E377388C69B430F8E8` |
| Actual unmasked comparisons against 5e7 and qualified original parent | Exit **-1**, reflecting intended structural changes |

Exact source-bound reconciliation checks all **194 changed method blocks**:
189 active native-handle substitutions, three producer ownership/invalid-value
groups, and five enum-use associations. The three removed synthetic type
blocks and ordering changes of three moved cleanup declarations are also
accounted for. All remaining declaration text is identical to 5e7:
**zero unmatched changes**. The first structural cohort's intended differences
and original five attribute-order-only moves also remain against the original
parent. No signature direction, extent, optionality, pointer depth, native
enum, import, availability, LastError or documentation changes are masked.

The full raw diff and unmasked comparer logs remain intact alongside
`next-structural-api-accounting.json`, which records every exact old/new block
and the one inactive selector. There is no known-difference file or comparer
normalization. Compilation alone was not acceptance.

All **189** directly related ownership/enum controls pass, including the
15 new cross-target semantic, C/C++ ABI and contextual-cleanup cases. The
three contextual cases were also rerun with actual diagnostics retained in
TRX. All **54** integrity cases pass against the actual new output copied to
`bin\Windows.Win32.winmd`. All **339/339** selected post-MIDL patches replay
strictly to byte-exact current headers; the binary-safe audit preserves native
bytes in 335 changed owners. Recipe selections remain 339 post-MIDL and
28 pre-MIDL patches; only five owning patch assets change in this batch.

### Concrete retained boundary: cleanup needs an engine context

The 13 remaining WFP/IPsec/IKE enumeration records are not blindly retired
with the engine record. Their exact native cleanup functions each require
**two** handles: the engine and the enumeration resource. For example:

```c
DWORD WINAPI FwpmFilterDestroyEnumHandle0(
    _In_ HANDLE engineHandle,
    _Inout_ HANDLE enumHandle);
```

The current accepted producer contract deliberately requires one declared
cleanup function accepting exactly the produced resource. A self-contained
current-header repro annotates `FwpmFilterCreateEnumHandle0::enumHandle`
with that real two-argument cleanup. The unannotated native control parses;
the annotated x86/x64/ARM64 cases all reject with:

```text
RAIIFree cleanup must resolve to one declared non-member, non-variadic native function accepting exactly the produced resource type.
```

The repro is
`HandleProducerSdkTests.FilteringEnumerationCleanup_RequiresEngineContextBeyondOneArgumentContract`;
actual diagnostics are in `next-contextual-cleanup-diagnostic.trx`.
`next-contextual-cleanup-records.json` binds all 13 retained records to their
two-handle native declarations. This is a demonstrated contract/context
boundary, not 13 independent reader regressions. Keep those exact sidecars
until contextual cleanup and engine-lifetime capture have defined semantics;
do not invent one-argument wrappers or discard the engine argument.

`NativeTypedef: false` is not a blanket fictitious-handle test:
the current SDK actually declares `typedef ULONG64 PROCESSTRACE_HANDLE`
in `shared\evntrace.h`. That genuine native identity remains. Other unattempted
records, including printer handles, trace IDs/streams and memory views, are
not classified as unsupported readers by this batch. The remaining earlier
parameter, enum-operand, constants integration and native-MIDL boundaries
remain in their preserved sections. No production reader/helper/converter/CI,
lower stack layer, stack metadata, timeout or test assertion was weakened.

## Printer, trace-ID and native COM enum continuation

This working checkpoint builds above preserved
`60f6fba3c29178eb481e832478f31d5681882a47`. It is another measured reduction,
not an approximate-minimum claim or the end of the remaining enum work.

The actual native printer declarations use `HANDLE`/`LPHANDLE`, not
`PRINTER_HANDLE`. Its record and 124 remaps are retired. `OpenPrinterA`,
`OpenPrinter2A/W` and `AddPrinterA/W` gain producer-site
`_Win32_RAIIFree_Handle_(ClosePrinter)`; existing `OpenPrinterW` ownership
remains. The complete native `ClosePrinter` declaration, including LastError,
moves before the first producer. Its old metadata-only forward declaration
and the two qualified-pilot raw-void projection overrides are removed.
Borrowed printer-queue getters acquire no ownership.

`CONTROLTRACE_HANDLE` and `RELOGSTREAM_HANDLE` were unused projection-only
shells: each appeared only in its own baseline type declaration. Their 20
remaps were also inactive. Current native SDK declarations instead use
`CONTROLTRACE_ID` and `RELOGSTREAM_ID`; these IDs do not acquire invented
cleanup ownership. The genuine native `PROCESSTRACE_HANDLE` remains.

Of the 146 retired handle/pilot rows, 106 match actual baseline parameter
or return facts; 40 selectors are inactive, including the 20 ETW rows.
Native declaration presence alone was not counted as emitted API coverage.
One additional duplicate WPC enum type-remap row is retired.

Twenty-six actual native-integer enum sites now carry `AssociatedEnum`:
17 rich-edit `tomConstants` uses, three disc-recorder uses, two authorization
uses, two parental-control uses, one `IPointerInactive` use, and one
`IBrowserService` use. Their real native enums remain. Sixteen complete
binding-only JSON records are deleted; the mixed Shell record retains its
three still-unmigrated selectors. All native direction, retval, pointer-depth
and other metadata are preserved, including the rich-edit returned pointers.

Eighteen dormant Ntms binding records/27 absent methods and the nonexistent
`STATSTG::grfStatFlag` selector are separately retired as inactive facts.
Real Ntms enums, their existing flags, and actual STATSTG fields remain
unchanged. In particular, two dormant `flags: true` requests did not produce
baseline flags and are not converted into new flag attributes.

| Retained input | Preserved 60f6 | This checkpoint |
|---|---:|---:|
| All RSP value rows | 134,189 | 134,042 |
| Emitter value rows / `memberRemap` rows | 1,955 / 1,747 | 1,808 / 1,600 |
| `autoTypes.json` records | 439 | 436 |
| `enums.json` records | 1,038 | 1,003 |
| `functionPointerFixups.json` records | 27 | 27 |
| Original parameter atoms retired / retained | 407 / 363 | 409 / 361 |

This is 147 fewer RSP rows, three fewer autoTypes records and 35 fewer enum
records than 60f6. The 90,502 allowed documentation mappings remain outside
the retirement backlog. The 26,376 automatic and 385 manual library mappings
remain separately inventoried, not granted the documentation exemption.

### Actual output and exhaustive accounting

The affected complete production partitions were refreshed, including the
transitive parental-control owner. A fresh complete union emitted in
**4m59.76s** to `files\sidecar-reduction\remaining-structural-candidate`.
Other qualified generated partitions were reused; no reference WinMD was
rebuilt or substituted.

| Output | Identity |
|---|---|
| WinMD SHA-256 | `3B60F59CAE171450AE766B57CF74EC7A8BD6C798431B3DEB8C3567C4FB350B1E` |
| Size | 24,176,640 bytes |
| Complete declaration dump SHA-256 | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` |
| Dumped types | 35,379 |
| Unmasked comparisons against 60f6 and original parent | Both exit -1, intentionally structural |

Exact source-bound reconciliation checks all **131 changed method blocks**
and the three removed synthetic types. It permits only the native HANDLE
substitutions, five new producer contracts, and 26 native-integer enum
associations. The complete remaining declaration text is identical:
**zero unmatched changes**. Raw comparer logs/diffs and every exact old/new
block remain in `remaining-structural-api-accounting.json` and adjacent
session evidence. No known-difference mask is used.

The original related run passed 147 and failed three of 150 controls. A
six-case corrected retry passed and covered each original failing name after
making the existing six-row legacy replay control read its frozen recovery
inputs, rather than expecting retired rows in the current production corpus.
This is corrected coverage across runs, not a subsequent single green
150-case run. All six original raw-signature assertions and header/legacy
equivalence checks remain. The actual-output
test now explicitly expects native `HANDLE*`/`HANDLE` for OpenPrinterW and
ClosePrinter; its ownership and remaining raw-void checks are unchanged.
All 54 actual-output integrity cases pass. All **343/343** selected post-MIDL
patches replay to byte-exact current headers; all eight newly changed native
owners preserve their non-ASCII bytes. There are still 28 pre-MIDL selections.

### Concrete retained boundaries and next closed groups

The 13 contextual WFP/IPsec/IKE records remain. The memory-view record also
remains: native `MapViewOfFile` produces `void*`, while `UnmapViewOfFile`
accepts `const void*`. All three current-target controls reject that exact
canonical-type mismatch under the current cleanup contract. Remote-process
unmapping additionally needs process context. No const-erasing signature,
wrong cleanup, or one-argument wrapper was introduced.

Four Cloudapi policy-field bindings remain. In C++,
`DEFINE_USHORT_ENUM` creates small wrapper **structs**, not integer aliases.
Correctly placed field carriers are rejected with `AssociatedEnum requires
integer or native enum storage, optionally through pointers.` The actual
macro/declarations reproduce this on x86/x64/ARM64. Changing wrapper storage
or teaching the reader a wrapper-flattening policy is a separate decision.

All seven candidate IDL owners compile through the unchanged converter and
native MIDL. The ocidl closure is accepted. Six others remain deferred after
native controls, without the enum annotations, already changed unrelated
existing SAL: ComAdmin/ExDisp lose `_Out_retval_`; oaidl/OleIdl alter unique
pointer optionality; WbemCli adds optional/inout pointer SAL; Wmcontainer
adds returned extents/direction SAL. These are owning-input/regeneration
boundaries, not 32 independent enum-reader bugs. Their original sidecars and
the actual native/control outputs remain. No unrelated change was accepted
merely because it came from MIDL.

The next prepared, not-yet-accepted cohort contains nine manual constant
groups plus `SNMP_PDU_TYPE`: 138 members and 59 unique uses. It includes their
exact manual definitions, JSON bindings and 28 nominal RSP overrides.
It is not counted as retired here. The other retained STGM/CLR and earlier
parameter facts remain individually inventoried; unattempted groups are not
called unsupported.

## Subdivided native constant-group continuation

The ten-group trial described above was actually run, then subdivided rather
than accepted on compilation alone. The working subset above preserved
`6d44e305c27a9186b90f34598eb05900da0328fe` transports six complete groups:
`ETO_OPTIONS`, `RGN_COMBINE_MODE`, `DIB_USAGE`,
`DEFINE_DOS_DEVICE_FLAGS`, `DI_FLAGS`, and `REG_SAM_FLAGS`.

Their **42 members and 26 unique uses** now come from seven exact native
header owners. Six manual enum definitions, eight JSON definition/binding
records, and eight duplicate REG_SAM emitter remaps are removed. Six namespace
bridges preserve the existing public owners; these are routing metadata, not
six new semantic facts. Documentation mappings are untouched and remain
explicitly allowed outside headers.

| Retained input | Preserved 6d44 | This working subset |
|---|---:|---:|
| All RSP value rows | 134,042 | 134,040 |
| Emitter rows / `memberRemap` | 1,808 / 1,600 | 1,800 / 1,592 |
| Namespace-owner rows | 2,491 | 2,497 |
| `enums.json` records | 1,003 | 995 |
| `autoTypes.json` records | 436 | 436 |
| `functionPointerFixups.json` records | 27 | 27 |
| Original parameter atoms retired / retained | 409 / 361 | 409 / 361 |

This is **12,749 net RSP rows fewer than recovery**, not 12,749 migrated
parameter facts. The 90,502 allowed documentation records and 26,376 automatic
plus 385 manual library mappings remain separately inventoried. The latter
are not exempted by the documentation allowance. The file/option and
individual-parameter inventories remain the authoritative mixed-unit counts.

### Actual accepted output

The actual traversal owners were refreshed, not inferred from projected
namespaces: WinBase declarations require `WinProg`, and WinUser declarations
require `MenuRc`. A complete generated-source union was copied to fresh
emission intermediates, excluding only nested emitter `obj` directories.
No qualified source or parent output was substituted. Production emission
completed in **4m43.64s**.

| Output | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\manual-group-six-candidate\Windows.Win32.winmd` |
| SHA-256 | `BD4281F64397B9DC842154DAC19848BC4C7046354227CCA6C4D0B0A55942A12A` |
| Complete dump SHA-256 | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` |
| Dumped types | 35,379 |
| Unmasked comparer against accepted 6d44 | Exit **0** |
| Complete raw declaration accounting | **Byte-identical**, zero unmatched changes |

Thus every previous intended handle/AssociatedEnum change remains, and this
additional whole-group transport changes no API facts. The actual new binary
is also in workspace `bin\Windows.Win32.winmd`. All **153 related controls**
pass in one new run, including three new real-SDK SNMP storage controls; all
**54 actual-output integrity cases** pass. All **343/343 post-MIDL patches**
replay through the unchanged strict production patcher to byte-exact current
headers. Seven native owners and four manual source owners preserve their
non-ASCII bytes and BOMs. There remain 28 pre-MIDL selections.

### Exact failed closures retained

| Retained closure | Observed trial and narrow boundary |
|---|---|
| `SNMP_PDU_TYPE` | The old JSON emits UInt32 storage; actual `smiINT`/`smiLPINT` PDU parameters are signed. Int32 group storage changes the old enum. UInt32 storage is rejected on all three current targets with `AssociatedEnum storage conflicts with enum group 'SNMP_PDU_TYPE'.` The new focused fixture reproduces both outcomes from actual SDK tokens. Retain the complete closure pending a storage-policy decision. |
| `PROCESS_CREATION_FLAGS` | After correcting traversal owners, the seven-group union fails emission: a Debug.Extensions constant cannot resolve `CREATE_UNICODE_ENVIRONMENT` (`CS0103`). Retain this complete definition/use closure; qualify dependent-constant integration before changing tooling. |
| `FILE_FLAGS_AND_ATTRIBUTES`, `REG_VALUE_TYPE` | The rejected initial ten-group output surfaced additional dependent/global constants, including `FILE_ATTRIBUTE_DEDICATED` and `REGSTR_*_TYPE` aliases. Both complete closures remain restored. Their individual causality is not yet isolated; these are not independently proved reader defects. |

The first ten-group output also contained two unrelated Identity callback
signature differences. They are absent from this accepted subset's complete
byte-identical dump. Restoring four closures and using fresh intermediates
changed more than one variable, so no cache or individual-group root cause is
claimed. The native Base-only control did not explain Identity behavior.
No callback remap, signature workaround, known-diff mask, or assertion
weakening was introduced.

Original named carriers on free functions were a mechanical input mistake:
those helpers require native instance methods. Supported inline parameter
annotations replaced them before acceptance; this is not counted as a new
reader bug. Original rejected outputs, complete logs/dumps, current-target
fixtures, strict patch receipts, and `manual-group-six-api-accounting.json`
remain in session evidence. Reader/helper/converter/CI and all lower layers
are unchanged. STGM and the residual CLR record remain for the next exact
owner/use-site subdivision, not classified globally as unsupported.

## Structured-storage subdivision and final native-enum control

The six-group working source is preserved at
`67e02a702cf454986b13300341d5332d1e7da905`. Work continued beyond that
checkpoint through an actual STGM production trial, not only a proposed
cohort. All rejected inputs and diagnostics are retained in
`files\sidecar-reduction\stgm-rejected-inputs` and the adjacent logs.

The STGM trial transported 18 actual SDK constants and 13 unique uses:
six free APIs in `coml2api.h`, five methods in `ObjIdl.Idl`, the STATSTG
field in `ObjIdlbase.Idl`, and one method in `mmdeviceapi.idl`.
The three unannotated native-MIDL controls compile, and all 498 existing
instance-method contracts remain textually unchanged after adding their
carriers and regenerating those owners. The remaining
`IFunctionInstance::OpenPropertyStore::dwStgAccess` use was kept as one exact
legacy RSP remap during the trial, not represented as migrated coverage.
Its untouched `functiondiscoveryapi.idl` control already changes 18 of 31
methods, including returned-pointer retval SAL and unique-pointer
optionality. This is another owning-input/current-converter regeneration
boundary, not 18 independent enum-reader failures.

An initial partial legacy STGM definition conflicted with the complete
header definition; that was corrected as an input-authoring mistake.
The corrected scrape/constants stage then passed in **1m18.71s**.
Actual production emission failed after **3m10.66s** with `CS0246` for
`DocumentationAttribute`/`Documentation`: the relocated
`Com.StructuredStorage.headerEnums.modified.cs` does not import
`Windows.Win32.Foundation.Metadata`, while the unchanged, explicitly
allowed documentation mapping adds:

```text
STGM=[Documentation("https://learn.microsoft.com/windows/win32/Stg/stgm-constants")]
```

Qualifying the same attribute with `global::` was not a supported
workaround: that attempt decoded to `[global]` and failed after
**3m15.08s**. No production global using, dropped documentation fact,
reader repair, or assertion weakening was introduced. All 14 trial files
were saved and completely restored to the accepted six-group inputs.
**No STGM WinMD was emitted, no STGM JSON retirement remains enabled, and
the one-row trial fallback is not part of the working state.**

`StructuredStorageGroup_DocumentationSidecarNeedsMetadataNamespaceImport`
is a self-contained, three-target real-SDK repro: the 18-member UInt32
group emits without the remap, while adding the exact documentation
remap must report CS0246. Its first fixture configuration accidentally
installed a SAL-fixture-only global metadata using, masking the actual
production failure; that fixture setup was corrected, not the assertion.
The corrected three cases pass, followed by a single complete related
run of **156 passed, zero failed, zero skipped**. This does not rewrite
the historical 147/3 result and six-case retry from the earlier checkpoint.

The final CLR control removes only the redundant JSON use selector
`ICLRHostProtectionManager::SetProtectedCategories::categories`.
The native declaration already takes the genuine `EApiCategories` enum;
its native type is preserved, not converted to integer AssociatedEnum
storage. The separate `flags: true` record remains pending qualification;
it is neither silently discarded nor added to the nine CLR exceptions.
This is one fewer legacy use selector, not another retired enum definition.
The CLR owner and constants were freshly regenerated in **1m12.66s**,
then a fresh complete production union emitted in **4m46.66s**. Only
accepted six-group generated sources supplied the unchanged partitions;
no rejected STGM source, qualified output binary, or rebuilt reference
was substituted.

| Final output | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\manual-group-final-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,176,640 bytes / `BD4281F64397B9DC842154DAC19848BC4C7046354227CCA6C4D0B0A55942A12A` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer against accepted 6d44 | Exit **0** |
| Complete raw declaration accounting | **Byte-identical**, zero unmatched changes |
| Unmasked original-parent comparison | Exit **-1**, preserving only previously accounted structural changes |

The deterministic new binary is byte-identical to the six-group output,
confirming that the deleted native-enum override was redundant. It is
also installed at workspace `bin\Windows.Win32.winmd`. The actual final
output passes all **54/54 integrity cases**; the complete related suite
above passed **156/156**. The previously accepted strict **343/343**
post-MIDL replay still binds the unchanged native headers and patches.
Reader/helper/converter/CI, the 28 pre-MIDL selections, genuine native
types, and all 90,502 allowed documentation mappings remain unchanged.
`manual-group-final-api-accounting.json`, full dumps, unmasked logs,
production logs, and original TRX retain the exact evidence.

The current 995 enum records are not 995 independently demonstrated
reader gaps. Of these, 971 have explicit members and/or auto-population
definitions. The other 24 are 23 additional-use records with 55 selector
rows, plus the retained CLR flags request. Those 55 rows belong to the
specific retained FILE/REG/SNMP/process, STGM, COM/MIDL and Cloud policy
closures documented here; definition-bearing records have not all been
independently subdivided. This remains a measured working reduction,
**not a mathematical minimum or a blanket unsupported-inventory claim**.

The narrow next fixes are to preserve the metadata-attribute namespace
when rewriting header enum sources, qualify the actual native-MIDL
regeneration differences before retiring those owner closures, resolve
SNMP storage intent, and isolate dependent-constant integration for the
four restored groups. Documentation URL rows remain allowed; no such
allowance is inferred for DLL mappings or other API metadata.

## Continued direct-header definitions

The renewed keep-going pass starts above preserved `85444f25`; publication is
a recovery checkpoint, not a reason to stop the next plausible cohort.
The first new accepted batch transports **25 complete definition/use closures,
216 members and 44 unique native uses**:

| Owning header | Groups | Native uses |
|---|---:|---:|
| `wincred.h` | 4 credential groups | 16 |
| `verrsrc.h` / `winver.h` | 9 version/resource groups | 14 |
| `Xinput.h` | 10 controller/battery groups | 10 |
| `commdlg.h` | 2 dialog groups | 4 |

All 25 JSON records are deleted, not replaced by binding-only records. Each
parameter, return or field binding has its exact current native carrier.
Existing implicit sequential members are included, and the old synthetic
`XINPUT_FLAG_ALL = 0` member is now a header constant. No inactive selector
or new inferred use is counted. Twenty-five namespace-owner bridges retain
the public owners: consequently total RSP rows increase by 25 even as 25
semantic JSON definitions and all their use records retire. The routing rows
are explicitly not counted as new API annotations or a retirement failure.

The actual Credentials, Fs, Xinput and DlgBox traversal owners were refreshed.
Corrected production scrape/constants passed in **1m15.78s**, and complete
emission passed in **4m42.56s**:

| Accepted artifact | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\direct-25-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,176,640 bytes / `A6FA0F41422F4F75BDACE30F3CA93721EDC72B4DBEF4194B31EED63930CDAC3D` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer / complete raw accounting | Exit **0** against 85444; entire dump byte-identical, zero unmatched changes |

Actual-output integrity passes **54/54**. Strict production replay passes
**345/345** selected post-MIDL patches, including the two new Xinput/verrsrc
patches; all five changed native owners retain non-ASCII bytes and BOMs.
The 28 pre-MIDL selections, reader/helper/converter/CI, existing assertions,
90,502 allowed documentation mappings and previous authorized structural
changes are unchanged. Exact inputs, original rejected parse log, corrected
production logs, dump, unmasked comparison, byte audit and TRX remain in
`files\sidecar-reduction\direct-25-*`.

The initial parse failures came from a mechanical dump-extraction error:
sequential enum members without explicit `=` were omitted, producing three
empty group declarations. Correcting all implicit members fixed the input;
these are not reader failures. The next 28 cfgmgr32/SetupAPI groups were then
actually tried and subdivided as follows, rather than revisiting the known
STGM/SNMP/MIDL boundaries.

## Device and setup dependency subdivision

The 28-group trial compiled and emitted, but its unmasked comparison reported
seven unwanted global constants: five `CM_LOCATE_DEVINST_*` aliases and
`NDW_INSTALLFLAG_NEEDREBOOT` / `NDW_INSTALLFLAG_NEEDRESTART`. Restoring
`CM_LOCATE_DEVNODE_FLAGS` and `SETUP_DI_DEVICE_INSTALL_FLAGS` removed those
additions, but the next actual 26-group output added
`DI_REMOVEDEVICE_GLOBAL` / `DI_REMOVEDEVICE_CONFIGSPECIFIC` to the restored
install-flags enum and failed the duplicate-member integrity assertion.
Restoring the linked `SETUP_DI_REMOVE_DEVICE_SCOPE` closure as well produces
the accepted **25 groups, 361 members and 71 active native uses**. All three
failed definition/use closures remain intact in JSON; no additions are masked.

Nine deleted legacy selectors are inactive configuration, not migrated API
coverage: four lowercase `sp_file_copy_params_*` / `filepaths_*` field
selectors, four nonexistent `CM_Get_Device_Interface_Size*` methods, and
`CM_Reenumerate_DevNodeEx`. Their exact names are absent from both native owners
and the complete baseline dump. In particular, no association was invented for
the differently named, uppercase native structures.

The exact DevInst traversal owner was regenerated. Final scrape/constants
passes in **1m02.33s**, complete emission in **4m45.56s**:

| Accepted artifact | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\direct-device-25-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,176,640 bytes / `6B8BD99F51ABC2431FD800A9870D855A12A674F3CE17C44B75A9CAEFA989E624` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer / complete raw accounting | Exit **0** against ae0ff; entire dump byte-identical, zero unmatched changes |

Actual-output integrity passes **54/54**, strict patch replay **345/345**.
Both native headers retain their original BOMs, non-ASCII bytes and non-ASCII
lines. Twenty-five additional JSON records retire, with 25 namespace routing
rows added; no parameter atoms or autoTypes records are counted as retired.
The complete inputs, each rejected output and diagnostic, final unmasked
comparison, full dump, byte audit and TRX remain in
`files\sidecar-reduction\direct-{28,26,device-25}-*`.

Two local command/cache mistakes produced incomplete or duplicated generated
sources before the final clean-cache run. Those logs are retained separately;
they are execution errors, not SDK reader defects or accepted outputs.
The next 71 WinUser/GDI/OLE-dialog/common-control groups were then evaluated,
not counted as accepted from preflight alone.

## UI, GDI, OLE and common-control continuation

The next accepted union retires **71 complete JSON definition/use records,
1,021 members and 156 active native bindings**:

| Owning header | Definition groups |
|---|---:|
| `WinUser.h` | 27 |
| `wingdi.h` | 13 |
| `OleDlg.h` | 9 |
| `CommCtrl.h` | 22 |

Two other old bindings, `DrawFrameControl::param2` and `::param3`, are
inactive selectors: actual native and baseline parameter names are `uType`
and `uState`, without those associations. They are deleted as inactive
configuration; no new association is silently added to the actual parameters.
Display-configuration bindings cross from wingdi constants to their actual
WinUser declaration owner within this same tested closure.

An initial authoring error placed four return carriers after `WINUSERAPI`.
The existing reader correctly rejected them with `must precede the native
declaration type`; moving the carriers before the complete declarations fixed
the input without a reader change. Macro-continuation calls and stale selectors
are distinguished from declarations in the session-only input authoring step.
These are not new unsupported-contract claims.

All four actual MenuRc/Gdi/ComOle/Controls owners scrape successfully in
**1m15.30s**; complete production emission passes in **4m29.13s**:

| Accepted artifact | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\direct-ui-71-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,176,640 bytes / `243E725BE3D7D77D4447271B4B28154F37CE220B7F3E1F537B724A62637B0E5E` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer / complete raw accounting | Exit **0** against 45a3; entire dump byte-identical, zero unmatched changes |

Actual-output integrity passes **54/54**, strict patch replay **345/345**.
All four native owners preserve their BOMs, non-ASCII bytes and non-ASCII
lines. Seventy-one namespace bridges preserve existing public owners and are
counted as routing additions, not hidden from RSP totals. Original parameter
atoms, autoTypes, fixups, DLL mappings and allowed documentation remain
unchanged in this cohort. Exact source facts, rejected/corrected diagnostics,
full raw accounting and TRX remain in `files\sidecar-reduction\direct-ui-71-*`.

The renewed pass has now retired 121 complete groups across three accepted
cohorts: 1,598 members and 271 active native uses, plus 11 explicitly inactive
legacy selectors. There are still 850 definition-bearing records, not a
blanket unsupported/minimum boundary. Preflight found another 195
explicit-member groups with native macro operands and local owner closures;
their 1,428 members / 349 listed selectors are preparation, not accepted
coverage. The six-genuine-handle DiagnosticDataQuery producer/storage cohort
was then actually evaluated and restored as below; all 436 autoTypes records
remain.

## Diagnostic native-handle subdivision

The six `HDIAGNOSTIC_*` aliases are genuine native `DECLARE_HANDLE`
declarations, not projection-only handle types. Selecting native storage and
moving their six exact cleanup/sentinel groups to producing output parameters
first fails on the actual native alias shape: DiagnosticDataQuery.h:8 reports
`Unsupported native typedef pointer shape; expected void or an incomplete
opaque record`. The SDK macro supplies a complete dummy record pointer.
All six storage/NativeTypedef selections were restored before the independent
producer-only retry; no reader or native declaration was weakened.

The producer-only retry scrapes in **59.80s**, emits in **4m35.54s**, and its
actual output passes integrity **54/54** and patch replay **345/345**.
Nevertheless it is **not accepted**: the unmasked comparison contains the
six intended type-to-producer cleanup/invalid-value moves plus eight unrelated
callback direction/pointer-depth changes in two Identity callback types.
Those are outside the allowed ownership transformation.

| Rejected artifact | Identity |
|---|---|
| WinMD / bytes | `D6F06CB05FD675EAD8AE380C7F34BEBEAF5C9D5A3CFC8041108F0717849C43D2` / 24,176,640 |
| Complete dump / types | `275AF494981D68042367BE0FAFFE6A91BBBBBF84FB6BE8623C53A5D3F50A3693` / 35,379 |
| Unmasked comparer | Exit -1; no filters or known-difference file |

After restoring the complete closure, an untouched-header production scrape
passes in **1m12.04s** and produces the exact same changed
`emitter.autoFnPtr.crossarch.generated.rsp` as the ownership retry. Its SHA-256
is `5E92BA3DBECCAE39E3F2052F45F3370E5D25EBC87472BEDB4C7DF55DAB6FE93F`;
the accepted prior response file was `B5196056...`. This isolates an
intermediate global function-pointer-reduction aggregation dependency during
filtered refresh, not a proved producer-reader defect. No full control
emission or speculative aggregation repair was performed.

All three trial inputs are restored, workspace output is again accepted
`243E725B...`, and no JSON field, parameter fact or native type retirement is
counted from this trial. Exact native-selection diagnostics, both attempted
input sets, rejected output/dump, unmasked comparison and untouched control
response-file accounting remain under
`files\sidecar-reduction\diagnostic-native-{six,control}-*`.
The next 195 explicit-member enum groups remain available for independent
subdivision rather than stopping on this boundary.

## Explicit-member native-owner subdivision

The next 195-group trial was actually applied, parsed, subdivided and emitted;
it was not accepted from preflight or compilation alone. The accepted subset
retires **183 complete definition/use records, 1,328 members and 317 active
native uses**, with no inactive selectors counted in this cohort:

| Owning header | Definition groups |
|---|---:|
| `WinUser.h` | 64 |
| `CommCtrl.h` | 31 |
| `wingdi.h` | 31 |
| `winnt.h` | 24 |
| `WinBase.h` | 21 |
| `commdlg.h` | 5 |
| `SetupAPI.h` | 3 |
| `wincred.h` | 3 |
| `cfgmgr32.h` | 1 |

Two straightforward authoring corrections preserve genuine native canonical
aliases: `LCSCSTYPE` uses its declared `LONG` backing and `EXECUTION_STATE`
uses `DWORD`. Primitive `int` / `unsigned int` spellings conflicted with those
existing typedef declarations; the actual corrected three-target parse passes.
No native declaration or reader policy changes.

Twelve complete definition/use closures remain restored:

| Restored closure | Actual boundary |
|---|---|
| `GET_CLASS_LONG_INDEX`, `WINDOW_LONG_PTR_INDEX`, `RTL_VIRTUAL_UNWIND_HANDLER_TYPE` | Listed operands are not available on every architecture (`GCL_HBRBACKGROUND`, `GWL_HINSTANCE`, `UNW_FLAG_CHAININFO`). |
| `GET_GUI_RESOURCES_FLAGS` | Historical `GR_GLOBAL` is a pointer-valued HANDLE sentinel, not an ordinary integer enum operand. |
| `IS_TEXT_UNICODE_RESULT` | Legacy UInt32 group conflicts with native `IsTextUnicode`'s `LPINT` result storage. |
| `MENU_ITEM_STATE`, `SET_BOUNDS_RECT_FLAGS`, `MODIFY_WORLD_TRANSFORM_MODE`, `HDC_MAP_MODE` | The emitted 190-group trial adds fourteen dependent global aliases: eight `IMFS_*`, `DCB_DIRTY`, two `MWT_*`, and three `MM_*`. |
| `QUEUE_STATUS_FLAGS`, `PEEK_MESSAGE_REMOVE_TYPE` | SDK-evaluated `QS_INPUT`, `QS_ALLINPUT`, `QS_ALLEVENTS` and dependent `PM_QS_INPUT` differ from legacy JSON values. Those value changes are not silently accepted as association migration. |
| `POWER_REQUEST_CONTEXT_FLAGS` | The 184-group output loses the association on `REASON_CONTEXT.Flags`; a carrier in WinBase reaches an alias/use, not the actual definition in `minwinbase.h`. The precise owner correction is queued, not classified as an unsupported reader. |

Refreshing only the initial eight partitions also lost associations on
`VerSetConditionMask` and both `STARTUPINFO*.dwFlags` fields. Refreshing their
actual SystemInformation/Threading consumers restores all three facts.
Only `REASON_CONTEXT.Flags` remains unmatched in the rejected 184-group output;
restoring its exact closure yields complete equality. Neither cached consumer
loss nor unwanted globals are accepted or masked.

The final ten-owner scrape passes in **1m51.70s**, production emission in
**4m43.46s**:

| Accepted artifact | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\explicit-183-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,175,616 bytes / `F12B23AA08FE7ED00EC414F307B1C2C4DB1AF76E4C4839423D1ED875026BCAC3` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer / complete raw accounting | Exit **0** against 491b; entire dump byte-identical, zero unmatched changes |

Actual-output integrity passes **54/54**, strict production patch replay
**345/345**, and all nine native byte/BOM audits pass. A patch filename-casing
authoring error was caught and corrected by strict replay, not bypassed.
The 182 new namespace routing rows are included in the RSP total: 134,343,
of which 90,502 allowed documentation rows remain separate. Parameter atoms
remain 409 retired / 361 retained; autoTypes and fixups are unchanged.

Since 85444, the four accepted cohorts retire **304 complete definitions,
2,926 members and 588 active native uses**, plus eleven separately inventoried
inactive selectors from earlier cohorts. Exact rejected inputs/diagnostics,
two emitted rejected candidates, accepted output, raw dumps/comparisons,
byte audit, patch replay and integrity TRX remain in
`files\sidecar-reduction\explicit-{195,191,190,184,183}-*`.
The next 94 shipped-header groups (718 members / 134 listed uses) are
preflighted; they are not yet migrated coverage.

## Fresh full-union 91-group checkpoint

The next 95-group trial (94 new groups plus the retained
`POWER_REQUEST_CONTEXT_FLAGS` owner retry) was subdivided to **91 accepted
definitions, 664 members and 130 active uses**. No inactive selector is counted.
The exact 91 JSON records are deleted; 91 explicit namespace routing rows are
added and included in the mixed RSP totals, not hidden as semantic retirement.

| Owning header | Definition groups |
|---|---:|
| `wincrypt.h` | 41 |
| `Richedit.h` | 11 |
| `winioctl.h` | 9 |
| `winsvc.h`, authored `mssign.h` | 6 each |
| `shellapi.h`, `newdev.h` | 5 each |
| `WinSock2.h` | 4 |
| `winreg.h` | 2 |
| `winhttp.h`, `winnt.h` | 1 each |

Four complete closures remain restored:

| Restored closure | Actual observed boundary |
|---|---|
| `SERVICE_NOTIFY` | The projection-only group name collides with the genuine native SERVICE_NOTIFYA/W struct typedef and its UNICODE alias. |
| `WTS_SECURITY_FLAGS` | Native WTS_SECURITY_CURRENT_USER_ACCESS has a missing `|` between continuation-line operands; evaluating that operand fails parsing. |
| `MIXERLINE_COMPONENTTYPE` | The emitted 93-group trial loses the entire group and two field associations, adding its 20 members as globals. Its traversal/definition-owner closure is not qualified. |
| `CERT_SELECT_CRITERIA_TYPE` | The same emitted trial adds the dependent CERT_SELECT_LAST global. |

`CFE_UNDERLINE` is accepted after narrow input hygiene: save its existing native
macro, undefine it only around the synthetic group typedef, and restore it
immediately. Its CFU_* operands and original native macro remain unchanged.
`REASON_CONTEXT.Flags` is accepted with an inline association on the actual
DWORD field in `minwinbase.h`; the earlier out-of-line carrier trials were
not accepted as evidence of unsupported reader behavior.

The signing input is the existing authored
`generation\WinSDK\AdditionalHeaders\mssign.h`, not a current NuGet SDK header.
That source and its consumed copy are byte-identical. An initially attempted
SDK-style patch failed strict replay and was removed; no preparation code or
patch-validation bypass was introduced.

This checkpoint uses a **new empty ObjDir and a complete production scrape**,
not copied generated sources or a rebuilt reference. Scraping takes
**14m46.57s**, emission **4m45.44s**. All three target architectures are scraped.

| Accepted artifact | Identity |
|---|---|
| Actual new WinMD | `files\sidecar-reduction\native-91-full-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,175,616 bytes / `F9D4F1E7ACB0A6D5BE18FE1A99B85C8D6C56B9E939EAAA4291557A467AFA9B0E` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer / complete raw accounting | Exit **0** against aed87853; entire raw dump byte-identical, zero unmatched changes |

The complete source union also preserves all eight Identity callback facts
that changed in the rejected filtered refresh. No generated reduction RSP was
edited. This qualifies the current union, not the separately restored
DiagnosticDataQuery producer trial or a speculative aggregation repair.

The crash interrupted the first integrity run after startup; it has no terminal
result and is not counted. The resumed bounded run passes **54/54**, with no
skips. Strict SDK patch replay passes **347/347**, the direct signing source
copy matches exactly, and all 18 loaded native-input byte/BOM audits pass.
Git-blob versus checkout comparison accounts explicitly for CRLF only;
non-ASCII bytes and lines are unchanged.

Since 85444, the five accepted cohorts retire **395 complete definitions,
3,590 members and 718 active uses**, plus eleven separately inventoried earlier
inactive selectors. Current totals are **600 enum JSON records** (576
definitions, 23 additional-use records / 55 selectors, one separate CLR flags
request), **436 autoTypes**, **27 fixups**, and **134,434 RSP rows** including
90,502 allowed documentation rows. Original parameter atoms remain
409 retired / 361 retained.

Exact rejected inputs, native diagnostics, 93-group output, fresh accepted
output/dump, unmasked comparison, replay, byte audit and resumed integrity TRX
remain under `files\sidecar-reduction\native-{95,93,91}-*`. The next 72 native
C-header groups (834 members / 141 listed uses) are preflighted only;
publication is a working checkpoint, not completion of the continuing reduction.

## Network, security and IME subdivision

The next preflight found 72 groups; the two already-restored setup closures
were excluded rather than re-tested. The actual new **70-group** trial was
subdivided to **64 whole definitions, 768 members and 124 active uses**:

| Owning header | Accepted groups |
|---|---:|
| `imm.h`, `NTSecAPI.h`, `WinTrust.h` | 9 each |
| `WinInet.h` | 8 |
| `ddeml.h` | 6 |
| `LMaccess.h` | 5 |
| `LMShare.h`, `WinNls.h`, `LMServer.h`, `OleAuto.h` | 3 each |
| `Shlwapi.h` | 2 |
| `vssym32.h`, `winsafer.h`, `Ole2.h`, `WinDNS.h` | 1 each |

All 64 JSON records are deleted and 64 namespace routing rows are added.
One exact old selector, `Msv1_0SubAuthenticationFilter::UserFlags`, has no
matching method/parameter in the native owner or complete baseline dump.
It is separately retired as inactive, not counted among the 124 active uses.
Six IME selectors use generated `paramN` names for genuinely unnamed native
DWORD/UINT parameters. Their zero-based ordinal, native declaration shape and
baseline group binding are checked before adding the inline annotation;
native signatures and emitted parameter names remain unchanged.

| Restored closure | Actual observed boundary |
|---|---|
| `VARCMP` | Native STDAPI/HRESULT return storage conflicts with the legacy UInt32 group at OleAuto.h:831. |
| `TRUSTED_DOMAIN_TRUST_TYPE`, `TRUSTED_DOMAIN_TRUST_ATTRIBUTES` | The parse cannot resolve native operands TRUST_TYPE_DCE and TRUST_ATTRIBUTE_FILTER_SIDS in the selected current SDK input. |
| `INTERNET_ACCESS_TYPE`, `INTERNET_STATE` | The emitted 67-group trial adds three old access aliases and three state aliases as global constants. |
| `SHARE_INFO_PERMISSIONS` | Native ACCESS_ALL evaluates to 127, while the legacy public group member is 32768. This value update is not silently accepted as annotation transport. |

The rejected 67-group output also lost three filesystem groups and the
AppLocker group. Refreshing their actual `Fs` and `Security.AppLocker`
owners preserves all four groups, use-site associations and global ownership.
No reader fix or output mask is needed. The 64-group union refreshes Identity
as well, and preserves every earlier callback fact.

The actual 16-partition scrape passes in **1m54.18s**, production emission in
**4m51.41s**, starting from the accepted complete source cache:

| Accepted artifact | Identity |
|---|---|
| New WinMD | `files\sidecar-reduction\native-c-64-candidate\Windows.Win32.winmd` |
| Size / SHA-256 | 24,175,104 bytes / `67F41F57593F44264A0022CF93332CAC3E2FA457F846C1BFD12044AF74F9B7F6` |
| Complete dump / types | `1F4F38395D602095972A5095C4A25EF6C117BE5E1D716DAB5C3108BE6AC589BA` / 35,379 |
| Unmasked comparer / complete raw accounting | Exit **0** against c991; whole dump byte-identical, zero unmatched changes |

Actual-output integrity passes **54/54**, strict production SDK patch replay
**348/348**, and all 15 native-header byte/BOM audits pass. Exact 70/67 rejected
inputs, diagnostics/output, the accepted output, raw dumps/comparisons,
audits and TRX remain in `files\sidecar-reduction\native-c-{70,67,64}-*`.

The six accepted cohorts since 85444 now retire **459 complete definitions,
4,358 members and 842 active uses**, plus twelve separately inventoried
inactive selectors. Current enum JSON is **536 records**: 512 definitions,
23 additional-use records / 55 selectors, and one separate CLR flags request.
RSP totals are **134,498**, including the unchanged 90,502 allowed documentation
rows. autoTypes 436, fixups 27 and original parameter atoms 361 remain unchanged.
Further shared-header and security/system owners remain plausible; this is
another working checkpoint, not a minimum or a stopping boundary.
