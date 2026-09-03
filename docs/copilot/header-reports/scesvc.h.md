# scesvc.h — accepted-normalized

**Partition:** Security.ConfigurationSnapin
**Size:** ~13782 bytes

## Analysis

`scesvc.h` declares GUIDs (`IID_ISceSvcAttachmentPersistInfo`,
`IID_ISceSvcAttachmentData`) and COM-interface method wrappers that
are implemented purely as **macros** dispatching through vtables
(`#define ISceSvcAttachmentData_Initialize(This, ...) This->lpVtbl->
Initialize(This, ...)` style) — not `extern`/`DllImport` function
declarations. Out of scope per the COM-vtable-methods convention
(blocker-class 5); macro-wrapped vtable calls are not scrapable
declarations.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
