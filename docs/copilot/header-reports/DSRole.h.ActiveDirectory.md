# `DSRole.h` / `ActiveDirectory`

## Queue position

- Header: `/um/DSRole.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` to:
  - `DsRoleGetPrimaryDomainInformation`
  - `DsRoleFreeMemory`
- Changed metadata-only SAL on `DsRoleGetPrimaryDomainInformation` to match legacy win32metadata:
  - `lpServer` is `_In_ LPCWSTR` rather than optional.
  - `Buffer` is `_Inout_ PBYTE *`.
- Changed `DsRoleFreeMemory.Buffer` to `_Inout_ PVOID`.

## Matched result

The x64, x86, and arm64 comparisons all report 14 classified declarations and
0 unresolved declarations.

## Accepted normalization

The `DSROLE_PRIMARY_*` and `DSROLE_UPGRADE_*` constants are native header
constants that are not present in the legacy win32metadata package for this
header slice, so they are classified as `accepted:noMetadata`.

## Patch artifact

- `generation/WinSDK/patches/post-midl/DSRole.h.win32metadata.patch`
