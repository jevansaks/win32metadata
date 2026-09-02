# `processthreadsapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-24`
- **Partitions:** FileHistory, TermServ
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the process/thread
  management API surface (`CreateProcess`, `CreateThread`, `OpenProcess`, etc.) declared in
  this header. No ownership/typedef annotations are present in this patch — process/thread
  `HANDLE` ownership is tracked separately (e.g. via `handleapi.h`/producer-site metadata),
  consistent with the corrected shared-handle policy.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/processthreadsapi.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
