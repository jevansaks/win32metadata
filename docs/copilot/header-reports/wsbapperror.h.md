# Header Report: wsbapperror.h

## Partitions
`Wsb`

## Ownership audit (producer-site-only policy)
- Declares only `WSBAPP_*` HRESULT error-code constants and `SEVERITY_OF`/`FACILITY_OF` bit-extraction macros. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error-code constants/macros only, no functions).
