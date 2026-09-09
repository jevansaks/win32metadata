# winconp.h — accepted-normalized

**Partition:** Console
**Size:** ~11750 bytes

## Analysis

`winconp.h` (internal console API, partially sourced from the Windows
Terminal repo) declares numerous internal console functions:
`OpenConsoleW`, `DuplicateConsoleHandle`, `CloseConsoleHandle`,
`VerifyConsoleIoHandle`, `GetConsoleInputWaitHandle`,
`RegisterConsoleVDM`, `InvalidateConsoleDIBits`, `SetConsoleFont`,
`GetConsoleFontInfo`, `SetConsoleCursor`, `ConsoleMenuControl`,
`SetConsolePalette`, `ReadConsoleInputEx*`, `WriteConsoleInputVDM*`,
`GetConsoleNlsMode`/`SetConsoleNlsMode`, `GetConsoleCharType`,
`RegisterConsoleIME`/`UnregisterConsoleIME`, etc.

All handle-producing functions (`OpenConsoleW`, `DuplicateConsoleHandle`,
`GetConsoleInputWaitHandle`) return/take the **generic, shared
`HANDLE` type** — no distinctly-named console-handle typedef exists in
this header (console handles are represented as plain `HANDLE`
throughout, closed via the internal `CloseConsoleHandle` or generic
`CloseHandle`).

`RegisterConsoleVDM`'s `_Outptr_ PVOID *lpState`/`_Outptr_ PVOID
*lpVDMBuffer` are untyped `PVOID` outputs — also generic, not a
dedicated type, out of scope for the same reason.

## Correction to prior investigation
This report previously concluded the header was "not fixable" because
`OpenConsoleW`/`DuplicateConsoleHandle` return the generic, shared `HANDLE`
type. That is incorrect: the `Function::return=[RAIIFree(...)]` mechanism is
scoped to the named function's return position only, not to `HANDLE`
globally (68+ existing precedents, e.g. `WTSOpenServerA::return`,
`FindFirstFileA::return`). Both functions were subsequently fixed via a
sidecar entry and are now migrated to inline annotations below.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer return declarations in
`RecompiledIdlHeaders/um/winconp.h`:
```
HANDLE
APIENTRY
OpenConsoleW(
    _In_ LPWSTR lpConsoleDevice,
    _In_ DWORD dwDesiredAccess,
    _In_ BOOL bInheritHandle,
    _In_ DWORD dwShareMode)
    _Win32_metadata_raii_free_(CloseConsoleHandle);

HANDLE
APIENTRY
DuplicateConsoleHandle(
    _In_ HANDLE hSourceHandle,
    _In_ DWORD dwDesiredAccess,
    _In_ BOOL bInheritHandle,
    _In_ DWORD dwOptions)
    _Win32_metadata_raii_free_(CloseConsoleHandle);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). Both former
sidecar entries were removed from `emitter.settings.rsp`. Consolidated into
a single new `generation/WinSDK/patches/post-midl/winconp.h.metadata.patch`
against the pristine `d154186c` SDK baseline. `RegisterConsoleVDM`'s bare
`PVOID` outputs remain out of scope (never had a sidecar entry - no
established closer function to name).

## Validation
- Patch replay: `git apply` of `winconp.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Console, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Conclusion

Producer-site inline annotations now cover `OpenConsoleW` and
`DuplicateConsoleHandle`; no sidecar `emitter.settings.rsp` entries remain
for this header. `RegisterConsoleVDM`'s untyped `PVOID` outputs remain a
narrower, genuine gap (no known single release function to name).
