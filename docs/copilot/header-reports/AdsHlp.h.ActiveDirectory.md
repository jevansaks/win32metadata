# `AdsHlp.h` / `ActiveDirectory`

## Queue position

`AdsHlp.h` is the first header in the generated all-SDK partition/header queue:

- Header: `/um/AdsHlp.h`
- Partition: `ActiveDirectory`
- Namespace: `Windows.Win32.Networking.ActiveDirectory`

## Generation result

The first run failed during RDL compilation because `LARGE_INTEGER` appeared as an
unqualified field type in generated RDL. The windows-rs scraper already collapsed
`LARGE_INTEGER` in typedef contexts, but not when clang surfaced it as a direct
record field spelling. The toolchain fix collapses semantic/fundamental scalar
record spellings before namespace resolution.

After that fix, `AdsHlp.h` generates a winmd successfully.

## Matched result

The final x64 comparison reports 22 classified declarations and 0 unresolved
declarations.

The root/dependency issue is solved by a two-pass full-header run:

| List | Meaning |
| --- | --- |
| root RDL | Declarations whose spelling/expansion location belongs to the requested header. |
| dependencies | Extra declarations emitted only so the header-root winmd compiles. |

Only root RDL symbols drive the fidelity comparison. Dependencies stay in the
compiled winmd but are excluded from the per-header completion gate.

## Changes required

- Added `_Windows_SupportedOS_WindowsVista_` to the 18 functions that carry
  `SupportedOSPlatform("windows6.0.6000")` in the reference metadata.
- Added `_Win32_SetLastError_` and `_Inout_` to `ADsGetLastError`.
- Added standard `_Out_` to `ADsBuildEnumerator.ppEnumVariant`.
- Added `_Win32_AssociatedEnum_(ADS_AUTHENTICATION_ENUM)` to
  `ADsOpenObject.dwReserved`.
- Excluded `ADsFreeAllErrorRecords` from the ActiveDirectory partition because it
  has no import library and is absent from the reference metadata.

## Accepted normalization

`ADsOpenObject.dwReserved` remains native `DWORD`/`uint` with
`AssociatedEnum("ADS_AUTHENTICATION_ENUM")` instead of changing the ABI type to the
enum.
