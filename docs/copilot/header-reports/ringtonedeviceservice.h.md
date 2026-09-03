# Header Report: ringtonedeviceservice.h

## Partitions
`WpdSdk`

## Ownership audit (producer-site-only policy)
- Declares only `DEFINE_DEVSVCGUID`/`DEFINE_DEVSVCPROPKEY` GUID constants and `NAME_*`/`TYPE_*` string/constant macros (Windows Portable Devices "Ringtones" service identifiers) — same pattern as the already-accepted `notesdeviceservice.h`. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/name constants only, no functions).
