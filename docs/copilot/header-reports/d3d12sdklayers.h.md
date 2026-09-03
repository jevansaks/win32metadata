# d3d12sdklayers.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Direct3D 12 debug/validation-layer COM interfaces (`ID3D12Debug`,
`ID3D12InfoQueue`, etc.). All 271 callable declarations found are
`STDMETHOD`/`STDMETHOD_` COM vtable methods — out of scope per blocker
class 5 (COM vtable methods). No free-function declarations found.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
