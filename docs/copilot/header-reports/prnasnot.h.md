# Header Report: prnasnot.h

## Partitions
`Gdi`, `Printing`

## Ownership audit (producer-site-only policy)
- `RegisterForPrintAsyncNotifications(..., _Out_ HANDLE* phNotify)` produces a generic `HANDLE` via a direct out-param, closed via `UnRegisterForPrintAsyncNotifications(_In_ HANDLE)` — the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`/`ondemandconnroutehelper.h`/`davclnt.h`/`RTWorkQ.h`/`FaxDev.h`/`wdstpdi.h`).
- `CreatePrintAsyncNotifyChannel`'s `_Out_ IPrintAsyncNotifyChannel** ppIAsynchNotification` is a standard COM interface pointer, out of scope.
- `IPrintAsyncNotifyDataObject`/`IPrintAsyncNotifyChannel`/`IPrintAsyncNotifyCallback` are `DECLARE_INTERFACE_`-style COM interfaces, out of scope.

## Conclusion
`blocked` — genuine gap in `RegisterForPrintAsyncNotifications`/`UnRegisterForPrintAsyncNotifications` (generic `HANDLE` direct-out-param, reuses established blocker class).
