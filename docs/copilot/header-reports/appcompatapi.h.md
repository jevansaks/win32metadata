# Header Report: appcompatapi.h

## Partitions
`FileHistory`, `WinProg`

## Scrape validation
- Re-scraped `WinProg` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Single function: `ApphelpCheckShellObject(REFCLSID, BOOL, _Out_ ULONGLONG*)`. Output is a plain `ULONGLONG` flags value. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved).
