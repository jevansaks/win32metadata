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
