# `mi.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-12`
- **Partitions:** Mi
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors (only the same pre-existing cross-partition auto-remap-consistency
  warnings already documented for `ual.h`, unrelated to this header). This header declares
  3 Management Infrastructure (MI) API functions; static review confirms no
  `DECLARE_HANDLE` typedefs and no `_Out_`/`_Outptr_ HANDLE` output-parameter patterns —
  MI application/session state is passed via `MI_Application`/`MI_Session` structs, not
  raw handles.
- **Resolution:** No patch needed — no ownership-relevant handle type present.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Mi`) succeeds with 0 warnings/errors beyond the
  pre-existing cross-partition remap notices. Static review confirms no ownership-relevant
  patterns.
- **Normalization:** No normalization required.
