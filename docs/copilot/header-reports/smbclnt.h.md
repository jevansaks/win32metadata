# Header Report: smbclnt.h

## Partitions
`MsCs`

## Scrape validation
- Re-scraped `MsCs` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `RegisterAppInstance`/`SetAppInstanceCsvFlags` take `HANDLE ProcessHandle` as an `_In_` (caller-supplied, existing process handle) — not produced here. `RegisterAppInstanceVersion`/`QueryAppInstanceVersion`/`ResetAllAppInstanceVersions` take only `GUID*`/`UINT64*`/no parameters. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (HANDLE is a caller-supplied input, not produced here).
