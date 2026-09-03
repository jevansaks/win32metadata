# Header Report: colordlg.h

## Partitions
`DlgBox`

## Scrape validation
- Re-scraped `DlgBox` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only dialog control ID constants (`DLG_COLOR`, `COLOR_HUESCROLL`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (dialog control ID constants only, no functions).
