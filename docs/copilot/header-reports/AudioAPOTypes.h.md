# Header Report: AudioAPOTypes.h

## Partitions
`Audio.Apo`, `TermServ`

## Scrape validation
- Re-scraped `Audio.Apo` and `TermServ` partitions (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (both)

## Ownership audit (producer-site-only policy)
- Declares only typedefs (`HNSTIME`, `MFTIME`, `FLOAT32`, `FLOAT64`), an enum (`APO_BUFFER_FLAGS`), and plain data structs (`APO_CONNECTION_PROPERTY`, `APO_CONNECTION_PROPERTY_V2`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (typedefs/enum/structs only, no functions).
