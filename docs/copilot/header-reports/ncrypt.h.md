# `ncrypt.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-04`
- **Partitions:** Security.Cryptography, Security.Cryptography.UI
- **Delta:** The retained `zzz-resource-ownership` patch annotated `NCRYPT_HANDLE` (base
  type) and its subtypes `NCRYPT_PROV_HANDLE`, `NCRYPT_KEY_HANDLE`, `NCRYPT_HASH_HANDLE`,
  `NCRYPT_SECRET_HANDLE` directly on their `typedef` sites, violating the corrected
  shared-handle policy (commits `165b5f09`, `7335ddc4`). Unlike `bcrypt.h`, all subtypes
  here share a single generic free function, `NCryptFreeObject`.
- **Resolution:** Removed the invalid-handle/RAIIFree annotations from all five typedefs
  (kept `AlsoUsableFor(NCRYPT_HANDLE)` on `NCRYPT_PROV_HANDLE`/`NCRYPT_KEY_HANDLE`, which is
  not ownership metadata). Added invalid-handle plus `_Win32_metadata_raii_free_(NCryptFreeObject)`
  annotations to every producer `_Out_`/`_Out_opt_` parameter:
  `NCryptOpenStorageProvider.phProvider`, `NCryptOpenKey.phKey`,
  `NCryptCreatePersistedKey.phKey`, `NCryptImportKey.phKey`,
  `NCryptTranslateHandle.phProvider`/`.phKey`, `NCryptSecretAgreement.phAgreedSecret` (7
  sites total). `NCRYPT_HASH_HANDLE` has no producer or consumer anywhere in this header
  (declared but unused); its invalid-handle annotation was dropped rather than misattached.
- **Artifact:** `generation/WinSDK/patches/post-midl/ncrypt.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef` site. Verified full sequential forward replay
  (pristine → `zz-crypto-security-enums.patch` → this regenerated
  `zzz-resource-ownership.patch`) reproduces the exact corrected file with zero diff,
  confirming compatibility with the unrelated co-resident enum-typing patch.
- **Assumption:** `NCRYPT_HASH_HANDLE` is left without invalid-handle metadata because
  neither a producer nor any consumer of it exists in this header to attach it to.
