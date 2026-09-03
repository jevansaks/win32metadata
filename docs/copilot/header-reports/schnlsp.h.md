# Header Report: schnlsp.h

## Partitions
`Identity`

## Scrape validation
- Re-scraped `Identity` partition (`ScanArch=x86`) earlier this session (batch `scraping-investigation-16`); result `0 Error(s)` (3 pre-existing cross-partition remap warnings unrelated to this header's content).

## Ownership audit (producer-site-only policy)
- `schnlsp.h` is a redirect-only header (`#include <schannel.h>`); declares nothing itself. `schannel.h` is already `accepted-normalized` in the ledger.

## Conclusion
`accepted-normalized` — redirect-only header; underlying content (`schannel.h`) already classified clean.
