# Header Report: WaaSApiTypes.h

## Partitions
`UpdateAssessment`

## Scrape validation
- Re-scraped `UpdateAssessment` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Pure MIDL-generated types header: enums (`UpdateImpactLevel`, `UpdateAssessmentStatus`), plain data structs (`UpdateAssessment`, `OSUpdateAssessment`, `CloudCampaignAssessment`), and MIDL boilerplate (`RPC_IF_HANDLE` ifspec externs). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/structs + MIDL boilerplate only, no functions).
