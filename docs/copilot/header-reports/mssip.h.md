# `mssip.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-19`
- **Partitions:** Security.Cryptography.Sip, Security.WinTrust
- **Delta:** Retained artifact applies ABI-compatible semantic enum typing to the Subject
  Interface Package (SIP) constants declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/mssip.h.zz-crypto-security-enums.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI.
