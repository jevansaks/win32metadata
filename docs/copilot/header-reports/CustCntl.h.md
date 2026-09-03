# Header Report: CustCntl.h

## Partitions
`Controls`

## Ownership audit (producer-site-only policy)
- Declares only size-limit constants (`CCHCCCLASS`/etc.) and transparent data structs (`CCSTYLEA`/`CCSTYLEW`) for the Custom Control Library. No `extern`/`DllImport` functions declared in this header (the custom-control callback procedures are implemented by third-party control DLLs, not declared here).

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/transparent structs only, no extern functions).
