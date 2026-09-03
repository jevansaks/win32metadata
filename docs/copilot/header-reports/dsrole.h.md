# Header Report: dsrole.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- `DsRoleGetPrimaryDomainInformation(..., OUT PBYTE *Buffer)`/`DsRoleFreeMemory(IN PVOID Buffer)` form a generic memory-buffer allocation pair (analogous to `NetApiBufferAllocate`/`NetApiBufferFree`, `dsrole.h`'s own equivalent convention), not an opaque `HANDLE`. Enums/structs (`DSROLE_MACHINE_ROLE`, `DSROLE_PRIMARY_DOMAIN_INFO_BASIC`, etc.) are plain data types.

## Conclusion
`accepted-normalized` — no ownership annotation required (generic memory-buffer allocation, not HANDLE-family).
