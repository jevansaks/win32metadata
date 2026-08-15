# Shift-Left Annotation Category Coverage

## Interpretation

The hard requirement is that every functional win32metadata category has an ABI-neutral
header representation and a windows-rs consumer path. A category is **proven** when its
syntax is covered by the comprehensive Clang fixture and emitted RDL. An SDK patch is
additional integration evidence, not a prerequisite for establishing that the category
is representable.

Primary consumer fixture:

- `C:\repos\windows-rs-shift-left\crates\tests\libs\clang\input\win32metadata_annotations.h`
- `C:\repos\windows-rs-shift-left\crates\tests\libs\clang\expected\win32metadata_annotations.rdl`

## Custom annotation coverage

| Category | Header representation | Consumer evidence | SDK integration evidence | Status |
| --- | --- | --- | --- | --- |
| Last error | `_Win32_metadata_set_last_error_` | Clang fixture and P/Invoke flag round trip | `SetupAPI.h.zzz-set-last-error.patch` | Proven |
| Import library | `_Win32_metadata_import_library_("x.dll")` | Override regression and precedence test | `winsvc.h.import-library-overrides.patch` | Proven |
| Supported OS | `_Win32_metadata_supported_os_(windows...)` | Function, record, enum fixture | DirectX and Shell/COM/UI batches | Proven |
| Errors as success | `_Win32_metadata_errors_as_success_` | Function fixture | Representative consumer fixture | Proven |
| Multiple success values | `_Win32_metadata_multiple_success_values_` | Function fixture | Representative consumer fixture | Proven |
| Static library | `_Win32_metadata_static_library_("x.lib")` | Function fixture | Representative consumer fixture | Proven |
| Agile | `_Win32_metadata_agile_` | Interface fixture | Representative consumer fixture | Proven |
| RAII free | `_Win32_metadata_raii_free_(CloseX)` | Return, parameter, and typedef fixtures | Printing, shared handles, security ownership | Proven |
| Invalid handle | `_Win32_metadata_invalid_handle_(value)` | Repeated signed/hex literal fixtures | Printing and ownership batches | Proven |
| Free with | `_Win32_metadata_free_with_(FreeX)` | Return, parameter, field, typedef fixtures | Ownership fixtures | Proven |
| Do not release | `_Win32_metadata_do_not_release_` | Return fixture | Representative consumer fixture | Proven |
| Not null terminated | `_Win32_metadata_not_null_terminated_` | Return, parameter, field fixtures | Representative consumer fixture | Proven |
| Double-null terminated | `_Win32_metadata_null_null_terminated_` | Parameter and field fixtures | Representative consumer fixture | Proven |
| Retained | `_Win32_metadata_retained_` | Parameter fixture | Representative consumer fixture | Proven |
| Ignore if return | `_Win32_metadata_ignore_if_return_(value)` | Parameter fixture | Representative consumer fixture | Proven |
| Array count parameter | `_Win32_metadata_array_count_param_(index)` | Parameter fixture | Representative consumer fixture | Proven |
| Fixed array count | `_Win32_metadata_array_count_(count)` | Hex literal parameter fixture | Representative consumer fixture | Proven |
| Array count field | `_Win32_metadata_array_count_field_(field)` | Record field fixture | Representative consumer fixture | Proven |
| Byte-size parameter | `_Win32_metadata_memory_size_param_(index)` | Parameter fixture | Representative consumer fixture | Proven |
| Also usable for | `_Win32_metadata_also_usable_for_(TYPE)` | Typedef fixture | Shared handle patches | Proven |
| Associated enum | `_Win32_metadata_associated_enum_(TYPE)` | Parameter, return, and field fixtures | Guarded enum batches prefer direct typing | Proven |
| Associated constant | `_Win32_metadata_associated_constant_(NAME)` | Enum fixture | WinUser, RichEdit, and multimedia enums | Proven |
| Native inheritance | `_Win32_metadata_native_inheritance_(BASE)` | Record fixture | Representative consumer fixture | Proven |
| Struct size field | `_Win32_metadata_struct_size_field_(field)` | Record fixture | Representative consumer fixture | Proven |
| Native encoding | `_Win32_metadata_native_encoding_(encoding)` | Constant and field fixtures | Representative consumer fixture | Proven |
| ANSI | `_Win32_metadata_ansi_` | Function fixture | Representative consumer fixture | Proven |
| Unicode | `_Win32_metadata_unicode_` | COM method fixture | Representative consumer fixture | Proven |
| Input | `_Win32_metadata_in_` | Parameter fixture | SAL remains preferred | Proven |
| Output | `_Win32_metadata_out_` | Parameter fixture | SAL and retval compatibility fallback | Proven |
| Optional | `_Win32_metadata_optional_` | Parameter fixture | SAL remains preferred | Proven |
| Reserved | `_Win32_metadata_reserved_` | Parameter fixture | SAL remains preferred | Proven |
| Retval | `_Win32_metadata_retval_` | Parameter fixture | `_Out_retval_` compatibility fallback | Proven |
| COM out pointer | `_Win32_metadata_com_out_ptr_` | Parameter fixture | COM retval compatibility fallback | Proven |
| Native const | `_Win32_metadata_const_` | Parameter/field target validation | Existing C `const` remains preferred | Proven |
| Canonical callback name | `_Win32_metadata_canonical_name_(NAME)` | Alias resolution fixture | Eight callback patches | Proven |
| Semantic type projection | `_Win32_metadata_project_as_(TYPE)` | Parameter, return, and field RDL plus winmd attribute round trip | `SECURITY_STATUS` return values are the motivating case | Proven |
| Reduce pointer level | `_Win32_metadata_reduce_pointer_level_` | Parameter and field fixtures | LDAP and ICU patches | Proven |
| Scoped enum | guarded `enum class` | Scoped-enum RDL fixture | Multiple guarded enum batches | Proven |
| Flags enum | `[[clang::flag_enum]]` | Clang cursor/RDL fixture | Multiple guarded flags batches | Proven |

## Categories represented by existing C/C++ or SAL syntax

| Metadata category | Source representation | Status |
| --- | --- | --- |
| Does not return | `__declspec(noreturn)` / compiler attribute | Existing parser support |
| Supported architecture | SDK preprocessor architecture guards and multi-arch merge | Existing parser support |
| Alignment and packing | `alignas`, `__declspec(align)`, and packing pragmas | Existing parser support |
| Native typedef identity | Real `typedef`/`using` declaration | Existing parser support |
| Native bitfield | Real C/C++ bit-field | Existing parser support |
| GUID/UUID | `DEFINE_GUID`, `MIDL_INTERFACE`, `__declspec(uuid)` | Existing parser support |
| Flexible array | Native trailing/flexible array declaration | Existing parser support |
| Constants | Enum member, `const`/`constexpr`, or guarded declaration for macros | Existing parser support |
| SAL direction/array/size | Existing SAL/MIDL annotations | Preferred over custom fallback |

## Non-declaration exceptions

These cases do not invalidate annotation completeness, but require explicit migration
rules:

- Function-like macros need a guarded metadata-only declaration or a pre-AST annotated
  macro representation.
- Object-like macros may use guarded typed constants.
- Non-owning aliases must move metadata to the declaration owner.
- Removed legacy APIs require exact guarded declarations recovered from an authoritative
  SDK or the reference metadata.
- Documentation URLs are a documentation-tooling concern rather than functional winmd
  metadata.

## Validation status

- Unknown, malformed, valued-versus-valueless, and misplaced annotations fail with
  source locations.
- Signed decimal, hexadecimal, and C integer suffixes are parsed.
- Repeated annotations survive Clang, RDL serialization, RDL reading, and winmd emission.
- Return annotations preserve calling conventions by trailing the complete declarator.
- All current SDK patches replay from a clean baseline and reproduce tracked headers.
