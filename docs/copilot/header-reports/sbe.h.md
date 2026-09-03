# sbe.h — accepted-normalized

**Partition:** MsTv
**Size:** ~127777 bytes

## Analysis

MIDL-generated COM header (Simple Broadcast Encryption / BDA CAS
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
