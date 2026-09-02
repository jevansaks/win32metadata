# `securitybaseapi.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-01`
- **Partitions:** Etw, Identity, Security
- **Delta:** Audited under the corrected shared-handle ownership policy
  (commits `165b5f09`, `7335ddc4`): cleanup ownership and invalid-value metadata must live
  on producer returns/output parameters, not on typedefs.
- **Resolution:** No code change required. `_Win32_metadata_raii_free_(CloseHandle)` and
  `_Win32_metadata_invalid_handle_(-1|0)` are attached to the `_Outptr_ PHANDLE` output
  parameters of `CreateRestrictedToken` (`NewTokenHandle`), `DuplicateToken`
  (`DuplicateTokenHandle`), and `DuplicateTokenEx` (`phNewToken`). No typedef in the file
  carries ownership metadata.
- **Artifact:** `generation/WinSDK/patches/post-midl/securitybaseapi.h.zzz-resource-ownership.patch`
  (retained as-is).
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Static audit
  confirms all three ownership annotation groups sit on `_Outptr_ PHANDLE` parameters, never
  on a `typedef`/`DECLARE_HANDLE` site.
- **Normalization:** Ownership annotations are producer-site metadata, consistent with the
  corrected policy; classification unchanged from a pre-audit "matched" state.
