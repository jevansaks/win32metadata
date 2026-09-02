# `commoncontrols.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-03`
- **Partitions:** Controls
- **Delta:** Two retained artifacts: `imagelist-creation-flags` (semantic enum typing for
  `IImageList` creation flag constants) and `zzz-supported-os` (OS-version annotations).
  Neither contains ownership/typedef metadata.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/commoncontrols.h.imagelist-creation-flags.patch`,
  `generation/WinSDK/patches/post-midl/commoncontrols.h.zzz-supported-os.patch`
- **Evidence:** `git apply --check --reverse` passes for both retained patches individually
  (no context overlap between them). Prior sequential x64, arm64, and x86 generation
  validated the annotation form (per `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-compatible enum typing and declaration annotation.
