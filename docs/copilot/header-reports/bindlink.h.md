# Header Report: bindlink.h

## Partitions
`Fs`

## Scrape validation
- Re-scraped `Fs` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Two functions: `CreateBindLink`/`RemoveBindLink`, both operate on `PCWSTR` file-system paths only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (path-string-only API, no handle).
