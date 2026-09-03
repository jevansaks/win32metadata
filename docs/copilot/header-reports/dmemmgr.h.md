# Header Report: dmemmgr.h

## Partitions
`DirectDraw`

## Ownership audit (producer-site-only policy)
- `VidMemAlloc`/`HeapVidMemAllocAligned` return `FLATPTR` (`typedef ULONG_PTR FLATPTR`) directly as the C return value, closed via `VidMemFree(LPVMEMHEAP pvmh, FLATPTR ptr)`. Although `FLATPTR` is a distinctly-named typedef, it is a **generic address/pointer-value alias** (`ULONG_PTR`) used pervasively throughout this header for unrelated internal struct fields (`VMEML.ptr`, `VMEMR.ptr`, `VMEMHEAP.fpGARTLin`/`fpGARTDev`, etc.) that are not owned resources — annotating the type itself would incorrectly apply ownership metadata to every `FLATPTR` value anywhere in the metadata. This is the already-established **generic-type** blocker class, combined with the **return-value handle** restriction (`getprocesshandlefromhwnd.h`).

## Conclusion
`blocked` — genuine gap in `VidMemAlloc`/`HeapVidMemAllocAligned`/`VidMemFree` (generic-address-type return value, reuses established blocker class).
