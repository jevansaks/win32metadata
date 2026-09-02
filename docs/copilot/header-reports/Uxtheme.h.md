# `Uxtheme.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-34`
- **Partitions:** Controls, WinTouch
- **Delta:** Two retained artifacts: `metadata-enums` (ABI-compatible semantic enum typing
  for visual-style/theme constants) and `zzz-supported-os`
  (`_Win32_metadata_supported_os_(...)`) on the Visual Styles / theming API surface declared
  in this header. Neither contains ownership/typedef metadata.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/Uxtheme.h.metadata-enums.patch`,
  `generation/WinSDK/patches/post-midl/Uxtheme.h.zzz-supported-os.patch`
- **Evidence:** Isolated `git apply --check --reverse` succeeds for `zzz-supported-os` (the
  last-applied patch in filename-sort order) but not for `metadata-enums` in isolation,
  because its hunk context overlaps lines also touched by `zzz-supported-os` — the same
  expected collision pattern documented for `bcrypt.h`/`CommCtrl.h`/`UserEnv.h`. Verified
  instead via full sequential forward replay: reverse-applied both patches in unwind order,
  then forward-applied both in filename-sort order; reproduces the committed header exactly
  (zero diff).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI;
  ABI-neutral declaration annotation.
