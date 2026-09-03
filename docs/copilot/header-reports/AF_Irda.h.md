# Header Report: AF_Irda.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Pure `#define` socket-address-family/option constants (`AF_IRDA`, `IRLMP_*`) for the IrDA transport. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
