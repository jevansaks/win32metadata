# d3dcommon.h — accepted-normalized

**Partition:** Direct2D, Direct3D, Direct3D10
**Size:** ~51756 bytes

## Analysis

MIDL-generated COM header (D3D common types/`ID3D10Blob`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
