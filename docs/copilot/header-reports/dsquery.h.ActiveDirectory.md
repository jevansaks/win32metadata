# `dsquery.h` / `ActiveDirectory`

## Queue position

- Header: `/um/dsquery.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Current patch

- Added the metadata annotation include.
- Added `_Win32_FlexibleArray_` to the trailing one-element arrays:
  - `DSQUERYPARAMS.aColumns`
  - `DSQUERYCLASSLIST.offsetClass`

## Matched result

The x64, x86, and arm64 comparisons all report 26 classified declarations and
0 unresolved declarations.

## Accepted normalization

The `CLSID_*`, `DSQPF_*`, `DSQPM_*`, and `DSCOLUMNPROP_*` constants are native
header constants that are not present in the legacy win32metadata package for
this header slice, so they are classified as `accepted:noMetadata`.

## Patch artifact

- `generation/WinSDK/patches/post-midl/dsquery.h.win32metadata.patch`
