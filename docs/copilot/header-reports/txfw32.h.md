# `txfw32.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-33`
- **Partitions:** Fs
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the Transactional
  NTFS Win32 API surface declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/txfw32.h.zzz-set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
