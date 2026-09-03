# mfapi.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Core Media Foundation platform API (`MFStartup`/`MFShutdown`/work-queue
scheduling/etc.). Contains 126 `STDAPI`-declared free functions (plus 4
`STDAPI_(...)`-declared scalar-returning helpers). The large majority
output COM interface pointers via out-parameters (standard clean COM
pattern) or plain scalar values.

One distinct scalar typedef was investigated: `MFWORKITEM_KEY`
(`typedef unsigned __int64 MFWORKITEM_KEY;`), produced as an out-param by
`MFPutWorkItem`/`MFPutWorkItemEx`/`MFScheduleWorkItem` and consumed by
`MFCancelWorkItem(MFWORKITEM_KEY Key)`. This was evaluated but **not**
annotated: `MFCancelWorkItem` is an *optional* cancellation of a
scheduled callback, not a mandatory resource-release like
`CloseHandle`/`gluDeleteQuadric`. A work item that is never canceled
simply runs to completion and self-releases — omitting the call is a
valid, common usage pattern, not a resource leak. Attaching `RAIIFree`
would incorrectly imply the key must always be closed, which is
semantically wrong for an optional-cancel token. No existing
`autoTypes.json`/`emitter.settings.rsp` precedent uses a `Cancel*`
function as a `CloseApi`, reinforcing this distinction from the
established RAII-ownership handle patterns.

## Ownership Analysis
No annotatable producer/consumer handle pattern found (COM interface
pointers out of scope per standard convention; `MFWORKITEM_KEY` is an
optional-cancellation token, not a mandatory-release handle). No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
