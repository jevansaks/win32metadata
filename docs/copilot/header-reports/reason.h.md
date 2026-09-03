# Header Report: reason.h

## Partitions
`FileHistory`, `Shutdown`

## Ownership audit (producer-site-only policy)
- Pure `#define` shutdown-reason-code and policy constants (`SHTDN_REASON_*`, `POLICY_SHOWREASONUI_*`, `SNAPSHOT_POLICY_*`, `MAX_REASON_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
