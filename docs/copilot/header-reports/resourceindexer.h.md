# resourceindexer.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`CreateResourceIndexer(..., _Outptr_ PVOID* ppResourceIndexer)` /
`DestroyResourceIndexer(PVOID)` form a genuine ownership pair via a direct
out-param.

## Correction to prior investigation
Prior report concluded that annotating a bare `PVOID` out-param "would
incorrectly apply RAIIFree to every void* in the published metadata" and
required "introducing a new named handle typedef first." This is incorrect:
the `emitter.settings.rsp` `Function::Parameter=[RAIIFree(...)]` mechanism is
scoped to the specific function and parameter name, not to the `PVOID` type
globally - precedent already exists for this exact shape, e.g.
`DnsAcquireContextHandle_A::pContext=[RAIIFree("DnsReleaseContextHandle")]`
(`pContext` is also a generic pointer-out-param, not a distinct handle
typedef).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/ResourceIndexer.h`:
```
STDAPI CreateResourceIndexer(
    _In_ PCWSTR projectRoot,
    _In_opt_ PCWSTR extensionDllPath,
    _Outptr_ PVOID* ppResourceIndexer
        _Win32_metadata_raii_free_(DestroyResourceIndexer));
```
No `#include <win32metadata_annotations.h>` guard was added: this header
already unconditionally includes `<windows.h>`, which transitively pulls in
`WinBase.h` (carries the guard), so the macro is available transitively. The
former sidecar entry
`CreateResourceIndexer::ppResourceIndexer=[RAIIFree("DestroyResourceIndexer")]`
was removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/ResourceIndexer.h.metadata.patch`
against the pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `ResourceIndexer.h.metadata.patch` against
  the `d154186c` baseline reproduces the current committed header
  byte-for-byte.
- ScrapeHeaders (MenuRc, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s) (scraped per-arch as x64/x86/arm64 automatically under
  crossarch).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
