# Header Report: dcommon.h

## Partitions
`Direct2D`, `Direct2D.Common`, `DirectWrite`

## Ownership audit (producer-site-only policy)
- Declares only enums (`DWRITE_MEASURING_MODE`, glyph-format flags) and a forward declaration of `IDXGISurface`. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/forward declaration only, no functions).
