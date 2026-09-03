# Header Report: wnvapi.h

## Partitions
`wnv`

## Scrape validation
- Re-scraped `wnv` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (pre-existing unrelated cross-partition remap warnings, not touched by this header).

## Ownership audit (producer-site-only policy) — BLOCKED (known blocker class)

- `HANDLE WINAPI WnvOpen()` returns a `HANDLE` **directly as the function return value** (not out-param), consumed by `WnvRequestNotification(HANDLE WnvHandle, ...)`.
- The type is the plain, generic system `HANDLE` (not a distinctly-named opaque handle). This is the same return-value-handle-ownership blocker class already documented for `getprocesshandlefromhwnd.h`/`wab.h`/`wincon.h`/`winppi.h`/`libloaderapi2.h`/`MSAJTransport.h`/`i_cryptasn1tls.h`.

## Conclusion
`blocked` — genuine `HANDLE` ownership relationship (`WnvOpen` → `WnvRequestNotification`) via return value; same already-documented blocker class, no new investigation required.
