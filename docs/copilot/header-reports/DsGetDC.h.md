# DsGetDC.h — accepted-normalized

**Partition:** ActiveDirectory
**Size:** ~18792 bytes

## Analysis

`DsGetDC.h` (`DsGetDcName` API) declares
`DsGetDcNameA(..., _Outptr_result_nullonfailure_
PDOMAIN_CONTROLLER_INFOA *DomainControllerInfo)`. The output is a
transparent struct freed via `NetApiBufferFree` (documented
convention) — matches the established Net API buffer-allocation
pattern (blocker-class 7).

`DsGetDcOpenA`/`DsGetDcOpenW(..., _Out_ PHANDLE RetGetDcContext)` produce a
generic `HANDLE` via a direct out-param, released via
`DsGetDcCloseW(HANDLE)` (a single close API handles both variants).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/DsGetDC.h`:
```
DsGetDcOpenW(
    ...,
    _Out_ PHANDLE RetGetDcContext
        _Win32_metadata_raii_free_(DsGetDcCloseW)
    );
DsGetDcOpenA(
    ...,
    _Out_ PHANDLE RetGetDcContext
        _Win32_metadata_raii_free_(DsGetDcCloseW)
    );
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). Both former
sidecar entries were removed from `emitter.settings.rsp`. Consolidated into
a single new `generation/WinSDK/patches/post-midl/DsGetDC.h.metadata.patch`
against the pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `DsGetDC.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (ActiveDirectory, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Conclusion

`DsGetDcNameA`'s Net API buffer output remains clean/no-patch-needed.
`DsGetDcOpenA`/`DsGetDcOpenW` now carry inline `_Win32_metadata_raii_free_`
annotations directly on their out-parameters; no sidecar entries remain.
