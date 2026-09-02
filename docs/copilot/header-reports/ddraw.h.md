# Header Report: ddraw.h

## Partitions
`DirectDraw`, `Gdiplus`, `Media.DShow`

## Scrape validation
- Re-scraped `Gdiplus` partition (`ScanArch=x86`, required — `Gdiplus` is `ExcludeFromCrossarch`) after touching `main.cpp` to invalidate the up-to-date marker.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `ddraw.h` conditionally guards `DECLARE_HANDLE(HMONITOR)` behind `#if !defined(HMONITOR_DECLARED) && (WINVER < 0x0500)`. Because `windows.fixed.h` is included before `ddraw.h` in every partition's `main.cpp`, `HMONITOR_DECLARED` is already defined by `winuser.h`, so this fallback typedef never actually fires in the scraped translation unit.
- `HMONITOR` in `ddraw.h` is used only as a plain input parameter to enumeration callback typedefs (`LPDDENUMCALLBACKEXA`/`LPDDENUMCALLBACKEXW`) — it is never produced or released by any DirectDraw function. `HMONITOR` values are queried (e.g. via `MonitorFromWindow`) elsewhere and are not a DirectDraw-owned resource, so no `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` annotation applies here.
- No other `DECLARE_HANDLE`/opaque-handle-producing pattern found in `ddraw.h` itself.

## Conclusion
`accepted-normalized` — no ownership annotation required. `HMONITOR` reference is a pass-through system handle, not a DirectDraw-owned resource.
