# Header Report: ondemandconnroutehelper.h

## Partitions
`Nla`, `WindowsConnectionManager`

## Ownership audit (producer-site-only policy)
- `OnDemandRegisterNotification(_In_ ONDEMAND_NOTIFICATION_CALLBACK callback, _In_opt_ void* callbackContext, _Out_ HANDLE* registrationHandle)` produces a generic `HANDLE` via a direct out-param (closed via `OnDemandUnRegisterNotification(_In_ HANDLE registrationHandle)`) — the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`).
- `GetInterfaceContextTableForHostName`'s `_Outptr_ NET_INTERFACE_CONTEXT_TABLE**` is a fully-defined (transparent) struct with a dedicated free function (`FreeInterfaceContextTable`) — still out of scope per the established transparent-struct-alloc/free-pairs precedent (structural transparency, not free-function specificity, is the deciding factor; `NET_INTERFACE_CONTEXT_TABLE`'s fields are all directly visible/accessible, unlike an opaque/incomplete `DECLARE_HANDLE` type).

## Conclusion
`blocked` — genuine gap in `OnDemandRegisterNotification`/`OnDemandUnRegisterNotification` (generic `HANDLE` direct-out-param, reuses established blocker class).
