# ctfutb.h — accepted-normalized

**Partition:** Tsf
**Size:** ~92125 bytes

## Analysis

MIDL-generated COM header (Text Framework Services language-bar
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
