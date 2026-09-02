# `amsi.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-05`
- **Partitions:** Antimalware
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. Found a genuine resource-ownership gap directly verifiable from the
  header itself (no external documentation needed): `HAMSICONTEXT` (`DECLARE_HANDLE`) is
  produced by `AmsiInitialize` (`_Outptr_ HAMSICONTEXT* amsiContext`) and released by
  `AmsiUninitialize` (`_In_ HAMSICONTEXT amsiContext`); `HAMSISESSION` is produced by
  `AmsiOpenSession` (`_Out_ HAMSISESSION* amsiSession`) and released by `AmsiCloseSession`.
- **Resolution:** Added `_Win32_metadata_invalid_handle_(0)` and
  `_Win32_metadata_raii_free_(...)` to both producer output parameters, per the corrected
  shared-handle ownership policy (165b5f09, 7335ddc4) — on the producer parameters, not the
  `HAMSICONTEXT`/`HAMSISESSION` typedefs. Added the `win32metadata_annotations.h` include
  guard (this is a MIDL-generated header; the guard was added immediately after its existing
  `#include <winapifamily.h>`, matching convention used elsewhere for MIDL-derived headers).
- **Artifact:**
  `generation/WinSDK/patches/post-midl/amsi.h.antimalware-context-ownership.patch` (new).
- **Evidence:** `git apply --check --reverse` passes for the new patch. Live re-scrape
  (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86 -t:ScrapeHeaders
  -p:PartitionFilter=Antimalware`) succeeds with 0 warnings/errors, confirming the
  annotation placement is syntactically valid C parsed cleanly by Clang. Static audit
  confirms zero remaining ownership annotations adjacent to a `typedef`/`DECLARE_HANDLE`
  site.
- **Normalization:** Cleanup ownership and invalid values are producer-use metadata, not
  typedef metadata.
