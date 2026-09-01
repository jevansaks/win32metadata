# `CmnQuery.h` / `ActiveDirectory`

## Queue position

- Header: `/um/CmnQuery.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` before the three COM interfaces (`IQueryForm`, `IPersistQuery`, `ICommonQuery`).
- Added `_Out_` to `ICommonQuery::OpenQueryWindow.ppDataObject`.

## Matched result

The x64, x86, and arm64 comparisons all report 31 classified declarations and
4 unresolved declarations.

## Resolved by tooling

- Preserved `LPARAM` and `WPARAM` for fields and callback/interface parameters named `lParam` and `wParam`, matching legacy win32metadata's semantic aliases instead of reducing these to raw pointer-sized integers.

## Remaining unresolved deltas

- `IQueryForm`, `IPersistQuery`, and `ICommonQuery` still miss `SupportedOSPlatform("windows6.0.6000")`. The annotation is present in the header patch, but clang does not attach the standalone annotation to the `DECLARE_INTERFACE_IID_` macro-authored record declaration. This should be fixed in the Rust annotation parser or by adding a supported type-annotation placement that works with COM interface macros.
- `OPENQUERYWINDOW` still differs only in the generated nested anonymous-union type name (`OPENQUERYWINDOW_6` vs `_Anonymous_e__Union`). The source RDL is already anonymous-field shaped; this is a Rust RDL-to-winmd naming compatibility gap.

## Accepted normalization

The GUID and `CQ*`/`OQWF*` constants are present natively but absent from the
legacy win32metadata package for this header slice, so they are classified as
`accepted:noMetadata`.
