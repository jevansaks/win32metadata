# Header Report: winusbio.h

## Partitions
`Buses`

## Scrape validation
- Re-scraped `Buses` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only pipe/power-policy constants, a `DEFINE_GUID`, and plain data structs (`WINUSB_PIPE_INFORMATION`, `WINUSB_PIPE_INFORMATION_EX`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants + data structs only, no functions).
