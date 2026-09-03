# Header Report: windows.data.pdf.interop.h

## Partitions
`WinRT.Pdf`

## Ownership audit (producer-site-only policy)
- `PdfCreateRenderer(_In_ IDXGIDevice* pDevice, _Out_ IPdfRendererNative** ppRenderer)` outputs a standard COM interface pointer, out of scope.
- `PdfRenderParams` is an `__inline` C++ helper function (no `extern`/`DllImport` linkage), out of scope.
- `IPdfRendererNative::RenderPageToSurface` is a COM vtable method, out of scope.

## Conclusion
`accepted-normalized` — no ownership annotation required (standard COM factory function + inline helper + COM interface method only).
