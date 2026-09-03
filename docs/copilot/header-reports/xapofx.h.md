# Header Report: xapofx.h

## Partitions
`Xaudio2`

## Ownership audit (producer-site-only policy)
- `CreateFX(REFCLSID clsid, _Outptr_ IUnknown** pEffect, ...)` outputs a standard COM interface pointer, out of scope. Remaining declarations are `CLSID_FX*` GUID constants and parameter-bound constants for the built-in Cross-platform Audio Processing Objects (XAPO).

## Conclusion
`accepted-normalized` — no ownership annotation required (standard COM factory function + GUID/parameter constants only).
