# rrascfg.h — accepted-normalized

**Partition:** Eap
**Size:** ~52040 bytes

## Analysis

MIDL-generated COM header (RRAS configuration `IRouterInfo`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
