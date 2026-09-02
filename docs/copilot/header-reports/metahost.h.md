# `metahost.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-03`
- **Partitions:** ClrHosting
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. This header declares the CLR hosting COM interfaces
  (`ICLRMetaHost`, `ICLRRuntimeInfo`, etc.) and `CLRCreateInstance`; no `DECLARE_HANDLE`
  typedefs are present (COM interface pointers use the standard `_Outptr_`/`ppInterface`
  pattern, not raw-handle ownership).
- **Resolution:** No patch needed. Live re-scrape confirms all real functions
  (`CLRCreateInstance`, etc.) already receive `[SupportedOSPlatform(...)]` from this
  repository's global `generation/WinSDK/supportedOS.rsp` mechanism with no inline
  annotation required.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=ClrHosting`) succeeds with 0 warnings/errors. Static
  review confirms no `DECLARE_HANDLE` typedefs in this header.
- **Normalization:** No normalization required.
