# `DSAdmin.h` / `ActiveDirectory`

## Queue position

- Header: `/um/DSAdmin.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` before the five COM interfaces.
- Added `_Out_` to `IDsAdminCreateObj::CreateModal.ppADsObj`.
- Changed `IDsAdminNewObjExt::AddPages.lpfnAddPage` from `LPFNADDPROPSHEETPAGE` to the existing SDK alias `LPFNSVADDPROPSHEETPAGE` to match legacy metadata without changing ABI.

## Matched result

The x64, x86, and arm64 comparisons all report 18 classified declarations and
5 unresolved declarations.

## Resolved by tooling

- Preserved `BSTR*` parameters as `BSTR*` rather than reducing them to `ushort**`.
- Reused the `LPARAM` semantic-alias rule introduced by `CmnQuery.h`.

## Remaining unresolved deltas

The five unresolved items are `IDsAdminCreateObj`, `IDsAdminNewObj`,
`IDsAdminNewObjExt`, `IDsAdminNewObjPrimarySite`, and
`IDsAdminNotifyHandler`. Their method shapes now match, but
`SupportedOSPlatform("windows6.0.6000")` is still missing for the same
`DECLARE_INTERFACE_` macro-authored COM interface annotation gap observed in
`CmnQuery.h`.

## Accepted normalization

The CLSID/IID and `DSA_*` constants are present natively but absent from the
legacy win32metadata package for this header slice, so they are classified as
`accepted:noMetadata`.
