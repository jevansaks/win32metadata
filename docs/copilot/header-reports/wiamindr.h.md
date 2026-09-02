# Header Report: wiamindr.h

## Partitions
`Wia`

## Scrape validation
- Re-scraped `Wia` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `wiamindr.h` is a redirect-only header: under `WINAPI_PARTITION_DESKTOP`, depending on `NTDDI_VERSION`, it `#include`s either `wiamindr_lh.h` (Vista+) or `wiamindr_xp.h`; it declares nothing itself.
- `wiamindr_lh.h` is still `pending` in the ledger (separate follow-up item); `wiamindr_xp.h` is not tracked (pre-Vista legacy path, not reachable under the SDK's default `NTDDI_VERSION`).

## Conclusion
`accepted-normalized` — redirect-only header with no direct declarations. The actual audit responsibility for the included content is tracked separately under `wiamindr_lh.h` (still pending).
