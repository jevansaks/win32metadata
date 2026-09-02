# `AuthZ.h`

- **Status:** accepted-normalized
- **Batch:** `resource-ownership-audit-03`
- **Partitions:** Authorization, Authorization.UI
- **Delta:** The retained `zzz-resource-ownership` patch annotated all seven AuthZ handle
  types directly on their `DECLARE_HANDLE` typedef sites with
  `_Win32_metadata_invalid_handle_(-1)`/`(0)` and (for five of them)
  `_Win32_metadata_raii_free_`, violating the corrected shared-handle policy (commits
  `165b5f09`, `7335ddc4`).
- **Resolution:** Removed all ownership annotations from the seven typedefs
  (`AUTHZ_ACCESS_CHECK_RESULTS_HANDLE`, `AUTHZ_CLIENT_CONTEXT_HANDLE`,
  `AUTHZ_RESOURCE_MANAGER_HANDLE`, `AUTHZ_AUDIT_EVENT_HANDLE`,
  `AUTHZ_AUDIT_EVENT_TYPE_HANDLE`, `AUTHZ_SECURITY_EVENT_PROVIDER_HANDLE`,
  `AUTHZ_CAP_CHANGE_SUBSCRIPTION_HANDLE`). Added invalid-handle/RAIIFree annotations to
  every producer `_Out_`/`_Out_opt_` output parameter:
  - `AuthzAccessCheck.phAccessCheckResults` → `AuthzFreeHandle`
  - `AuthzInitializeResourceManager`/`AuthzInitializeResourceManagerEx`/
    `AuthzInitializeRemoteResourceManager`.`phAuthzResourceManager` → `AuthzFreeResourceManager`
  - `AuthzInitializeContextFromToken`/`AuthzInitializeContextFromSid`/
    `AuthzInitializeContextFromAuthzContext`/`AuthzInitializeCompoundContext`/
    `AuthzAddSidsToContext`.`phAuthzClientContext`/`phNewAuthzClientContext`/
    `phCompoundContext` → `AuthzFreeContext`
  - `AuthzInitializeObjectAccessAuditEvent`/`AuthzInitializeObjectAccessAuditEvent2`.
    `phAuditEvent` → `AuthzFreeAuditEvent`
  - `AuthzRegisterSecurityEventSource.phEventProvider` → `AuthzUnregisterSecurityEventSource`
  - `AuthzRegisterCapChangeNotification.phCapChangeSubscription` →
    `AuthzUnregisterCapChangeNotification`
  - `AUTHZ_AUDIT_EVENT_TYPE_HANDLE` has no producer function in this header (all uses are
    `_In_` consumers); its invalid-handle annotation was dropped rather than misattached.
- **Artifact:** `generation/WinSDK/patches/post-midl/AuthZ.h.zzz-resource-ownership.patch`
  (regenerated).
- **Evidence:** Regenerated the patch via pristine-baseline reconstruction (reverse-apply of
  the prior patch, then forward diff of the corrected header). `git apply --check --reverse`
  passes for the regenerated patch. Static audit confirms zero remaining ownership
  annotations adjacent to a `typedef`/`DECLARE_HANDLE` site.
- **Assumption:** Added `_Win32_metadata_raii_free_(AuthzUnregisterSecurityEventSource)`
  (absent from the original, non-compliant patch) because that function is the confirmed
  in-header counterpart accepting `PAUTHZ_SECURITY_EVENT_PROVIDER_HANDLE` for teardown.
  `AUTHZ_AUDIT_EVENT_TYPE_HANDLE` is left without invalid-handle metadata because no
  producer exists in this header to attach it to; flagged here rather than silently dropped.
