# Header Report: inaddr.h

## Partitions
`IpHlp`, `RRas`, `WinSock`

## Ownership audit (producer-site-only policy)
- Declares only the `in_addr`/`IN_ADDR` data struct (IPv4 on-wire address) and `s_addr`-family macros. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (plain data struct + macros only, no functions).
