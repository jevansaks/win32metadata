# fdi.h — accepted-normalized (producer-site fix applied)

**Partition:** Cabinets
**Size:** ~50386 bytes

## Analysis

`fdi.h` (Diamond/Cabinet File Decompression Interface) declares
`HFDI FDICreate(...)` (producer, direct return value) and
single-arg `FDIDestroy(HFDI hfdi)` (destroyer). `HFDI`
(`typedef void FAR *HFDI;`) is a distinct pointer-sized opaque
typedef (not the raw `HANDLE` type).

Checked `autoTypes.json` first: **no entry existed**. Confirmed
single-file usage (`fdi.h` only), `Cabinets` partition (not
`ExcludeFromCrossarch`), namespace `Windows.Win32.Storage.Cabinets`
(from partition `settings.rsp`). Added new `autoTypes.json` entry
(`ValueType: DECLARE_HANDLE`, `CloseApi: FDIDestroy`,
`InvalidHandleValues: [0]`). Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=Cabinets` → 0 errors.

## Conclusion

Producer-site fix applied — new `HFDI` `autoTypes.json` entry added
at the producer/close-function level.
