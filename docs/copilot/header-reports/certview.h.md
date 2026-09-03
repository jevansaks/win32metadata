# certview.h — accepted-normalized

**Partition:** Certificates
**Size:** ~50503 bytes

## Analysis

MIDL-generated COM header (Certificate Services `ICertView`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
