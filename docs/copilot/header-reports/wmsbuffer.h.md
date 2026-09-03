# wmsbuffer.h — accepted-normalized

**Partition:** WmFormat
**Size:** ~29868 bytes

## Analysis

MIDL-generated COM header (Windows Media Format SDK buffer
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
