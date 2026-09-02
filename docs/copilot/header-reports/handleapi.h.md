# `handleapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-14`
- **Partitions:** FileHistory, Foundation
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the generic HANDLE
  lifetime functions (`CloseHandle`, `DuplicateHandle`, `CompareObjectHandles`, etc.)
  declared in this header. No ownership/typedef annotations are present — `HANDLE` itself is
  not annotated here (ownership is tracked per producing API elsewhere), consistent with the
  corrected shared-handle policy.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/handleapi.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
