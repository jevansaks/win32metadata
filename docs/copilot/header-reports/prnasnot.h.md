# prnasnot.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`RegisterForPrintAsyncNotifications(..., _Out_ HANDLE* phNotify)` produces a
`HANDLE`, released via `UnRegisterForPrintAsyncNotifications(HANDLE)`.
(`CreatePrintAsyncNotifyChannel`'s `IPrintAsyncNotifyChannel**` out-param is a
standard COM interface out-pointer, already handled by the generic
`ComOutPtr` convention - not part of this gap.)

## Correction to prior investigation
Prior report treated the `HANDLE` out-param as the unrepresentable
"generic-type" blocker class. Fixable per-function.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/prnasnot.h`:
```
RegisterForPrintAsyncNotifications(
    _In_opt_ PCWSTR                            pszName,
    _In_     PrintAsyncNotificationType*       pNotificationType,
             PrintAsyncNotifyUserFilter        eUserFilter,
             PrintAsyncNotifyConversationStyle eConversationStyle,
    _In_     IPrintAsyncNotifyCallback*        pCallback,
    _Out_    HANDLE*                           phNotify
        _Win32_metadata_raii_free_(UnRegisterForPrintAsyncNotifications)
    );
```
No `#include <win32metadata_annotations.h>` guard was added: this header
already unconditionally includes `<windows.h>`, which transitively pulls in
`WinBase.h` (carries the guard), so the macro is available transitively. The
former sidecar entry
`RegisterForPrintAsyncNotifications::phNotify=[RAIIFree("UnRegisterForPrintAsyncNotifications")]`
was removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/prnasnot.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `prnasnot.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Gdi, Printing, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s) (Printing scraped per-arch as x64/x86/arm64 automatically under
  crossarch).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
