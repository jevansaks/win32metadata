# fsrmscreen.h — accepted-normalized

**Partition:** Fsrm
**Size:** ~115256 bytes

## Analysis

MIDL-generated COM header (File Server Resource Manager screening
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
