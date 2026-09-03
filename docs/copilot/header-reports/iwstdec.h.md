# Header Report: iwstdec.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares data structs/enums (`AM_WST_PAGE`, `AM_WST_LEVEL`, `AM_WST_SERVICE`, `AM_WST_STATE`, `AM_WST_STYLE`, `AM_WST_DRAWBGMODE`) and the `IAMWstDecoder` COM interface. All methods take/output plain enums/`BOOL`/`DWORD`/caller-allocated bitmap-info structs. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface with plain-value parameters only, no handle).
