# WabUtil.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`FtgRegisterIdleRoutine(PFNIDLE lpfnIdle, ...)` returns `FTG` (an idle-routine
registration handle) directly as the function return value, released via
`DeregisterIdleRoutine(FTG)`.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/WabUtil.h`:
```
STDAPI_(FTG)
FtgRegisterIdleRoutine (PFNIDLE lpfnIdle, LPVOID lpvIdleParam,
	short priIdle, ULONG csecIdle, USHORT iroIdle)
	_Win32_metadata_raii_free_(DeregisterIdleRoutine);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry `FtgRegisterIdleRoutine::return=[RAIIFree("DeregisterIdleRoutine")]`
was removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/WabUtil.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `WabUtil.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Wab, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Note
`WabUtil.h` is reached only via the `Wab` partition's `settings.rsp`
`IncludeRoot` (`/um/wabutil.h`), not a `main.cpp` include, so it is not part
of the 1403-item `header-progress.json` ledger; this report is the sole
tracking artifact for this header.
