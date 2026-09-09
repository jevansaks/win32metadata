# ratings.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`RatingObtainQuery`/`RatingObtainQueryW` produce a generic `HANDLE` via a
direct out-param (`_Out_opt_ HANDLE *phRatingObtainQuery`), consumed by
`RatingObtainCancel(HANDLE)`.

## Correction to prior investigation
Prior report treated this as the same unrepresentable "generic/shared-type"
blocker class as resourceindexer.h/wslapi.h. Both of those are now fixed;
same corrected reasoning applies here.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/Ratings.h`:
```
STDAPI RatingObtainQuery(LPCSTR pszTargetUrl, DWORD dwUserData,
    void (*fCallback)(DWORD dwUserData, HRESULT hr, LPCSTR pszRating, void *lpvRatingDetails),
    _Out_opt_ HANDLE *phRatingObtainQuery
        _Win32_metadata_raii_free_(RatingObtainCancel));
STDAPI RatingObtainQueryW(LPCWSTR pszTargetUrl, DWORD dwUserData,
    void (*fCallback)(DWORD dwUserData, HRESULT hr, LPCWSTR pszRating, void *lpvRatingDetails),
    _Out_opt_ HANDLE *phRatingObtainQuery
        _Win32_metadata_raii_free_(RatingObtainCancel));
```
No `#include <win32metadata_annotations.h>` guard was added: this header
already unconditionally includes `<Shlwapi.h>`, which carries the guard, so
the macro is available transitively. Both former sidecar entries were
removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/Ratings.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `Ratings.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (InternetExplorer, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
