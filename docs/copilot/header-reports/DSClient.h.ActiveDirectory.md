# `DSClient.h` / `ActiveDirectory`

## Queue position

- Header: `/um/DSClient.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Windows_SupportedOS_WindowsVista_` to `DsBrowseForContainerA/W`, `DsGetIcon`, `DsGetFriendlyClassName`, `IDsBrowseDomainTree`, and `IDsDisplaySpecifier`.
- Added `_Win32_FlexibleArray_` to `DOMAIN_TREE.aDomains`, `DSCLASSCREATIONINFO.aWizardExtensions`, and `DSOBJECTNAMES.aObjects`.
- Added `_Win32_CanonicalName_(DOMAINDESC)` to the duplicate `DOMAIN_DESC` typedef.
- Guarded the duplicate `DOMAINTREE` typedef out of WIN32METADATA generation so the metadata surface keeps only `DOMAIN_TREE`.
- Changed `IDsBrowseDomainTree::{GetDomains,FreeDomains}` to spell the canonical `DOMAIN_TREE**` type in the metadata build.

## Matched result

The x64, x86, and arm64 comparisons all report 82 classified declarations and
2 unresolved declarations.

## Resolved by tooling

- Fixed canonical-name handling so renamed struct typedefs emit under the canonical metadata name.
- Suppressed self-alias typedefs such as `type DOMAIN_TREE = DOMAIN_TREE`.
- Preserved fixed `CHAR[N]` buffers as `CHAR[]` while still allowing `CHAR*` parameters to project as primitive `sbyte*`.
- Reused the `LPARAM` semantic-alias rule introduced by `CmnQuery.h`.

## Remaining unresolved deltas

`IDsBrowseDomainTree` and `IDsDisplaySpecifier` have matching method shapes,
but still miss `SupportedOSPlatform("windows6.0.6000")` because clang/Rust does
not yet attach standalone supported-OS annotations to `DECLARE_INTERFACE_IID_`
macro-authored COM interface declarations.

## Accepted normalization

The CLSID/IID and `DS*` constants are present natively but absent from the
legacy win32metadata package for this header slice, so they are classified as
`accepted:noMetadata`.
