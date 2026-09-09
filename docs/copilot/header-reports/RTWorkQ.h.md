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

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/RTWorkQ.h`:
```
STDAPI RtwqSetDeadline(DWORD workQueueId, LONGLONG deadlineInHNS, _Out_ HANDLE* pRequest
    _Win32_metadata_raii_free_(RtwqCancelDeadline));
#if (WINVER >= _WIN32_WINNT_WIN10)
STDAPI RtwqSetDeadline2(DWORD workQueueId, LONGLONG deadlineInHNS, LONGLONG preDeadlineInHNS, _Out_ HANDLE* pRequest
    _Win32_metadata_raii_free_(RtwqCancelDeadline));
#endif
```
Added the `#include <winapifamily.h>` + `#if defined(WIN32METADATA)
#include <win32metadata_annotations.h> #endif` guard block right after the
`__RTWORKQ_H__` include guard (this header had no unconditional include of
its own that already carried the guard, and no prior patch existed). Both
former sidecar entries were removed from `emitter.settings.rsp`.
Consolidated into a single new
`generation/WinSDK/patches/post-midl/RTWorkQ.h.metadata.patch` against the
pristine `d154186c` SDK baseline. `RtwqJoinWorkQueue`'s two-argument release
gap (documented above) is unrelated to the emitter.settings.rsp RAIIFree
sidecar-removal tranche - it was never added as a sidecar entry - and
remains an open, narrower gap.

## Validation
- Patch replay: `git apply` of `RTWorkQ.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Threading, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Note
Full EmitWinmd validation could not be completed in this environment: the AllJoyn/WinRT.AllJoyn partitions fail with a pre-existing, unrelated MSVC/Clang toolchain mismatch (`__builtin_verbose_trap`), and a separate pre-existing cross-arch-merge gap (NTSTATUS-returning autoTypes such as CLFS_MGMT_CLIENT/HIORING are only resolvable via the full 3-arch scrape-then-merge CI pipeline, not a single local invocation). Both are documented, project-wide, pre-existing limitations unrelated to this change (see prior batch notes, e.g. winbio.h). Per-partition `ScrapeHeaders` for the affected partition(s) was confirmed to succeed with 0 errors (no header/main.cpp changes were made). The `emitter.settings.rsp` syntax used matches 68 existing, already-shipped precedents exactly (e.g. `WTSOpenServerA::return=[RAIIFree(...)]`, `DnsAcquireContextHandle_A::pContext=[RAIIFree(...)]`).
