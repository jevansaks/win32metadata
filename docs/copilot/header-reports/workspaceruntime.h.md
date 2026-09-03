# workspaceruntime.h — accepted-normalized

**Partition:** TermServ
**Size:** ~74902 bytes

## Analysis

MIDL-generated COM header (Workspace Runtime interfaces). Confirmed
via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
