# Header Report: msdasql.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- Pure GUID constants (`IID_*`/`CLSID_*`/`DBPROPSET_*`) and `#define` property-ID/bitmask constants. No functions declared directly in this header (includes `msdasql_interfaces.h` separately for interface definitions).

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/property constants only, no functions).
