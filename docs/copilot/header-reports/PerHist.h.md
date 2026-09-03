# Header Report: PerHist.h

## Partitions
`InternetExplorer`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IPersistHistory : public IPersist` (`LoadHistory`/`SaveHistory` take `IStream*`; `SetPositionCookie`/`GetPositionCookie` operate on plain `DWORD`). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
