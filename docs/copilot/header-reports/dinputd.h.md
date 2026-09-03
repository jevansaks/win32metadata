# dinputd.h — accepted-normalized

**Partition:** Hid
**Size:** ~33146 bytes

## Analysis

`dinputd.h` (DirectInput device-driver-implementor header) declares
only COM interfaces (`IDirectInputEffectDriver`, etc.). Confirmed via
multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
