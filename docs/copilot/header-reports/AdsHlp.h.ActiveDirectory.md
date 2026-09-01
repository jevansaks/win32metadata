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

## Current blocker

The generated `AdsHlp.rdl` contains compile dependencies from related Active
Directory headers, such as `ADSVALUE` from `Iads.h`. This is required so the
single-header winmd compiles, but it means an RDL-only symbol inventory is not the
same as the declared-by-`AdsHlp.h` root surface.

The all-SDK loop needs windows-rs to emit an explicit root list while generating:

| List | Meaning |
| --- | --- |
| header roots | Declarations whose spelling/expansion location belongs to the requested header. |
| dependencies | Extra declarations emitted only so the header-root winmd compiles. |

Only header roots should drive the fidelity comparison. Dependencies should be
present for compilation but excluded from the per-header completion gate.

## Next action

Add root-list output to the windows-rs header-filter path and update
`Dump-ShiftLeftHeaderSurface.ps1` / `Compare-ShiftLeftHeader.ps1` to consume it.
