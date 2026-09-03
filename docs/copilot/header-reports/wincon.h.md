# Header Report: wincon.h

## Partitions
`Console`

## Scrape validation
- Re-scraped `Console` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — ~120 `DllImport` functions scraped.

## Ownership audit (producer-site-only policy) — BLOCKED

`wincon.h` itself declares no functions directly — it is mostly a redirect (`#include <wincontypes.h>
<wingdi.h> <consoleapi.h> <consoleapi2.h> <consoleapi3.h>`) plus two pseudo-handle sentinel macros
(`CONSOLE_REAL_OUTPUT_HANDLE`/`CONSOLE_REAL_INPUT_HANDLE`, which are not real handles to close — they are
special values recognized by console APIs, analogous to `INVALID_HANDLE_VALUE`). None of
`wincontypes.h`/`consoleapi.h`/`consoleapi2.h`/`consoleapi3.h` are separately tracked in the ledger, so
(unlike `mtx.h`/`schnlsp.h`) the actual console API surface pulled in by this header has not been
audited elsewhere and had to be checked directly.

The live-scraped `Console` partition (~120 functions) contains a genuine, well-defined `HANDLE`
ownership relationship with **three producers returning the handle directly as the function return
value** (not via an out-param):
- `IntPtr CreateConsoleScreenBuffer(DWORD dwDesiredAccess, ...)` — creates a new console screen buffer.
- `IntPtr OpenConsoleW(LPWSTR lpConsoleDevice, ...)` — opens a handle to the console input/output.
- `IntPtr DuplicateConsoleHandle(HANDLE hSourceHandle, ...)` — duplicates an existing console handle.
- Consumer: `int CloseConsoleHandle(HANDLE handle)` — releases handles produced by the above.

This is the **same return-value-handle-ownership blocker class** already documented for
`getprocesshandlefromhwnd.h` (batch `scraping-investigation-14`) and `wab.h`
(`scraping-investigation-15`): confirmed via `WinmdUtils.exe dump` of the baseline `Windows.Win32.winmd`
that no function anywhere in the published metadata annotates a bare return-value `HANDLE` with
`RAIIFree`/`InvalidHandleValue` — every occurrence attaches to a `struct` type declaration only. There
is no precedent in this repository for expressing ownership on a direct function return value.

(Two other return-value `IntPtr` functions in this partition, `GetConsoleWindow()` and
`GetConsoleInputWaitHandle()`, are documented Microsoft APIs that return **borrowed** system handles —
callers must *not* close them — so they are correctly excluded from this ownership concern regardless
of the annotation-placement question.)

## Conclusion
`blocked` — genuine multi-producer/single-consumer `HANDLE` ownership relationship
(`CreateConsoleScreenBuffer`/`OpenConsoleW`/`DuplicateConsoleHandle` → `CloseConsoleHandle`), all via
return value rather than out-param. Same unresolved return-value-handle-ownership class as
`getprocesshandlefromhwnd.h`/`wab.h`; needs the same dedicated policy decision before this can be fixed.
