# `newdev.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-12`
- **Partitions:** DevInst
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the driver-install
  API surface declared in this header.
- **History:** The `shared-handle-policy-01` batch (2026-09-02T19:35:00Z) flagged this header
  as a "pre-existing replay mismatch" during a full clean-checkout replay of all 254 retained
  patches, deferring reconciliation to its own queue entry. The patch file's timestamp
  (2026-08-14) postdates that batch's original patch set (2026-08-12), indicating the mismatch
  was already addressed in the interim.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/newdev.h.zzz-set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes. Isolated round-trip verified in this
  session: reverse-applied the patch against the committed header, then forward-reapplied it;
  the result matches the committed header exactly (zero diff). A full pristine-SDK checkout
  replay (as originally used to flag the mismatch) was not re-run in this session — no fresh
  SDK checkout was available — so if the mismatch recurs during the next full `DoAll.ps1`
  build, it should be re-escalated with fresh evidence.
- **Normalization:** ABI-neutral declaration annotation; no ownership/typedef metadata
  present.
