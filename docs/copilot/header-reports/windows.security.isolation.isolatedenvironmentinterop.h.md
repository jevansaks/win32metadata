# Header Report: windows.security.isolation.isolatedenvironmentinterop.h

## Partitions
`WinRT.Isolation`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IIsolatedEnvironmentInterop` (single method `GetHostHwndInterop(_In_ HWND containerHwnd, _Out_retval_ HWND *hostHwnd)`). This returns a *reference* to an existing host window (`HWND`), not a newly-created, disposable resource requiring release — `HWND` values are not closed/freed by callers in this pattern (unlike `CloseHandle`-style ownership). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method returning a non-owned HWND reference, out of scope).
