# ntddndis.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
NDIS network driver interface constants/structs, consumed by drivers
directly or via `DeviceIoControl`/OID requests with a generic `HANDLE`
at the application layer. No callable functions declared directly in
this header — consistent with the ks.h/ksmedia.h/ntddstor.h/winddi.h
precedent for kernel driver contract headers.

## Ownership Analysis
No functions exist in this header. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
