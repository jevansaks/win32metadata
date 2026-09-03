# Header Report: pdhmsg.h

## Partitions
`Perf`

## Ownership audit (producer-site-only policy)
- Message-Compiler-generated file containing only a provider GUID constant and `#define` PDH status/message code constants (`PDH_*`/`MSG_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
