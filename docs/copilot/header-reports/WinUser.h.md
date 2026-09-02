# `WinUser.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-38`
- **Partitions:** Base, Controls, DataXchg, DlgBox, Dwm, FileHistory, Gdi, Input.Ime, Intl,
  IO, MenuRc, Security, Security.AppLocker, Security.ConfigurationSnapin,
  Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection,
  Security.Tpm, Security.WinTrust, Security.WinWlx, Shell, TermServ, WinAuto, WinProg,
  WinTouch
- **Delta:** Three retained artifacts on the very large core Window/User API surface
  declared in this header: `set-last-error` (`_Win32_metadata_set_last_error_`),
  `zz-metadata-enums` (ABI-compatible semantic enum typing for window-message/style
  constants — the largest single patch reviewed this session at 226 KB), and
  `zzz-supported-os` (`_Win32_metadata_supported_os_(...)`, 133 KB). None contains
  ownership/typedef metadata.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/WinUser.h.set-last-error.patch`,
  `generation/WinSDK/patches/post-midl/WinUser.h.zz-metadata-enums.patch`,
  `generation/WinSDK/patches/post-midl/WinUser.h.zzz-supported-os.patch`
- **Evidence:** Isolated `git apply --check --reverse` succeeds only for `zzz-supported-os`
  (the last-applied patch in filename-sort order); `set-last-error` and
  `zz-metadata-enums` fail in isolation because their hunk contexts overlap lines also
  touched by later-applied patches — the same expected collision pattern documented
  throughout this session for multi-patch headers. Verified via full sequential forward
  replay: reverse-applied all three patches in unwind order (`zzz-supported-os` →
  `zz-metadata-enums` → `set-last-error`), then forward-applied in filename-sort order
  (`set-last-error` → `zz-metadata-enums` → `zzz-supported-os`); reproduces the committed
  header exactly (zero diff).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI;
  ABI-neutral declaration annotation.
