# Header Report: DocumentSource.h

## Partitions
`WinRT.Printing`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interfaces `IPrintPreviewPageCollection` and `IPrintDocumentPageSource` plus a constant (`JOB_PAGE_APPLICATION_DEFINED`). No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface methods only, out of scope).
