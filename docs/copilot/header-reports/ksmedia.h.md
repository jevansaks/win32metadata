# ksmedia.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
WDM-CSA (Windows Driver Model - Circuit Switched Audio) Kernel Streaming
multimedia IOCTL constants/structs, requires `ks.h` to be included
first. Same category as `ks.h`/`ntddstor.h`: kernel driver contract
constants/structs consumed via `DeviceIoControl` with a generic
`HANDLE`, no callable functions declared directly in this header.

## Ownership Analysis
No functions exist in this header. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
