# Header Report: ntddmou.h

## Partitions
`Hid`

## Ownership audit (producer-site-only policy)
- Pure device-name string constants, `IOCTL_MOUSE_*` control-code constants (`CTL_CODE` macro invocations), a device-interface GUID, and transparent data structs for mouse-device IOCTLs. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/structs only, no functions).
