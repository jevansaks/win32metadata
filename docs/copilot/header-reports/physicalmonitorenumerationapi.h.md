# physicalmonitorenumerationapi.h

**Classification:** blocked (corrected evidence)

## Summary
`GetPhysicalMonitorsFromHMONITOR`/`FromIDirect3DDevice9` fill a
caller-allocated array of `PHYSICAL_MONITOR` structs
(`_Out_writes_(dwPhysicalMonitorArraySize) LPPHYSICAL_MONITOR
pPhysicalMonitorArray`), each containing a `HANDLE hPhysicalMonitor` field,
released per-element via `DestroyPhysicalMonitor(HANDLE)` (a unary free
function) or in bulk via `DestroyPhysicalMonitors(DWORD, LPPHYSICAL_MONITOR)`.

## Correction to prior investigation
Prior blocker text ("same generic/shared-type blocker class as
resourceindexer.h") is now out of date: resourceindexer.h's bare-`PVOID`
out-param case was fixed this batch and is not actually the same class.

The correct, narrower reason this remains blocked: the producer's out-param
is the **array itself**, not a `HANDLE`/`HANDLE*` value directly - ownership
lives on a field nested inside each array element
(`PHYSICAL_MONITOR::hPhysicalMonitor`). Expressing this would require a
struct-field-level `RAIIFree` annotation (e.g.
`PHYSICAL_MONITOR::hPhysicalMonitor=[RAIIFree("DestroyPhysicalMonitor")]`),
which has **zero precedent** in this codebase's 68 existing `RAIIFree`
entries (every one attaches to a function's return value or a direct
parameter, never to a bare struct field) and could not be validated in this
session (full `EmitWinmd` could not be run to completion locally due to
unrelated, pre-existing toolchain/cross-arch-merge limitations - see Note).
Applying an untested annotation surface without validation risks a build
break, so it was not attempted.

## Ownership Analysis
No `emitter.settings.rsp` change applied (would require validating
struct-field-level `RAIIFree` support first).

## Conclusion
`blocked` - remains blocked pending either (a) confirmation that
`Type::Field=[RAIIFree(...)]` is accepted by the emitter/`MetadataSyntaxTreeCleaner`,
or (b) a policy decision on how to express per-element ownership for an
out-param array. This is a genuinely different, narrower blocker than the
one previously recorded.
