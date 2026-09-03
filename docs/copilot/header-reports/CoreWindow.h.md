# CoreWindow.h — accepted-normalized

**Partition:** WinRT
**Size:** ~26857 bytes

## Analysis

MIDL-generated WinRT COM header (`ICoreWindow*` interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
