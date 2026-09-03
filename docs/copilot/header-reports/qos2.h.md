# qos2.h — accepted-normalized (producer-site fix applied)

**Partition:** Qos
**Size:** ~50196 bytes

## Analysis

`qos2.h` (Quality of Service API) declares `QOSCreateHandle`
(`_Out_ PHANDLE QOSHandle`) and single-arg `QOSCloseHandle(HANDLE
QOSHandle)`. The output parameter uses the generic `HANDLE` type
(not a distinct typedef), but per doc comment it is a specific,
single-purpose "Handle to the QOS subsystem obtained through
QOSCreateHandle" — directly analogous to the already-fixed
`CfOpenFileWithOplock::ProtectedHandle` case (also generic `HANDLE`,
fixed via inline per-parameter `RAIIFree`).

Checked `emitter.settings.rsp` first: **no existing inline
`[RAIIFree(...)]` annotation for `QOSCreateHandle::QOSHandle`** — a
genuine gap. Added `QOSCreateHandle::QOSHandle=[RAIIFree("QOSCloseHandle")]`
immediately following the `CfOpenFileWithOplock` precedent line,
using the identical established pattern (per-parameter inline
annotation for a semantically-specific generic-`HANDLE` output).

## Conclusion

Producer-site fix applied — new inline `RAIIFree` annotation added on
the `QOSCreateHandle` output parameter in `emitter.settings.rsp`,
consistent with the `CfOpenFileWithOplock` precedent.
