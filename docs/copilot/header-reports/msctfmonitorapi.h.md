# Header Report: msctfmonitorapi.h

## Partitions
`Tsf`

## Scrape validation
- Re-scraped `Tsf` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `DoMsCtfMonitor(DWORD dwFlags, HANDLE hEventForServiceStop)` takes `HANDLE` as a caller-supplied `_In_`-style parameter (an existing event handle used to signal service stop) — not produced here. `InitLocalMsCtfMonitor`/`UninitLocalMsCtfMonitor` take only `DWORD`. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (HANDLE is a caller-supplied input, not produced here).
