# Header Report: dmort.h

## Partitions
`Media.DxMediaObjects`

## Scrape validation
- Re-scraped `Media.DxMediaObjects` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `MoInitMediaType`/`MoFreeMediaType`, `MoCreateMediaType`/`MoDeleteMediaType`, `MoCopyMediaType`, `MoDuplicateMediaType` all allocate/free the **fully-defined, transparent** `DMO_MEDIA_TYPE` struct (not an opaque pointer-to-incomplete-type handle) — a memory-allocation convention (analogous to `keycredmgr.h`'s `KeyCredentialManagerInfo`, batch `scraping-investigation-39`), out of scope for the `HANDLE`-specific ownership mechanism. The struct internally may hold an `IUnknown*` released via its own COM refcounting, independent of this allocation pattern.

## Conclusion
`accepted-normalized` — no ownership annotation required (`DMO_MEDIA_TYPE` alloc/free is a transparent-struct memory-allocation pattern, not an opaque-handle ownership relationship).
