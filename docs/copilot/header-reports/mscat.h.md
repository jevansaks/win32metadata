# mscat.h — accepted-normalized (2 items blocked, evidence-documented)

**Partition:** Security.Cryptography.Catalog, Security.Cryptography.Sip
**Size:** ~39804 bytes

## Analysis

`mscat.h` (Catalog API) declares several handle-producing functions.

**Blocked / not representable** (concise, reused evidence):
- `CryptCATOpen`/`CryptCATClose`: return/consume the fully generic
  `HANDLE` type directly (not a distinct typedef) — blocker-class 2.
- `HCATADMIN` (`typedef HANDLE HCATADMIN;`, produced by
  `CryptCATAdminAcquireContext`/`CryptCATAdminAcquireContext2`): its
  release function `CryptCATAdminReleaseContext(HCATADMIN, DWORD
  dwFlags /* reserved, must be 0 */)` requires a second parameter.
  No `autoTypes.json` entry exists yet — deferred pending confirmation
  of how downstream consumers handle reserved-flag close functions
  (kept consistent with the `SQLFreeStmt` precedent: multi-parameter
  close functions are treated as unrepresentable via the single-arg
  `CloseApi` convention until demonstrated otherwise).
- `HCATINFO` (`typedef HANDLE HCATINFO;`, produced by
  `CryptCATAdminEnumCatalogFromHash`/`CryptCATAdminAddCatalog`): its
  only release function, `CryptCATAdminReleaseCatalogContext(HCATADMIN
  hCatAdmin, HCATINFO hCatInfo, DWORD dwFlags)`, requires an
  **external `HCATADMIN` context parameter** in addition to the
  handle itself and the reserved flags — genuinely unrepresentable
  via a single-arg `CloseApi` (the close operation depends on
  external state beyond the handle).

Confirmed via `emitter.settings.rsp` review that this repo also
supports an inline per-parameter `[RAIIFree("Func")]` annotation
mechanism (e.g. `CfOpenFileWithOplock::ProtectedHandle=[RAIIFree(...)]`)
in addition to the `autoTypes.json` per-typedef mechanism — both
still assume a close function callable with just the handle, so
neither resolves the multi-mandatory-parameter cases above.

## Conclusion

No representable producer-site fix applies to `HCATADMIN`/`HCATINFO`
(multi-parameter close functions); `CryptCATOpen`/`Close` use the
generic `HANDLE` type (blocker-class 2). No patch required; evidence
recorded for future reconsideration if consumer tooling is confirmed
to support extra close-function parameters.
