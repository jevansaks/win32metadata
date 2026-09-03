# Header Report: hvsocket.h

## Partitions
`Hypervisor`

## Ownership audit (producer-site-only policy)
- Pure `#define` socket-option/GUID constants (`HVSOCKET_*`, `HV_GUID_*`) and plain data structs (`SOCKADDR_HV`, `HVSOCKET_ADDRESS_INFO`). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/structs only, no functions).
