# vfw.h — accepted-normalized

**Partition:** Multimedia
**Size:** ~148507 bytes

## Analysis

`vfw.h` (Video for Windows) declares `ICOpen`/`ICOpenFunction`
(producers of `HIC`, direct return). Checked `autoTypes.json` first:
**`HIC` already has a complete entry** (`CloseApi: ICClose`,
`InvalidHandleValues: [-1, 0]`) — producer-site ownership metadata is
already correctly in place. No gap.

## Conclusion

Clean. No patch required — existing `HIC` metadata already correct
and producer-site (not typedef-site).
