# ftpext.h — accepted-normalized

**Partition:** Iis
**Size:** ~57375 bytes

## Analysis

MIDL-generated COM header (IIS FTP extension interfaces). Confirmed
via multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
