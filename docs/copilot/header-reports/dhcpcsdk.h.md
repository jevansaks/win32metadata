# dhcpcsdk.h — accepted-normalized

**Partition:** Dhcp
**Size:** ~10605 bytes

## Analysis

`dhcpcsdk.h` declares DHCP option constants (`OPTION_*`) and
transparent parameter structs used by the DHCP client API:
`DHCPAPI_PARAMS`, `DHCPCAPI_PARAMS_ARRAY`, `DHCPCAPI_CLASSID`. It
contains **no function declarations** — the actual exported DHCP
client API functions (`DhcpCApiInitialize`, `DhcpRequestParams`, etc.)
live in a separate, distinct header (not this one).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
