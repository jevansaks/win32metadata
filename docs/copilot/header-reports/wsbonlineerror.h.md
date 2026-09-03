# Header Report: wsbonlineerror.h

## Partitions
`Wsb`

## Ownership audit (producer-site-only policy)
- Declares only `WSB_ONLINE_*` HRESULT error-code constants and `SEVERITY_OF`/`FACILITY_OF` bit-extraction macros (same pattern as the already-accepted `wsbapperror.h`, batch `scraping-investigation-44`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (HRESULT error-code constants/macros only, no functions).
