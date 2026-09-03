# Header Report: schedule.h

## Partitions
`ActiveDirectory`

## Scrape validation
- Re-scraped `ActiveDirectory` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `sockaddr`/`timeval`/`_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only `SCHEDULE_*` constants and the `_SCHEDULE_HEADER`/`_SCHEDULE` data structs. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants + data structs only, no functions).
