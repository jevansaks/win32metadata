# Header Report: tcpxcv.h

## Partitions
`Printing`

## Ownership audit (producer-site-only policy)
- Declares only plain data structs (`PORT_DATA_1`, `PORT_DATA_2`, `PORT_DATA_LIST_1`, `DELETE_PORT_DATA_1`, `CONFIG_INFO_DATA_1`) and protocol-type constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs + constants only, no functions).
