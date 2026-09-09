# winppi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`GdiGetSpoolFileHandle` returns a spool-file `HANDLE` directly, released via
`GdiDeleteSpoolFileHandle`.

`GdiGetPageHandle` also returns a `HANDLE` but (confirmed via Windows driver
documentation - "Using GDI Functions in Print Processors") this is a
**borrowed** page handle scoped to the current page/document, with cleanup
happening implicitly via `GdiEndPageEMF`/`GdiEndDocEMF`; there is no public
free API for it, so it is correctly left unannotated (not an ownership gap).

## Correction to prior investigation
Prior report treated both functions as the same unrepresentable blocker
class. Corrected: `GdiGetSpoolFileHandle` is a genuine, fixable
producer/consumer pair; `GdiGetPageHandle` is a borrowed handle and requires
no annotation at all (same treatment as `GetConsoleWindow` in wincon.h).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/winppi.h`:
```
HANDLE WINAPI GdiGetSpoolFileHandle(
    LPWSTR     pwszPrinterName,
    LPDEVMODEW pDevmode,
    LPWSTR     pwszDocName)
    _Win32_metadata_raii_free_(GdiDeleteSpoolFileHandle);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry
`GdiGetSpoolFileHandle::return=[RAIIFree("GdiDeleteSpoolFileHandle")]` was
removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/winppi.h.metadata.patch` against the
pristine `d154186c` SDK baseline. `GdiGetPageHandle` remains intentionally
unannotated (borrowed handle, no free API), unaffected by this change.

## Validation
- Patch replay: `git apply` of `winppi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Printing, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s) (scraped per-arch as x64/x86/arm64 automatically under
  crossarch).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
