# dmemmgr.h

**Classification:** blocked (corrected evidence)

## Summary
`VidMemAlloc`/`HeapVidMemAllocAligned` return `FLATPTR` (`ULONG_PTR` alias)
directly, released via `VidMemFree(LPVMEMHEAP pvmh, FLATPTR ptr)`.

## Correction to prior investigation
Prior blocker text ("generic FLATPTR return value cannot be annotated") was
an inaccurate generalization - a bare integer/pointer-sized return value CAN
be annotated per-function (see i_cryptasn1tls.h's `HCRYPTASN1MODULE`, a
`DWORD`-backed handle, fixed in this same batch).

The correct, narrower reason this remains blocked: `VidMemFree` requires a
**second** caller-supplied argument (`pvmh`, the originating heap) beyond the
value being freed. The established `RAIIFree` convention (68 existing
`emitter.settings.rsp` entries, every one a single-argument release function)
has no mechanism to supply that additional argument.

## Ownership Analysis
No `emitter.settings.rsp` change possible with the current unary-only
`RAIIFree` convention.

## Conclusion
`blocked` - remains blocked, but for the corrected reason above (free
function needs a second argument, not "generic type cannot be annotated").
