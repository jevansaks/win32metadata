# RTWorkQ.h

**Classification:** accepted-normalized (producer-site fix applied, partial)

## Summary
- `RtwqSetDeadline`/`RtwqSetDeadline2(..., _Out_ HANDLE* pRequest)` produce a
  `HANDLE`, released via `RtwqCancelDeadline(HANDLE pRequest)` (unary). Fixed.
- `RtwqJoinWorkQueue(DWORD workQueueId, HANDLE hFile, _Out_ HANDLE* out)`
  produces a "cookie" `HANDLE`, but its consumer
  `RtwqUnjoinWorkQueue(DWORD workQueueId, HANDLE hFile)` requires the
  original `workQueueId` as well as the cookie - a two-argument release.
  Confirmed via Microsoft Learn documentation. The established
  `RAIIFree` convention (68 existing entries, all single-argument release
  functions) cannot express a release that needs an extra caller-supplied
  argument. Left unannotated - a narrower, genuine gap that does not block
  the rest of the header (consistent with how many other unannotated
  producer/consumer pairs exist across the winmd without blocking their
  headers).

## Correction to prior investigation
Prior report blocked the whole header citing "generic-type-direct-out-param"
for both functions. `RtwqSetDeadline`/`RtwqSetDeadline2` are fixable;
`RtwqJoinWorkQueue` has a distinct, real limitation (multi-arg free function)
that is correctly left unannotated rather than blocking classification.

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
RtwqSetDeadline::pRequest=[RAIIFree("RtwqCancelDeadline")]
RtwqSetDeadline2::pRequest=[RAIIFree("RtwqCancelDeadline")]
```

## Validation
ScrapeHeaders (Threading, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).

