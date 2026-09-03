# Header Report: dv.h

## Partitions
`Audio.DirectMusic`

## Scrape validation
- Re-scraped `Audio.DirectMusic` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only DV (digital video) bitmask constants and the `DVAudInfo` plain data struct. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants + data struct only, no functions).
