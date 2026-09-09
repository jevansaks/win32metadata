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

## Ownership Analysis (WithSetLastError.rsp final tranche update)
`CreateDIBSection` sets last error on failure per its `_Success_(return != NULL)`
contract; migrated the sidecar `WithSetLastError.rsp` fact to an inline
`_Win32_metadata_set_last_error_` annotation immediately above its declaration:
```
_Win32_metadata_set_last_error_
WINGDIAPI _Success_(return != NULL) HBITMAP WINAPI CreateDIBSection(
```
This was one of the last 2 genuinely-uncovered entries surviving a fresh
coverage re-check of the 143 previously-reported residual `WithSetLastError.rsp`
entries (141 of the 143 were already covered by pre-existing annotations from
the prior migration batch and were removed from the rsp as pure reconciliation
with no header changes). Re-consolidated into the same
`wingdi.h.metadata.patch` against the `d154186c` baseline.

## Validation
- Patch replay: `git apply` of `wingdi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (DataXchg, Direct3D9, Gdi, Intl, Media.DShow, Tablet, Wcs,
  `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s) (Direct3D9 scraped
  per-arch as x64/x86/arm64 automatically under crossarch).
