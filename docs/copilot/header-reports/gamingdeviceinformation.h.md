# Header Report: gamingdeviceinformation.h

## Partitions
`GamingDvcInfo`

## Scrape validation
- Re-scraped `GamingDvcInfo` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Single function: `GetGamingDeviceModelInformation(_Out_ GAMING_DEVICE_MODEL_INFORMATION* information)`. Output is a plain data struct (two enum fields). No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (plain struct output, no handle).
