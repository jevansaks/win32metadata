# Header Report: wcmapi.h

## Partitions
`Wcm`

## Ownership audit (producer-site-only policy)
- `WcmQueryProperty`'s `_Outptr_result_buffer_maybenull_ PBYTE* ppData` is a generic byte buffer, freed via `WcmFreeMemory(__deallocate(Mem) void* pMemory)` — a generic memory-allocation convention, not a distinctly-named opaque handle.
- `WcmGetProfileList`'s `WCM_PROFILE_INFO_LIST** ppProfileList` is a fully transparent (non-opaque) struct, also freed via the same generic `WcmFreeMemory` — out of scope per the transparent-struct-alloc/free-pairs precedent.

## Conclusion
`accepted-normalized` — no ownership annotation required (generic memory-allocation convention / transparent struct, not a distinctly-named opaque handle).
