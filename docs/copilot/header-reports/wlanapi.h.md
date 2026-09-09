# `wlanapi.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 8
- **Partitions:** Ndis, NWifi

## Ownership Analysis (sidecar-removal tranche update)
`WlanOpenHandle(..., _Out_ PHANDLE phClientHandle)` produces a `HANDLE` via
a direct out-param, released via `WlanCloseHandle(HANDLE)`. Moved from
`emitter.settings.rsp` memberRemap (added in an earlier batch) to an inline
ABI-neutral annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/wlanapi.h`:
```
WlanOpenHandle(
    _In_ DWORD dwClientVersion,
    _Reserved_ PVOID pReserved,
    _Out_ PDWORD pdwNegotiatedVersion,
    _Out_ PHANDLE phClientHandle
        _Win32_metadata_raii_free_(WlanCloseHandle)
);
```
This header already carried the `win32metadata_annotations.h` guard from the
existing `wlanapi.h.set-last-error.patch`. The former sidecar entry was
removed from `emitter.settings.rsp`. Consolidated the pre-existing
set-last-error changes and this new annotation into a single cumulative
`generation/WinSDK/patches/post-midl/wlanapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed the now-superseded
`wlanapi.h.set-last-error.patch`.

## Validation
- Patch replay: `git apply` of `wlanapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Ndis, NWifi, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).
