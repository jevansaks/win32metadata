# Header Report: dxprogrammablecapture.h

## Partitions
`Dxgi`, `Dxgi.Common`

## Scrape validation
- Re-scraped `Dxgi` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only the `IDXGraphicsAnalysis` COM interface (`BeginCapture()`/`EndCapture()`, no parameters at all). No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (parameterless COM interface methods, no handle).
