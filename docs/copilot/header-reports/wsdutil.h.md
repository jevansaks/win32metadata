# Header Report: wsdutil.h

## Partitions
`FunctionDiscovery`

## Ownership audit (producer-site-only policy)
- `WSDAllocateLinkedMemory`/`WSDFreeLinkedMemory`/`WSDAttachLinkedMemory`/`WSDDetachLinkedMemory` operate on raw `void*` memory blocks — the same custom-heap-allocator pattern already classified clean for `wsdapi.h` (batch `scraping-investigation-20`).
- `WSDXMLBuildAnyForSingleElement`/`WSDGenerateFault`/`WSDGenerateFaultEx` output heap-allocated XML-element/fault-structure pointers (`WSDXML_ELEMENT**`, `WSD_SOAP_FAULT**`) freed via the same linked-memory convention (`WSDFreeLinkedMemory`/`WSDXMLCleanupElement`) — not opaque `HANDLE`-family resources.
- `WSDSetConfigurationOption`/`WSDGetConfigurationOption` operate on a raw `LPVOID` byte buffer. `WSDUriEncode`/`WSDUriDecode` output caller-freed string buffers.

## Conclusion
`accepted-normalized` — no ownership annotation required (all outputs use the established WSDAPI linked-memory/buffer convention, not HANDLE-family; consistent with `wsdapi.h` precedent).
