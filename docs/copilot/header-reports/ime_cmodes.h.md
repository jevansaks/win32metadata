# Header Report: ime_cmodes.h

## Partitions
`Input.Ime`

## Scrape validation
- Re-scraped `Input.Ime` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `IME_CMODE_*` bit-field constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (bit-field constants only, no functions).
