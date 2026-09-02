# Header Report: wia.h

## Partitions
`Wia`

## Scrape validation
- Re-scraped `Wia` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 185 `DllImport` functions, 1364 auto-remaps written.

## Ownership audit (producer-site-only policy)
- `wia.h` is a redirect-only header (`#include <wia_lh.h>` on Vista+, `<wia_xp.h>` pre-Vista); declares nothing itself.
- All 185 scraped functions in the `Wia` partition are MIDL-generated COM RPC proxy/stub marshalling helpers (`IWiaDevMgr_*_Proxy`/`_Stub`, `IEnumWIA_DEV_INFO_RemoteNext_Proxy`, etc.) for the WIA COM interfaces — not resource-owning functions.
- No `DECLARE_HANDLE` found in `wia_lh.h`.

## Conclusion
`accepted-normalized` — redirect-only header; underlying content is COM/RPC-marshalling-only, no ownership annotation required.
