# Header Report: wsnetbs.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Declares only the `sockaddr_nb`/`SOCKADDR_NB` data struct, `NETBIOS_*` constants, and the `SET_NETBIOS_SOCKADDR` helper macro. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants/macro only, no functions).
