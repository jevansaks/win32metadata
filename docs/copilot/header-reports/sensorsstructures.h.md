# Header Report: sensorsstructures.h

## Partitions
`WinSensors`

## Scrape validation
- Re-scraped `WinSensors` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only plain data structs (`VEC3D`, `MATRIX3X3`, `QUATERNION`) and the `AXIS` enum. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs/enum only, no functions).
