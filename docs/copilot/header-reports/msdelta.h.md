# msdelta.h — accepted-normalized

**Partition:** Setup
**Size:** ~27105 bytes

## Analysis

`msdelta.h` (Delta Compression Engine API) declares free functions
(`CreateDeltaB`, `ApplyDeltaB`, `GetDeltaInfoB`, `GetDeltaSignatureB`,
etc.) that populate caller-supplied `LPDELTA_OUTPUT` output
parameters. Per doc comments, the resultant `DELTA_OUTPUT.lpStart`
memory block is "Caller `DeltaFree`" — i.e. ownership is transferred
via a raw `LPVOID` field embedded inside a **transparent struct**
(`DELTA_OUTPUT`), freed with the dedicated `DeltaFree` function.

This matches the established **Net API buffer-allocation convention**
blocker class (blocker-class 7): a raw pointer field embedded in a
transparent struct, not a distinct opaque handle typedef. The
producer-site ownership metadata system (`autoTypes.json`
`CloseApi`/`InvalidHandleValues`) annotates opaque handle *typedefs*
only — it has no mechanism to annotate a raw-pointer *struct field*.
`DELTA_OUTPUT`/`LPDELTA_OUTPUT` are themselves plain structs/pointers,
not owned-resource typedefs, so there is no typedef to (mis)annotate
and no representable producer-site fix here. Out of scope, consistent
with the `NetApiBufferAllocate`/`NetApiBufferFree` and other
struct-embedded-buffer precedents.

## Conclusion

Clean (no ownership-metadata gap representable at the typedef level).
No patch required.
