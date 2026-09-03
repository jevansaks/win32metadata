# Header Report: msacmdlg.h

## Partitions
`Multimedia`

## Scrape validation
- Re-scraped `Multimedia` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only dialog/control resource ID constants (`DLG_ACMFORMATCHOOSE_ID`, `IDD_ACMFORMATCHOOSE_*`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (dialog resource ID constants only, no functions).
