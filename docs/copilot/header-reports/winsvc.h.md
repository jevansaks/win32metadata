# `winsvc.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-03`
- **Partitions:** Services
- **Delta:** The retained `zzz-resource-ownership` patch annotated `SC_HANDLE` and
  `SERVICE_STATUS_HANDLE` directly on their `DECLARE_HANDLE` typedef sites, violating the
  corrected shared-handle policy (commits `165b5f09`, `7335ddc4`). Both handle types are
  produced as direct function **return values** rather than `_Out_` parameters.
- **Resolution:** Removed the invalid-handle/RAIIFree annotations from both typedefs. Added
  equivalent annotations to every producer function's return-value position (trailing after
  the closing parameter-list paren, before the semicolon — the same placement convention
  already established in this repo for function-pointer typedef return values, e.g.
  `NTSecPKG.h`'s `LSA_CREATE_THREAD`):
  - `OpenSCManagerA`/`OpenSCManagerW`, `OpenServiceA`/`OpenServiceW`,
    `CreateServiceA`/`CreateServiceW` → `_Win32_metadata_raii_free_(CloseServiceHandle)`
    (6 sites, `SC_HANDLE`)
  - `RegisterServiceCtrlHandlerA`/`RegisterServiceCtrlHandlerW`/
    `RegisterServiceCtrlHandlerExA`/`RegisterServiceCtrlHandlerExW` → invalid-handle only, no
    RAIIFree (4 sites, `SERVICE_STATUS_HANDLE`; no close function exists — the handle becomes
    invalid when `ServiceMain` returns), matching the original patch's scope for this type.
- **Artifact:** `generation/WinSDK/patches/post-midl/winsvc.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef`/`DECLARE_HANDLE` site.
- **Assumption:** Return-value ownership annotations use the same trailing-attribute
  placement (after the closing `)`, before `;`) already established in this repo for
  function-pointer typedefs, since no non-typedef precedent existed prior to this change and
  the codebase's custom annotation macros expand to declaration-level
  `__attribute__((annotate(...)))`, which the scraper associates with the return value when
  not attached to a specific parameter.
