# Header Report: dragdropinterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interop interface `IDragDropManagerInterop` (derives `IInspectable`; `GetForWindow` is a standard `QueryInterface`-style COM output, `hwnd` is a pre-owned caller input). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interop interface method only, out of scope).
