# `schannel.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-27`
- **Partitions:** Certificates, Identity, Security, Security.AppLocker,
  Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog,
  Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust,
  Security.WinWlx, Wsw
- **Delta:** Retained artifact applies ABI-compatible semantic enum typing to the Secure
  Channel (Schannel) SSP constants declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/schannel.h.zz-crypto-security-enums.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI.
