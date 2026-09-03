# Header Report: DownloadMgr.h

## Partitions
`InternetExplorer`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IDownloadManager` (single method `Download`, all `_In_` inputs, no outputs) plus `IID_IDownloadManager` GUID. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method + GUID constant only, no extern functions).
