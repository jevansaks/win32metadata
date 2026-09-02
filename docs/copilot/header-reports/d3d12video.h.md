# `d3d12video.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-08`
- **Partitions:** Mf
- **Delta:** Retained artifact adds `_Win32_metadata_supported_os_(...)` annotations to the
  Direct3D 12 video API surface declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/d3d12video.h.zz-supported-os.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
