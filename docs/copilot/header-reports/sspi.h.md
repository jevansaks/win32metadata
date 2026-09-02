# `sspi.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-01`
- **Partitions:** Certificates, Identity, Security, Security.AppLocker,
  Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog,
  Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust,
  Security.WinWlx
- **Delta:** Audited under the corrected shared-handle ownership policy
  (commits `165b5f09`, `7335ddc4`): cleanup ownership and invalid-value metadata must live
  on producer returns/output parameters, not on typedefs.
- **Resolution:** No code change required. `_Win32_metadata_raii_free_(CloseHandle)` and
  `_Win32_metadata_invalid_handle_(-1|0)` are attached to `SspiGetCredUIContext`'s
  `_Out_opt_ HANDLE* TokenHandle` output parameter. No typedef in the file carries
  ownership metadata. (This header also carries unrelated, separately tracked
  `zz-crypto-security-enums` and `zzzz-project-as` patches, which are out of scope for this
  audit.)
- **Artifact:** `generation/WinSDK/patches/post-midl/sspi.h.zzz-resource-ownership.patch`
  (retained as-is).
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Static audit
  confirms the ownership annotation group sits on the `_Out_opt_ HANDLE*` parameter, never
  on a `typedef`/`DECLARE_HANDLE` site.
- **Normalization:** Ownership annotations are producer-site metadata, consistent with the
  corrected policy; classification unchanged from a pre-audit "matched" state.
