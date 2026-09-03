# Header Report: mpeg2bits.h

## Partitions
`MsTv`

## Scrape validation
- `MsTv` partition previously re-scraped this session (batch `scraping-investigation-24`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only bitfield data structs (`PID_BITS`, `MPEG_HEADER_BITS`, `MPEG_HEADER_VERSION_BITS`, and their `__midl` variants). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (bitfield data structs only, no functions).
