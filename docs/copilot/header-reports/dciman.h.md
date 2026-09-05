# dciman.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`DCIOpenProvider()` returns `HDC` directly, released via
`DCICloseProvider(HDC)`. (The `HWINWATCH` gap in this same header -
`WinWatchOpen`/`WinWatchClose` - was already fixed in a prior batch via
`autoTypes.json`.)

## Correction to prior investigation
`HDC` has no single universal close API (context-dependent: `ReleaseDC`,
`DeleteDC`, `EndPaint`, etc.), so it correctly has no type-level
`autoTypes.json` `CloseApi`. But `DCIOpenProvider` specifically only ever
pairs with `DCICloseProvider` - a per-function annotation is exactly the
right tool here, not a type-level one, and does not conflict with any other
`HDC`-returning function.

## Ownership Analysis
Added to `emitter.settings.rsp`:
```
DCIOpenProvider::return=[RAIIFree("DCICloseProvider")]
```

## Validation
ScrapeHeaders (WinProg, x64): Build succeeded, 0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
