# Header Report: wlclient.h

## Partitions
`NWifi`

## Scrape validation
- Re-scraped `NWifi` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only data structs (`DOT11_ADAPTER`, `DOT11_BSS_LIST`, `DOT11_PORT_STATE`, `DOT11_SECURITY_PACKET_HEADER`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs only, no functions).
