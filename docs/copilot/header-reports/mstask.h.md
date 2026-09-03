# mstask.h — accepted-normalized

**Partition:** TaskSchd
**Size:** ~62714 bytes

## Analysis

MIDL-generated COM header (legacy Task Scheduler `ITask` interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
