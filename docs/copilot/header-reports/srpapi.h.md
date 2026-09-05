# srpapi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`SrpCreateThreadNetworkContext(..., _Out_ HTHREAD_NETWORK_CONTEXT*
threadNetworkContext)` produces a struct-pointer out-param, released via
`SrpCloseThreadNetworkContext(_Inout_ HTHREAD_NETWORK_CONTEXT*
threadNetworkContext)` - the same pointer/type, single argument.

## Correction to prior investigation
Prior report classified this as "generic-type-nested-in-struct", implying the
ownership target was a bare field (`HTHREAD_NETWORK_CONTEXT.ThreadContext`)
requiring unproven struct-field-level annotation. This is incorrect: the
out-param and the consumer's parameter are both the *struct pointer itself*
(`HTHREAD_NETWORK_CONTEXT*`), which is a completely ordinary
function-parameter-level ownership pair - no different in kind from any
other out-param/consumer pair fixed in this batch. No struct-field
annotation is needed.

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
SrpCreateThreadNetworkContext::threadNetworkContext=[RAIIFree("SrpCloseThreadNetworkContext")]
```

## Validation
ScrapeHeaders (Edp, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
