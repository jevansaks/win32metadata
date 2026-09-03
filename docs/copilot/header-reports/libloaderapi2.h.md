# Header Report: libloaderapi2.h

## Partitions
`LibraryLoader`

## Scrape validation
- Re-scraped `LibraryLoader` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy) — BLOCKED

- `_Ret_maybenull_ HMODULE WINAPI LoadPackagedLibrary(_In_ LPCWSTR, _Reserved_ DWORD)` returns an
  `HMODULE` directly as the function return value (not via out-param), released via `FreeLibrary`
  (declared in the sibling header `libloaderapi.h`) — the same well-known `LoadLibrary`/`FreeLibrary`
  ownership pattern.
- This is the **same return-value-handle-ownership blocker class** already documented in depth for
  `getprocesshandlefromhwnd.h` (batch `scraping-investigation-14`), `wab.h` (`-15`), `wincon.h` (`-22`),
  and `winppi.h` (`-31`). Notably, `getprocesshandlefromhwnd.h`'s original investigation used
  `LoadLibraryExW`/`LoadLibraryExA` (the sibling of this exact `LoadPackagedLibrary` function, same
  `HMODULE`-return/`FreeLibrary`-release pattern) as the direct comparison point when confirming — via
  `WinmdUtils.exe dump` of the published baseline `Windows.Win32.winmd` — that **no** `HMODULE`-returning
  function anywhere in the current published metadata carries any `RAIIFree`/`InvalidHandleValue`
  annotation. This is not a newly-introduced gap; it is the ambient, pervasive, already-confirmed state
  of the entire `LoadLibrary`-family API surface.
- `QueryOptionalDelayLoadedAPI(_In_ HMODULE hParentModule, ...)` takes `HMODULE` only as an `_In_`
  consumer parameter — not a producer, no issue there.

## Conclusion
`blocked` — `LoadPackagedLibrary` is a genuine instance of the already-documented return-value-handle
blocker class (same root cause as `getprocesshandlefromhwnd.h`/`wab.h`/`wincon.h`/`winppi.h`). Recorded
for per-header traceability; no new investigation was required since the underlying policy gap and its
evidence are already fully documented under `getprocesshandlefromhwnd.h.md`.
