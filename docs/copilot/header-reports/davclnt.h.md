# davclnt.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
- `DavAddConnection(_Inout_ HANDLE *ConnectionHandle, ...)` populates a
  `HANDLE`, released via `DavDeleteConnection(HANDLE)`.
- `DavRegisterAuthCallback` returns `OPAQUE_HANDLE` (`#define OPAQUE_HANDLE
  DWORD`) directly, released via `DavUnregisterAuthCallback(OPAQUE_HANDLE)`.

## Correction to prior investigation
Prior report treated both as the unrepresentable "generic-type" blocker
class. Both are fixable per-function; `OPAQUE_HANDLE` being a `DWORD` alias
does not prevent RAIIFree annotation (RAIIFree does not require a
pointer-shaped type - the attribute simply names the value's release API).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer declarations in
`RecompiledIdlHeaders/um/davclnt.h`:
```
DavAddConnection(
    _Inout_ HANDLE *ConnectionHandle
        _Win32_metadata_raii_free_(DavDeleteConnection),
    ...
    );

OPAQUE_HANDLE
WINAPI
DavRegisterAuthCallback(_In_ PFNDAVAUTHCALLBACK CallBack,
                        _In_ ULONG Version)
    _Win32_metadata_raii_free_(DavUnregisterAuthCallback);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). Both former
sidecar entries were removed from `emitter.settings.rsp`. Consolidated into a
single cumulative `generation/WinSDK/patches/post-midl/davclnt.h.metadata.patch`
against the pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `davclnt.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (WebDav, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
