# Header Report: ddkernel.h

## Partitions
`DirectDraw`

## Ownership audit (producer-site-only policy)
- Declares kernel-mode DirectDraw COM-style interfaces (`IDirectDrawKernel`, `IDirectDrawSurfaceKernel`) plus GUID constants. No `extern`/`DllImport` functions — pure interface/struct forward declarations.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM-style interfaces + GUID constants only, no extern functions).
