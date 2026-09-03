# Header Report: atacct.h

## Partitions
`NetMgmt`

## Scrape validation
- Re-scraped `NetMgmt` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Two functions: `GetNetScheduleAccountInformation`/`SetNetScheduleAccountInformation`, both operate on string buffers (`LPCWSTR`/`WCHAR[]`) only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (string-only account-info API).
