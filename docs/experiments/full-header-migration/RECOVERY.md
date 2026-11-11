# Working-first recovery

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
