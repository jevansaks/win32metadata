# Header Report: computestorage.h

## Partitions
`HostComputeSystem`

## Ownership audit (producer-site-only policy)
- All `Hcs*` functions operate on paths (`PCWSTR`) and pre-owned, caller-supplied `HANDLE vhdHandle` values (input only — never produced/returned by this header).
- `HcsGetLayerVhdMountPath`'s `_Outptr_ PWSTR* mountPath` is a generic string buffer output (not a handle) — no documented distinct ownership type; out of scope for the `HANDLE`-specific annotation mechanism.
- `Is*Present()` query routines are `BOOLEAN` presence-check probes, no ownership implications.

## Conclusion
`accepted-normalized` — no ownership annotation required (no opaque handles produced; all `HANDLE` params are pre-owned caller inputs).
