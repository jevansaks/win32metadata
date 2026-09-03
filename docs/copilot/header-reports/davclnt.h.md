# Header Report: davclnt.h

## Partitions
`WebDav`

## Ownership audit (producer-site-only policy)
Two distinct genuine gaps, both already-established blocker classes:

1. `DavAddConnection(_Inout_ HANDLE *ConnectionHandle, ...)` produces a generic `HANDLE` via an out-param, closed via `DavDeleteConnection(_In_ HANDLE ConnectionHandle)` — the **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`/`ondemandconnroutehelper.h`).
2. `DavRegisterAuthCallback` returns `OPAQUE_HANDLE` (`#define OPAQUE_HANDLE DWORD` — a plain `DWORD`, not even a pointer-sized handle) directly as its C return value, closed via `DavUnregisterAuthCallback(_In_ OPAQUE_HANDLE hCallback)` — the **return-value handle ownership** blocker class (`getprocesshandlefromhwnd.h`).

Other functions (`DavGetUNCFromHTTPPath`, `DavGetHTTPFromUNCPath`, `DavGetTheLockOwnerOfTheFile`, `DavGetExtendedError`, `DavFlushFile`) take pre-owned `HANDLE`/write into caller buffers only, no additional gaps.

## Conclusion
`blocked` — genuine gaps in `DavAddConnection`/`DavDeleteConnection` (generic `HANDLE` direct-out-param) and `DavRegisterAuthCallback`/`DavUnregisterAuthCallback` (return-value handle), both reusing established blocker classes.
