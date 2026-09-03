# wdstptmgmt.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Windows Deployment Services (WDS) Transport Management COM interfaces
(`IWdsTransportManager`, `IWdsMulticastSession`, etc.). All 562 callable
declarations found are `STDMETHOD`/`STDMETHODCALLTYPE` COM vtable
methods — out of scope per blocker class 5 (COM vtable methods). No
free-function declarations found.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
