# Header Report: imapierror.h

## Partitions
`IMapi`

## Scrape validation
- Re-scraped `IMapi` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Pure HRESULT `const` status codes (`IMAPI_*`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
