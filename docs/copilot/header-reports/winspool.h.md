# `winspool.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-06`
- **Partitions:** Gdi, Printing
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_`, a
  `_Win32_metadata_const_` marker, and ownership metadata for `HANDLE` printer/notification
  handles produced by `OpenPrinterA`/`OpenPrinterW`/`OpenPrinter2A`/`OpenPrinter2W`
  (`phPrinter` output parameter → `ClosePrinter`) and
  `FindFirstPrinterChangeNotification` (return-value position →
  `FindClosePrinterChangeNotification`). This is a resource-ownership patch not covered by
  the prior 11-header audit; verified here per the same standing instruction to confirm no
  typedef carries ownership metadata.
- **Resolution:** No further changes needed. All ownership annotations already target
  producer `_Out_` parameters or return-value positions; no typedef in the file is
  annotated.
- **Artifact:** `generation/WinSDK/patches/post-midl/winspool.h.printing-handle-ownership.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Static audit
  (grep with context) confirms all 4 ownership annotation groups sit on `_Out_ LPHANDLE`
  parameters or a function return-value position, never on a `typedef`/`DECLARE_HANDLE`
  site.
- **Normalization:** Ownership annotations are producer-site metadata, consistent with the
  corrected policy (165b5f09, 7335ddc4); no code changes required.
