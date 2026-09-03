# Header Report: dsquery.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- Declares only `CLSID_*` GUID constants and plain data structs (`DSQUERYINITPARAMS`, `DSQUERYPARAMS`, `DSCOLUMN`, `DSQUERYCLASSLIST`) used for the Active Directory find/query UI dialogs. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constants/structs only, no functions).
