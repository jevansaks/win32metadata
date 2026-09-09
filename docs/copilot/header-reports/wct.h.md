# wct.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`OpenThreadWaitChainSession` returns `HWCT` (typedef `LPVOID`) directly as the
function return value, released via `CloseThreadWaitChainSession`.

## Correction to prior investigation
Same corrected mechanism as getprocesshandlefromhwnd.h.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral annotation
directly on the producer declaration in `RecompiledIdlHeaders/um/wct.h`:
```
_Check_return_
WINADVAPI
HWCT WINAPI
OpenThreadWaitChainSession (
    _In_ DWORD Flags,
    _In_opt_ PWAITCHAINCALLBACK callback
    )
    _Win32_metadata_raii_free_(CloseThreadWaitChainSession);
```
Added the `#include <winapifamily.h>` + `#if defined(WIN32METADATA) #include
<win32metadata_annotations.h> #endif` guard block (wct.h had no prior patch and
did not otherwise pull in a header carrying the guard). The former sidecar
entry `OpenThreadWaitChainSession::return=[RAIIFree("CloseThreadWaitChainSession")]`
was removed from `emitter.settings.rsp`. Consolidated into a single cumulative
`generation/WinSDK/patches/post-midl/wct.h.metadata.patch` against the pristine
`d154186c` SDK baseline (no prior per-reason patch existed for this header).

## Validation
- Patch replay: `git apply` of `wct.h.metadata.patch` against the `d154186c`
  baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Base/Debug, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s)
  (Debug scraped per-arch as x64/x86/arm64 automatically under crossarch).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
