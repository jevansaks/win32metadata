# `drt.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-09`
- **Partitions:** P2p
- **Delta:** Retained artifact adds `_Win32_metadata_import_library_("drtprov.dll")`
  annotations to the Distributed Routing Table (DRT) bootstrap-resolver functions declared
  in this header, plus the `win32metadata_annotations.h` include guard.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/drt.h.import-library-overrides.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation (import-library override; no
  ownership/typedef metadata present).
