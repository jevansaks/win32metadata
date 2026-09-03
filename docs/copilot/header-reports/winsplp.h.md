# winsplp.h — accepted-normalized (producer-site fix applied)

**Partition:** Printing
**Size:** ~64733 bytes

## Analysis

`WinSplp.h` ("Internal Header file for Print APIs") mixes print
processor/monitor **plugin-contract functions** (`InitializePrintProvidor`,
`OpenPrintProcessor`/`ClosePrintProcessor`, `OpenPort`/`ClosePort`,
`XcvOpenPort`/`XcvClosePort` — called by the spooler service, not
P/Invoked by applications; out of scope, no representable fix) with
genuine **application-callable Win32 APIs**: `CreatePrinterIC`
(returns generic `HANDLE` directly) and single-arg
`DeletePrinterIC(HANDLE hPrinterIC)`.

Checked `emitter.settings.rsp` first: **no existing inline
`RAIIFree` annotation for `CreatePrinterIC::return`** (only an
unrelated `CreatePrinterIC::hPrinter=PRINTER_HANDLE` input-parameter
annotation) — a genuine gap. Added
`CreatePrinterIC::return=[RAIIFree("DeletePrinterIC")]`, consistent
with the `HeapCreate`/`WTSOpenServer`/`QOSCreateHandle` precedent for
generic-`HANDLE`-returning functions.

## Conclusion

Producer-site fix applied — new inline `RAIIFree` return-value
annotation for `CreatePrinterIC` in `emitter.settings.rsp`. The
plugin-contract functions remain correctly out of scope (no
representable ownership pattern; called by the spooler, not exported
system APIs).
