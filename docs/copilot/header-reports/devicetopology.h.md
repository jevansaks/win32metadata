# devicetopology.h — accepted-normalized

**Partition:** Audio
**Size:** ~125488 bytes

## Analysis

MIDL-generated COM header (`IDeviceTopology`/`IConnector` interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
