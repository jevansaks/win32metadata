# MMCObj.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Microsoft Management Console (MMC) snap-in COM interfaces
(`IComponent`, `IComponentData`, `IConsole`, etc.). All 485 callable
declarations found are `STDMETHOD`/`STDMETHOD_` COM vtable methods — out
of scope per blocker class 5 (COM vtable methods). No free-function
declarations found.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
