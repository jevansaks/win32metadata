# Header Report: stireg.h

## Partitions
`ImagingDevice`

## Ownership audit (producer-site-only policy)
- Pure registry key/value string constants (`REGSTR_VAL_*`/`STI_DEVICE_VALUE_*`) and `DEFINE_DEVPROPKEY` GUID property-key constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
