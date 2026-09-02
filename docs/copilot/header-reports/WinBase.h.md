# `WinBase.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-35`
- **Partitions:** Base, DataXchg, FileHistory, Fs, Identity, Input.Ime, Intl, IO, MenuRc,
  Registry, Security, Security.AppLocker, Security.ConfigurationSnapin,
  Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust,
  Security.WinWlx, Setup, Shutdown, TermServ, WinProg
- **Delta:** Three retained artifacts: `callback-canonical-name` (canonical naming for a
  callback function-pointer typedef), `service-security-set-last-error`, and
  `zzz-set-last-error` (both `_Win32_metadata_set_last_error_` on disjoint parts of the
  large core Win32 base API surface declared in this header). None contains
  ownership/typedef metadata, and all three reverse-check independently (no hunk-context
  overlap between them).
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/WinBase.h.callback-canonical-name.patch`,
  `generation/WinSDK/patches/post-midl/WinBase.h.service-security-set-last-error.patch`,
  `generation/WinSDK/patches/post-midl/WinBase.h.zzz-set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for all three retained patches
  individually. Prior sequential x64, arm64, and x86 generation validated the annotation
  form (per `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef spelling preserving ABI/signature; ABI-neutral
  declaration annotation.
