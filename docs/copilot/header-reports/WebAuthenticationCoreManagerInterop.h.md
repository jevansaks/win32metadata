# Header Report: WebAuthenticationCoreManagerInterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interop interface `IWebAuthenticationCoreManagerInterop` (derives `IInspectable`; `RequestTokenForWindowAsync` is a standard `QueryInterface`-style COM output, `appWindow`/`request` are pre-owned caller inputs). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interop interface method only, out of scope).
