# `winnetwk.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-37`
- **Partitions:** Wnet
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the WNet (network
  resource enumeration/connection) API surface declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/winnetwk.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
