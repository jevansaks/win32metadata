# Header Report: faxmmc.h

## Partitions
`Fax`

## Scrape validation
- Re-scraped `Fax` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `FAXSRV_*_NODETYPE_GUID*`/`CF_MSFAXSRV_*` GUID and clipboard-format string constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/string constants only, no functions).
