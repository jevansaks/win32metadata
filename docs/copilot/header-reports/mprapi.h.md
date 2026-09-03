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
   precedent. **No existing inline `emitter.settings.rsp`
   annotation** — added
   `MprConfigServerConnect::phMprConfig=[RAIIFree("MprConfigServerDisconnect")]`.

Validated via `dotnet build generation/WinSDK -c Release
-p:ScanArch=x64 -t:ScrapeHeaders -p:PartitionFilter=RRas` → 0 errors.

## Conclusion

Producer-site fixes applied: new `MPR_SERVER_HANDLE` `autoTypes.json`
entry and new inline `RAIIFree` annotation for
`MprConfigServerConnect::phMprConfig`.
