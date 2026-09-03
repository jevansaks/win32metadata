# Header Report: windowsdefender.h

## Partitions
`Lwef`

## Scrape validation
- Re-scraped `Lwef` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `WDStatus(_Out_ BOOL*)`/`WDEnable(_In_ BOOL)` — plain `BOOL` in/out only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (BOOL-only API, no handle).
