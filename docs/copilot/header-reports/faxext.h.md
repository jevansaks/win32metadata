# faxext.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`FaxExtRegisterForEvents` returns a `HANDLE` directly, released via
`FaxExtUnregisterForEvents(HANDLE hNotification)`.

## Correction to prior investigation
Prior report treated this as an unrepresentable return-value-`HANDLE` case.
Fixable per-function, same as other headers in this batch.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/FaxExt.h`:
```
HANDLE
WINAPI
FaxExtRegisterForEvents (
    HINSTANCE                   hInst,
    DWORD                       dwDeviceId,
    FAX_ENUM_DEVICE_ID_SOURCE   DevIdSrc,
    LPCWSTR                     lpcwstrDataGUID,
    PFAX_EXT_CONFIG_CHANGE      lpConfigChangeCallback
)
    _Win32_metadata_raii_free_(FaxExtUnregisterForEvents);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry
`FaxExtRegisterForEvents::return=[RAIIFree("FaxExtUnregisterForEvents")]` was
removed from `emitter.settings.rsp`. Consolidated into a single cumulative
`generation/WinSDK/patches/post-midl/FaxExt.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `FaxExt.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Fax, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
