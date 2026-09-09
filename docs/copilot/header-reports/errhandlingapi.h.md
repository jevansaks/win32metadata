# `errhandlingapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-13`
- **Partitions:** Base, Debug
- **Delta:** Canonical naming for the vectored-exception-handler callback typedef and
  `_Win32_metadata_set_last_error_` on the Win32 error-handling API surface.
- **Resolution:** Consolidated both logical changes into one cumulative header patch.
- **Artifact:** `generation/WinSDK/patches/post-midl/errhandlingapi.h.metadata.patch`
- **Evidence:** Full post-MIDL replay from clean `d154186c` applies successfully and the
  resulting header matches the tracked output byte-for-byte.
  Prior sequential x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation and canonical callback naming.
