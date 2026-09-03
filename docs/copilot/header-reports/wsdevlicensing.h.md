# Header Report: wsdevlicensing.h

## Partitions
`DevLic`

## Scrape validation
- Re-scraped `DevLic` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Three functions: `CheckDeveloperLicense(_Out_ FILETIME*)`, `AcquireDeveloperLicense(_In_opt_ HWND, _Out_ FILETIME*)`, `RemoveDeveloperLicense(_In_opt_ HWND)`. The `HWND` parameters are caller-supplied parent-window inputs (`_In_opt_`), not produced here; outputs are plain `FILETIME` values. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (HWND is an input parameter, not produced/owned here; outputs are plain FILETIME values).
