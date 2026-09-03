# Header Report: fhsvcctl.h

## Partitions
`FileHistory`

## Ownership audit (producer-site-only policy)
- `DECLARE_HANDLE(FH_SERVICE_PIPE_HANDLE)` is a genuine opaque handle, produced via
  `FhServiceOpenPipe(_In_ BOOL, _Out_ FH_SERVICE_PIPE_HANDLE *Pipe)` and released via
  `FhServiceClosePipe(_In_ FH_SERVICE_PIPE_HANDLE Pipe)` — the same fixable shape as `appnotify.h`
  (batch `scraping-investigation-39`). All these functions are also `__declspec(deprecated(...))`.
- **Checked `autoTypes.json` first** (established methodology from `swdevice.h`/`featurestagingapi.h`,
  batches `-45`/`-46`): `FH_SERVICE_PIPE_HANDLE` **already has a complete, correct entry** —
  `"ValueType": "DECLARE_HANDLE", "CloseApi": "FhServiceClosePipe", "InvalidHandleValues": [-1, 0]` —
  matching this header exactly. No new inline annotation needed.

## Conclusion
`accepted-normalized` — `FH_SERVICE_PIPE_HANDLE` ownership is already correctly captured via the existing `autoTypes.json` entry; no gap to fix.
