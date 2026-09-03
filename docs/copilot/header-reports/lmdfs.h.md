# lmdfs.h — accepted-normalized

**Partition:** Dfs
**Size:** ~38237 bytes

## Analysis

`lmdfs.h` (NetDfs API) declares `NetDfsEnum`/`NetDfsGetInfo`-family
functions whose `_Out_ LPBYTE *Buffer` output is allocated by the API
and freed via the generic `NetApiBufferFree` (declared elsewhere) —
matches the established Net-API-buffer-allocation convention
(blocker-class 7), out of scope for typedef-level annotation (raw
`LPBYTE`/`LPBYTE*` buffer, not a distinct opaque handle typedef).

## Conclusion

Clean (no ownership-metadata gap representable at the typedef level).
No patch required.
