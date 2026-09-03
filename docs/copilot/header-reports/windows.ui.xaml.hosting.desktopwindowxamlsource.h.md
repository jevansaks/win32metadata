# Header Report: windows.ui.xaml.hosting.desktopwindowxamlsource.h

## Partitions
`WinRT.Xaml`

## Ownership audit (producer-site-only policy)
- Declares only the COM interfaces `IDesktopWindowXamlSourceNative`/`IDesktopWindowXamlSourceNative2`. `AttachToWindow` takes `HWND` as a pre-owned input; `get_WindowHandle` returns a reference to an existing `HWND` (not a newly-created disposable resource). COM vtable methods — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface methods only, out of scope).
