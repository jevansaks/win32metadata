# `gpmgmt.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-12`
- **Partitions:** Gpmc
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. This header declares the Group Policy Management Console (GPMC) COM
  interface API surface (`IGPM`, `IGPMDomain`, etc., 16 methods across interfaces); static
  review confirms no `DECLARE_HANDLE` typedefs and no `_Out_`/`_Outptr_ HANDLE`
  output-parameter patterns.
- **Resolution:** No patch needed — COM interface pointers use standard `_Outptr_`
  semantics, not raw-handle ownership.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Gpmc`) succeeds with 0 warnings/errors. Static review
  confirms no ownership-relevant patterns.
- **Normalization:** No normalization required.
