# `fileapi.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 10
- **Partitions:** Fs

## Ownership Analysis (sidecar-removal tranche update)
Nine find-first functions return/produce a search `HANDLE` directly or via
out-param, each released via its matching close API. Moved from
`emitter.settings.rsp` memberRemaps to inline ABI-neutral annotations
directly on the producer declarations in
`RecompiledIdlHeaders/um/fileapi.h`:
```
FindFirstChangeNotificationA(...) _Win32_metadata_raii_free_(FindCloseChangeNotification);
FindFirstChangeNotificationW(...) _Win32_metadata_raii_free_(FindCloseChangeNotification);
FindFirstFileA(...) _Win32_metadata_raii_free_(FindClose);
FindFirstFileW(...) _Win32_metadata_raii_free_(FindClose);
FindFirstFileExA(...) _Win32_metadata_raii_free_(FindClose);
FindFirstFileExW(...) _Win32_metadata_raii_free_(FindClose);
FindFirstVolumeW(...) _Win32_metadata_raii_free_(FindVolumeClose);
FindFirstStreamW(...) _Win32_metadata_raii_free_(FindClose);
FindFirstFileNameW(...) _Win32_metadata_raii_free_(FindClose);
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `fileapi.h.set-last-error.patch`. The unrelated
`FindFirstFileExA/W::dwAdditionalFlags=FIND_FIRST_EX_FLAGS` entries are
untouched; the transacted/mount-point variants
(`FindFirstFileTransactedA/W`, `FindFirstFileNameTransactedW`,
`FindFirstStreamTransactedW`, `FindFirstVolumeA`,
`FindFirstVolumeMountPointA/W`) live in `WinBase.h`, tracked separately. All
9 former sidecar entries were removed from `emitter.settings.rsp`.
Consolidated the pre-existing set-last-error changes and these new
annotations into a single cumulative
`generation/WinSDK/patches/post-midl/fileapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed the now-superseded
`fileapi.h.set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `fileapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Fs, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).
