# consoleapi2.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`CreateConsoleScreenBuffer(...)` returns a console-screen-buffer `HANDLE`
directly as the function return value, released via the generic
`CloseHandle`.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer return declaration in
`RecompiledIdlHeaders/um/consoleapi2.h`:
```
_Win32_metadata_set_last_error_
WINBASEAPI
HANDLE
WINAPI
CreateConsoleScreenBuffer(
    _In_ DWORD dwDesiredAccess,
    _In_ DWORD dwShareMode,
    _In_opt_ CONST SECURITY_ATTRIBUTES* lpSecurityAttributes,
    _In_ DWORD dwFlags,
    _Reserved_ LPVOID lpScreenBufferData
    )
    _Win32_metadata_raii_free_(CloseHandle);
```
This header already carried the `win32metadata_annotations.h` guard from an
existing `consoleapi2.h.set-last-error.patch`. The former sidecar entry
`CreateConsoleScreenBuffer::return=[RAIIFree("CloseHandle")]` was removed
from `emitter.settings.rsp`. Consolidated the pre-existing set-last-error
changes and this new annotation into a single cumulative
`generation/WinSDK/patches/post-midl/consoleapi2.h.metadata.patch` against
the pristine `d154186c` SDK baseline; removed the now-superseded
`consoleapi2.h.set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `consoleapi2.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Console, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Note
`consoleapi2.h` is reached only via the `Console` partition's
`settings.rsp` `IncludeRoot`, not a `main.cpp` include, so it is not part of
the 1403-item `header-progress.json` ledger; this report is the sole
tracking artifact for this header.
