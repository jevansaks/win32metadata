# Header Report: dde.h

## Partitions
`DataXchg`

## Ownership audit (producer-site-only policy)
- `DdeSetQualityOfService`/`ImpersonateDdeClientWindow` take only `HWND`/security-descriptor inputs, no opaque handles produced.
- `FreeDDElParam`/`ReuseDDElParam` operate on `LPARAM`-packed DDE message data, not opaque handles.
- Remaining declarations are `WM_DDE_*` message constants and plain data structs. No ownership gaps.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle).
