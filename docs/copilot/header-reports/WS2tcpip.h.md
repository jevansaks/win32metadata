# `WS2tcpip.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-39`
- **Partitions:** WinSock
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the Winsock
  TCP/IP-specific extension API surface (`getaddrinfo`, `GetAddrInfoEx`, etc.) declared in
  this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/WS2tcpip.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.

---

**Session milestone note:** This header completes the classification of every pending
header in the authoritative `header-progress.json` ledger that already had a retained
post-MIDL patch artifact (the last of an initial ~200-header cohort processed across
39 `existing-patches-*`/`resource-ownership-audit-*` batches this session). Remaining
pending headers have no retained patch and require header-scraping investigation
(established in the `scraping-investigation-*` batches) to determine whether any patch is
needed at all.
