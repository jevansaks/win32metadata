# `RtmV2.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-27`
- **Partitions:** RRas
- **Delta:** Retained artifact applies canonical naming to a Routing Table Manager (RTM)
  callback function-pointer typedef declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/RtmV2.h.callback-canonical-name.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef spelling preserving ABI/signature.
