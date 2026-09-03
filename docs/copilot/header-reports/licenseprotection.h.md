# Header Report: licenseprotection.h

## Partitions
`Security.LicenseProtection`

## Scrape validation
- Re-scraped `Security.LicenseProtection` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (2 pre-existing unrelated cross-partition remap warnings for `_CERT_CONTEXT`/`SecPkgContext_IssuerListInfoEx`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Two functions: `RegisterLicenseKeyWithExpiration(PCWSTR, UINT32, _Out_ LicenseProtectionStatus*)` and `ValidateLicenseKeyProtection(PCWSTR, _Out_ PFILETIME, _Out_ PFILETIME, _Out_ LicenseProtectionStatus*)`. All outputs are plain enum/`FILETIME` values. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum/FILETIME output only, no handle).
