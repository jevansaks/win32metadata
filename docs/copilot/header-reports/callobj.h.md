# `callobj.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-01`
- **Partitions:** Com.CallObj, ComOle
- **Delta:** Six COM interfaces and `CoGetInterceptor` lacked supported-OS metadata.
- **Resolution:** Added guarded Windows 5.0 annotations to the owning declarations.
- **Artifact:** `generation/WinSDK/patches/post-midl/callobj.h.zzz-supported-os.patch`
- **Evidence:** Reverse patch application succeeds. The retained patch was included in
  the previously completed sequential x64, arm64, and x86 generation.
- **Normalization:** Documentation and namespace partitioning are excluded from logical
  equivalence; declaration identity and supported-OS behavior are preserved.
