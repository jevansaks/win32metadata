# Header Report: winsdkver.h

## Partitions
`Foundation`

## Scrape validation
- Re-scraped `Foundation` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only SDK version-max constants (`_WIN32_MAXVER`, `NTDDI_MAXVER`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (version constants only, no functions).
