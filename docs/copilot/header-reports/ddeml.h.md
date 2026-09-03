# ddeml.h — accepted-normalized (patch applied)

**Partition:** DataXchg (`Windows.Win32.System.DataExchange`)
**Size:** ~19365 bytes

## Analysis

`ddeml.h` (DDEML API) declares four `DECLARE_HANDLE` types:
`HCONVLIST`, `HCONV`, `HSZ`, `HDDEDATA`. Checking `autoTypes.json`:

| Type | Producer | Prior state |
|---|---|---|
| `HCONVLIST` | `DdeConnectList` | Complete (`CloseApi: DdeDisconnectList`) |
| `HCONV` | `DdeConnect` | Complete (`CloseApi: DdeDisconnect`) |
| `HDDEDATA` | `DdeCreateDataHandle` | Complete (`CloseApi: DdeFreeDataHandle`) |
| `HSZ` | `DdeCreateStringHandleA`/`W` | **Missing `CloseApi`** |

`HSZ` is freed via `DdeFreeStringHandle`, confirmed used only in this
one file (repo-wide grep).

## Patch

Edited `generation/WinSDK/autoTypes.json` — added
`"CloseApi": "DdeFreeStringHandle"` to the existing `HSZ` entry.

## Validation

Re-scraped the `DataXchg` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — completed a pre-existing
incomplete `autoTypes.json` entry; the other three types were already
complete.
