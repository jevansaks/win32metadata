# `dsparse.h` / `ActiveDirectory`

## Queue position

- Header: `/um/dsparse.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` to the public APIs that carry Vista support metadata in the legacy win32metadata package.
- Changed `DsCrackSpn3W.pPortNumber` from legacy `OUT` spelling to `_Inout_`, matching the documented in/out behavior and legacy metadata.

## Tooling changes required

- Added `LPCH/PCH/LPCCH/PCCH/LPWCH/PWCH/LPCWCH/PCWCH` to the windows-rs string-alias policy so counted-character SDK aliases preserve `PSTR/PWSTR` metadata shape instead of degrading to primitive pointers.
- Inferred `[NotNullTerminated]` for direct narrow counted-character buffers (`LPCCH/PCCH/LPCH/PCH`) and direct mutable wide counted-character output buffers (`LPWCH/PWCH`). This matches the legacy metadata for the RDN quote/unquote APIs while preserving the legacy absence of `[NotNullTerminated]` on the wide input counted buffers.
- Normalized comparison-only ordering for `[Const]`, `[NotNullTerminated]`, and `[NativeArrayInfo]`.

## Matched result

The x64, x86, and arm64 comparisons all report 20 classified declarations and
0 unresolved declarations.

## Patch artifact

- `generation/WinSDK/patches/post-midl/dsparse.h.win32metadata.patch`
