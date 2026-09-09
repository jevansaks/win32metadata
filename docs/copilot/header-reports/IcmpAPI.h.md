# `IcmpAPI.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 7
- **Partitions:** IpHlp

## Ownership Analysis (sidecar-removal tranche update)
`IcmpCreateFile()`/`Icmp6CreateFile()` return a generic `HANDLE` directly,
released via `IcmpCloseHandle(HANDLE)`. Moved from `emitter.settings.rsp`
memberRemaps (added in an earlier batch) to inline ABI-neutral annotations
directly on the producer return declarations in
`RecompiledIdlHeaders/um/IcmpAPI.h`:
```
IPHLPAPI_DLL_LINKAGE
HANDLE
WINAPI
IcmpCreateFile(
    VOID
    )
    _Win32_metadata_raii_free_(IcmpCloseHandle);

IPHLPAPI_DLL_LINKAGE
HANDLE
WINAPI
Icmp6CreateFile(
    VOID
    )
    _Win32_metadata_raii_free_(IcmpCloseHandle);
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `IcmpAPI.h.set-last-error.patch`. Both former sidecar entries were
removed from `emitter.settings.rsp`. Consolidated the pre-existing
set-last-error changes and these new annotations into a single cumulative
`generation/WinSDK/patches/post-midl/IcmpAPI.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed the now-superseded
`IcmpAPI.h.set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `IcmpAPI.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (IpHlp, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s) (scraped per-arch as x64/x86/arm64 automatically under
  crossarch).
