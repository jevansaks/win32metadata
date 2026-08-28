# `winspool.h` / `Printing`

## Scope

The first pass covers `OpenPrinterA/W`, `OpenPrinter2A/W`, and `ClosePrinter` in
`Windows.Win32.Graphics.Printing`.

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

## Delta classification

| Symbol | Result |
| --- | --- |
| `OpenPrinterA/W` | Accepted pseudo-handle replacement; unresolved winmd constness |
| `OpenPrinter2A/W` | Accepted pseudo-handle replacement; unresolved winmd constness |
| `ClosePrinter` | Accepted borrowed `HANDLE` replacement |

The source-to-RDL path correctly preserves `*const` for printer names. The generated
winmd loses `ConstAttribute`, so this is an RDL-to-winmd tooling defect rather than a
header annotation gap.

The next pass must enumerate every `PRINTER_HANDLE` sidecar use. Consumer inputs become
borrowed `HANDLE`; each actual producer must independently prove whether it transfers
ownership and, if so, receive lifecycle metadata.

## Projection gate

CsWin32 must generate the same `ClosePrinterSafeHandle` experience from lifecycle
metadata on a raw `HANDLE` output parameter. That source-generation test is required
before this unit can move from `classified` to `matched`.
