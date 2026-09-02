# Header Report: GameInput.h

## Partitions
`GameInput`

## Scrape validation
- Re-scraped `GameInput` partition (`ScanArch=x86`, required — `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 1 `DllImport` function, 1311 auto-remaps, 10 fn-ptr excludes written.

## Ownership audit (producer-site-only policy)
- The sole scraped `DllImport` is `GameInputCreate(IGameInput** gameInput)` — a COM-interface-pointer producer, consistent with the clean COM-factory pattern already established for `dxcapi.h`/`directml.h`. No `HANDLE`-family output here.
- `GameInput.h` does contain a raw `HANDLE` out-parameter: `IGameInputDispatcher::OpenWaitHandle(_Outptr_result_nullonfailure_ HANDLE * waitHandle)`. However, this is a **COM vtable interface method** (`IFACEMETHOD`), not a top-level `DllImport`/`extern "C"` function. Repo-wide precedent for `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` (every header currently carrying these annotations — `bcrypt.h`, `amsi.h`, `AuthZ.h`, `sspi.h`, `tbs.h`, `ncrypt.h`, `ncryptprotect.h`, `NTSecAPI.h`, `NTSecPKG.h`, `prntvpt.h`, `securitybaseapi.h`, `wincrypt.h`) shows the mechanism applied only to extern function/typedef declarations, never to COM interface vtable methods. Annotating COM interface out-parameters is a distinct, out-of-scope capability not currently supported by this ledger's established policy or any prior patch.

## Conclusion
`accepted-normalized` — no ownership annotation required under current policy scope. The one real `HANDLE`-producing API (`IGameInputDispatcher::OpenWaitHandle`) is a COM interface method, outside the scope of the `DllImport`-based ownership-annotation mechanism used throughout this repo.
