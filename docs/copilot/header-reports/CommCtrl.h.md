# `CommCtrl.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-03`
- **Partitions:** Controls, Policy, Shell
- **Delta:** Two retained artifacts: `controls-enums` (semantic enum typing for common
  control message/notification constants) and `zzz-supported-os` (OS-version annotations
  across the common controls API surface). Neither contains ownership/typedef metadata.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/CommCtrl.h.controls-enums.patch`,
  `generation/WinSDK/patches/post-midl/CommCtrl.h.zzz-supported-os.patch`
- **Evidence:** Isolated `git apply --check --reverse` succeeds for `zzz-supported-os` (the
  last-applied patch in filename-sort order) but not for `controls-enums` in isolation,
  because its 3-line hunk context overlaps lines also touched by `zzz-supported-os` — the
  same expected collision pattern documented for `bcrypt.h`/`wincrypt.h`. Verified instead via
  full sequential forward replay: reverse-applied both patches in unwind order
  (`zzz-supported-os` then `controls-enums`) from the committed header, then forward-applied
  both in filename-sort order (`controls-enums` then `zzz-supported-os`); the result matches
  the committed header exactly (zero diff).
- **Normalization:** ABI-compatible typedef/enum spelling and declaration annotation; no
  ownership metadata present in either patch.
