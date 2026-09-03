# uiribbon.h — accepted-normalized

**Partition:** WindowsRibbon
**Size:** ~60492 bytes

## Analysis

MIDL-generated COM header (Windows Ribbon `IUIFramework` interfaces).
Confirmed via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
