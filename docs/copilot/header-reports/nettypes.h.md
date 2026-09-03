# Header Report: nettypes.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- Declares only typedefs/data structs (`OFFSET`, `FLAT_STRING`, `NETWORK_NAME`, `HARDWARE_ADDRESS`) and `NETMAN_VARTYPE_*` constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (typedefs/data structs/constants only, no functions).
