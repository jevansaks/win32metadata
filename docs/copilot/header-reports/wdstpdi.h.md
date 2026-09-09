# wdstpdi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
- `WdsTransportProviderCreateInstance(..., _Out_ PHANDLE phInstance)` produces
  a `HANDLE`, released via `WdsTransportProviderCloseInstance(HANDLE)`.
- `WdsTransportProviderOpenContent(..., _Out_ PHANDLE phContent)` produces a
  `HANDLE`, released via `WdsTransportProviderCloseContent(HANDLE)`.

## Correction to prior investigation
Prior report treated this as the unrepresentable "generic-type" blocker
class. Both pairs are fixable per-function.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/wdstpdi.h`:
```
WdsTransportProviderCreateInstance(
    _In_ PCWSTR pwszConfigString,
    _Out_ PHANDLE phInstance
        _Win32_metadata_raii_free_(WdsTransportProviderCloseInstance)
);
WdsTransportProviderOpenContent(
    _In_ HANDLE hInstance,
    _In_ PCWSTR pwszContentName,
    _Out_ PHANDLE phContent
        _Win32_metadata_raii_free_(WdsTransportProviderCloseContent)
);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). Both former
sidecar entries were removed from `emitter.settings.rsp`. Consolidated into
a single new `generation/WinSDK/patches/post-midl/wdstpdi.h.metadata.patch`
against the pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `wdstpdi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Wds, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
