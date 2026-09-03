# Header Report: dxva2trace.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Pure ETW tracing GUID constants (`DXVA2Trace_*`) and plain data structs (`DXVA2Trace_DecodeDevCreatedData`, etc., all containing raw `ULONGLONG` object-pointer fields for tracing purposes, not owned handles). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (ETW tracing constants/structs only, no functions).
