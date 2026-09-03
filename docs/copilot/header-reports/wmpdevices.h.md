# Header Report: wmpdevices.h

## Partitions
`Wmp`

## Ownership audit (producer-site-only policy)
- `WMPNotifyDeviceArrival`/`WMPNotifyDeviceRemoval` are `__inline` functions — no `extern "C"`/`DllImport` linkage, so nothing is scraped as a callable P/Invoke binding.
- Remaining declarations are IOCTL constants and plain data structs (`WMP_WMDM_METADATA_ROUND_TRIP_PC2DEVICE`/`DEVICE2PC`) for `IMDSPDevice3::DeviceIoControl` interop.

## Conclusion
`accepted-normalized` — no ownership annotation required (inline functions generate no bindings; remaining declarations are IOCTL constants/structs).
