# Header Report: dxgicommon.h

## Partitions
`Dxgi`, `Dxgi.Common`

## Scrape validation
- `Dxgi` partition previously re-scraped this session (batch `scraping-investigation-29`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only data structs (`DXGI_RATIONAL`, `DXGI_SAMPLE_DESC`), the `DXGI_COLOR_SPACE_TYPE` enum, and `DXGI_*_MULTISAMPLE_QUALITY_PATTERN` constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs/enum/constants only, no functions).
