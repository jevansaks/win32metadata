# Header Report: statehelpers.h

## Partitions
`FileHistory`, `Registry`

## Scrape validation
- Re-scraped `Registry` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Single function: `GetRegistryValueWithFallbackW(__in_opt HKEY hkeyPrimary, ..., __in_opt HKEY hkeyFallback, ...)`. Both `HKEY` parameters are caller-supplied **inputs** (existing, already-open registry keys) — this function does not produce or release an `HKEY`. Output is a raw `PVOID`/`LPDWORD` data buffer.

## Conclusion
`accepted-normalized` — no ownership annotation required (HKEY parameters are inputs only, not produced here).
