# Header Report: wsrm.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Pure `#define` socket-option constants for the PGM Reliable Multicast Transport (`RM_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
