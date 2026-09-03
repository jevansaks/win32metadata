# Header Report: ElsCore.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- `MappingGetServices(..., _Out_ PMAPPING_SERVICE_INFO *prgServices, ...)` outputs an array of the fully transparent `MAPPING_SERVICE_INFO` struct, freed via `MappingFreeServices(_In_ PMAPPING_SERVICE_INFO pServiceInfo)` — out of scope per the transparent-struct-alloc/free-pairs precedent (structurally visible fields, not an opaque handle).
- `MappingRecognizeText`/`MappingDoAction` populate a caller-supplied `PMAPPING_PROPERTY_BAG` in place (`_Inout_`), freed via `MappingFreePropertyBag(_In_ PMAPPING_PROPERTY_BAG pBag)` — same transparent-struct pattern.

## Conclusion
`accepted-normalized` — no ownership annotation required (all producer/consumer pairs operate on transparent structs, not opaque handles).
