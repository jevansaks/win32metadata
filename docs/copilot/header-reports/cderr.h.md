# Header Report: cderr.h

## Partitions
`DlgBox`

## Ownership audit (producer-site-only policy)
- Declares only `CDERR_*`/`PDERR_*`/`CFERR_*`/`FNERR_*`/`FRERR_*`/`CCERR_*` common-dialog error-code constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (error-code constants only, no functions).
