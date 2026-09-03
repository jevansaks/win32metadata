# Header Report: persist.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- Declares only `DBPROP*` constants and `extern const CLSID`/`GUID`/`char*`/`WCHAR*` constants (`CLSID_MSPersist`, `PROGID_MSPersist*`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
