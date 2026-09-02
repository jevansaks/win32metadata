# NtDsAPI.h / ActiveDirectory shift-left migration

## Result

`NtDsAPI.h` now compares with 0 unresolved deltas for `ActiveDirectory` on x64, x86, and arm64.

## Header patch

- Added `win32metadata_annotations.h`.
- Attached `_Windows_SupportedOS_WindowsVista_` to the `NTDSAPI` and `NTDSAPI_POSTXP` export macros so the exported API surface receives the legacy Vista support metadata without annotating each declaration individually.
- Added `_Windows_SupportedOS_WindowsVista_` directly to `DsQuerySitesFree`, which does not use the `NTDSAPI` macro.
- Added `_Win32_FlexibleArray_` to native one-element trailing arrays whose MIDL branch already declares `[size_is(...)]` flexible arrays:
  - `DS_REPL_NEIGHBORSW.rgNeighbor`
  - `DS_REPL_CURSORS.rgCursor`
  - `DS_REPL_CURSORS_2.rgCursor`
  - `DS_REPL_CURSORS_3W.rgCursor`
  - `DS_REPL_OBJ_META_DATA.rgMetaData`
  - `DS_REPL_OBJ_META_DATA_2.rgMetaData`
  - `DS_REPL_KCC_DSA_FAILURESW.rgDsaFailure`
  - `DS_REPL_PENDING_OPSW.rgPendingOp`
  - `DS_REPL_ATTR_VALUE_META_DATA.rgMetaData`
  - `DS_REPL_ATTR_VALUE_META_DATA_2.rgMetaData`
  - `DS_REPL_ATTR_VALUE_META_DATA_EXT.rgMetaData`

Durable patch artifact: `generation\WinSDK\patches\post-midl\NtDsAPI.h.win32metadata.patch`.

## Tooling changes required

- Collapse `RPC_AUTH_IDENTITY_HANDLE` to raw `void*` / `void**`, matching legacy win32metadata, rather than emitting a dangling local type.
- Preserve native const metadata for `LPCSTR*` / `LPCWSTR*` string-pointer arrays and `const PSCHEDULE` parameters.
- Treat inline callback function-pointer parameters as input `IntPtr`, matching the existing sidecar metadata for `DsReplicaSyncAllA/W`.
- Normalize non-semantic `[Optional][Reserved][Const]` vs `[Optional][Const][Reserved]` attribute ordering during comparison.

## Evidence

- x64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\NtDsAPI\ActiveDirectory\x64\comparison\comparison.md`
- x86: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\NtDsAPI\ActiveDirectory\x86\comparison\comparison.md`
- arm64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\NtDsAPI\ActiveDirectory\arm64\comparison\comparison.md`
