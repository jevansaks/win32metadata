# ntdsapi.h — accepted-normalized

**Partition:** ActiveDirectory
**Size:** ~118533 bytes

## Analysis

`ntdsapi.h` (Active Directory Domain Services API) declares
`DsBindW/A` (producers, `_Out_ HANDLE *phDS` — the generic `HANDLE`
type, not a distinct typedef) and `DsUnBindW/A(_In_ HANDLE *phDS)`
(destroyers — note the destroyer **also takes the handle by
pointer**, the same structural pattern identified as unrepresentable
in `rpcdce.h`/`usp10.h`). Doubly out of scope: generic `HANDLE`
(blocker-class 2) and by-pointer close signature.

## Conclusion

Clean. No patch required — generic `HANDLE` with by-pointer close
signature, consistent with established blocker precedents.
