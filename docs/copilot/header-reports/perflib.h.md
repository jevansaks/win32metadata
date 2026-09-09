# perflib.h — accepted-normalized

**Partition:** Perf
**Size:** ~41757 bytes

## Analysis

`perflib.h` (Performance Counters V2 API) declares several
handle-producing functions. Checked `emitter.settings.rsp` first:
**`PerfOpenQueryHandle::phQuery`, `PerfStartProvider::phProvider`,
`PerfStartProviderEx::Provider` already have inline `[RAIIFree(...)]`
parameter annotations** (`PerfCloseQueryHandle`/`PerfStopProvider`
respectively) — already correctly handled via the per-parameter
mechanism, no gap.

`PerfCreateInstance` returns `PPERF_COUNTERSET_INSTANCE` (a raw
pointer, not via `_Out_` param); its destroyer,
`PerfDeleteInstance(HANDLE Provider, PPERF_COUNTERSET_INSTANCE
InstanceBlock)`, requires an **external `Provider` handle parameter**
in addition to the instance pointer — genuinely unrepresentable via a
single-arg `CloseApi`/`RAIIFree` (same reasoning as `mscat.h`'s
`HCATINFO`/`CryptCATAdminReleaseCatalogContext`: the close operation
depends on external state beyond the handle itself).

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer out-parameters in
`RecompiledIdlHeaders/um/perflib.h`:
```
PerfStartProvider(..., _Out_ HANDLE * phProvider
    _Win32_metadata_raii_free_(PerfStopProvider));   // first declaration
PerfStartProviderEx(..., _Out_ PHANDLE Provider
    _Win32_metadata_raii_free_(PerfStopProvider));
PerfStartProvider(..., _Out_ PHANDLE Provider
    _Win32_metadata_raii_free_(PerfStopProvider));   // second, redeclared with a differently-named out-param
PerfOpenQueryHandle(..., _Out_ HANDLE * phQuery
    _Win32_metadata_raii_free_(PerfCloseQueryHandle));
```
`PerfStartProvider` is redeclared twice in this header with different
out-parameter names (`phProvider` then `Provider`); both declarations were
annotated so the fix applies regardless of which declaration the scraper
resolves as canonical. Added the `#if defined(WIN32METADATA) #include
<win32metadata_annotations.h> #endif` guard block (no prior patch existed
for this header). All three former sidecar entries were removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/perflib.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `perflib.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Perf, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s)
  (scraped per-arch as x64/x86/arm64 automatically under crossarch).

## Conclusion

`PerfOpenQueryHandle`/`PerfStartProvider`/`PerfStartProviderEx` now carry
inline `_Win32_metadata_raii_free_` annotations directly on their
out-parameters; no sidecar entries remain for this header.
`PerfCreateInstance`/`PerfDeleteInstance` remain genuinely unrepresentable
(multi-arg, external-context-dependent close), unaffected by this change.
