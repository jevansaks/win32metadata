# Header Report: delayloadhandler.h

## Partitions
`WinProg`

## Scrape validation
- `WinProg` partition previously re-scraped this session (batch `scraping-investigation-23`); result `0 Error(s)` (1 pre-existing unrelated cross-partition remap warning).

## Ownership audit (producer-site-only policy)
- Declares only data structs (`DELAYLOAD_PROC_DESCRIPTOR`, `DELAYLOAD_INFO`), a callback function-pointer typedef (`PDELAYLOAD_FAILURE_DLL_CALLBACK`), a constant, and an `extern` global variable. No `DllImport` functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (structs/callback-typedef/extern-variable only, no functions).
