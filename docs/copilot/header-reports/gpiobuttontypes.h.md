# Header Report: gpiobuttontypes.h

## Partitions
`Hid`

## Scrape validation
- Re-scraped `Hid` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares the `GPIOBUTTONS_BUTTON_TYPE` enum, `BUTTON_BIT_*`/`IOCTL_BUTTON_*` constants, and the `_INPUT_BUTTON_ENABLE_INFO` data struct. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum/constants/data struct only, no functions).
