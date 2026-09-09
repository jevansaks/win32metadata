# `winnetwk.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 8
- **Partitions:** Wnet

## Ownership Analysis (sidecar-removal tranche update)
`WNetOpenEnumA`/`WNetOpenEnumW(..., _Out_ LPHANDLE lphEnum)` produce a
`HANDLE` via a direct out-param, released via `WNetCloseEnum(HANDLE)`.
Moved from `emitter.settings.rsp` memberRemaps (added in an earlier batch)
to inline ABI-neutral annotations directly on the producer out-parameters
in `RecompiledIdlHeaders/um/winnetwk.h`:
```
WNetOpenEnumA(..., _Out_ LPHANDLE lphEnum
    _Win32_metadata_raii_free_(WNetCloseEnum));
WNetOpenEnumW(..., _Out_ LPHANDLE lphEnum
    _Win32_metadata_raii_free_(WNetCloseEnum));
```
The unrelated `WNetOpenEnumA::return=WIN32_ERROR`/`WNetOpenEnumW::return=WIN32_ERROR`
memberRemaps are untouched. This header already carried the
`win32metadata_annotations.h` guard from the existing
`winnetwk.h.set-last-error.patch`. Both former sidecar `RAIIFree` entries
were removed from `emitter.settings.rsp`. Consolidated the pre-existing
set-last-error changes and these new annotations into a single cumulative
`generation/WinSDK/patches/post-midl/winnetwk.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed the now-superseded
`winnetwk.h.set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `winnetwk.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Wnet, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).
