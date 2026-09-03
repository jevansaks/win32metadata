# Header Report: ccgplugins.h

## Partitions
`Identity`

## Scrape validation
- Re-scraped `Identity` partition (x64) earlier this session (batch 62); re-confirmed 0 errors.

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `ICcgDomainAuthCredentials` (single method `GetPasswordCredentials`, outputting `LPWSTR*` strings freed via standard `CoTaskMemFree`/`SysFreeString`-style COM string conventions). COM vtable method — out of scope for the producer-site annotation mechanism (no `extern "C"`/`DllImport` linkage).

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
