# activprof.h — accepted-normalized

**Partition:** Debug.ActiveScript
**Size:** ~65024 bytes

## Analysis

MIDL-generated COM header (Active Scripting Profiling interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
