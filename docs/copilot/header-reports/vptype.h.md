# Header Report: vptype.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares only enums (`_AMVP_SELECT_FORMAT_BY`, `_AMVP_MODE`) and plain data structs (`_AMVPSIZE`, `_AMVPDIMINFO`, `_AMVPDATAINFO`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/data structs only, no functions).
