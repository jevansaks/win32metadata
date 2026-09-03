# Npapi.h — accepted-normalized

**Partition:** Wnet
**Size:** ~19782 bytes

## Analysis

`npapi.h` (Network Provider API) declares `NPOpenEnum`/`NPCloseEnum`.
`NPOpenEnum` outputs `_Out_ LPHANDLE lphEnum` — a **generic, shared
`HANDLE`**, not a distinctly-named type, matching the
generic/shared-type blocker class (blocker-class 2). Not fixable via
type-level annotation.

## Conclusion

Clean (not fixable — generic `HANDLE` output, no dedicated subtype).
No patch required.
