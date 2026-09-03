# wlanihv.h — accepted-normalized

**Partition:** NWifi
**Size:** ~22814 bytes

## Analysis

`wlanihv.h` (WLAN Extensibility Framework public APIs) declares only
a forward-declared struct typedef (`DOT11EXT_APIS`) used by IHV
DLL-implemented function tables. It contains **no `extern`/`DllImport`
function declarations** (0 `WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
