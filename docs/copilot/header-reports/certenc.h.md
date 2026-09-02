# `certenc.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-01`
- **Partitions:** Certificates
- **Delta:** COM method parameters used raw `LONG` where the baseline carries semantic
  enum typing.
- **Resolution:** Replaced use-site spelling with ABI-compatible
  `CERT_RDN_ATTR_VALUE_TYPE` and `CERT_ALT_NAME` typedefs.
- **Artifact:** `generation/WinSDK/patches/post-midl/CertEnc.h.zz-crypto-security-enums.patch`
- **Evidence:** Reverse patch application succeeds. The retained patch was included in
  the previously completed sequential x64, arm64, and x86 generation.
- **Normalization:** Typedef spelling changes are accepted when size, signedness, and COM
  ABI remain unchanged.
