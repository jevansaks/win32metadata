# Header Report: tvout.h

## Partitions
`Gdi`

## Scrape validation
- Re-scraped `Gdi` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only the `_VIDEOPARAMETERS` data struct and `VP_*` bitfield/command constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants only, no functions).
