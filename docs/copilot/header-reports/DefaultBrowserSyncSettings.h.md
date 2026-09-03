# Header Report: DefaultBrowserSyncSettings.h

## Partitions
`WinProg`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IDefaultBrowserSyncSettings` (single method `IsEnabled()` returning `BOOL`, no outputs) plus `CLSID_DefaultBrowserSyncSettings`/`LIBID_BrowserSyncSettings` constants and MIDL boilerplate. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID constants only, no extern functions).
