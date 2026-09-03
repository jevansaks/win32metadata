# Header Report: dxgitype.h

## Partitions
`Dxgi`, `Dxgi.Common`

## Scrape validation
- `Dxgi` partition previously re-scraped this session (batch `scraping-investigation-38`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only data structs (`DXGI_RGB`, `DXGI_GAMMA_CONTROL*`, `DXGI_MODE_DESC`, `DXGI_JPEG_*`) and enums (`DXGI_MODE_SCANLINE_ORDER`, `DXGI_MODE_SCALING`, `DXGI_MODE_ROTATION`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs/enums only, no functions).
