# Header Report: iswindowarranged.h

## Partitions
`MenuRc`

## Scrape validation
- Re-scraped `MenuRc` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Single function: `BOOL IsWindowArranged([in] HWND hwnd)`. Takes an existing `HWND` as input only — does not produce or release any handle.

## Conclusion
`accepted-normalized` — no ownership annotation required (query-only function, no handle production).
