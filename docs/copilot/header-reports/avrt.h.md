# avrt.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
Two genuine ownership relationships, both previously blocked as
"return-value-handle+generic-type-direct-out-param":
- `AvSetMmThreadCharacteristicsA/W`, `AvSetMmMaxThreadCharacteristicsA/W`
  return a `HANDLE` directly, released via `AvRevertMmThreadCharacteristics`.
- `AvRtCreateThreadOrderingGroup`/`Ex(A/W)` produce a `HANDLE` via out-param
  `Context`, released via `AvRtDeleteThreadOrderingGroup`.
- `AvRtJoinThreadOrderingGroup` produces `Context` released via
  `AvRtLeaveThreadOrderingGroup` (a *different* consumer than Create, since
  join/leave and create/delete are distinct lifecycle pairs sharing the same
  `Context` out-param name).

## Correction to prior investigation
Both blocker classes are fixable with the standard per-function
`Function::return`/`Function::Parameter=[RAIIFree(...)]` mechanism; no
generic-type ambiguity actually applies since annotations are scoped per
function.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps (8 entries) to inline
ABI-neutral annotations directly on the producer return/out-parameter
declarations in `RecompiledIdlHeaders/um/avrt.h`:
```
AvSetMmThreadCharacteristicsA (...)
    _Win32_metadata_raii_free_(AvRevertMmThreadCharacteristics);
AvSetMmThreadCharacteristicsW (...)
    _Win32_metadata_raii_free_(AvRevertMmThreadCharacteristics);
AvSetMmMaxThreadCharacteristicsA (...)
    _Win32_metadata_raii_free_(AvRevertMmThreadCharacteristics);
AvSetMmMaxThreadCharacteristicsW (...)
    _Win32_metadata_raii_free_(AvRevertMmThreadCharacteristics);
AvRtCreateThreadOrderingGroup (_Out_ PHANDLE Context
    _Win32_metadata_raii_free_(AvRtDeleteThreadOrderingGroup), ...);
AvRtCreateThreadOrderingGroupExA (_Out_ PHANDLE Context
    _Win32_metadata_raii_free_(AvRtDeleteThreadOrderingGroup), ...);
AvRtCreateThreadOrderingGroupExW (_Out_ PHANDLE Context
    _Win32_metadata_raii_free_(AvRtDeleteThreadOrderingGroup), ...);
AvRtJoinThreadOrderingGroup (_Out_ PHANDLE Context
    _Win32_metadata_raii_free_(AvRtLeaveThreadOrderingGroup), ...);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The unrelated
`AvRtCreateThreadOrderingGroup*::Context=AVRT_THREAD_ORDERING_GROUP_HANDLE*`
and `AvSetMm*::return=AVRT_TASK_HANDLE` type-override entries are untouched.
All 8 former sidecar `RAIIFree` entries were removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/avrt.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `avrt.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Threading, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
