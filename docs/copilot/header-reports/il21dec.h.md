# Header Report: il21dec.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares enums (`_AM_LINE21_CCLEVEL`, `_AM_LINE21_CCSERVICE`, `_AM_LINE21_CCSTATE`, `_AM_LINE21_CCSTYLE`, `_AM_LINE21_DRAWBGMODE`) and the `IAMLine21Decoder` COM interface — all methods take/output plain enums/`BOOL`/`DWORD`/caller-allocated bitmap-info structs. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface with plain-value parameters only, no handle).
