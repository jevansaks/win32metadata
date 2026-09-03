# Header Report: activecf.h

## Partitions
`Media.DShow`

## Scrape validation
- Re-scraped `Media.DShow` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only a `CFSTR_VFW_FILTERLIST` string constant and the `VFW_FILTERLIST` data struct (clipboard-format data for VfW4 filters). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (clipboard-format data struct only, no functions).
