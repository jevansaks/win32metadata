# Header Report: lmuseflg.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- Declares only `USE_NOFORCE`/`USE_FORCE`/`USE_LOTS_OF_FORCE` force-level constants and the `FORCE_LEVEL` macro. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/macro only, no functions).
