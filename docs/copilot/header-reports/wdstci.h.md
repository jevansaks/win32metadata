# Header Report: wdstci.h

## Partitions
`Wds`

## Ownership audit (producer-site-only policy)
- Declares only `#define` constants, enums (`TRANSPORTCLIENT_CALLBACK_ID`), and plain data structs (`TRANSPORTCLIENT_SESSION_INFO`) describing the content-receiver/transport-client interface. No functions or callback-function-pointer typedefs declared in this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/enums/structs only, no functions).
