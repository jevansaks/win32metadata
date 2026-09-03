# Header Report: WeakReference.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the foundational COM/WinRT interfaces `IWeakReference` (`Resolve`) and `IWeakReferenceSource` (`GetWeakReference`), both standard `QueryInterface`-style COM outputs. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface methods only, out of scope).
