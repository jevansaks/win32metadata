# Header Report: MrmResourceIndexer.h

## Partitions
`MenuRc`

## Ownership audit (producer-site-only policy)
- `MrmResourceIndexerHandle` (`struct MrmResourceIndexerHandle { PVOID handle; }`) is produced by-value via `_Inout_ struct MrmResourceIndexerHandle* indexer` (multiple `MrmCreateResourceIndexer*` variants), closed via `MrmDestroyIndexerAndMessages`. Although a genuine close function exists, the struct is a fully transparent (non-opaque) wrapper — its single `PVOID handle` field is directly visible/accessible — out of scope per the established transparent-struct-alloc/free-pairs precedent (the `autoTypes.json`/`NativeTypedefStructsCreator` `RAIIFree` mechanism targets `DECLARE_HANDLE`-style opaque types or native typedefs of pointer-to-incomplete-struct types, not by-value transparent wrapper structs).
- Remaining declarations are plain enums (`MrmPlatformVersion`, `MrmPackagingMode`, etc.).

## Conclusion
`accepted-normalized` — no ownership annotation required (transparent wrapper struct, not an opaque handle type representable by the annotation mechanism).
