# Header Report: afunix.h

## Partitions
`WinSock`

## Scrape validation
- `WinSock` partition previously re-scraped this session (batch `scraping-investigation-16`); result `0 Error(s)` (pre-existing unrelated cross-partition remap warnings for `sockaddr`/`timeval`/`_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only `SOCKADDR_UN`/`PSOCKADDR_UN` (a plain data struct) and `SIO_AF_UNIX_*` IOCTL constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + IOCTL constants only, no functions).
