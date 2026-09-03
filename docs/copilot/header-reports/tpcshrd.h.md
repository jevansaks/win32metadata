# Header Report: tpcshrd.h

## Partitions
`Tablet`

## Ownership audit (producer-site-only policy)
- Declares only `WM_TABLET_*`/`TABLET_*` message and flag constants, and typedefs/enums (`CURSOR_ID`, `SYSTEM_EVENT`, `TABLET_CONTEXT_ID`, `_PROPERTY_UNITS`). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/typedefs/enums only, no functions).
