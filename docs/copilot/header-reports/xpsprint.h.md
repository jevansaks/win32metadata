# xpsprint.h — accepted-normalized

**Partitions:** Gdi, XpsPrinting
**Size:** ~10593 bytes

## Analysis

`xpsprint.h` is MIDL-generated, declaring `IXpsPrintJob`/
`IXpsPrintJobStream` COM interfaces plus the entry points
`StartXpsPrintJob`/`StartXpsPrintJob1`.

`StartXpsPrintJob` signature:
```c
HRESULT __stdcall StartXpsPrintJob(
    LPCWSTR printerName, LPCWSTR jobName, LPCWSTR outputFileName,
    HANDLE progressEvent, HANDLE completionEvent,
    UINT8 *printablePagesOn, UINT32 printablePagesOnCount,
    IXpsPrintJob **xpsPrintJob,
    IXpsPrintJobStream **documentStream,
    IXpsPrintJobStream **printTicketStream);
```

`HANDLE progressEvent`/`completionEvent` are caller-supplied,
caller-owned event handles passed as *input* (generic-type blocker
class 2 would apply even if they were outputs, but here they are not
produced by this API at all). The `IXpsPrintJob`/`IXpsPrintJobStream`
outputs are standard COM interface pointers, out of scope per the
COM-vtable/interface-pointer convention (interface pointers use
`IUnknown::Release`, tracked generically, not via
`[RAIIFree]`/`[InvalidHandleValue]`).

## Conclusion

Clean. No opaque handle production requiring new metadata. No patch
required.
