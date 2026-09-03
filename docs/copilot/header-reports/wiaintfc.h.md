# Header Report: wiaintfc.h

## Partitions
`Wia`

## Scrape validation
- `Wia` partition previously re-scraped this session (batch `scraping-investigation-15`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only a single `DEFINE_GUID(GUID_DEVINTERFACE_IMAGE, ...)` constant. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (device-interface GUID constant only).
