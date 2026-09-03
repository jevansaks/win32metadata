# raseapif.h — accepted-normalized

**Partition:** Eap
**Size:** ~30284 bytes

## Analysis

`raseapif.h` declares the contract functions (`RasEapGetInfo`,
`RasEapInvokeInteractiveUI`, `RasEapInvokeConfigUI`,
`RasEapGetIdentity`) that **third-party EAP plugin DLLs must export**
(per doc comments: "should be exported by the 3rd party EAP dll
installed in the registry..."), plus `RasEapFreeMemory` to free their
raw `BYTE*`/`BYTE**` output buffers.

This is a raw-buffer allocation/free convention (`BYTE*` output
freed via a dedicated `RasEapFreeMemory` function) — not a distinct
opaque handle typedef. Matches the established Net-API-buffer-style
blocker class (blocker-class 7): out of scope for typedef-level
producer-site annotation, since there is no owned-resource typedef
to (mis)annotate, only raw pointer parameters.

## Conclusion

Clean (no ownership-metadata gap representable at the typedef level).
No patch required.
