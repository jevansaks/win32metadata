# Header Report: Wscapi.h

## Partitions
`FileHistory`, `SecurityCenter`

## Ownership audit (producer-site-only policy)
- `WscRegisterForChanges(LPVOID Reserved, PHANDLE phCallbackRegistration, LPTHREAD_START_ROUTINE lpCallbackAddress, PVOID pContext)` produces a generic `HANDLE` via a direct out-param, closed via `WscUnRegisterChanges(HANDLE hRegistrationHandle)` — the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`/`ondemandconnroutehelper.h`/`davclnt.h`/`RTWorkQ.h`/`FaxDev.h`/`wdstpdi.h`/`prnasnot.h`).
- `WscGetAntiMalwareUri`'s `_Outptr_ LPWSTR *ppszUri` is a generic string buffer, out of scope.
- `WscGetSecurityProviderHealth` outputs a plain enum value, no handle.

## Conclusion
`blocked` — genuine gap in `WscRegisterForChanges`/`WscUnRegisterChanges` (generic `HANDLE` direct-out-param, reuses established blocker class).
