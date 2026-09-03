# Header Report: keycredmgr.h

## Partitions
`Credentials`

## Scrape validation
- Re-scraped `Credentials` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `KeyCredentialManagerGetInformation(_Outptr_ KeyCredentialManagerInfo** keyCredentialManagerInfo)` /
  `KeyCredentialManagerFreeInformation(_In_ KeyCredentialManagerInfo* keyCredentialManagerInfo)` form an
  allocate/free pair, but `KeyCredentialManagerInfo` is a **fully-defined, transparent data struct**
  (`typedef struct KeyCredentialManagerInfo { GUID containerId; } KeyCredentialManagerInfo;`) — not an
  opaque pointer-to-incomplete-type handle (contrast with `appnotify.h`'s `PAPPSTATE_REGISTRATION`,
  fixed in this same batch). This is a plain memory-allocation convention (allocate a visible struct,
  free it later), analogous to `CoTaskMemAlloc`/`CoTaskMemFree` or `WSDAllocateLinkedMemory`/
  `WSDFreeLinkedMemory` (already classified out-of-scope in batch `scraping-investigation-20`) — outside
  the scope of the `HANDLE`-specific `_Win32_metadata_raii_free_`/`_invalid_handle_` mechanism, which
  targets opaque handle types specifically.
- `KeyCredentialManagerShowUIOperation(_In_ HWND hWndOwner, ...)` takes `HWND` as a caller-supplied input only.

## Conclusion
`accepted-normalized` — no ownership annotation required (`KeyCredentialManagerInfo` alloc/free is a transparent-struct memory-allocation pattern, not an opaque-handle ownership relationship).
