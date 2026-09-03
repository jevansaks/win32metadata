# Header Report: msimeapi.h

## Partitions
`Input.Ime`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IImePlugInDictDictionaryList`. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface only, no extern functions).
