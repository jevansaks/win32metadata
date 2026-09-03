# Header Report: d2derr.h

## Partitions
`Direct2D`

## Ownership audit (producer-site-only policy)
- Declares only `D2DERR_*` HRESULT error-code macros (`MAKE_D2DHR`/`MAKE_D2DHR_ERR`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error-code macros only, no functions).
