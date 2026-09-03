# Header Report: windows.ui.composition.interop.h

## Partitions
`WinRT.Composition`

## Ownership audit (producer-site-only policy)
- Declares only COM interop interfaces (`ICompositionDrawingSurfaceInterop`, `ICompositorInterop`, `ICompositionGraphicsDeviceInterop`, `ICompositorDesktopInterop`, etc.), all pure vtable methods (`IFACEMETHOD`/`DECLARE_INTERFACE_IID_`). `ICompositorInterop::CreateCompositionSurfaceForHandle(_In_ HANDLE swapChain, ...)` takes a `HANDLE` as a pre-owned caller input only. COM vtable methods — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interop interface methods only, out of scope).
