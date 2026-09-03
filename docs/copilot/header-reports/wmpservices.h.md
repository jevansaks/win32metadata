# wmpservices.h — accepted-normalized

**Partition:** Wmp
**Size:** ~29816 bytes

## Analysis

MIDL-generated COM header (`IWMPServices`, `IWMPPlugin`,
`IWMPGraphCreation`, etc. interfaces). Confirmed via grep that there
are **no free function declarations** — all methods are COM vtable
methods, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
