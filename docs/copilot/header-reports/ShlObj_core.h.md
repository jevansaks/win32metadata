# `ShlObj_core.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 8
- **Partitions:** ActiveDirectory, Lwef, Properties, Shell

## Ownership Analysis (sidecar-removal tranche update)
`SHChangeNotification_Lock(...)` returns a `HANDLE` directly, released via
`SHChangeNotification_Unlock(HANDLE)`. Moved from `emitter.settings.rsp`
memberRemap (added in an earlier batch) to an inline ABI-neutral annotation
directly on the producer return declaration in
`RecompiledIdlHeaders/um/ShlObj_core.h`:
```
_Win32_metadata_supported_os_(windows5.0)
SHSTDAPI_(HANDLE) SHChangeNotification_Lock(_In_ HANDLE hChange, DWORD dwProcId, _Outptr_opt_result_buffer_(2) PIDLIST_ABSOLUTE **pppidl, _Out_opt_ LONG *plEvent)
    _Win32_metadata_raii_free_(SHChangeNotification_Unlock);
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `ShlObj_core.h.zzz-supported-os.patch`. The former sidecar entry
was removed from `emitter.settings.rsp`. Consolidated the pre-existing
supported-os changes and this new annotation into a single cumulative
`generation/WinSDK/patches/post-midl/ShlObj_core.h.metadata.patch` against
the pristine `d154186c` SDK baseline; removed the now-superseded
`ShlObj_core.h.zzz-supported-os.patch`.

## Validation
- Patch replay: `git apply` of `ShlObj_core.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (ActiveDirectory, Lwef, Properties, Shell,
  `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s) (Shell scraped
  per-arch as x64/x86/arm64 automatically under crossarch).
