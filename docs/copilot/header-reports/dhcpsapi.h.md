# dhcpsapi.h — accepted-normalized

**Partition:** Dhcp
**Size:** ~128616 bytes

## Analysis

`dhcpsapi.h` (DHCP Server API) declares many `DhcpXxx` functions
whose allocated output buffers are freed via the generic
`DhcpRpcFreeMemory` — matches the established Net-API-buffer-
allocation convention (blocker-class 7), out of scope for typedef-
level annotation (raw buffer, not a distinct opaque handle typedef).

## Conclusion

Clean. No patch required.
