# Header Report: jsrt.h

## Partitions
`Js`

## Scrape validation
- Re-scraped `Js` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `jsrt.h` is a redirect-only header: without `USE_EDGEMODE_JSRT` defined it `#include`s `jsrt9.h`; with it defined, `chakrart.h`. It declares nothing itself.
- Verified `Js/main.cpp` explicitly does **not** define `USE_EDGEMODE_JSRT` — it directly includes both `jsrt.h` and `jsrt9.h`, with `chakrart.h` explicitly commented out (`// Can't include this with the others`). So only the `jsrt9.h` branch is ever reached in this partition; `jsrt9.h` is already separately tracked in the ledger (still `pending`). `chakrart.h` is confirmed unreachable from this partition and is not itself tracked.

## Conclusion
`accepted-normalized` — redirect-only header; the only reachable branch (`jsrt9.h`) is already separately tracked (pending); the unreachable branch (`chakrart.h`) is out of scope for this partition.
