# Header Report: bcp47mrm.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- Two functions: `GetDistanceOfClosestLanguageInList` (out-param is `double*`) and `IsWellFormedTag` (returns `bool`). No `HANDLE`/`DECLARE_HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle-producing functions).
