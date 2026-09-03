# Header Report: mediaerr.h

## Partitions
`Media.DShow`, `Media.DxMediaObjects`

## Scrape validation
- `Media.DShow` partition previously re-scraped this session (batch `scraping-investigation-19`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only `DMO_E_*` HRESULT error-code constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error constants only, no functions).
