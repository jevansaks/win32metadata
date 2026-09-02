# `winsafer.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-02`
- **Partitions:** Security.AppLocker
- **Delta:** The retained `zzz-resource-ownership` patch annotated `SAFER_LEVEL_HANDLE`
  directly on its `DECLARE_HANDLE` typedef site with `_Win32_metadata_invalid_handle_(-1)`/
  `(0)`, violating the corrected shared-handle policy (commits `165b5f09`, `7335ddc4`). The
  unrelated `SaferComputeTokenFromLevel.OutAccessToken` (`HANDLE`/`CloseHandle`) annotation
  in the same patch was already correctly placed on a producer output parameter.
- **Resolution:** Removed the invalid-handle annotation from the `SAFER_LEVEL_HANDLE`
  typedef. Added invalid-handle plus `_Win32_metadata_raii_free_(SaferCloseLevel)`
  annotations to both producer output parameters: `SaferCreateLevel.pLevelHandle` and
  `SaferIdentifyLevel.pLevelHandle`.
- **Artifact:** `generation/WinSDK/patches/post-midl/winsafer.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef`/`DECLARE_HANDLE` site.
- **Assumption:** Added `_Win32_metadata_raii_free_(SaferCloseLevel)` (absent from the
  original, non-compliant patch) because `SaferCloseLevel` is declared in this same header
  as the only function accepting `SAFER_LEVEL_HANDLE` for teardown, making the ownership
  relationship unambiguous.
