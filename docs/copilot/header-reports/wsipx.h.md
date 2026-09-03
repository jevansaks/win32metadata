# Header Report: wsipx.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Declares only the `sockaddr_ipx`/`SOCKADDR_IPX` data struct and `NSPROTO_*` protocol constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants only, no functions).
