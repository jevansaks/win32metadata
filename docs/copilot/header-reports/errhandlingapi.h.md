# `errhandlingapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-13`
- **Partitions:** Base, Debug
- **Delta:** Two retained artifacts: `callback-canonical-name` (canonical naming for the
  vectored-exception-handler callback typedef) and `set-last-error`
  (`_Win32_metadata_set_last_error_` on the Win32 error-handling API surface). Neither
  contains ownership/typedef metadata, and their hunks do not overlap.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/errhandlingapi.h.callback-canonical-name.patch`,
  `generation/WinSDK/patches/post-midl/errhandlingapi.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for both retained patches individually.
  Prior sequential x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation and canonical callback naming.
