# IPTypes.h — accepted-normalized

**Partition:** wnv
**Size:** ~14948 bytes

## Analysis

`iptypes.h` declares transparent structs used by
`GetNetworkParams`/`GetAdaptersInfo` (declared elsewhere, e.g.
`IPHLPAPI.h`) — `FIXED_INFO`, `IP_ADDR_STRING`, etc. It contains **no
function declarations** (0 `WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
