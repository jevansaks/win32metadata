# Header Report: msdaora.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- Declares only `EXTERN_C const CLSID`/`GUID` constants (`CLSID_MSDAORA*`, `DBPROPSET_MSDAORA*`) and forward-declared C++ classes with `DECLSPEC_UUID`. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (CLSID/GUID constants only, no functions).
