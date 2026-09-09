# `prsht.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-25`
- **Partitions:** Controls
- **Delta:** Canonical naming for a property-sheet callback function-pointer typedef and
  `_Win32_metadata_supported_os_(...)` on the property-sheet API surface.
- **Resolution:** Consolidated both logical changes into one cumulative header patch.
- **Artifact:** `generation/WinSDK/patches/post-midl/prsht.h.metadata.patch`
- **Evidence:** Full post-MIDL replay from clean `d154186c` applies successfully and the
  resulting header matches the tracked output byte-for-byte.
  Prior sequential x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation and canonical callback naming.
