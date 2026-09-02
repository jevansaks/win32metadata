# `DsGetDC.h` / `ActiveDirectory`

## Queue position

- Header: `/um/DsGetDC.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` to the exported `Ds*` APIs in this header.
- Added `_Win32_RAIIFree_(DsGetDcCloseW)` to the `RetGetDcContext` output handle from `DsGetDcOpenA/W`.

## Matched result

The x64, x86, and arm64 comparisons all report 91 classified declarations and
0 unresolved declarations.

## Accepted normalization

The `DS_*` constants are present natively but absent from the legacy
win32metadata package for this header slice, so they are classified as
`accepted:noMetadata`.
