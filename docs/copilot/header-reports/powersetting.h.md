# powersetting.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`PowerRegisterForEffectivePowerModeNotifications(..., _Outptr_ PVOID*
RegistrationHandle)` produces a generic `PVOID`, released via
`PowerUnregisterFromEffectivePowerModeNotifications(PVOID)`. (The
`HPOWERNOTIFY`/`PowerSettingRegisterNotification` portion of this header was
already fixed in a prior batch via `autoTypes.json`.)

## Correction to prior investigation
Prior report treated the remaining `PVOID` out-param as the unrepresentable
"generic-type" blocker class. Fixable per-function, same reasoning as
resourceindexer.h.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/powersetting.h`:
```
PowerRegisterForEffectivePowerModeNotifications(
    _In_ ULONG Version,
    _In_ EFFECTIVE_POWER_MODE_CALLBACK* Callback,
    _In_opt_ PVOID Context,
    _Outptr_ PVOID* RegistrationHandle
        _Win32_metadata_raii_free_(PowerUnregisterFromEffectivePowerModeNotifications)
    );
```
No `#include <win32metadata_annotations.h>` guard was added: this header
already unconditionally includes `<minwindef.h>`, which carries the guard, so
the macro is available transitively. The former sidecar entry
`PowerRegisterForEffectivePowerModeNotifications::RegistrationHandle=[RAIIFree("PowerUnregisterFromEffectivePowerModeNotifications")]`
was removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/powersetting.h.metadata.patch` against
the pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `powersetting.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Power, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
