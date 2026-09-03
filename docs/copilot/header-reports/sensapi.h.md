# Header Report: sensapi.h

## Partitions
`Sens`

## Scrape validation
- Re-scraped `Sens` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Three functions: `IsDestinationReachableA`/`IsDestinationReachableW(LPCSTR/LPCWSTR, LPQOCINFO)`, `IsNetworkAlive(LPDWORD)`. All outputs are plain struct/`DWORD` values (`QOCINFO`). No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved).
