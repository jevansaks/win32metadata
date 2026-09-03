# Header Report: wsbonline.h

## Partitions
`Wsb`

## Scrape validation
- Re-scraped `Wsb` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `RegisterOnlineBackupWithWindowsServerBackup`/`DeregisterOnlineBackupFromWindowsServerBackup`/`UpdateOBStatusInWindowsServerBackup` all take caller-supplied structs/GUID as `_In_` parameters only. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (struct/GUID input-only API, no handle).
