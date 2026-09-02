# Header Report: gdiplus.h

## Partitions
`Gdiplus`

## Scrape validation
- Re-scraped `Gdiplus` partition (`ScanArch=x86`, required — `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 632 `DllImport` functions scraped, 1324 auto-remaps written.

## Ownership audit (producer-site-only policy)
- `gdiplus.h` itself is a thin umbrella header (`#include <gdiplus.h>` pulls in the GDI+ flat C API, physically declared in `gdiplusflat.h`).
- The GDI+ C API follows a create/delete-pair pattern over ~30 opaque C++ classes (`GpGraphics`, `GpBitmap`, `GpBrush`, `GpPen`, `GpFont`, `GpImage`, `GpMatrix`, `GpPath`, `GpRegion`, etc.), e.g. `GdipCreateFromHDC(HDC, GpGraphics**)` / `GdipDeleteGraphics(GpGraphics*)`.
- These are **strongly-typed C++ pointer types** (`GpGraphics*`), not `HANDLE`/`DECLARE_HANDLE`/`typedef PVOID` opaque scalar handles. Repo-wide precedent for `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` (searched across every header currently carrying these annotations: `bcrypt.h`, `amsi.h`, `AuthZ.h`, `sspi.h`, `tbs.h`, `ncrypt.h`, `ncryptprotect.h`, `NTSecAPI.h`, `NTSecPKG.h`, `prntvpt.h`, `securitybaseapi.h`, `wincrypt.h`) shows the mechanism is applied exclusively to `HANDLE`-family opaque scalar types, never to strongly-typed opaque C++ class pointers. `autoTypes.json` and `emitter.settings.rsp` contain no `Gp*`/`Gdiplus` entries, confirming there is no pre-existing ownership metadata for these types to migrate off a typedef.
- Strongly-typed pointers already carry unambiguous, self-documenting type information for consumers (unlike a generic `HANDLE`), and adding RAII-handle metadata to them is a distinct, out-of-scope enhancement not covered by this ledger's established policy or any prior patch in the repository.

## Conclusion
`accepted-normalized` — no ownership annotation required under current policy scope. GDI+'s create/delete object pattern uses strongly-typed C++ pointers, not `HANDLE`-family typedefs, and has no pre-existing RAII metadata to correct.
