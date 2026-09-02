# Shift-Left Metadata Migration Worklog

## Scope

Move functional win32metadata sidecars into ABI-neutral Windows SDK header annotations
or guarded metadata-only declarations, teach windows-rs to consume them, and demonstrate
that the annotated SDK can generate logically equivalent Win32 metadata.

## Repositories and branches

| Repository | Branch | Purpose |
| --- | --- | --- |
| `microsoft/win32metadata` | `user/jevansa/shift-left-sdk-patches` | SDK ingestion patches and review artifacts |
| `jevansaks/windows-rs` | `user/jevansa/shift-left-metadata` | Annotation parser, RDL, winmd emission, and tests |

The windows-rs branch has been pushed. Important commits are:

- `30be978e8` partition-aligned Win32 metadata generation
- `cf325af15` initial annotation support
- `995986501` authoritative import-library overrides
- `4efb99ddb` annotation syntax, validation, enums, ownership, and pointer reduction
- `f01081b55` canonical callbacks and annotated constants
- `badda6fb6` repeated parameter attributes
- `a91d9567d` repeated nested Clang annotation regression
- `e4be7f388` ownership metadata on memory typedefs

## Decisions and assumptions

1. The Windows SDK headers become the source of truth. The PR #2248 patch system is a
   staging mechanism, not a permanent sidecar replacement.
2. Normal SDK ABI must remain unchanged.
3. `WIN32METADATA` activates metadata-only syntax. A future SDK integration should
   consider accepting both `WIN32METADATA` and historical `_WIN32METADATA_` during
   transition.
4. windows-rs import-library scanning is the default; a header annotation overrides it.
5. API-specific pseudo handles are intentionally removed. Ownership belongs on raw
   `HANDLE` producer returns/output parameters.
6. Handle/resource typedefs do not carry cleanup ownership. `RAIIFree` and invalid
   values belong only on producer returns and output parameters.
7. Repeated nested Clang annotations are written on separate source lines.
8. Enums use guarded declarations with balanced macro push/undef/pop operations.
9. Current logical comparison ignores namespace partition differences between the NuGet
   winmd and windows-rs output, but not behavioral differences.
10. Documentation URLs are not part of functional winmd equivalence.
11. `ProjectAs` is use-site metadata for parameters, return values, and fields. It
    preserves raw native typedefs while allowing opinionated generators to substitute
    an ABI-compatible semantic type at an annotated use. `SECURITY_STATUS` returns
    projected as `HRESULT` are the initial case.
12. COM out pointers use existing `_COM_Outptr_` SAL variants or IID/PPV convention
    inference. The staged SDK vocabulary no longer defines a duplicate
    `_Win32_metadata_com_out_ptr_` annotation.
13. `FreeWith` is removed from the proposed vocabulary. Generic allocation ownership
    uses `RAIIFree` on producer returns/output parameters rather than attaching cleanup
    behavior to pointer typedefs.
14. `DoNotRelease` is removed from the proposed vocabulary. With pseudo-handle types
    removed, plain `HANDLE` is borrowed unless a producer site explicitly carries
    `RAIIFree`.
15. The windows-rs Clang path now recognizes a bare `_Out_retval_` token and emits
    RDL `#[retval]`; the existing RDL, winmd, and bindgen paths already consume that
    attribute.

## windows-rs implementation completed

- Parses annotations on functions, methods, returns, parameters, records, interfaces,
  enums, fields, constants, and typedefs.
- Emits all custom metadata attributes required by the annotation specification.
- Sets the P/Invoke `SupportsLastError` flag.
- Supports authoritative import-library overrides.
- Supports signed/hexadecimal C integer literals and suffixes.
- Supports enum class/scoped and Clang flags-enum semantics.
- Supports canonical callback aliases and one-level pointer reduction.
- Emits `ProjectAsAttribute` on annotated parameters, return values, and fields without
  changing their raw native types.
- Preserves repeated annotations, including multiple invalid handle values.
- Validates unknown, malformed, valued/valueless, and misplaced annotations with source
  locations.
- Enables an annotated SDK/package override for generation experiments.

Validation completed on the windows-rs branch:

- 88 Clang tests passed.
- 8 header-partition tests passed.
- RDL tests, including repeated parameter attributes, passed.
- The `ProjectAs` Clang fixture and RDL-to-winmd custom-attribute round trip passed.
- Full user-mode generation passed for x64, arm64, and x86.
- 663 partitions were generated.

## SDK patch infrastructure

PR #2248 was integrated by cherry-picking:

- `a1ef0cf1`
- `1f77bd2f`
- `d154186c`

Authoritative locations:

- `generation/WinSDK/patches/pre-midl`
- `generation/WinSDK/patches/post-midl`
- `generation/WinSDK/RecompiledIdlHeaders`
- `scripts/ApplySDKPatches.ps1`

Patches are named `<filename>.<reason>.patch` and applied lexically. When multiple patches
touch one header, each later patch is generated against the result of earlier patches.

Current result:

- 254 post-MIDL patches.
- 225 patched headers, including the annotation vocabulary.
- All 254 patches apply in lexical order from clean `d154186c`.
- Patched outputs match the tracked `RecompiledIdlHeaders`.
- The initial 133-patch checkpoint passed full three-architecture windows-rs generation.
- The expanded checkpoint passed clean replay with 225/225 SHA-256 header matches.
- Sequential x64, arm64, and x86 generation completed in 1,899 seconds and wrote a
  662-partition winmd.
- The equivalent parallel run hung after completing x64 and arm64 rather than reporting
  a metadata diagnostic. windows-rs now accepts `WIN32METADATA_SEQUENTIAL=1` so the
  bounded validation mode is reproducible without source edits.
- The first broad logical comparison measured 62.30% weighted identity overlap and
  72.56% P/Invoke-name coverage. The remaining delta is documented in
  `docs/copilot/shift-left-winmd-delta.md`.
- Partition-aligned generation using all 321 win32metadata translation units improved
  weighted identity overlap to 88.37% and P/Invoke-name coverage to 98.06%.
- The comparison script's P/Invoke parser was corrected to recognize top-level methods
  independently of the current type-body state. The corrected missing-name count is
  354.
- Explicit partition scraping now force-includes the same SAL capture shim as the normal
  user-mode and kernel-mode paths.
- `DiInstallDevice.Flags` now carries a guarded `DIINSTALLDEVICE_FLAGS`
  `AssociatedEnum`; targeted RDL also recovered the SDK's existing SAL optionality.

## Completed SDK migration batches

| Batch | Result |
| --- | --- |
| Annotation vocabulary | Added `win32metadata_annotations.h` with the complete custom vocabulary; COM out pointers use existing SAL. |
| Printing ownership pilot | Raw `HANDLE` outputs carry `RAIIFree(ClosePrinter)` and invalid values; no `PRINTER_HANDLE`. |
| Callback fixups | Migrated all 27 classified fixups using canonical-name or pointer-reduction annotations. |
| Manual import overrides | Migrated current-header overrides to authoritative declaration annotations. |
| WinUser enums | 95 enums, 1,359 members, and 159 direct enum typings. |
| Controls/RichEdit enums | 73 enums and 524 balanced macro push/pop pairs across four headers. |
| Credential/UxTheme enums | Initial guarded enum batches completed. |
| Crypto/security enums | 84 enums and 619 balanced macro pairs across 12 headers. |
| Handle/resource typedefs | 27 genuine existing typedefs annotated; no pseudo handles added. |
| WinUser last-error | Header declarations annotated. |
| Kernel/process/file last-error | 404 declarations across 15 headers. |
| Service/security last-error | 337 declarations across 14 headers. |
| Networking last-error | 406 annotations covering 407 conditional declarations across 20 headers. |
| DirectX/graphics supported OS | 408 annotations for 407 names across 35 headers from 413 reviewed entries. |
| Storage/setup/device/system last-error | 854 declarations across 32 headers; no unresolved declarations. |
| Multimedia enums | 8 guarded enums, 66 members, 22 direct typings, and 5 associated constants across 8 headers. |
| Shell/COM/UI supported OS | 3,363 annotations for 3,129 declarations across 69 headers. |
| Security resource ownership | 30 of 31 candidates migrated across 11 headers, including 17 raw-`HANDLE` producers. |

The five DirectX review exceptions are:

| Name | Resolution |
| --- | --- |
| `ID3DDestructionNotifier` | Declaration is in `d3dcommon.h`, outside the assigned graphics family batch. |
| `ID3DInclude` | Declaration is in `d3dcommon.h`; `d3d10shader.h` only aliases it. |
| `IDirect3DDeviceManager9` | Declaration is in `dxva2api.h`. |
| `DWriteEngine2Events` | Declaration is in `imapi2.h` and is unrelated to DirectWrite. |
| `DWRITE_MAKE_OPENTYPE_TAG` | Function-like macro; declaration annotations cannot attach to it. It needs guarded constant/macro metadata handling. |

Additional concrete unresolved cases from the second bulk pass:

| Name/category | Resolution needed |
| --- | --- |
| `TIMECODE_SAMPLE_FLAGS` | Seven sidecar constants are absent from the current SDK headers. Recover an authoritative declaration/value source before adding a guarded enum. |
| `PSECURITY_DESCRIPTOR` ownership | Existing typedef is outside the assigned security batch and its invalid-value semantics are ambiguous. Resolve whether ownership belongs on specific producers rather than the pointer typedef. |
| 616 supported-OS macros | Declaration annotations cannot attach to function-like/object-like macros. Use guarded annotated constants where a constant declaration is possible; otherwise extend the metadata macro representation. |
| `PixelFormat` supported OS | Alias does not own the underlying declaration. Annotate the owning declaration or add guarded alias metadata. |
| 78 supported-OS references | The matched header references but does not own the declaration. Move each annotation to the actual owner header. |

## Corrected generation defects

| Defect | Resolution |
| --- | --- |
| Function/return annotation hid `WINAPI` | Return annotations now trail the complete declarator before `;`. |
| Repeated invalid values lost | RDL reader now emits every matching parameter pseudo-attribute rather than only the first. |
| Repeated nested Clang attributes collapsed | Put nested attribute macros on separate source lines and added a regression fixture. |
| Ownership disappeared on fields/typedefs | Non-function declarations use an emit-all path rather than return-only filtering. |
| Numeric invalid values rejected | Added signed decimal, hexadecimal, and C suffix parsing. |
| Callback aliases emitted internal names | Canonical-name collection is applied during callback/type resolution. |
| Annotation macros unavailable in foundational headers | `minwindef.h` and `windef.h` include the no-op vocabulary unconditionally. |
| `DECLARE_HANDLE` parse errors | Corrected annotation visibility and declarator placement; full generation now passes. |
| Missing/wrong import DLL mapping | Added authoritative header import-library annotation and precedence. |

## Focused comparison result

The printing pilot verifies the key HANDLE policy:

- Signatures remain raw `HANDLE`.
- Producer parameters contain `RAIIFree("ClosePrinter")`.
- Both `InvalidHandleValue(-1)` and `InvalidHandleValue(0)` survive into RDL/winmd where
  specified.
- Last-error semantics survive.

The full generated winmd is not byte-identical to the NuGet baseline because the two
toolchains partition namespaces differently. The remaining comparison must therefore
match declarations by logical identity and compare behavior field-by-field.

## Remaining bulk migration

The syntax and consumer behavior are resolved; remaining work is primarily mechanical
header migration and cumulative equivalence comparison:

1. Complete enum sidecars outside WinUser, Controls/RichEdit, credentials, UxTheme,
   crypto/security, and the initial multimedia batch.
2. Complete `supportedOS.rsp` outside the DirectX/graphics and Shell/COM/UI batches,
   including owner-header relocation and macro representation.
3. Complete `WithSetLastError.rsp` outside WinUser, kernel/process/file,
   service/security, networking, and storage/setup/device/system.
4. Classify and migrate the remaining auto-type candidates outside the completed shared
   and security batches. Move cleanup and invalid-value metadata to producer
   returns/output parameters, remove typedef ownership and `DoNotRelease` sidecars, and
   reject pseudo handles.
5. Migrate member remaps, array/count/byte-size/string semantics, COM method metadata,
   native inheritance, struct-size fields, success semantics, agility, and encoding.
6. Replace remaining scraper remaps/exclusions with corrected or guarded declarations.
7. Recover exact declarations for baseline APIs absent from current headers and place
   required compatibility declarations in a guarded legacy metadata header.
8. Run broad logical comparison against
   `Microsoft.Windows.SDK.Win32Metadata` `70.0.11-preview`.
9. For every functional difference, add or update a spec row, write the source patch,
   add a consumer regression test when needed, regenerate, and compare again.

## Review artifacts

- Canonical syntax and solution matrix:
  `docs/copilot/plans/shift-left-annotation-spec.md`
- Category-by-category proof matrix:
  `docs/copilot/shift-left-annotation-coverage.md`
- Representative patch sampling and review strategy:
  `docs/copilot/shift-left-patch-review-guide.md`
- Quantified logical winmd comparison:
  `docs/copilot/shift-left-winmd-delta.md`
- This decision/progress/assumption log:
  `docs/copilot/shift-left-metadata-worklog.md`
- SDK patch workflow:
  `docs/sdk_patches.md`
- Actual SDK patches:
  `generation/WinSDK/patches/post-midl`
- Patched SDK headers:
  `generation/WinSDK/RecompiledIdlHeaders`
- windows-rs implementation:
  `C:\repos\windows-rs-shift-left`
- Logical comparator:
  `scripts/Compare-ShiftLeftWinmd.ps1`

## Environment

- SDK package: `Microsoft.Windows.SDK.CPP` `10.0.26100.7705`
- Include version: `10.0.26100.0`
- Baseline winmd:
  `C:\.tools\.nuget\packages\microsoft.windows.sdk.win32metadata\70.0.11-preview\Windows.Win32.winmd`
- windows-rs libclang: `22.1.8`
- Rust tests require:

```powershell
$env:LIBCLANG_PATH = cargo run -q -p tool_clang -- path
```
