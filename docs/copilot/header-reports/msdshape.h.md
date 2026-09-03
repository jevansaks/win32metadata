# Header Report: msdshape.h

## Partitions
`Search`

## Scrape validation
- Re-scraped `Search` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `extern const CLSID`/`char*` constants (`CLSID_DataShapeProvider`, `PROGID_DataShapeProvider`, etc.) and two data-only enums (`MSDSDBINITPROPENUM`, `MSDSSESSIONPROPENUM`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/enums only, no functions).
