# `NTSecAPI.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-02`
- **Partitions:** ActiveDirectory, Certificates, Identity, Security, Security.AppLocker,
  Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog,
  Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust,
  Security.WinWlx
- **Delta:** The retained `zzz-resource-ownership` patch was mostly compliant with the
  corrected shared-handle policy (commits `165b5f09`, `7335ddc4`), except one violation:
  `typedef PVOID LSA_HANDLE, *PLSA_HANDLE;` carried `_Win32_metadata_invalid_handle_(-1)`,
  `_Win32_metadata_invalid_handle_(0)`, and `_Win32_metadata_raii_free_(LsaClose)` directly
  on the typedef.
- **Resolution:** Removed the ownership annotations from the `LSA_HANDLE` typedef. Added the
  same invalid-handle/RAIIFree annotations to every producer `_Out_ PLSA_HANDLE` output
  parameter that actually vends a new `LSA_HANDLE`: `LsaOpenPolicy.PolicyHandle`,
  `LsaOpenTrustedDomainByName.TrustedDomainHandle`, and
  `LsaCreateTrustedDomainEx.TrustedDomainHandle`. The other three annotation groups in this
  file (`LsaRegisterLogonProcess.LsaHandle`, `LsaLogonUser.Token`,
  `LsaConnectUntrusted.LsaHandle`) were already correctly placed on producer output
  parameters and are unchanged.
- **Artifact:** `generation/WinSDK/patches/post-midl/NTSecAPI.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch by reconstructing the pre-annotation baseline (reverse
  application of the prior patch against the committed header) and diffing forward against
  the corrected header. `git apply --check --reverse` passes for the regenerated patch.
  Static audit confirms zero remaining `_Win32_metadata_raii_free_`/
  `_Win32_metadata_invalid_handle_` occurrences adjacent to a `typedef`/`DECLARE_HANDLE`
  site; all occurrences are on `_Out_` parameters.
- **Assumption:** `LsaOpenPolicy`, `LsaOpenTrustedDomainByName`, and
  `LsaCreateTrustedDomainEx` are the only functions in this header that produce a new
  `LSA_HANDLE`/`PLSA_HANDLE` (via `_Out_`); all other `LSA_HANDLE` parameters in the file are
  `_In_` consumers of an already-open handle.
