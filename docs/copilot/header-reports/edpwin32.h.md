# Header Report: edpwin32.h

## Partitions
`Edp`

## Ownership audit (producer-site-only policy)
- Two functions: `ProtectFileToEnterpriseIdentity(PCWSTR, PCWSTR)` and `UnprotectFile(PCWSTR, const FILE_UNPROTECT_OPTIONS*)`. Both operate on file paths and a plain options struct — no `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (path-string-only API, no handle involved).
