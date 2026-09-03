# ks.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Kernel Streaming (KS) core definitions: structs, IOCTL property/method/event
descriptors, and kernel-mode COM-style vtable interfaces (`IKsObject`,
clock property-set interfaces, etc. via `STDMETHOD_`). All 13 callable
declarations found are `STDMETHOD_` vtable methods inside kernel-mode
interface definitions — out of scope per blocker class 5 (COM/kernel
vtable methods), consistent with the winddi.h precedent (kernel driver
contract headers are not producer-site-annotatable).

## Ownership Analysis
No free (non-vtable) functions exist. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
