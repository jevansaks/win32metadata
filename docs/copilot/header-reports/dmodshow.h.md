# Header Report: dmodshow.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IDMOWrapperFilter` (single method `Init`, taking `REFCLSID` inputs, no outputs) and `CLSID_DMOWrapperFilter`/`CLSID_DMOFilterCategory` GUID constants plus MIDL boilerplate. COM vtable method — out of scope; no `extern`/`DllImport` functions at all.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID constants only, no extern functions).
