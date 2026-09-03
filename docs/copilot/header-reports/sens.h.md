# Header Report: sens.h

## Partitions
`Sens`

## Scrape validation
- `Sens` partition previously re-scraped this session (batch `scraping-investigation-28`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only `CONNECTION_*` constants and `SENSGUID_*` GUID constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
