# Header Report: wiamicro.h

## Partitions
`Wia`

## Ownership audit (producer-site-only policy)
- `MicroEntry`/`Scan`/`SetPixelWindow` are `__declspec(dllexport)` functions implemented BY the WIA scanner/camera microdriver and called by the WIA service — operate on `_Inout_ PVAL`/`PSCANINFO` transparent structs.
- `SCANINFO.DeviceIOHandles[MAX_IO_HANDLES]` is an array of generic `HANDLE` values populated by the WIA service caller *before* invoking the microdriver (per comment: "Device IO handles needed for device communication") — not produced by these functions.
- `VAL` is a tagged-union-style struct with a `HANDLE *pHandle`/`HGLOBAL handle` member used for certain `MicroEntry` commands; there is no single dedicated producer function with clear `_Out_` ownership-transfer semantics for this generic-typed union field (unlike the established direct-out-param blocker pattern) — the exact ownership semantics are command-specific and not staticly determinable from the declaration alone.

## Conclusion
`accepted-normalized` — no concrete, annotatable ownership gap identified (no direct function signature exhibits a clear `_Out_`-style handle-producing pattern; the ambiguous union field is out of scope for the annotation mechanism).
