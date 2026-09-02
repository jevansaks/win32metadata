# `NetCon.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-20`
- **Partitions:** WindowsFirewall
- **Delta:** Retained artifact adds `_Win32_metadata_import_library_(...)` annotations to
  the network connection manager COM interface API surface declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/NetCon.h.import-library-overrides.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
