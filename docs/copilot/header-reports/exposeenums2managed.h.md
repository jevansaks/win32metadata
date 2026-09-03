# Header Report: exposeenums2managed.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
- Pure macro-definition helper (multi-include design, no include guard by design — redefines `ENUM`/`FLAGS`/etc. macros depending on `MANAGED_ENUMS`). Declares no types, functions, or handles itself.

## Conclusion
`accepted-normalized` — no ownership annotation required (macro-definitions only, no declarations).
