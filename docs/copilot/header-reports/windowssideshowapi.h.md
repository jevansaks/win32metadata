# `windowssideshowapi.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-06`
- **Partitions:** SideShow
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. This header declares core Windows SideShow gadget/content COM
  interface constants and base types; static review confirms no `DECLARE_HANDLE` typedefs.
- **Resolution:** No patch needed — there is no ownership-relevant function surface to
  annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=SideShow`) succeeds with 0 warnings/errors. Static
  review confirms no ownership-relevant patterns.
- **Normalization:** No normalization required.
