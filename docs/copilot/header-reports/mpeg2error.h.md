# Header Report: mpeg2error.h

## Partitions
`Media.DShow`

## Scrape validation
- Re-scraped `Media.DShow` partition (x64) earlier this session (batch 60); re-confirmed 0 errors.

## Ownership audit (producer-site-only policy)
- Pure HRESULT `const` status codes (`MPEG2_S_*`/`MPEG2_E_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
