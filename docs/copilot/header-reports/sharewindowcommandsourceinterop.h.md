# Header Report: sharewindowcommandsourceinterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interop interfaces `IShareWindowCommandEventArgsInterop` (`GetWindow` returns a reference to an existing `HWND`, not a newly-created disposable resource) and `IShareWindowCommandSourceInterop`. COM vtable methods — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interop interface methods only, out of scope).
