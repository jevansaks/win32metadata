# `winspool.h` / `Printing`

## Calibration scope

The lifecycle patch currently covers `OpenPrinterA/W`, `OpenPrinter2A/W`, and
`ClosePrinter`. The comparison scope has expanded to every declaration defined by
`winspool.h` in `Windows.Win32.Graphics.Printing`.

The corrected native inventory contains:

| Category | Count |
| --- | ---: |
| Functions | 160 |
| Structs | 93 |
| Enums | 6 |
| Constants | 329 |
| Interfaces | 0 |
| C typedef aliases | 292 |
| **Total** | **880** |

The earlier 795-declaration report was not a valid header inventory. It included
interfaces and other declarations defined by `BiDiSpl.h`, `PrinterExtension.h`,
`printoem.h`, and other headers sharing the same metadata namespace.

## Patch

`generation/WinSDK/patches/post-midl/winspool.h.win32metadata.patch`

- Keeps every native parameter as `HANDLE`/`LPHANDLE`.
- Adds `_Win32_SetLastError_` to the five reviewed APIs.
- Adds `_Win32_RAIIFree_(ClosePrinter)` and invalid values `-1` and `0` only to
  the four producer output parameters.
- Does not attach ownership to `ClosePrinter` or other borrowed input handles.

## Generated result

The targeted x64 pass completed in about 19 seconds. The RDL contains:

```text
#[library("winspool.drv", set_last_error)]
extern "system" fn OpenPrinterW(
    #[opt] pPrinterName: *const u16,
    #[raii_free("ClosePrinter")]
    #[invalid_handle(-1)]
    #[invalid_handle(0)]
    phPrinter: *mut HANDLE,
    #[opt] pDefault: *const PRINTER_DEFAULTSW) -> BOOL;
```

## Root-presence result

Existing windows-rs and the new conversion contain all 880 header-derived roots. The
NuGet win32metadata reference contains all 160 functions, 93 structs, and 6 enums, but
omits 19 native constants and all 292 C typedef wrappers. No `winspool.h` APIs were
actually missing.

This result depends on two generation rules:

1. Declarations written directly in the partition `main.cpp` are not roots in a
   header-focused run, and namespace membership does not imply header ownership.
2. The legacy flat windows-rs `Windows.Win32.winmd` is not supplied as a dependency
   while discovering roots because it can satisfy selected declarations externally and
   make them appear absent from the generated output.

## Fidelity classification

| Symbol | Result |
| --- | --- |
| `OpenPrinterA/W` | Accepted pseudo-handle replacement; lifecycle payload matches |
| `OpenPrinter2A/W` | Accepted pseudo-handle replacement; lifecycle payload matches |
| `ClosePrinter` | Accepted borrowed `HANDLE` replacement |

The RDL writer now preserves `ConstAttribute` for const pointer parameters and fields.
All 167 const occurrences in the NuGet reference survive into the regenerated
header-focused winmd. The new conversion contains 62 additional const occurrences that
reflect native header constness and require projection review rather than header edits.

The complete comparison also identified:

- 26 functions whose legacy `SetLastError` sidecar must move to the header.
- `AddPrinterA/W` and `FindFirstPrinterChangeNotification` as additional owning handle
  producers.
- `PRINTER_OPTIONSA/W` struct-size and associated-enum sidecars.
- Four conditional `_When_` direction/optionality gaps on `SetJobA/W` and
  `SetPrinterA/W`; these are windows-rs SAL parser fixes.
- One `_At_(*ppProperties, _Pre_readable_size_(cProperties) ...)` count relationship on
  `FreePrintNamedPropertyArray`; this is also a SAL parser fix.
- 301 common constants whose projected type is `uint` in win32metadata and `int` in
  windows-rs.
- Six string constants with additional `NativeEncoding` metadata.

Consumer uses of `PRINTER_HANDLE` and `FINDPRINTERCHANGENOTIFICATION_HANDLE` become
borrowed raw `HANDLE`. Only producer returns and output parameters receive `RAIIFree`
and invalid-value metadata.

## Projection gate

CsWin32 must generate the same `ClosePrinterSafeHandle` experience from lifecycle
metadata on a raw `HANDLE` output parameter. That source-generation test is required
before this unit can move from `classified` to `matched`.
