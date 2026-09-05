# resourceindexer.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`CreateResourceIndexer(..., _Outptr_ PVOID* ppResourceIndexer)` /
`DestroyResourceIndexer(PVOID)` form a genuine ownership pair via a direct
out-param.

## Correction to prior investigation
Prior report concluded that annotating a bare `PVOID` out-param "would
incorrectly apply RAIIFree to every void* in the published metadata" and
required "introducing a new named handle typedef first." This is incorrect:
the `emitter.settings.rsp` `Function::Parameter=[RAIIFree(...)]` mechanism is
scoped to the specific function and parameter name, not to the `PVOID` type
globally - precedent already exists for this exact shape, e.g.
`DnsAcquireContextHandle_A::pContext=[RAIIFree("DnsReleaseContextHandle")]`
(`pContext` is also a generic pointer-out-param, not a distinct handle
typedef).

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
CreateResourceIndexer::ppResourceIndexer=[RAIIFree("DestroyResourceIndexer")]
```

## Validation
ScrapeHeaders (MenuRc, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
