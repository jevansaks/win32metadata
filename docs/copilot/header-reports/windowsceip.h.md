# Header Report: windowsceip.h

## Partitions
`Base`, `CEIP`, `FileHistory`

## Scrape validation
- Re-scraped `CEIP` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Single function: `BOOL WINAPI CeipIsOptedIn()`. No parameters, no `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved).
