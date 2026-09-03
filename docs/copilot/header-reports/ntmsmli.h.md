# Header Report: ntmsmli.h

## Partitions
`Fs`

## Scrape validation
- `Fs` partition previously re-scraped this session (batch `scraping-investigation-25`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares the `MediaLabelInfo` data struct and three function-pointer *typedefs* (`MAXMEDIALABEL`, `CLAIMMEDIALABEL`, `CLAIMMEDIALABELEX`) used as callback signatures by media label libraries — **no actual `DllImport` functions** are declared. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + callback-signature typedefs only, no functions).
