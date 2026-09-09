# winfax.h — accepted-normalized (producer-site fix applied)

**Partition:** Fax
**Size:** ~65135 bytes

## Analysis

`winfax.h` (Windows Fax API) declares `FaxConnectFaxServerA/W`
(`_Out_ LPHANDLE FaxHandle`) and `FaxOpenPort` (`_Out_ LPHANDLE
FaxPortHandle`) — both output the generic `HANDLE` type directly (not
distinct typedefs) — closed via the single generic `FaxClose(HANDLE)`
(confirmed no dedicated `FaxClosePort`; `FaxClose` documented to
close both server and port handles).

Checked `emitter.settings.rsp` first: **no existing inline
annotations for any of the three functions** — a genuine gap. Added
`FaxConnectFaxServerA/W::FaxHandle=[RAIIFree("FaxClose")]` and
`FaxOpenPort::FaxPortHandle=[RAIIFree("FaxClose")]`, consistent with
the `CfOpenFileWithOplock`/`QOSCreateHandle` precedent for
semantically-specific generic-`HANDLE` outputs.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/WinFax.h`:
```
FaxConnectFaxServerA(
    IN  LPCSTR MachineName OPTIONAL,
    OUT LPHANDLE FaxHandle
        _Win32_metadata_raii_free_(FaxClose)
    );
FaxConnectFaxServerW(
    IN  LPCWSTR MachineName OPTIONAL,
    OUT LPHANDLE FaxHandle
        _Win32_metadata_raii_free_(FaxClose)
    );
FaxOpenPort(
    IN  HANDLE FaxHandle,
    IN  DWORD DeviceId,
    IN  DWORD Flags,
    OUT LPHANDLE FaxPortHandle
        _Win32_metadata_raii_free_(FaxClose)
    );
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). All three
former sidecar entries were removed from `emitter.settings.rsp`.
Consolidated into a single new
`generation/WinSDK/patches/post-midl/WinFax.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `WinFax.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Fax, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Conclusion

Producer-site fix applied — 3 inline `_Win32_metadata_raii_free_`
parameter annotations now live directly on the declarations in
`RecompiledIdlHeaders/um/WinFax.h`; no sidecar entries remain.
