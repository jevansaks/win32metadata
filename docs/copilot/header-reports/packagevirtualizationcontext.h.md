# Header Report: packagevirtualizationcontext.h

## Partitions
`AppxPackaging`

## Scrape validation
- Re-scraped `AppxPackaging` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — the new annotation syntax parses cleanly.

## Ownership audit (producer-site-only policy) — GENUINE GAP FOUND AND FIXED (plus documented residual limitation)

- `DECLARE_HANDLE(PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE)` is a genuine opaque handle. Checked
  `autoTypes.json` first: an entry exists (`"ValueType": "DECLARE_HANDLE", "NativeTypedef": true`) but
  **has no `CloseApi`/`InvalidHandleValues`** — unlike `swdevice.h`/`featurestagingapi.h`/`fhsvcctl.h`
  (batches `-45`/`-46`/`-49`), this is a genuine, currently-unaddressed gap, not an already-complete
  legacy entry.
- **Fix applied**: added the `win32metadata_annotations.h` include guard and, on both producer
  out-parameters, `_Win32_metadata_invalid_handle_(0)` + `_Win32_metadata_raii_free_(ReleasePackageVirtualizationContext)`:
  - `CreatePackageVirtualizationContext(..., _Out_ PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE* context)`
  - `DuplicatePackageVirtualizationContext(..., _Out_ PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE* destContext)`
  - Both released via the same `ReleasePackageVirtualizationContext(_In_ PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE context)`.
  - Patch artifact: `generation/WinSDK/patches/post-midl/packagevirtualizationcontext.h.context-ownership.patch`;
    verified with `git apply --check --reverse` (exit code 0).
- **`GetCurrentPackageVirtualizationContext()`** returns `PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE` directly
  as a "get current ambient context" query — analogous to `GetConsoleWindow()`/`GetConsoleInputWaitHandle()`
  (batch `scraping-investigation-22`): a **borrowed** reference to the currently-active context, not a
  newly-owned handle the caller must release. Left unannotated (correctly, per that precedent).
- **`ActivatePackageVirtualizationContext`/`DeactivatePackageVirtualizationContext`** manage a plain
  `ULONG_PTR cookie` — a generic integer "activation token", not an opaque handle type; out of scope for
  the `HANDLE`-specific mechanism (same reasoning as `mmiscapi2.h`'s/`timeapi.h`'s timer IDs).
- **`GetProcessesInVirtualizationContext(..., _Out_ UINT* count, _Out_ HANDLE** processes)`** produces an
  array of generic process `HANDLE`s via double-indirection — this is the **same generic/shared-type
  blocker class** already documented for `resourceindexer.h`/`physicalmonitorenumerationapi.h`: `HANDLE`
  is used for thousands of unrelated resource kinds, so it cannot be annotated here. This residual gap
  is *not* separately blocking this header (the genuine, distinctly-typed `PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE`
  gap has been fixed above); it is documented here for traceability and is the same already-tracked,
  already-accepted-as-a-class limitation as `physicalmonitorenumerationapi.h`.

## Conclusion
`accepted-normalized` — genuine `PACKAGE_VIRTUALIZATION_CONTEXT_HANDLE` ownership gap fixed at the
producer sites (`CreatePackageVirtualizationContext`/`DuplicatePackageVirtualizationContext`). The
remaining `GetProcessesInVirtualizationContext` `HANDLE**` array output is a documented instance of the
already-established generic-HANDLE-array limitation, not a new blocker.
