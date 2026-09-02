# `propapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-24`
- **Partitions:** Com.StructuredStorage
- **Delta:** Retained artifact adds `_Win32_metadata_supported_os_(...)` annotations to the
  legacy structured-storage property-set API surface declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/propapi.h.zzz-supported-os.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
