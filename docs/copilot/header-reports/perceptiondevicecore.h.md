# Header Report: perceptiondevicecore.h

## Partitions
`MixedReality`

## Scrape validation
- Re-scraped `MixedReality` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `PERCEPTION_PAYLOAD_FIELD`/`PERCEPTION_STATE_STREAM_TIMESTAMPS` data structs and a `DEFINE_GUID` constant. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs + GUID constant only, no functions).
