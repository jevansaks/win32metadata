# Header Report: devquery.h

## Partitions
`DeviceQuery`, `FileHistory`

## Ownership audit (producer-site-only policy) — genuine fix applied
- `HDEVQUERY` (`DECLARE_HANDLE`) is produced via `_Out_ PHDEVQUERY phDevQuery` by `DevCreateObjectQuery`/`DevCreateObjectQueryEx`/`DevCreateObjectQueryFromId`/`DevCreateObjectQueryFromIdEx`/`DevCreateObjectQueryFromIds`/`DevCreateObjectQueryFromIdsEx`, closed via the single canonical `DevCloseObjectQuery(_In_ HDEVQUERY hDevQuery)`. `autoTypes.json` had an entry for `HDEVQUERY` but it was **missing `CloseApi`** — the same incomplete-entry pattern previously found for `appnotify.h`/`packagevirtualizationcontext.h`/`dciman.h`. Added `"CloseApi": "DevCloseObjectQuery"` (and `InvalidHandleValues: [0]`) to the existing entry. Confirmed via grep that `HDEVQUERY`/`DevCloseObjectQuery` are declared only in this one header (no cross-namespace conflict). Re-scraped `DeviceQuery` partition (0 errors).
- `DevGetObjects`/`DevGetObjectsEx` output `const DEV_OBJECT **ppObjects` (transparent struct array, freed via `DevFreeObjects`); `DevGetObjectProperties`/`Ex` output `const DEVPROPERTY **ppProperties` (transparent struct array, freed via `DevFreeObjectProperties`) — both out of scope per the transparent-struct-alloc/free-pairs precedent.

## Conclusion
`accepted-normalized` — genuine gap fixed via completing the existing `autoTypes.json` entry for `HDEVQUERY` (`CloseApi: DevCloseObjectQuery`).
