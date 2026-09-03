# Header Report: dshow.h

## Partitions
`Media.DShow`

## Scrape validation
- Re-scraped `Media.DShow` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `dshow.h` is a pure umbrella redirect header (`#include`s `ddraw.h`, `mmsystem.h`, `strmif.h`, `amvideo.h`, `amaudio.h`, `control.h`, `evcode.h`, `uuids.h`, `errors.h`, `edevdefs.h`, `audevcod.h`, `dvdevcod.h`); it declares nothing itself. All included headers are already tracked as individual ledger items (`control.h`/`ddraw.h`/`amaudio.h`/`audevcod.h`/`errors.h` already `accepted-normalized`; `strmif.h`/`evcode.h`/`uuids.h`/`edevdefs.h`/`dvdevcod.h`/`mmsystem.h` still `pending`) — no audit coverage is lost by closing this umbrella.

## Conclusion
`accepted-normalized` — pure umbrella redirect header; all included sub-headers are already individually tracked ledger items.
