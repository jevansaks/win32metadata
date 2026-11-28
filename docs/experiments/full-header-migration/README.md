# Full SDK header migration experiment

> This is the historical diagnostic report for frozen commit
> `88991baf654ff0d89855a7005873addd92a76859`. The subsequent
> [working-first recovery](RECOVERY.md) restores conservative sidecars and
> qualified header inputs and produces a new WinMD. The findings below remain
> evidence for incremental migration, not the current generation status.

**Result: the broad migration is ported, but production scraping is blocked.
There is no fresh full candidate winmd and no equivalence claim.** Independent
SDK slices expose additional policy boundaries and two surviving native-MIDL
conversion losses beyond the initial compiler failures. This is an evidence
layer, not a proposal to merge the old tooling or weaken validation.

## Frozen inputs and scope

| Input | Exact identity |
|---|---|
| Parent, fork PR #29 | `60188deb6ed2c22b14a5df028897b3a941a5d2aa` |
| Parent tree | `301e9500bec6f4e37884720debbebcf9e46be695` |
| Migration, microsoft/win32metadata#2365 | `6becf8d2d76aafe86c8cb05b7a0ed6c0e038450a` |
| Migration's recorded and ancestral base | `2488f9879b8ca7f00c7c19c5dbad3dba3c8065eb` |
| Accepted contract, microsoft/win32metadata#2402 | `db1e3cd9e4ed4fcb872d00af68f6503ea72dbef1` |
| Selected SDK | `Microsoft.Windows.SDK.CPP/10.0.28000.2705` |
| Migration's recorded header tree | `114b05b4db93918a563294f2be0981d93766aaa4` |

The source recipe admits 741 patch assets. This layer carries 737 changed
assets: **458 directly consumed headers in post-MIDL patches and 279
authoritative IDL owners in pre-MIDL patches**. Three admitted assets produce no
additional changed patch after preserving parent overlaps:
`um/errhandlingapi.h.patch`, `um/htmlhelp.h.patch`, and `um/sti.h.patch`.
`um/alljoyn_c/AboutDataListener.h.patch` is excluded
because its owner is not shipped in the selected SDK. The evaluated partition
list also excludes AllJoyn and WinRT.AllJoyn. No removed SDK surface is restored.

Nine other source assets are outside that source recipe and are not silently
enabled: `um/control.odl.patch`, `um/l2cmn.h.patch`, `um/mmstream.idl.patch`,
`um/vdscmprv.idl.patch`, `um/vdshpcm.idl.patch`, `um/vdsprvcm.idl.patch`,
`um/wcmconfig.idl.patch`, `post-midl/shared/hidpi.annotations.patch`, and
`post-midl/shared/tbs.annotations.patch`.

[input-patches.json](input-patches.json) records every imported source asset,
current patch, target, and phase. There are 259 additional explicit pre-MIDL
selections; the existing selected owners and pilots remain. This is not a
reduction to the small pilots.

The current helper, SAL converter, scraper/emitter implementations, and CI are
unchanged. The source's old tooling base is not imported. Direct-header patch
replay matches all 458 checked-in effective headers, ignoring CRLF versus LF.
Raw-byte comparison against the parent finds no new UTF-8 replacement characters
and no changed non-ASCII byte sequences.

### Integration and translations

- Preserve current pilot hunks rather than replacing them with older versions.
  [preserved-overlaps.json](preserved-overlaps.json) records all **137**
  header/IDL overlap hunks whose old source replacement was not applied. The
  largest groups are `combaseapi.h` (45), `winhttp.h` (28), `propsys.idl` (7),
  `shobjidl_core.idl` (6), and `errhandlingapi.h` (6). Eight earlier RSP text-hunk
  conflicts were superseded by semantic `(option section, exact entry)` handling;
  they are not eight additional unresolved header problems.
- Retain the genuine pointer-adding `LPWINHTTP_STATUS_CALLBACK` declaration.
- Translate `_Win32_Reserved_` to stock `_Reserved_`. Remove obsolete custom
  Optional/ComOutPtr markers without inventing blanket scalar-In or pointer-Inout
  defaults. Native SAL/MIDL remains authoritative. No obsolete Optional,
  Reserved, ComOutPtr, Flags, or NativeTypedef custom vocabulary is newly authored.
- Make helper inclusion explicit. There are 447 direct headers with an include
  absent from the parent, including includes already present in the old source.
- Import source-derived RSP/manual retirements semantically, not by replacing
  current configuration files wholesale. The retirement pass removes 114,233 RSP
  entries before the explicit bridges below.
- Retire 436 `autoTypes` entries, retaining the source's eight exceptions; retire
  1,070 `enums.json` items, leaving an empty item list. Neither corpus is restored
  wholesale to obtain a green result. These retirements are experimental, not
  proof that every corresponding contract has migrated.

## Explicit compatibility bridges

[sidecar-bridges.tsv](sidecar-bridges.tsv) inventories **3,320 exact retained or
restored entries**, including their option section and reason:

| File | Entries | Coverage this does not prove |
|---|---:|---|
| `supportedOS.rsp` | 3,135 | Availability formerly transported by old comments |
| `emitter.settings.rsp` | 184 | Old comment facts and the CLR compatibility exception |
| `scraper.settings.rsp` | 1 | An old comment fact |

Of these, 3,319 bridge the source's obsolete comment transport; one is the
explicitly restored CLR compatibility fact. Eight other CLR facts already remain
in the resulting source-derived configuration. All **nine** parent CLR facts
were checked against their exact parent spelling, including
`CLRCreateInstance::ppInterface=[Out][ComOutPtr]`. Their unresolved ownership is
unchanged; see [the existing CLR audit](../../sdk_patches.md#clr-hosting-and-metadata-activation-outputs).

The source contains 3,592 old `Win32Metadata: ... => ...` comment records:
2,819 interface, 327 function, 276 parameter, 85 method, 82 field, two enum, and
one return. A comment count is not a bridge count: several spellings do not have
a one-to-one sidecar counterpart, and retired or incompatible facts are not
reintroduced merely to make the counts match. These bridges explicitly mask the
absence of migrated header transport. They must not be reported as reader support.

## Evaluation performed

| Stage | Actual outcome |
|---|---|
| Current `BuildTools`, Release | Success |
| Complete selected pre-MIDL and post-MIDL patch application | Success |
| Independent native MIDL, `/sal /sal_local` | All 11 tested owners emitted: amsi, waasapi, wtypes, oaidl, objidlbase, dxgi, dispex, docobj, mfobjects, mfidl, bdaiface |
| Ten x86/common partition scrapes | All blocked at C/C++ parsing; 210 reported errors across those runs |
| WinHTTP x64, uncapped diagnostics | 117 compiler diagnostics across 51 unresolved names; [exact inventory](parse-diagnostics.json) |
| SDK-derived repros plus producer/native-typedef controls | 100 cases passed, including 11 new reproduction/control cases |
| Existing current MIDL/SAL/retval controls | 38 cases passed |
| Persistent native-MIDL comparison script | Both native and current-converted inputs compile; paired contracts reproduce the two losses below |
| Full candidate emission, integrity tests, API comparison | Not reached; no fresh production candidate exists |

The ten real partition probes were WinHTTP, Foundation, Gdi, Security, Services,
Com, Dxgi, HttpServer, Memory, and Threading. Common partitions run on the x86
pass; an x64 filter alone does not exercise those partitions. The 210 errors are
not 210 distinct bugs. An uncapped WinHTTP TU exposes the wider shared-header
dependency closure without repeating a doomed full build.

The qualified parent is reused, not regenerated: ordinary run `42133942944`,
attempt 1, successful at the exact parent. Its winmd SHA-256 is
`78B3D19EEB7793FB953F2FEFAEE029081520466452F6DA83747674C14A6DE3A2`.
Comparison/build artifact IDs are `13467206999` / `13468041187`; the supplied
complete parent dump SHA-256 is
`C18E31F0C0BFF9FEC72477DD4D539766806E4B282A2ADD1C285E5555F99C4D36`.
Preexisting generated files are not treated as an experimental candidate.

The initial missing-`excpt.h` environment error, scratch patch reconstruction
ambiguities, and port-generated patch-owner casing mistakes were corrected.
They are not counted as production reader defects.

## Demonstrated problems and classification

### 1. Typed operands expose old declaration-order assumptions

**Migration input; high confidence.** The current typed contract deliberately
requires compiler-resolved operands. The old source often assumed deferred
string lookup:

| Current effective input | Diagnostic |
|---|---|
| `um/fileapi.h:351`, `FindFirstVolumeW` | undeclared `FindVolumeClose` |
| `um/memoryapi.h:258,275` | undeclared `UnmapViewOfFile` |
| `um/threadpoolapiset.h:48` and adjacent producers | cleanup functions declared later |
| `shared/minwindef.h:264`, HRGN association | undeclared `HGDIOBJ`, owned by `windef.h` |
| `um/winnt.h:27072,27108,27126` | undeclared `SERVICE_NO_CHANGE`, owned by `winsvc.h` |

The complete 117-diagnostic inventory also includes GDI, BCrypt/NCrypt, menus,
window stations, media handles, socket cleanup, and Automation allocators.

`FileApiProducer_RequiresCleanupDeclarationBeforeTypedOperand` extracts the real
SDK creator and cleanup declarations: failure without the cleanup declaration,
success and final return `RAIIFree` metadata with that exact declaration first,
on x86/x64/ARM64. `ServiceErrorGroup_RequiresConstantFromItsActualOwner` similarly
fails without the actual owning constant and succeeds with it.

**Recommendation:** repair authoritative input placement/dependency closure
before changing readers. For associations spanning headers, place the metadata
declaration where its genuine operand is visible. For producer cleanup, choose
a reviewed declaration strategy; do not fabricate cleanup signatures or replace
typed validation with unchecked strings.

### 2. Genuine GDI handle association exceeds canonical-type equality

**Current nominal-reader policy boundary; high confidence, not an unexplained
parser regression.** After making the real `HGDIOBJ` declaration visible,
`_Win32_AlsoUsableFor_(HGDIOBJ) typedef HRGN HRGN;` still fails:

```text
AlsoUsableFor target 'HGDIOBJ' has conflicting native storage.
```

In the SDK's strict declarations, HRGN is an opaque `HRGN__*`; HGDIOBJ is `void*`.
`NativeTypedefs` compares canonical Clang types, not Win32's documented generic
GDI-handle relationship. The new SDK-derived test repairs only declaration
visibility and then preserves this second diagnostic.

**Recommendation:** discuss a narrow native-typedef policy extension in the
existing nominal layer if these genuine generic-handle relationships are in
scope. Do not equate arbitrary same-width pointers or add pseudo handles.

### 3. Generic GDI cleanup has the same conservative resource-type boundary

**Current producer-reader policy boundary; high confidence.** Even with
`DeleteObject` declared first, the actual SDK `CreateRectRgn` producer annotated
with `_Win32_RAIIFree_(DeleteObject, -1, 0)` fails:

```text
RAIIFree cleanup must resolve to one declared non-member, non-variadic native
function accepting exactly the produced resource type.
```

The producer returns HRGN; cleanup accepts HGDIOBJ. The reproduction holds on
all three target architectures. This is separate from the earlier undeclared
cleanup error and follows the current intentionally exact contract.

**Recommendation:** coordinate any reviewed generic-handle compatibility with
the producer layer, rather than independently relaxing its native type checks.
Keep borrowed functions such as `GetCurrentProcess` unowned.

### 4. Native typedef discovery is not nominal projection policy

**Migration completeness/policy boundary; high confidence.** The real SDK
`typedef void *HANDLE;` and borrowed `GetCurrentProcess` scrape and emit without
a nominal HANDLE struct by default. Adding the existing `--native-typedef
HANDLE` policy emits that identity, still without ownership. Both paths are
covered by a fresh final-winmd fixture.

Retiring the old JSON contracts therefore does not automatically preserve every
named handle identity, invalid value, cleanup, namespace, or exceptional policy.
The capability exists; the broad old migration has not supplied a complete
reviewed current policy. This is not evidence that native typedefs cannot be
discovered.

**Recommendation:** inventory genuine nominal identities and ownership separately
before accepting the 436 retirements. Retain the boundary instead of restoring
the JSON corpus wholesale or inventing a `_Win32_NativeTypedef_` annotation.

### 5. Old comment transport cannot retire sidecars

**Stale migration input; high confidence.** The source's AmsiUninitialize comment
compiles but does not produce availability metadata. The current supported-OS
macro on the same real SDK declaration does. The final-winmd fixture verifies
both outcomes. Many IDL interface facts use the same obsolete transport.

**Recommendation:** translate each owned fact into the current native/SAL
contract and the appropriate pre-MIDL owner, then retire its exact bridge. Do
not add a legacy comment parser merely to bless this old migration.

### 6. GR_GLOBAL is still put into the wrong native domain

**Malformed migration input/native-type defect; high confidence.** The source
and port retain the native macro `#define GR_GLOBAL ((HANDLE)-2)` but also insert:

```cpp
typedef _WIN32META_ANNOTATION_("Enum(GR_GLOBAL)", (unsigned int)-2)
    unsigned int GET_GUI_RESOURCES_FLAGS;
```

The fresh fixture emits **UInt32 `4294967294`** exactly as this malformed input
requests. GR_GLOBAL is a HANDLE sentinel for the process argument, not another
UInt32 `uiFlags` member. The explicit cast destroys the native pointer domain.

**Recommendation:** correct the migration's enum membership/operand rather than
teaching the enum reader to guess around an authored cast. This confirms
[the prior review](https://github.com/microsoft/win32metadata/pull/2365#issuecomment-6645573150),
not a newly discovered enum evaluation failure.

### 7. Current SAL conversion still weakens returned-pointer contracts

**Preexisting current converter limitation; high confidence.** This is reproduced
with the current converter, not by importing the old source script. Under
identical native MIDL `/sal /sal_local` settings:

| Actual method | Native MIDL | Current converted input |
|---|---|---|
| `IDispatchEx::GetNameSpaceParent` | `__RPC__deref_out_opt IUnknown **ppunk` | `_Out_ IUnknown **ppunk` |
| `IDispError::QueryErrorInfo` | `__RPC__deref_out_opt IDispError **ppde` | `_Out_ IDispError **ppde` |
| `IDispError::GetNext` | `__RPC__deref_out_opt IDispError **ppde` | `_Out_ IDispError **ppde` |

`Get-SalAnnotation` chooses generic `_Out_` from direction; its COM-output
specialization uses `iid_is`, not this concrete returned-interface-pointer
shape. Injecting `annotation("_Out_")` suppresses MIDL's stronger native SAL.
This proves native-header information loss before the reader, not a measured
whole-winmd API delta.

**Recommendation:** expand the current SAL-converter layer with these actual
IDL cases. Preserve native MIDL SAL when conversion cannot supply an equivalent
returned-pointer contract. Do not infer every double pointer is COM, or replace
the qualified converter with its old version.

### 8. Multidimensional size_is still loses a representable outer extent

**Preexisting current converter limitation; high confidence.** The two actual
IFrequencyMap getters in `bdaiface.idl` contain `[out, size_is(1, *count)]`.
Native MIDL preserves `__RPC__out_ecount_full(1) ULONG **ppulList`; the current
converted input emits `_Out_ ULONG **ppulList`.

`SALParseSize` explicitly returns null for an internal comma, but the caller
still inserts generic `_Out_`. Unsupported conversion therefore overrides and
weakens a native outer-extent fact.

**Recommendation:** preserve native generation for unsupported dimension
expressions before considering broader multidimensional support. This belongs
in the existing SAL-converter layer; no new annotation vocabulary is needed.

### Prior risks that did not reproduce

The current converter **does preserve** `[in, unique]` optionality:
`QueryStatus::pguidCmdGroup` is `_In_opt_`, and its optional inout text pointer
is `_Inout_opt_`. It also combines the split attribute lists on the actual
`IOleCommandTarget::QueryStatus` buffer into `_Inout_updates_(cCmds)`, preserving
the native count. These two prior-review warnings are not counted as current
failures. The paired native-MIDL output makes the distinction explicit.

## Reproduction and retained evidence

The new tests use actual installed selected-SDK declarations and the existing
fresh scrape/emission fixture:

```powershell
dotnet test tests\ClangSharpSourceToWinmdTests -c Release --filter "FullyQualifiedName~FullHeaderMigrationExperimentTests|FullyQualifiedName~ProducerHeaderTests|FullyQualifiedName~NativeTypedefHeaderTests"

. scripts\CommonUtils.ps1
Install-VsDevShell
dotnet msbuild generation\WinSDK -p:Configuration=Release -t:ScrapeHeaders -p:ScanArch=x86 '-p:PartitionFilter=WinHttp%2cFoundation%2cGdi%2cSecurity%2cServices%2cCom%2cDxgi%2cHttpServer%2cMemory%2cThreading' -p:MaxProcessors=2

.\docs\experiments\full-header-migration\Repro-NativeMidl.ps1 -SdkPackageRoot <selected-sdk-package> -OutputPath <new-evidence-directory>
```

The MIDL repro uses the current converter and unconverted native controls. It
retains inputs, tool arguments, logs, both headers, and extracted method pairs.
Its default input is the shipped selected SDK; `-HeaderRoot` can point at an
isolated tree with this project's selected patches applied. Successful native
compilation does not assert SAL equivalence.

[observed-native-sal.json](observed-native-sal.json) preserves the six actual
paired method contracts. [input-audit.json](input-audit.json) records the exact
CLR spellings, byte/patch-replay checks, frozen identities, and missing-name
inventory.

The original run's frozen source exports, reconstruction results, selected patch
list, prepared SDK tree, all partition logs, uncapped log, MIDL response files,
native controls, test TRX files, raw method comparison, and qualified parent
winmd are retained in the producing session's `files` directory. The checked-in
inventories retain exact inputs, bridges, overlaps, and diagnostic locations
without committing the SDK tree or a stale candidate.

## Interpretation and next decisions

Expected differences include native SAL defaults, native `[retval]`, retired
Agile metadata, removed SDK/AllJoyn surfaces, and intentional removal of synthetic
handle models. They must be separated from accidental loss of owned contracts,
incorrect enum membership, or unresolved nominal policy. No whole-candidate
comparison exists to quantify those differences in this experiment.

The narrow order of work is input declaration closure first; explicit genuine
nominal policy and reviewed generic-handle relationships second; native-MIDL
non-weakening fallbacks and actual-source regressions in the existing SAL layer;
then per-owner comment/sidecar retirement. Only after those decisions should the
broad candidate be regenerated and compared with the already-qualified parent.
No existing published layer is changed by this experiment.
