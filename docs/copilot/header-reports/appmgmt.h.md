# `appmgmt.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-01`
- **Partitions:** Policy, Shell
- **Delta:** Supported-OS metadata for five application-management APIs.
- **Resolution:** Added guarded header annotations for Windows 6.0.6000.
- **Artifact:** `generation/WinSDK/patches/post-midl/appmgmt.h.zzz-supported-os.patch`
- **Evidence:** Reverse patch application succeeds. The retained patch was included in
  the previously completed sequential x64, arm64, and x86 generation.
- **Normalization:** Documentation and namespace partitioning are excluded from logical
  equivalence; API identity and supported-OS behavior are preserved.
