# Header Report: notesdeviceservice.h

## Partitions
`WpdSdk`

## Scrape validation
- Re-scraped `WpdSdk` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `DEFINE_DEVSVCGUID` GUID constants and `NAME_*`/`TYPE_*` string/constant macros (Windows Portable Devices "Notes" service identifiers). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/name constants only, no functions).
