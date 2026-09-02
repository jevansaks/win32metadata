# `ncryptprotect.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-02`
- **Partitions:** Security.Cryptography, Security.Cryptography.UI
- **Delta:** The retained `zzz-resource-ownership` patch annotated `NCRYPT_DESCRIPTOR_HANDLE`
  and `NCRYPT_STREAM_HANDLE` directly on their `DECLARE_HANDLE` typedef sites with
  `_Win32_metadata_invalid_handle_(-1)`/`(0)`, violating the corrected shared-handle policy
  (commits `165b5f09`, `7335ddc4`).
- **Resolution:** Removed the invalid-handle annotations from both typedefs. Added
  invalid-handle plus `_Win32_metadata_raii_free_` annotations to every producer output
  parameter: `NCryptCreateProtectionDescriptor.phDescriptor` and
  `NCryptUnprotectSecret.phDescriptor` (both closed via `NCryptCloseProtectionDescriptor`,
  confirmed present in this header); `NCryptStreamOpenToProtect.phStream`,
  `NCryptStreamOpenToUnprotect.phStream`, and `NCryptStreamOpenToUnprotectEx.phStream` (all
  closed via `NCryptStreamClose`, confirmed present in this header).
- **Artifact:** `generation/WinSDK/patches/post-midl/ncryptprotect.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef`/`DECLARE_HANDLE` site.
- **Assumption:** Added `_Win32_metadata_raii_free_` (absent from the original, non-compliant
  patch) because both close functions (`NCryptCloseProtectionDescriptor`,
  `NCryptStreamClose`) are declared in this same header, making the ownership relationship
  unambiguous.
