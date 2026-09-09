# wslapi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`WslLaunch(..., _Out_ HANDLE* process)` produces a process `HANDLE` via a
direct out-param, released via `CloseHandle` (standard process handle
lifecycle, matching `CreateProcess`'s `PROCESS_INFORMATION.hProcess`).

## Correction to prior investigation
Prior report treated this as the same unrepresentable "generic/shared-type"
blocker class as resourceindexer.h. Both are now fixed the same way: a
per-function parameter annotation does not require the parameter's type to be
distinctly named, only that the (function, parameter) pair is unique - which
it always is.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/wslapi.h`:
```
HRESULT
WslLaunch(
    _In_ PCWSTR distributionName,
    _In_opt_ PCWSTR command,
    _In_ BOOL useCurrentWorkingDirectory,
    _In_ HANDLE stdIn,
    _In_ HANDLE stdOut,
    _In_ HANDLE stdErr,
    _Out_ HANDLE* process
        _Win32_metadata_raii_free_(CloseHandle)
    );
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block right after the header's `<apiset.h>`/`<apisetcconv.h>`
includes (no prior patch existed for this header, and no unconditional
include already carried the guard). The former sidecar entry
`WslLaunch::process=[RAIIFree("CloseHandle")]` was removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/wslapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `wslapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Wsl, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
