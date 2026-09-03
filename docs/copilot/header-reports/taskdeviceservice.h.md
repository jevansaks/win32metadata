# Header Report: taskdeviceservice.h

## Partitions
`WpdSdk`

## Ownership audit (producer-site-only policy)
- Declares only `DEFINE_DEVSVCGUID`/`DEFINE_DEVSVCPROPKEY` GUID/property-key constants and `NAME_*`/`TYPE_*`/`ENUM_*` string/constant macros (same pattern as the already-accepted `notesdeviceservice.h`/`ringtonedeviceservice.h`/`deviceservices.h`/`metadatadeviceservice.h`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/property-key constants only, no functions).
