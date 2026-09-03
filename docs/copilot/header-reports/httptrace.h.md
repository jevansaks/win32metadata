# Header Report: httptrace.h

## Partitions
`Iis`

## Ownership audit (producer-site-only policy)
- Declares only an enum (`HTTP_TRACE_TYPE`), plain data structs (`HTTP_TRACE_EVENT`, `HTTP_TRACE_EVENT_ITEM`, `HTTP_TRACE_CONFIGURATION`) for the IIS ETW trace infrastructure, and `DEFINE_GUID` trace-provider constants. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum/structs/GUID constants only, no functions).
