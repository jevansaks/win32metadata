# Header Report: loadperf.h

## Partitions
`FileHistory`, `Perf`, `WinProg`

## Scrape validation
- Re-scraped `Perf` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `InstallPerfDll(A/W)`/`LoadPerfCounterTextStrings(A/W)`/`UnloadPerfCounterTextStrings(A/W)`/`UpdatePerfNameFiles(A/W)`/`SetServiceAsTrusted(A/W)`/`BackupPerfRegistryToFileW`/`RestorePerfRegistryFromFileW` all take only strings and `ULONG_PTR` flags as parameters. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (string/flag-only API, no handle).
