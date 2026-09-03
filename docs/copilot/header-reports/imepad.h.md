# imepad.h — accepted-normalized

**Partition:** Input.Ime
**Size:** ~27707 bytes

## Analysis

MIDL-style COM header (`IImePadApplet` and related interfaces), using
classic `STDMETHOD`/`THIS_`/`PURE` macro style rather than
auto-generated MIDL boilerplate. Confirmed via grep that there are
**no free function declarations** — all methods are COM vtable
methods, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
