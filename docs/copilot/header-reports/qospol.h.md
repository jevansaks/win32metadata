# Header Report: qospol.h

## Partitions
`Qos`

## Scrape validation
- Re-scraped `Qos` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `sockaddr`/`timeval`/`_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares only the `IDPE_ATTR` data struct and `PE_*`/`CREDENTIAL_*`/`POLICY_LOCATOR_*` constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants only, no functions).
