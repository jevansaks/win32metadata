# rtutils.h — accepted-normalized

**Partition:** NetMgmt
**Size:** ~24292 bytes

## Analysis

`rtutils.h` (Router process utility/tracing functions) declares
`TraceRegisterExA`/`W`, returning a plain `DWORD` trace ID (matching
the `INVALID_TRACEID 0xFFFFFFFF` sentinel constant) — not a
distinctly-named handle type. Per the generic/shared-type blocker
class (blocker-class 2, extended to untyped scalar IDs), not fixable
via type-level annotation.

## Conclusion

Clean (not fixable — generic `DWORD` trace ID, no dedicated type). No
patch required.
