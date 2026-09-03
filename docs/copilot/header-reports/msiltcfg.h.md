# Header Report: msiltcfg.h

## Partitions
`Setup`

## Scrape validation
- Re-scraped `Setup` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Two functions: `ShutdownMsi(DWORD dwReserved)`/`RestartMsi(DWORD dwReserved)`. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved).
