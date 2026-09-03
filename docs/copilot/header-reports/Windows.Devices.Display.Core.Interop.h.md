# Header Report: Windows.Devices.Display.Core.Interop.h

## Partitions
`WinRT.Display`

## Ownership audit (producer-site-only policy)
- Declares only the COM interfaces `IDisplayDeviceInterop` (`CreateSharedHandle` outputs a `HANDLE`) and `IDisplayPathInterop` (`CreateSourcePresentationHandle` outputs a `HANDLE`). Both are COM vtable methods — out of scope for the producer-site annotation mechanism regardless of the `HANDLE`-family output, per the established COM-vtable-methods precedent (no `extern "C"`/`DllImport` linkage to attach an annotation to).
- `Int64FromLuid`/`LuidFromInt64` are `inline` C++ helper functions, generating no bindings.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface methods + inline helpers only, out of scope).
