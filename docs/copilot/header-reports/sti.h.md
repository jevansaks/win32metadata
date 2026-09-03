# sti.h — accepted-normalized

**Partition:** ImagingDevice
**Size:** ~27600 bytes

## Analysis

`sti.h` (Still Image APIs, "in COM format") is a COM-only header.
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
