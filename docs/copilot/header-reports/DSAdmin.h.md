# Header Report: DSAdmin.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- Pure GUID constants (`CLSID_DsAdminCreateObj`/`IID_IDsAdmin*`) plus COM interfaces (`IDsAdminCreateObj`, `IDsAdminNewObj`, etc.). No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constants + COM interfaces only, no extern functions).
