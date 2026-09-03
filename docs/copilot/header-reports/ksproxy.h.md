# ksproxy.h — accepted-normalized

**Partition:** Media.KernelStreaming
**Size:** ~23649 bytes

## Analysis

`ksproxy.h` (WDM-CSA proxy filter interface definitions) declares
`IKsObject`/`IKsPin`/`IKsPinEx` COM interfaces. It contains **no free
functions** (`KSDDKAPI`/`STDAPI` count: 0) — all methods are COM
vtable methods, out of scope per the COM-vtable-methods convention
(blocker-class 5).

## Conclusion

Clean. No patch required.
