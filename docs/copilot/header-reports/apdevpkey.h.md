# Header Report: apdevpkey.h

## Partitions
`Base`, `Devices.Properties`

## Scrape validation
- Re-scraped `Devices.Properties` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only a single `DEFINE_DEVPROPKEY(DEVPKEY_DeviceInterface_Autoplay_Silent, ...)` constant. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (device property key constant only).
