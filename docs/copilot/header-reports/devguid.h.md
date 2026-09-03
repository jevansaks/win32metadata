# devguid.h — accepted-normalized

**Partition:** DevInst
**Size:** ~14479 bytes

## Analysis

`devguid.h` (`shared\devguid.h`) declares only `DEFINE_GUID`-based
Plug & Play device-class GUID constants (`GUID_DEVCLASS_*`). It
contains **no function declarations, no structs, no handle
typedefs**.

## Conclusion

Clean. No patch required.
