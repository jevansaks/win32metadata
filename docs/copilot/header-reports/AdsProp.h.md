# Header Report: AdsProp.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- `ADsPropCreateNotifyObj(LPDATAOBJECT pAppThdDataObj, _In_ PWSTR pwzADsObjName, HWND * phNotifyObj)` produces a generic `HWND` (hidden notification window) via a direct out-param. `HWND` already has an `autoTypes.json` entry (`AlsoUsableFor: HANDLE`) but **deliberately has no `CloseApi`** — like generic `HANDLE`, `HWND` is a shared type used across the entire windowing subsystem by thousands of different creation/destruction APIs with different semantics, so it cannot safely carry a single universal close function. This is the already-established **generic-type direct-out-param** blocker class, now confirmed to extend to `HWND` (alongside `HANDLE`/`PVOID`/`DWORD`) for the same reason.
- No explicit `ADsPropDestroyNotifyObj` exists; the notification window is presumably torn down via a `WM_ADSPROP_NOTIFY_EXIT` message or standard `DestroyWindow`, neither of which is a dedicated, single, unambiguous close API distinct from generic window teardown.

## Conclusion
`blocked` — genuine gap in `ADsPropCreateNotifyObj` (generic `HWND` direct-out-param, extends established generic-type blocker class to `HWND`).
