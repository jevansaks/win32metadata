# `bcrypt.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-03`
- **Partitions:** Security.Cryptography, Security.Cryptography.UI
- **Delta:** The retained `zzz-resource-ownership` patch annotated `BCRYPT_HANDLE` (base
  type) and its four `AlsoUsableFor` subtypes (`BCRYPT_ALG_HANDLE`, `BCRYPT_KEY_HANDLE`,
  `BCRYPT_HASH_HANDLE`, `BCRYPT_SECRET_HANDLE`) directly on their `typedef` sites, violating
  the corrected shared-handle policy (commits `165b5f09`, `7335ddc4`). The unrelated
  `BCryptRegisterConfigChangeNotify.phEvent` annotation in the same patch was already
  correctly placed on a producer output parameter.
- **Resolution:** Removed the invalid-handle/RAIIFree annotations from all five typedefs
  (kept the `AlsoUsableFor(BCRYPT_HANDLE)` relationships, which are not ownership metadata).
  Added invalid-handle plus RAIIFree annotations to every producer `_Out_` parameter:
  - `BCryptOpenAlgorithmProvider.phAlgorithm` → `BCryptCloseAlgorithmProvider`
    (`BCRYPT_ALG_HANDLE`)
  - `BCryptGenerateSymmetricKey`/`BCryptGenerateKeyPair`/`BCryptImportKey`/
    `BCryptImportKeyPair`.`phKey`, `BCryptDuplicateKey.phNewKey` → `BCryptDestroyKey`
    (`BCRYPT_KEY_HANDLE`, 5 sites)
  - `BCryptSecretAgreement.phAgreedSecret` → `BCryptDestroySecret` (`BCRYPT_SECRET_HANDLE`)
  - `BCryptCreateHash`/`BCryptCreateMultiHash`.`phHash`, `BCryptDuplicateHash.phNewHash` →
    `BCryptDestroyHash` (`BCRYPT_HASH_HANDLE`, 3 sites)
  - `BCRYPT_HANDLE` (the base/union type) has no producer of its own in this header (it only
    appears as a generic parameter type, e.g. `BCryptProcessMultiOperations`); its
    invalid-handle annotation was dropped rather than misattached.
- **Artifact:** `generation/WinSDK/patches/post-midl/bcrypt.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef` site (all 10 groups are on `_Out_` parameters, plus the
  pre-existing compliant `BCryptRegisterConfigChangeNotify` group).
- **Note:** This header also carries an independent `zz-crypto-security-enums` patch whose
  `BCryptOpenAlgorithmProvider` hunk shares a 3-line context window with this change (the
  `phAlgorithm`/`pszAlgId`/`pszImplementation` parameter block, immediately preceding its
  `dwFlags` type-enum edit). Isolated `git apply --check --reverse` of the enums patch alone
  against the fully-patched file therefore no longer succeeds (its context block is now
  interrupted by the inserted annotation lines) — this is expected given overlapping context,
  not a regression. Full sequential forward replay was verified instead: pristine SDK header
  → `zz-crypto-security-enums.patch` → this regenerated `zzz-resource-ownership.patch`
  reproduces the exact corrected file with zero diff.
