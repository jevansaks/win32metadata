# Header Report: wnnc.h

## Partitions
`Ifsk`

## Scrape validation
- Re-scraped `Ifsk` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `WNNC_NET_*`/`WNNC_CRED_MANAGER` network-type constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
