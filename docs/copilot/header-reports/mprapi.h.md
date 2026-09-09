# mprapi.h — accepted-normalized (2 producer-site fixes applied)

**Partition:** RRas
**Size:** ~102003 bytes

## Analysis

`mprapi.h` (Router Management API) declares two genuine handle
patterns:

1. **`MPR_SERVER_HANDLE`** (`typedef HANDLE MPR_SERVER_HANDLE;` —
   distinct named typedef) — produced by `MprAdminServerConnect`
   (`_Out_ MPR_SERVER_HANDLE*`), destroyed by single-arg
   `MprAdminServerDisconnect(MPR_SERVER_HANDLE)`. **No `autoTypes.json`
   entry existed** — added new entry (`ValueType: DECLARE_HANDLE`,
   `CloseApi: MprAdminServerDisconnect`, `InvalidHandleValues: [0]`).

2. **`MprConfigServerConnect`** outputs a generic `_Out_ HANDLE*
   phMprConfig` (not a distinct typedef) — a semantically specific
   "MPR config handle", closed via single-arg
   `MprConfigServerDisconnect(HANDLE)`. Directly analogous to the
   `CfOpenFileWithOplock`/`QOSCreateHandle`/`MQCreateCursor`
   precedent.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/Mprapi.h`:
```
DWORD APIENTRY
MprConfigServerConnect(
    _In_opt_  LPWSTR                  lpwsServerName,
    _Out_     HANDLE*                 phMprConfig
        _Win32_metadata_raii_free_(MprConfigServerDisconnect)
);
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry
`MprConfigServerConnect::phMprConfig=[RAIIFree("MprConfigServerDisconnect")]`
was removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/Mprapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline. `MPR_SERVER_HANDLE` remains a separate
type-level `autoTypes.json` entry, unaffected by this change.

## Validation
- Patch replay: `git apply` of `Mprapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (RRas, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s)
  (scraped per-arch as x64/x86/arm64 automatically under crossarch).

## Conclusion

Producer-site fixes applied: new `MPR_SERVER_HANDLE` `autoTypes.json`
entry and new inline `RAIIFree` annotation for
`MprConfigServerConnect::phMprConfig`.
