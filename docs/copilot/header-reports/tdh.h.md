# tdh.h — accepted-normalized

**Partition:** Etw
**Size:** ~51610 bytes

## Analysis

`tdh.h` (Trace Data Helper) declares `TdhOpenDecodingHandle`
(producer, `_Out_ PTDH_HANDLE`) and single-arg
`TdhCloseDecodingHandle(TDH_HANDLE)` (destroyer). Checked
`autoTypes.json` first: **`TDH_HANDLE` already has a complete entry**
(`ValueType: IntPtr`, `CloseApi: TdhCloseDecodingHandle`,
`InvalidHandleValues: [-1, 0]`) — producer-site ownership metadata is
already correctly in place. No gap.

## Conclusion

Clean. No patch required — existing `TDH_HANDLE` metadata already
correct and producer-site (not typedef-site).
