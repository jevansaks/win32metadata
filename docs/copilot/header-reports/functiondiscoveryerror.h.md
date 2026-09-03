# Header Report: functiondiscoveryerror.h

## Partitions
`FunctionDiscovery`

## Scrape validation
- Re-scraped `FunctionDiscovery` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only `E_FDPAIRING_*` HRESULT error-code constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error-code constants only, no functions).
