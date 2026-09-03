# Header Report: ndkinfo.h

## Partitions
`Ndis`

## Scrape validation
- Re-scraped `Ndis` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only data structs (`NDK_VERSION`, `NDK_ADAPTER_INFO`), the `NDK_RDMA_TECHNOLOGY` enum, and `NDK_ADAPTER_FLAG_*` constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs/enum/constants only, no functions).
