# Header Report: wsvns.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Declares only the `sockaddr_vns`/`SOCKADDR_VNS` data struct and `VNSPROTO_*` constants (VINES IP socket addressing). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants only, no functions).
