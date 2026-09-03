# Header Report: PrintManagerInterop.h

## Partitions
`WinRT.Printing`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interop interface `IPrintManagerInterop` (derives `IInspectable`; `GetForWindow` is a standard `QueryInterface`-style COM output, `appWindow` is a pre-owned caller input). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interop interface method only, out of scope).
