# Header Report: swdevicedef.h

## Partitions
`SwDevice`

## Scrape validation
- Re-scraped `SwDevice` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only the `SW_DEVICE_CAPABILITIES`/`SW_DEVICE_LIFETIME` enums and the `SW_DEVICE_CREATE_INFO` data struct. **No functions are declared in this header.**
- The actual functions producing/consuming the `HSWDEVICE` handle (`SwDeviceCreate` → `HSWDEVICE`, `SwDeviceClose(HSWDEVICE)`) live in a separate, already-tracked header (`swdevice.h`, still `pending`) — that is where any future producer-site ownership work belongs, not here.

## Conclusion
`accepted-normalized` — this header declares only data types (enums/struct), no functions; the `HSWDEVICE` ownership work is tracked separately under `swdevice.h` (still pending).
