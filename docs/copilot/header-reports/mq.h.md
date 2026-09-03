# mq.h — accepted-normalized (2 producer-site fixes applied)

**Partition:** MessageQueuing
**Size:** ~74857 bytes

## Analysis

`mq.h` (MSMQ API) declares two genuine handle-producing patterns:

1. **`QUEUEHANDLE`** (`typedef HANDLE QUEUEHANDLE;` — a distinct
   named typedef, not the raw `HANDLE` type) — produced by
   `MQOpenQueue` (`_Out_ QUEUEHANDLE*`), destroyed by single-arg
   `MQCloseQueue(QUEUEHANDLE)`. **No `autoTypes.json` entry
   existed** — added new entry (`ValueType: DECLARE_HANDLE`,
   `CloseApi: MQCloseQueue`, `InvalidHandleValues: [0]`).

2. **`MQCreateCursor`** outputs a generic `_Out_ PHANDLE phCursor`
   (not a distinct typedef) — a semantically specific "MSMQ cursor
   handle", closed via single-arg `MQCloseCursor(HANDLE)`. Directly
   analogous to the `CfOpenFileWithOplock`/`QOSCreateHandle`
   precedent. **No existing inline `emitter.settings.rsp`
   annotation** — added
   `MQCreateCursor::phCursor=[RAIIFree("MQCloseCursor")]`.

Validated via `dotnet build generation/WinSDK -c Release
-p:ScanArch=x64 -t:ScrapeHeaders -p:PartitionFilter=MessageQueuing` →
0 errors.

## Conclusion

Producer-site fixes applied: new `QUEUEHANDLE` `autoTypes.json` entry
and new inline `RAIIFree` annotation for `MQCreateCursor::phCursor`.
