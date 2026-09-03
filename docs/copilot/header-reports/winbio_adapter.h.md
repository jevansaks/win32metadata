# winbio_adapter.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
WinBio Service plug-in adapter contract (sensor/engine/storage/framework
interface function tables), consumed by biometric adapter DLLs. All
`HRESULT`-returning declarations are `typedef HRESULT (WINAPI *...)(...)`
function-pointer typedefs assigned into the `WINBIO_SENSOR_INTERFACE` /
`WINBIO_ENGINE_INTERFACE` / `WINBIO_STORAGE_INTERFACE` / etc. function
tables (e.g. `PIBIO_SENSOR_ATTACH_FN`, `PIBIO_SENSOR_DETACH_FN`) — blocker
class 4 (function-pointer-typedef-in-struct), consistent with the
cardmod.h precedent.

## Ownership Analysis
No directly-callable producer/consumer API pair exists; these are
callback prototypes implemented by adapter plugins and invoked by the
WinBio service, not exported handle-producing APIs. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
