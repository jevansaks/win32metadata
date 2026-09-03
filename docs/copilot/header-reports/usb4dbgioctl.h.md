# Header Report: usb4dbgioctl.h

## Partitions
`Buses`

## Scrape validation
- `Buses` partition previously re-scraped this session (batch `scraping-investigation-45`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only enums (`USB4_CONFIG_SPACE_TYPE`, `USB4_STATUS`), IOCTL/GUID constants, and plain data structs (`_USB4_HRD_DEBUG_ROUTE_STRING`, `_USB4_HRD_DEBUG_READ_CONFIGURATION_SPACE_INPUT/OUTPUT`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/constants/data structs only, no functions).
