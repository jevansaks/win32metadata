# Header Report: apiquery2.h

## Partitions
`FileHistory`, `WinProg`

## Scrape validation
- `WinProg` partition previously re-scraped this session (batch `scraping-investigation-23`); result `0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Two functions: `IsApiSetImplemented(PCSTR)`/`GetApiSetModuleBaseName(PCSTR, UINT32, _Out_writes_to_(...) PWSTR, _Out_opt_ UINT32*)`. Output is a caller-allocated string buffer. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (caller-allocated string buffer output only).
