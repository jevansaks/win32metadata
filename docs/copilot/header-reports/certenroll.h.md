# certenroll.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Certificate Enrollment API COM interfaces (`ICertificateRequest`,
`IX509Certificate2`, etc.). Contains COM `STDMETHOD` vtable methods
only — out of scope per blocker class 5 (COM vtable methods). No
free-function declarations found.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
