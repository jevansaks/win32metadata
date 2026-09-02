# `NTSecPKG.h`

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
- **Resolution:** No code change required. Every `_Win32_metadata_raii_free_` /
  `_Win32_metadata_invalid_handle_` annotation in the retained patch already targets a
  producer `_Out_`/`_Out_opt_` output parameter (`LSA_DUPLICATE_HANDLE.DestionationHandle`,
  `LSA_CREATE_THREAD.ThreadId`-adjacent handle return, `LsaFn*` `Token` parameters,
  `LSA_OPEN_TOKEN_BY_LOGON_ID.RetTokenHandle`, `SpGetContextTokenFn.ImpersonationToken`,
  `SpExportSecurityContextFn.pToken`, `KspGetTokenFn.ImpersonationToken`). No typedef in
  the file carries ownership metadata.
- **Artifact:** `generation/WinSDK/patches/post-midl/NTSecPKG.h.zzz-resource-ownership.patch`
  (retained as-is).
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Static audit
  (`_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` grep with context) shows
  every occurrence immediately follows an `_Out_`/`_Out_opt_` parameter declaration, never a
  `typedef`/`DECLARE_HANDLE` site.
- **Normalization:** Ownership annotations are producer-site metadata, consistent with the
  corrected policy; classification unchanged from a pre-audit "matched" state.
