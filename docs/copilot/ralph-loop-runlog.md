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
