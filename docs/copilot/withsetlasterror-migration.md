# WithSetLastError.rsp Sidecar-Removal Tranche

## Summary

`generation/WinSDK/WithSetLastError.rsp` is a plain list of API names (one per
line, plus a `--with-setlasterror` header line) passed to the scraper so the
listed functions are flagged as SetLastError-observing P/Invokes. This is a
separate mechanism from the `emitter.settings.rsp` `--memberRemap` sidecar
used by the earlier RAIIFree tranche, but the same shift-left goal applies:
every API-specific fact should live as an inline `_Win32_metadata_set_last_error_`
annotation directly on the producer's declaration in the owning SDK header,
not in a project-wide sidecar list.

The rsp originally contained **3365** API name entries.

## Phase 1: Reconciliation (redundant entries already covered)

Built a declaration-anchored scanner over `generation/WinSDK/RecompiledIdlHeaders`
that, for every name in the rsp, locates its real producer declaration(s) and
checks whether `_Win32_metadata_set_last_error_` is already present in the
declaration's immediate prefix block (walking upward through SAL/specifier
lines, and through `#if`/`#elif`/`#else` conditional guards so an annotation
placed once before a version-guarded pair of redeclarations is still found).

**2231 entries** were already fully represented by an existing header
annotation (added by prior batches under various now-superseded per-reason
patch names such as `*.set-last-error.patch`, `*.zzz-set-last-error.patch`,
`*.service-security-set-last-error.patch`) and were removed from the rsp as
pure redundancy, with no header changes required.

Two rounds of this reconciliation were needed: the first heuristic (a simple
forward text-scan from each existing annotation) had a false-positive bug -
SAL annotations and calling-convention macros shaped like `IDENTIFIER(`
(`_Success_(...)`, `_WINSOCK_DEPRECATED_BY(...)`, `STDAPI_(rettype)`,
`INTERNETAPI_(rettype)`) were mistaken for the annotated function's own name,
under-counting real coverage. The corrected declaration-anchored scanner
found 250 more already-covered entries than the first pass. See
`docs/copilot/ralph-loop-runlog.md` for the detailed history of this fix.

## Phase 2: Non-representable entries (documented blockers)

**9 entries** are not real P/Invoke-able functions at all and cannot carry a
function-level annotation:

- `HasOverlappedIoCompleted`, `PropSheet_GetResult` - pure C preprocessor
  macros (`#define HasOverlappedIoCompleted(lpOverlapped) (((DWORD)...)`,
  `#define PropSheet_GetResult(hDlg) ...`), not functions.
- `IEnroll`, `IGetClusterGroupInfo`, `IGetClusterNetInterfaceInfo`,
  `IGetClusterNetworkInfo`, `IGetClusterNodeInfo`, `IGetClusterObjectInfo`,
  `IGetClusterResourceInfo` - COM interface *type* names (`MIDL_INTERFACE`
  declarations), not free functions. Their real member methods have
  different names and are not what these list entries denote.

These 9 were removed from the rsp with no substitute header change - keeping
them would have been permanently inert (the scraper's `--with-setlasterror`
mechanism only ever matches real `FunctionDecl` nodes) and silently leaving
them was avoided per the explicit instruction to document rather than drop.

## Phase 3: Producer-site migration

The remaining **1125** entries were classified by locating their real
declaration(s):

- **~795 single-location** names: exactly one real producer declaration
  found; annotated directly.
- **~187 multi-location** names: legitimate duplicate declarations across two
  real headers (the classic `DbgHelp.h`/`ImageHlp.h` legacy-compatibility
  pattern, ~180 pairs) or a single header with a version-guarded
  `#if/#else` pair of redeclarations (e.g. `CryptCATAdminAddCatalog` in
  `mscat.h`) - both declarations annotated in each case, since only one is
  ever compiled at a time and either could be the one a given partition
  resolves.
- A substantial false-positive elimination pass was required before trusting
  any location as a real declaration: excluded SAL/`specstrings*.h` infra
  headers, `.idl` `cpp_quote` duplicates of a `.h` declaration, comment prose
  mentioning a function name, backslash-continued multi-line macro bodies,
  `winrt/` COM/WinRT projection headers (interface methods sharing a common
  name with a real Win32 function, e.g. `GetClassName`, `SendMessage`),
  `STDMETHODCALLTYPE` COM vtable methods in regular `um/` COM headers, and
  `return NAME(...)` forwarding-shim call sites (e.g. `WinBase.h`'s inline
  `FormatMessage` picking either `FormatMessageA` or `FormatMessageW`).
  Every exclusion rule was validated with an explicit self-test asserting
  the known-good/known-bad location before being trusted at scale, and a
  final automated dry-run confirmed zero would-be duplicate insertions
  before any file was touched.

**982 entries across 63 headers** were migrated to inline
`_Win32_metadata_set_last_error_` annotations in this tranche (**1165**
total insertion locations, since ~187 names needed two locations each).

Headers touched: `AclUI.h`, `AdsHlp.h`, `avrt.h`, `ClusApi.h`, `CommCtrl.h`,
`cryptdlg.h`, `cryptuiapi.h`, `datetimeapi.h`, `DbgHelp.h`, `dde.h`,
`ddeml.h`, `debugapi.h`, `dpapi.h`, `enclaveapi.h`, `ErrorRep.h`, `EvColl.h`,
`FaxDev.h`, `FaxRoute.h`, `fibersapi.h`, `gb18030.h`, `gdiplusheaders.h`,
`highlevelmonitorconfigurationapi.h`, `Icm.h`, `ImageHlp.h`,
`libloaderapi2.h`, `lowlevelmonitorconfigurationapi.h`, `magnification.h`,
`memoryapi.h`, `minidumpapiset.h`, `MSAJTransport.h`, `mscat.h`, `mssip.h`,
`muiload.h`, `namespaceapi.h`, `Ole2.h`, `oleacc.h`, `PeerDist.h`,
`perflib.h`, `physicalmonitorenumerationapi.h`, `prsht.h`,
`realtimeapiset.h`, `Resapi.h`, `ScrnSave.h`, `SensAPI.h`, `shellapi.h`,
`ShlObj_core.h`, `Shlwapi.h`, `stringapiset.h`, `threadpoolapiset.h`,
`threadpoollegacyapiset.h`, `utilapiset.h`, `Uxtheme.h`, `wct.h`,
`WinBase.h`, `wincrypt.h`, `WinFax.h`, `wingdi.h`, `WinNls.h`, `winscard.h`,
`winspool.h`, `WinTrust.h`, `WinUser.h`, `WtsApi32.h`.

Each header's complete change set (this tranche's new annotations plus every
pre-existing patch for that header, including annotations from the earlier
RAIIFree tranche for headers such as `avrt.h`, `WinBase.h`, `wct.h`,
`WtsApi32.h`, `wingdi.h`, `perflib.h`, `ShlObj_core.h`) was consolidated into
one cumulative `<header>.metadata.patch` against the pristine `d154186c`
baseline. 20 now-superseded per-reason patches were removed:
`CommCtrl.h.controls-enums.patch`, `CommCtrl.h.zzz-supported-os.patch`,
`Icm.h.callback-canonical-name.patch`, `Ole2.h.zzz-supported-os.patch`,
`ScrnSave.h.zzz-supported-os.patch`, `Shlwapi.h.zzz-supported-os.patch`,
`Uxtheme.h.metadata-enums.patch`, `Uxtheme.h.zzz-supported-os.patch`,
`WinUser.h.set-last-error.patch`, `WinUser.h.zz-metadata-enums.patch`,
`WinUser.h.zzz-supported-os.patch`, `cryptuiapi.h.zz-crypto-security-enums.patch`,
`magnification.h.zzz-supported-os.patch`, `memoryapi.h.set-last-error.patch`,
`mssip.h.zz-crypto-security-enums.patch`, `oleacc.h.zzz-supported-os.patch`,
`shellapi.h.zzz-supported-os.patch`, `wincrypt.h.zz-crypto-security-enums.patch`,
`wincrypt.h.zzz-resource-ownership.patch`, `winspool.h.printing-handle-ownership.patch`.

42 of the 63 headers needed the `#if defined(WIN32METADATA) #include
<win32metadata_annotations.h> #endif` guard added explicitly (28 anchored
right after an existing `#include <winapifamily.h>`, 2 - `dpapi.h`,
`MSAJTransport.h` - had no such include and got the guard added right after
their own include-guard `#define`). The remaining 21 already had it from an
earlier patch, or (for a further 12 apiset-style headers such as
`stringapiset.h`, `debugapi.h`, `namespaceapi.h`) get it transitively via an
unconditional `<minwindef.h>`/`<minwinbase.h>`/`<windows.h>` include.

## Validation

- All 63 consolidated patches replay byte-for-byte from a pristine `d154186c`
  checkout of their header.
- Zero adjacent-duplicate `_Win32_metadata_set_last_error_` lines anywhere
  under `RecompiledIdlHeaders` (checked after every insertion pass).
- `ScrapeHeaders -p:ScanArch=crossarch` run across all 71 partitions that
  transitively include any touched header (`ActiveDirectory`, `Authorization`,
  `Authorization.UI`, `Base`, `Com`, `Com.CallObj`, `Com.ChannelCreds`,
  `Com.StructuredStorage`, `Com.Urlmon`, `ComOle`, `Controls`, `Credentials`,
  `DataXchg`, `Debug`, `Direct3D9`, `DlgBox`, `Dwm`, `Enclave`, `Fax`,
  `FileHistory`, `Fs`, `Gdi`, `Gdiplus`, `Identity`, `Input.Ime`, `Intl`,
  `IO`, `LibraryLoader`, `Lwef`, `MagApi`, `Media.DShow`, `Memory`, `MenuRc`,
  `Monitor`, `MsCs`, `NetMgmt`, `P2p`, `Perf`, `Policy`, `Printing`,
  `Properties`, `Registry`, `Rpc`, `Security`, `Security.AppLocker`,
  `Security.ConfigurationSnapin`, `Security.Cryptography`,
  `Security.Cryptography.Catalog`, `Security.Cryptography.Sip`,
  `Security.Cryptography.UI`, `Security.DiagnosticDataQuery`,
  `Security.DirectoryServices`, `Security.LicenseProtection`,
  `Security.Tpm`, `Security.WinTrust`, `Security.WinWlx`, `Sens`, `Setup`,
  `Shell`, `Shutdown`, `Tablet`, `TermServ`, `Threading`,
  `TransactionServer`, `Wcs`, `Wec`, `Wer`, `WinAuto`, `WinProg`, `WinRT`,
  `WinTouch`) - all succeeded with 0 errors (arch-specific partitions
  scraped per-arch as x64/x86/arm64 automatically under crossarch).
  `AllJoyn` was excluded from validation as a pre-existing, unrelated
  toolchain blocker (`__builtin_verbose_trap`), documented in earlier
  batches of the prior RAIIFree tranche - not caused by this change.

## Residual state (final tranche)

The follow-up batch re-checked the reported 143 residual entries against
current `HEAD` using the same declaration-anchored scanner (Style 1/2
declaration matching, comment/`.idl`/`winrt/`/`STDMETHODCALLTYPE`/forwarding-shim
exclusions). This turned up a bookkeeping gap from the prior batch: the rsp
update at the end of that batch only removed the 982 names actually migrated
in that pass, not the larger "already covered" set the more-accurate v4
scanner had by then determined - so **141 of the 143** were in fact already
fully represented by pre-existing annotations (mostly from the same 830bcb6c
migration commit) and were removed from the rsp as pure reconciliation, with
no header changes.

Only **2 entries were genuinely uncovered** and required new inline
annotations:

- **`CreateDIBSection`** (`wingdi.h`) - single-line declaration
  `WINGDIAPI _Success_(return != NULL) HBITMAP WINAPI CreateDIBSection(...)`.
  Missed by the prior batch's scanner because its "looks like an expression,
  skip this candidate" prefix heuristic matched on the bare `=` character
  inside the SAL condition `_Success_(return != NULL)` (a false positive:
  `!=` is a comparison operator, not an assignment). Fixed by inserting
  `_Win32_metadata_set_last_error_` as its own line directly above the
  declaration.
- **`CryptCATAdminAddCatalog`** (`mscat.h`) - a version-guarded
  `#if (NTDDI_VERSION < NTDDI_WINBLUE) / #else` pair of redeclarations. This
  name had been manually queued for annotation in the prior batch's plan but
  the edit did not survive into the final insertion pass; both declaration
  branches now carry their own `_Win32_metadata_set_last_error_` line per
  this project's per-branch annotation convention.

Both fixes were consolidated into the existing cumulative
`wingdi.h.metadata.patch` and `mscat.h.metadata.patch` (both headers already
had a consolidated patch from earlier tranches), validated via byte-for-byte
patch replay from the pristine `d154186c` baseline, and validated via
`ScrapeHeaders -p:ScanArch=crossarch` across all partitions that reach either
header (`DataXchg`, `Direct3D9`, `Gdi`, `Intl`, `Media.DShow`, `Tablet`,
`Wcs`, `Security.Cryptography.Catalog`, `Security.Cryptography.Sip`) - Build
succeeded, 0 Error(s) (unrelated pre-existing cross-partition remap warnings
for `Security.Cryptography.Sip`/`wincrypt.h`/`schannel.h` observed, not
caused by this change).

**`WithSetLastError.rsp` now contains zero API entries.** Since the file
would otherwise be reduced to just its `--with-setlasterror` header line
with no values (behaviorally identical to the flag being entirely absent),
the file and its `<ScraperRsp Include="WithSetLastError.rsp"/>` reference in
`generation/WinSDK/Windows.Win32.proj` were both removed. No other build
file references `WithSetLastError.rsp` (`ResponseFiles` are read via
MSBuild's `@rsp` mechanism from the `ScraperRsp`/`FinalScraperRsp` item
groups only). The unrelated audit/regeneration helper script
`scripts/CreateWithSetLastError.ps1` (which re-derives a full candidate list
from Microsoft Docs UID metadata, not from the removed rsp) was left as-is;
it is not invoked by the build and is out of scope for this tranche.

No entries were silently dropped: every removal across both batches of this
tranche is either (a) redundant with an existing header annotation, (b) a
documented non-representable macro/interface-type blocker, or (c) migrated
to an inline annotation.
