# pdh.h — accepted-normalized (producer-site fix applied)

**Partition:** Perf
**Size:** ~46341 bytes

## Analysis

`pdh.h` (Performance Data Helper) declares `PdhAddCounterW/A`/
`PdhAddEnglishCounterW/A` (producers of `_Out_ PDH_HCOUNTER *
phCounter`) and single-arg `PdhRemoveCounter(PDH_HCOUNTER hCounter)`
(destroyer). Checked `autoTypes.json` first: **`PDH_HLOG`/`PDH_HQUERY`
already have complete entries** (`CloseApi: PdhCloseLog`/
`PdhCloseQuery` respectively — no gap), but **`PDH_HCOUNTER` had an
entry with `ValueType`/`NativeTypedef` but was MISSING `CloseApi` and
`InvalidHandleValues`** — a genuine stale-metadata gap. Filled in
`CloseApi: PdhRemoveCounter`, `InvalidHandleValues: [-1, 0]`
(matching the sibling `PDH_HLOG`/`PDH_HQUERY` convention). Validated
via `dotnet build generation/WinSDK -c Release -p:ScanArch=x64
-t:ScrapeHeaders -p:PartitionFilter=Perf` → 0 errors.

## Conclusion

Producer-site fix applied — filled in the missing `CloseApi`/
`InvalidHandleValues` on the pre-existing `PDH_HCOUNTER`
`autoTypes.json` entry.
