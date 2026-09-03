# Header Report: appserviceinterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interfaces `IAppServiceConnectionExtendedExecution` and `ICorrelationVectorSource`, both derived from `IUnknown`/COM vtable methods. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface methods only, out of scope).
