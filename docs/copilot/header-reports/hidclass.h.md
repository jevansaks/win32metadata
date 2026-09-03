# Header Report: hidclass.h

## Partitions
`Hid`

## Ownership audit (producer-site-only policy)
- Pure device-interface GUID constants (`GUID_DEVINTERFACE_HID`/etc.) and `DEFINE_DEVPROPKEY` device-property-key constants for HID class driver clients. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/property-key constants only, no functions).
