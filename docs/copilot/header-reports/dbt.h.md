# dbt.h — accepted-normalized

**Partition:** MenuRc
**Size:** ~15962 bytes

## Analysis

`dbt.h` (`WM_DEVICECHANGE`/`BroadcastSystemMessage` equates) declares
only constants and transparent `DEV_BROADCAST_*` structs consumed by
`RegisterDeviceNotification` (declared elsewhere). It contains **no
function declarations** (0 `WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
