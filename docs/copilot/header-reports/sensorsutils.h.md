# Header Report: sensorsutils.h

## Partitions
`WinSensors`

## Ownership audit (producer-site-only policy)
- All functions (`GetPerformanceTime`, `InitPropVariantFromFloat`, `PropKeyFindKeyGet*`/`PropKeyFindKeySetPropVariant`) output plain value types (`PULONG`/`FILETIME`/`GUID`/`BOOL`/`ULONG`/etc.) or operate on transparent `PROPVARIANT`/`SENSOR_COLLECTION_LIST` structs — no opaque handle produced.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle).
