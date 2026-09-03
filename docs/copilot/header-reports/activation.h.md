# Header Report: activation.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interface `IActivationFactory` (derives `IInspectable`; `ActivateInstance` outputs an `IInspectable**`, a standard COM interface pointer). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface method only, out of scope).
