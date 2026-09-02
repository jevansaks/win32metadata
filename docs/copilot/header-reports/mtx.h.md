# Header Report: mtx.h

## Partitions
`ComOle`, `TransactionServer`

## Scrape validation
- Re-scraped `ComOle` (`ScanArch=x86`, `ExcludeFromCrossarch`) and `TransactionServer` (`ScanArch=x86` default) partitions after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` for both.

## Ownership audit (producer-site-only policy)
- `mtx.h` is a redirect-only header: under `WINAPI_PARTITION_DESKTOP` it `#include "comsvcs.h"`; it declares nothing itself.
- `comsvcs.h` is already `accepted-normalized` in the ledger, so all actual declarations reachable through `mtx.h` have already been audited.

## Conclusion
`accepted-normalized` — redirect-only header; underlying content (`comsvcs.h`) already classified clean.
