# Shift-Left Winmd Delta

## Compared inputs

- Reference:
  `Microsoft.Windows.SDK.Win32Metadata` `70.0.11-preview`
- Generated:
  windows-rs user-mode winmd from the 254-patch SDK overlay
- SDK:
  `Microsoft.Windows.SDK.CPP` `10.0.26100.7705`
- Architectures:
  x64, arm64, and x86, processed sequentially
- Generated partitions:
  662

The comparison ignores namespace partitioning and documentation attributes. Two levels
are reported:

1. **Identity overlap** asks whether a simple type, P/Invoke, or type/member name exists.
2. **Exact agreement** additionally requires the normalized declaration signature, type
   kind, member value, and emitted attributes to match.

## Current measurement

| Identity surface | Reference | Generated | Matches | Coverage |
| --- | ---: | ---: | ---: | ---: |
| Type simple names | 32,188 | 32,817 | 19,179 | 59.58% |
| P/Invoke names | 15,086 | 11,327 | 10,947 | 72.56% |
| Type/member names | 205,615 | 163,211 | 127,419 | 61.97% |
| **Weighted identity overlap** | 252,889 |  | 157,545 | **62.30%** |

| Exact surface | Reference | Generated | Exact matches | Coverage |
| --- | ---: | ---: | ---: | ---: |
| Types by kind and simple name | 32,251 | 32,869 | 19,179 | 59.47% |
| P/Invoke signatures | 18,352 | 14,093 | 3,717 | 20.25% |
| Member signatures and enum values | 206,227 | 163,725 | 76,426 | 37.06% |

These values are a baseline for the remaining migration, not a claim that 37-62% of the
native API is functional. A single typedef spelling or annotation difference makes an
otherwise equivalent declaration fail exact matching.

## High-level delta

| Delta | Count | Initial classification |
| --- | ---: | --- |
| Reference P/Invoke names absent from generated winmd | 4,139 | Primarily headers outside the current windows-rs scrape scope, legacy/current-SDK differences, and macros/manual declarations. |
| Generated P/Invoke names absent from reference | 380 | Header-version differences, scan scope differences, or declarations excluded/renamed by win32metadata. |
| Reference type names absent from generated winmd | 13,009 | Dominated by anonymous aggregate naming, synthetic enums, aliases, manual metadata, and legacy declarations. |
| Generated type names absent from reference | 13,638 | Dominated by windows-rs anonymous aggregate/canonical naming and raw native typedefs where win32metadata remaps names. |
| Reference type/member identities absent from generated | 78,196 | Missing types plus enum/member remaps and anonymous aggregate naming differences. |
| Generated type/member identities absent from reference | 35,792 | Extra/raw declarations and alternate aggregate naming. |

Representative missing P/Invoke families include AllJoyn, iSCSI, setup/installation,
legacy licensing, ink/input, and APIs supplied by headers not currently in the windows-rs
header list. This is a scrape-surface issue rather than an annotation-vocabulary gap.

## Common-name signature differences

10,947 P/Invoke names exist in both outputs, but only 3,717 normalized signatures match
exactly. Representative causes:

- Raw `HANDLE` versus win32metadata pseudo handles. The shift-left design intentionally
  keeps raw `HANDLE` and moves ownership to attributes.
- Semantic typedef differences such as `SECURITY_STATUS` versus `HRESULT`.
- SAL direction differences (`Out` versus `In|Out`) still sourced from generated headers
  or sidecars.
- Pointer alias differences (`PSTR`/`PCSTR`, `PWSTR`/`PCWSTR`, `FD_SET`/`fd_set`).
- Enum typing already present in the reference but not yet migrated for every header
  family.
- COM-out and const annotations not yet shifted for every declaration.
- Member/type remaps still living in scraper and emitter sidecars.

Some exact differences are intentional outcomes of the new policy. In particular,
`AbortPrinter(HANDLE)` plus output/ownership annotations is preferred over retaining a
synthetic `PRINTER_HANDLE`.

## Annotation completeness versus migration completeness

The annotation model itself is complete for the currently identified functional
categories. See `shift-left-annotation-coverage.md` for syntax, consumer evidence, and
representative SDK integration.

The measured delta remains large because most individual sidecar entries have not been
migrated. Closing it is predominantly mechanical header work:

1. Expand the windows-rs header/satellite scope for missing P/Invokes.
2. Complete enum and direct-use typing across remaining header families.
3. Migrate array, size, string, const, COM-out, retval, success, and encoding sidecars.
4. Replace member/type remaps and exclusions with source declarations or annotations.
5. Recover legacy declarations absent from the current SDK.
6. Decide which reference pseudo-handle differences are intentional policy deltas.

## Detailed artifacts

Generated locally under `artifacts/shift-left-winmd-delta`:

- `reference.cs`
- `generated.cs`
- `missing-type-names.txt` / `extra-type-names.txt`
- `missing-pinvoke-names.txt` / `extra-pinvoke-names.txt`
- `missing-member-names.txt` / `extra-member-names.txt`
- exact-signature missing/extra lists
- `winmd-delta.md`

The comparison is reproducible with:

```powershell
.\scripts\Compare-ShiftLeftWinmd.ps1 `
  -ReferenceWinmd <reference-winmd> `
  -GeneratedWinmd <windows-rs-winmd> `
  -OutputDirectory artifacts\shift-left-winmd-delta
```

## Review recommendation

Review functional common-name differences before attempting to eliminate every missing
or extra anonymous type name. Prioritize:

1. Resource ownership and invalid values.
2. Calling convention and import library.
3. Parameter count, direction, pointer depth, and array/size semantics.
4. Return and success semantics.
5. Enum values and direct typing.
6. Struct layout and COM method signatures.
7. Supported OS and architecture.

Anonymous aggregate naming and namespace partitioning should be classified separately
unless they affect generated bindings.
