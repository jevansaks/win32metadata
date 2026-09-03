# ras.h — accepted-normalized (producer-site fix applied)

**Partition:** RRas
**Size:** ~68157 bytes

## Analysis

`ras.h` declares `RasDialA/W` (producers, `_Out_ LPHRASCONN`) and
single-arg `RasHangUpA/W(HRASCONN)` (destroyers). `HRASCONN` is a
distinct pointer-sized opaque typedef (not raw `HANDLE`).

Checked `autoTypes.json` first: **`HRASCONN` had an existing entry
but was MISSING `CloseApi`** (`ValueType: DECLARE_HANDLE`,
`InvalidHandleValues: [-1, 0]`, no `CloseApi`) — a genuine gap in
already-present metadata, not a new type. Added the missing
`CloseApi: RasHangUpW` (canonical Unicode variant; `RasHangUpA` is
the equivalent ANSI export). Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=RRas` → 0 errors.

## Conclusion

Producer-site fix applied — filled in the missing `CloseApi` on the
pre-existing `HRASCONN` `autoTypes.json` entry.
