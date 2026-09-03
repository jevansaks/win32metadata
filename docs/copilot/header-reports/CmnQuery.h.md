# Header Report: CmnQuery.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- Pure GUID constants (`IID_IQueryForm`/`CLSID_CommonQuery`/etc.) plus `#define` flag constants for the Common Query dialog. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/flag constants only, no extern functions).
