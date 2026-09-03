# Header Report: stierr.h

## Partitions
`ImagingDevice`

## Scrape validation
- Re-scraped `ImagingDevice` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Pure HRESULT/status-code `#define` constants (`STI_*`/`STIERR_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
