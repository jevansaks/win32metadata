# Header Report: cryptcat.h

## Partitions
`Security.Cryptography.Catalog`

## Scrape validation
- Re-scraped `Security.Cryptography.Catalog` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (2 pre-existing unrelated cross-partition remap warnings for `_CERT_CONTEXT`/`SecPkgContext_IssuerListInfoEx`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Two functions: `CryptCATCDFEnumMembersByCDFTagEx`/`CryptCATCDFEnumAttributesWithCDFTag`. Both take an already-open `CRYPTCATCDF*`/`CRYPTCATMEMBER*` as `_In_` parameters (enumeration over an existing catalog-definition-file handle) — neither opens nor closes a `CRYPTCATCDF`.
- The actual `CRYPTCATCDF` producer/consumer pair (`CryptCATCDFOpen`/`CryptCATCDFClose`) lives in `mscat.h` (`#include <mscat.h>` at the top of this header), which is separately tracked in the ledger and still `pending` — that is where any genuine ownership-annotation work for `CRYPTCATCDF` belongs, not in `cryptcat.h`.

## Conclusion
`accepted-normalized` — this header's own declarations only *consume* an existing catalog handle (enumeration functions), they do not produce or release one. The producer/consumer pair itself is tracked separately under `mscat.h` (still pending).
