# Header Report: atsmedia.h

## Partitions
`MsTv`

## Scrape validation
- Re-scraped `MsTv` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only a `BDANETWORKTYPE_ATSC` GUID-struct constant (via `DEFINE_GUIDSTRUCT`/`DEFINE_GUIDNAMED`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constant only, no functions).
