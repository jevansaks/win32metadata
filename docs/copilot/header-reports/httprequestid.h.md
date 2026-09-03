# Header Report: httprequestid.h

## Partitions
`Iis`

## Scrape validation
- Re-scraped `Iis` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only `DISPID_HTTPREQUEST_*` command-ID constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (DISPID constants only, no functions).
