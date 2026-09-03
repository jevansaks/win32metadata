# dhcpssdk.h — accepted-normalized

**Partition:** Dhcp
**Size:** ~16967 bytes

## Analysis

`dhcpssdk.h` (header for writing a DHCP Callout DLL) declares only
transparent structs (`DHCP_SERVER_OPTIONS`) consumed by
callout-DLL-implemented callback functions (implemented BY the
callout DLL, not imported from a system DLL). It contains **no
`extern`/`DllImport` function declarations** (0
`WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
