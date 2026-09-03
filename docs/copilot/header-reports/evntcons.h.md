# evntcons.h — accepted-normalized

**Partition:** Etw
**Size:** ~11644 bytes

## Analysis

`evntcons.h` declares the ETW event-consumer security API:
`EventAccessControl`, `EventAccessQuery`, `EventAccessRemove`. Outputs
are `PSECURITY_DESCRIPTOR Buffer` — a caller-allocated buffer filled
in-place (not an opaque RAII handle), and plain `PULONG`/`ULONG`
scalars. `OpenTrace` is referenced only in a struct-field comment
(`UserContext`); the real `OpenTrace`/`CloseTrace`/`ProcessTrace`
functions and `TRACEHANDLE` type are declared in the separate
`evntrace.h` header, tracked independently.

## Conclusion

Clean. No opaque handle production in this header. No patch required.
