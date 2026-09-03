# Header Report: wmdmlog.h

## Partitions
`Wmdm`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IWMDMLogger : public IUnknown`. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface only, no extern functions).
