# Header Report: dcomptypes.h

## Partitions
`DirectComp`

## Ownership audit (producer-site-only policy)
- Declares only enums (`DCOMPOSITION_*`, `COMPOSITION_FRAME_ID_TYPE`) and plain data structs (`DCOMPOSITION_FRAME_STATISTICS`, `COMPOSITION_FRAME_STATS`, `COMPOSITION_TARGET_ID`, `COMPOSITION_STATS`, `COMPOSITION_TARGET_STATS`) plus `#define` access-flag constants. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/structs/constants only, no functions).
