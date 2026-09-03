# Header Report: deviceservices.h

## Partitions
`WpdSdk`

## Ownership audit (producer-site-only policy)
- `deviceservices.h` `#include`s `BridgeDeviceService.h` (already separately tracked in the ledger, still `pending`).
- Its own direct declarations are `DEVSVC_SERVICEINFO_VERSION`/`DEVSVCTYPE_*` constants and `DEFINE_DEVSVCGUID`/`DEFINE_DEVSVCPROPKEY` GUID/property-key constants (same pattern as the already-accepted `notesdeviceservice.h`/`ringtonedeviceservice.h`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/property-key constants only, no functions); `BridgeDeviceService.h` remains separately tracked (pending).
