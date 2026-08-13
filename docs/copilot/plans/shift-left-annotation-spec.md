# Shift-Left Win32 Metadata Annotation Specification

## Status

This document is the canonical proposal for representing win32metadata semantics in
Windows SDK C/C++ headers and consuming them with the windows-rs metadata toolchain.
The syntax below has been implemented and tested on
`jevansaks/windows-rs:user/jevansa/shift-left-metadata`.

The migration is staged through the SDK ingestion patch system in
`generation/WinSDK/patches`. The eventual source of truth is the Windows SDK headers;
the patches are temporary until the corresponding header changes ship in the SDK.

## Non-negotiable rules

1. Metadata must not change the native ABI: no layout, calling convention, mangling,
   parameter count, native type, vtable, or exported symbol changes.
2. Ordinary SDK compilation sees every annotation macro as empty.
3. Metadata-only declarations are guarded by `WIN32METADATA`.
4. Existing genuine SDK typedefs may be annotated. New API-specific pseudo handles
   such as `PRINTER_HANDLE` must not be introduced.
5. Raw `HANDLE` producers carry cleanup and invalid-value metadata on their return or
   output parameter.
6. Header import-library annotations are authoritative. Import-library scanning remains
   the default.
7. Every SDK patch is a per-header, per-reason patch applied through the PR #2248
   ingestion mechanism, not a generated patch series under `docs`.

## Transport and vocabulary

The ingested SDK defines the vocabulary in
`RecompiledIdlHeaders/shared/win32metadata_annotations.h`:

```cpp
#if defined(WIN32METADATA) && defined(__clang__)
#define _WIN32META_ANNOTATION_(text) __attribute__((annotate(text)))
#else
#define _WIN32META_ANNOTATION_(text)
#endif
```

windows-rs enables `WIN32METADATA` while scraping annotated headers. Unknown,
malformed, valued-versus-valueless, and incorrectly placed annotations are errors with
source locations.

## Declaration placement

### Functions and methods

Function metadata precedes the complete declaration. It must not be inserted between
`WINAPI` and the function name.

```cpp
_Win32_metadata_set_last_error_
_Win32_metadata_import_library_("correct.dll")
BOOL WINAPI Example(void);
```

Return-value metadata follows the complete declarator and precedes the semicolon. This
preserves the calling-convention cursor extent.

```cpp
HANDLE WINAPI OpenThing(void)
    _Win32_metadata_raii_free_(CloseHandle)
    _Win32_metadata_invalid_handle_(INVALID_HANDLE_VALUE)
    _Win32_metadata_invalid_handle_(NULL);
```

### Parameters and fields

Annotations follow the declarator name. Put repeated nested attributes on separate
source lines.

```cpp
BOOL WINAPI CreateThing(
    _Out_ HANDLE *result
        _Win32_metadata_raii_free_(CloseThing)
        _Win32_metadata_invalid_handle_(-1)
        _Win32_metadata_invalid_handle_(0));
```

```cpp
struct BUFFER {
    BYTE *data _Win32_metadata_array_count_field_(size);
    DWORD size;
};
```

### Records, interfaces, and enums

Attributes appear between the declaration keyword and name.

```cpp
struct
    _Win32_metadata_struct_size_field_(cbSize)
    DESCRIPTION {
    DWORD cbSize;
};
```

```cpp
enum class
    _Win32_metadata_supported_os_(windows10.0.10240)
    [[clang::flag_enum]]
    OPTION : unsigned long {
    OPTION_NONE = 0,
};
```

### Typedefs and callback aliases

Type metadata precedes the typedef. A canonical callback annotation preserves the
public metadata name while references continue to use the native typedef.

```cpp
_Win32_metadata_canonical_name_(PUBLIC_CALLBACK)
typedef BOOL (WINAPI *INTERNAL_CALLBACK)(DWORD value);
```

## Attribute solution matrix

| winmd semantic | Header syntax | Valid target and consumer behavior |
| --- | --- | --- |
| `SetLastError` / P/Invoke `SupportsLastError` | `_Win32_metadata_set_last_error_` | Function. Sets the ECMA-335 P/Invoke flag. |
| P/Invoke module override | `_Win32_metadata_import_library_("name.dll")` | Function. Overrides import-library scan results. |
| `SupportedOSPlatform("windows...")` | `_Win32_metadata_supported_os_(windows10.0.10240)` | Function, method, record, enum, or typedef. |
| `CanReturnErrorsAsSuccess` | `_Win32_metadata_errors_as_success_` | Function or method. |
| `CanReturnMultipleSuccessValues` | `_Win32_metadata_multiple_success_values_` | Function or method. |
| `StaticLibrary("name.lib")` | `_Win32_metadata_static_library_("name.lib")` | Function. |
| `Agile` | `_Win32_metadata_agile_` | Class/struct/interface declaration. |
| `RAIIFree("CloseX")` | `_Win32_metadata_raii_free_(CloseX)` | Function return, method return, parameter, or genuine typedef. |
| `InvalidHandleValue(value)` | `_Win32_metadata_invalid_handle_(value)` | Function return, method return, parameter, or genuine typedef. Repeatable; signed decimal and hexadecimal C literals are accepted. |
| `FreeWith("FreeX")` | `_Win32_metadata_free_with_(FreeX)` | Return, parameter, field, or typedef. |
| `DoNotRelease` | `_Win32_metadata_do_not_release_` | Return, parameter, field, or typedef. |
| `NotNullTerminated` | `_Win32_metadata_not_null_terminated_` | Return, parameter, field, or typedef. |
| `NullNullTerminated` | `_Win32_metadata_null_null_terminated_` | Return, parameter, field, or typedef. |
| `Retained` | `_Win32_metadata_retained_` | Parameter. |
| `IgnoreIfReturn("value")` | `_Win32_metadata_ignore_if_return_(value)` | Parameter. |
| `NativeArrayInfo.CountParamIndex` | `_Win32_metadata_array_count_param_(index)` | Parameter; zero-based metadata parameter index. |
| `NativeArrayInfo.CountConst` | `_Win32_metadata_array_count_(count)` | Parameter; accepts C integer literals. |
| `NativeArrayInfo.CountFieldName` | `_Win32_metadata_array_count_field_(field)` | Field. |
| `MemorySize.BytesParamIndex` | `_Win32_metadata_memory_size_param_(index)` | Parameter; byte-size parameter index. |
| `AlsoUsableFor("TYPE")` | `_Win32_metadata_also_usable_for_(TYPE)` | Typedef. |
| `AssociatedEnum("TYPE")` | `_Win32_metadata_associated_enum_(TYPE)` | Parameter, field, or constant when direct enum typing is impossible. |
| `AssociatedConstant("NAME")` | `_Win32_metadata_associated_constant_(NAME)` | Enum declaration for mask/composite constants left as macros. |
| `NativeInheritance("BASE")` | `_Win32_metadata_native_inheritance_(BASE)` | Record declaration. |
| `StructSizeField("field")` | `_Win32_metadata_struct_size_field_(field)` | Record declaration. |
| `NativeEncoding("encoding")` | `_Win32_metadata_native_encoding_(encoding)` | Field or constant. |
| `Ansi` | `_Win32_metadata_ansi_` | Function, method, field, or constant. |
| `Unicode` | `_Win32_metadata_unicode_` | Function, method, field, or constant. |
| input direction | `_Win32_metadata_in_` | Parameter; explicit fallback when SAL/MIDL cannot express the metadata. |
| output direction | `_Win32_metadata_out_` | Parameter. |
| optional | `_Win32_metadata_optional_` | Parameter. |
| reserved | `_Win32_metadata_reserved_` | Parameter. |
| `RetVal` | `_Win32_metadata_retval_` | Parameter. |
| `ComOutPtr` | `_Win32_metadata_com_out_ptr_` | Parameter. |
| native constness | `_Win32_metadata_const_` | Parameter or field. |
| canonical callback name | `_Win32_metadata_canonical_name_(NAME)` | Typedef. Replaces sidecar function-pointer prototype/alias fixups. |
| reduce one pointer level | `_Win32_metadata_reduce_pointer_level_` | Parameter or field. Removes exactly one metadata pointer level without changing C type. |
| scoped enum | guarded `enum class` | Emitted as `ScopedEnum`; normal branch retains the original ABI type. |
| flags enum | `[[clang::flag_enum]]` | Enum. Emitted with flags semantics. |

Existing compiler/SAL facts remain the preferred source for attributes already expressed
by native syntax, including `noreturn`, architecture guards, alignment, native typedef
identity, bitfields, UUIDs, packing, parameter direction, optionality, and array sizes.
The custom annotations are fallbacks where existing syntax is absent or wrong.

## Enum and constant migration

Synthetic enums from `enums.json` become guarded SDK declarations. Normal compilation
must preserve the original integer ABI:

```cpp
#ifdef WIN32METADATA
#pragma push_macro("MODE_A")
#undef MODE_A
enum class MODE : DWORD {
    MODE_A = 1,
};
#pragma pop_macro("MODE_A")
#else
typedef DWORD MODE;
#endif
```

Rules:

- Use the original underlying integer type.
- Balance every `push_macro` with `pop_macro`.
- Prefer changing metadata parameter/field types directly to the enum in the
  `WIN32METADATA` branch.
- Use `AssociatedEnum` only when direct typing is impossible.
- Keep composite or mask macros that cannot be enum members and associate them with
  `_Win32_metadata_associated_constant_`.
- Metadata-only constants use guarded annotated `constexpr`/`const` declarations while
  preserving the ordinary macro.

## HANDLE ownership

Genuine SDK handle typedefs may carry type-level metadata:

```cpp
_Win32_metadata_raii_free_(CloseServiceHandle)
_Win32_metadata_invalid_handle_(NULL)
DECLARE_HANDLE(SC_HANDLE);
```

APIs that natively use `HANDLE` remain `HANDLE`:

```cpp
BOOL WINAPI OpenPrinterW(
    _In_ LPWSTR name,
    _Out_ HANDLE *printer
        _Win32_metadata_raii_free_(ClosePrinter)
        _Win32_metadata_invalid_handle_(NULL));
```

Do not create `PRINTER_HANDLE`, `HEAP_HANDLE`, or similar metadata-only pseudo types.

## Import-library precedence

1. `_Win32_metadata_import_library_` on the declaration.
2. Import-library scanning.
3. Configured default.

This permits headers to correct missing or incorrect `.lib` information without
duplicating the complete scan-derived mapping.

## Sidecar migration matrix

| Existing source | Header representation |
| --- | --- |
| `WithSetLastError.rsp` | `_Win32_metadata_set_last_error_` on each function. |
| `supportedOS.rsp` | `_Win32_metadata_supported_os_` on the owning declaration. |
| `libMappingsManual.rsp` | `_Win32_metadata_import_library_`; scanning remains default. |
| `libMappings.rsp` | Import-library scan, with header override only where needed. |
| `autoTypes.json` | Genuine typedef annotations or raw-`HANDLE` producer annotations; no pseudo handles. |
| `enums.json` | Guarded enum declarations, direct enum typing, and associated constants. |
| `functionPointerFixups.json` | Canonical-name and pointer-reduction annotations on typedefs/usages. |
| `emitter.settings.rsp --memberRemap` | Correct guarded declaration/name in the header; use an annotation only when the native spelling must remain different. |
| scraper type/tag remaps | Correct typedef/tag relationship in headers; namespace-qualified C++ types remain native. |
| exclusions | Correct header guards or metadata-only exclusion annotation if the declaration truly must not be emitted. No silent consumer list. |
| array/string/size overrides | Existing SAL/MIDL first; explicit parameter/field annotations above as fallback. |
| COM/manual metadata | Guarded declarations and method/parameter annotations in the authoritative header. |
| documentation mappings | Documentation tooling concern; not required for functional winmd equivalence. |
| removed legacy APIs | Recover the exact historical declaration from an authoritative SDK and place it in a guarded metadata-only legacy header when compatibility requires it. |

### Macro-only and non-owning declarations

Declaration annotations cannot attach to a preprocessor macro or to a header that merely
references a declaration owned elsewhere.

- For object-like constant macros, add a guarded annotated constant declaration when the
  value and native type are authoritative.
- For function-like macros, preserve the macro for normal compilation and represent
  functional metadata through a guarded metadata-only function/constant declaration or
  an explicit annotated-macro construct consumed before Clang declaration traversal.
- Put supported-OS and ownership annotations on the header that owns the complete
  declaration, not on aliases or references.
- Do not synthesize values absent from the current SDK. Recover them from an
  authoritative released SDK or baseline metadata and record that source.
- Pointer aliases such as `PSECURITY_DESCRIPTOR` do not automatically own pointed-to
  memory. Prefer producer-specific return/parameter ownership unless the typedef has
  universal ownership semantics.

## Consumer requirements

The windows-rs consumer must:

- parse annotations on functions, methods, returns, parameters, records, enums, fields,
  constants, and typedefs;
- preserve repeated attributes;
- validate annotation names, values, and targets;
- preserve calling conventions and pointer depths unless an explicit metadata annotation
  changes the emitted type;
- resolve canonical callback aliases consistently at declarations and references;
- use annotations only while `WIN32METADATA` is enabled;
- emit the standard `Windows.Win32.Metadata` attribute types into the generated winmd;
- generate x86, x64, and arm64 metadata from the same annotated headers.

For large validation runs, windows-rs supports `WIN32METADATA_SEQUENTIAL=1` to process
architectures serially. This does not change metadata semantics; it avoids a worker-pool
deadlock observed with the expanded header set and makes architecture failures
deterministic.

## Equivalence definition

Logical equivalence is evaluated by declaration identity and behavior rather than byte
identity or namespace partitioning. For each API/type, compare:

- native name and kind;
- calling convention and import module;
- parameter order, direction, native type, pointer depth, and array/size semantics;
- return type and ownership;
- enum underlying type, members, values, scoped/flags semantics;
- struct layout, fields, packing, alignment, inheritance, and size field;
- interfaces, methods, UUIDs, and agility;
- last-error and success semantics;
- supported OS;
- callback canonical names;
- repeatable metadata such as invalid handle values.

Any difference affecting generated bindings or safe resource use is a functional gap and
requires a source syntax, consumer behavior, patch, and regression test.
