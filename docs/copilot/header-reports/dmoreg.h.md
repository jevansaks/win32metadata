# Header Report: dmoreg.h

## Partitions
`Media.DxMediaObjects`

## Scrape validation
- Re-scraped `Media.DxMediaObjects` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `DMORegister`/`DMOUnregister`/`DMOGetTypes`/`DMOGetName` operate on `REFCLSID`/`DMO_PARTIAL_MEDIATYPE` data only — no opaque handles produced.
- `DMOEnum`'s `IEnumDMO **ppEnum` output is a standard COM interface pointer (released via `IUnknown::Release`), not a `HANDLE`-family value — out of scope for the `RAIIFree`/`InvalidHandleValue` annotation mechanism (COM interfaces use the standard QueryInterface/AddRef/Release convention already represented in metadata).

## Conclusion
`accepted-normalized` — no ownership annotation required (registration/enumeration functions operate on CLSIDs and standard COM interface pointers only, no opaque handles).
