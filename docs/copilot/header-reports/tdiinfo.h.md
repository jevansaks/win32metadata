# Header Report: tdiinfo.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
- Declares only plain data structs (`TDIEntityID`, `TDIObjectID`, etc.) and legacy CTE typedefs (`ulong`/`ushort`/`uchar`/`uint`) for extended TDI query/set-info calls. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (structs/typedefs only, no functions).
