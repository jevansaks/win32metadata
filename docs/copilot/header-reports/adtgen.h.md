# adtgen.h — accepted-normalized

**Partitions:** Authorization, Authorization.UI
**Size:** ~10775 bytes

## Analysis

`adtgen.h` (generic-audit-generation type definitions, shared with
`lsarpc.idl`) declares audit-parameter structs/enums plus a single
handle-family typedef:

```c
typedef
#ifdef MIDL_PASS
[context_handle]
#endif
PVOID AUDIT_HANDLE, *PAUDIT_HANDLE;
```

`AUDIT_HANDLE` is an RPC `[context_handle]` — a marshaled handle whose
producer/consumer RPC server functions are internal to the audit/LSA
subsystem and are **not present anywhere in the scraped SDK headers**
(confirmed via repo-wide grep: `AUDIT_HANDLE` appears only in this one
file, with zero function declarations consuming or producing it in
this header or any other tracked header). There is no
`extern`/`DllImport`-visible producer or consumer to annotate — the
type is declared but never used by any scrapable function signature.

No free functions of any kind are declared in this header (the
`AdtInitParams` helper mentioned in comments is not declared here).

## Conclusion

Clean. No representable ownership metadata gap — `AUDIT_HANDLE` has no
visible producer/consumer function in the scraped surface. No patch
required.
