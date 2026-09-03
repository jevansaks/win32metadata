# Header Report: wcnfunctiondiscoverykeys.h

## Partitions
`Wcn`

## Scrape validation
- `Wcn` partition previously re-scraped this session (batch `scraping-investigation-17`); result `0 Warning(s), 0 Error(s)` (0 `DllImport` functions in the entire partition).

## Ownership audit (producer-site-only policy)
- Declares only `DEFINE_GUID(SID_WcnProvider, ...)` and `DEFINE_PROPERTYKEY(PKEY_WCN_*, ...)` constants (via `PropKeyDef.h`, already `accepted-normalized`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/PROPERTYKEY constants only, no functions).
