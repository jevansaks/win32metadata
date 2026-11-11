# Working-first recovery and sidecar reduction

**Current accepted state:** the working reduction below removes 407 parameter
atoms, 2,894 LastError rows, 9,187 availability rows, and 29 enum definitions;
eight additional genuine aliases use compiler-backed storage. The next
[structural migration](#producer-ownership-and-enum-use-site-migration) removes
two fictitious handle records and 14 forced type remaps, and replaces five
JSON enum bindings with native `AssociatedEnum`. Its new WinMD has exactly the
intended structural differences, exhaustively accounted for without masks;
it is not claimed API-equivalent. The preceding
[equivalent union](#final-accepted-enumhandle-union) remains preserved.
The earlier quantities below describe preserved historical checkpoints.

The user redirected this experiment from broad sidecar retirement to a working
generation anchor. The diagnostic experiment remains in commit
`88991baf654ff0d89855a7005873addd92a76859`; its findings are not withdrawn.

The recovery restores the complete qualified parent **generation input set**
from `60188deb6ed2c22b14a5df028897b3a941a5d2aa`, without reverting tooling,
rewriting history, or changing lower stack layers. This conservative batch keeps
only the already-qualified parent header pilots. **No additional broad-migration
header coverage is claimed.** All 737 broad patches and their sidecar retirements
are deferred until they can be enabled in small batches that each emit.

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
