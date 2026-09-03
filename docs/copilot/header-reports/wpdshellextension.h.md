# Header Report: wpdshellextension.h

## Partitions
`WpdSdk`

## Scrape validation
- Re-scraped `WpdSdk` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `DEFINE_GUID`/`DEFINE_PROPERTYKEY` constants and `#define` bind-context/property-sheet integer constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/property-key constants only, no functions).
