# ioevent.h — accepted-normalized

**Partition:** MenuRc
**Size:** ~11107 bytes

## Analysis

`ioevent.h` (shared\ioevent.h) declares I/O system event GUIDs
(`GUID_IO_*`) and transparent event-data structs
(`DEVICE_EVENT_MOUNT`, `DEVICE_EVENT_BECOMING_READY`,
`DEVICE_EVENT_EXTERNAL_REQUEST`, etc.) reported via
`IoReportTargetDeviceChangeAsynchronous`/WMI notification, not via any
function declared in this header. It contains **no function
declarations**.

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
