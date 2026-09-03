# Header Report: stringapiset.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- `CompareStringEx`/`CompareStringOrdinal`/`CompareStringW`/`FoldStringW`/`GetStringTypeExW`/`GetStringTypeW`/`MultiByteToWideChar`/`WideCharToMultiByte` all operate on caller-allocated string buffers and plain integer flags. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (string/buffer conversion API only, no handle).
