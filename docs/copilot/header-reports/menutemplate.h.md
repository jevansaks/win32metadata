# Header Report: menutemplate.h

## Partitions
`MenuRc`

## Scrape validation
- `MenuRc` partition re-scraped earlier this session (batch `scraping-investigation-14`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only plain data structs (`_MENUEX_TEMPLATE_HEADER`, `_MENUEX_TEMPLATE_ITEM`, `_MENUTEMPLATEEX`) used to interpret raw menu-resource byte blobs. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (plain data structs only, no functions).
