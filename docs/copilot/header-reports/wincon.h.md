# wincon.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`wincon.h` redirects to `consoleapi.h`/`consoleapi2.h`/`consoleapi3.h`/`winconp.h`.
Three producers return a console `HANDLE` directly as the function return value:
- `CreateConsoleScreenBuffer` (documented API) - closed via `CloseHandle` per MSDN.
- `OpenConsoleW` / `DuplicateConsoleHandle` (internal, declared in `winconp.h`) -
  closed via `CloseConsoleHandle`.

## Correction to prior investigation
Prior report claimed no precedent exists anywhere for annotating a bare
return-value `HANDLE`. Corrected: 68 existing `emitter.settings.rsp` entries
already do exactly this (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`).

Two other return-value `IntPtr` functions in this partition,
`GetConsoleWindow()`/`GetConsoleInputWaitHandle()`, remain correctly
unannotated - they return **borrowed** handles that callers must not close.

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
CreateConsoleScreenBuffer::return=[RAIIFree("CloseHandle")]
OpenConsoleW::return=[RAIIFree("CloseConsoleHandle")]
DuplicateConsoleHandle::return=[RAIIFree("CloseConsoleHandle")]
```

## Validation
ScrapeHeaders (Console, x86): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
