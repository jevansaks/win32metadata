# Header Report: vbinterf.h

## Partitions
`Com`, `Com.CallObj`, `Com.ChannelCreds`, `Com.Urlmon`, `ComOle`, `TransactionServer`

## Scrape validation
- Re-scraped `Com` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Declares the `IVBGetControl`/`IGetOleObject`/`IVBFormat`/`IGetVBAObject` COM interfaces (all `IUnknown`-derived, standard `QueryInterface`/`AddRef`/`Release` refcounting) plus `IID_*` GUID constants. `EnumControls`/`GetOleObject`/`GetObject` all output COM interface pointers (`LPVOID FAR*`/`void FAR* FAR*`) per standard COM convention. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface refcounting pattern, not HANDLE-family).
