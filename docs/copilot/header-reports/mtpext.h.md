# Header Report: mtpext.h

## Partitions
`Wmdm`

## Scrape validation
- `Wmdm` partition previously re-scraped this session (batch `scraping-investigation-43`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only `IOCTL_MTP_CUSTOM_COMMAND`/`MTP_*` constants and the `MTP_COMMAND_DATA_IN`/`MTP_COMMAND_DATA_OUT` data structs (passed through `IWMDMDevice3::DeviceIoControl`, a COM method). No `DllImport` functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants + data structs only, no functions).
