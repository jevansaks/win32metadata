# `WinDNS.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 8
- **Partitions:** Dns, IpHlp

## Ownership Analysis (sidecar-removal tranche update)
`DnsAcquireContextHandle_A`/`DnsAcquireContextHandle_W(..., _Outptr_ PHANDLE
pContext)` produce a `HANDLE` via a direct out-param, released via
`DnsReleaseContextHandle(HANDLE)`. Moved from `emitter.settings.rsp`
memberRemaps (added in an earlier batch) to inline ABI-neutral annotations
directly on the producer out-parameters in
`RecompiledIdlHeaders/um/WinDNS.h`:
```
DnsAcquireContextHandle_W(
    ...,
    _Outptr_     PHANDLE         pContext
        _Win32_metadata_raii_free_(DnsReleaseContextHandle)
    );
DnsAcquireContextHandle_A(
    ...,
    _Outptr_     PHANDLE         pContext
        _Win32_metadata_raii_free_(DnsReleaseContextHandle)
    );
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `WinDNS.h.set-last-error.patch`. Both former sidecar entries were
removed from `emitter.settings.rsp`. Consolidated the pre-existing
set-last-error changes and these new annotations into a single cumulative
`generation/WinSDK/patches/post-midl/WinDNS.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed the now-superseded
`WinDNS.h.set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `WinDNS.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Dns, IpHlp, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s) (both scraped per-arch as x64/x86/arm64 automatically under
  crossarch).
