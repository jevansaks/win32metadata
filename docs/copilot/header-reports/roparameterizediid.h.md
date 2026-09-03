# Header Report: roparameterizediid.h

## Partitions
`WinRT.Metadata`

## Ownership audit (producer-site-only policy)
- `DECLARE_HANDLE(ROPARAMIIDHANDLE)` produced via `RoGetParameterizedTypeInstanceIID(..., _Outptr_opt_ ROPARAMIIDHANDLE* pExtra)` and released via `RoFreeParameterizedTypeExtra(_In_ ROPARAMIIDHANDLE extra)` — the same fixable shape as `appnotify.h`/`packagevirtualizationcontext.h`.
- **Checked `autoTypes.json` first**: `ROPARAMIIDHANDLE` **already has a complete, correct entry** — `"ValueType": "DECLARE_HANDLE", "CloseApi": "RoFreeParameterizedTypeExtra", "InvalidHandleValues": [-1, 0]` — matching this header exactly. No new inline annotation needed (same pattern as `swdevice.h`/`featurestagingapi.h`/`fhsvcctl.h`).
- `IRoSimpleMetaDataBuilder`/`IRoMetaDataLocator` are pure C++ interfaces (no `extern "C"`/`DllImport` surface), out of scope for the annotation mechanism (same reasoning as `scclient.h`).

## Conclusion
`accepted-normalized` — `ROPARAMIIDHANDLE` ownership is already correctly captured via the existing `autoTypes.json` entry; no gap to fix.
