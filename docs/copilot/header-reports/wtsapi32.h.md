# wtsapi32.h — accepted-normalized (producer-site fix applied)

**Partition:** TermServ
**Size:** ~51638 bytes

## Analysis

`wtsapi32.h` declares `WTSOpenServerW`/`WTSOpenServerA`/
`WTSOpenServerExW`/`WTSOpenServerExA` — all returning the generic
`HANDLE` type directly (not via `_Out_` param, not a distinct
typedef) — and single-arg `WTSCloseServer(HANDLE)`. Directly analogous
to the already-established `HeapCreate::return=[RAIIFree("HeapDestroy")]`
and `WTSVirtualChannelOpen::return=[RAIIFree("WTSVirtualChannelClose")]`
precedents (both also generic-`HANDLE`-returning functions fixed via
inline per-function `RAIIFree` on the return value).

Checked `emitter.settings.rsp` first: **no existing inline
annotation for any `WTSOpenServer*` variant** — a genuine gap. Added
4 new entries (`WTSOpenServerA/W/ExA/ExW::return=[RAIIFree("WTSCloseServer")]`)
immediately following the `WTSVirtualChannelOpen(Ex)` precedent
lines.

## Conclusion

Producer-site fix applied — 4 new inline `RAIIFree` return-value
annotations added for `WTSOpenServerA/W/ExA/ExW` in
`emitter.settings.rsp`, consistent with the `HeapCreate`/
`WTSVirtualChannelOpen` precedents.
