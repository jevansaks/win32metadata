# `icucommon.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-15`
- **Partitions:** Intl
- **Delta:** Retained artifact adjusts pointer indirection levels on ICU callback function
  parameters declared in this header to match the correct native signature.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/icucommon.h.callback-pointer-levels.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef spelling preserving ABI/signature.
