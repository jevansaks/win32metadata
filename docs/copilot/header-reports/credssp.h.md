# Header Report: credssp.h

## Partitions
`Credentials`

## Scrape validation
- `Credentials` partition previously re-scraped this session (batch `scraping-investigation-39`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares only structs (`SecPkgContext_ClientCreds`, `CREDSSP_CRED`, `CREDSSP_CRED_EX`), the `_CREDSSP_SUBMIT_TYPE` enum, and `CREDSSP_*`/`SECPKG_ATTR_*` constants.
- `SpInitSecurityInterfaceW(VOID)` (guarded by `SECURITY_KERNEL`, kernel-only) returns `PSecurityFunctionTableW` — a pointer to a static/singleton SSPI function-dispatch table (standard SSPI pattern; the table is not a caller-owned allocation to be freed). No `HANDLE` production requiring ownership annotation.

## Conclusion
`accepted-normalized` — no ownership annotation required (structs/enum/constants only; the one function returns a non-owned static function table, not an owned resource).
