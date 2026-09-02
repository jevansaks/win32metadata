# `cfgmgr32.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-01`
- **Partitions:** DevInst
- **Delta:** Three configuration-manager APIs lacked set-last-error behavior.
- **Resolution:** Added guarded `_Win32_metadata_set_last_error_` annotations.
- **Artifact:** `generation/WinSDK/patches/post-midl/cfgmgr32.h.zzz-set-last-error.patch`
- **Evidence:** Reverse patch application succeeds. The retained patch was included in
  the previously completed sequential x64, arm64, and x86 generation.
- **Normalization:** ABI and native signatures are unchanged.
