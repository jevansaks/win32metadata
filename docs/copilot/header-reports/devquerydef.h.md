# Header Report: devquerydef.h

## Partitions
`DeviceQuery`

## Scrape validation
- Re-scraped `DeviceQuery` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only enums (`_DEV_OBJECT_TYPE`, `_DEV_QUERY_FLAGS`, `_DEV_QUERY_STATE`, `_DEV_QUERY_RESULT_ACTION`) and data structs (`_DEV_OBJECT`, `_DEV_QUERY_RESULT_ACTION_DATA`, `_DEV_QUERY_PARAMETER`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/data structs only, no functions).
