# Header Report: mfmp2dlna.h

## Partitions
`Mf`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IMFDLNASinkInit` (single method `Initialize(IMFByteStream*, BOOL)`, no outputs), `EXTERN_GUID` constants, and a transparent `MFMPEG2DLNASINKSTATS` struct. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID constants/struct only, no extern functions).
