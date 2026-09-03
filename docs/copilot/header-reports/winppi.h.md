# Header Report: winppi.h

## Partitions
`Printing`

## Scrape validation
- Re-scraped `Printing` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy) — BLOCKED

This header contains a genuine, well-defined `HANDLE` ownership relationship over a "spool file handle":
- `HANDLE WINAPI GdiGetSpoolFileHandle(LPWSTR, LPDEVMODEW, LPWSTR)` — **produces** the handle, returned
  **directly as the function return value** (not via out-param).
- `BOOL WINAPI GdiDeleteSpoolFileHandle(HANDLE SpoolFileHandle)` — the corresponding release function.
- `HANDLE WINAPI GdiGetPageHandle(HANDLE SpoolFileHandle, DWORD Page, LPDWORD pdwPageType)` — a second
  return-value handle producer (a "page handle" derived from the spool file handle), with no visible
  corresponding release function in this header (its lifetime is presumably tied to the parent spool
  file handle).
- All other functions (`GdiGetPageCount`, `GdiGetDC`, `GdiStartDocEMF`, `GdiStartPageEMF`,
  `GdiPlayPageEMF`, `GdiEndPageEMF`, `GdiEndDocEMF`, `GdiGetDevmodeForPage`, `GdiResetDCEMF`) take the
  spool file `HANDLE` as an `_In_`-style consumer parameter only.

This is the **same return-value-handle-ownership blocker class** already documented for
`getprocesshandlefromhwnd.h` (batch `scraping-investigation-14`), `wab.h`
(`scraping-investigation-15`), and `wincon.h` (`scraping-investigation-22`): confirmed via
`WinmdUtils.exe dump` of the baseline `Windows.Win32.winmd` that no function anywhere in the published
metadata annotates a bare return-value `HANDLE` — every `RAIIFree`/`InvalidHandleValue` occurrence
attaches to a `struct` type declaration only. There is no precedent in this repository for expressing
ownership on a direct function return value.

## Conclusion
`blocked` — genuine `HANDLE`-producing functions (`GdiGetSpoolFileHandle`/`GdiGetPageHandle`), both via
return value rather than out-param, released via `GdiDeleteSpoolFileHandle`. Same unresolved
return-value-handle-ownership class as `getprocesshandlefromhwnd.h`/`wab.h`/`wincon.h`; needs the same
dedicated policy decision before this can be fixed.
