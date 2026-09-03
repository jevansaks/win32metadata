# Header Report: evalcom2.h

## Partitions
`Setup`

## Ownership audit (producer-site-only policy)
- Declares `DEFINE_GUID(CLSID_EvalCom2, ...)`, `DEFINE_GUID(IID_IValidate, ...)`, and the `IValidate` COM interface (`OpenDatabase`/`OpenCUB`/`CloseDatabase`/`CloseCUB`/`SetDisplay`/`SetStatus`/`Validate`). `SetDisplay`/`SetStatus` take a caller-supplied `LPVOID pContext` (an opaque user-context pointer passed back to the caller's own callback, not produced/owned by this interface). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface with caller-supplied context pointers only, no handle).
