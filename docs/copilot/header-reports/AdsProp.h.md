# AdsProp.h

**Classification:** blocked (corrected evidence)

## Summary
`ADsPropCreateNotifyObj(..., HWND* phNotifyObj)` produces an `HWND` for a
hidden notification window.

## Correction to prior investigation
Prior blocker text ("generic HWND out-param cannot be annotated, extends
blocker class to HWND") was an inaccurate generalization - per-function
out-param annotation of a generic handle type is representable in principle,
as demonstrated by the other headers fixed in this same batch (e.g.
resourceindexer.h, wslapi.h).

The correct, narrower reason this remains blocked: confirmed via Microsoft
Learn documentation that the notification object has **no companion free
function** at all. The caller sends a `WM_ADSPROP_NOTIFY_EXIT` window message
to the notification window, which then destroys itself; there is no
`ADsPropDestroyNotifyObj`-style API to name in a `RAIIFree` attribute. This is
a genuine, different limitation (self-managed lifetime via a windowing
message protocol, not a function call), not a type-genericity problem.

## Ownership Analysis
No `emitter.settings.rsp` change possible - no consumer function exists.

## Conclusion
`blocked` - remains blocked, but for the corrected reason above (no
close/destroy *function* exists to reference, not "generic type cannot be
annotated").
