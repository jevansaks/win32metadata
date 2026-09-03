# Header Report: profinfo.h

## Partitions
`Shell`

## Ownership audit (producer-site-only policy)
- Declares only the `PROFILEINFOA`/`PROFILEINFOW` data structs — **no functions**. Per the corrected
  policy, no ownership annotation belongs on this struct declaration itself.
- The `hProfile` `HANDLE` field within these structs is populated by `LoadUserProfileW` (declared in
  `UserEnv.h`) and released via `UnloadUserProfile`. While auditing this, discovered that `UserEnv.h`'s
  prior classification (`accepted-normalized` in batch `existing-patches-34`) only verified unrelated
  retained patches (`set-last-error`/`supported-os`), not ownership — **reclassified `UserEnv.h` to
  `blocked`** in this batch (see `docs/copilot/header-reports/userenv.h.reaudit.md`) as a genuine
  instance of the generic/shared-type-nested-in-struct blocker class (same as
  `physicalmonitorenumerationapi.h`).

## Conclusion
`accepted-normalized` — this header declares only the data struct (no functions); the real ownership
gap belongs to (and was addressed under) `UserEnv.h`, now reclassified `blocked` for this reason.
