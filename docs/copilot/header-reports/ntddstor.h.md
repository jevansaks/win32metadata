# ntddstor.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Storage class driver IOCTL constants/structs (`IOCTL_STORAGE_*`, kernel
driver contract consumed via `DeviceIoControl` with a generic `HANDLE`
at the application layer). No callable functions declared directly in
this header — consistent with the winddi.h/ntddvdeo.h precedent for
kernel driver contract headers.

## Ownership Analysis
No functions exist in this header, so no producer/consumer handle
relationship is annotatable here (the consuming `HANDLE` is generic,
already covered by the shared-type blocker class). No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
