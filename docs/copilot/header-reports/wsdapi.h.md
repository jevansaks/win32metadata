# Header Report: wsdapi.h

## Partitions
`WebServicesOnDevices`

## Scrape validation
- Re-scraped `WebServicesOnDevices` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header). 32 `DllImport` functions scraped.

## Ownership audit (producer-site-only policy)
- `wsdapi.h` is a redirect-only header pulling in `wsdbase.h`, `wsdattachment.h`, `wsdxml.h`, `wsddisco.h`, `wsdclient.h`, `wsdhost.h`, `wsdtypes.h`, `wsdutil.h`, `wsdns.h` (Web Services for Devices API); declares nothing itself.
- No `DECLARE_HANDLE` found in any of these sub-headers.
- All 32 scraped functions are either COM-interface-pointer producers (`_Outptr_ IWSD*`, e.g. `WSDCreateUdpAddress`, `WSDCreateDeviceProxy*`, `WSDCreateDeviceHost*`) or plain memory/XML-node helper pairs (`WSDAllocateLinkedMemory`/`WSDFreeLinkedMemory`, `WSDAttachLinkedMemory`/`WSDDetachLinkedMemory`, `WSDXMLAddSibling`/`WSDXMLAddChild`/`WSDXMLCleanupElement`) operating on raw `void*` memory blocks, not `HANDLE`-family opaque handles. The allocate/free pattern here is analogous to a custom heap allocator (like `CoTaskMemAlloc`/`CoTaskMemFree`), which is outside the scope of the `HANDLE`-specific `_Win32_metadata_raii_free_`/`_invalid_handle_` mechanism (consistent with no repo precedent for annotating raw memory-block alloc/free pairs this way).

## Conclusion
`accepted-normalized` — redirect-only header; underlying content is COM-interface/data-structure/memory-helper only, no `HANDLE`-family ownership annotation required. Sub-headers remain separately tracked (still `pending`) for any future direct-declaration audit.
