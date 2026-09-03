# Header Report: ddpcommon.h

## Partitions
`Dedup`

## Scrape validation
- Re-scraped `Dedup` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- MIDL-generated header declaring only enums (`_DedupDataPortManagerOption`, `_DedupDataPortVolumeStatus`, etc.) and data structs (`_DedupHash`, `_DedupChunk`, `_DedupStream`, etc.) plus MIDL boilerplate. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/data structs + MIDL boilerplate only, no functions).
