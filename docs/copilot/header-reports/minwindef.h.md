# `minwindef.h`

- **Status:** accepted-normalized
- **Batch:** `shared-handle-policy-01`
- **Partitions:** Foundation, Multimedia
- **Delta:** Resource typedefs incorrectly owned cleanup and invalid-value behavior.
- **Resolution:** Removed type-owned `RAIIFree`/invalid-handle annotations; retained
  `AlsoUsableFor` on `HINSTANCE`/`HMODULE` and `HRGN`/`HGDIOBJ`.
- **Artifact:** `generation/WinSDK/patches/post-midl/minwindef.h.handle-type-relations.patch`
- **Evidence:** Reverse patch application succeeds; static audit finds no type-owned
  cleanup or invalid-handle annotations. Prior sequential x64, arm64, and x86 generation
  validated the retained annotation form.
- **Normalization:** Ownership policy intentionally differs from pseudo-handle/type-owned
  baseline metadata while preserving ABI and native type identity.
