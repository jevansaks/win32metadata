# Shift-Left SDK Patch Review Guide

## Purpose

The patch set is intentionally large because each sidecar entry must ultimately live
beside its authoritative SDK declaration. Review should establish that each annotation
category has a sound, ABI-neutral source representation and that the patch machinery
reproduces the tracked headers. It is not necessary to manually inspect every repeated
function annotation.

## Recommended review order

1. Review the annotation vocabulary and placement rules:
   `docs/copilot/plans/shift-left-annotation-spec.md`.
2. Review one or two representative patches from every category below.
3. Review the machine-validation summary in
   `docs/copilot/shift-left-metadata-worklog.md`.
4. Review the logical winmd delta report, with attention to functional differences.
5. Sample additional patches from high-volume categories using the selection strategy
   below.

## Representative patches

| Category | Recommended patch | What to inspect |
| --- | --- | --- |
| Vocabulary | `win32metadata_annotations.h.annotation-vocabulary.patch` | Annotations disappear outside `WIN32METADATA`; values are stringified correctly; SAL compatibility fallbacks are guarded. |
| Raw `HANDLE` ownership | `winspool.h.printing-handle-ownership.patch` | Native type remains `HANDLE`; cleanup and every invalid value are attached to the producer output. |
| Genuine handle typedef | `windef.h.shared-handle-types.patch` | Only existing typedefs are annotated; no pseudo handles are introduced. |
| Last-error | `SetupAPI.h.zzz-set-last-error.patch` | Annotation precedes the complete function declaration and does not disturb `WINAPI`. |
| Import override | `winsvc.h.import-library-overrides.patch` | Only missing/incorrect scan results are overridden; the header is authoritative. |
| Supported OS, function | `d3d12.h.zz-supported-os.patch` | Exact platform value and owning declaration are used. |
| Supported OS, COM/UI | `ShObjIdl.h.zzz-supported-os.patch` | Attributes attach to complete interfaces/methods rather than aliases or references. |
| Guarded flags enum | `WinUser.h.zz-metadata-enums.patch` | Underlying type and normal ABI alias match; macro collisions are balanced; use sites are directly typed. |
| Guarded enum with associated constants | `Richedit.h.richedit-enums.patch` | Mask/composite macros remain associated constants; enum-name macro collisions are also protected. |
| Multimedia enum | `mfobjects.h.zzz-multimedia-enums.patch` | A smaller representative enum batch with direct signature typing. |
| Callback canonical name | `WinBase.h.callback-canonical-name.patch` | Native callback typedef remains intact while metadata uses the intended public name. |
| Pointer-level correction | `Winldap.h.callback-pointer-levels.patch` | Only metadata pointer depth changes; the native declaration is untouched. |
| Security resource ownership | `wincrypt.h.zzz-resource-ownership.patch` | Ownership is applied only to genuine typedefs or confirmed producers; callback-owned handles are not marked owning. |
| Cross-header enum identity | `minschannel.h.metadata-alg-id.patch` | `ALG_ID` has one guarded metadata identity across include orders while normal ABI remains `unsigned int`. |

## Category-level invariants

### All patches

- Paths are rooted at `generation/WinSDK/RecompiledIdlHeaders`.
- Patches apply in lexical filename order.
- A later patch for the same header is generated against all earlier patches.
- Normal compilation does not see a changed ABI.
- The patched result byte-matches the tracked header.

### Function annotations

- Attributes precede the complete declaration.
- Nothing is inserted between a calling-convention macro and function name.
- Conditional duplicate declarations are consistently annotated.

### Return, parameter, and field annotations

- Postfix placement attaches to the intended declarator.
- Repeated annotations appear on separate source lines.
- Parameter indexes use metadata parameter ordering.
- Ownership is attached to the producing return/output, not consumers.

### Enums

- The underlying native integer type is exact.
- Normal compilation retains the previous integer typedef/signature.
- Every macro push has a matching pop.
- Enum names as well as enum members are protected from macro collisions.
- Composite masks left as macros use `AssociatedConstant`.
- Values are valid C/C++ expressions, not C# sidecar syntax.

### Supported OS

- The annotation is on the declaration-owning header.
- Macro-only entries are not silently treated as declarations.
- Aliases and non-owning references are recorded as unresolved or moved to the owner.

## Sampling strategy

For high-volume repetitive categories, review:

1. The largest patch.
2. The smallest patch.
3. One patch with conditional declarations.
4. One patch that already had an earlier lexical patch.
5. A random sample of approximately 5% of the remaining headers.

Suggested high-volume samples:

| Category | Largest/complex sample | Small sample | Conditional/ordered sample |
| --- | --- | --- | --- |
| Last-error | `SetupAPI.h.zzz-set-last-error.patch` | `FileHC.h.zzz-set-last-error.patch` | `WinBase.h.zzz-set-last-error.patch` |
| Supported OS | `ShObjIdl.h.zzz-supported-os.patch` | `ScrnSave.h.zzz-supported-os.patch` | `WinUser.h.zzz-supported-os.patch` |
| Crypto enums | `wincrypt.h.zz-crypto-security-enums.patch` | `SubAuth.h.zz-crypto-security-enums.patch` | `NTSecAPI.h.zz-crypto-security-enums.patch` |
| Ownership | `wincrypt.h.zzz-resource-ownership.patch` | `bcrypt.h.zzz-resource-ownership.patch` | `winsvc.h.zzz-resource-ownership.patch` |

## Machine validation expected with each checkpoint

```text
1. Apply every post-MIDL patch from the clean infrastructure baseline.
2. Require zero patch failures.
3. SHA-256 compare every rebuilt header with the tracked result.
4. Compile representative headers normally and with WIN32METADATA.
5. Run targeted windows-rs partition scrapes.
6. Run the Clang and RDL regression suites.
7. Generate x64, arm64, and x86 metadata.
8. Produce a logical delta against the reference winmd.
```

The logical delta is the final authority. A syntactically correct patch is insufficient
if ownership, calling convention, enum identity, layout, array semantics, or another
binding-relevant behavior differs from the reference.
