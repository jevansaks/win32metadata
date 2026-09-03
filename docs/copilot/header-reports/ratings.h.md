# Header Report: ratings.h

## Partitions
`InternetExplorer`

## Ownership audit (producer-site-only policy) — BLOCKED (known blocker class)

- `RatingObtainQuery(..., _Out_opt_ HANDLE *phRatingObtainQuery)`/`RatingObtainQueryW(...)` **produce** a generic `HANDLE` via a direct out-parameter, consumed/cancelled by `RatingObtainCancel(HANDLE hRatingObtainQuery)`.
- The type is the plain, generic system `HANDLE` (not distinctly named) — same generic/shared-type blocker class already documented for `resourceindexer.h`/`physicalmonitorenumerationapi.h`/`userenv.h`/`wslapi.h` (direct out-param variant, like `wslapi.h`).
- `RatingCheckUserAccess(W)`/`RatingAccessDeniedDialog(2)(W)`/`RatingFreeDetails` allocate/free a generic untyped `void*` ("RatingDetails") — out of scope for the same reason as `resourceindexer.h`'s `PVOID`.

## Conclusion
`blocked` — genuine `HANDLE` ownership relationship (`RatingObtainQuery(W)` → `RatingObtainCancel`) via a direct out-parameter, but the generic `HANDLE` type precludes producer-site annotation; same root blocker class as `wslapi.h`.
