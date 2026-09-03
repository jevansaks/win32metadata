# Header Report: dxcore.h

## Partitions
`Debug`, `DXCore`

## Scrape validation
- Re-scraped `DXCore` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `DXCoreCreateAdapterFactory(REFIID riid, _COM_Outptr_ void** ppvFactory)` — a clean COM QueryInterface-style factory pattern, consistent with precedent established this session. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory pattern).
