# Header Report: highlevelmonitorconfigurationapi.h

## Partitions
`Monitor`

## Ownership audit (producer-site-only policy)
- All functions (`GetMonitorCapabilities`/`SetMonitorBrightness`/`SetMonitorContrast`/etc.) take `HANDLE hMonitor` as a pre-owned caller input only — the physical monitor handle is produced elsewhere by `physicalmonitorenumerationapi.h`'s `GetPhysicalMonitorsFromHMONITOR` (already `blocked`, established generic-type-nested-in-struct class). This header never produces a new opaque handle itself.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle; `HANDLE` param is a pre-owned caller input from a separately-tracked header).
