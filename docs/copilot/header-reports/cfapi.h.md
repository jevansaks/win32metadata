# cfapi.h — accepted-normalized

**Partition:** Cloudapi
**Size:** ~36046 bytes

## Analysis

`cfapi.h` (Cloud Filter API) declares numerous functions. Checked
`autoTypes.json` first: **`CF_CONNECTION_KEY`
(`DECLARE_OPAQUE_KEY( CF_CONNECTION_KEY )`, produced by
`CfConnectSyncRoot`) already has a complete entry**
(`ValueType: DECLARE_OPAQUE_KEY`, `CloseApi: CfDisconnectSyncRoot`,
`InvalidHandleValues: [-1, 0]`) — producer-site ownership metadata is
already correctly in place.

Other notable functions: `CfOpenFileWithOplock` outputs a generic
`PHANDLE ProtectedHandle` (blocker-class 2 — generic `HANDLE`, closed
via `CfReleaseProtectedHandle`/`CfCloseHandle`, but the parameter type
itself is the fully generic `HANDLE`, not a distinct typedef);
`CfGetTransferKey`/`CfReleaseTransferKey` operate on `CF_TRANSFER_KEY`
(`typedef LARGE_INTEGER CF_TRANSFER_KEY` — a plain value type, not an
owned resource, freed via ordinary function call semantics rather
than resource ownership). No gap found.

# cfapi.h — accepted-normalized

**Partition:** Cloudapi
**Size:** ~36046 bytes

## Analysis

`cfapi.h` (Cloud Filter API) declares numerous functions. Checked
`autoTypes.json` first: **`CF_CONNECTION_KEY`
(`DECLARE_OPAQUE_KEY( CF_CONNECTION_KEY )`, produced by
`CfConnectSyncRoot`) already has a complete entry**
(`ValueType: DECLARE_OPAQUE_KEY`, `CloseApi: CfDisconnectSyncRoot`,
`InvalidHandleValues: [-1, 0]`) — producer-site ownership metadata is
already correctly in place.

`CfOpenFileWithOplock` outputs a generic `PHANDLE ProtectedHandle`, released
via `CfCloseHandle`; the parameter type itself is the fully generic `HANDLE`,
not a distinct typedef, so it is annotated at the producer parameter, not the
typedef. `CfGetTransferKey`/`CfReleaseTransferKey` operate on `CF_TRANSFER_KEY`
(`typedef LARGE_INTEGER CF_TRANSFER_KEY` — a plain value type, not an owned
resource, freed via ordinary function call semantics rather than resource
ownership).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/cfapi.h`:
```
STDAPI
CfOpenFileWithOplock (
    _In_ LPCWSTR FilePath,
    _In_ CF_OPEN_FILE_FLAGS Flags,
    _Out_ PHANDLE ProtectedHandle
        _Win32_metadata_raii_free_(CfCloseHandle)
    );
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry `CfOpenFileWithOplock::ProtectedHandle=[RAIIFree("CfCloseHandle")]`
was removed from `emitter.settings.rsp`. Consolidated into a single cumulative
`generation/WinSDK/patches/post-midl/cfapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `cfapi.h.metadata.patch` against the `d154186c`
  baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Cloudapi, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Conclusion

Producer-site ownership metadata for both `CF_CONNECTION_KEY` (autoTypes.json)
and `CfOpenFileWithOplock` (inline annotation) is now in place; no sidecar
memberRemap entries for this header remain.
