# Header Report: Ime.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
- `SendIMEMessageExA`/`SendIMEMessageExW(IN HWND, IN LPARAM)` take only a `HWND` (window reference, not owned) and `LPARAM`, returning `LRESULT` — no opaque handle produced. Remaining declarations are `IMESTRUCT` and `CP_*` constants.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle).
