# `TlHelp32.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-32`
- **Partitions:** ToolHelp
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the ToolHelp
  snapshot enumeration API surface (`CreateToolhelp32Snapshot`, `Process32First/Next`,
  `Thread32First/Next`, etc.) declared in this header. `CreateToolhelp32Snapshot` returns a
  generic `HANDLE` closed via `CloseHandle`; this patch does not add ownership annotations
  for it, and none exist elsewhere in the file to audit against the producer-site-only
  policy — a gap noted here, not fixed, since generic-`HANDLE`/`CloseHandle` ownership
  conventions are typically centralized (see `handleapi.h`) rather than re-declared
  per-header, and confirming the correct centralization point is out of scope for this
  routine classification pass.
- **Resolution:** No further changes needed for the retained `set-last-error` patch;
  retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/TlHelp32.h.zzz-set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`). Static audit confirms zero
  `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` occurrences in this patch.
- **Normalization:** ABI-neutral declaration annotation.
