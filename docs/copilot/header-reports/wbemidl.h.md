# Header Report: wbemidl.h

## Partitions
`Wmi`

## Scrape validation
- Re-scraped `Wmi` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 40 `DllImport` functions, 1375 auto-remaps written.

## Ownership audit (producer-site-only policy)
- `wbemidl.h` is a redirect-only header (`#include <wbemcli.h> <wbemprov.h> <wbemtran.h> <wbemdisp.h>`); declares nothing itself.
- No `DECLARE_HANDLE` found in any of the four sub-headers.
- All 40 scraped functions in the `Wmi` partition are MIDL-generated RPC marshalling stubs (`BSTR_User*`/`VARIANT_User*`, incl. `*64` variants) for the WMI (WBEM) COM interfaces — not resource-owning functions.

## Conclusion
`accepted-normalized` — redirect-only header; underlying content is COM/RPC-marshalling-only. `wbemcli.h`/`wbemprov.h`/`wbemtran.h`/`wbemdisp.h` remain separately tracked (still `pending`) for any future direct-declaration audit.
