# Header Report: tabflicks.h

## Partitions
`Tablet`

## Scrape validation
- Re-scraped `Tablet` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only enums (`FLICKDIRECTION`, `FLICKMODE`, `FLICKACTION_COMMANDCODE`, `SCROLLDIRECTION`, `KEYMODIFIER`) and bitfield data structs (`FLICK_POINT`, `FLICK_DATA`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/bitfield structs only, no functions).
