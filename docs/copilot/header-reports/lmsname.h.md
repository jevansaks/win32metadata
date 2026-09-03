# Header Report: lmsname.h

## Partitions
`NetMgmt`

## Scrape validation
- Re-scraped `NetMgmt` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Pure `#define` service-name string constants (`SERVICE_*`/`SERVICE_LM20_*`/`*_DISPLAY_NAME`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (string constants only, no functions).
