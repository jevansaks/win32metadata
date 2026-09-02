# `tbs.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-07`
- **Partitions:** Tbs
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. Found a genuine resource-ownership gap directly verifiable from the
  header itself (no external documentation needed): `TBS_HCONTEXT` (`typedef PVOID
  TBS_HCONTEXT, *PTBS_HCONTEXT;`) is produced by `Tbsi_Context_Create` and
  `Tbsi_Tpm_Vendor_Maintenance_Mode` (both `_Out_ PTBS_HCONTEXT phContext`) and released by
  `Tbsip_Context_Close` (`_In_ TBS_HCONTEXT hContext`).
- **Resolution:** Added `_Win32_metadata_invalid_handle_(0)` and
  `_Win32_metadata_raii_free_(Tbsip_Context_Close)` to both producer output parameters, per
  the corrected shared-handle ownership policy (165b5f09, 7335ddc4) — on the producer
  parameters, not the `TBS_HCONTEXT` typedef. Added the `win32metadata_annotations.h`
  include guard.
- **Artifact:** `generation/WinSDK/patches/post-midl/tbs.h.tpm-context-ownership.patch`
  (new).
- **Evidence:** `git apply --check --reverse` passes for the new patch. Live re-scrape
  (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86 -t:ScrapeHeaders
  -p:PartitionFilter=Tbs`) succeeds with 0 warnings/errors, confirming the annotation
  placement is syntactically valid C parsed cleanly by Clang. Static audit confirms zero
  remaining ownership annotations adjacent to a `typedef` site.
- **Normalization:** Cleanup ownership and invalid values are producer-use metadata, not
  typedef metadata.
