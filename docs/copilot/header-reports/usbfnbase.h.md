# usbfnbase.h — accepted-normalized

**Partition:** Buses
**Size:** ~10612 bytes

## Analysis

`usbfnbase.h` (shared\usbfnbase.h) defines USB Function (USBFN) class
driver registry paths, size limits, and transparent struct/enum
definitions used by kernel and user-mode USBFN class drivers. It
contains **no function declarations at all** — purely constants and
structs.

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
