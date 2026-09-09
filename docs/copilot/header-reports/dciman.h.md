# dciman.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`DCIOpenProvider()` returns `HDC` directly, released via
`DCICloseProvider(HDC)`. (The `HWINWATCH` gap in this same header -
`WinWatchOpen`/`WinWatchClose` - was already fixed in a prior batch via
`autoTypes.json`.)

## Correction to prior investigation
`HDC` has no single universal close API (context-dependent: `ReleaseDC`,
`DeleteDC`, `EndPaint`, etc.), so it correctly has no type-level
`autoTypes.json` `CloseApi`. But `DCIOpenProvider` specifically only ever
pairs with `DCICloseProvider` - a per-function annotation is exactly the
right tool here, not a type-level one, and does not conflict with any other
`HDC`-returning function.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/dciman.h`:
```
extern HDC WINAPI DCIOpenProvider(void)
    _Win32_metadata_raii_free_(DCICloseProvider);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header; the `HWINWATCH`
fix in the same header is a type-level `autoTypes.json` entry, not a header
patch). The former sidecar entry
`DCIOpenProvider::return=[RAIIFree("DCICloseProvider")]` was removed from
`emitter.settings.rsp`. Consolidated into a single cumulative
`generation/WinSDK/patches/post-midl/dciman.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `dciman.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (FileHistory, WinProg, `-p:ScanArch=crossarch`): Build
  succeeded, 0 Error(s) (WinProg scraped per-arch as x64/x86/arm64
  automatically under crossarch).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
