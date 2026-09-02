# Header Report: search.h

## Partitions
`Search`

## Scrape validation
- Re-scraped `Search` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `search.h` is a pure CRT include-guard redirect (`#include <corecrt_search.h>`) — it declares nothing
  itself. `corecrt_search.h` (C runtime `bsearch`/`qsort` family) is outside the Win32 API surface
  tracked by this ledger and contains no handle-producing functions.

## Conclusion
`accepted-normalized` — redirect-only header with no direct declarations; underlying CRT content has no ownership semantics.
