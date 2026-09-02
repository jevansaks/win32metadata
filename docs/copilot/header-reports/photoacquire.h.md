# Header Report: photoacquire.h

## Partitions
`PicAcq`, `Setup`

## Scrape validation
- Re-scraped `PicAcq` partition (`ScanArch=x86`, required — `ExcludeFromCrossarch`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 40 `DllImport` functions, 1528 auto-remaps written.

## Ownership audit (producer-site-only policy)
- No `DECLARE_HANDLE` in `photoacquire.h`.
- All 40 scraped `DllImport` entries are MIDL-generated RPC marshalling helpers for the Photo Acquire COM interfaces: `BSTR_User*`, `HBITMAP_User*`, `HICON_User*`, `HWND_User*`, `LPSAFEARRAY_User*` (`Size`/`Marshal`/`Unmarshal`/`Free`, plus `*64` variants). These are proxy/stub marshalling utilities that pass through existing GDI handles (`HBITMAP`/`HICON`/`HWND`) for RPC transport — they do not create or release these resources; ownership of `HBITMAP`/`HICON`/`HWND` values is established elsewhere (standard GDI/USER32 handles), not by this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (MIDL marshalling-stub-only API surface; no resource-owning functions).
