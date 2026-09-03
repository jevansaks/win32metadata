# Header Report: wcnapi.h

## Partitions
`Wcn`

## Scrape validation
- Re-scraped `Wcn` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 0 `DllImport` functions.

## Ownership audit (producer-site-only policy)
- `wcnapi.h` is a redirect-only header (`#include <WcnTypes.h> <WcnDevice.h> <WcnFunctionDiscoveryKeys.h>`); declares nothing itself.
- No `DECLARE_HANDLE` found in `WcnTypes.h`/`WcnDevice.h`. Zero `DllImport` functions scraped for the entire `Wcn` partition (this is a types/constants-only WCN — Windows Connect Now — data model header set, no functions).

## Conclusion
`accepted-normalized` — redirect-only header; zero functions in the entire partition, no ownership annotation possible or required.
