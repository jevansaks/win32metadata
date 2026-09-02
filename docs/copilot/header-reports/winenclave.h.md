# `winenclave.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-04`
- **Partitions:** Enclave
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape — this partition is **not** in `ExcludeFromCrossarch`, so its output is
  architecture-specific under `obj/generated/x86/`, unlike most other partitions
  investigated this session) succeeded with 0 warnings/errors (only the same pre-existing
  cross-partition auto-remap-consistency warnings already documented for `ual.h`/`Ual`,
  unrelated to this header's own content). The header declares 19 enclave (VBS/SGX)
  functions; static review confirms no `DECLARE_HANDLE` typedefs and no `_Out_`/`_Outptr_
  HANDLE` output-parameter patterns requiring ownership annotation.
- **Resolution:** No patch needed for ownership; supported-os/last-error already handled by
  this repository's global `supportedOS.rsp`/`WithSetLastError.rsp` mechanisms where
  applicable.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Enclave`) succeeds with 0 warnings/errors beyond the
  pre-existing cross-partition remap notices. Static review confirms no ownership-relevant
  patterns.
- **Normalization:** No normalization required.
