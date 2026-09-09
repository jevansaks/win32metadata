# namespaceapi.h

**Classification:** accepted-normalized (producer-site fix applied)

## Summary
`namespaceapi.h` declares the Unicode private-namespace APIs
(`CreatePrivateNamespaceW`, `OpenPrivateNamespaceW`,
`CreateBoundaryDescriptorW`), all of which return a `HANDLE`-family value
directly as the function return. The corresponding ANSI entry points
(`CreatePrivateNamespaceA`, `OpenPrivateNamespaceA`,
`CreateBoundaryDescriptorA`) live in `WinBase.h` and are tracked/consolidated
separately there.

## Ownership Analysis (sidecar-removal tranche update)
Moved from `emitter.settings.rsp` memberRemaps to inline ABI-neutral
annotations directly on the producer return declarations in
`RecompiledIdlHeaders/um/namespaceapi.h`:
```
WINBASEAPI
HANDLE
WINAPI
CreatePrivateNamespaceW(
    _In_opt_ LPSECURITY_ATTRIBUTES lpPrivateNamespaceAttributes,
    _In_ LPVOID lpBoundaryDescriptor,
    _In_ LPCWSTR lpAliasPrefix
    )
    _Win32_metadata_raii_free_(ClosePrivateNamespace);

WINBASEAPI
HANDLE
WINAPI
OpenPrivateNamespaceW(
    _In_ LPVOID lpBoundaryDescriptor,
    _In_ LPCWSTR lpAliasPrefix
    )
    _Win32_metadata_raii_free_(ClosePrivateNamespace);

WINBASEAPI
HANDLE
WINAPI
CreateBoundaryDescriptorW(
    _In_ LPCWSTR Name,
    _In_ ULONG Flags
    )
    _Win32_metadata_raii_free_(DeleteBoundaryDescriptor);
```
No `#include <win32metadata_annotations.h>` guard was added: this header
already unconditionally includes `<minwindef.h>` and `<minwinbase.h>`, both of
which carry the guard from earlier batches, so the macro is available
transitively.

The three former sidecar entries
(`CreatePrivateNamespaceW::return`, `OpenPrivateNamespaceW::return`,
`CreateBoundaryDescriptorW::return`, all `=[RAIIFree(...)]`) were removed from
`emitter.settings.rsp`. Consolidated into a single new
`generation/WinSDK/patches/post-midl/namespaceapi.h.metadata.patch` against the
pristine `d154186c` SDK baseline (no prior per-reason patch existed for this
header).

## Validation
- Patch replay: `git apply` of `namespaceapi.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (Threading, `-p:ScanArch=crossarch`): Build succeeded, 0 Error(s).
