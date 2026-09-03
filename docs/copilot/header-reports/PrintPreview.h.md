# Header Report: PrintPreview.h

## Partitions
`Printing`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IPrintPreviewDxgiPackageTarget` (methods operate on `DXGI`/page-count parameters) plus a `PageCountType` enum. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
