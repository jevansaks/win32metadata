# Header Report: wpc.h

## Partitions
`Parcon`

## Scrape validation
- Re-scraped `Parcon` partition (`ScanArch=x86`, required — `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 16 `DllImport` functions, 1382 auto-remaps, 10 fn-ptr excludes written (consistent with prior batches; no new warnings).

## Ownership audit (producer-site-only policy)
- No `DECLARE_HANDLE` in `wpc.h`.
- All 16 scraped `DllImport` entries are MIDL-generated RPC marshalling helpers (`BSTR_UserSize`/`UserMarshal`/`UserUnmarshal`/`UserFree` and `HWND_UserSize`/`UserMarshal`/`UserUnmarshal`/`UserFree`, plus their `*64` variants) for the parental-controls COM interfaces (`IWindowsParentalControls*`). These are proxy/stub marshalling utilities, not resource producer/consumer APIs — no raw `HANDLE`-producing function exists in this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/BSTR-only API surface).
