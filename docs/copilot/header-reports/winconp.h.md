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
`CloseHandle`). Per the generic/shared-type blocker class
(blocker-class 2), annotating `HANDLE` itself would incorrectly apply
to every value of that type across the SDK.

`RegisterConsoleVDM`'s `_Outptr_ PVOID *lpState`/`_Outptr_ PVOID
*lpVDMBuffer` are untyped `PVOID` outputs — also generic, not a
dedicated type, out of scope for the same reason.

## Conclusion

Clean (not fixable — all handle-family outputs use the generic shared
`HANDLE`/`PVOID` types, no dedicated subtype to annotate). No patch
required.
