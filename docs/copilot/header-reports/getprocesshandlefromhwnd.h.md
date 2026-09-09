# getprocesshandlefromhwnd.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`GetProcessHandleFromHwnd(HWND hwnd)` returns a process `HANDLE` directly as
the function return value, released via `CloseHandle`.

## Correction to prior investigation
The prior report (`scraping-investigation-14`) concluded this was unrepresentable,
citing a `WinmdUtils` dump search that found RAIIFree/InvalidHandleValue attributes
only on `struct` type declarations. That search missed the separate, already-shipped
`emitter.settings.rsp` `--memberRemap` mechanism, which applies `[RAIIFree(...)]`
directly to a specific function's return value or parameter (68 existing entries,
e.g. `WTSOpenServerA::return=[RAIIFree("WTSCloseServer")]`,
`FindFirstFileA::return=[RAIIFree("FindClose")]`). This mechanism is scoped to the
named function only - it does not assert ownership for `HANDLE` globally - so it is
exactly applicable here.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/getprocesshandlefromhwnd.h`:
```
HANDLE WINAPI GetProcessHandleFromHwnd(
  _In_ HWND hwnd
)
    _Win32_metadata_raii_free_(CloseHandle);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry `GetProcessHandleFromHwnd::return=[RAIIFree("CloseHandle")]` was
removed from `emitter.settings.rsp`. Consolidated into a single cumulative
`generation/WinSDK/patches/post-midl/getprocesshandlefromhwnd.h.metadata.patch`
against the pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `getprocesshandlefromhwnd.h.metadata.patch`
  against the `d154186c` baseline reproduces the current committed header
  byte-for-byte.
- ScrapeHeaders (Threading, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (__builtin_verbose_trap), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition ScrapeHeaders for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The mitter.settings.rsp syntax used matches 68 existing, already-shipped precedents exactly (e.g. WTSOpenServerA::return=[RAIIFree(...)], DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]).
