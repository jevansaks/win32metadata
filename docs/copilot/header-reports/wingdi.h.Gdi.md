# `wingdi.h` / `Gdi`

## Scope

The first pass covers `FONT_RESOURCE_CHARACTERISTICS` and
`AddFontResourceExA/W` in `Windows.Win32.Graphics.Gdi`.

## Patch

`generation/WinSDK/patches/post-midl/wingdi.h.win32metadata.patch`

- Adds a guarded scoped `FONT_RESOURCE_CHARACTERISTICS : unsigned int` declaration.
- Keeps each native `fl` parameter as `DWORD`.
- Associates `fl` with the enum using `_Win32_AssociatedEnum_`.
- Adds the fixed `_Windows_SupportedOS_Windows2000_` macro to both A and W entry
  points.

## Generated result

The targeted x64 pass completed in about 14 seconds. The RDL contains:

```text
#[supported_os("windows5.0")]
#[library("GDI32.dll")]
extern "system" fn AddFontResourceExW(
    name: *const u16,
    #[associated_enum("FONT_RESOURCE_CHARACTERISTICS")] fl: u32,
    #[reserved] res: *const void) -> i32;

#[repr(u32)]
#[scoped]
enum FONT_RESOURCE_CHARACTERISTICS {
    FR_PRIVATE = 16,
    FR_NOT_ENUM = 32,
}
```

## Delta classification

| Symbol | Result |
| --- | --- |
| `FONT_RESOURCE_CHARACTERISTICS` | Accepted scoped-enum improvement; values and width match |
| `AddFontResourceExA/W` | Accepted native `DWORD` plus `AssociatedEnum`; unresolved const and reserved optionality |

`AssociatedEnum` survives both RDL and winmd generation, and supported-OS metadata is
present on both functions. The reference changes the metadata signature itself to the
enum; the proposed result deliberately retains `uint` plus `AssociatedEnum`.

The two initial windows-rs fidelity defects are closed:

1. Native constness survives RDL and winmd emission.
2. `_Reserved_` produces `OptionalAttribute` and `ReservedAttribute` without
   incorrectly inferring an output direction.

## Full-header calibration

The corrected header-root pass contains 2,030 RDL declarations:

| Category | Count |
| --- | ---: |
| Functions | 364 |
| Structs | 195 |
| Enums | 11 |
| Constants | 1,444 |
| Type roots | 16 |

The generated winmd contains every declaration present in the NuGet reference. It
also contains 788 native declarations omitted by the reference, primarily constants.

The first normalized classification pass leaves 392 textual or semantic differences:

| Category | Count | Primary cause |
| --- | ---: | --- |
| Functions | 300 | Mostly legacy `SupportedOS` metadata not yet shifted to headers |
| Structs | 48 | Flexible arrays and other field sidecars |
| Constants | 32 | Signedness and sentinel-value policy |
| Types | 11 | Handle invalid values and callback typedef representation |
| Enums | 1 | Native force-width sentinel omitted by the legacy metadata |

The reviewed `FONT_RESOURCE_CHARACTERISTICS` and `AddFontResourceExA/W` unit is
matched. The rest of `wingdi.h` remains the next full-header classification task.

## Projection gate

CsWin32 must continue generating a friendly enum-typed overload while preserving a
native `uint` extern signature. That source-generation baseline is required before this
unit can move from `classified` to `matched`.
