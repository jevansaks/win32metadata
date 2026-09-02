# `madcapcl.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-11`
- **Partitions:** MadCap
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. This header declares the Multicast Address Dynamic Client (MADCAP) API
  surface (`McastApiStartup`/`McastApiCleanup`, `McastRequestAddress`, etc., 7 functions);
  static review confirms no `HANDLE`-typed parameters at all — all state is passed via
  plain structs (`LPMCAST_CLIENT_UID`, `PMCAST_LEASE_REQUEST`, etc.) and `DWORD`s.
- **Resolution:** No patch needed — no ownership-relevant handle type present.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=MadCap`) succeeds with 0 warnings/errors. Static
  review of all 7 function signatures confirms no `HANDLE`/`DECLARE_HANDLE` parameters.
- **Normalization:** No normalization required.
