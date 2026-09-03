# winioctl.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Windows kernel I/O control code (`IOCTL_*`/`FSCTL_*`) constants/structs,
consumed via `DeviceIoControl` with a generic `HANDLE` at the
application layer. No callable functions declared directly in this
header — consistent with the ks.h/ksmedia.h/ntddstor.h/ntddndis.h/
winddi.h precedent for kernel driver contract headers.

## Ownership Analysis
No functions exist in this header. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
