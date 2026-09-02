# `richole.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-27`
- **Partitions:** Controls.RichEdit
- **Delta:** Two retained artifacts: `richedit-context-menu-enum` (ABI-compatible semantic
  enum typing for the Rich Edit context-menu constants) and `zzz-supported-os`
  (`_Win32_metadata_supported_os_(...)` annotations). Neither contains ownership/typedef
  metadata, and their hunks do not overlap.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/RichOle.h.richedit-context-menu-enum.patch`,
  `generation/WinSDK/patches/post-midl/RichOle.h.zzz-supported-os.patch`
- **Evidence:** `git apply --check --reverse` passes for both retained patches individually.
  Prior sequential x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI;
  ABI-neutral declaration annotation.
