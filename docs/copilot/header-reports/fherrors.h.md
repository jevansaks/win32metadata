# Header Report: fherrors.h

## Partitions
`FileHistory`

## Ownership audit (producer-site-only policy)
- Pure HRESULT `#define` error codes (`FHCFG_E_*`/`FHSVC_E_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
