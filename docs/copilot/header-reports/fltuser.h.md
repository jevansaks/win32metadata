# fltuser.h — accepted-normalized

**Partition:** Ifsk
**Size:** ~11456 bytes

## Analysis

`fltuser.h` declares the Filter Manager user-mode API
(`FilterConnectCommunicationPort`, `FilterFindFirst`,
`FilterVolumeFindFirst`, `FilterInstanceFindFirst`, etc.). All handle
outputs use the **generic, shared `HANDLE` type**
(`_Outptr_ HANDLE *hPort`, `_Out_ LPHANDLE lpFilterFind`,
`_Out_ PHANDLE lpVolumeFind`, `_Out_ LPHANDLE lpFilterInstanceFind`,
`_Out_ LPHANDLE lpVolumeInstanceFind`) — no distinctly-named handle
typedef is declared anywhere in this header (confirmed via grep for
`DECLARE_HANDLE`/`typedef HANDLE`: zero matches).

## Correction to prior investigation
This report previously concluded the header was "not fixable" because the
handle-producing functions output the generic, shared `HANDLE` type. That is
incorrect: the `Function::Parameter=[RAIIFree(...)]` mechanism is scoped to
the named function and parameter only, not to `HANDLE` globally (68+
existing precedents). All four `FilterFindFirst`/`FilterVolumeFindFirst`/
`FilterInstanceFindFirst`/`FilterVolumeInstanceFindFirst` out-params were
subsequently fixed via sidecar entries and are now migrated to inline
annotations below, each released via its matching single-arg
`*FindClose`/`*Close` function.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/fltUser.h`:
```
FilterFindFirst(..., _Out_ LPHANDLE lpFilterFind
    _Win32_metadata_raii_free_(FilterFindClose));
FilterVolumeFindFirst(..., _Out_ PHANDLE lpVolumeFind
    _Win32_metadata_raii_free_(FilterVolumeFindClose));
FilterInstanceFindFirst(..., _Out_ LPHANDLE lpFilterInstanceFind
    _Win32_metadata_raii_free_(FilterInstanceFindClose));
FilterVolumeInstanceFindFirst(..., _Out_ LPHANDLE lpVolumeInstanceFind
    _Win32_metadata_raii_free_(FilterVolumeInstanceFindClose));
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). All four
former sidecar entries were removed from `emitter.settings.rsp`.
Consolidated into a single new
`generation/WinSDK/patches/post-midl/fltUser.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `fltUser.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Ifsk, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Conclusion

All four find-first functions now carry inline `_Win32_metadata_raii_free_`
annotations directly on their out-parameters; no sidecar entries remain for
this header.
