# `winspool.h` / `Printing`

## Calibration scope

The patch and comparison cover every declaration defined by `winspool.h` in
`Windows.Win32.Graphics.Printing`.

The corrected native inventory contains:

| Category | Count |
| --- | ---: |
| Functions | 160 |
| Structs | 93 |
| Enums | 6 |
| Constants | 329 |
| Interfaces | 0 |
| C typedef aliases emitted as metadata roots | 0 |
| **Total** | **588** |

The native RDL also contains 292 C typedef aliases. They collapse to their semantic
metadata types and are not public winmd roots.

## Patch

`generation/WinSDK/patches/post-midl/winspool.h.win32metadata.patch`

- Keeps every native parameter as `HANDLE`/`LPHANDLE`.
- Moves all 31 `winspool.h` last-error sidecars to `_Win32_SetLastError_`.
- Moves all 92 string constness sidecars into native `LPCSTR`/`LPCWSTR`
  declarations.
- Adds lifecycle and invalid-value metadata to `OpenPrinterA/W`,
  `OpenPrinter2A/W`, `AddPrinterA/W`, and
  `FindFirstPrinterChangeNotification`.
- Adds struct-size, associated-enum, and flexible-array metadata.
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

## Full-header result

The conversion emits all 588 representable roots. The NuGet reference contains 569
of them and omits 19 native constants. No functions, structs, or enums are missing.

This result depends on two generation rules:

1. Declarations written directly in the partition `main.cpp` are not roots in a
   header-focused run, and namespace membership does not imply header ownership.
2. The legacy flat windows-rs `Windows.Win32.winmd` is not supplied as a dependency
   while discovering roots because it can satisfy selected declarations externally and
   make them appear absent from the generated output.

The final normalized comparison has no unaccepted logical differences. The remaining
textual differences are intentional:

- Pseudo handles become raw `HANDLE`; producer sites carry `RAIIFree` and invalid
  values while consumer sites remain borrowed.
- `PRINTER_DEFAULTSA/W.DesiredAccess` remains native `ACCESS_MASK` with
  `AssociatedEnum(PRINTER_ACCESS_RIGHTS)`.
- `StartDocPrinterA/W.pDocInfo` remains `byte*` because SAL permits both
  `DOC_INFO_1` and `DOC_INFO_3`; the legacy sidecar incorrectly narrowed it to
  `DOC_INFO_1A/W*`.
- Three anonymous nested records have compiler-generated names that differ from the
  legacy emitter, with equivalent layout and fields.

CsWin32 SafeHandle behavior is intentionally deferred to the consumer follow-on.
