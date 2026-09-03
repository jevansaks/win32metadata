# Header Report: lmcons.h

## Partitions
`Dfs`, `NetMgmt`

## Ownership audit (producer-site-only policy)
- Pure LAN Manager API constants (string-length limits like `UNLEN`, `PWLEN`, etc.), included by other LAN Manager headers. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
