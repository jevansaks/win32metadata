# `libloaderapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-16`
- **Partitions:** Intl, LibraryLoader
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the module/resource
  loading API surface (`LoadLibrary`, `GetProcAddress`, `FreeLibrary`, etc.) declared in this
  header. No ownership/typedef annotations are present — `HMODULE` is not annotated here.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/libloaderapi.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
