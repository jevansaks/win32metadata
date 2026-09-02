# `restartmanager.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-09`
- **Partitions:** RstMgr
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. The header declares 11 Restart Manager functions
  (`RmStartSession`/`RmEndSession`, etc.); static review confirms the session handle is a
  plain `DWORD` (`dwSessionHandle`), not a pointer-sized `HANDLE`/`DECLARE_HANDLE` type, so
  no producer-site ownership annotation applies.
- **Resolution:** No patch needed — no ownership-relevant handle type present.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=RstMgr`) succeeds with 0 warnings/errors. Static
  review confirms `RmStartSession`'s session identifier is a `DWORD`, not a `HANDLE`.
- **Normalization:** No normalization required.
