# netlistmgr.h — accepted-normalized

**Partition:** Nla, WindowsConnectionManager
**Size:** ~101765 bytes

## Analysis

MIDL-generated COM header (Network List Manager `INetworkListManager`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
