# Header Report: rometadata.h

## Partitions
`WinRT.Metadata`

## Scrape validation
- Re-scraped `WinRT.Metadata` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Single function: `MetaDataGetDispenser(REFCLSID rclsid, REFIID riid, LPVOID FAR* ppv)` — a classic COM `QueryInterface`-style factory, output via `_Outptr_ void** ppv` interpreted per the caller-supplied `riid`. Consistent with the clean COM-factory pattern already established this session (e.g. `GameInputCreate`, `MetaDataGetDispenser` itself confirmed scraped with `_Outptr_` SAL on `ppv`). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM QueryInterface-style factory pattern).
