# Header Report: syncdeviceservice.h

## Partitions
`WpdSdk`

## Ownership audit (producer-site-only policy)
- Declares only `DEFINE_DEVSVCGUID`/`DEFINE_DEVSVCPROPKEY` GUID/property-key constants and `NAME_*`/`ENUM_*` string/constant macros (same pattern as `notesdeviceservice.h`/`ringtonedeviceservice.h`/`deviceservices.h`/`metadatadeviceservice.h`/`taskdeviceservice.h`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/property-key constants only, no functions).
