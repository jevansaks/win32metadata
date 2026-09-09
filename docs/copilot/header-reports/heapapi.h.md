# heapapi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`HeapCreate(...)` returns a private-heap `HANDLE` directly as the function
return value, released via `HeapDestroy(HANDLE)`.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/heapapi.h`:
```
_Ret_maybenull_
HANDLE
WINAPI
HeapCreate(
    _In_ DWORD flOptions,
    _In_ SIZE_T dwInitialSize,
    _In_ SIZE_T dwMaximumSize
    )
    _Win32_metadata_raii_free_(HeapDestroy);
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `heapapi.h.zzz-set-last-error.patch`. The former sidecar entry
`HeapCreate::return=[RAIIFree("HeapDestroy")]` was removed from
`emitter.settings.rsp`. Consolidated the pre-existing set-last-error changes
and this new annotation into a single cumulative
`generation/WinSDK/patches/post-midl/heapapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed the now-superseded
`heapapi.h.zzz-set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `heapapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Memory, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Note
`heapapi.h` is reached only via the `Memory` partition's `settings.rsp`
`IncludeRoot`, not a `main.cpp` include, so it is not part of the
1403-item `header-progress.json` ledger; this report is the sole tracking
artifact for this header.
