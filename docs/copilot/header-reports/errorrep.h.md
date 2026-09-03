# Header Report: errorrep.h

## Partitions
`Wer`

## Scrape validation
- Re-scraped `Wer` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `ReportFault`/`AddERExcludedApplicationA`/`AddERExcludedApplicationW`/`WerReportHang` all take caller-supplied inputs only (`LPEXCEPTION_POINTERS`, `LPCSTR`/`LPCWSTR`, `HWND hwndHungApp` as `_In_`). No `HANDLE` is produced or released.

## Conclusion
`accepted-normalized` — no ownership annotation required (HWND is a caller-supplied input, not produced here).
