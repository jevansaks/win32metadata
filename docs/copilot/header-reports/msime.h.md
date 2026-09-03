# msime.h — accepted-normalized

**Partition:** Input.Ime
**Size:** ~44294 bytes

## Analysis

`MSIME.H` (Japanese-specific IME COM interfaces: `IFECommon`,
`IFELanguage`, `IFEDictionary`, Per-IME interfaces) declares only COM
interfaces. Confirmed via grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
