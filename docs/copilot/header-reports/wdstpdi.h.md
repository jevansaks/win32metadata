# Header Report: wdstpdi.h

## Partitions
`Wds`

## Ownership audit (producer-site-only policy)
Two genuine gaps, both reusing the already-established **generic-type direct-out-param** blocker class:

- `WdsTransportProviderCreateInstance(..., _Out_ PHANDLE phInstance)` produces a generic `HANDLE`, closed via `WdsTransportProviderCloseInstance(_In_ HANDLE hInstance)`.
- `WdsTransportProviderOpenContent(..., _Out_ PHANDLE phContent)` produces a generic `HANDLE`, closed via `WdsTransportProviderCloseContent(_In_ HANDLE hContent)`.

These are content-provider callback functions (implemented by content providers, invoked by the WDS multicast server), but are still declared as ordinary `extern` functions in this header and scraped the same as any other producer/consumer pair. `WdsTransportServerAllocateBuffer`/`WdsTransportServerFreeBuffer` operate on generic `PVOID` buffers (out of scope, generic-buffer convention, not `HANDLE`-family).

## Conclusion
`blocked` — genuine gaps in `WdsTransportProviderCreateInstance`/`CloseInstance` and `WdsTransportProviderOpenContent`/`CloseContent` (generic `HANDLE` direct-out-param, reuses established blocker class).
