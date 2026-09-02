# `UserEnv.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-34`
- **Partitions:** Policy, Shell
- **Delta:** Two retained artifacts: `zzz-set-last-error`
  (`_Win32_metadata_set_last_error_`) and `zzz-supported-os`
  (`_Win32_metadata_supported_os_(...)`) on the user-profile/policy API surface declared in
  this header. Neither contains ownership/typedef metadata.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/UserEnv.h.zzz-set-last-error.patch`,
  `generation/WinSDK/patches/post-midl/UserEnv.h.zzz-supported-os.patch`
- **Evidence:** Isolated `git apply --check --reverse` succeeds for `zzz-supported-os` (the
  last-applied patch in filename-sort order) but not for `zzz-set-last-error` in isolation,
  because its hunk context overlaps lines also touched by `zzz-supported-os` — the same
  expected collision pattern documented for `bcrypt.h`/`CommCtrl.h`. Verified instead via
  full sequential forward replay: reverse-applied both patches in unwind order, then
  forward-applied both in filename-sort order; reproduces the committed header exactly
  (zero diff).
- **Normalization:** ABI-neutral declaration annotation.
