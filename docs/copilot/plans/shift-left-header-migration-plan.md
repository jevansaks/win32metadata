# Shift-Left Header Migration Plan

## Goal

Replace win32metadata sidecars with SDK-header semantics, generate the canonical
`Windows.Win32.winmd` with windows-rs, and preserve projection behavior in CsWin32.
The work proceeds one SDK header at a time so each patch and every accepted difference
is reviewable.

## Unit of work

The patch unit is one SDK header:

```text
generation/WinSDK/patches/post-midl/<header>.win32metadata.patch
```

A header may contribute declarations to more than one win32metadata partition. In that
case the same patch is validated as separate `(header, partition)` work units because
each partition supplies a different namespace, traversal scope, and exclusion set.

Every work unit advances through these states:

1. `queued`: header and owning partition are known.
2. `patching`: sidecars and known fidelity gaps are being moved or classified.
3. `generated`: the patched header successfully produced header-scoped RDL and winmd.
4. `classified`: every delta is categorized as required, accepted, tooling, projection,
   legacy/reference-only, or documentation-only.
5. `matched`: no unresolved functional delta remains.
6. `blocked`: a named tooling or policy decision prevents completion.

`generation/WinSDK/patches/header-progress.json` is the machine-readable source of
truth. Each run writes disposable evidence under
`artifacts/shift-left-headers/<header>/<partition>/`; the reviewed summary is committed
under `docs/copilot/header-reports/`.

## Header loop

For each `(header, partition)` unit:

1. Start from the pristine tracked SDK header.
2. Inventory sidecars that affect declarations owned by the header:
   `autoTypes.json`, `enums.json`, `emitter.settings.rsp`, `WithSetLastError.rsp`,
   library mappings, supported OS, documentation, and callback fixups.
3. Add only reviewed source semantics:
   existing SAL/MIDL/C++ first, then the approved `win32metadata_annotations.h`
   vocabulary.
4. Regenerate the single `<header>.win32metadata.patch`.
5. Run windows-rs with:
   - `WIN32METADATA_ROOT` pointing at this repository;
   - `WIN32METADATA_PARTITION_FILTER` selecting the namespace owner;
   - `WIN32METADATA_HEADER_FILTER` selecting one defining header;
   - one architecture for iteration, then x64/x86/arm64 before `matched`.
   During early iteration, `targetSymbols` limits emission to the reviewed declarations
   and their dependencies. Before a unit reaches `matched`, expand the inventory to
   every declaration owned by that `(header, partition)` unit by passing `-FullHeader`
   to `Invoke-ShiftLeftHeader.ps1`.
   A header-focused run selects declarations by defining-header location, excludes
   declarations written directly in the partition `main.cpp`, and does not force every
   type already present in the output namespace into the root set. Reference winmds may
   satisfy dependencies, but the legacy flat windows-rs `Windows.Win32.winmd` must not
   be supplied while discovering roots because it can mask selected declarations as
   external references.
6. Compare header-scoped RDL and winmd declarations with the NuGet reference.
7. Apply agreed normalization rules, then classify every remaining difference.
8. Add or update windows-rs parser/RDL tests and CsWin32 projection tests when the
   header exposes a new semantic.
9. Update the progress manifest and commit the reviewed report.

## Matching policy

An exact metadata byte or signature match is not required when the reviewed source
model intentionally improves the contract. A work unit is `matched` only when its
projection-relevant behavior is equivalent after these explicit normalizations:

- A reference pseudo handle may match raw `HANDLE` when producer return/output
  metadata carries the cleanup function and invalid values. Consumer parameters remain
  borrowed raw handles.
- A reference enum-typed parameter may match its native integer type plus
  `AssociatedEnum`. The SDK ABI declaration remains the native integer type while
  projections may expose the enum.
- Removed `Ansi`, `Unicode`, documentation, and unused sidecar attributes do not count
  as functional gaps.
- Direct windows-rs fixes that are more faithful to C/C++ semantics are retained:
  native constness, alignment, raw success values, and correct architecture-dependent
  typedef widths.
- Native C typedef aliases are not automatically public metadata types. Pointer aliases,
  charset-selection aliases, and other ABI-equivalent wrappers collapse unless the
  reference metadata or a projection contract demonstrates a distinct public type.
- Constants are compared by name, value, and projected storage type. The legacy
  win32metadata scraper projects non-negative untyped integer macros as unsigned; this
  remains a functional compatibility requirement until an explicit projection review
  approves compiler-signed integer typing.
- Native constants present in the selected header but absent from the NuGet reference are
  tracked as additions, not regressions. They still require review for namespace and type.
- `NativeEncoding` on string constants is ignored only after confirming it does not alter
  CsWin32 output; until then it remains an explicitly classified metadata addition.
- Namespace differences are not accepted. The windows-rs partition configuration must
  emit the same namespace selected by the current win32metadata partition.
- Calling convention, import library/entry point, parameter direction and optionality,
  pointer depth, buffer relationships, layout, enum values, ownership, invalid values,
  retval selection, and exact-result preservation are functional.

Any new normalization rule requires a concrete example, a recorded decision in
`header-progress.json`, and a projection test where generated source behavior changes.

## Calibration rules from `winspool.h`

The first complete header inventory established the reusable comparison procedure:

1. Build the root inventory from declarations whose spelling or macro expansion belongs
   to the selected header. Do not infer ownership from namespace membership.
2. Keep dependency closure separate from roots. A dependency may be emitted so the winmd
   compiles, but it is not charged to the header unless the header defines it.
3. Generate without the flat windows-rs reference when measuring roots; otherwise selected
   declarations may resolve from that reference and appear missing.
4. Compare presence first, then compare normalized declaration details. For `winspool.h`,
   all 160 functions, 93 structs, and 6 enums were present; the apparent missing APIs in
   the original report were an isolation error.
5. Treat standard SAL as source truth. Fix nested `_When_`, `_At_`, and count-expression
   parsing in windows-rs rather than duplicating those contracts with custom annotations.
6. Preserve Clang constness through RDL and winmd for parameters and fields.
7. Add custom annotations only for semantics not represented by standard SAL: lifecycle,
   invalid values, associated enums, struct-size fields, last-error behavior, and similar
   approved vocabulary.
8. For handle normalization, consumers use borrowed raw `HANDLE`; producer returns and out
   parameters carry the exact cleanup function and every invalid value.
9. Compare enum underlying width, members, and values. Compare lifecycle payloads exactly.
10. Run a CsWin32 generated-source baseline before accepting any metadata normalization as
    projection-equivalent.

The corrected `winspool.h` root inventory is 880 declarations: 160 functions, 93 structs,
6 enums, 329 constants, 0 interfaces, and 292 C typedef aliases. Excluding aliases, the
representable native surface is 588 declarations. The NuGet reference contains 569 because
19 native constants were not emitted by the legacy toolchain.

## Initial ordered header backlog

The initial order deliberately starts with small policy-defining examples, then expands
to parser and namespace stress cases:

| Order | Header | First partition | Primary gap |
| ---: | --- | --- | --- |
| 1 | `winspool.h` | `Printing` | raw handle ownership and invalid values |
| 2 | `wingdi.h` | `Gdi` | associated enum without changing the native parameter |
| 3 | `webservices.h` | `Wsw` | SAL direction, names, and retained parameters |
| 4 | `esent.h` | `Storage.Jet` | architecture-neutral integer-width policy |
| 5 | `d2d1_1.h` | `Direct2D` | cross-partition owner suppression |
| 6 | `psapi.h` | `PsApi2` | aliases and public P/Invoke naming |
| 7 | `dxgi1_2.h` | `Dxgi` | retval and COM output semantics |
| 8 | `ole2.h` | `ComOle` | exact successful result preservation |
| 9 | `http.h` | `HttpServer` | retained buffers and byte counts |
| 10 | `ras.h` | `RRas` | simultaneous byte-capacity and element-count SAL |
| 11 | `sspi.h` | `Identity` | native result preservation and security handles |
| 12 | `errhandlingapi.h` | `Debug` | callback aliases and last-error behavior |
| 13 | `icucommon.h` | `Intl` | callback pointer direction/depth |
| 14 | `cfgmgr32.h` | `DevInst` | struct-size metadata decision |
| 15 | `winsvc.h` | `Services` | guarded enums and repeated loose values |
| 16 | `mmsystem.h` | `Multimedia` | binary buffers represented as string pointers |
| 17 | `fileapi.h` | `Fs` | existing double-NUL SAL propagation |
| 18 | `objidl.h` | `Com` | generated MIDL comments, retval, and agility |
| 19 | `oaidl.h` | `Automation` | automation aliases and MIDL ownership |
| 20 | `winuser.h` | `MenuRc` | high-volume enums, callbacks, and last-error data |

The manifest may add another partition for one of these headers when the same defining
header intentionally contributes declarations to multiple namespaces.

## Repository deliverables

### win32metadata

- Patch ingestion infrastructure.
- One patch file per SDK header.
- Approved annotation header.
- Header progress manifest and per-header reports.
- Header-scoped generation/comparison driver.
- Removal of migrated sidecars after equivalence is established.

### windows-rs

- Parser support and validation for the approved annotations.
- Header-scoped iteration support.
- Partition/namespace configuration aligned with win32metadata.
- RDL and winmd emission for ownership, invalid values, associated enums, retval,
  retained inputs, supported OS, last error, import overrides, agility, and exact-result
  preservation.
- Generator fixes for SAL defaults/capture, constness, aliases, pointer depth,
  architecture merging, owner suppression, and deterministic parallel completion.
- Unit fixtures plus per-header and whole-surface regeneration tests.

### CsWin32

- Treat raw `HANDLE` producer returns/output parameters carrying `RAIIFree` and
  `InvalidHandleValue` as SafeHandle-producing APIs.
- Continue projecting `AssociatedEnum` parameters as friendly enums while preserving
  the native extern signature.
- Preserve retval, retained input, exact-result, buffer, and COM-output behavior.
- Add source-generation baselines for every accepted metadata normalization.

## Completion gates

1. Every manifest unit is `matched` or has an explicitly approved nonfunctional delta.
2. Every patch reapplies to a clean SDK ingestion.
3. Normal header compilation and `WIN32METADATA` compilation both succeed.
4. x64, x86, and arm64 generation complete deterministically.
5. The final logical comparison has no unclassified functional delta.
6. CsWin32 generated-source tests demonstrate equivalent safe APIs.
7. Separate reviewable PRs exist for the SDK patch set, windows-rs tooling/config, and
   CsWin32 consumer changes.

The first whole-surface x64 checkpoint parsed all 321 translation units but stopped
during ownership resolution at `dxva9typ.h:266` because a declaration was not an
integral constant expression. This is an independent windows-rs blocker. It does not
invalidate the targeted `winspool.h` and `wingdi.h` evidence, but it must be fixed
before either header can receive whole-surface `matched` status.
