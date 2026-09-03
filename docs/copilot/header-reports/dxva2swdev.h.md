# Header Report: dxva2swdev.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- `PDXVA2SW_CREATEVIDEOPROCESSDEVICE`/`PDXVA2SW_DESTROYVIDEOPROCESSDEVICE`/etc. (including the `_Out_ HANDLE* phDevice` producer) are function-pointer *typedefs*, not `extern`/`DllImport` declarations — stored as fields in the `DXVA2SW_CALLBACKS` struct and dispatched dynamically by the software video-processing device provider. Same out-of-scope class as `mswsockdef.h`'s `RIO_EXTENSION_FUNCTION_TABLE` (no scrapable function declaration exists to attach an annotation to).
- Remaining declarations are enums/transparent structs (`DXVA2_SampleFlags`, `DXVA2_VIDEOSAMPLE`, etc.).

## Conclusion
`accepted-normalized` — no ownership annotation required (function-pointer-typedef-in-struct pattern, architecturally out of scope; no extern functions).
