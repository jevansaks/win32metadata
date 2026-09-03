# dxcore_interface.h — accepted-normalized

**Partitions:** Debug, DXCore
**Size:** ~14943 bytes

## Analysis

`dxcore_interface.h` declares a C++ `enum class DXCoreAdapterProperty`
plus (further in the file) COM-style interfaces for DXCore adapter
enumeration. It contains **no `extern "C"`/`STDAPI` free function
declarations** — `DXCoreCreateAdapterFactory` (the actual entry point)
is declared in a separate header (`dxcore.h`), not here.

## Conclusion

Clean. No scrapable functions in this header. No patch required.
