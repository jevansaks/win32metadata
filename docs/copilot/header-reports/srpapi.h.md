# Header Report: srpapi.h

## Partitions
`Edp`

## Ownership audit (producer-site-only policy)
- `SrpCreateThreadNetworkContext(_In_ PCWSTR enterpriseId, _Out_ HTHREAD_NETWORK_CONTEXT* threadNetworkContext)` populates a transparent struct (`HTHREAD_NETWORK_CONTEXT { DWORD ThreadId; HANDLE ThreadContext; }`) whose `ThreadContext` field is a generic `HANDLE` — closed via `SrpCloseThreadNetworkContext(_Inout_ HTHREAD_NETWORK_CONTEXT* threadNetworkContext)`. This is the already-established **generic type nested in a struct/array field** blocker sub-class (`physicalmonitorenumerationapi.h`/`userenv.h`): the generic `HANDLE` field cannot be annotated without incorrectly applying ownership metadata to every `HANDLE` value anywhere in the metadata.
- `SrpSetTokenEnterpriseId`/`SrpGetEnterpriseIds`/`SrpGetEnterprisePolicy`/`SrpIsTokenService` take `HANDLE tokenHandle` as a pre-owned caller INPUT only — no ownership implications.
- `SrpDoesPolicyAllowAppExecution` outputs a plain `BOOL`; `SrpIsAllowed` takes an opaque `PSRP_REQUEST` as input only (never produced by this header).

## Conclusion
`blocked` — genuine gap in `SrpCreateThreadNetworkContext`/`SrpCloseThreadNetworkContext` (generic `HANDLE` nested in a struct field, reuses established blocker class).
