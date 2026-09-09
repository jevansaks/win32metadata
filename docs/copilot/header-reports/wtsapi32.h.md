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

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps (6 entries total, including
`WTSVirtualChannelOpen`/`WTSVirtualChannelOpenEx`) to inline ABI-neutral
annotations directly on the producer return declarations in
`RecompiledIdlHeaders/um/WtsApi32.h`:
```
WTSOpenServerW(...) _Win32_metadata_raii_free_(WTSCloseServer);
WTSOpenServerA(...) _Win32_metadata_raii_free_(WTSCloseServer);
WTSOpenServerExW(...) _Win32_metadata_raii_free_(WTSCloseServer);
WTSOpenServerExA(...) _Win32_metadata_raii_free_(WTSCloseServer);
WTSVirtualChannelOpen(...) _Win32_metadata_raii_free_(WTSVirtualChannelClose);
WTSVirtualChannelOpenEx(...) _Win32_metadata_raii_free_(WTSVirtualChannelClose);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The unrelated
`WTSVirtualChannelOpen::hServer=[Optional][Reserved]` entry is untouched.
All 6 former sidecar `RAIIFree` entries were removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/WtsApi32.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `WtsApi32.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (TermServ, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Conclusion

All six producer functions in this header now carry inline
`_Win32_metadata_raii_free_` return-value annotations directly in
`RecompiledIdlHeaders/um/WtsApi32.h`; no sidecar entries remain.
