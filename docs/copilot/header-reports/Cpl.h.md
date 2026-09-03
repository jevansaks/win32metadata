# Header Report: Cpl.h

## Partitions
`Shell`

## Ownership audit (producer-site-only policy)
- Declares only `APPLET_PROC` (a function-pointer typedef for `CPlApplet`, implemented by control-panel DLLs but not declared as an `extern`/`DllImport` function here) and transparent data structs (`CPLINFO`, `NEWCPLINFOA`). No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (function-pointer typedef + transparent structs only, no extern functions).
