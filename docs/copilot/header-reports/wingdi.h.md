# `wingdi.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 8
- **Partitions:** DataXchg, Direct3D9, Gdi, Intl, Media.DShow, Tablet, Wcs

## Ownership Analysis (sidecar-removal tranche update)
`CreateMetaFileA`/`CreateMetaFileW(...)` return an `HDC` directly, released
via `DeleteMetaFile(HDC)`; `CreateEnhMetaFileA`/`CreateEnhMetaFileW(...)`
return an `HDC` directly, released via `DeleteEnhMetaFile(HDC)`. Moved from
`emitter.settings.rsp` memberRemaps to inline ABI-neutral annotations
directly on the producer return declarations in
`RecompiledIdlHeaders/um/wingdi.h`:
```
WINGDIAPI HDC     WINAPI CreateMetaFileA( _In_opt_ LPCSTR pszFile)
    _Win32_metadata_raii_free_(DeleteMetaFile);
WINGDIAPI HDC     WINAPI CreateMetaFileW( _In_opt_ LPCWSTR pszFile)
    _Win32_metadata_raii_free_(DeleteMetaFile);
...
WINGDIAPI HDC   WINAPI CreateEnhMetaFileA( _In_opt_ HDC hdc, _In_opt_ LPCSTR lpFilename, _In_opt_ CONST RECT *lprc, _In_opt_ LPCSTR lpDesc)
    _Win32_metadata_raii_free_(DeleteEnhMetaFile);
WINGDIAPI HDC   WINAPI CreateEnhMetaFileW( _In_opt_ HDC hdc, _In_opt_ LPCWSTR lpFilename, _In_opt_ CONST RECT *lprc, _In_opt_ LPCWSTR lpDesc)
    _Win32_metadata_raii_free_(DeleteEnhMetaFile);
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `wingdi.h.callback-canonical-name.patch`. All four former sidecar
entries were removed from `emitter.settings.rsp`. Consolidated the
pre-existing canonical-name change and these new annotations into a single
cumulative `generation/WinSDK/patches/post-midl/wingdi.h.metadata.patch`
against the pristine `d154186c` SDK baseline; removed the now-superseded
`wingdi.h.callback-canonical-name.patch`.

## Validation
- Patch replay: `git apply` of `wingdi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (DataXchg, Direct3D9, Gdi, Intl, Media.DShow, Tablet, Wcs,
  `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s) (Direct3D9 scraped
  per-arch as x64/x86/arm64 automatically under crossarch).
