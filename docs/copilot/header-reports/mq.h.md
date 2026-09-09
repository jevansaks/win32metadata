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
   precedent.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/Mq.h`:
```
MQCreateCursor(
    _In_ QUEUEHANDLE hQueue,
    _Out_ PHANDLE phCursor
        _Win32_metadata_raii_free_(MQCloseCursor)
    );
```
Added the `#if defined(WIN32METADATA) #include <win32metadata_annotations.h>
#endif` guard block (no prior patch existed for this header). The former
sidecar entry `MQCreateCursor::phCursor=[RAIIFree("MQCloseCursor")]` was
removed from `emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/Mq.h.metadata.patch` against the
pristine `d154186c` SDK baseline. `QUEUEHANDLE` remains a separate
type-level `autoTypes.json` entry, unaffected by this change.

## Validation
- Patch replay: `git apply` of `Mq.h.metadata.patch` against the `d154186c`
  baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (MessageQueuing, `-p:ScanArch=crossarch`): Build succeeded,
  0 Error(s).

## Conclusion

Producer-site fixes applied: new `QUEUEHANDLE` `autoTypes.json` entry
and new inline `RAIIFree` annotation for `MQCreateCursor::phCursor`.
