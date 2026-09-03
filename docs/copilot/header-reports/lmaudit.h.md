# lmaudit.h — accepted-normalized

**Partition:** NetMgmt
**Size:** ~11229 bytes

## Analysis

`lmaudit.h` declares the legacy `NetAudit*` API family
(`NetAuditClear`, `NetAuditRead`, `NetAuditWrite`). `NetAuditRead`
outputs `OUT LPBYTE *bufptr` — the canonical
`NetApiBufferAllocate`/`NetApiBufferFree` convention (blocker-class 7),
already established clean across many `lm*.h` headers. `HLOG` is a
transparent struct (`time`/`last_flags`/`offset`/`rec_offset`), not an
opaque handle.

## Conclusion

Clean. Net API buffer-allocation convention. No patch required.
