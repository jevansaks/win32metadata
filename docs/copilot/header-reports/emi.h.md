# Header Report: emi.h

## Partitions
`Power`

## Ownership audit (producer-site-only policy)
- Declares only a `DEFINE_GUID`, `IOCTL_EMI_*` constants, an enum (`EMI_MEASUREMENT_UNIT`), and plain data structs (`EMI_VERSION`, `EMI_METADATA_SIZE`, `EMI_CHANNEL_MEASUREMENT_DATA`, `EMI_METADATA_V1/V2`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/IOCTL constants + data structs only, no functions).
