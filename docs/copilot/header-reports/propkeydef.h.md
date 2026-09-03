# Header Report: propkeydef.h

## Partitions
`Console`, `Shell`, `WinContacts`, `WpdSdk`

## Scrape validation
- Re-scraped `Shell` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `in6_addr`/`in_addr`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only `PROPERTYKEY`-related macros (`DEFINE_PROPERTYKEY`, `REFPROPERTYKEY`, `IsEqualPropertyKey`) and C++ equality operator overloads. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (macros/operators only, no functions).
