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

## Ownership Analysis
Added to `emitter.settings.rsp` (8 entries):
```
AvSetMmThreadCharacteristicsA::return=[RAIIFree("AvRevertMmThreadCharacteristics")]
AvSetMmThreadCharacteristicsW::return=[RAIIFree("AvRevertMmThreadCharacteristics")]
AvSetMmMaxThreadCharacteristicsA::return=[RAIIFree("AvRevertMmThreadCharacteristics")]
AvSetMmMaxThreadCharacteristicsW::return=[RAIIFree("AvRevertMmThreadCharacteristics")]
AvRtCreateThreadOrderingGroup::Context=[RAIIFree("AvRtDeleteThreadOrderingGroup")]
AvRtCreateThreadOrderingGroupExA::Context=[RAIIFree("AvRtDeleteThreadOrderingGroup")]
AvRtCreateThreadOrderingGroupExW::Context=[RAIIFree("AvRtDeleteThreadOrderingGroup")]
AvRtJoinThreadOrderingGroup::Context=[RAIIFree("AvRtLeaveThreadOrderingGroup")]
```

## Validation
ScrapeHeaders (Threading, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
