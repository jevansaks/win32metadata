# evntprov.h — accepted-normalized

**Partition:** Etw
**Size:** ~37482 bytes

## Analysis

`evntprov.h` (ETW event provider API) declares `EventRegister`
(producer, `_Out_ PREGHANDLE RegHandle`) and `EventUnregister`
(destroyer, single-arg). Checked `autoTypes.json` first: **`REGHANDLE`
already has a complete entry** (`ValueType: long`, `CloseApi:
EventUnregister`, `InvalidHandleValues: [0]`) — producer-site
ownership metadata is already correctly in place. No gap.

## Conclusion

Clean. No patch required — existing `REGHANDLE` metadata already
correct and producer-site (not typedef-site).
