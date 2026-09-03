# dxmini.h — accepted-normalized

**Partition:** DirectDraw
**Size:** ~13705 bytes

## Analysis

`dxmini.h` (DirectDraw DXAPI miniport support, kernel-mode) declares
only a GUID, the kernel `MDL` transparent struct, and function-pointer
typedefs used as driver-supplied callback table entries
(`PDX_IRQCALLBACK`, `PDX_GETIRQINFO`, `PDX_ENABLEIRQ`,
`PDX_SKIPNEXTFIELD`, `PDX_LOCK`, etc.) — struct-field/table-entry
function pointers, not `extern`/`DllImport` declarations, out of scope
per the function-pointer-typedef convention (blocker-class 4).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
