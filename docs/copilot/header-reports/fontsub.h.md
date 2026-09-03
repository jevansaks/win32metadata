# Header Report: fontsub.h

## Partitions
`Gdi`

## Ownership audit (producer-site-only policy)
- `CreateFontPackage`/`MergeFontPackage` allocate their output buffers (`ppuchFontPackageBuffer`) via caller-supplied `CFP_ALLOCPROC`/`CFP_REALLOCPROC`/`CFP_FREEPROC` callback functions — the caller controls and frees the buffer directly with its own allocator, not a Win32 opaque handle or fixed system API. Out of scope.

## Conclusion
`accepted-normalized` — no ownership annotation required (buffer allocation delegated entirely to caller-supplied callbacks, no opaque handle).
