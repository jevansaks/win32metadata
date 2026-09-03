# Header Report: d2d1effects_1.h

## Partitions
`Direct2D`

## Ownership audit (producer-site-only policy)
- Declares only a `DEFINE_GUID(CLSID_D2D1YCbCr, ...)` constant and enums (`D2D1_YCBCR_PROP`, `D2D1_YCBCR_CHROMA_SUBSAMPLING`, `D2D1_YCBCR_INTERPOLATION_MODE`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constant + enums only, no functions).
