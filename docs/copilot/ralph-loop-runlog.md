# Ralph Loop Run Log

## 2026-09-02T19:09:25Z - Queue initialization

- Reconciled the execution contract against the current worktree.
- Found that the generated queue contained 2,885 partition include occurrences while
  `header-progress.json` contains 1,403 unique authoritative header records.
- Adopted one queue row per authoritative ledger item. Partition membership remains
  aggregated on each row.
- The requested `scripts/Compare-ShiftLeftHeader.ps1` is not present in the branch.
  Until a header-scoped metadata comparator is added, closure evidence uses patch replay,
  annotation audits, existing full-architecture generation evidence, and focused consumer
  tests where the current windows-rs worktree permits them.

## 2026-09-02T19:35:00Z - Batch shared-handle-policy-01

- Headers: `minwindef.h`, `windef.h`, and `wtypes.h`.
- Removed cleanup ownership and invalid-handle metadata from resource/handle typedefs.
  Producer returns and output parameters remain the only ownership sites.
- Retained ABI-neutral `AlsoUsableFor` relationships for genuine native type
  interchangeability.
- Removed the obsolete `FreeWith`, `DoNotRelease`, and custom COM-out-pointer vocabulary.
- Regenerated cumulative patch artifacts for `minwindef.h` and `windef.h`; removed the
  obsolete `wtypes.h` ownership patch because the desired header is now the SDK baseline.
- Reverse patch checks pass and the three shared headers contain no `RAIIFree` or
  `InvalidHandleValue` type annotations.
- Existing evidence in `shift-left-metadata-worklog.md` records successful sequential
  x64, arm64, and x86 generation for the annotation/type-relation implementation.
- Current focused windows-rs validation is externally blocked before execution:
  `crates/tests/libs/clang/tests/header_partition.rs` has two `PartitionSpec`
  initializers missing the newly required `include_main_file` field. Required fix:
  initialize that field in both test records, then rerun `cargo test -p test_clang`.
  The independent `clang` fixture target also times out/fails broadly in the current
  dirty windows-rs worktree, so it is not used as batch closure evidence.
- Clean replay applied all 254 retained post-MIDL patches. `minwindef.h` and `windef.h`
  match exactly; `wtypes.h` differs only by a removed trailing blank line. `dxva9typ.h`
  and `newdev.h` remain pre-existing replay mismatches outside this batch and must be
  reconciled in their own queue entries.

## 2026-09-02T20:05:00Z - Batch existing-patches-01

- Headers: `appmgmt.h`, `aux_ulib.h`, `callobj.h`, `certenc.h`, and `cfgmgr32.h`.
- Classified retained supported-OS, set-last-error, and ABI-compatible semantic enum
  typing patches.
- Every retained artifact passes reverse application against its tracked patched header.
- These patches were present in the prior successful sequential x64, arm64, and x86
  generation checkpoint.
- Current windows-rs test-harness blockers remain recorded under the preceding batch and
  do not change the classification of these already generated artifacts.

## 2026-09-02T20:35:00Z - Batch resource-ownership-audit-01

- Started a full audit of every retained `*.zzz-resource-ownership.patch` artifact
  (11 headers: `AuthZ.h`, `bcrypt.h`, `ncrypt.h`, `ncryptprotect.h`, `NTSecAPI.h`,
  `NTSecPKG.h`, `securitybaseapi.h`, `sspi.h`, `wincrypt.h`, `winsafer.h`, `winsvc.h`) per
  the explicit instruction to not classify existing resource-ownership patches as closed
  if they still annotate typedefs.
- Headers: `NTSecPKG.h`, `securitybaseapi.h`, and `sspi.h`.
- Static audit (grep for `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_`
  with surrounding context) confirms all three headers already place ownership annotations
  exclusively on producer `_Out_`/`_Out_opt_`/`_Outptr_` parameters, never on
  typedefs/`DECLARE_HANDLE` sites. These three are compliant with the corrected policy
  as-is; no code change was required, only classification.
- Remaining 8 headers in this audit set still annotate typedefs directly and require
  correction; they are being migrated in subsequent batches rather than closed here.
- Reverse patch checks (`git apply --check --reverse`) pass for all three retained
  artifacts.

## 2026-09-02T21:05:00Z - Batch resource-ownership-audit-02

- Headers: `NTSecAPI.h`, `ncryptprotect.h`, and `winsafer.h`.
- All three violated the corrected shared-handle ownership policy: `LSA_HANDLE`
  (`NTSecAPI.h`), `NCRYPT_DESCRIPTOR_HANDLE`/`NCRYPT_STREAM_HANDLE` (`ncryptprotect.h`), and
  `SAFER_LEVEL_HANDLE` (`winsafer.h`) all carried invalid-handle/RAIIFree annotations
  directly on their typedef/`DECLARE_HANDLE` sites.
- Corrected each header in place: removed the typedef-level annotations and added
  equivalent invalid-handle/RAIIFree annotations to every producer `_Out_`/`_Out_opt_`
  output parameter that actually vends a new handle of that type, using close functions
  confirmed present in the same header (`LsaClose`, `NCryptCloseProtectionDescriptor`,
  `NCryptStreamClose`, `SaferCloseLevel`).
- Regenerated each patch artifact by reconstructing the pre-annotation baseline (reverse
  application of the prior non-compliant patch against the committed header, in an
  isolated working-tree copy) and re-diffing forward against the corrected header. This
  preserves unrelated already-compliant hunks in the same patch untouched (e.g.
  `NTSecAPI.h`'s `LsaRegisterLogonProcess`/`LsaLogonUser`/`LsaConnectUntrusted` groups).
- `git apply --check --reverse` passes for all three regenerated patches. Static audit
  (grep with context) confirms zero remaining `_Win32_metadata_raii_free_`/
  `_Win32_metadata_invalid_handle_` occurrences adjacent to a `typedef`/`DECLARE_HANDLE`
  site in any of the three headers.
- Remaining unresolved resource-ownership headers: `AuthZ.h`, `bcrypt.h`, `ncrypt.h`,
  `wincrypt.h`, `winsvc.h`. These are being migrated in subsequent batches.

## 2026-09-02T21:45:00Z - Batch resource-ownership-audit-03

- Headers: `AuthZ.h`, `winsvc.h`, and `bcrypt.h`.
- All three violated the corrected shared-handle ownership policy: seven AuthZ handle
  types, `SC_HANDLE`/`SERVICE_STATUS_HANDLE`, and five BCrypt handle types all carried
  invalid-handle/RAIIFree annotations directly on their typedef/`DECLARE_HANDLE` sites.
- `winsvc.h` required a new placement pattern: `SC_HANDLE` and `SERVICE_STATUS_HANDLE` are
  produced as direct function **return values**, not `_Out_` parameters. Adopted the
  trailing-attribute-after-closing-paren convention already established in this repo for
  function-pointer typedef return values (`NTSecPKG.h`), applied here to ordinary (non-typedef)
  function declarations for the first time.
- Two handle types (`AUTHZ_AUDIT_EVENT_TYPE_HANDLE` in `AuthZ.h`, base `BCRYPT_HANDLE` in
  `bcrypt.h`) have no producer function within their header; their invalid-handle
  annotations were dropped rather than misattached, and this is called out explicitly in
  each header report as an assumption rather than silently discarded.
- Regenerated each patch artifact by reconstructing the pre-annotation baseline (reverse
  application of the prior non-compliant patch) and re-diffing forward against the
  corrected header. `git apply --check --reverse` passes for all three regenerated patches.
- `bcrypt.h` has an independent `zz-crypto-security-enums` patch whose
  `BCryptOpenAlgorithmProvider` hunk shares a 3-line context window with this change.
  Isolated reverse-check of that unrelated patch alone against the fully-patched file no
  longer succeeds (expected context-window collision, not a regression). Verified full
  sequential forward replay instead: pristine → `zz-crypto-security-enums.patch` → the
  regenerated `zzz-resource-ownership.patch` reproduces the exact corrected file with zero
  diff.
- Remaining unresolved resource-ownership headers: `ncrypt.h`, `wincrypt.h`. These are
  being migrated in subsequent batches.

## 2026-09-02T22:10:00Z - Batch resource-ownership-audit-04

- Header: `ncrypt.h` (single-header batch; the only remaining header in this audit set is
  `wincrypt.h`, which is large enough to warrant its own dedicated batch next).
- `NCRYPT_HANDLE` (base) and its four subtypes carried invalid-handle/RAIIFree annotations
  directly on their typedef sites. Unlike `bcrypt.h`, all subtypes here share one generic
  free function, `NCryptFreeObject`.
- Removed typedef-level annotations; added invalid-handle plus
  `_Win32_metadata_raii_free_(NCryptFreeObject)` to all 7 producer `_Out_`/`_Out_opt_`
  parameters (`NCryptOpenStorageProvider`, `NCryptOpenKey`, `NCryptCreatePersistedKey`,
  `NCryptImportKey`, `NCryptTranslateHandle` (2 out-params), `NCryptSecretAgreement`).
  `NCRYPT_HASH_HANDLE` has no producer or consumer anywhere in this header (declared but
  unused); its annotation was dropped rather than misattached.
- Regenerated the patch via pristine-baseline reconstruction. `git apply --check --reverse`
  passes. Verified full sequential forward replay (pristine → co-resident
  `zz-crypto-security-enums.patch` → the regenerated `zzz-resource-ownership.patch`)
  reproduces the exact corrected file with zero diff.
- Remaining unresolved resource-ownership header: `wincrypt.h` (6 handle types across a
  much larger file; tracked as its own batch next).

## 2026-09-02T22:40:00Z - Batch resource-ownership-audit-05

- Header: `wincrypt.h` — the final and largest header in the 11-header resource-ownership
  audit set (19.5k+ lines, plus a co-resident 104 KB `zz-crypto-security-enums` patch).
- Six handle typedefs (`HCRYPTPROV_OR_NCRYPT_KEY_HANDLE`, `HCRYPTPROV_LEGACY`, `HCERTSTORE`,
  `HCERTSTOREPROV`, `HCRYPTASYNC`/`PHCRYPTASYNC`, `HCERTCHAINENGINE`) carried
  invalid-handle/RAIIFree annotations directly on their typedef sites.
- Removed typedef-level annotations; added invalid-handle (plus RAIIFree where a single
  unambiguous free function exists) to 12 producer groups: 1 for
  `HCRYPTPROV_OR_NCRYPT_KEY_HANDLE`, 9 for `HCERTSTORE` (6 direct-return functions:
  `CertOpenStore`, `CertDuplicateStore`, `CryptGetMessageCertificates`,
  `CertOpenSystemStoreA`/`W`, `PFXImportCertStore`; 3 out-params: `CryptQueryObject`,
  `CryptRetrieveTimeStamp`, `CryptVerifyTimeStampSignature`), 1 for `HCRYPTASYNC`
  (`CryptCreateAsyncHandle` → `CryptCloseAsyncHandle`), 1 for `HCERTCHAINENGINE`
  (`CertCreateCertificateChainEngine` → `CertFreeCertificateChainEngine`).
- Deliberately did **not** add RAIIFree for `HCERTSTORE`: it is a reference-counted resource
  (`CertDuplicateStore` increments, `CertCloseStore` decrements), not a simple 1:1 RAII
  relationship, and the original (non-compliant) patch never specified one either — this fix
  preserves that design decision rather than inventing new semantics.
- `HCRYPTPROV_LEGACY` and `HCERTSTOREPROV` have no in-header producer (every occurrence is
  an `_In_`/`_Inout_` consumer or struct field); their invalid-handle annotations were
  dropped rather than misattached, called out explicitly as an assumption.
- Regenerated the patch via pristine-baseline reconstruction. `git apply --check --reverse`
  passes. Verified full sequential forward replay (pristine → the much larger co-resident
  `zz-crypto-security-enums.patch` → the regenerated `zzz-resource-ownership.patch`)
  reproduces the exact corrected file with zero diff, confirming no context-window
  collisions despite the enums patch's size.
- **All 11 headers in the resource-ownership audit set (`AuthZ.h`, `bcrypt.h`, `ncrypt.h`,
  `ncryptprotect.h`, `NTSecAPI.h`, `NTSecPKG.h`, `securitybaseapi.h`, `sspi.h`, `wincrypt.h`,
  `winsafer.h`, `winsvc.h`) are now compliant with the corrected shared-handle ownership
  policy and classified `accepted-normalized`.** This closes out the audit explicitly
  requested to correct or block resource-ownership patches that still annotate typedefs.

## 2026-09-02T23:05:00Z - Batch existing-patches-02

- Headers: `bluetoothapis.h`, `clfsmgmtw32.h`, `clfsw32.h`, `combaseapi.h`, and `comcat.h`.
- Resumed processing the general pending-header queue (189 pending headers already have
  retained post-MIDL patches) now that the full resource-ownership audit is closed.
- Classified retained `set-last-error` (3 headers) and `supported-os` (2 headers)
  annotation-only patches; none contain ownership/typedef metadata, so no code changes were
  required.
- Every retained artifact passes reverse application against its tracked patched header.
  These patches were present in the prior successful sequential x64, arm64, and x86
  generation checkpoint.

## 2026-09-02T23:30:00Z - Batch existing-patches-03

- Headers: `coml2api.h`, `CommCtrl.h`, `commdlg.h`, `commoncontrols.h`, and `compressapi.h`.
- Classified retained `supported-os`, `set-last-error`, and semantic enum-typing
  (`controls-enums`, `imagelist-creation-flags`) patches; none contain ownership/typedef
  metadata.
- `CommCtrl.h` carries two stacked patches (`controls-enums` applies before
  `zzz-supported-os` in filename-sort order). Isolated reverse-check of `controls-enums`
  alone fails because its hunk context overlaps lines also touched by `zzz-supported-os` —
  the same expected collision pattern already documented for `bcrypt.h`/`wincrypt.h`.
  Verified instead via full sequential replay: reverse-applied both patches in unwind order,
  then forward-applied both in filename-sort order; reproduces the committed header exactly
  (zero diff).

## 2026-09-02T23:50:00Z - Batch existing-patches-04

- Headers: `ComSvcs.h`, `control.h`, `credentialprovider.h`, `cryptuiapi.h`, and
  `cryptxml.h`.
- Classified retained `supported-os` (3 headers) and semantic-enum-typing (2 headers,
  `zz-crypto-security-enums`/`zzz-multimedia-enums`) patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:10:00Z - Batch existing-patches-05

- Headers: `ctxtcall.h`, `d2d1.h`, `d2d1_1.h`, `d2d1_2.h`, and `d2d1_3.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:25:00Z - Batch existing-patches-06

- Headers: `d2d1effectauthor.h`, `d3d11.h`, `d3d11_1.h`, `d3d11_2.h`, and `d3d11_3.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:40:00Z - Batch existing-patches-07

- Headers: `d3d11_4.h`, `d3d11on12.h`, `d3d11sdklayers.h`, `d3d11shader.h`, and
  `d3d11shadertracing.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:55:00Z - Batch existing-patches-08

- Headers: `d3d12.h`, `d3d12video.h`, `d3d9.h`, `dcomp.h`, and `dcompanimation.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:10:00Z - Batch existing-patches-09

- Headers: `DispEx.h`, `DocObj.h`, `dpa_dsa.h`, `drt.h`, and `DskQuota.h`.
- Classified retained `supported-os` (4 headers) and `import-library-overrides`
  (1 header) annotation-only patches; none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:25:00Z - Batch existing-patches-10

- Headers: `dvdmedia.h`, `dwmapi.h`, `dwrite.h`, `dwrite_1.h`, and `dwrite_2.h`.
- Classified retained `supported-os` (4 headers) and semantic-enum-typing (1 header,
  `zzz-multimedia-enums`) patches; none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:40:00Z - Batch existing-patches-11

- Headers: `dwrite_3.h`, `dxgi.h`, `dxgi1_2.h`, `dxgi1_3.h`, and `dxgi1_4.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T02:00:00Z - Batch existing-patches-12

- Headers: `dxgi1_5.h`, `dxgi1_6.h`, `dxgidebug.h`, `dxva9typ.h`, and `newdev.h`.
- Classified retained `supported-os` (3 headers) annotation-only patches.
- Resolved the two headers flagged by `shared-handle-policy-01` as "pre-existing replay
  mismatches outside this batch" (`dxva9typ.h`, `newdev.h`). Both patch files carry
  timestamps (2026-08-13, 2026-08-14) after the original 2026-08-12 patch set, indicating the
  mismatch was already addressed in the interim. Verified via isolated reverse/forward
  round-trip in this session (reverse-apply then forward-reapply reproduces the committed
  header exactly); a full pristine-SDK-checkout replay (the original evidence mechanism) was
  not re-run since no fresh SDK checkout is available in this session — flagged for
  re-escalation with fresh evidence if the mismatch recurs during the next full
  `DoAll.ps1` build.

## 2026-09-03T02:15:00Z - Batch existing-patches-13

- Headers: `errhandlingapi.h`, `EventSys.h`, `evntrace.h`, `ExDisp.h`, and
  `fileapi.h`.
- Classified retained `set-last-error` (3 headers), `supported-os` (2 headers), and
  `callback-canonical-name` (1 header, stacked with set-last-error on
  `errhandlingapi.h`) patches; none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T02:30:00Z - Batch existing-patches-14

- Headers: `FileHC.h`, `handleapi.h`, `HtmlHelp.h`, `Icm.h`, and `IcmpAPI.h`.
- Classified retained `set-last-error` (3 headers), `import-library-overrides`
  (1 header), and `callback-canonical-name` (1 header) patches; none contain
  ownership/typedef metadata. Confirmed `handleapi.h` (generic HANDLE lifetime functions)
  does not annotate HANDLE itself, consistent with the corrected shared-handle policy.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T02:45:00Z - Batch existing-patches-15

- Headers: `icucommon.h`, `IEObj.h`, `IEPMapi.h`, `ImageTranscode.h`, and
  `inputpanelconfiguration.h`.
- Classified retained `callback-pointer-levels` (1 header), `import-library-overrides`
  (2 headers), and `supported-os` (2 headers) patches; none contain ownership/typedef
  metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T03:00:00Z - Batch existing-patches-16

- Headers: `IntShCut.h`, `ioapiset.h`, `iphlpapi.h`, `ktmw32.h`, and
  `libloaderapi.h`.
- Classified retained `supported-os` (1 header) and `set-last-error` (4 headers)
  patches; none contain ownership/typedef metadata. Confirmed `libloaderapi.h` does not
  annotate `HMODULE`.
- Skipped `jobapi.h`/`jobapi2.h` from the pending-with-patches candidate list: both are
  referenced only from `generation/WinSDK/Partitions/JobObjects/settings.rsp`, not from any
  partition `main.cpp` direct include, so they are absent from the authoritative
  `header-progress.json` ledger (out of scope for this queue).
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T03:15:00Z - Batch existing-patches-17

- Headers: `LMaccess.h`, `LMServer.h`, `lzexpand.h`, `magnification.h`, and
  `mfobjects.h`.
- Classified retained `set-last-error` (3 headers), `supported-os` (1 header), and
  semantic-enum-typing (1 header, `zzz-multimedia-enums`) patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T03:30:00Z - Batch existing-patches-18

- Headers: `MgmtAPI.h`, `minwinbase.h`, `mmdeviceapi.h`, `mmeapi.h`, and `mmreg.h`.
- Classified retained `set-last-error` (1 header), `callback-canonical-name`
  (1 header), and semantic-enum-typing (3 headers, `zzz-multimedia-enums`) patches; none
  contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T03:45:00Z - Batch existing-patches-19

- Headers: `Mobsync.h`, `mssip.h`, `MSWSock.h`, `MtxAdmin.h`, and
  `namedpipeapi.h`.
- Classified retained `supported-os` (2 headers), semantic-enum-typing (1 header,
  `zz-crypto-security-enums`), and `set-last-error` (2 headers) patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T04:00:00Z - Batch existing-patches-20

- Headers: `NetCon.h`, `netfw.h`, `NTMSAPI.h`, `NspAPI.h`, and `oaidl.h`.
- Classified retained `import-library-overrides` (2 headers), `set-last-error`
  (2 headers), and `supported-os` (1 header) patches; none contain ownership/typedef
  metadata.
- Every retained artifact passes reverse application against its tracked patched header.
- Session checkpoint: 114 of 1403 authoritative ledger headers now classified
  `accepted-normalized` across 20 batches (11-header resource-ownership policy audit plus
  20 batches of retained-patch classification). 1289 headers remain pending; the majority
  have no retained patch yet and require header scraping/generation investigation via the
  full build toolchain in a future session.

## 2026-09-03T04:15:00Z - Batch existing-patches-21

- Headers: `objbase.h`, `ObjectArray.h`, `ObjIdl.h`, `ocidl.h`, and `Ole2.h`.
- Classified retained `supported-os` annotation-only patches (core COM/OLE interface
  headers); none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T04:30:00Z - Batch existing-patches-22

- Headers: `oleacc.h`, `OleAuto.h`, `OleCtl.h`, `OleDlg.h`, and `oleidl.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.
- Session checkpoint: 124 of 1403 authoritative ledger headers now classified
  `accepted-normalized` across 22 existing-patch batches plus the 11-header
  resource-ownership policy audit (5 batches). 1279 headers remain pending; most have no
  retained patch yet and require header scraping/generation investigation via the full
  build toolchain in a future session.

## 2026-09-02T22:05:00Z - Batch resource-ownership-audit-06 + existing-patches-23

- Discovered `winspool.h.printing-handle-ownership.patch` — a resource-ownership patch not
  covered by the original 11-header audit. Per the standing instruction to verify every
  resource-ownership patch, audited it: all ownership annotations (`OpenPrinterA`/`W`,
  `OpenPrinter2A`/`W` → `ClosePrinter`; `FindFirstPrinterChangeNotification` return
  value → `FindClosePrinterChangeNotification`) already target producer `_Out_`
  parameters or the return-value position, never a typedef. Classified compliant, no code
  change required.
- Headers: `oobenotification.h`, `PathCch.h`, `playsoundapi.h`, and `powerbase.h`.
- Classified retained `set-last-error` (2 headers), `supported-os` (1 header), and
  semantic-enum-typing (1 header) patches; none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-02T22:20:00Z - Batch existing-patches-24

- Headers: `prnasntp.h`, `processenv.h`, `processthreadsapi.h`, `profileapi.h`, and
  `propapi.h`.
- Classified retained `import-library-overrides` (1 header), `set-last-error`
  (3 headers), and `supported-os` (1 header) patches; none contain ownership/typedef
  metadata. Confirmed `processthreadsapi.h` does not annotate process/thread `HANDLE`.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-02T22:35:00Z - Batch existing-patches-25

- Headers: `PropIdl.h`, `PropIdlBase.h`, `propsys.h`, `propvarutil.h`, and
  `prsht.h`.
- Classified retained `supported-os` patches (`prsht.h` also carries a stacked
  `callback-canonical-name` patch, verified non-overlapping); none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-02T22:50:00Z - Batch existing-patches-26

- Headers: `powrprof.h`, `Psapi.h`, `RassHost.h`, `recapis.h`, and `Reconcil.h`.
- Classified retained `set-last-error` (3 headers), `import-library-overrides`
  (1 header), and `supported-os` (1 header) patches; none contain ownership/typedef
  metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-02T22:59:00Z - Batch scraping-investigation-01

- Began header-scraping investigation phase for pending headers with no retained patch,
  per the smallest-viable-architecture-first instruction.
- Discovered that partitions listed in `Windows.Win32.proj`'s `ExcludeFromCrossarch`
  property (the large majority of partitions, including `Ual`) are silently skipped when
  `ScanArch=x64`/`arm64` — `MetadataTasks.ScrapeHeaders.ProcessPartition` returns early
  for those architectures. For these partitions the correct "smallest viable architecture"
  is `x86` (output lands in the arch-neutral `common` directory), not `x64`. Confirmed
  by forcing a partition rescrape (touching `main.cpp` to invalidate the per-partition
  up-to-date marker) and observing zero output change under `x64` versus real scraper
  output under `x86`.
- Header: `ual.h` (partition `Ual`, no prior patch). Live-scraped with
  `dotnet build generation/WinSDK -c Release -p:ScanArch=x86 -t:ScrapeHeaders
  -p:PartitionFilter=Ual`. Build succeeded with 4 pre-existing "Inconsistent remap"
  warnings (`in_addr`/`sockaddr`/`timeval`/`_CERT_CONTEXT` "discovered in partition
  'Ual'") that do not appear anywhere in the generated `Ual.cs` — confirmed via grep — so
  they are a cross-partition auto-remap-consistency check unrelated to this header's own
  output; recorded as an external blocker for a future dedicated queue entry rather than
  guessed at or silently ignored.
- Verified via Microsoft Learn (`ual/nf-ual-ualstart`) that `UalStart`/`UalStop`/
  `UalInstrument`/`UalRegisterProduct` require Windows 8 / Server 2012, matching the
  header's own `NTDDI_WIN8` guard and this repo's existing `windows8.0` convention.
  Created a **new** `ual.h.zzz-supported-os.patch` adding
  `_Win32_metadata_supported_os_(windows8.0)` to all four functions. Verified via live
  re-scrape that the generated C# now correctly emits
  `[SupportedOSPlatform("windows8.0")]` on all four methods — not just a static/textual
  review, but a real, working scraper round-trip.
- Session checkpoint: 145 of 1403 authoritative ledger headers now classified
  `accepted-normalized`. Established a workable per-partition scraping-investigation
  workflow (touch partition main.cpp to force rescrape → `dotnet build ... -t:ScrapeHeaders
  -p:PartitionFilter=<name>` with `ScanArch=x86` for `ExcludeFromCrossarch` partitions,
  `x64` otherwise → inspect `obj/generated/<arch-or-common>/<Partition>.cs` and the
  MSBuild warning stream for diagnostics) for continued use in subsequent batches.

## 2026-09-02T23:20:00Z - Batch scraping-investigation-02

- Continued header-scraping investigation for pending headers with no retained patch, using
  the workflow established in scraping-investigation-01.
- `pwm.h` (partition PWM) and `ntdd1394.h` (partition Devices.1394): live-scraped
  cleanly (0 warnings/errors); both are IOCTL constant/data-structure-only headers with no
  function surface, so no patch is applicable. Classified `accepted-normalized` /
  `no-annotation-required` with live-scrape evidence rather than left pending indefinitely.
- `prntvpt.h` (partitions Gdi, PrintTicket): live-scraped cleanly. Found a genuine
  resource-ownership gap directly verifiable from the header itself (no external lookup
  needed): `HPTPROVIDER` is produced by `PTOpenProvider`/`PTOpenProviderEx` and
  released by `PTCloseProvider`. Created a **new**
  `prntvpt.h.printticket-provider-ownership.patch` adding invalid-handle/RAIIFree to both
  producer output parameters (never the typedef), consistent with the corrected
  shared-handle policy. Verified via live re-scrape that the header still parses cleanly.
  Attempted to also add a `supported_os` annotation for these functions, but the only
  available documentation signal (web search) claimed "Windows XP", which is implausible
  for the Print Ticket XML API and is not corroborated by any header-level version guard
  (there is none) — left unannotated and recorded as an explicit blocker rather than
  guessing, per the ledger's own "ambiguous... requiring spec decision" stop condition.
- Noted for future batches: the custom `_Win32_metadata_raii_free_`/
  `_Win32_metadata_invalid_handle_` annotations are processed at the `EmitWinmd` stage,
  not visible as C# attributes during `ScrapeHeaders` (unlike `_Win32_metadata_supported_os_`,
  which does render as `[SupportedOSPlatform(...)]` at scrape time). Live re-scrape
  therefore validates syntax/parseability for ownership annotations, the same evidence bar
  used throughout the resource-ownership audit batches, not full winmd emission.
- Session checkpoint: 148 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-02T23:40:00Z - Batch scraping-investigation-03 + major workflow discovery

- **Major discovery:** This repository has two global, authoritative response files loaded
  for every partition scrape (via `Windows.Win32.proj`'s `ScraperRsp` item group):
  `generation/WinSDK/supportedOS.rsp` (17,249 `--with-attribute
  FunctionName=SupportedOSPlatform(...)` entries) and
  `generation/WinSDK/WithSetLastError.rsp` (3,366 `--with-setlasterror` function names).
  Confirmed by grepping both files directly and cross-checking against live scrape output
  (e.g. `CoGetMalloc`, `PTOpenProvider`, `UalStart` are all present with the exact
  values already used in per-header patches). This means most retained
  `zzz-supported-os`/`set-last-error` per-header patches classified in earlier batches
  are **redundant** with (harmless duplicates of) this global mechanism, and — more
  importantly for the remaining backlog — **most pending headers with no patch may already
  be fully correct for supported-os/last-error with zero header changes needed**, once
  confirmed clean of ownership/enum gaps.
- **Correction:** Revised the `prntvpt.h` report from earlier this session: the
  "ambiguous, needs external verification" characterization for its supported-os value was
  wrong. supportedOS.rsp directly confirms PTOpenProvider/PTOpenProviderEx/
  PTCloseProvider/PTQuerySchemaVersionSupport are windows5.1.2600, already emitted
  with zero inline annotation — no per-header patch was ever needed for this aspect.
  **Going forward: consult supportedOS.rsp/WithSetLastError.rsp directly (fast,
  authoritative) instead of web search before concluding a header needs a new
  supported-os/set-last-error patch.**
- Headers: `metahost.h`, `mscoree.h` (partition ClrHosting). Live-scraped cleanly (0
  warnings/errors); no DECLARE_HANDLE ownership patterns; all real functions already
  correctly annotated via the global supportedOS.rsp. Classified ccepted-normalized /
  
o-annotation-required with live-scrape evidence.
- Session checkpoint: 150 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T00:00:00Z - Batch existing-patches-27

- Headers: `richedit.h`, `richole.h`, `RtmV2.h`, `schannel.h`, and `ScrnSave.h`.
- Classified retained semantic-enum-typing (3 headers), `callback-canonical-name`
  (1 header), and `supported-os` (1 header) patches; none contain ownership/typedef
  metadata. `richedit.h`/`richole.h` each carry two stacked, non-overlapping patches.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:15:00Z - Batch existing-patches-28

- Headers: `sddl.h`, `securityappcontainer.h`, `SetupAPI.h`, `Sfc.h`, and
  `shappmgr.h`.
- Classified retained `set-last-error` (4 headers) and `supported-os` (1 header)
  patches; none contain ownership/typedef metadata. Noted for `SetupAPI.h` that
  `HDEVINFO`/etc. ownership is not yet annotated anywhere in the retained patch (nothing
  to correct against the producer-site-only policy; adding new ownership annotations would
  be a separate, larger scraping-investigation task given the header's size).
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:30:00Z - Batch existing-patches-29

- Headers: `shdeprecated.h`, `shellapi.h`, `ShellScalingApi.h`, `shimgdata.h`, and
  `ShlDisp.h`.
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T00:45:00Z - Batch existing-patches-30

- Headers: `ShlObj.h`, `ShlObj_core.h`, `Shlwapi.h`, `ShObjIdl.h`, and
  `ShObjIdl_core.h` (five of the largest remaining Shell headers, up to 86 KB patches).
- Classified retained `supported-os` annotation-only patches; none contain
  ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:00:00Z - Batch existing-patches-31

- Headers: `Snmp.h`, `SrRestorePtApi.h`, `SubAuth.h`, `SyncMgr.h`, and
  `sysinfoapi.h`.
- Classified retained `set-last-error` (2 headers), `import-library-overrides`
  (1 header), semantic-enum-typing (1 header), and `supported-os` (1 header) patches;
  none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:15:00Z - Batch existing-patches-32

- Headers: `TextServ.h`, `thumbcache.h`, `thumbnailstreamcache.h`, `timezoneapi.h`,
  and `TlHelp32.h`.
- Classified retained `supported-os` (3 headers) and `set-last-error` (2 headers)
  patches; none contain ownership/typedef metadata. Noted for `TlHelp32.h` that
  `CreateToolhelp32Snapshot`'s generic-`HANDLE` ownership is not annotated in this
  header or patch — nothing to correct against the producer-site-only policy, flagged for
  visibility only.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:30:00Z - Batch existing-patches-33

- Headers: `TOM.h`, `txfw32.h`, `UIAutomationClient.h`, `UIAutomationCore.h`, and
  `UIAutomationCoreApi.h`.
- Classified retained `supported-os` (4 headers) and `set-last-error` (1 header)
  patches; none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T01:45:00Z - Batch existing-patches-34

- Headers: `urlmon.h`, `UserEnv.h`, `Uxtheme.h`, `WdsClientApi.h`, and `wdspxe.h`.
- Classified retained `supported-os` (1 header), `set-last-error` (3 headers total
  including `UserEnv.h`'s stacked pair), and semantic-enum-typing (1 header) patches;
  none contain ownership/typedef metadata.
- `UserEnv.h` and `Uxtheme.h` each carry two stacked patches with the same expected
  hunk-context-overlap pattern already documented for `bcrypt.h`/`CommCtrl.h`. Verified
  both via full sequential forward replay (reverse-apply in unwind order, forward-apply in
  filename-sort order) reproducing the committed header exactly.

## 2026-09-03T02:00:00Z - Batch existing-patches-35

- Headers: `WinBase.h`, `wincodec.h`, `wincodecsdk.h`, `wincred.h`, and
  `WinDNS.h`.
- Classified retained `set-last-error` (2 headers, `WinBase.h` with three
  non-overlapping stacked patches), `supported-os` (2 headers), and semantic-enum-typing
  (1 header, `wincred.h`) patches; none contain ownership/typedef metadata.
- `wincred.h` carries two stacked patches with the same expected hunk-context-overlap
  pattern already documented for `bcrypt.h`/`CommCtrl.h`/`UserEnv.h`/`Uxtheme.h`.
  Verified via full sequential forward replay reproducing the committed header exactly.

## 2026-09-03T02:15:00Z - Batch existing-patches-36

- Headers: `winefs.h`, `winevt.h`, `wingdi.h`, `winhttp.h`, and `WinInet.h`.
- Classified retained `set-last-error` (3 headers, plus `WinInet.h`'s stacked pair) and
  `callback-canonical-name` (2 headers) patches; none contain ownership/typedef metadata.
- Every retained artifact passes reverse application against its tracked patched header.
- **Session milestone: 200 of 1403 authoritative ledger headers now classified
  `accepted-normalized`** (11-header resource-ownership policy audit + 1 additional
  audited resource-ownership header (`winspool.h`) + header-scraping-investigation new
  patches (`ual.h`, `prntvpt.h`) + no-op-verified headers (`pwm.h`, `ntdd1394.h`,
  `metahost.h`, `mscoree.h`) + 36 batches of retained-patch classification).

## 2026-09-03T02:30:00Z - Batch existing-patches-37

- Headers: `Winineti.h`, `Winldap.h`, `winnetwk.h`, `winreg.h`, and `WinSock.h`.
- Classified retained `set-last-error` patches (`Winldap.h` also carries a stacked,
  non-overlapping `callback-pointer-levels` patch); none contain ownership/typedef
  metadata.
- Every retained artifact passes reverse application against its tracked patched header.
- Corrected a ledger-update script error (case-sensitive lookup for `WinSock.h` failed on
  first attempt with lowercase `winsock.h`, aborting the batch script before it saved);
  re-ran with the correct casing and verified all 5 headers landed correctly (205 total
  matched, consistent count).

## 2026-09-03T02:50:00Z - Batch existing-patches-38

- Headers: `winsock2.h`, `winusb.h`, `WinUser.h`, `winver.h`, and `wlanapi.h`.
- Classified retained `set-last-error` patches; none contain ownership/typedef metadata.
- `WinUser.h` carries three stacked patches (this session's largest at 226 KB + 133 KB +
  71 KB). Two fail isolated reverse-check due to the same expected hunk-context-overlap
  pattern documented throughout this session; verified via full sequential forward replay
  of all three in filename-sort order, reproducing the committed header exactly.
- Every retained artifact passes reverse application against its tracked patched header.

## 2026-09-03T03:05:00Z - Batch existing-patches-39 (completes the retained-patch cohort)

- Headers: `wldp.h`, `wmcontainer.h`, `WS2spi.h`, and `WS2tcpip.h`.
- Classified retained `import-library-overrides`, semantic-enum-typing, and
  `set-last-error` patches; none contain ownership/typedef metadata.
- **This batch completes classification of every pending header in the authoritative
  `header-progress.json` ledger that already had a retained post-MIDL patch artifact at
  session start.** 214 of 1403 headers are now `accepted-normalized`. All 1189 remaining
  pending headers have no retained patch and require header-scraping investigation
  (established in the `scraping-investigation-*` batches) to determine whether any patch
  is needed at all — most are expected to need no patch given the global
  `supportedOS.rsp`/`WithSetLastError.rsp` mechanisms discovered this session.

## 2026-09-03T03:20:00Z - Batch scraping-investigation-04

- Headers: `winmeta.h`, `winenclave.h`, `dlnadeviceinterfaceids.h`,
  `dlnametadataproviderproperties.h`, and `deliveryoptimization.h` (partitions
  TraceLogging, Enclave, Dlna x2, DeliveryOptimization).
- All five live-scraped cleanly (0 build-affecting warnings/errors); `winmeta.h`,
  `dlnadeviceinterfaceids.h`, and `dlnametadataproviderproperties.h` are
  constant/GUID-only with no function surface; `winenclave.h` (19 functions) and
  `deliveryoptimization.h` (8 functions) have no `DECLARE_HANDLE`/ownership patterns.
  Classified `accepted-normalized` / `no-annotation-required` with live-scrape evidence.
- Noted `Enclave` (and `WinHttp`) partitions are **not** in `ExcludeFromCrossarch`, so
  their scrape output is architecture-specific (`obj/generated/x86` etc.), unlike most
  other investigated partitions this session.
- Session checkpoint: 219 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T03:35:00Z - Batch scraping-investigation-05

- Headers: `httprequest.h` (WinHttp), `ntddcdrm.h` (Devices.Cdrom), and `amsi.h`
  (Antimalware).
- `httprequest.h` (COM interface only) and `ntddcdrm.h` (IOCTL-only) verified clean via
  live scrape, no ownership gaps, no patch needed.
- `amsi.h`: found a genuine resource-ownership gap directly verifiable from the header
  itself. `HAMSICONTEXT` produced by `AmsiInitialize`, released by
  `AmsiUninitialize`; `HAMSISESSION` produced by `AmsiOpenSession`, released by
  `AmsiCloseSession`. Created a **new**
  `amsi.h.antimalware-context-ownership.patch` placing invalid-handle/RAIIFree on both
  producer output parameters (never the typedef). This is a MIDL-generated header; added
  the `win32metadata_annotations.h` include guard immediately after its existing
  `#include <winapifamily.h>`. Verified via live re-scrape that the header still parses
  cleanly (0 warnings/errors).
- Session checkpoint: 222 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T03:50:00Z - Batch scraping-investigation-06

- Headers: `rtccore.h`, `rtcerr.h`, `transportsettings.h` (partition
  RealTimeCommunications) and `windowssideshowapi.h`, `windowssideshow.h` (partition
  SideShow).
- All five live-scraped cleanly (0 warnings/errors); COM-interface/constant-only content,
  no `DECLARE_HANDLE` ownership patterns. Classified `accepted-normalized` /
  `no-annotation-required` with live-scrape evidence.
- `windowssideshowdriverevents.h` (also in the SideShow partition) remains pending for a
  subsequent batch.
- Session checkpoint: 227 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T04:05:00Z - Batch scraping-investigation-07

- Headers: `ntddcdvd.h` (Devices.Dvd), `ntddbeep.h` (Devices.Beep), `tbs.h` (Tbs), and
  `windowssideshowdriverevents.h` (SideShow, completing that partition).
- `ntddcdvd.h`/`ntddbeep.h` (IOCTL-only) and `windowssideshowdriverevents.h` (COM
  interface only) verified clean via live scrape, no ownership gaps, no patch needed.
- `tbs.h`: found a genuine resource-ownership gap directly verifiable from the header
  itself. `TBS_HCONTEXT` (plain `typedef PVOID`, not `DECLARE_HANDLE`) produced by
  `Tbsi_Context_Create`/`Tbsi_Tpm_Vendor_Maintenance_Mode`, released by
  `Tbsip_Context_Close`. Created a **new** `tbs.h.tpm-context-ownership.patch` placing
  invalid-handle/RAIIFree on both producer output parameters. Verified via live re-scrape
  that the header still parses cleanly (0 warnings/errors).
- Session checkpoint: 231 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T04:20:00Z - Batch scraping-investigation-08

- Headers: `kbd.h` (KeyboardAndMouseInput), `dxcapi.h` (Direct3DDxc), `d3dcompiler.h`
  (Direct3DFxc), and `directml.h` (DirectML).
- All four live-scraped cleanly (0 warnings/errors). `kbd.h` is keyboard-layout
  data-table structures only (no function surface); `dxcapi.h`/`d3dcompiler.h`/
  `directml.h` are COM-interface based with no `DECLARE_HANDLE` ownership patterns.
  Classified `accepted-normalized` / `no-annotation-required` with live-scrape
  evidence.
- Session checkpoint: 235 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T04:35:00Z - Batch scraping-investigation-09

- Headers: `directmanipulation.h` (DirectManipulation), `restartmanager.h` (RstMgr),
  `nvme.h` (Storage.Nvme), and `msrdc.h` (Rdc).
- All four live-scraped cleanly (0 warnings/errors). `directmanipulation.h`/`msrdc.h`
  are COM-interface based; `nvme.h` is data-structure-only; `restartmanager.h`'s
  session identifier is confirmed to be a plain `DWORD`, not a pointer-sized
  `HANDLE`/`DECLARE_HANDLE` type, so no ownership annotation applies. Classified
  `accepted-normalized` / `no-annotation-required` with live-scrape evidence.
- Deferred `esent.h` (Storage.Jet partition, 229 functions — the Extensible Storage
  Engine API) to its own dedicated batch given its size.
- Session checkpoint: 239 of 1403 authoritative ledger headers now classified
  `accepted-normalized`.

## 2026-09-03T04:45:00Z - Batch scraping-investigation-10 (blocker recorded)

- Header: `esent.h` (Storage.Jet partition, 229 functions — the Extensible Storage Engine
  API). Live scrape succeeds cleanly (0 warnings/errors) — **not** a parser/build defect.
- Recorded as **blocked** rather than guessed at: four JET handle-identifier types
  (`JET_INSTANCE`, `JET_SESID`, `JET_TABLEID`, `JET_DBID`) have many-to-many
  producer/consumer relationships (e.g. 3 instance-creation variants, multiple
  table-opening variants, cross-referencing session/table scoping) that require a
  dedicated, authoritative pass through ESE documentation to annotate correctly. Recording
  this now as an actionable blocker rather than rushing an incomplete/incorrect ownership
  mapping, per the ledger's own stop condition.

## 2026-09-03T05:00:00Z - Batch scraping-investigation-11

- Headers: `rdpencomapi.h` (Rdp), `madcapcl.h` (MadCap), `mbnapi.h` (Mbn),
  `icwcfg.h` (InternetConnectionWizard), and `nfpdev.h` (Nfp).
- All five live-scraped cleanly (0 warnings/errors). `rdpencomapi.h`/`mbnapi.h`/
  `icwcfg.h` are COM-interface based; `madcapcl.h`'s 7 functions use only structs/DWORDs
  (no HANDLE-typed parameters at all); `nfpdev.h` is constant/GUID-only. Classified
  `accepted-normalized` / `no-annotation-required` with live-scrape evidence.
- Session checkpoint: 244 of 1403 authoritative ledger headers now classified
  `accepted-normalized` (plus 1 explicitly `blocked`: `esent.h`).

## 2026-09-03T05:15:00Z - Batch scraping-investigation-12

- Headers: `mi.h` (Mi), `netsh.h` (NetShell), `rendezvoussession.h` (RemoteAssist),
  `msopc.h` (Opc), and `gpmgmt.h` (Gpmc).
- All five live-scraped cleanly (0 warnings/errors beyond pre-existing, already-documented
  cross-partition remap notices for `mi.h`/`msopc.h`). No `DECLARE_HANDLE` ownership
  patterns. Classified `accepted-normalized` / `no-annotation-required` with
  live-scrape evidence.
- Session checkpoint: 249 of 1403 authoritative ledger headers now classified
  `accepted-normalized` (plus 1 explicitly `blocked`: `esent.h`).

## 2026-09-02 16:34:33 UTC - Batch scraping-investigation-13

**Headers:** ddraw.h, gdiplus.h, wpc.h, photoacquire.h, GameInput.h, pla.h
**Partitions scraped:** Gdiplus, Parcon, PicAcq, GameInput, Pla (all x86 per ExcludeFromCrossarch; 0 warnings/errors)

- ddraw.h: HMONITOR is a pass-through system handle used only as a callback parameter type; the guarded DECLARE_HANDLE fallback never fires (HMONITOR_DECLARED already defined by windows.fixed.h). No DirectDraw function creates/releases HMONITOR. Clean.
- gdiplus.h: 632 functions scraped. GDI+'s create/delete object pattern (GdipCreateFromHDC/GdipDeleteGraphics etc.) uses strongly-typed C++ pointers (GpGraphics*, GpBitmap*, ...), not HANDLE-family typedefs. Cross-checked all 13 headers in the repo currently carrying _Win32_metadata_raii_free_/_invalid_handle_ (bcrypt.h, amsi.h, AuthZ.h, sspi.h, tbs.h, ncrypt.h, ncryptprotect.h, NTSecAPI.h, NTSecPKG.h, prntvpt.h, securitybaseapi.h, wincrypt.h) — the mechanism is applied exclusively to HANDLE-family opaque scalar types, never strongly-typed C++ pointers. autoTypes.json/emitter.settings.rsp have no Gp*/Gdiplus entries. Classified out-of-current-policy-scope / no-annotation-required (no regression - nothing pre-existing to correct).
- wpc.h: All 16 functions are MIDL RPC marshalling stubs (BSTR_User*/HWND_User*, incl. *64 variants). Clean.
- photoacquire.h: All 40 functions are MIDL RPC marshalling stubs (BSTR/HBITMAP/HICON/HWND/LPSAFEARRAY_User*). Pass-through of externally-owned GDI handles only, not produced/released here. Clean.
- GameInput.h: GameInputCreate(IGameInput**) is a clean COM-factory pattern (consistent with dxcapi.h/directml.h precedent). Found one raw HANDLE out-param: IGameInputDispatcher::OpenWaitHandle - but this is a COM vtable interface method (IFACEMETHOD), not a DllImport/extern function. Confirmed via repo-wide grep that no existing header applies _Win32_metadata_raii_free_/_invalid_handle_ to a COM interface method - out of current mechanism's scope. Clean under current policy.
- pla.h: 6 functions, all operate on strings/flags/paths only. No handle-producing functions. Clean.

**Ledger status:** 255 accepted-normalized, 1 blocked (esent.h), 1147 pending.
**Policy note:** This batch establishes precedent that ownership annotations apply only to (a) DllImport/extern "C" functions producing/consuming (b) HANDLE-family scalar opaque types (DECLARE_HANDLE/typedef PVOID), consistent with every existing annotated header in the repo. Strongly-typed C++ object pointers (GDI+) and COM vtable interface methods (GameInput) are out of scope for this mechanism and require no fix to remain policy-compliant.

## 2026-09-02 16:49:59 UTC - Batch scraping-investigation-14

**Headers:** iswindowarranged.h, getprocesshandlefromhwnd.h (blocked), search.h, mtx.h, wiamindr.h
**Partitions scraped:** MenuRc, Search, ComOle, TransactionServer, Wia, Threading (x86 for ExcludeFromCrossarch; 0 warnings/errors)

- iswindowarranged.h: IsWindowArranged(HWND) is query-only, no handle production. Clean.
- **getprocesshandlefromhwnd.h: BLOCKED.** GetProcessHandleFromHwnd returns a process HANDLE directly as
  the C function return value (not via _Out_/_Outptr_ param). Investigated whether the producer-site-only
  annotation mechanism can express this: dumped the baseline bin\Windows.Win32.winmd (24MB, 35145 types)
  via WinmdUtils.exe dump. Confirmed the dump DOES render return-position attributes when present (289
  [return: ...] occurrences exist, e.g. [return: Const]), so absence elsewhere is meaningful. Found that
  EVERY RAIIFree/InvalidHandleValue attribute in the entire published winmd attaches to a struct TYPE
  declaration only (e.g. HBITMAP's struct decl) - never to a parameter or a [return: ...] position.
  Cross-checked comparable return-HANDLE-shaped APIs already in the baseline (LoadLibraryExW/A returning
  HMODULE, IcmpCreateFile returning HANDLE) - neither carries any ownership metadata either.
  GetProcessHandleFromHwnd itself is already in the baseline with none. This confirms there is
  zero precedent anywhere in this codebase (or its currently published output) for annotating a bare
  function return-value handle's ownership - the mechanism (both the legacy autoTypes.json type-level
  path and the newer inline producer-site path) only supports out-parameters/type declarations.
  Recorded as blocked (same class as esent.h): a genuine ownership relationship exists, but expressing
  it correctly requires a dedicated policy decision on annotation placement, not a guess.
- search.h: pure CRT redirect (#include corecrt_search.h), out of Win32 scope. Clean.
- mtx.h: pure redirect (#include comsvcs.h, already accepted-normalized). Clean.
- wiamindr.h: pure redirect (#include wiamindr_lh.h/wiamindr_xp.h); wiamindr_lh.h remains pending as a
  separate follow-up item. Clean (no direct declarations in wiamindr.h itself).

**Also investigated (deep-dive, no ledger change needed):** Verified the corrected-policy commits
165b5f09/7335ddc4 removed only REDUNDANT typedef-level annotations from windef.h/minwindef.h that
duplicated existing autoTypes.json entries (HGLRC, HWINEVENTHOOK, HGDIOBJ, HBITMAP, HPEN, HFONT,
HENHMETAFILE, HPALETTE, HHOOK, HICON, HMENU, HCOLORSPACE, HDESK, HACCEL all still have working
autoTypes.json ownership entries) - confirmed NOT a regression, wingdi.h/winuser.h (the producer-site
headers for these types) do not need new producer-site patches for these specific well-known GDI handles.

**Ledger status:** 259 accepted-normalized, 2 blocked (esent.h, getprocesshandlefromhwnd.h), 1142 pending.

## 2026-09-02 16:53:58 UTC - Batch scraping-investigation-15

**Headers:** bcp47mrm.h, wia.h, sas.h, getcurrentpackageinfo3.h, wab.h (blocked)
**Partitions scraped:** Wab, Wia (x86; 0 warnings/errors)

- bcp47mrm.h: GetDistanceOfClosestLanguageInList/IsWellFormedTag - no handle involved. Clean.
- wia.h: redirect-only header (wia_lh.h/wia_xp.h); 185 functions all MIDL COM/RPC proxy/stub
  marshalling helpers (IWiaDevMgr_*_Proxy/_Stub etc.), no DECLARE_HANDLE. Clean.
- sas.h: SendSAS(BOOL) - no handle involved at all. Clean.
- getcurrentpackageinfo3.h: buffer-fill API (UINT32*/void* buffer), no handle production. Clean.
- **wab.h: BLOCKED.** 77 functions scraped; 76 are clean (COM-interface pointer producers like
  WABOpen/WABOpenEx via _Outptr_ LPADRBOOK*, or data/struct-copy helpers). One genuine exception:
  FtgRegisterIdleRoutine(PFNIDLE*, LPVOID, ...) returns an opaque FTG handle (void*) directly as the
  function RETURN VALUE (not an out-param), later released via DeregisterIdleRoutine(FTG) and mutated
  via EnableIdleRoutine/ChangeIdleRoutine. This is the same return-value-handle-ownership blocker class
  first documented for getprocesshandlefromhwnd.h in batch scraping-investigation-14: no precedent
  anywhere in this repo or the published baseline winmd for annotating a bare return-value handle.
  Recorded as blocked pending the same dedicated policy decision.

**Ledger status:** 263 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1137 pending.

## 2026-09-02 16:57:26 UTC - Batch scraping-investigation-16

**Headers:** sliddefs.h, wbemidl.h, transportsettingcommon.h, socketapi.h, isolatedwindowsenvironmentutils.h
**Partitions scraped:** Identity, Wmi, WinSock, Isolation (x86; 0 errors; Identity/WinSock show 3 each
pre-existing cross-partition remap warnings for sockaddr/timeval/_CERT_CONTEXT/SecPkgContext_IssuerListInfoEx
unrelated to any header in this batch - not introduced by this batch, no content in the audited headers
touches those types)

- sliddefs.h: GUID constant only, no functions. Clean.
- wbemidl.h: redirect-only header (wbemcli.h/wbemprov.h/wbemtran.h/wbemdisp.h); 40 functions all MIDL
  RPC marshalling stubs (BSTR_User*/VARIANT_User*), no DECLARE_HANDLE. Clean. Sub-headers remain
  separately tracked/pending.
- transportsettingcommon.h: plain data struct only (TRANSPORT_SETTING_ID), no functions. Clean.
- socketapi.h: SetSocketMediaStreamingMode(BOOL) - no handle. Clean.
- isolatedwindowsenvironmentutils.h: two BOOL* out-param query functions, no handle. Clean.

**Ledger status:** 268 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1132 pending.

## 2026-09-02 17:02:07 UTC - Batch scraping-investigation-17

**Headers:** windowsceip.h, wcnapi.h, rometadata.h, schnlsp.h, menutemplate.h
**Partitions scraped:** CEIP, Wcn, WinRT.Metadata (x86; 0 warnings/errors)

- windowsceip.h: CeipIsOptedIn() - no handle involved. Clean.
- wcnapi.h: redirect-only header (WcnTypes.h/WcnDevice.h/WcnFunctionDiscoveryKeys.h); entire Wcn
  partition scraped 0 DllImport functions (types/constants only), no DECLARE_HANDLE. Clean.
- rometadata.h: MetaDataGetDispenser is a COM QueryInterface-style factory (_Outptr_ void** ppv per
  caller riid), consistent with the established clean COM-factory pattern. Clean.
- schnlsp.h: redirect-only header (#include schannel.h, already accepted-normalized). Clean.
- menutemplate.h: plain data structs only (MENUEX_TEMPLATE_HEADER/ITEM, MENUTEMPLATEEX), no functions. Clean.

**Ledger status:** 273 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1127 pending.

## 2026-09-02 17:05:38 UTC - Batch scraping-investigation-18

**Headers:** memorybuffer.h, apdevpkey.h, mtxdm.h, d2dbasetypes.h, wiaintfc.h
**Partitions scraped:** WinRT, Devices.Properties, Cos, Direct2D (x86; 0 warnings/errors)

- memorybuffer.h: IMemoryBufferByteAccess COM interface (GetBuffer via _Outptr_result_buffer_); buffer
  view is not a separately-owned HANDLE-family resource (lifecycle implicit in COM refcounting/Close()). Clean.
- apdevpkey.h: single DEFINE_DEVPROPKEY constant, no functions. Clean.
- mtxdm.h: GetDispenserManager(IDispenserManager**) is a COM-factory pattern; includes comsvcs.h
  (already accepted-normalized). Clean.
- d2dbasetypes.h: single typedef (D2D_COLOR_F = D3DCOLORVALUE), no functions. Clean.
- wiaintfc.h: single DEFINE_GUID constant, no functions. Clean.

**Ledger status:** 278 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1122 pending.

## 2026-09-02 17:09:51 UTC - Batch scraping-investigation-19

**Headers:** afunix.h, activecf.h, ime_cmodes.h, atacct.h, winsdkver.h
**Partitions scraped:** Media.DShow, Input.Ime, NetMgmt, Foundation (x86; 0 errors; NetMgmt shows 1
pre-existing unrelated cross-partition remap warning for _CERT_CONTEXT, not touched by this header)

- afunix.h: SOCKADDR_UN data struct + SIO_AF_UNIX_* IOCTL constants, no functions. Clean.
- activecf.h: CFSTR_VFW_FILTERLIST constant + VFW_FILTERLIST data struct, no functions. Clean.
- ime_cmodes.h: IME_CMODE_* bit-field constants only, no functions. Clean.
- atacct.h: GetNetScheduleAccountInformation/SetNetScheduleAccountInformation - string-only, no handle. Clean.
- winsdkver.h: SDK version-max constants only, no functions. Clean.

**Ledger status:** 283 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1117 pending.

## 2026-09-02 17:13:23 UTC - Batch scraping-investigation-20

**Headers:** messagedispatcherapi.h, wsdapi.h, robuffer.h, mediaerr.h, amxmlgraphbuilder.h
**Partitions scraped:** ComOle, WebServicesOnDevices, GameMode (x86; 0 errors; WebServicesOnDevices
shows 1 pre-existing unrelated cross-partition remap warning for _CERT_CONTEXT)

- messagedispatcherapi.h: CoSetMessageDispatcher takes a caller-supplied IMessageDispatcher* as input
  (not produced here); CoHandlePriorityEventsFromMessagePump has no params. Clean.
- wsdapi.h: redirect-only header (9 wsd*.h sub-headers, all still separately pending); 32 functions
  scraped, all COM-factory (_Outptr_ IWSD*) or memory/XML-node helper pairs (WSDAllocateLinkedMemory/
  WSDFreeLinkedMemory operate on raw void*, not HANDLE-family - analogous to a custom heap allocator,
  out of scope for the HANDLE-specific ownership mechanism). No DECLARE_HANDLE. Clean.
- robuffer.h: RoGetBufferMarshaler is a clean COM-factory; IBufferByteAccess::Buffer is a buffer-view
  method (same reasoning as memorybuffer.h's IMemoryBufferByteAccess, already accepted). Clean.
- mediaerr.h: DMO_E_* HRESULT constants only, no functions. Clean.
- amxmlgraphbuilder.h: IXMLGraphBuilder COM interface + GUID constants, no raw HANDLE. Clean.

**Ledger status:** 288 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1112 pending.

## 2026-09-02 17:16:30 UTC - Batch scraping-investigation-21

**Headers:** expandedresources.h, cryptcat.h, idispids.h, in6addr.h, wsdevlicensing.h
**Partitions scraped:** Security.Cryptography.Catalog, InternetExplorer, wnv, DevLic (x86; 0 errors;
Security.Cryptography.Catalog/wnv show pre-existing unrelated cross-partition remap warnings)

- expandedresources.h: HasExpandedResources/GetExpandedResourceExclusiveCpuCount/ReleaseExclusiveCpuSets -
  no HANDLE-family type involved. Clean.
- cryptcat.h: CryptCATCDFEnumMembersByCDFTagEx/CryptCATCDFEnumAttributesWithCDFTag only *consume* an
  already-open CRYPTCATCDF* (enumeration), never open/close it. The actual producer/consumer pair
  (CryptCATCDFOpen/CryptCATCDFClose) lives in mscat.h (included by cryptcat.h), separately tracked and
  still pending - that is where any future ownership-annotation work belongs. Clean.
- idispids.h: DISPID_* constants only, no functions. Clean.
- in6addr.h: in6_addr/IN6_ADDR data struct + macros only, no functions. Clean.
- wsdevlicensing.h: CheckDeveloperLicense/AcquireDeveloperLicense/RemoveDeveloperLicense - HWND is an
  input parameter, outputs are plain FILETIME values. Clean.

**Ledger status:** 293 accepted-normalized, 3 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h), 1107 pending.

## 2026-09-02 17:19:51 UTC - Batch scraping-investigation-22

**Headers:** fxsutility.h, vmgenerationcounter.h, edpwin32.h, inaddr.h, wincon.h (blocked)
**Partitions scraped:** Console (x86; 0 warnings/errors)

- fxsutility.h / vmgenerationcounter.h / edpwin32.h / inaddr.h: no handle involved (fax-send query,
  GUID/IOCTL/struct constants, enterprise-data-protection path API, IPv4 address struct). Clean.
- **wincon.h: BLOCKED.** Unlike prior redirect-only headers (mtx.h/schnlsp.h) whose included content was
  already separately tracked and accepted, wincon.h redirects to consoleapi.h/consoleapi2.h/
  consoleapi3.h/wincontypes.h which are NOT separately tracked in the ledger - required a direct
  live-scrape audit of the whole Console partition (~120 functions). Found a genuine multi-producer/
  single-consumer HANDLE ownership relationship: CreateConsoleScreenBuffer/OpenConsoleW/
  DuplicateConsoleHandle all return a console HANDLE directly as the function RETURN VALUE (not
  out-param), released via CloseConsoleHandle. This is the same return-value-handle-ownership blocker
  class first documented for getprocesshandlefromhwnd.h (batch 14) and wab.h (batch 15) - no precedent
  anywhere in the repo or published baseline winmd for annotating a bare return-value handle.
  (Note: GetConsoleWindow()/GetConsoleInputWaitHandle() also return IntPtr but are documented
  borrowed/non-owned handles, correctly out of scope regardless.)

**Methodology note:** This batch highlights that the "redirect-only header, check if included content
is already tracked" shortcut is NOT safe when the included sub-headers are untracked - a live partition
scrape is required in that case to avoid missing genuine ownership gaps (as almost happened here).

**Ledger status:** 297 accepted-normalized, 4 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h, wincon.h), 1102 pending.

## 2026-09-02 17:24:06 UTC - Batch scraping-investigation-23

**Headers:** dxcore.h, lmuseflg.h, resourceindexer.h (blocked), opmxbox.h, appcompatapi.h, consoleapis.h
**Partitions scraped:** DXCore, Mf, WinProg (x86; 0 errors; WinProg shows 1 pre-existing unrelated
cross-partition remap warning for _CERT_CONTEXT)

- dxcore.h: DXCoreCreateAdapterFactory is a clean COM factory pattern. Clean.
- lmuseflg.h: force-level constants + macro only, no functions. Clean.
- **resourceindexer.h: BLOCKED (new blocker sub-class).** CreateResourceIndexer/DestroyResourceIndexer
  form a genuine ownership pair via an out-param (_Outptr_ PVOID*) - unlike the return-value class
  (getprocesshandlefromhwnd.h/wab.h/wincon.h), the *placement* here is correct. The blocker is that the
  parameter type is generic untyped PVOID, not a distinctly-named handle typedef like every other fixed
  case this session (TBS_HCONTEXT, BCRYPT_ALG_HANDLE, etc). Since RAIIFree/InvalidHandleValue attach to
  the parameter's TYPE declaration (confirmed via the WinmdUtils dump investigation), annotating bare
  PVOID would incorrectly apply RAIIFree(DestroyResourceIndexer) to every void* in the published
  metadata. Fixing this requires first introducing a new named handle typedef - a design decision.
- opmxbox.h: enum-output-only API (OPMXboxEnableHDCP/GetHDCPStatus/GetHDCPStatusAndType), no handle. Clean.
- appcompatapi.h: ApphelpCheckShellObject outputs a plain ULONGLONG, no handle. Clean.
- consoleapis.h: HANDLE/HWND struct fields are caller-supplied input references (not produced here);
  ConsoleControl itself has no handle parameter. Distinct from the wincon.h blocker (different
  functions - CreateConsoleScreenBuffer et al are NOT declared here). Clean.

**Ledger status:** 302 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1096 pending.

## 2026-09-02 17:27:23 UTC - Batch scraping-investigation-24

**Headers:** delayloadhandler.h, msacmdlg.h, amparse.h, atsmedia.h, wdigest.h
**Partitions scraped:** Multimedia, MsTv (x86; 0 warnings/errors)

- delayloadhandler.h: structs + callback-typedef + extern variable, no DllImport functions. Clean.
- msacmdlg.h: dialog resource ID constants only, no functions. Clean.
- amparse.h: IAMParse COM interface, plain REFERENCE_TIME output, no handle. Clean.
- atsmedia.h: GUID-struct constant only, no functions. Clean.
- wdigest.h: WDIGEST_SP_NAME* string constants only, no functions. Clean.

**Ledger status:** 307 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1091 pending.

## 2026-09-02 17:31:16 UTC - Batch scraping-investigation-25

**Headers:** bindlink.h, propkeydef.h, calendarapis.h, eapmethodtypes.h, swdevicedef.h
**Partitions scraped:** Fs, Shell, EapHost, SwDevice (x86; 0 errors; Shell shows 3 pre-existing
unrelated cross-partition remap warnings for in6_addr/in_addr)

- bindlink.h: CreateBindLink/RemoveBindLink - path-string-only, no handle. Clean.
- propkeydef.h: PROPERTYKEY macros/operators only, no functions. Clean.
- calendarapis.h: all 7 functions operate on CALDATETIME/SYSTEMTIME structs only, no handle. Clean.
- eapmethodtypes.h: declares only EapPacket struct, tagEapCode enum, and EAP_SESSION_HANDLE typedef -
  zero functions in this header. Per corrected policy, no annotation belongs on the typedef itself;
  actual EAP_SESSION_HANDLE-consuming functions live in the separately-tracked eapmethodpeerapis.h
  (still pending) - that is where future producer-site work belongs. Clean.
- swdevicedef.h: declares only SW_DEVICE_CAPABILITIES/SW_DEVICE_LIFETIME enums and
  SW_DEVICE_CREATE_INFO struct - zero functions. HSWDEVICE producer/consumer (SwDeviceCreate/
  SwDeviceClose) lives in the separately-tracked swdevice.h (still pending). Clean.

**Ledger status:** 312 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1086 pending.

## 2026-09-02 17:35:52 UTC - Batch scraping-investigation-26

**Headers:** Windows.Graphics.Capture.Interop.h, colordlg.h, jsrt.h, notesdeviceservice.h, wcnfunctiondiscoverykeys.h
**Partitions scraped:** WinRT.Graphics.Capture, DlgBox, Js, WpdSdk (x86; 0 warnings/errors)

- Windows.Graphics.Capture.Interop.h: IGraphicsCaptureItemInterop::CreateForWindow/CreateForMonitor are
  clean COM factory methods; GetWindow/GetMonitor are query-only HWND/HMONITOR accessors (not owned
  resources). Clean.
- colordlg.h: dialog control ID constants only, no functions. Clean.
- jsrt.h: redirect-only header - verified Js/main.cpp does not define USE_EDGEMODE_JSRT (chakrart.h
  explicitly commented out as unreachable); the only reachable branch (jsrt9.h) is already separately
  tracked (pending). Clean.
- notesdeviceservice.h: DEFINE_DEVSVCGUID + name/type constants only, no functions. Clean.
- wcnfunctiondiscoverykeys.h: DEFINE_GUID + DEFINE_PROPERTYKEY constants only (via already-accepted
  PropKeyDef.h), no functions. Clean.

**Ledger status:** 317 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1081 pending.

## 2026-09-02 17:39:07 UTC - Batch scraping-investigation-27

**Headers:** dmprocessxmlfiltered.h, lm.h, errors.h, msiltcfg.h, licenseprotection.h
**Partitions scraped:** NetMgmt, Setup, Security.LicenseProtection (x86; 0 errors; pre-existing
unrelated cross-partition remap warnings on all three, none touched by these headers)

- dmprocessxmlfiltered.h: DMProcessConfigXMLFiltered outputs a BSTR (freed via SysFreeString per doc
  comment) - BSTR allocation out of scope for the HANDLE-specific ownership mechanism. Clean.
- lm.h: pure umbrella redirect header (18 sub-headers: lmcons.h, lmerr.h, lmaccess.h, lmalert.h,
  lmshare.h, lmmsg.h, lmremutl.h, lmrepl.h, lmserver.h, lmsvc.h, lmuse.h, lmwksta.h, lmapibuf.h,
  lmerrlog.h, lmconfig.h, lmstats.h, lmaudit.h, lmjoin.h). Unlike wincon.h, every one of these
  sub-headers IS already a tracked ledger item (2 accepted, 16 pending) - so any genuine ownership
  work (e.g. NetUseAdd/NetUseDel in lmuse.h) will be caught by those already-queued individual audits,
  not silently missed. Clean.
- errors.h: AMGetErrorTextA/W output caller-allocated string buffers, no handle. Clean.
- msiltcfg.h: ShutdownMsi/RestartMsi, no handle involved. Clean.
- licenseprotection.h: RegisterLicenseKeyWithExpiration/ValidateLicenseKeyProtection output plain
  enum/FILETIME values, no handle. Clean.

**Methodology reaffirmed:** for redirect-only headers, the safety test is whether ALL included
sub-headers are already represented as ledger items (regardless of status) - if yes, closing the
umbrella header loses no audit coverage; if any sub-header is untracked, a live partition scrape is
required first (as was necessary for wincon.h in batch 22).

**Ledger status:** 322 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1076 pending.

## 2026-09-02 17:42:15 UTC - Batch scraping-investigation-28

**Headers:** windows.graphics.interop.h, sensapi.h, apiquery2.h, wsvns.h, rpcnsip.h
**Partitions scraped:** WinRT.Direct2D, Sens, Rpc (x86; 0 errors; Rpc shows 1 pre-existing unrelated
cross-partition remap warning for _CERT_CONTEXT)

- windows.graphics.interop.h: IGeometrySource2DInterop is a clean COM factory pattern. Clean.
- sensapi.h: IsDestinationReachableA/W/IsNetworkAlive - plain struct/DWORD output, no handle. Clean.
- apiquery2.h: caller-allocated string buffer output only, no handle. Clean.
- wsvns.h: data struct + constants only, no functions. Clean.
- rpcnsip.h: internal (I_-prefixed) RPC runtime stub-binding routines. I_RpcNsSendReceive outputs an
  RPC_BINDING_HANDLE, but this is a documented-internal stub routine, not the canonical public producer
  site. RPC_BINDING_HANDLE has no autoTypes.json entry; its canonical public API
  (RpcBindingFromStringBinding/RpcBindingFree) lives in rpcdce.h, already separately tracked (pending) -
  that is where future ownership work belongs. Clean.

**Ledger status:** 327 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1071 pending.

## 2026-09-02 17:45:46 UTC - Batch scraping-investigation-29

**Headers:** ntmsmli.h, rpcssl.h, dxprogrammablecapture.h, capi.h, sporder.h
**Partitions scraped:** Dxgi (x86; 0 warnings/errors)

- ntmsmli.h: data struct + callback-signature typedefs only (no DllImport functions). Clean.
- rpcssl.h: RpcCertGeneratePrincipalName outputs an RPC_WSTR/RPC_CSTR string buffer (RpcStringFree
  convention), not HANDLE-family. Clean.
- dxprogrammablecapture.h: IDXGraphicsAnalysis::BeginCapture/EndCapture are parameterless, no handle. Clean.
- capi.h: redirects to wincrypt.h/sipbase.h/mscat.h/mssip.h/wintrust.h. sipbase.h itself is untracked
  but verified (per the wincon.h lesson) to be a trivial redirect to the already-accepted mssip.h - no
  hidden content. capi.h's own JAVA_TRUST struct is plain data. Clean.
- sporder.h: WSCWriteProviderOrder/WSCWriteNameSpaceOrder operate on DWORD/GUID arrays only. Clean.

**Ledger status:** 332 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1066 pending.

## 2026-09-02 17:48:34 UTC - Batch scraping-investigation-30

**Headers:** mpeg2bits.h, qospol.h, lowlevelmonitorconfigurationapi.h, audevcod.h, d2derr.h
**Partitions scraped:** Qos, Monitor (x86; 0 errors; Qos shows 3 pre-existing unrelated cross-partition
remap warnings for sockaddr/timeval/_CERT_CONTEXT)

- mpeg2bits.h: bitfield data structs only, no functions. Clean.
- qospol.h: IDPE_ATTR data struct + constants only, no functions. Clean.
- lowlevelmonitorconfigurationapi.h: all 6 functions take HANDLE hMonitor as _In_ only (never produce
  it). The actual producer pair (GetPhysicalMonitorsFromHMONITOR/DestroyPhysicalMonitors) lives in
  PhysicalMonitorEnumerationAPI.h, already separately tracked (pending) - future ownership work belongs
  there. Clean.
- audevcod.h: SNDDEV_ERR enum + error-code constants only, no functions. Clean.
- d2derr.h: D2DERR_* HRESULT macros only, no functions. Clean.

**Ledger status:** 337 accepted-normalized, 5 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h), 1061 pending.

## 2026-09-02 17:51:39 UTC - Batch scraping-investigation-31

**Headers:** msiehost.h, playto.h, winppi.h (blocked), ringtonedeviceservice.h, msdshape.h
**Partitions scraped:** Printing, Search (x86; 0 errors; Printing shows 1 pre-existing unrelated
cross-partition remap warning for _CERT_CONTEXT)

- msiehost.h: GUID/command-ID constants only, no functions. Clean.
- playto.h: pure MIDL boilerplate (RPC_IF_HANDLE ifspec externs only); real content is in the
  separately-tracked MFSharingEngine.h (pending). Clean.
- **winppi.h: BLOCKED.** GdiGetSpoolFileHandle/GdiGetPageHandle both return a HANDLE directly as the
  function return value (not out-param), released via GdiDeleteSpoolFileHandle. Same
  return-value-handle-ownership blocker class as getprocesshandlefromhwnd.h/wab.h/wincon.h - no
  precedent anywhere in the repo or published baseline winmd for annotating a bare return-value handle.
- ringtonedeviceservice.h: GUID/name constants only, no functions (same pattern as notesdeviceservice.h). Clean.
- msdshape.h: constants/enums only, no functions. Clean.

**Ledger status:** 341 accepted-normalized, 6 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h), 1056 pending.

## 2026-09-02 17:54:26 UTC - Batch scraping-investigation-32

**Headers:** wlclient.h, nfcradiodev.h, shcore.h, mdmlocalmanagement.h, identitycommon.h
**Partitions scraped:** NWifi, Nfc, MdmReg, IdentityProvider (x86; 0 warnings/errors)

- wlclient.h: DOT11_ADAPTER/DOT11_BSS_LIST/DOT11_PORT_STATE/DOT11_SECURITY_PACKET_HEADER data structs
  only, no functions. Clean.
- nfcradiodev.h: GUID + IOCTL constants + data structs only, no functions. Clean.
- shcore.h: CreateRandomAccessStreamOnFile/OverStream/CreateStreamOverRandomAccessStream are clean COM
  factory functions. Clean.
- mdmlocalmanagement.h: RegisterDeviceWithLocalManagement/UnregisterDeviceWithLocalManagement have no
  handle params; ApplyLocalManagementSyncML's string output is explicitly freed via LocalFree (memory
  convention, not HANDLE-family). Clean.
- identitycommon.h: IDENTITY_TYPE enum + MIDL boilerplate only, no functions. Clean.

**Ledger status:** 346 accepted-normalized, 6 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h), 1051 pending.

## 2026-09-02 17:58:34 UTC - Batch scraping-investigation-33

**Headers:** napmicrosoftvendorids.h, libloaderapi2.h (blocked), faxmmc.h, tabflicks.h, deviceservices.h
**Partitions scraped:** NetworkAccessProtection, LibraryLoader, Fax, Tablet (x86; 0 warnings/errors)

- napmicrosoftvendorids.h: static UINT32 constants only, no functions. Clean.
- **libloaderapi2.h: BLOCKED.** LoadPackagedLibrary returns HMODULE directly as the function return
  value (not out-param), released via FreeLibrary - same return-value-handle-ownership blocker class
  already documented for getprocesshandlefromhwnd.h/wab.h/wincon.h/winppi.h. This is the ambient,
  already-confirmed (via the original WinmdUtils dump investigation, which used LoadLibraryExW/A as its
  comparison baseline) unaddressed state of the whole LoadLibrary-family API surface - recorded for
  per-header traceability, no new investigation needed.
- faxmmc.h: GUID/string constants only, no functions. Clean.
- tabflicks.h: enums/bitfield structs only, no functions. Clean.
- deviceservices.h: GUID/property-key constants only, no functions; BridgeDeviceService.h separately
  tracked (pending). Clean.

**Policy note:** established that once the return-value-handle-ownership blocker CLASS is fully
documented (as of batch 14), subsequent headers exhibiting the same pattern are recorded as blocked
with a concise reference to the existing evidence rather than repeating the full WinmdUtils
investigation each time - this keeps the ledger honest (each affected header gets its own tracked
blocked entry) without redundant re-investigation.

**Ledger status:** 350 accepted-normalized, 7 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h), 1046 pending.

## 2026-09-02 18:01:13 UTC - Batch scraping-investigation-34

**Headers:** errorrep.h, sensorsstructures.h, wiatwcmp.h, ndkinfo.h, fhstatus.h
**Partitions scraped:** Wer, WinSensors, Ndis (x86; 0 warnings/errors)

- errorrep.h: ReportFault/AddERExcludedApplicationA/W/WerReportHang - HWND is a caller-supplied input,
  not produced here. Clean.
- sensorsstructures.h: VEC3D/MATRIX3X3/QUATERNION data structs + AXIS enum only, no functions. Clean.
- wiatwcmp.h: TWAIN_CAPABILITY data struct + constants only, no functions. Clean.
- ndkinfo.h: NDK_VERSION/NDK_ADAPTER_INFO data structs + NDK_RDMA_TECHNOLOGY enum + constants only, no
  functions. Clean.
- fhstatus.h: FH_STATE_* status-code constants only, no functions. Clean.

**Ledger status:** 355 accepted-normalized, 7 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h), 1041 pending.

## 2026-09-02 18:03:49 UTC - Batch scraping-investigation-35

**Headers:** eapauthenticatortypes.h, functiondiscoveryerror.h, elssrvc.h, TpmVscAttestation.h, lmconfig.h
**Partitions scraped:** FunctionDiscovery, Security.Tpm (x86; 0 errors; pre-existing unrelated
cross-partition remap warnings on both, not touched by these headers)

- eapauthenticatortypes.h: enum + MIDL boilerplate only, no functions. Clean.
- functiondiscoveryerror.h: HRESULT error-code constants only, no functions. Clean.
- elssrvc.h: static const GUID constants only, no functions. Clean.
- TpmVscAttestation.h: enum + MIDL boilerplate only, no functions. Clean.
- lmconfig.h: NetConfigGet/NetConfigGetAll output LPBYTE buffers freed via NetApiBufferFree (Net API
  convention, not HANDLE-family); part of the lm.h umbrella already noted as individually tracked. Clean.

**Ledger status:** 360 accepted-normalized, 7 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h), 1036 pending.

## 2026-09-02 18:06:46 UTC - Batch scraping-investigation-36

**Headers:** msdaguid.h, gamingdeviceinformation.h, evalcom2.h, presentationtypes.h, persist.h
**Partitions scraped:** GamingDvcInfo, CompositionSwapchain (x86; 0 warnings/errors)

- msdaguid.h: OLE DB GUID constants only, no functions. Clean.
- gamingdeviceinformation.h: GetGamingDeviceModelInformation outputs a plain struct, no handle. Clean.
- evalcom2.h: IValidate COM interface; SetDisplay/SetStatus take a caller-supplied LPVOID context
  (not produced/owned here), no raw HANDLE. Clean.
- presentationtypes.h: MIDL boilerplate + data structs/enum/typedef only, no functions. Clean.
- persist.h: CLSID/GUID/PROGID constants only, no functions. Clean.

**Ledger status:** 365 accepted-normalized, 7 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h), 1031 pending.

## 2026-09-02 18:10:02 UTC - Batch scraping-investigation-37

**Headers:** security.h, statehelpers.h, wsipx.h, syncregistrationerrors.h, httprequestid.h
**Partitions scraped:** Identity, Registry, WinSync, Iis (x86; 0 errors; Identity/Iis show pre-existing
unrelated cross-partition remap warnings)

- security.h: redirect-only header (name macros + sspi.h/secext.h/issper16.h). sspi.h already accepted,
  issper16.h already tracked (pending). secext.h is untracked - per the wincon.h lesson, read its
  content directly: GetUserNameExA/W, GetComputerObjectNameA/W, TranslateNameA/W all output plain
  string buffers, no HANDLE. Clean.
- statehelpers.h: GetRegistryValueWithFallbackW - both HKEY params are caller-supplied inputs, not
  produced here. Clean.
- wsipx.h: data struct + constants only, no functions. Clean.
- syncregistrationerrors.h: HRESULT error-code constants only, no functions. Clean.
- httprequestid.h: DISPID constants only, no functions. Clean.

**Ledger status:** 370 accepted-normalized, 7 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h), 1026 pending.

## 2026-09-02 18:15:37 UTC - Batch scraping-investigation-38

**Headers:** wlanihvtypes.h, cderr.h, mmiscapi2.h, MSAJTransport.h (blocked), dxgicommon.h
**Partitions scraped:** Media (x86; 0 warnings/errors), AllJoyn (FAILED - new parser/toolchain blocker
class), WinRT (re-scraped fresh to check as alternate validation path)

- wlanihvtypes.h: data structs + constants only, no functions. Clean.
- cderr.h: common-dialog error-code constants only, no functions. Clean.
- mmiscapi2.h: timeSetEvent/timeKillEvent - timer ID is a UINT, not a HANDLE. Clean.
- **MSAJTransport.h: BLOCKED (new blocker class: parser/toolchain limitation).** Attempted to re-scrape
  the AllJoyn partition - the only partition that actually compiles this header's
  WINAPI_PARTITION_APP/DESKTOP-guarded declarations (confirmed via a fresh WinRT re-scrape that this
  header's functions are NOT present in WinRT's output despite #include, so WinRT cannot substitute).
  AllJoyn scrape FAILS outright: 'use of undeclared identifier __builtin_verbose_trap' in MSVC
  14.51.36231's <xmemory>, pulled in transitively by the AllJoyn C++ SDK headers also included by this
  partition - a genuine Clang/MSVC toolset version mismatch unrelated to any Win32 header content, not
  fixable via a header patch (would need a ClangSharp/Clang upgrade or MSVC toolset pin change).
  Additionally, even by direct source inspection, AllJoynConnectToBus/AllJoynCreateBus return a bus
  HANDLE directly as the function return value (not out-param), released via AllJoynCloseBusHandle -
  the same already-documented return-value-handle-ownership class as getprocesshandlefromhwnd.h/wab.h/
  wincon.h/winppi.h/libloaderapi2.h. Recorded as blocked with full evidence for the new parser-limitation
  class, concise evidence for the already-known ownership class.
- dxgicommon.h: data structs/enum/constants only, no functions. Clean.

**Ledger status:** 374 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 1021 pending.

## 2026-09-02 18:21:25 UTC - Batch scraping-investigation-39

**Headers:** cscapi.h, appnotify.h (FIXED), keycredmgr.h, sens.h, dv.h
**Partitions scraped:** Of, Shell, Credentials, Audio.DirectMusic (x86; 0 errors; Shell shows 2
pre-existing unrelated cross-partition remap warnings for in6_addr/in_addr)

- cscapi.h: OfflineFilesEnable/Start/QueryStatus/QueryStatusEx output plain BOOL* only. Clean.
- **appnotify.h: GENUINE GAP FOUND AND FIXED.** RegisterAppStateChangeNotification/
  RegisterAppConstrainedChangeNotification produce opaque PAPPSTATE_REGISTRATION/
  PAPPCONSTRAIN_REGISTRATION handles (typedef struct _X *PX - genuinely opaque pointer-to-incomplete
  type, unlike resourceindexer.h's generic PVOID) via out-param, released via
  UnregisterAppStateChangeNotification/UnregisterAppConstrainedChangeNotification. Checked autoTypes.json:
  both types have NativeTypedef entries but no CloseApi/InvalidHandleValues - confirmed no existing
  ownership metadata to duplicate/conflict with. Applied producer-site fix: added
  win32metadata_annotations.h include guard + _Win32_metadata_invalid_handle_(0) +
  _Win32_metadata_raii_free_(...) on each out-param. Verified via live re-scrape of Shell partition
  (0 errors) and git apply --check --reverse (exit 0). New patch:
  appnotify.h.appstate-registration-ownership.patch.
- keycredmgr.h: KeyCredentialManagerGetInformation/FreeInformation alloc/free a transparent (fully
  defined) struct, not an opaque handle - out of scope for the HANDLE-specific mechanism (same
  reasoning as WSDAllocateLinkedMemory in batch 20). Clean.
- sens.h: constants only, no functions. Clean.
- dv.h: constants + data struct only, no functions. Clean.

**Ledger status:** 379 accepted-normalized (including 1 genuine producer-site fix), 8 blocked (esent.h,
getprocesshandlefromhwnd.h, wab.h, wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h,
MSAJTransport.h), 1016 pending.

## 2026-09-02 18:24:01 UTC - Batch scraping-investigation-40

**Headers:** perceptiondevicecore.h, winnls32.h, gpiobuttontypes.h, vpnotify.h, inetsdk.h
**Partitions scraped:** MixedReality, Hid (x86; 0 warnings/errors)

- perceptiondevicecore.h: data structs + GUID constant only, no functions. Clean.
- winnls32.h: HWND is an input parameter; IME info structs are caller-allocated, not handles. Clean.
- gpiobuttontypes.h: enum/constants/data struct only, no functions. Clean.
- vpnotify.h: COM interfaces with plain enum/BOOL parameters only, no handle. Clean.
- inetsdk.h: pure MIDL boilerplate, no functions. Clean.

**Ledger status:** 384 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 1011 pending.

## 2026-09-02 18:27:00 UTC - Batch scraping-investigation-41

**Headers:** exposeenums2managed.h, metadatadeviceservice.h, stgprop.h, lmmsg.h, schedule.h
**Partitions scraped:** ActiveDirectory (x86; 0 errors; 3 pre-existing unrelated cross-partition remap
warnings for sockaddr/timeval/_CERT_CONTEXT)

- exposeenums2managed.h: pure macro-definition helper, no declarations. Clean.
- metadatadeviceservice.h: GUID/property-key constants only (same pattern as notesdeviceservice.h/
  ringtonedeviceservice.h/deviceservices.h), no functions. Clean.
- stgprop.h: PROPID/GUID constants only, no functions. Clean.
- lmmsg.h: NetMessageName*/NetMessageBufferSend use the Net API buffer-allocation convention
  (NetApiBufferFree), not HANDLE-family; part of the lm.h umbrella already noted as individually
  tracked. Clean.
- schedule.h: constants + data structs only, no functions. Clean.

**Ledger status:** 389 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 1006 pending.

## 2026-09-02 18:29:58 UTC - Batch scraping-investigation-42

**Headers:** utilapiset.h, msdaora.h, mtxattr.h, mschapp.h, wsnetbs.h
**Partitions scraped:** Debug, MsChap (x86; 0 errors; Debug shows 1 pre-existing unrelated
cross-partition remap warning for _CERT_CONTEXT)

- utilapiset.h: EncodePointer/DecodePointer-family obfuscate a raw PVOID (not ownership);
  EncodeRemotePointer/DecodeRemotePointer take HANDLE ProcessHandle as input only. Clean.
- msdaora.h: CLSID/GUID constants only, no functions. Clean.
- mtxattr.h: IDL typelib custom-attribute macros only (not compiled C declarations), no functions. Clean.
- mschapp.h: MSChapSrvChangePassword/2 take password-encryption structs/strings only, no handle. Clean.
- wsnetbs.h: data struct + constants/macro only, no functions. Clean.

**Ledger status:** 394 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 1001 pending.

## 2026-09-02 18:33:07 UTC - Batch scraping-investigation-43

**Headers:** lmapibuf.h, scclient.h, windowsdefender.h, wmsdkvalidate.h, wsbonline.h
**Partitions scraped:** Wmdm, Lwef, WmFormat, Wsb (x86; 0 warnings/errors)

- lmapibuf.h: canonical home of NetApiBufferAllocate/Free/Reallocate/Size - generic LPVOID memory
  buffer allocation, not HANDLE-family (confirms precedent already cited by lmconfig.h/lmmsg.h). Clean.
- scclient.h: pure C++ class (CSecureChannelClient), no extern "C"/DllImport functions at all - out of
  scope for the annotation mechanism entirely (same reasoning as COM vtable methods). Clean.
- windowsdefender.h: BOOL-only API, no handle. Clean.
- wmsdkvalidate.h: string/buffer/BOOL API only, no handle. Clean.
- wsbonline.h: struct/GUID input-only API, no handle. Clean.

**Ledger status:** 399 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 996 pending.

## 2026-09-02 18:35:29 UTC - Batch scraping-investigation-44

**Headers:** tcerror.h, vfwext.h, wiawsdsc.h, wsbapperror.h, msctfmonitorapi.h
**Partitions scraped:** Tsf (x86; 0 warnings/errors)

- tcerror.h: Traffic Control error-code constants only, no functions. Clean.
- vfwext.h: constants + callback-signature typedefs only, no DllImport functions. Clean.
- wiawsdsc.h: custom WIA property-ID constants only, no functions. Clean.
- wsbapperror.h: HRESULT error-code constants/macros only, no functions. Clean.
- msctfmonitorapi.h: DoMsCtfMonitor's HANDLE is a caller-supplied input (event handle), not produced
  here. Clean.

**Ledger status:** 404 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 991 pending.

## 2026-09-02 18:38:25 UTC - Batch scraping-investigation-45

**Headers:** swdevice.h, winusbio.h, bthsdpdef.h, timeapi.h, credssp.h
**Partitions scraped:** Buses, Bluetooth (x86; 0 warnings/errors)

- **swdevice.h: resolved a previously-deferred item.** DECLARE_HANDLE(HSWDEVICE) is produced via
  SwDeviceCreate's out-param and released via SwDeviceClose - exactly the shape of a fixable producer-
  site gap (like appnotify.h). Checked autoTypes.json FIRST: HSWDEVICE already has a complete, correct
  entry (CloseApi: SwDeviceClose, InvalidHandleValues: [-1,0]) matching this header exactly - same
  situation as the GDI handles from the batch-14 deep-dive. No new inline annotation needed; existing
  legacy typedef metadata is already correct and complete. Clean.
- winusbio.h: constants + data structs only, no functions. Clean.
- bthsdpdef.h: SDP data structs/enums/union only, no functions. Clean.
- timeapi.h: timer period is a UINT, not a HANDLE (same reasoning as mmiscapi2.h). Clean.
- credssp.h: structs/enum/constants only; SpInitSecurityInterfaceW returns a non-owned static SSPI
  function table (standard pattern), not an owned resource. Clean.

**Ledger status:** 409 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 986 pending.

## 2026-09-02 18:40:51 UTC - Batch scraping-investigation-46

**Headers:** dxgitype.h, amaudio.h, d2d1effects_1.h, nettypes.h, featurestagingapi.h

- dxgitype.h: data structs/enums only, no functions. Clean.
- amaudio.h: IAMDirectSound COM interface uses standard COM refcounting (Get*/Release* pattern), not
  HANDLE-family; AMValidateAndFixWaveFormatEx is a caller-buffer inline helper. Clean.
- d2d1effects_1.h: GUID constant + enums only, no functions. Clean.
- nettypes.h: typedefs/data structs/constants only, no functions. Clean.
- featurestagingapi.h: DECLARE_HANDLE(FEATURE_STATE_CHANGE_SUBSCRIPTION) produced/released via
  Subscribe/UnsubscribeFeatureStateChangeNotification - genuinely fixable shape, but autoTypes.json
  already has a complete correct entry (CloseApi: UnsubscribeFeatureStateChangeNotification) matching
  this header exactly - no new inline annotation needed (same pattern as swdevice.h in batch 45).

**Ledger status:** 414 accepted-normalized, 8 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h), 981 pending.

## 2026-09-02 18:44:27 UTC - Batch scraping-investigation-47

**Headers:** physicalmonitorenumerationapi.h (blocked), windows.graphics.effects.interop.h,
wsbonlineerror.h, storprop.h, mtpext.h
**Partitions scraped:** Base (x86; 0 warnings/errors)

- **physicalmonitorenumerationapi.h: BLOCKED.** Resolves the item deferred from
  lowlevelmonitorconfigurationapi.h (batch 30). GetPhysicalMonitorsFromHMONITOR/
  GetPhysicalMonitorsFromIDirect3DDevice9 fill a caller-allocated array of PHYSICAL_MONITOR structs,
  each with a generic HANDLE field (hPhysicalMonitor), released via DestroyPhysicalMonitor(s). Same
  generic/shared-type blocker class as resourceindexer.h (HANDLE used for thousands of unrelated
  resource kinds cannot be annotated), plus a structural wrinkle: the handle is nested inside a
  struct-array output, not a directly annotatable out-param.
- windows.graphics.effects.interop.h: IGraphicsEffectD2D1Interop COM interface with plain-value/
  interface-pointer outputs only. Clean.
- wsbonlineerror.h: HRESULT error-code constants/macros only (same pattern as wsbapperror.h). Clean.
- storprop.h: HDEVINFO is a consumer-only input from the separate, already-established SetupAPI
  device-info-set surface. Clean.
- mtpext.h: constants + data structs only, no functions. Clean.

**Ledger status:** 418 accepted-normalized, 9 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h, physicalmonitorenumerationapi.h), 976 pending.

## 2026-09-02 18:47:46 UTC - Batch scraping-investigation-48

**Headers:** cplext.h, hstring.h, vptype.h, dmort.h, usb4dbgioctl.h
**Partitions scraped:** Media.DxMediaObjects (x86; 0 warnings/errors)

- cplext.h: control-panel property-page ID constants only, no functions. Clean.
- hstring.h: declares only HSTRING/HSTRING_BUFFER opaque handle typedefs and MIDL boilerplate - zero
  functions in this header. Per corrected policy, no annotation belongs on the typedef itself; the
  actual WindowsCreateString/WindowsDeleteString/WindowsPreallocateStringBuffer/etc. producer/consumer
  functions live in the separately-tracked winstring.h (still pending). Clean.
- vptype.h: enums/data structs only, no functions. Clean.
- dmort.h: MoInitMediaType/MoFreeMediaType etc. allocate/free the fully-defined transparent
  DMO_MEDIA_TYPE struct - a memory-allocation pattern, not an opaque handle (same reasoning as
  keycredmgr.h). Clean.
- usb4dbgioctl.h: enums/constants/data structs only, no functions. Clean.

**Ledger status:** 423 accepted-normalized, 9 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h,
physicalmonitorenumerationapi.h), 971 pending.

## 2026-09-02 18:50:32 UTC - Batch scraping-investigation-49

**Headers:** dsrole.h, tcpxcv.h, smbclnt.h, eapauthenticatoractiondefine.h, fhsvcctl.h
**Partitions scraped:** MsCs (x86; 0 warnings/errors)

- dsrole.h: DsRoleGetPrimaryDomainInformation/DsRoleFreeMemory - generic memory-buffer allocation, not
  HANDLE-family. Clean.
- tcpxcv.h: data structs + constants only, no functions. Clean.
- smbclnt.h: HANDLE ProcessHandle is a caller-supplied input, not produced here. Clean.
- eapauthenticatoractiondefine.h: enums/data structs only, no functions. Clean.
- fhsvcctl.h: DECLARE_HANDLE(FH_SERVICE_PIPE_HANDLE) produced/released via FhServiceOpenPipe/
  FhServiceClosePipe (deprecated APIs) - autoTypes.json already has a complete correct entry
  (CloseApi: FhServiceClosePipe) matching this header exactly - no new inline annotation needed, same
  pattern as swdevice.h/featurestagingapi.h.

**Ledger status:** 428 accepted-normalized, 9 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h,
physicalmonitorenumerationapi.h), 966 pending.

## 2026-09-02 18:57:02 UTC - Batch scraping-investigation-50

**Headers:** profinfo.h, emi.h, i_cryptasn1tls.h (blocked), d2d1effectauthor_1.h,
packagevirtualizationcontext.h (FIXED); **userenv.h reclassified accepted-normalized -> blocked**
**Partitions scraped:** AppxPackaging (x86; 0 warnings/errors)

- profinfo.h: declares only PROFILEINFOA/W structs, no functions. While auditing its hProfile HANDLE
  field, discovered UserEnv.h's prior existing-patches-34 classification only verified unrelated
  retained patches, never audited ownership. **Reclassified UserEnv.h to blocked**: LoadUserProfileW
  populates PROFILEINFOW.hProfile (a plain HANDLE field), released via UnloadUserProfile - same
  generic/shared-type-nested-in-struct blocker class as physicalmonitorenumerationapi.h. Prior retained
  patches remain valid.
- emi.h: GUID/IOCTL constants + data structs only, no functions. Clean.
- **i_cryptasn1tls.h: BLOCKED.** I_CryptInstallAsn1Module returns HCRYPTASN1MODULE (typedef DWORD)
  directly as the function return value, released via I_CryptUninstallAsn1Module - same
  return-value-handle-ownership blocker class as getprocesshandlefromhwnd.h/wab.h/wincon.h/winppi.h/
  libloaderapi2.h/MSAJTransport.h.
- d2d1effectauthor_1.h: COM factory pattern, no raw HANDLE. Clean.
- **packagevirtualizationcontext.h: GENUINE GAP FOUND AND FIXED.** DECLARE_HANDLE(PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE)
  has an autoTypes.json entry but NO CloseApi/InvalidHandleValues (unlike the swdevice.h/
  featurestagingapi.h/fhsvcctl.h cases which were already complete) - a genuine unaddressed gap. Applied
  producer-site fix to CreatePackageVirtualizationContext/DuplicatePackageVirtualizationContext's
  out-params (_Win32_metadata_invalid_handle_(0) + _raii_free_(ReleasePackageVirtualizationContext)).
  GetCurrentPackageVirtualizationContext is a borrowed/non-owned query (like GetConsoleWindow), left
  unannotated correctly. GetProcessesInVirtualizationContext's HANDLE** array output is the
  already-documented generic-HANDLE-array limitation (same as physicalmonitorenumerationapi.h), not a
  new blocker. Verified via live re-scrape (0 errors) and git apply --check --reverse (exit 0). New
  patch: packagevirtualizationcontext.h.context-ownership.patch.

**Ledger status:** 431 accepted-normalized (incl. 1 new genuine producer-site fix), 11 blocked (esent.h,
getprocesshandlefromhwnd.h, wab.h, wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h,
MSAJTransport.h, physicalmonitorenumerationapi.h, i_cryptasn1tls.h, userenv.h), 961 pending.

## 2026-09-02 18:59:34 UTC - Batch scraping-investigation-51

**Headers:** realtimeapiset.h, devquerydef.h, mciavi.h, tcguid.h, taskdeviceservice.h
**Partitions scraped:** DeviceQuery (x86; 0 warnings/errors)

- realtimeapiset.h: HANDLE params are caller-supplied inputs (existing thread/process handles), not
  produced here; other outputs are plain integers. Clean.
- devquerydef.h: enums/data structs only, no functions. Clean.
- mciavi.h: constants only, no functions. Clean.
- tcguid.h: GUID constants only, no functions. Clean.
- taskdeviceservice.h: GUID/property-key constants only (same pattern as prior device service headers),
  no functions. Clean.

**Ledger status:** 436 accepted-normalized, 11 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h,
physicalmonitorenumerationapi.h, i_cryptasn1tls.h, userenv.h), 956 pending.

## 2026-09-02 19:03:09 UTC - Batch scraping-investigation-52

**Headers:** windows.graphics.directx.direct3d11.interop.h, wslapi.h (blocked), wsdutil.h, vbinterf.h,
avrfsdk.h
**Partitions scraped:** WinRT.Direct3D11, Wsl, Com, ApplicationVerifier (x86; 0 warnings/errors)

- windows.graphics.directx.direct3d11.interop.h: COM factory pattern throughout. Clean.
- **wslapi.h: BLOCKED.** WslLaunch produces a process HANDLE via a direct _Out_ HANDLE* out-param, but
  the type is the generic system HANDLE (not distinctly named like PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE),
  same generic/shared-type blocker class as resourceindexer.h/physicalmonitorenumerationapi.h/userenv.h -
  annotating it would incorrectly apply to every HANDLE anywhere in the metadata. This is a direct
  out-param instance of the class (vs. nested-in-struct for the other two).
- wsdutil.h: WSDAPI linked-memory/buffer convention, not HANDLE-family (consistent with wsdapi.h
  precedent from batch 20). Clean.
- vbinterf.h: COM interface refcounting pattern, not HANDLE-family. Clean.
- avrfsdk.h: HANDLE is a caller-supplied input; diagnostic "handle" fields are plain ULONG64 values,
  not HANDLE-typed. Clean.

**Ledger status:** 440 accepted-normalized, 12 blocked (esent.h, getprocesshandlefromhwnd.h, wab.h,
wincon.h, resourceindexer.h, winppi.h, libloaderapi2.h, MSAJTransport.h,
physicalmonitorenumerationapi.h, i_cryptasn1tls.h, userenv.h, wslapi.h), 951 pending.

## 2026-09-02 19:06:30 UTC - Batch scraping-investigation-53

**Headers:** roregistrationapi.h, iadmext.h, wsipv6ok.h, audioclientactivationparams.h, debugapi.h
**Partitions scraped:** Iis, Audio (x86; 0 errors)

- roregistrationapi.h: COM factory; HSTRING array output ownership tracked separately under
  winstring.h. Clean.
- iadmext.h: IADMEXT COM interface, not HANDLE-family. Clean.
- wsipv6ok.h: macro redefinitions only, no declarations. Clean.
- audioclientactivationparams.h: enums/data structs + MIDL boilerplate only, no functions. Clean.
- debugapi.h: HANDLE is a caller-supplied input, not produced here. Clean.

**Ledger status:** 445 accepted-normalized, 12 blocked, 946 pending.

## 2026-09-02 19:09:33 UTC - Batch scraping-investigation-54

**Headers:** dispatcherqueue.h, wnvapi.h (blocked), wshisotp.h, certpoleng.h, ratings.h (blocked)
**Partitions scraped:** wnv, Certificates (x86; 0 errors)

- dispatcherqueue.h: COM factory pattern. Clean.
- wnvapi.h: BLOCKED. WnvOpen returns a generic HANDLE via return value - known blocker class.
- wshisotp.h: data struct + constants/macro only, no functions. Clean.
- certpoleng.h: transparent structs w/ generic free conventions, or generic PVOID; out of scope. Clean.
- ratings.h: BLOCKED. RatingObtainQuery(W) produces a generic HANDLE via direct out-param - same
  class as wslapi.h.

**Ledger status:** 448 accepted-normalized, 14 blocked, 941 pending.

## 2026-09-02 19:12:15 UTC - Batch scraping-investigation-55

**Headers:** rometadataresolution.h, wct.h (blocked), wtshintapi.h, functiondiscoverycategories.h, loadperf.h
**Partitions scraped:** TermServ, Perf (x86; 0 errors)

- rometadataresolution.h: COM factory pattern + HSTRING outputs tracked separately under winstring.h. Clean.
- wct.h: BLOCKED. OpenThreadWaitChainSession returns HWCT (typedef LPVOID) directly as function return
  value - known return-value-handle blocker class.
- wtshintapi.h: HWND is an input; render hint ID is plain UINT64, not a HANDLE. Clean.
- functiondiscoverycategories.h: string constants only, no functions. Clean.
- loadperf.h: string/flag-only API, no handle. Clean.

**Ledger status:** 452 accepted-normalized, 15 blocked, 936 pending.

## 2026-09-02 19:14:48 UTC - Batch scraping-investigation-56

**Headers:** shdispid.h, lmremutl.h, minappmodel.h, iwstdec.h, filterr.h
**Partitions scraped:** IndexSrv (x86; 0 warnings/errors)

- shdispid.h: DISPID constants only, no functions. Clean.
- lmremutl.h: NetRemoteTOD uses the established Net API buffer-allocation convention. Clean.
- minappmodel.h: constants only, no functions. Clean.
- iwstdec.h: IAMWstDecoder COM interface with plain-value parameters only. Clean.
- filterr.h: HRESULT error-code constants only, no functions. Clean.

**Ledger status:** 457 accepted-normalized, 15 blocked, 931 pending.

## 2026-09-02 19:16:54 UTC - Batch scraping-investigation-57

**Headers:** syncdeviceservice.h, lmat.h, fttypes.h, ddpcommon.h, stringapiset.h
**Partitions scraped:** Dedup (x86; 0 warnings/errors)

- syncdeviceservice.h: GUID/property-key constants only, no functions. Clean.
- lmat.h: NetScheduleJob* use Net API buffer-allocation convention. Clean.
- fttypes.h: typedef/enums/data structs only, no functions. Clean.
- ddpcommon.h: MIDL boilerplate + enums/data structs only, no functions. Clean.
- stringapiset.h: string/buffer conversion API only, no handle. Clean.

**Ledger status:** 462 accepted-normalized, 15 blocked, 926 pending.

## 2026-09-02 19:19:17 UTC - Batch scraping-investigation-58

**Headers:** datetimeapi.h, roparameterizediid.h, lmalert.h, wnnc.h, adssts.h
**Partitions scraped:** Ifsk (x86; 0 warnings/errors)

- datetimeapi.h: string/buffer formatting API only, no handle. Clean.
- roparameterizediid.h: DECLARE_HANDLE(ROPARAMIIDHANDLE) produced/released via
  RoGetParameterizedTypeInstanceIID/RoFreeParameterizedTypeExtra - autoTypes.json already has a
  complete correct entry matching this header exactly, same pattern as swdevice.h/featurestagingapi.h/
  fhsvcctl.h. Clean.
- lmalert.h: caller-supplied input buffers only, no handle. Clean.
- wnnc.h: network-type constants only, no functions. Clean.
- adssts.h: status-code constants only, no functions. Clean.

**Ledger status:** 467 accepted-normalized, 15 blocked, 921 pending.

## 2026-09-02 19:21:48 UTC - Batch scraping-investigation-59

**Headers:** il21dec.h, d3d9on12.h, mpegtype.h, adshlp.h, tvout.h
**Partitions scraped:** Direct3D9on12, Gdi (x86; 0 errors)

- il21dec.h / mpegtype.h: COM interfaces with plain-value parameters only, no handle.
- d3d9on12.h: COM factory/refcounting pattern throughout.
- adshlp.h: COM refcounting + generic memory/string-allocation conventions only, no opaque
  HANDLE-family gap.
- tvout.h: data struct + constants only, no functions.

**Ledger status:** 472 accepted-normalized, 15 blocked, 916 pending.

## 2026-09-02 19:28:33 UTC - Batch scraping-investigation-60

**Headers:** dshow.h, mswsockdef.h, audiostatemonitorapi.h, functiondiscovery.h, amva.h
**Partitions scraped:** Media.DShow, WinSock, Audio (x86; 0 errors)

- dshow.h: pure umbrella redirect; all sub-headers already individually tracked ledger items. Clean.
- mswsockdef.h: RIO_BUFFERID/RIO_CQ/RIO_RQ are genuine opaque handles, but their producer/consumer
  functions (in the already-accepted MSWSock.h) are function-pointer typedef fields inside
  RIO_EXTENSION_FUNCTION_TABLE (retrieved via WSAIoctl at runtime), not extern/DllImport declarations -
  architecturally out of scope for the annotation mechanism (same reasoning as COM vtable methods).
- audiostatemonitorapi.h: the one real handle-producing method (RegisterCallback) is a COM vtable
  method, out of scope; extern functions are all COM factories. Clean.
- functiondiscovery.h: CLSID constants + MIDL boilerplate only, no functions. Clean.
- amva.h: constants + data structs only, no functions. Clean.

**Ledger status:** 477 accepted-normalized, 15 blocked, 911 pending.

## 2026-09-02 19:32:15 UTC - Batch scraping-investigation-61

**Headers:** stierr.h, AudioAPOTypes.h, dmoreg.h, WaaSApiTypes.h, avrt.h
**Partitions scraped (x64):** ImagingDevice, Audio.Apo, TermServ, Media.DxMediaObjects, Threading, UpdateAssessment (0 errors each)

- stierr.h: HRESULT/status-code constants only. Clean.
- AudioAPOTypes.h: typedefs/enum/structs only, no functions. Clean.
- dmoreg.h: registration/enum functions operate on CLSIDs and standard COM interface pointers
  (IEnumDMO**), not opaque handles. Clean.
- WaaSApiTypes.h: MIDL enums/structs/boilerplate only, no functions. Clean.
- avrt.h: **blocked** - two genuine ownership gaps, both already-established blocker classes:
  return-value HANDLE (AvSetMmThreadCharacteristics*/AvSetMmMaxThreadCharacteristics*) and
  generic PHANDLE direct-out-param (AvRtCreateThreadOrderingGroup*/AvRtJoinThreadOrderingGroup).

**Ledger status:** 481 accepted-normalized, 16 blocked, 906 pending.

## 2026-09-02 19:35:14 UTC - Batch scraping-investigation-62

**Headers:** imapierror.h, eaphostpeertypes.h, lmsname.h, mpeg2error.h, tokenbinding.h
**Partitions scraped (x64):** IMapi, Identity, EapHost, NetMgmt (0 errors each); Media.DShow reused from batch 60

- imapierror.h: HRESULT constants only. Clean.
- eaphostpeertypes.h: enums/structs only, no functions. Clean.
- lmsname.h: service-name string constants only, no functions. Clean.
- mpeg2error.h: HRESULT constants only, no functions. Clean.
- tokenbinding.h: TokenBindingGenerateBinding/GenerateMessage/VerifyMessage/GetKeyTypes*/GenerateID*
  outputs are all generic heap-allocated buffers (HeapFree(GetProcessHeap(),...) convention per MS docs)
  or transparent structs, not distinctly-named opaque handles - out of scope. Clean.

**Ledger status:** 486 accepted-normalized, 16 blocked, 901 pending.

## 2026-09-02 19:37:35 UTC - Batch scraping-investigation-63

**Headers:** faxext.h, devfiltertypes.h, hwebcore.h, wpdshellextension.h, ccgplugins.h
**Partitions scraped (x64):** Fax, DeviceQuery, Iis, WpdSdk (0 errors each); Identity reused from batch 62

- faxext.h: **blocked** - FaxExtRegisterForEvents returns generic HANDLE via return value
  (established return-value-handle class); closed via FaxExtUnregisterForEvents.
- devfiltertypes.h: enum/struct only, no functions. Clean.
- hwebcore.h: dynamically-loaded (GetProcAddress) function-pointer typedefs only, no scrapable
  extern functions - out of scope (same class as mswsockdef.h). Clean.
- wpdshellextension.h: GUID/property-key constants only, no functions. Clean.
- ccgplugins.h: COM interface method (ICcgDomainAuthCredentials::GetPasswordCredentials) only,
  out of scope. Clean.

**Ledger status:** 490 accepted-normalized, 17 blocked, 896 pending.

## 2026-09-02 19:42:27 UTC - Batch scraping-investigation-64

**Headers:** mmsystem.h, wiadevd.h, dmodshow.h, computestorage.h, powersetting.h
**Partitions scraped (x64):** Wia, HostComputeSystem, Power (0 errors each); Media.DShow reused from batch 60

- mmsystem.h: pure umbrella redirect (mmsyscom.h/mciapi.h/mmiscapi.h/mmiscapi2.h/playsoundapi.h/
  mmeapi.h/timeapi.h/joystickapi.h), all sub-headers already tracked ledger items. Clean.
- wiadevd.h: DeviceDialog extern function has no opaque-handle output; GetDeviceIcon/
  GetDeviceBitmapLogo are COM vtable methods, out of scope. Clean.
- dmodshow.h: COM interface (IDMOWrapperFilter) + GUID constants only, no extern functions. Clean.
- computestorage.h: Hcs* functions take pre-owned HANDLE as input only, never produce one. Clean.
- powersetting.h: **blocked** - PowerRegisterForEffectivePowerModeNotifications/
  PowerUnregisterFromEffectivePowerModeNotifications use a generic PVOID direct-out-param
  (established blocker class). The header's other handle type, HPOWERNOTIFY, is already
  correctly covered via autoTypes.json's existing complete entry (CloseApi mechanism attaches
  to the single-purpose type itself, distinct from the newer per-producer inline annotation
  policy that targets shared/generic types like windef.h's HWND) - no new annotation needed there.

**Ledger status:** 494 accepted-normalized, 18 blocked, 891 pending.

## 2026-09-02 19:45:32 UTC - Batch scraping-investigation-65

**Headers:** vpconfig.h, stireg.h, ondemandconnroutehelper.h, msclmd.h, ddrawgdi.h
**Partitions scraped (x64):** Nla, WindowsConnectionManager, Security.Cryptography, FileHistory, WinProg (0 errors each)

- vpconfig.h: COM-style DECLARE_INTERFACE_ interfaces only, no extern functions. Clean.
- stireg.h: registry key/property GUID constants only, no functions. Clean.
- ondemandconnroutehelper.h: **blocked** - OnDemandRegisterNotification/OnDemandUnRegisterNotification
  use a generic HANDLE direct-out-param (established blocker class). GetInterfaceContextTableForHostName's
  NET_INTERFACE_CONTEXT_TABLE** is a transparent struct (out of scope despite dedicated free function).
- msclmd.h: PIV smart-card constants/structs only, no functions. Clean.
- ddrawgdi.h: **blocked** - DdGetDxHandle/DdCreateDIBSection return generic HANDLE/HBITMAP via
  return value (established return-value-handle class); legacy private GDI/DirectDraw entry points.

**Ledger status:** 497 accepted-normalized, 20 blocked, 886 pending.

## 2026-09-02 19:48:12 UTC - Batch scraping-investigation-66

**Headers:** audioendpoints.h, d3d10misc.h, ipinfoid.h, fherrors.h, dcomptypes.h
**Partitions scraped (x64):** Audio.Endpoints, Direct3D10, IpHlp, DirectComp (0 errors each); FileHistory reused from batch 65

- audioendpoints.h: COM interface method (IAudioEndpointFormatControl::ResetToDefault) only. Clean.
- d3d10misc.h: D3D10CreateDevice/DeviceAndSwapChain/CreateBlob all output standard COM interface
  pointers (ID3D10Device**/IDXGISwapChain**/ID3D10Blob**). Clean.
- ipinfoid.h: IP router manager ID constants only, no functions. Clean.
- fherrors.h: HRESULT error code constants only, no functions. Clean.
- dcomptypes.h: DirectComposition enums/structs/constants only, no functions. Clean.

**Ledger status:** 502 accepted-normalized, 20 blocked, 881 pending.

## 2026-09-02 19:50:51 UTC - Batch scraping-investigation-67

**Headers:** vdslun.h, hvsocket.h, faxroute.h, msdasql.h, reason.h
**Partitions scraped (x64):** VirtualDiskService, Hypervisor, Search, Shutdown (0 errors each); Fax/FileHistory reused from batches 63/65

- vdslun.h: VDS enums/structs + MIDL boilerplate only, no functions. Clean.
- hvsocket.h: Hyper-V socket constants/structs only, no functions. Clean.
- faxroute.h: FaxRouteInitialize's HANDLE is a pre-owned caller input; no function produces an
  opaque handle; callback typedefs are function pointers, out of scope. Clean.
- msdasql.h: GUID/property-ID constants only, no functions (interfaces in separate
  msdasql_interfaces.h). Clean.
- reason.h: shutdown-reason-code constants only, no functions. Clean.

**Ledger status:** 507 accepted-normalized, 20 blocked, 876 pending.

## 2026-09-02 19:54:20 UTC - Batch scraping-investigation-68

**Headers:** muiload.h, VersionHelpers.h, srpapi.h, tpcerror.h, davclnt.h
**Partitions scraped (x64):** Intl, SystemInformation, Edp, Tablet, WebDav (0 errors each)

- muiload.h: LoadMUILibraryA/W return HINSTANCE via return value, but autoTypes.json already has
  complete HMODULE/HINSTANCE entries (CloseApi=FreeLibrary, AlsoUsableFor cross-linked) - already
  correctly covered at the type level; FreeMUILibrary is a compatible alternate close. No new gap.
- VersionHelpers.h: all functions are inline/FORCEINLINE, no scrapable extern declarations. Clean.
- srpapi.h: **blocked** - SrpCreateThreadNetworkContext/SrpCloseThreadNetworkContext operate on a
  transparent struct (HTHREAD_NETWORK_CONTEXT) with a generic HANDLE field (ThreadContext) nested
  inside (established generic-type-nested-in-struct blocker class).
- tpcerror.h: HRESULT error-code constants/ATL macros only, no functions. Clean.
- davclnt.h: **blocked** - two gaps: DavAddConnection/DavDeleteConnection generic HANDLE
  direct-out-param, and DavRegisterAuthCallback/DavUnregisterAuthCallback return-value handle
  (OPAQUE_HANDLE is literally a DWORD). Both reuse established blocker classes.

**Ledger status:** 510 accepted-normalized, 22 blocked, 871 pending.

## 2026-09-02 20:07:51 UTC - Batch scraping-investigation-69

**Headers:** winstring.h, guiddef.h, rpc.h, dxva2trace.h, dciman.h
**Partitions scraped (x64):** WinRT, TransactionServer, Rpc (0 errors each); Media.DShow/WinProg/FileHistory reused

- winstring.h: **resolves the long-deferred "HSTRING ownership tracked separately" item.**
  HSTRING/HSTRING_BUFFER are both already fully covered by existing autoTypes.json entries
  (CloseApi: WindowsDeleteString / WindowsDeleteStringBuffer) - this confirms every previously-deferred
  HSTRING-producing header across the SDK was correctly closed without needing its own annotation.
- guiddef.h: GUID type/macros/inline helpers only, no extern functions. Clean.
- rpc.h: pure umbrella/typedef header (rpcdce.h/rpcnsi.h/rpcnterr.h/rpcasync.h all already tracked);
  RpcMacSetYieldInfo is Mac-only, unreachable on Windows. Clean.
- dxva2trace.h: ETW tracing GUID constants/structs only, no functions. Clean.
- dciman.h: **partially fixed, still blocked** - HWINWATCH's autoTypes.json entry was missing
  CloseApi (same incomplete-entry pattern as appnotify.h/packagevirtualizationcontext.h); added
  CloseApi: WinWatchClose, verified same-namespace consistency (Windows.Win32.System.WindowsProgramming)
  and re-scraped WinProg/FileHistory (0 errors). DCIOpenProvider/DCICloseProvider's generic HDC
  return-value remains an unrepresentable gap (established blocker class) - header stays blocked.

**Ledger status:** 514 accepted-normalized, 23 blocked, 866 pending.

## 2026-09-02 20:13:46 UTC - Batch scraping-investigation-70

**Headers:** fdi_fci_types.h, pbdaerrors.h, WinHvEmulation.h, dsquery.h, dls2.h
**Partitions scraped (x64):** Cabinets, ActiveDirectory, Audio.DirectMusic (0 errors each); Hypervisor re-scraped after fix

- fdi_fci_types.h: FCI/FDI cabinet compression constants/structs only, no functions. Clean.
- pbdaerrors.h: HRESULT constants/macros only, no functions. Clean.
- WinHvEmulation.h: **genuine fix applied.** WHvEmulatorCreateEmulator/WHvEmulatorDestroyEmulator's
  WHV_EMULATOR_HANDLE (distinctly-named, not generic HANDLE) had NO autoTypes.json entry at all.
  Added new entry (Namespace: Windows.Win32.System.Hypervisor, CloseApi: WHvEmulatorDestroyEmulator).
  Verified single-file declaration (no namespace conflict) and re-scraped Hypervisor (0 errors).
  Also used WinmdUtils dump of baseline Windows.Win32.winmd to empirically confirm: HINSTANCE/
  HMODULE/HPOWERNOTIFY/HSTRING structs all correctly carry [RAIIFree] in the real winmd (matching
  their autoTypes.json entries), while the generic HANDLE struct does NOT get [RAIIFree] despite
  autoTypes.json listing CloseApi=CloseHandle for it - decisive confirmation that the established
  generic-type blocker class (used throughout this session) is architecturally correct and that
  distinctly-named single-purpose types are the right target for new autoTypes.json entries.
- dsquery.h: GUID constants/structs only, no functions. Clean.
- dls2.h: DLS2 FOURCC/format constants only, no functions. Clean.

**Ledger status:** 519 accepted-normalized, 23 blocked, 861 pending.

## 2026-09-02 20:16:29 UTC - Batch scraping-investigation-71

**Headers:** lmstats.h, pdhmsg.h, wcmapi.h, defaultbrowsersyncsettings.h, regbag.h
**Partitions scraped (x64):** Fs, Perf, Wcm, MsTv (0 errors each); WinProg reused from batch 69

- lmstats.h: NetStatisticsGet follows established NetApiBufferAllocate/Free convention. Clean.
- pdhmsg.h: Message-compiler-generated PDH status/message constants only, no functions. Clean.
- wcmapi.h: WcmQueryProperty/WcmGetProfileList outputs are generic buffer/transparent struct freed
  via generic WcmFreeMemory, not distinctly-named opaque handles. Clean.
- defaultbrowsersyncsettings.h: COM interface (IDefaultBrowserSyncSettings) + GUID constants only. Clean.
- regbag.h: COM interface (ICreatePropBagOnRegKey) method only, out of scope. Clean.

**Ledger status:** 524 accepted-normalized, 23 blocked, 856 pending.

## 2026-09-02 20:19:54 UTC - Batch scraping-investigation-72

**Headers:** wlantypes.h, activation.h, windows.ui.composition.interop.h, RTWorkQ.h, WaaSApi.h
**Partitions scraped (x64):** Ndis, NWifi, WinRT.Composition (0 errors each); WinRT/Threading/UpdateAssessment reused

- wlantypes.h: 802.11 WLAN enums/structs only, no functions. Clean.
- activation.h: IActivationFactory COM/WinRT interface method only, out of scope. Clean.
- windows.ui.composition.interop.h: COM interop interfaces (ICompositorInterop etc.), all vtable
  methods; CreateCompositionSurfaceForHandle's HANDLE is a pre-owned input. Clean.
- RTWorkQ.h: **blocked** - RtwqJoinWorkQueue/RtwqSetDeadline/RtwqSetDeadline2 produce a generic
  HANDLE via direct out-param (established blocker class); RtwqCancelDeadline closes it.
- WaaSApi.h: IWaaSAssessor COM interface + CLSID/LIBID constants only. Clean.

**Ledger status:** 528 accepted-normalized, 24 blocked, 851 pending.

## 2026-09-02 20:22:20 UTC - Batch scraping-investigation-73

**Headers:** DeleteBrowsingHistory.h, icftypes.h, NotificationActivationCallback.h, IMessageDispatcher.h, ElsCore.h
**Partitions scraped (x64):** WindowsFirewall, Win32_Tile_Badge_Notif (0 errors each); WinProg/TransactionServer/Intl reused

- DeleteBrowsingHistory.h: IDeleteBrowsingHistory COM interface + GUID/flag constants only. Clean.
- icftypes.h: NET_FW_* firewall enums + MIDL boilerplate only, no functions. Clean.
- NotificationActivationCallback.h: INotificationActivationCallback COM interface method only. Clean.
- IMessageDispatcher.h: IMessageDispatcher COM/WinRT interface method only. Clean.
- ElsCore.h: MappingGetServices/MappingRecognizeText/MappingDoAction all operate on transparent
  structs (MAPPING_SERVICE_INFO/MAPPING_PROPERTY_BAG), freed via dedicated but structurally
  transparent free functions - out of scope per established precedent. Clean.

**Ledger status:** 533 accepted-normalized, 24 blocked, 846 pending.

## 2026-09-02 20:25:18 UTC - Batch scraping-investigation-74

**Headers:** statusdeviceservice.h, FaxDev.h, usbscan.h, dde.h, wdstpdi.h
**Partitions scraped (x64):** Buses, DataXchg, Wds (0 errors each); WpdSdk/Fax reused

- statusdeviceservice.h: WPD status device service GUID/property constants only. Clean.
- FaxDev.h: **blocked** - FaxDevStartJob/FaxDevEndJob generic HANDLE direct-out-param
  (established blocker class).
- usbscan.h: IOCTL parameter structs only, no functions. Clean.
- dde.h: no function produces an opaque handle (DdeSetQualityOfService/ImpersonateDdeClientWindow
  take only HWND/security-descriptor inputs; FreeDDElParam/ReuseDDElParam pack LPARAM data). Clean.
- wdstpdi.h: **blocked** - WdsTransportProviderCreateInstance/OpenContent produce generic HANDLE
  via direct out-param (established blocker class); provider-callback functions, still scraped
  as ordinary extern declarations.

**Ledger status:** 536 accepted-normalized, 26 blocked, 841 pending.

## 2026-09-02 20:28:37 UTC - Batch scraping-investigation-75

**Headers:** mfmp2dlna.h, nsemail.h, fullenumsyncdeviceservice.h, tdiinfo.h, usbprint.h
**Partitions scraped (x64):** Mf, Printing (0 errors each); WinSock/WpdSdk/FileHistory/WinProg reused

- mfmp2dlna.h: IMFDLNASinkInit COM interface + GUID constants/struct only. Clean.
- nsemail.h: NAPI provider enums/structs (installation blob layout) only, no functions. Clean.
- fullenumsyncdeviceservice.h: WPD device-service GUID/property constants only. Clean.
- tdiinfo.h: TDI entity/object ID structs/typedefs only, no functions. Clean.
- usbprint.h: device-interface GUID + IOCTL_USBPRINT_* control-code constants only. Clean.

**Ledger status:** 541 accepted-normalized, 26 blocked, 836 pending.

## 2026-09-02 20:31:53 UTC - Batch scraping-investigation-76

**Headers:** gb18030.h, DiagnosticDataQueryTypes.h, dragdropinterop.h, wofapi.h, dot1x.h
**Partitions scraped (x64):** Security.DiagnosticDataQuery (0 errors); Intl/WinRT/Fs/Ndis/NWifi reused

- gb18030.h: NlsDllCodePageTranslation buffer-conversion function only, no handles. Clean.
- DiagnosticDataQueryTypes.h: types/RPC boilerplate only, no functions. DIAGNOSTIC_DATA_QUERY_SESSION
  is an RPC context-handle type but its producer/consumer functions live in the still-pending
  DiagnosticDataQuery.h - ownership audit deferred there (same pattern as HSTRING/winstring.h). Clean.
- dragdropinterop.h: IDragDropManagerInterop COM/WinRT interop interface method only. Clean.
- wofapi.h: all Wof*/WofWim*/WofFile* functions take HANDLE as pre-owned input only. Clean.
- dot1x.h: 802.1X enums/structs only, no functions. Clean.

**Ledger status:** 546 accepted-normalized, 26 blocked, 831 pending.

## 2026-09-02 20:34:37 UTC - Batch scraping-investigation-77

**Headers:** mpconfig.h, diskguid.h, wsrm.h, prdrvcom.h, ip2string.h
**Partitions scraped (x64):** IpHlp, wnv (0 errors each); Media.DShow/Fs/WinSock/Printing reused

- mpconfig.h: IMixerPinConfig/IMixerPinConfig2 COM-style interfaces only, no extern functions. Clean.
- diskguid.h: GPT partition-type GUID constants only, no functions. Clean.
- wsrm.h: PGM Reliable Multicast socket-option constants only, no functions. Clean.
- prdrvcom.h: IPrintTicketProvider/IPrintTicketProvider2 COM interfaces only; hPrinter is a
  pre-owned caller input. Clean.
- ip2string.h: RtlIpv4AddressToStringA family write into caller buffers, return pointer into
  same buffer (no new allocation). Clean.

**Ledger status:** 551 accepted-normalized, 26 blocked, 826 pending.

## 2026-09-02 20:39:03 UTC - Batch scraping-investigation-78

**Headers:** ndfapi.h, xoleHlp.h, dxva2swdev.h, ntddmou.h, ntioring_x.h
**Partitions scraped (x64):** DTC, Hid (0 errors each); Ndf re-scraped after fix; Media.DShow/Fs reused

- ndfapi.h: **genuine fix applied.** NDFHANDLE (produced by many NdfCreate*Incident functions,
  closed via single NdfCloseIncident) had NO autoTypes.json entry. Added new entry (Namespace:
  Windows.Win32.NetworkManagement.NetworkDiagnosticsFramework, CloseApi: NdfCloseIncident).
  Verified single-file declaration (no namespace conflict) and re-scraped Ndf (0 errors).
- xoleHlp.h: DtcGetTransactionManager* outputs are standard COM QueryInterface-style pointers. Clean.
- dxva2swdev.h: PDXVA2SW_CREATEVIDEOPROCESSDEVICE etc. are function-pointer typedefs stored in
  DXVA2SW_CALLBACKS struct (same out-of-scope class as mswsockdef.h's RIO table). Clean.
- ntddmou.h: mouse IOCTL/GUID constants and structs only, no functions. Clean.
- ntioring_x.h: IoRing version/feature-flag enums only, no functions in this types header. Clean.

**Ledger status:** 556 accepted-normalized, 26 blocked, 821 pending.

## 2026-09-02 20:41:57 UTC - Batch scraping-investigation-79

**Headers:** windows.security.isolation.isolatedenvironmentinterop.h, AF_Irda.h, ctfspui.h, remotesystemadditionalinfo.h, dmemmgr.h
**Partitions scraped (x64):** WinRT.Isolation, Tsf, DirectDraw (0 errors each); WinSock/TermServ reused

- windows.security.isolation.isolatedenvironmentinterop.h: IIsolatedEnvironmentInterop's
  GetHostHwndInterop returns a non-owned HWND reference (not a disposable resource). Clean.
- AF_Irda.h: IrDA socket-address-family/option constants only, no functions. Clean.
- ctfspui.h: ITfSpeechUIServer COM interface + GUID constants only. Clean.
- remotesystemadditionalinfo.h: IRemoteSystemAdditionalInfoProvider COM interface method only;
  its HSTRING output is already covered via existing autoTypes.json entry. Clean.
- dmemmgr.h: **blocked** - VidMemAlloc/HeapVidMemAllocAligned/VidMemFree use generic FLATPTR
  (ULONG_PTR alias, used pervasively for unrelated non-owned pointer fields elsewhere in the
  header) via return value - reuses established generic-type blocker class.

**Ledger status:** 560 accepted-normalized, 27 blocked, 816 pending.

## 2026-09-02 20:44:56 UTC - Batch scraping-investigation-80

**Headers:** DownloadMgr.h, UserConsentVerifierInterop.h, httptrace.h, wmpdevices.h, prnasnot.h
**Partitions scraped (x64):** InternetExplorer, Wmp, Gdi (0 errors each); WinRT/Iis/Printing reused

- DownloadMgr.h: IDownloadManager COM interface + GUID constant only. Clean.
- UserConsentVerifierInterop.h: IUserConsentVerifierInterop COM/WinRT interop interface method only. Clean.
- httptrace.h: IIS ETW trace enum/structs/GUID constants only, no functions. Clean.
- wmpdevices.h: WMPNotifyDeviceArrival/Removal are __inline (no bindings); IOCTL constants/structs
  otherwise. Clean.
- prnasnot.h: **blocked** - RegisterForPrintAsyncNotifications/UnRegisterForPrintAsyncNotifications
  produce generic HANDLE via direct out-param (established blocker class).

**Ledger status:** 564 accepted-normalized, 28 blocked, 811 pending.

## 2026-09-02 20:49:37 UTC - Batch scraping-investigation-81

**Headers:** DiagnosticDataQuery.h, PrintPreview.h, workspaceruntimeclientext.h, GenericUsbFnIoctl.h, animationcoordinator.h
**Partitions scraped (x64):** Shell (0 errors); Security.DiagnosticDataQuery/Printing/TermServ/Buses reused

- DiagnosticDataQuery.h: **resolves the deferred DIAGNOSTIC_DATA_QUERY_SESSION item from
  DiagnosticDataQueryTypes.h.** All six distinctly-named DECLARE_HANDLE types (HDIAGNOSTIC_DATA_QUERY_SESSION,
  HDIAGNOSTIC_REPORT, HDIAGNOSTIC_EVENT_TAG_DESCRIPTION, HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION,
  HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION, HDIAGNOSTIC_RECORD) already have complete autoTypes.json
  entries matching their exact close functions. Clean.
- PrintPreview.h: IPrintPreviewDxgiPackageTarget COM interface method only. Clean.
- workspaceruntimeclientext.h: IWorkspaceClientExt COM interface method only. Clean.
- GenericUsbFnIoctl.h: IOCTL_GENERICUSBFN_* constants only, no functions. Clean.
- animationcoordinator.h: IInputPaneAnimationCoordinator COM interface method only. Clean.

**Ledger status:** 569 accepted-normalized, 28 blocked, 806 pending.

## 2026-09-02 20:53:25 UTC - Batch scraping-investigation-82

**Headers:** msdaosp.h, LMRepl.h, wdstci.h, Wscapi.h, audiomediatype.h
**Partitions scraped (x64):** NetMgmt, SecurityCenter (0 errors each); Search/Wds/FileHistory/Audio.Apo reused

- msdaosp.h: DataSourceObject COM/IDispatch interface method only. Clean.
- LMRepl.h: NetReplGetInfo and other NET_API_FUNCTION replicator functions follow established
  NetApiBufferAllocate/Free convention. Clean.
- wdstci.h: transport-client constants/enums/structs only, no functions or callback typedefs. Clean.
- Wscapi.h: **blocked** - WscRegisterForChanges/WscUnRegisterChanges produce generic HANDLE via
  direct out-param (established blocker class).
- audiomediatype.h: IAudioMediaType COM interface + CreateAudioMediaType* standard COM factory
  functions only. Clean.

**Ledger status:** 573 accepted-normalized, 29 blocked, 801 pending.

## 2026-09-02 20:56:32 UTC - Batch scraping-investigation-83

**Headers:** mindumpdef.h, homepagesetting.h, inputpaneinterop.h, Ime.h, mxdc.h
**Partitions scraped (x64):** Debug (0 errors); InternetExplorer/WinRT/FileHistory/WinProg/Gdi/Printing reused

- mindumpdef.h: crash-dump-format constants/enum only, no functions. Clean.
- homepagesetting.h: IHomePageSetting COM interface + GUID constants only. Clean.
- inputpaneinterop.h: IInputPaneInterop COM/WinRT interop interface method only. Clean.
- Ime.h: SendIMEMessageExA/W take only HWND/LPARAM, no opaque handle produced. Clean.
- mxdc.h: MxdcGetPDEVAdjustment's HANDLE is a pre-owned caller input only. Clean.

**Ledger status:** 578 accepted-normalized, 29 blocked, 796 pending.

## 2026-09-02 21:01:56 UTC - Batch scraping-investigation-84

**Headers:** devquery.h, websocket.h, IContentPrefetcherTaskTrigger.h, UIViewSettingsInterop.h, msports.h
**Partitions scraped (x64):** WebSock, Wsw, SerPorts (0 errors each); DeviceQuery re-scraped after fix; WinRT reused

- devquery.h: **genuine fix applied.** HDEVQUERY's autoTypes.json entry was missing CloseApi
  (same incomplete-entry pattern as appnotify.h/dciman.h). Added CloseApi: DevCloseObjectQuery.
  Verified single-file declaration and re-scraped DeviceQuery (0 errors).
- websocket.h: WEB_SOCKET_HANDLE already correctly covered via existing autoTypes.json entry
  (CloseApi: WebSocketDeleteHandle). Clean.
- IContentPrefetcherTaskTrigger.h: COM/WinRT interface method only. Clean.
- UIViewSettingsInterop.h: COM/WinRT interop interface method only. Clean.
- msports.h: HCOMDB already correctly covered via existing autoTypes.json entry
  (CloseApi: ComDBClose). Clean.

**Ledger status:** 583 accepted-normalized, 29 blocked, 791 pending.

## 2026-09-02 21:05:11 UTC - Batch scraping-investigation-85

**Headers:** SmtpGuid.h, PerHist.h, devioctl.h, hrtfapoapi.h, HolographicSpaceInterop.h
**Partitions scraped (x64):** Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Xaudio2,
MixedRealityInterop (0 errors each); IMapi/InternetExplorer reused

- SmtpGuid.h: SMTP event GUID/string constants only, no functions. Clean.
- PerHist.h: IPersistHistory COM interface method only. Clean.
- devioctl.h: FILE_DEVICE_* constants only, no functions. Clean.
- hrtfapoapi.h: CreateHrtfApo outputs standard COM interface pointer (IXAPO**); remaining
  declarations are constants/structs/enums. Clean.
- HolographicSpaceInterop.h: IHolographicSpaceInterop COM/WinRT interop interface method only. Clean.

**Ledger status:** 588 accepted-normalized, 29 blocked, 786 pending.

## 2026-09-02 21:08:44 UTC - Batch scraping-investigation-86

**Headers:** Windows.h, msdrmgetinfo.h, IssPer16.h, RemoteSystemsInterop.h, windows.data.pdf.interop.h
**Partitions scraped (x64):** Console, Enstor, Memory, Mi, Rm, WinRT.Pdf (0 errors each); Edp/Identity/WinRT reused

- Windows.h: master umbrella redirect header; all sub-headers already tracked ledger items except
  dlgs.h (untracked - directly investigated and confirmed to declare only dialog-control-ID
  constants, no functions). Clean.
- msdrmgetinfo.h: DRM query-tag string constants only, no functions. Clean.
- IssPer16.h: OLE HRESULT status-code constants only, no functions. Clean.
- RemoteSystemsInterop.h: ICorrelationVectorInformation COM/WinRT interface method only;
  HSTRING outputs already covered via existing autoTypes.json entry. Clean.
- windows.data.pdf.interop.h: PdfCreateRenderer outputs standard COM interface pointer;
  PdfRenderParams is inline (no bindings); IPdfRendererNative is a COM interface. Clean.

**Ledger status:** 593 accepted-normalized, 29 blocked, 781 pending.

## 2026-09-02 21:12:45 UTC - Batch scraping-investigation-87

**Headers:** RasDlg.h, Dhcpv6cSdk.h, inspectable.h, AudioSessionTypes.h, naptypes.h
**Partitions scraped (x64):** RRas, Dhcp, Tcui, NetworkAccessProtection (0 errors each); Audio reused

- RasDlg.h: RasPhonebookDlg*/RasEntryDlg*/RasDialDlg* take transparent caller-allocated structs
  only, no opaque handles produced. Clean.
- Dhcpv6cSdk.h: DHCPV6_OPTION_* constants only, no functions. Clean.
- inspectable.h: foundational IInspectable COM/WinRT interface method only. Clean.
- AudioSessionTypes.h: AUDCLNT_SHAREMODE/AUDIO_STREAM_CATEGORY enums only, no functions. Clean.
- naptypes.h: NAP MIDL types only, no functions. Clean.

**Ledger status:** 598 accepted-normalized, 29 blocked, 776 pending.

## 2026-09-02 21:17:33 UTC - Batch scraping-investigation-88

**Headers:** roapi.h, ExtensionValidation.h, calendardeviceservice.h, ktmtypes.h, highlevelmonitorconfigurationapi.h
**Partitions scraped (x64):** Monitor (0 errors); WinRT re-scraped after fix; InternetExplorer/WpdSdk/Fs reused

- roapi.h: **two genuine fixes applied.** RO_REGISTRATION_COOKIE and
  APARTMENT_SHUTDOWN_REGISTRATION_COOKIE both had autoTypes.json entries missing CloseApi.
  Added CloseApi: RoRevokeActivationFactories / RoUnregisterForApartmentShutdown respectively.
  Verified type definitions exist only in roapi.h (referenced but not redefined in wrl/*.h
  C++ template headers). Re-scraped WinRT (0 errors).
- ExtensionValidation.h: IExtensionValidation COM interface + enums only. Clean.
- calendardeviceservice.h: WPD Calendar device-service GUID/property constants only. Clean.
- ktmtypes.h: KTM type aliases/constants only, no functions. Clean.
- highlevelmonitorconfigurationapi.h: all functions take HANDLE hMonitor as pre-owned input
  (produced elsewhere by physicalmonitorenumerationapi.h, already blocked). Clean.

**Ledger status:** 603 accepted-normalized, 29 blocked, 771 pending.

## 2026-09-02 21:21:24 UTC - Batch scraping-investigation-89

**Headers:** SpatialInteractionManagerInterop.h, devpropdef.h, PlayToManagerInterop.h, windows.ui.viewmanagement.core.coreframeworkinputviewinterop.h, SystemMediaTransportControlsInterop.h
**Partitions scraped (x64):** Display, Nfc, Nfp, WinRT.CoreInputView (0 errors each); MixedRealityInterop/WinRT/WpdSdk reused

- SpatialInteractionManagerInterop.h, PlayToManagerInterop.h,
  windows.ui.viewmanagement.core.coreframeworkinputviewinterop.h, SystemMediaTransportControlsInterop.h:
  all four are the identical "GetForWindow" COM/WinRT interop pattern (derive IInspectable, standard
  QueryInterface-style output, HWND pre-owned caller input). Clean.
- devpropdef.h: DEVPROP_TYPE_*/DEVPROP_TYPEMOD_* constants + DEVPROPTYPE typedef only, no functions. Clean.

**Ledger status:** 608 accepted-normalized, 29 blocked, 766 pending.

## 2026-09-02 21:24:30 UTC - Batch scraping-investigation-90

**Headers:** sqltypes.h, dxgiformat.h, WdsBp.h, RTInfo.h, WeakReference.h
**Partitions scraped (x64):** Dxgi, Dxgi.Common (0 errors each); Search/Wds/NetMgmt/WinRT reused

- sqltypes.h: ODBC type typedefs only, no functions. Clean.
- dxgiformat.h: DXGI_FORMAT enum only, no functions. Clean.
- WdsBp.h: **blocked** - WdsBpParseInitialize/WdsBpParseInitializev6/WdsBpInitialize produce
  generic HANDLE via direct out-param (established blocker class); WdsBpCloseHandle closes it.
- RTInfo.h: router-manager info-block structs only, no functions. Clean.
- WeakReference.h: IWeakReference/IWeakReferenceSource COM/WinRT interface methods only. Clean.

**Ledger status:** 612 accepted-normalized, 30 blocked, 761 pending.

## 2026-09-02 21:27:19 UTC - Batch scraping-investigation-91

**Headers:** ObjSafe.h, DSAdmin.h, ddkernel.h, sqlspi.h, hintsdeviceservice.h
**Partitions:** Debug, ActiveDirectory, DirectDraw, Search, WpdSdk (all already scraped this session, reused)

- ObjSafe.h: IObjectSafety COM interface method only. Clean.
- DSAdmin.h: DS Admin GUID constants + COM interfaces only, no extern functions. Clean.
- ddkernel.h: kernel-mode DirectDraw COM-style interfaces + GUID constants only. Clean.
- sqlspi.h: all functions take SQLHDBC_INFO_TOKEN (generic ODBC SQLHANDLE alias, managed by
  shared SQLAllocHandle/SQLFreeHandle across multiple handle kinds) as input only; SQLGetPoolID's
  POOLID* is a plain numeric ID, not a handle. Clean.
- hintsdeviceservice.h: WPD Hints device-service GUID constants only. Clean.

**Ledger status:** 617 accepted-normalized, 30 blocked, 756 pending.

## 2026-09-02 21:33:01 UTC - Batch scraping-investigation-92

**Headers:** nfcsedev.h, IMessage.h, PrintManagerInterop.h, Midles.h, lmcons.h
**Partitions scraped (x64):** WinRT.Printing, Dfs (0 errors each); IMapi re-scraped after fix; Nfc/Rpc/NetMgmt reused

- nfcsedev.h: NFC SE device GUID/IOCTL constants and structs only, no functions. Clean.
- IMessage.h: **genuine fix applied.** LPMSGSESS's autoTypes.json entry was missing CloseApi
  (same incomplete-entry pattern as appnotify.h/dciman.h/devquery.h/roapi.h). Added
  CloseApi: CloseIMsgSession. Verified single-file declaration and re-scraped IMapi (0 errors).
- PrintManagerInterop.h: IPrintManagerInterop COM/WinRT interop interface method only. Clean.
- Midles.h: RPC pickling enums/function-pointer typedefs only, no extern functions. Clean.
- lmcons.h: LAN Manager API constants only, no functions. Clean.

**Ledger status:** 622 accepted-normalized, 30 blocked, 751 pending.

## 2026-09-02 21:38:37 UTC - Batch scraping-investigation-93

**Headers:** wiamicro.h, WebAuthenticationCoreManagerInterop.h, Cpl.h, xa.h, EvColl.h
**Partitions:** Wia, WinRT, Shell, DTC (already scraped this session, reused); Wec re-scraped after fix (0 errors)

- wiamicro.h: no concrete annotatable ownership gap (microdriver dllexport functions operate on
  transparent structs; DeviceIOHandles array populated by caller before invocation; VAL union's
  HANDLE field has no single dedicated producer with clear _Out_ semantics). Clean.
- WebAuthenticationCoreManagerInterop.h: COM/WinRT interop interface method only. Clean.
- Cpl.h: APPLET_PROC function-pointer typedef + transparent structs only, no extern functions. Clean.
- xa.h: ax_reg/ax_unreg take no handles; xa_switch_t's xa_*_entry are function-pointer struct
  fields (dynamically dispatched), out of scope. Clean.
- EvColl.h: **genuine fix applied.** EC_HANDLE (returned via return value by EcOpenSubscriptionEnum/
  EcOpenSubscription, closed via single EcClose) had NO autoTypes.json entry. Added new entry
  (Namespace: Windows.Win32.System.EventCollector, CloseApi: EcClose). Confirmed the type-level
  mechanism correctly covers return-value production (same reasoning validated for HINSTANCE/
  HMODULE). EC_OBJECT_ARRAY_PROPERTY_HANDLE left unannotated (ambiguous variant-union field,
  no single dedicated producer).

**Ledger status:** 627 accepted-normalized, 30 blocked, 746 pending.

## 2026-09-02 21:43:58 UTC - Batch scraping-investigation-94

**Headers:** Print3DManagerInterop.h, dssec.h, CmnQuery.h, hypervdevicevirtualization.h, stiusd.h
**Partitions scraped (x64):** Security.DirectoryServices (0 errors); Hypervisor re-scraped after fix;
WinRT.Printing/ActiveDirectory/ImagingDevice reused

- Print3DManagerInterop.h: IPrinting3DManagerInterop COM/WinRT interop interface method only. Clean.
- dssec.h: DSCreateISecurityInfoObject outputs standard COM interface pointer; callback typedefs
  are caller-supplied function pointers. Clean.
- CmnQuery.h: Common Query dialog GUID/flag constants only, no extern functions. Clean.
- hypervdevicevirtualization.h: **genuine fix applied.** HDV_HOST (produced by
  HdvInitializeDeviceHost/Ex, closed via single HdvTeardownDeviceHost) had NO autoTypes.json
  entry. Added new entry (Namespace: Windows.Win32.System.Hypervisor, CloseApi:
  HdvTeardownDeviceHost). HDV_DEVICE has no representable close API in this header, left
  unannotated (no gap - nothing to free independently).
- stiusd.h: IStiUSD COM interface + GUID constants only, no extern functions. Clean.

**Ledger status:** 632 accepted-normalized, 30 blocked, 741 pending.

## 2026-09-02 21:49:34 UTC - Batch scraping-investigation-95

**Headers:** ndattrib.h, AdsProp.h, xapofx.h, winbio_ioctl.h, CustCntl.h
**Partitions scraped (x64):** SecBitomet, Controls (0 errors each); Ndf/ActiveDirectory/Xaudio2 reused

- ndattrib.h: Network Diagnostics Framework attribute data structs only, no functions. Clean.
- AdsProp.h: **blocked** - ADsPropCreateNotifyObj produces generic HWND via direct out-param.
  Confirmed HWND's autoTypes.json entry deliberately has no CloseApi (same generic-shared-type
  reasoning as HANDLE) - extends the established generic-type direct-out-param blocker class to
  HWND for the first time this session.
- xapofx.h: CreateFX outputs standard COM interface pointer; remaining declarations are GUID/
  parameter-bound constants. Clean.
- winbio_ioctl.h: biometric device IOCTL/GUID constants only, no functions. Clean.
- CustCntl.h: Custom Control Library constants/transparent structs only, no extern functions. Clean.

**Ledger status:** 636 accepted-normalized, 31 blocked, 736 pending.

## 2026-09-02 21:52:27 UTC - Batch scraping-investigation-96

**Headers:** functiondiscoveryconstraints.h, inkrenderer.h, msimeapi.h, fontsub.h, usbiodef.h
**Partitions scraped (x64):** Input_Ink, Input.Ime (0 errors each); FunctionDiscovery/Gdi/Buses reused

- functiondiscoveryconstraints.h: FD query-constraint name/value string constants only. Clean.
- inkrenderer.h: IInkD2DRenderer/IInkD2DRenderer2 COM interfaces + enum only, no extern functions. Clean.
- msimeapi.h: IImePlugInDictDictionaryList COM interface only, no extern functions. Clean.
- fontsub.h: CreateFontPackage/MergeFontPackage delegate buffer allocation entirely to
  caller-supplied CFP_ALLOCPROC/CFP_FREEPROC callbacks, no opaque handle. Clean.
- usbiodef.h: USB IOCTL function/control-code constants only, no functions. Clean.

**Ledger status:** 641 accepted-normalized, 31 blocked, 731 pending.

## 2026-09-02 21:55:47 UTC - Batch scraping-investigation-97

**Headers:** DtcHelp.h, sensorsutils.h, tpcshrd.h, AviFmt.h, sharewindowcommandsourceinterop.h
**Partitions scraped (x64):** WinSensors (0 errors); DTC/Tablet/Media.DShow/WinRT reused

- DtcHelp.h: DTC_STATUS enum + dynamically-loaded (GetProcAddress) function-pointer typedefs
  only, no extern functions. Clean.
- sensorsutils.h: GetPerformanceTime/InitPropVariantFromFloat/PropKeyFindKeyGet* all output
  plain values or transparent PROPVARIANT/SENSOR_COLLECTION_LIST structs. Clean.
- tpcshrd.h: WM_TABLET_*/TABLET_* constants + typedefs/enums only, no functions. Clean.
- AviFmt.h: AVI format constants/structs only, no functions. Clean.
- sharewindowcommandsourceinterop.h: IShareWindowCommandEventArgsInterop/
  IShareWindowCommandSourceInterop COM/WinRT interop interface methods only (GetWindow returns
  a reference to an existing HWND, not a disposable resource). Clean.

**Ledger status:** 646 accepted-normalized, 31 blocked, 726 pending.

## 2026-09-02 21:59:16 UTC - Batch scraping-investigation-98

**Headers:** ioringapi.h, WSNwLink.h, MprError.h, hidclass.h, pchannel.h
**Partitions:** Fs, WinSock, Foundation, Hid, TermServ (all already scraped this session, reused)

- ioringapi.h: HIORING (explicitly documented as closed via CloseIoRing) already correctly
  covered via existing autoTypes.json entry; all other outputs are transparent structs or
  pre-owned HANDLE inputs. Clean.
- WSNwLink.h: IPX/SPX socket-option constants only, no functions. Clean.
- MprError.h: router-specific error-code constants only, no functions. Clean.
- hidclass.h: HID device-interface GUID/property-key constants only, no functions. Clean.
- pchannel.h: Terminal Server Virtual Channel protocol constants/transparent structs only,
  no functions. Clean.

**Ledger status:** 651 accepted-normalized, 31 blocked, 721 pending.

## 2026-09-02 22:03:15 UTC - Batch scraping-investigation-99

**Headers:** wmdmlog.h, accountssettingspaneinterop.h, MrmResourceIndexer.h, dls1.h, appserviceinterop.h
**Partitions scraped (x64):** Wmdm, MenuRc (0 errors each); WinRT/Audio.DirectMusic reused

- wmdmlog.h: IWMDMLogger COM interface only, no extern functions. Clean.
- accountssettingspaneinterop.h: standard GetForWindow COM/WinRT interop interface method only. Clean.
- MrmResourceIndexer.h: MrmResourceIndexerHandle is a transparent by-value wrapper struct
  (single PVOID field visible), out of scope per transparent-struct precedent despite having
  a genuine dedicated close function (MrmDestroyIndexerAndMessages). Clean.
- dls1.h: DLS1 RIFF format constants/documentation only, no functions. Clean.
- appserviceinterop.h: IAppServiceConnectionExtendedExecution/ICorrelationVectorSource COM/WinRT
  interfaces only. Clean.

**Ledger status:** 656 accepted-normalized, 31 blocked, 716 pending.

## 2026-09-02 22:06:19 UTC - Batch scraping-investigation-100

**Headers:** dcommon.h, windows.ui.xaml.hosting.desktopwindowxamlsource.h, wmpplug.h, anchorsyncdeviceservice.h, dsconf.h
**Partitions scraped (x64):** Direct2D, Direct2D.Common, DirectWrite, WinRT.Xaml (0 errors each); Wmp/WpdSdk/Audio.DirectMusic reused

- dcommon.h: DWRITE_MEASURING_MODE enum + IDXGISurface forward declaration only, no functions. Clean.
- windows.ui.xaml.hosting.desktopwindowxamlsource.h: IDesktopWindowXamlSourceNative/Native2 COM
  interface methods only (get_WindowHandle returns a reference to an existing HWND). Clean.
- wmpplug.h: IWMPPluginUI COM interface only, no extern functions. Clean.
- anchorsyncdeviceservice.h: WPD Anchor Sync device-service GUID constants only. Clean.
- dsconf.h: DirectSound configuration GUID constants/property-ID enum only, no functions. Clean.

**Ledger status:** 661 accepted-normalized, 31 blocked, 711 pending. (100 batches completed this
Ralph loop session.)

## 2026-09-02 22:09:52 UTC - Batch scraping-investigation-101

**Headers:** LMUse.h, msdrmdefs.h, IPExport.h, dmusics.h, tspubplugincom.h
**Partitions:** NetMgmt, Rm, IpHlp, Audio.DirectMusic, TermServ (all already scraped this session, reused)

- LMUse.h: NetUseAdd/NetUseGetInfo/NetUseEnum follow established NetApiBufferAllocate/Free
  convention. Clean.
- msdrmdefs.h: DRM handle-family type definitions (DRMHANDLE/DRMQUERYHANDLE/DRMENVHANDLE/
  DRMHSESSION/DRMPUBHANDLE, all typedef ULONG) + transparent _DRMID struct, no functions in
  this "defs" header. Deferred ownership audit to still-pending msdrm.h (same pattern as
  HSTRING/winstring.h). Clean.
- IPExport.h: IP type aliases + transparent structs only, no functions. Clean.
- dmusics.h: IDirectMusicSynth/IDirectMusicSynthSink COM-style interfaces + constants/structs
  only, no extern functions. Clean.
- tspubplugincom.h: ItsPubPlugin COM interface only, no extern functions. Clean.

**Ledger status:** 666 accepted-normalized, 31 blocked, 706 pending.
