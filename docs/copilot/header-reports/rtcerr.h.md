# `rtcerr.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-06`
- **Partitions:** RealTimeCommunications
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. This header declares only RTC `HRESULT` error-code constants; static
  review confirms no `DECLARE_HANDLE` typedefs.
- **Resolution:** No patch needed — there is no function surface to annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=RealTimeCommunications`) succeeds with 0
  warnings/errors. Static review confirms this header is constant-only.
- **Normalization:** No normalization required; header is constant-only.
