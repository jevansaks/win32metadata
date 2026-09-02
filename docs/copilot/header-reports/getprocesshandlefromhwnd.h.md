# Header Report: getprocesshandlefromhwnd.h

## Partitions
`Threading`

## Scrape validation
- Re-scraped `Threading` partition (`ScanArch=x64` default; not `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy) — BLOCKED

Single function: `HANDLE WINAPI GetProcessHandleFromHwnd(_In_ HWND hwnd)`. This function genuinely
produces a process `HANDLE` that the caller must eventually `CloseHandle()` — a real ownership
relationship exists. However, the value is returned **directly as the C function return value**,
not through an `_Out_`/`_Outptr_` pointer parameter.

### Why this cannot be classified clean or fixed with the existing pattern
Every ownership annotation currently present anywhere in this repository (13 headers audited this
session: `bcrypt.h`, `amsi.h`, `AuthZ.h`, `sspi.h`, `tbs.h`, `ncrypt.h`, `ncryptprotect.h`,
`NTSecAPI.h`, `NTSecPKG.h`, `prntvpt.h`, `securitybaseapi.h`, `wincrypt.h`, plus the corrected-policy
commits `165b5f09`/`7335ddc4` which stripped stale typedef-level annotations from `windef.h`) attaches
`_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` **exclusively to an `_Out_`-style
pointer parameter declarator** — never to a bare function return type.

To confirm there is no existing precedent (in this repo's own scraper output, or in the currently
published/baseline winmd) for annotating a raw return-value `HANDLE`:
- Dumped the baseline `bin\Windows.Win32.winmd` (24MB, 35,145 types) via `WinmdUtils.exe dump`.
- Confirmed the dump tool *does* render return-position attributes when present (289 `[return: ...]`
  occurrences found, e.g. `[return: Const]`), so its absence elsewhere is meaningful, not a tooling gap.
- Searched for `[RAIIFree(...)]`/`[InvalidHandleValue(...)]` usage: **every single occurrence attaches
  to a `struct` type declaration** (e.g. `[InvalidHandleValue(-1L)] [InvalidHandleValue(0L)]
  [NativeTypedef] public struct HBITMAP { ... }`), never to a method or a `[return: ...]` position.
- Checked well-known return-HANDLE-shaped APIs already in the baseline winmd for comparison:
  `LoadLibraryExW`/`LoadLibraryExA` (return `HMODULE` directly, definitely require `FreeLibrary`) and
  `IcmpCreateFile` (returns `HANDLE` directly) — **neither carries any RAIIFree-equivalent metadata**
  in the currently published winmd. `GetProcessHandleFromHwnd` itself is already present in the
  baseline winmd with **no** ownership metadata.
- This confirms the pipeline's ownership-annotation mechanism (both the legacy `autoTypes.json`
  type-level path and the newer inline producer-site path) supports HANDLE ownership only via
  out-parameters / type declarations — a direct function return value has no established annotation
  surface anywhere in this codebase.

### Blocker
Adding ownership metadata for a return-value-only HANDLE producer would require inventing a new,
unprecedented annotation placement (e.g. a `[return: RAIIFree(...)]`-equivalent convention) with no
existing example anywhere in this repository to validate against, and no visibility into whether any
downstream consumer (this repo's own `EmitWinmd`, or the separate windows-rs-shift-left parser) would
even recognize such a placement. This is the same class of blocker as `esent.h` — a genuine ownership
relationship exists, but expressing it correctly requires a dedicated policy decision, not a guess.

## Conclusion
`blocked` — genuine `HANDLE`-producing function (return value, not out-param); no precedent anywhere
in the repository or the published baseline winmd for annotating a direct function return value's
ownership. Needs a dedicated decision on whether/how to extend the annotation vocabulary to cover
return-value handles before this can be classified or fixed.
