# Header Report: inkrenderer.h

## Partitions
`Input_Ink`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interfaces `IInkD2DRenderer`/`IInkD2DRenderer2` plus an enum (`INK_HIGH_CONTRAST_ADJUSTMENT`). No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interfaces + enum only, no extern functions).
