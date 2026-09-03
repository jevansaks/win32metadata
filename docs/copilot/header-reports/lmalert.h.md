# Header Report: lmalert.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- `NetAlertRaise`/`NetAlertRaiseEx` take `LPVOID Buffer`/`VariableInfo` as caller-allocated **input** buffers (not allocated/returned by the API). `STD_ALERT`/`ADMIN_OTHER_INFO`/`ERRLOG_OTHER_INFO`/`PRINT_OTHER_INFO`/`USER_OTHER_INFO` are plain data structs. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (caller-supplied input buffers only, no handle).
