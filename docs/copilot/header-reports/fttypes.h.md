# Header Report: fttypes.h

## Partitions
`Fs`

## Ownership audit (producer-site-only policy)
- Declares only a typedef (`FT_LOGICAL_DISK_ID`), enums (`FT_LOGICAL_DISK_TYPE`, `FT_MEMBER_STATE`), and plain data structs (`FT_PARTITION_CONFIGURATION_INFORMATION`, `FT_STRIPE_SET_*`, `FT_MIRROR_SET_*`, etc.). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (typedef/enums/data structs only, no functions).
