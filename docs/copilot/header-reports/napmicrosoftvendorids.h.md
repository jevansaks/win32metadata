# Header Report: napmicrosoftvendorids.h

## Partitions
`NetworkAccessProtection`

## Scrape validation
- Re-scraped `NetworkAccessProtection` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `static UINT32` NAP vendor-ID constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (integer constants only, no functions).
