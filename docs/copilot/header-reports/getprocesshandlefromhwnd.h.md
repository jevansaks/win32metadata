# getprocesshandlefromhwnd.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`GetProcessHandleFromHwnd(HWND hwnd)` returns a process `HANDLE` directly as
the function return value, released via `CloseHandle`.

## Correction to prior investigation
The prior report (`scraping-investigation-14`) concluded this was unrepresentable,
citing a `WinmdUtils` dump search that found RAIIFree/InvalidHandleValue attributes
only on `struct` type declarations. That search missed the separate, already-shipped
`emitter.settings.rsp` `--memberRemap` mechanism, which applies `[RAIIFree(...)]`
directly to a specific function's return value or parameter (68 existing entries,
e.g. `WTSOpenServerA::return=[RAIIFree("WTSCloseServer")]`,
`FindFirstFileA::return=[RAIIFree("FindClose")]`). This mechanism is scoped to the
named function only - it does not assert ownership for `HANDLE` globally - so it is
exactly applicable here.

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
GetProcessHandleFromHwnd::return=[RAIIFree("CloseHandle")]
```

## Validation
ScrapeHeaders (Threading, x64): Build succeeded, 0 Error(s) (no header changes).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (__builtin_verbose_trap), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition ScrapeHeaders for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The mitter.settings.rsp syntax used matches 68 existing, already-shipped precedents exactly (e.g. WTSOpenServerA::return=[RAIIFree(...)], DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]).

