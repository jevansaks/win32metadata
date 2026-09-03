# Header Report: devioctl.h

## Partitions
`Devices.1394`, `Devices.Beep`, `Devices.Cdrom`, `Devices.Dvd`

## Ownership audit (producer-site-only policy)
- Pure `FILE_DEVICE_*` device-type `#define` constants used to construct `CTL_CODE` IOCTL values. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
