# Header Report: usbprint.h

## Partitions
`Printing`

## Ownership audit (producer-site-only policy)
- Pure `DEFINE_GUID`/`#define` constants: device-interface GUIDs and `IOCTL_USBPRINT_*` control codes (`CTL_CODE` macro invocations). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/IOCTL constants only, no functions).
