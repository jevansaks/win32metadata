# winppi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`GdiGetSpoolFileHandle` returns a spool-file `HANDLE` directly, released via
`GdiDeleteSpoolFileHandle`.

`GdiGetPageHandle` also returns a `HANDLE` but (confirmed via Windows driver
documentation - "Using GDI Functions in Print Processors") this is a
**borrowed** page handle scoped to the current page/document, with cleanup
happening implicitly via `GdiEndPageEMF`/`GdiEndDocEMF`; there is no public
free API for it, so it is correctly left unannotated (not an ownership gap).

## Correction to prior investigation
Prior report treated both functions as the same unrepresentable blocker
class. Corrected: `GdiGetSpoolFileHandle` is a genuine, fixable
producer/consumer pair; `GdiGetPageHandle` is a borrowed handle and requires
no annotation at all (same treatment as `GetConsoleWindow` in wincon.h).

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
GdiGetSpoolFileHandle::return=[RAIIFree("GdiDeleteSpoolFileHandle")]
```

## Validation
ScrapeHeaders (Printing, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).

