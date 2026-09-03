# Header Report: accountssettingspaneinterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only a COM/WinRT interop interface following the standard "GetForWindow" pattern (derives `IInspectable`, standard `QueryInterface`-style output, `HWND` pre-owned caller input). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interop interface method only, out of scope).
