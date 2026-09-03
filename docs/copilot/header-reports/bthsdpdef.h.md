# Header Report: bthsdpdef.h

## Partitions
`Bluetooth`

## Scrape validation
- Re-scraped `Bluetooth` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only SDP (Service Discovery Protocol) data structs, enums, and a union (`SDP_LARGE_INTEGER_16`, `NodeContainerType`, `SDP_TYPE`, `SDP_SPECIFICTYPE`, `SdpAttributeRange`, `SdpQueryUuidUnion`, `SdpQueryUuid`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs/enums/union only, no functions).
