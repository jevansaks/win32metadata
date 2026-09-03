# objsel.h — accepted-normalized

**Partition:** ActiveDirectory
**Size:** ~20490 bytes

## Analysis

`objsel.h` (Object Picker Dialog) declares GUID constants
(`CLSID_DsObjectPicker`) and the `IDsObjectPicker` COM interface. It
contains **no free functions** (`STDAPI` count: 0) — all methods are
COM vtable methods, out of scope per the COM-vtable-methods
convention (blocker-class 5).

## Conclusion

Clean. No patch required.
