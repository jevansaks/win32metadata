# `combaseapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-02`
- **Partitions:** Base, Com.Events, Com.StructuredStorage, Cos, IO, Multimedia, Shell,
  TransactionServer, VSS
- **Delta:** Retained artifact adds `_Win32_metadata_supported_os_(...)` annotations to the
  core COM API surface (CoInitialize/CoCreateInstance family, marshaling, class objects,
  task memory allocation, etc.), plus the `win32metadata_annotations.h` include guard.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/combaseapi.h.zzz-supported-os.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation; no ownership/typedef metadata
  present in this patch.
