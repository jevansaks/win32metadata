# Header Report: inspectable.h

## Partitions
`Tcui`

## Ownership audit (producer-site-only policy)
- Declares only the foundational WinRT interface `IInspectable` (`GetIids`/`GetRuntimeClassName`/`GetTrustLevel`, base interface for all WinRT types). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface method only, out of scope).
