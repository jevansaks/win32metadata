# Header Report: presentationtypes.h

## Partitions
`CompositionSwapchain`

## Scrape validation
- Re-scraped `CompositionSwapchain` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- MIDL-generated header declaring only plain data structs (`SystemInterruptTime`, `PresentationTransform`), a typedef (`CompositionFrameId`), the `PresentStatisticsKind` enum, and standard MIDL boilerplate (`RPC_IF_HANDLE` ifspec externs). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs/enum/typedef + MIDL boilerplate only, no functions).
