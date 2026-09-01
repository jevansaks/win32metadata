# `AdsProp.h` / `ActiveDirectory`

## Queue position

- Header: `/um/AdsProp.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` to the seven exported property-page helpers.
- Changed `ADsPropSetHwndWithTitle.ptzTitle` from `PTSTR` to `CHAR*` for metadata generation because the legacy reference projects the non-A/W API as `sbyte*`.
- Changed `ADsPropCheckIfWritable` inputs to native const pointee types so existing `[Const]` metadata comes from the header instead of sidecars.

## Matched result

The final x64 comparison reports 17 classified declarations and 0 unresolved
declarations. The x86 and arm64 smoke runs also match.

## Accepted normalization

The `WM_ADSPROP_NOTIFY_*` constants are native SDK constants absent from the
legacy win32metadata package. They are classified as `nativeOnlyConstants` rather
than removed from the SDK-derived output.
