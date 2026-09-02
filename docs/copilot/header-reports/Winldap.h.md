# `Winldap.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-37`
- **Partitions:** Ldap
- **Delta:** Two retained artifacts: `callback-pointer-levels` (pointer-indirection fix for
  an LDAP callback parameter) and `set-last-error` (`_Win32_metadata_set_last_error_` on
  the LDAP client API surface). Neither contains ownership/typedef metadata, and their
  hunks do not overlap.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/Winldap.h.callback-pointer-levels.patch`,
  `generation/WinSDK/patches/post-midl/Winldap.h.set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for both retained patches individually.
  Prior sequential x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef spelling preserving ABI/signature; ABI-neutral
  declaration annotation.
