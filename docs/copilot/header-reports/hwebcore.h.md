# Header Report: hwebcore.h

## Partitions
`Iis`

## Scrape validation
- Re-scraped `Iis` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares only `#define` DLL/entry-point name string constants and function-pointer *typedefs* (`PFN_WEB_CORE_SET_METADATA_DLL_ENTRY`, `PFN_WEB_CORE_ACTIVATE`, `PFN_WEB_CORE_SHUTDOWN`) intended for `GetProcAddress` resolution against `hwebcore.dll` — no `extern`/`DllImport` function declarations exist to attach a producer-site annotation to. Out of scope for the same reason as other function-pointer-typedef patterns (`mswsockdef.h`).

## Conclusion
`accepted-normalized` — no ownership annotation required (dynamically-loaded function-pointer typedefs only, no scrapable extern functions).
