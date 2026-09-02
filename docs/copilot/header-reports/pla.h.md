# Header Report: pla.h

## Partitions
`Pla`

## Scrape validation
- Re-scraped `Pla` partition (`ScanArch=x86`, required — `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 6 `DllImport` functions, 1313 auto-remaps written.

## Ownership audit (producer-site-only policy)
- No `DECLARE_HANDLE` in `pla.h`.
- All 6 scraped functions (`PlaExpandTaskArguments`, `PlaGetServerCapabilities`, `PlaGetLegacyAlertActionsStringFromFlags`, `PlaGetLegacyAlertActionsFlagsFromString`, `PlaExtractCabinet`, `PlaDeleteReport`) operate on strings/flags/file paths only — no `HANDLE`-producing or `HANDLE`-consuming parameters.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle-producing functions in this header).
