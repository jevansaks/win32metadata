# Header Report: audioclientactivationparams.h

## Partitions
`Audio`

## Scrape validation
- Re-scraped `Audio` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- MIDL-generated header declaring only enums (`PROCESS_LOOPBACK_MODE`, `AUDIOCLIENT_ACTIVATION_TYPE`), data structs (`AUDIOCLIENT_PROCESS_LOOPBACK_PARAMS`, `AUDIOCLIENT_ACTIVATION_PARAMS`), and MIDL boilerplate — no functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/data structs + MIDL boilerplate only, no functions).
