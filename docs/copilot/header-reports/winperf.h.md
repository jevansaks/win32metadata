# winperf.h — accepted-normalized

**Partition:** Perf
**Size:** ~38242 bytes

## Analysis

`winperf.h` (Performance Monitor data) declares only transparent
structs (`PERF_DATA_BLOCK`, `PERF_OBJECT_TYPE`, etc.) returned via the
generic `RegQueryValueEx`/`HKEY_PERFORMANCE_DATA` mechanism (declared
elsewhere). It contains **no direct function declarations**.

## Conclusion

Clean. No patch required.
