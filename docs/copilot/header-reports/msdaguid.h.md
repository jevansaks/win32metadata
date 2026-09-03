# Header Report: msdaguid.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- Declares only `EXTERN_C const GUID` (OLE DB) constants (`CLSID_OLEDB_ENUMERATOR`, `CLSID_EXTENDEDERRORINFO`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constants only, no functions).
