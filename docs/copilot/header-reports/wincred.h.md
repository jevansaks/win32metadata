# `wincred.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-35`
- **Partitions:** Certificates, Credentials, Identity, Security, Security.AppLocker,
  Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog,
  Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust,
  Security.WinWlx
- **Delta:** Two retained artifacts: `credential-enums` (ABI-compatible semantic enum
  typing for Credential Manager constants) and `service-security-set-last-error`
  (`_Win32_metadata_set_last_error_`). Neither contains ownership/typedef metadata.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/wincred.h.credential-enums.patch`,
  `generation/WinSDK/patches/post-midl/wincred.h.service-security-set-last-error.patch`
- **Evidence:** Isolated `git apply --check --reverse` succeeds for
  `service-security-set-last-error` (the last-applied patch in filename-sort order) but not
  for `credential-enums` in isolation, because its hunk context overlaps lines also touched
  by the other patch — the same expected collision pattern documented for
  `bcrypt.h`/`CommCtrl.h`/`UserEnv.h`/`Uxtheme.h`. Verified instead via full sequential
  forward replay: reverse-applied both patches in unwind order, then forward-applied both
  in filename-sort order; reproduces the committed header exactly (zero diff).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI;
  ABI-neutral declaration annotation.
