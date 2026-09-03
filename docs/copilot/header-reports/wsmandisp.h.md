# wsmandisp.h — accepted-normalized

**Partition:** WinRm
**Size:** ~133749 bytes

## Analysis

MIDL-generated COM header (WSMan Automation/Dispatch interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
