# `SetupAPI.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-28`
- **Partitions:** DevInst, Setup
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the Setup/Device
  Installation API surface declared in this header. This patch does not annotate
  `HDEVINFO`/`HSPFILEQ`/etc. ownership (`SetupDiGetClassDevs` →
  `SetupDiDestroyDeviceInfoList`, etc.) — that is a separate, still-`pending` concern (no
  ownership annotations exist yet for this header, so there is nothing to audit against the
  producer-site-only policy here; adding new ownership annotations is out of scope for this
  routine classification pass and would need its own dedicated investigation given the
  header's size, 48 KB patch, and the number of distinct handle types it declares).
- **Resolution:** No further changes needed for the retained `set-last-error` patch;
  retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/SetupAPI.h.zzz-set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`). Static audit confirms zero
  `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_` occurrences in this patch,
  so there is no typedef-ownership violation to correct.
- **Normalization:** ABI-neutral declaration annotation.
