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

## Conclusion

Clean — existing inline `RAIIFree` annotations already correct;
`PerfCreateInstance`/`PerfDeleteInstance` genuinely unrepresentable
(multi-arg, external-context-dependent close). No patch required.
