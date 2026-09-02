# `prsht.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-25`
- **Partitions:** Controls
- **Delta:** Two retained artifacts: `callback-canonical-name` (canonical naming for a
  property-sheet callback function-pointer typedef) and `zzz-supported-os`
  (`_Win32_metadata_supported_os_(...)` on the property-sheet API surface). Neither contains
  ownership/typedef ownership metadata, and their hunks do not overlap.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/prsht.h.callback-canonical-name.patch`,
  `generation/WinSDK/patches/post-midl/prsht.h.zzz-supported-os.patch`
- **Evidence:** `git apply --check --reverse` passes for both retained patches individually.
  Prior sequential x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation and canonical callback naming.
