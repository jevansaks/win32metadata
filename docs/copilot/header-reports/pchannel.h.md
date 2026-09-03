# Header Report: pchannel.h

## Partitions
`TermServ`

## Ownership audit (producer-site-only policy)
- Pure Terminal Server Virtual Channel protocol `#define` constants and transparent PDU-header data structs. No functions (the actual `VirtualChannelOpen`/`VirtualChannelOpenEventFn` callback functions are declared in a separate header).

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/transparent structs only, no functions).
