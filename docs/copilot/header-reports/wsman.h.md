# wsman.h — accepted-normalized (3 producer-site fixes applied; 2 items blocked)

**Partition:** WinRm
**Size:** ~71794 bytes

## Analysis

`wsman.h` (WS-Management API) declares a family of opaque
`typedef struct X *X_HANDLE` handle types, all with pre-existing
`autoTypes.json` entries (`ValueType: "typedef struct ..."`) but
**all missing `CloseApi`**:

**Producer-site fixes applied** (2-arg close: handle + `DWORD flags`
only, confirmed no `async` parameter):
- `WSMAN_API_HANDLE` → produced by `WSManInitialize`, destroyed by
  `WSManDeinitialize(WSMAN_API_HANDLE, DWORD flags)`.
- `WSMAN_SESSION_HANDLE` → produced by `WSManCreateSession`,
  destroyed by `WSManCloseSession(WSMAN_SESSION_HANDLE, DWORD flags)`.
- `WSMAN_OPERATION_HANDLE` → produced by `WSManSignalShell`/
  `WSManReceiveShellOutput`/`WSManSendShellInput`, destroyed by
  `WSManCloseOperation(WSMAN_OPERATION_HANDLE, DWORD flags)`.

Filled in the missing `CloseApi` for all three. Validated via `dotnet
build generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=WinRm` → 0 errors.

**Blocked (not representable)**:
- `WSMAN_COMMAND_HANDLE` → its only destroyer,
  `WSManCloseCommand(WSMAN_COMMAND_HANDLE, DWORD flags, _In_
  WSMAN_SHELL_ASYNC *async)`, requires a **mandatory third parameter**
  (an async-completion callback struct, not a reserved placeholder) —
  genuinely unrepresentable via a 2-arg `CloseApi` convention.
- `WSMAN_SHELL_HANDLE` → same issue: `WSManCloseShell(WSMAN_SHELL_HANDLE,
  DWORD flags, _In_ WSMAN_SHELL_ASYNC *async)` also requires the
  mandatory async struct.

## Conclusion

Producer-site fixes applied for `WSMAN_API_HANDLE`/
`WSMAN_SESSION_HANDLE`/`WSMAN_OPERATION_HANDLE`. `WSMAN_COMMAND_HANDLE`/
`WSMAN_SHELL_HANDLE` remain out of scope (mandatory async-struct
parameter beyond flags, not a simple reserved-flags case).
