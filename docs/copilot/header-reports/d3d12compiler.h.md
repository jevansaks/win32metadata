# d3d12compiler.h — accepted-normalized

**Partition:** Direct3D12
**Size:** ~38770 bytes

## Analysis

MIDL-generated COM header (D3D12 shader compiler interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
