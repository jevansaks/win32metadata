# Header Report: isolatedwindowsenvironmentutils.h

## Partitions
`Isolation`

## Scrape validation
- Re-scraped `Isolation` partition (`ScanArch=x86`, default) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Two functions: `IsProcessInIsolatedWindowsEnvironment(_Out_ BOOL*)` and `IsCrossIsolatedEnvironmentClipboardContent(_Out_ BOOL*)`. Both out-params are `BOOL*` — no `HANDLE`/`DECLARE_HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (boolean query functions only).
