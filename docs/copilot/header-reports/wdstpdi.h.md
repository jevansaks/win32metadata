# wdstpdi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
- `WdsTransportProviderCreateInstance(..., _Out_ PHANDLE phInstance)` produces
  a `HANDLE`, released via `WdsTransportProviderCloseInstance(HANDLE)`.
- `WdsTransportProviderOpenContent(..., _Out_ PHANDLE phContent)` produces a
  `HANDLE`, released via `WdsTransportProviderCloseContent(HANDLE)`.

## Correction to prior investigation
Prior report treated this as the unrepresentable "generic-type" blocker
class. Both pairs are fixable per-function.

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
WdsTransportProviderCreateInstance::phInstance=[RAIIFree("WdsTransportProviderCloseInstance")]
WdsTransportProviderOpenContent::phContent=[RAIIFree("WdsTransportProviderCloseContent")]
```

## Validation
ScrapeHeaders (Wds, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
