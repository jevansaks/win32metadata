# Header Report: lmstats.h

## Partitions
`Fs`

## Ownership audit (producer-site-only policy)
- `NetStatisticsGet(..., _Outptr_ LPBYTE *Buffer)` follows the already-established `NetApiBufferAllocate`/`NetApiBufferFree` convention (canonical clean pattern reused across many `lm*.h` headers) — out of scope for the `HANDLE`-specific mechanism.

## Conclusion
`accepted-normalized` — no ownership annotation required (Net API buffer-allocation convention, already-established clean pattern).
