# Header Report: nfcradiodev.h

## Partitions
`Nfc`

## Scrape validation
- Re-scraped `Nfc` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares GUID constants, `IOCTL_NFCRM_*`/`IOCTL_NFCSERM_*` IOCTL codes, and plain data structs (`NFCRM_SET_RADIO_STATE`, `NFCRM_RADIO_STATE`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/IOCTL constants + data structs only, no functions).
