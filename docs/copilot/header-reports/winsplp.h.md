# winsplp.h — accepted-normalized (producer-site fix applied)

**Partition:** Printing
**Size:** ~64733 bytes

## Analysis

`WinSplp.h` ("Internal Header file for Print APIs") mixes print
processor/monitor **plugin-contract functions** (`InitializePrintProvidor`,
`OpenPrintProcessor`/`ClosePrintProcessor`, `OpenPort`/`ClosePort`,
`XcvOpenPort`/`XcvClosePort` — called by the spooler service, not
P/Invoked by applications; out of scope, no representable fix) with
genuine **application-callable Win32 APIs**: `CreatePrinterIC`
(returns generic `HANDLE` directly) and single-arg
`DeletePrinterIC(HANDLE hPrinterIC)`.

Checked `emitter.settings.rsp` first: **no existing inline
`RAIIFree` annotation for `CreatePrinterIC::return`** (only an
unrelated `CreatePrinterIC::hPrinter=PRINTER_HANDLE` input-parameter
annotation) — a genuine gap. Added
`CreatePrinterIC::return=[RAIIFree("DeletePrinterIC")]`, consistent
with the `HeapCreate`/`WTSOpenServer`/`QOSCreateHandle` precedent for
generic-`HANDLE`-returning functions.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/winsplp.h`:
```
HANDLE
CreatePrinterIC(
    _In_        HANDLE      hPrinter,
    _In_opt_    LPDEVMODEW  pDevMode
    )
    _Win32_metadata_raii_free_(DeletePrinterIC);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry `CreatePrinterIC::return=[RAIIFree("DeletePrinterIC")]` was
removed from `emitter.settings.rsp` (the unrelated
`CreatePrinterIC::hPrinter=PRINTER_HANDLE` type-override entry is untouched).
Consolidated into a single new
`generation/WinSDK/patches/post-midl/winsplp.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `winsplp.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Printing, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s) (scraped per-arch as x64/x86/arm64 automatically under
  crossarch).

## Conclusion

Producer-site fix applied — inline `_Win32_metadata_raii_free_` return-value
annotation now lives directly on `CreatePrinterIC` in
`RecompiledIdlHeaders/um/winsplp.h`; no sidecar `RAIIFree` entry remains. The
plugin-contract functions remain correctly out of scope (no representable
ownership pattern; called by the spooler, not exported system APIs).
