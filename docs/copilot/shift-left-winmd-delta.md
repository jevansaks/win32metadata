# Shift-Left Winmd Delta

## Compared inputs

- Reference: current `bin/Windows.Win32.winmd` from win32metadata.
- Generated: full x64 windows-rs output using all 321 win32metadata partition
  translation units and their header traversal settings.
- SDK headers: the patched `generation/WinSDK/RecompiledIdlHeaders` tree.
- Generated partitions: 321.

This generated winmd predates the partition-path SAL-shim fix and the
`DIINSTALLDEVICE_FLAGS` patch. SAL-heavy signature counts therefore overstate the
remaining direction, optionality, constness, and array-size delta. Identity counts are
still useful.

The comparison ignores namespace partitioning and documentation attributes. Two levels
are reported:

1. **Identity overlap** asks whether a simple type, P/Invoke, or type/member name exists.
2. **Exact agreement** additionally requires the normalized declaration signature, type
   kind, member value, and emitted attributes to match.

## Current measurement

| Identity surface | Reference | Generated | Matches | Coverage |
| --- | ---: | ---: | ---: | ---: |
| Type simple names | 32,214 | 35,383 | 28,838 | 89.52% |
| P/Invoke names | 18,276 | 19,694 | 17,922 | 98.06% |
| Type/member names | 205,729 | 216,137 | 179,667 | 87.33% |
| **Weighted identity overlap** | 256,219 |  | 226,427 | **88.37%** |

| Exact surface | Reference | Generated | Exact matches | Coverage |
| --- | ---: | ---: | ---: | ---: |
| Types by kind and simple name | 32,277 | 35,433 | 28,846 | 89.37% |
| P/Invoke signatures | 18,320 | 19,705 | 2,291 | 12.51% |
| Member signatures and enum values | 206,342 | 216,633 | 103,881 | 50.34% |

These values are a baseline for the remaining migration, not a claim that 37-62% of the
native API is functional. A single typedef spelling or annotation difference makes an
otherwise equivalent declaration fail exact matching.

## High-level delta

| Delta | Count | Initial classification |
| --- | ---: | --- |
| Reference P/Invoke names absent from generated winmd | 354 | RPC/MIDL plumbing, raw export aliases, legacy aliases, inline pseudo APIs, and a small number of real emission gaps. |
| Generated P/Invoke names absent from reference | 1,772 | Newer SDK declarations, alternate canonical names, and declarations intentionally excluded by win32metadata. |
| Reference type names absent from generated winmd | 3,376 | Synthetic enums, anonymous aggregate naming, pseudo handles, aliases, and real interface/typedef ownership gaps. |
| Generated type names absent from reference | 6,545 | Raw SDK declarations, alternate aggregate naming, and types excluded or remapped by win32metadata. |
| Reference type/member identities absent from generated | 26,062 | Missing types plus enum/member remaps and anonymous aggregate naming differences. |
| Generated type/member identities absent from reference | 36,470 | Extra/raw declarations and alternate aggregate naming. |

The 354 missing P/Invoke names divide into these principal families:

| Family | Count | Classification |
| --- | ---: | --- |
| MIDL user-marshalling helpers | 136 | RPC plumbing currently filtered by windows-rs. |
| Classic NDR runtime functions | 145 | Real scraper/emission investigation; newer NDR forms emit while many older forms do not. |
| `K32*` PSAPI exports | 27 | Canonical-name difference: generated metadata exposes `EnumProcesses` with import `K32EnumProcesses`. |
| Legacy dbghelp exports | 22 | Mostly canonicalized to `*64` or `Ex` forms. |
| CLFS raw exports | 6 | Canonical-name difference such as `ClfsLsnEqual` with import `LsnEqual`. |
| Inline pseudo-token APIs | 3 | Manually synthesized by win32metadata from `FORCEINLINE` SDK helpers. |
| Direct2D APIs | 2 | Real regression: present in an earlier partition run and absent from the latest run. |
| Other individual APIs | 13 | Require API-specific classification. |

The missing type list includes 961 enums, of which 775 match synthetic definitions in
`enums.json`, and 133 interfaces. The enum set is predominantly annotation/header
migration work. The interface set contains substantive ownership/emission gaps,
especially the central Direct2D interface hierarchy.

## Common-name signature differences

17,922 P/Invoke names exist in both outputs. Exact textual agreement is intentionally a
poor metric because the toolchains preserve different aliases and attributes. A
projection-aware comparison found 2,371 shared-name signatures with ABI-width
differences; 1,803 differ at one return/parameter position. Representative causes:

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

## Representative end-to-end traces

### `HtmlHelpA`

The header already supplies optionality, string direction, `UINT`, and pointer-sized
`DWORD_PTR`. The reference additionally applies
`AssociatedEnum("HTML_HELP_COMMAND")` from `emitter.settings.rsp`. The old generated
winmd lost optionality, string aliasing, associated-enum metadata, and the pointer-sized
alias. This combines a stale pre-SAL-shim result, an enum migration, and alias
preservation.

### `OpenTnefStreamEx`

The header uses `LPADRBOOK`, declared through
`DECLARE_MAPI_INTERFACE_PTR(IAddrBook, LPADRBOOK)`. The generated signature reduced this
parameter to `void*`. This is not an ABI mismatch, but it is a functional projection
gap for CsWin32. The generator must discover macro-generated COM interface pointer
aliases or the use site needs `ProjectAs(IAddrBook)`.

### `IWbemEventProviderSecurity::AccessCheck` and `WsRequestReply`

The headers contain `_In_reads_`, `_In_opt_`, `_Out_opt_`, and const-qualified pointer
contracts. The old partition output lost constness, optionality, direction, and
count/byte-size relationships. The root cause was that explicit partition scraping did
not force-include the windows-rs SAL capture shim. That path is now fixed; a new full
generation is required to measure the remaining annotation gap.

### `JET_API_PTR` and `JetTerm`

`esent.h` defines `JET_API_PTR` as `JET_UINT64` under `_WIN64` and `JET_UINT32`
otherwise. Generated RDL uses `JET_API_PTR` throughout and defines
`JET_INSTANCE = JET_API_PTR`, but emits no `JET_API_PTR` definition. This leaves the
architecture-dependent typedef root unresolved and accounts for many x64 width
mismatches. This is a generator ownership/dependency bug, not missing source metadata.

### `D2D1CreateDevice` and `ID2D1Device`

The declarations and DLL mappings are present in `d2d1_1.h` and the Direct2D partition
traverses that header. An earlier full run emitted both APIs, while the latest run emits
neither and omits the central `ID2D1Device`/`ID2D1DeviceContext` hierarchy. Secondary
Direct2D interfaces still emit. This is a regression in cross-partition ownership or
reference suppression, not a header-list gap.

### `NdrClientCall2`

`rpcndr.h` declares `NdrClientCall2` as a variadic `RPC_VAR_ENTRY` function and
`libMappings.rsp` maps it to `RPCRT4.dll`. The same generated RDL emits the similarly
variadic `NdrClientCall3`, proving that variadic functions are not categorically
unsupported. The remaining likely causes are cursor collection, partition filtering,
or duplicate/reference ownership.

### `K32EnumProcesses`

Generated metadata exposes `EnumProcesses` with
`DllImport(EntryPoint = "K32EnumProcesses")`; the reference also retains a method named
`K32EnumProcesses`. This should be treated as equivalent only if the target projection
does not require both public aliases. The same policy question applies to CLFS and
legacy dbghelp names.

### `GetCurrentProcessToken`

The SDK declaration is a `FORCEINLINE` helper returning the pseudo-handle constant
`-4`. Current win32metadata manually synthesizes a method with `Constant("-4")` and
`DoNotRelease`. Reproducing it requires guarded metadata-only representation rather than
ordinary exported-function scraping.

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
