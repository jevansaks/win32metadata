# Dimm.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Text Services Framework IME (Input Method Manager) COM interfaces
(`ITfInputProcessorProfiles`, `ITfLanguageProfileNotifySink`, etc.). All
432 callable declarations found are `STDMETHOD`/`STDMETHOD_` COM vtable
methods — out of scope per blocker class 5 (COM vtable methods). No
free-function declarations found.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
