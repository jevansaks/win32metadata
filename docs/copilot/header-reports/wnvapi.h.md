# wnvapi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`WnvOpen()` returns a generic `HANDLE` directly as the function return value.

## Correction to prior investigation
Prior evidence claimed the handle is "consumed by `WnvRequestNotification`"
and blocked as an unrepresentable return-value-HANDLE case. Corrected via web
search of Microsoft Learn documentation: `WnvRequestNotification` merely uses
the handle for an overlapped I/O request; the actual release API is the
standard `CloseHandle`, matching the documented WNV usage pattern (open with
`WnvOpen`, use for notifications, close with `CloseHandle`).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/wnvapi.h`:
```
HANDLE
WINAPI
WnvOpen(
    )
    _Win32_metadata_raii_free_(CloseHandle);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry `WnvOpen::return=[RAIIFree("CloseHandle")]` was removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/wnvapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `wnvapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (wnv, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
