# Header Report: windows.graphics.interop.h

## Partitions
`WinRT.Direct2D`

## Scrape validation
- Re-scraped `WinRT.Direct2D` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `IGeometrySource2DInterop::GetGeometry`/`TryGetGeometryUsingFactory` are clean COM QueryInterface-style factory methods (`_COM_Outptr_ ID2D1Geometry**`). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory pattern).
