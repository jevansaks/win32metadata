# Header Report: IContentPrefetcherTaskTrigger.h

## Partitions
`Wsw`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interface `IContentPrefetcherTaskTrigger` (derives `IInspectable`; methods take `LPCWSTR` inputs and output `BOOL`/registration status). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface method only, out of scope).
