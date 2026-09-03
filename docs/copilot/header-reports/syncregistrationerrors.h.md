# Header Report: syncregistrationerrors.h

## Partitions
`WinSync`

## Scrape validation
- Re-scraped `WinSync` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `SYNC_E_REGISTRATION_*` HRESULT error-code constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error-code constants only, no functions).
