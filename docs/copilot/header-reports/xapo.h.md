# xapo.h — accepted-normalized

**Partition:** Xaudio2
**Size:** ~35322 bytes

## Analysis

`xapo.h` (Cross-platform Audio Processing Object interfaces) declares
only COM interfaces (`IXAPO`, `IXAPOParameters`). Confirmed via grep
that there are **no free function declarations** — all methods are
COM vtable methods, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
