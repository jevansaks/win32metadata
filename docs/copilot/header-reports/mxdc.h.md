# Header Report: mxdc.h

## Partitions
`Gdi`, `Printing`

## Ownership audit (producer-site-only policy)
- `MxdcGetPDEVAdjustment(_In_ HANDLE hPrinter, ...)` takes `hPrinter` as a pre-owned caller input only — no opaque handle produced. Remaining declarations are `MXDC_*` property-name string constants and enums.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle; `HANDLE` param is a pre-owned caller input).
