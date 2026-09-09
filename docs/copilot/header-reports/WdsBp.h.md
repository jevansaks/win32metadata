# WdsBp.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`WdsBpParseInitialize`/`WdsBpParseInitializev6`/`WdsBpInitialize` each produce
a `HANDLE` via out-param `phHandle`, released via `WdsBpCloseHandle(HANDLE)`.

## Correction to prior investigation
Prior report treated this as the unrepresentable "generic-type" blocker
class. Fixable per-function.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/WdsBp.h`:
```
WdsBpParseInitialize (
    ...,
    _Out_ HANDLE *phHandle
        _Win32_metadata_raii_free_(WdsBpCloseHandle)
    );
WdsBpParseInitializev6 (
    ...,
    _Out_ HANDLE *phHandle
        _Win32_metadata_raii_free_(WdsBpCloseHandle)
    );
WdsBpInitialize (
    _In_ BYTE bPacketType,
    _Out_ HANDLE *phHandle
        _Win32_metadata_raii_free_(WdsBpCloseHandle)
    );
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). All three
former sidecar entries were removed from `emitter.settings.rsp`.
Consolidated into a single new
`generation/WinSDK/patches/post-midl/WdsBp.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `WdsBp.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Wds, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
