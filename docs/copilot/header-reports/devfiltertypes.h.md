# Header Report: devfiltertypes.h

## Partitions
`DeviceQuery`

## Scrape validation
- Re-scraped `DeviceQuery` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only an enum (`DEVPROP_OPERATOR`) and a plain data struct (`DEVPROP_FILTER_EXPRESSION`). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum/struct only, no functions).
