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

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemap to an inline ABI-neutral
annotation directly on the producer out-parameter in
`RecompiledIdlHeaders/um/qos2.h`:
```
QOSCreateHandle(
    _In_    PQOS_VERSION    Version,
    _Out_   PHANDLE         QOSHandle
        _Win32_metadata_raii_free_(QOSCloseHandle)
);
```
No `#include <win32metadata_annotations.h>` guard was added: this header
already unconditionally includes `<ws2tcpip.h>`, which carries the guard, so
the macro is available transitively. The former sidecar entry
`QOSCreateHandle::QOSHandle=[RAIIFree("QOSCloseHandle")]` was removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/qos2.h.metadata.patch` against the
pristine `d154186c` SDK baseline.

## Validation
- Patch replay: `git apply` of `qos2.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Qos, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).

## Conclusion

Producer-site fix applied — inline `_Win32_metadata_raii_free_` annotation
now lives directly on the `QOSCreateHandle` output parameter in
`RecompiledIdlHeaders/um/qos2.h`; no sidecar entry remains.
