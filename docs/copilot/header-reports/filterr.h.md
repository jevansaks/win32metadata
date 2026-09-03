# Header Report: filterr.h

## Partitions
`IndexSrv`

## Scrape validation
- Re-scraped `IndexSrv` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `FILTER_*`/`FACILITY_*`/`STATUS_SEVERITY_*` HRESULT error-code constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error-code constants only, no functions).
