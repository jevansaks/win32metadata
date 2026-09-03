# Header Report: LMUse.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- `NetUseAdd`/`NetUseGetInfo`/`NetUseEnum`/etc. follow the already-established `NetApiBufferAllocate`/`NetApiBufferFree` convention (canonical clean pattern reused across many `lm*.h` headers) — out of scope for the `HANDLE`-specific mechanism.

## Conclusion
`accepted-normalized` — no ownership annotation required (Net API buffer-allocation convention, already-established clean pattern).
