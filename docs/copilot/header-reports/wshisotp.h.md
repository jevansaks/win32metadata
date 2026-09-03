# Header Report: wshisotp.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Declares only the `sockaddr_tp`/`SOCKADDR_TP` data struct, `ISOPROTO_*`/`ISO_*` protocol constants, and the `ISO_SET_TP_ADDR` helper macro. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants/macro only, no functions).
