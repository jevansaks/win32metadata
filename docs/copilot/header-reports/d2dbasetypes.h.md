# Header Report: d2dbasetypes.h

## Partitions
`Direct2D`

## Scrape validation
- Re-scraped `Direct2D` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only a single type alias: `typedef D3DCOLORVALUE D2D_COLOR_F;`. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (type alias only, no functions).
