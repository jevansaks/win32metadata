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

## Conclusion

Clean. No patch required — existing `CF_CONNECTION_KEY` metadata
already correct and producer-site (not typedef-site).
