# `wincrypt.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-05`
- **Partitions:** NetMgmt, Rpc, Security.Cryptography, Security.Cryptography.UI
- **Delta:** The retained `zzz-resource-ownership` patch annotated six handle typedefs
  directly on their `typedef` sites, violating the corrected shared-handle policy (commits
  `165b5f09`, `7335ddc4`): `HCRYPTPROV_OR_NCRYPT_KEY_HANDLE`, `HCRYPTPROV_LEGACY`,
  `HCERTSTORE`, `HCERTSTOREPROV`, `HCRYPTASYNC`/`PHCRYPTASYNC`, `HCERTCHAINENGINE`.
- **Resolution:** Removed the ownership annotations from all six typedefs. Added
  invalid-handle (plus RAIIFree where a clear free function exists) to every producer site:
  - `CryptAcquireCertificatePrivateKey.phCryptProvOrNCryptKey` → invalid-handle only
    (`HCRYPTPROV_OR_NCRYPT_KEY_HANDLE`; no single free function — freed via either
    `CryptReleaseContext` or `NCryptFreeObject` depending on `pfCallerFreeProvOrNCryptKey`
    and handle kind, so RAIIFree is intentionally omitted, matching the original patch).
  - `HCERTSTORE` (9 producer sites: return values of `CertOpenStore`, `CertDuplicateStore`,
    `CryptGetMessageCertificates`, `CertOpenSystemStoreA`, `CertOpenSystemStoreW`,
    `PFXImportCertStore`; output parameters of `CryptQueryObject.phCertStore`,
    `CryptRetrieveTimeStamp.phStore`, `CryptVerifyTimeStampSignature.phStore`) →
    invalid-handle only, no RAIIFree added. `HCERTSTORE` is reference-counted
    (`CertDuplicateStore` increments, `CertCloseStore` decrements/may not actually free),
    so it is not a simple 1:1 RAII relationship; the original patch never specified a
    RAIIFree for it and this fix preserves that decision rather than inventing one.
  - `CryptCreateAsyncHandle.phAsync` → `CryptCloseAsyncHandle` (`HCRYPTASYNC`)
  - `CertCreateCertificateChainEngine.phChainEngine` → `CertFreeCertificateChainEngine`
    (`HCERTCHAINENGINE`)
  - `HCRYPTPROV_LEGACY` has no producer anywhere in this header — every occurrence is an
    `_In_` parameter or struct field (the SDK comment states callers should always pass
    `NULL`); its invalid-handle annotation was dropped rather than misattached.
  - `HCERTSTOREPROV` has no producer either — every occurrence is `_Inout_`/`_In_` inside
    store-provider callback signatures (the provider sets it internally; the SDK does not
    expose a function that returns/out-params a new one); its invalid-handle annotation was
    dropped rather than misattached.
- **Artifact:** `generation/WinSDK/patches/post-midl/wincrypt.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef` site (12 groups total, all on producer parameters or
  return values). Verified full sequential forward replay (pristine → the much larger
  co-resident `zz-crypto-security-enums.patch` (104 KB) → this regenerated
  `zzz-resource-ownership.patch`) reproduces the exact corrected file with zero diff,
  confirming no context-window collisions with that patch despite its size.
- **Assumption:** `HCERTSTORE` deliberately keeps no RAIIFree annotation (reference-counted
  resource); `HCRYPTPROV_LEGACY` and `HCERTSTOREPROV` are left without invalid-handle
  metadata because neither has an in-header producer to attach it to.
