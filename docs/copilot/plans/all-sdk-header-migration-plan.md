# All-SDK shift-left metadata migration plan

This is now a full SDK migration, not a curated case-study backlog. The source of
truth is generated from the SDK headers in `generation/WinSDK/RecompiledIdlHeaders`
and the partition `<IncludeRoot>` declarations in `generation/WinSDK/Partitions`.

## Completion target

Every SDK header must end in one of these states:

| State | Meaning |
| --- | --- |
| `matched` | The shift-left windows-rs conversion matches existing win32metadata for all representable declarations, modulo accepted normalizations. |
| `classified` | The header generates and every delta has an owner: SDK patch, windows-rs fix, accepted normalization, or deferred CsWin32/projection work. |
| `blocked` | The header cannot be generated or compared yet, with a specific blocking toolchain or SDK issue recorded. |
| `dependency-only` | The header is not a direct partition root but is covered transitively by one or more partition-root headers. |
| `out-of-scope-empty` | The header contributes no winmd-representable declarations. |

## Work queue

The generated queue lives under `generation/WinSDK/patches/header-plan/`:

- `sdk-headers.csv` lists every recompiled SDK header.
- `partition-header-queue.csv` lists every partition/header root with its namespace and current status.
- `README.md` summarizes counts by area, status, and partition.

The direct queue is partition/header based because metadata identity is
`(header, partition)`, not just `header`. A header included in multiple partitions
must be generated and compared in each partition namespace. Headers that are not
direct roots are still part of the SDK plan; they are tracked as dependency-only or
promoted to explicit roots if they define declarations that otherwise escape review.

## Per-header loop

1. Generate the full header for x64 with `scripts/Invoke-ShiftLeftHeader.ps1 -FullHeader`.
2. Dump the reference, pre-existing windows-rs, and shift-left conversion surfaces with `scripts/Dump-ShiftLeftHeaderSurface.ps1`.
3. Classify presence first: no reference declaration may be missing from the shift-left conversion unless it is explicitly accepted.
4. Normalize only agreed non-functional differences: docs, Ansi/Unicode projection hints, pseudo-handle replacement with lifecycle metadata, scoped enum spelling, native ABI-preserving associated enums, dependency-only typedef wrappers, and anonymous nested-record names when layout is identical.
5. For real differences, prefer fixes in this order: standard SAL or corrected native type, custom win32metadata annotation, windows-rs parser/emitter fix, then accepted normalization.
6. Regenerate x64 and then smoke x86/arm64 for matched headers.
7. Regenerate the one-header patch in `generation/WinSDK/patches/post-midl/`.
8. Update the header report, machine-readable manifest, and consolidated final report.

## Global toolchain work discovered so far

- Header roots must be discovered by declaration location, not namespace membership.
- Partition `main.cpp` declarations are not header roots.
- The flat pre-existing windows-rs winmd must not be supplied while discovering shift-left roots because it can mask generated declarations.
- SAL parsing must recover nested `_When_` and `_At_` annotations.
- `_Reserved_` implies `OptionalAttribute` and must not infer an output direction.
- Compatibility output needs legacy unsigned macro-constant policy for non-negative untyped macros.
- Data-pointer typedefs, charset aliases, and secondary record aliases should not become public metadata roots.
- Existing win32metadata string const sidecars should become native `LPCSTR`/`LPCWSTR` declarations when that does not change ABI.
- Flexible array sidecars require explicit `_Win32_FlexibleArray_` because `[1]` cannot be changed without ABI impact.

## Current gates

- `winspool.h` / `Printing`: matched for metadata generation; CsWin32 SafeHandle behavior is deferred.
- `wingdi.h` / `Gdi`: the reviewed `FONT_RESOURCE_CHARACTERISTICS` unit is matched; the full header is classified and now drives the next patching pass.
- `AdsHlp.h` / `ActiveDirectory`: first full-queue run now compiles after a windows-rs scalar-collapse fix for direct `LARGE_INTEGER` field spellings. The remaining blocker is comparison infrastructure: generated RDL contains compile dependencies, so the next toolchain step is emitting an explicit declared-by-header root list from windows-rs and using that list for full-header comparisons.
