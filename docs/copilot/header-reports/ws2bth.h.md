# ws2bth.h — accepted-normalized

**Partition:** Bluetooth
**Size:** ~12134 bytes

## Analysis

`ws2bth.h` (Winsock 2 Bluetooth Annex) declares only protocol
constants (`AF_BTH`, `BT_PORT_*`), a `DEFINE_GUID` service-class GUID,
and transparent address structs (`SOCKADDR_BTH`). It contains **no
function declarations** — Bluetooth sockets are used via the standard
Winsock API (`socket`, `bind`, `connect`, tracked elsewhere), not via
any function declared in this header.

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
