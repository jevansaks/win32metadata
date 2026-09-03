# Header Report: FaxDev.h

## Partitions
`Fax`

## Ownership audit (producer-site-only policy)
- `FaxDevStartJob(..., OUT PHANDLE FaxHandle, ...)` produces a generic `HANDLE` via a direct out-param, closed via `FaxDevEndJob(IN HANDLE FaxHandle)` — the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`/`ondemandconnroutehelper.h`/`davclnt.h`/`RTWorkQ.h`).
- `FaxDevInitialize(..., OUT PFAX_LINECALLBACK *LineCallbackFunction, ...)` outputs a function-pointer typedef, not an opaque handle — out of scope.
- `FaxDevVirtualDeviceCreation`/`FaxDevReportStatus`/`FaxDevConfigure` write into caller-allocated buffers/output plain `DWORD`/`HPROPSHEETPAGE*` (a standard dialog-page resource, not tracked by this mechanism) — no additional gaps.

## Conclusion
`blocked` — genuine gap in `FaxDevStartJob`/`FaxDevEndJob` (generic `HANDLE` direct-out-param, reuses established blocker class).
