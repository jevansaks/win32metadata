# Header Report: usbscan.h

## Partitions
`Buses`

## Ownership audit (producer-site-only policy)
- Declares only IOCTL-parameter data structs (`DRV_VERSION`, `IO_BLOCK`, `IO_BLOCK_EX`, etc.) with SAL in/out annotations for `DeviceIoControl` buffer semantics. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (IOCTL parameter structs only, no functions).
