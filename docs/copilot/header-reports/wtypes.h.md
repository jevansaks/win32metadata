# `wtypes.h`

- **Status:** accepted-normalized
- **Batch:** `shared-handle-policy-01`
- **Partitions:** Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon,
  ComOle, FileHistory, Properties, Registry, Shutdown, TransactionServer, WinProg
- **Delta:** `BSTR` carried typedef-owned `FreeWith(SysFreeString)` metadata.
- **Resolution:** Removed the annotation and obsolete vocabulary. `BSTR` remains the
  unmodified SDK typedef; ownership must be expressed at producer use sites.
- **Artifact:** The obsolete
  `generation/WinSDK/patches/post-midl/wtypes.h.resource-typedef-ownership.patch` was
  removed; no replacement patch is required because the accepted result is the SDK
  baseline declaration.
- **Evidence:** Static audit finds no type-owned cleanup or invalid-handle annotations.
  Prior sequential x64, arm64, and x86 generation validated the shared-header pipeline.
- **Normalization:** Type-owned allocation cleanup is an explicitly accepted policy
  normalization; native ABI and declaration identity are unchanged.
