# slpublic.h — accepted-normalized (producer-site fix applied)

**Partition:** Identity
**Size:** ~76185 bytes

## Analysis

`slpublic.h` (Software Licensing Client public API) declares `SLOpen`
(producer, `_Out_ HSLC*`) and single-arg `SLClose(HSLC)` (destroyer).
`HSLC` (`typedef PVOID HSLC;`) is a distinct pointer-sized opaque
typedef.

Checked `autoTypes.json` first: **no entry existed**. Confirmed
single-file usage, `Identity` partition (not `ExcludeFromCrossarch`),
namespace `Windows.Win32.Security.Authentication.Identity` (from
partition `settings.rsp`). Added new `autoTypes.json` entry
(`ValueType: DECLARE_HANDLE`, `CloseApi: SLClose`,
`InvalidHandleValues: [0]`). Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=Identity` → 0 errors.

`SLQueryLicenseValueFromApp` and other `SL*` functions output plain
data buffers, not handles — no additional gap.

## Conclusion

Producer-site fix applied — new `HSLC` `autoTypes.json` entry added.
