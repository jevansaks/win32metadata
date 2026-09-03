# Header Report: wlanihvtypes.h

## Partitions
`NWifi`

## Scrape validation
- `NWifi` partition previously re-scraped this session (batch `scraping-investigation-32`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only data structs (`DOT11_MSSECURITY_SETTINGS`, `DOT11EXT_IHV_SSID_LIST`, `DOT11EXT_IHV_PROFILE_PARAMS`, `DOT11EXT_IHV_PARAMS`) and `MS_*` constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs + constants only, no functions).
