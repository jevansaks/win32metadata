# Header Report: vdslun.h

## Partitions
`VirtualDiskService`

## Ownership audit (producer-site-only policy)
- Declares only enums (`VDS_STORAGE_IDENTIFIER_CODE_SET`, `VDS_STORAGE_IDENTIFIER_TYPE`, `VDS_STORAGE_BUS_TYPE`, `VDS_INTERCONNECT_ADDRESS_TYPE`) and plain data structs (`VDS_STORAGE_IDENTIFIER`, `VDS_STORAGE_DEVICE_ID_DESCRIPTOR`, `VDS_INTERCONNECT`, `VDS_LUN_INFORMATION`) plus MIDL boilerplate. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/structs + MIDL boilerplate only, no functions).
