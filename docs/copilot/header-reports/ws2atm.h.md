# ws2atm.h — accepted-normalized

**Partition:** WinSock
**Size:** ~17108 bytes

## Analysis

`WS2atm.h` (Winsock 2 ATM Annex) declares only protocol constants and
transparent structs (`ATM_ADDRESS`, etc.) consumed via the standard
Winsock API (`socket`, `setsockopt`, tracked elsewhere). It contains
**no function declarations** (0 `WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
