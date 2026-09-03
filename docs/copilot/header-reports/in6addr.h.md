# Header Report: in6addr.h

## Partitions
`WinSock`, `wnv`

## Scrape validation
- Re-scraped `wnv` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `sockaddr`/`timeval`/`_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only the `in6_addr`/`IN6_ADDR` data struct and `s6_addr`-family macros (RFC 2553 IPv6 address representation). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (plain data struct + macros only, no functions).
