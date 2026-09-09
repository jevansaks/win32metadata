# mscat.h — accepted-normalized (producer-site fix applied; 1 item blocked)

**Partition:** Security.Cryptography.Catalog, Security.Cryptography.Sip
**Size:** ~39804 bytes

## Analysis

`mscat.h` (Catalog API) declares several handle-producing functions.

**Correction (self-audit, batch 191):** Originally (this batch)
`HCATADMIN` was documented as blocked because its release function,
`CryptCATAdminReleaseContext(HCATADMIN, DWORD dwFlags /* reserved,
must be 0 */)`, requires a second parameter. Subsequent investigation
of `msacm.h` (batch 191) found the **already-shipped**
`HACMDRIVER`/`acmDriverClose(HACMDRIVER, DWORD fdwClose /* reserved
*/)` `autoTypes.json` entry — proving the metadata system already
tolerates a reserved-flags second parameter on `CloseApi` functions.
Corrected: added `HCATADMIN` `autoTypes.json` entry (`CloseApi:
CryptCATAdminReleaseContext`, `InvalidHandleValues: [0]`). Validated
via `dotnet build generation/WinSDK -c Release -p:ScanArch=x64
-t:ScrapeHeaders -p:PartitionFilter=Security.Cryptography.Catalog` →
0 errors.

**Still blocked / not representable**:
- `CryptCATOpen`/`CryptCATClose`: return/consume the fully generic
  `HANDLE` type directly (not a distinct typedef) — blocker-class 2.
- `HCATINFO` (`typedef HANDLE HCATINFO;`, produced by
  `CryptCATAdminEnumCatalogFromHash`/`CryptCATAdminAddCatalog`): its
  only release function, `CryptCATAdminReleaseCatalogContext(HCATADMIN
  hCatAdmin, HCATINFO hCatInfo, DWORD dwFlags)`, requires an
  **external `HCATADMIN` context parameter** in addition to the
  handle itself and the reserved flags — genuinely unrepresentable
  via a single-arg `CloseApi` (the close operation depends on
  external state beyond the handle, unlike a pure reserved-flags
  case).

## Conclusion

Producer-site fix applied for `HCATADMIN` (corrected from an
over-cautious initial block). `HCATINFO` remains unrepresentable
(external-context-dependent close); `CryptCATOpen`/`Close` use the
generic `HANDLE` type (blocker-class 2).

## WithSetLastError.rsp final tranche update

`CryptCATAdminAddCatalog` sets last error on failure (returns `NULL`
`HCATINFO` on failure per its doc contract). Migrated the sidecar
`WithSetLastError.rsp` fact to an inline `_Win32_metadata_set_last_error_`
annotation on both `#if (NTDDI_VERSION < NTDDI_WINBLUE)` / `#else`
declaration branches (each conditional branch requires its own
annotation instance per this project's convention):
```
#if (NTDDI_VERSION < NTDDI_WINBLUE)
_Win32_metadata_set_last_error_
extern HCATINFO WINAPI CryptCATAdminAddCatalog(
    ...
#else
_Win32_metadata_set_last_error_
extern HCATINFO WINAPI CryptCATAdminAddCatalog(
    ...
#endif
```
This was one of the last 2 genuinely-uncovered entries surviving a fresh
coverage re-check of the 143 previously-reported residual `WithSetLastError.rsp`
entries (141 of the 143 were already covered by pre-existing annotations
from the prior migration batch and were removed from the rsp as pure
reconciliation with no header changes). Re-consolidated into
`mscat.h.metadata.patch` against the `d154186c` baseline; validated via
patch replay (byte-for-byte) and `ScrapeHeaders -p:ScanArch=crossarch
-p:PartitionFilter=Security.Cryptography.Catalog%3BSecurity.Cryptography.Sip`
→ Build succeeded, 0 Error(s).
