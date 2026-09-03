# Header Report: wmsdkvalidate.h

## Partitions
`WmFormat`

## Scrape validation
- Re-scraped `WmFormat` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `WMCheckURLExtension`/`WMCheckURLScheme`/`WMValidateData`/`WMIsAvailableOffline` operate on strings, a caller-supplied `BYTE*` buffer, and `BOOL*` output only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (string/buffer/BOOL API only, no handle).
