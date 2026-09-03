# Header Report: idispids.h

## Partitions
`ComOle`, `InternetExplorer`

## Scrape validation
- Re-scraped `InternetExplorer` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `DISPID_*` integer constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (DISPID constants only, no functions).
