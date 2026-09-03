# Header Report: wiadevd.h

## Partitions
`Wia`

## Ownership audit (producer-site-only policy)
- `DeviceDialog(_In_ PDEVICEDIALOGDATA pDeviceDialogData)` is an extern function, but it only takes a struct pointer in — no opaque handle produced or returned.
- `IWiaUIExtension`/`IWiaUIExtension2` are COM interfaces; their `GetDeviceIcon(_Out_ HICON*)`/`GetDeviceBitmapLogo(_Out_ HBITMAP*)` are COM vtable methods, out of scope for the annotation mechanism (no `extern "C"`/`DllImport` linkage).
- `DeviceDialogFunction` is a function-pointer typedef (not an `extern` declaration), out of scope.

## Conclusion
`accepted-normalized` — no ownership annotation required (only extern function takes no opaque-handle output; remaining handle-producing methods are COM vtable methods, out of scope).
