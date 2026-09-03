# Header Report: cscapi.h

## Partitions
`Of`

## Scrape validation
- Re-scraped `Of` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `OfflineFilesEnable`/`OfflineFilesStart`/`OfflineFilesQueryStatus`/`OfflineFilesQueryStatusEx` all output plain `BOOL*` values only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (BOOL-output-only API, no handle).
