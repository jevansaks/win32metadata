# Header Report: msdrmgetinfo.h

## Partitions
`Rm`

## Ownership audit (producer-site-only policy)
- Pure `PWSTR` query-tag string constants (`g_wsz*`) used with `DRMGetInfo`/`DRMGetUnboundLicenceAttribute`/etc. (declared elsewhere, in `msdrm.h`). No functions in this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (string constants only, no functions).
